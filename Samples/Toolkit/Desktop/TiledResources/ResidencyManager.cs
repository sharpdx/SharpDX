namespace TiledResources
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Effects;
    using SharpDX;
    using SharpDX.DXGI;
    using SharpDX.Toolkit;

    /// <summary>
    /// Class is responsible for tile loading, unloading and processing sampled data
    /// </summary>
    internal sealed class ResidencyManager : Component
    {
        private static uint _frame; // current frame, used to track when a tile was loaded

        private SharpDX.Toolkit.Graphics.Buffer _tilePool;

        private ResidencyViewerEffect _viewerEffect;
        private SharpDX.Toolkit.Graphics.GeometricPrimitive<ResidencyVertex> _viewerGeometry;
        private readonly GraphicsDeviceManager _graphicsDeviceManager;
        private readonly SharpDX.Toolkit.Content.IContentManager _contentManager;

        /// <summary>
        /// A list of the tiled texture associated with the current manager
        /// </summary>
        private readonly List<ManagedTiledResource> _managedTiledResources = new List<ManagedTiledResource>();

        /// <summary>
        /// A list of tracked tiles that have been processed by this manager
        /// </summary>
        private readonly Dictionary<TileKey, TrackedTile> _trackedTiles = new Dictionary<TileKey, TrackedTile>();

        /// <summary>
        /// Tiles that are seed but are not scheduled for loading, computed from provided <see cref="DecodedSample"/>
        /// </summary>
        private readonly List<TrackedTile> _seenTiles = new List<TrackedTile>();

        /// <summary>
        /// Tiles that are being scheduled for loading but are not mapped yet
        /// </summary>
        private readonly List<TrackedTile> _loadingTiles = new List<TrackedTile>();

        /// <summary>
        /// Tiles that are mapped to GPU memory
        /// </summary>
        private readonly List<TrackedTile> _mappedTiles = new List<TrackedTile>();

        /// <summary>
        /// The number of loading operations that are active at the moment
        /// </summary>
        private volatile int _activeTileLoadingOperations;

        private int _reservedTiles;
        private int _defaultTileIndex = -1;
        private string _diffuseFilepath; // the path to the diffuse texture

        private byte[] _residencyBuffer;

        public ResidencyManager(GraphicsDeviceManager graphicsDeviceManager, SharpDX.Toolkit.Content.IContentManager contentManager)
        {
            _graphicsDeviceManager = graphicsDeviceManager;
            _contentManager = contentManager;
        }

        /// <summary>
        /// Render the visualisation showing that tiles are loaded for each managed resource
        /// </summary>
        public void RenderVisualization()
        {
            var viewport = _graphicsDeviceManager.GraphicsDevice.Viewport;

            var yOffset = 0f;
            foreach (var resource in _managedTiledResources)
            {
                var visualWidth = Math.Max(256.0f, viewport.Width / SampleSettings.Sampling.Ratio);
                var visualHeight = visualWidth * (float)Math.Sqrt(3) / 3f;
                var xPosition = viewport.Width - visualWidth - 48f;
                var yPosition = viewport.Height - visualHeight - 48f - yOffset;

                _viewerEffect.Scale = new Vector2(visualWidth / viewport.Width, visualHeight / viewport.Height);
                _viewerEffect.Offset = new Vector2(2f * (xPosition + visualWidth / 2f) / viewport.Width - 1f, 1f - 2f * (yPosition + visualHeight / 2f) / viewport.Height);
                _viewerEffect.Texture = resource.ResidencyTexture;

                _viewerGeometry.Draw(_viewerEffect);

                yOffset += visualHeight + 24f;
            }
        }

        /// <summary>
        /// Process decoded samples to determine which tiles are brought into view and need to be loaded
        /// </summary>
        /// <param name="samples">The list of the samples</param>
        public void EnqueueSamples(List<DecodedSample> samples)
        {
            foreach (var sample in samples)
            {
                // iterate trough each managed resource for each sample
                foreach (var resource in _managedTiledResources)
                {
                    var desc = resource.Texture.Description;

                    var mipCount = desc.Format == SharpDX.Toolkit.Graphics.PixelFormat.BC1.UNorm
                                       ? SampleSettings.TerrainAssets.Diffuse.UnpackedMipCount
                                       : SampleSettings.TerrainAssets.Normal.UnpackedMipCount;

                    // determine the available mip level
                    var actualMip = Math.Max(0, Math.Min(mipCount - 1, sample.Mip));

                    // process all mip levels from the current one to the least detailed
                    for (var mip = actualMip; mip < desc.MipLevels; mip++)
                    {
                        var coordinate = new SharpDX.Direct3D11.TiledResourceCoordinate();
                        coordinate.Subresource = mip + sample.Face * desc.MipLevels; // compute subresource index

                        var widthInTiles = resource.SubresourceTilings[coordinate.Subresource].WidthInTiles;
                        var heightInTiles = resource.SubresourceTilings[coordinate.Subresource].HeightInTiles;

                        // compute tile coordinate in the face texture for the current mip level
                        coordinate.X = (int)Math.Min(widthInTiles - 1, Math.Max(0, widthInTiles * sample.U));
                        coordinate.Y = (int)Math.Min(heightInTiles - 1, Math.Max(0, heightInTiles * sample.V));

                        var key = new TileKey(coordinate, resource.Texture);
                        TrackedTile trackedTile;
                        // if the tile has not been tracked yet - mark it as seen
                        if (!_trackedTiles.TryGetValue(key, out trackedTile))
                        {
                            trackedTile = new TrackedTile(resource, coordinate, (short)mip, sample.Face)
                                          {
                                              lastSeen = _frame,
                                              State = TileState.Seen
                                          };

                            _trackedTiles[key] = trackedTile;
                            _seenTiles.Add(trackedTile);
                        }
                        else // ... otherwise just update its last seen time
                        {
                            trackedTile.lastSeen = _frame;
                        }
                    }
                }
            }

            _frame++;
        }

        /// <summary>
        /// Process all tile queues - loaded, unloaded, etc.
        /// </summary>
        public void ProcessQueues()
        {
            _seenTiles.Sort(TrackedTile.LoadComparison);
            _loadingTiles.Sort(TrackedTile.MapComparison);
            _mappedTiles.Sort(TrackedTile.EvictComparison);

            // schedule tiles for loading
            for (var i = _activeTileLoadingOperations; i < SampleSettings.TileResidency.MaxSimultaneousFileLoadTasks; i++)
            {
                // check if there is something to load
                if (_seenTiles.Count == 0) break;

                var tileToLoad = _seenTiles[0];
                _seenTiles.Remove(tileToLoad); // remove the tile from the seen list

                Interlocked.Increment(ref _activeTileLoadingOperations);

                // schedule the tile for loading in a separate task
                tileToLoad.ManagedTiledResource
                          .Loader
                          .LoadTileAsync(tileToLoad.Coordinate)
                          .ContinueWith(dataTask =>
                                        {
                                            // store the loaded data and mark the tile as loaded
                                            tileToLoad.TileData = dataTask.Result;
                                            tileToLoad.State = TileState.Loaded;
                                            Interlocked.Decrement(ref _activeTileLoadingOperations);
                                        });

                _loadingTiles.Add(tileToLoad);
            }

            // holds all information about changed tiles that will need any updates on GPU
            var coalescedMapArguments = new Dictionary<SharpDX.Toolkit.Graphics.Texture2DBase, TileMappingUpdateArguments>();

            // process loaded tiles
            for (var i = 0; i < SampleSettings.TileResidency.MaxTilesLoadedPerFrame; i++)
            {
                if (_loadingTiles.Count == 0) break;

                var tileToMap = _loadingTiles[0];

                // check if there are ready loaded tiles
                if (tileToMap.State != TileState.Loaded)
                    break;

                _loadingTiles.Remove(tileToMap);

                var physicalTileOffset = _reservedTiles + _mappedTiles.Count;

                // if there is no space for the new tile - need to find one to unload
                if (_mappedTiles.Count + _reservedTiles == SampleSettings.TileResidency.PoolSizeInTiles)
                {
                    var tileToEvict = _mappedTiles[0];

                    // if the tile that needs to be loaded is actually older than the tile to evict - keep the last one
                    if (tileToMap.lastSeen < tileToEvict.lastSeen)
                    {
                        _trackedTiles.Remove(new TileKey(tileToMap.Coordinate, tileToMap.ManagedTiledResource.Texture));

                        continue;
                    }

                    // untrack the evicted tile
                    _mappedTiles.Remove(tileToEvict);
                    _trackedTiles.Remove(new TileKey(tileToEvict.Coordinate, tileToEvict.ManagedTiledResource.Texture));

                    // store the index of the evicted tile
                    physicalTileOffset = tileToEvict.PhysicalTileOffset;

                    var argumentsForEvicted = GetOrCreate(coalescedMapArguments, tileToEvict.ManagedTiledResource.Texture);
                    argumentsForEvicted.Coordinates.Add(tileToEvict.Coordinate);
                    argumentsForEvicted.RangeFlags.Add(SharpDX.Direct3D11.TileRangeFlags.Null);
                    argumentsForEvicted.PhysicalOffsets.Add(physicalTileOffset);

                    // update the residency map for the evicted tile and decrease the loaded mip level for the current coordinate
                    UpdateResidencyMap(tileToEvict, false);
                }

                // store the offset in the tile pool
                tileToMap.PhysicalTileOffset = physicalTileOffset;
                tileToMap.State = TileState.Mapped;

                var argumentsForMapped = GetOrCreate(coalescedMapArguments, tileToMap.ManagedTiledResource.Texture);
                argumentsForMapped.Coordinates.Add(tileToMap.Coordinate);
                argumentsForMapped.RangeFlags.Add(0);
                argumentsForMapped.PhysicalOffsets.Add(physicalTileOffset);
                argumentsForMapped.TilesToMap.Add(tileToMap);

                // update the residency map with the information of the newly mapped tile and increase the mip level for the current coordinate
                UpdateResidencyMap(tileToMap, true);

                _mappedTiles.Add(tileToMap);
            }

            // get a reference to DirectX 11.2 device context
            var context = _graphicsDeviceManager.GetContext2();

            // for each managed resource ...
            foreach (var pair in coalescedMapArguments)
            {
                var arguments = pair.Value;
                var count = arguments.RangeFlags.Count;

                var size = new SharpDX.Direct3D11.TileRegionSize { TileCount = 1 };
                // prepare the mapped regions array
                var rangeCounts = new int[count];
                Set(rangeCounts, 1);

                // prepare the mapped tile region size array
                var sizes = new SharpDX.Direct3D11.TileRegionSize[count];
                Set(sizes, size);

                // update tile mappings in a single call
                context.UpdateTileMappings(pair.Key,
                                           arguments.Coordinates.Count,
                                           arguments.Coordinates.ToArray(),
                                           sizes,
                                           _tilePool,
                                           count,
                                           arguments.RangeFlags.ToArray(),
                                           arguments.PhysicalOffsets.ToArray(),
                                           rangeCounts,
                                           0);

                context.TiledResourceBarrier(null, (SharpDX.Direct3D11.Resource)pair.Key);
            }

            // for each managed resource ...
            foreach (var pair in coalescedMapArguments)
            {
                var arguments = pair.Value;

                // ... for each tile coordinate ...
                for (var i = 0; i < arguments.Coordinates.Count; i++)
                {
                    // if the tile is present
                    if (arguments.RangeFlags[i] != SharpDX.Direct3D11.TileRangeFlags.Null)
                    {
                        var regionSize = new SharpDX.Direct3D11.TileRegionSize { TileCount = 1 };
                        var tile = arguments.TilesToMap[0];

                        unsafe
                        {
                            fixed (void* pData = &tile.TileData[0])
                            {
                                // update the tile data from the provided byte array
                                context.UpdateTiles(pair.Key,
                                                    arguments.Coordinates[i],
                                                    regionSize,
                                                    new IntPtr(pData),
                                                    0);
                            }
                        }

                        tile.ManagedTiledResource.Loader.CompleteLoading(tile.TileData);

                        arguments.TilesToMap.Remove(tile);

                        tile.TileData = null;
                    }
                }
            }

            // update the residency textures
            foreach (var resource in _managedTiledResources)
            {
                var subresourceTiling = resource.SubresourceTilings[0];

                var baseMaxTileDimension = Math.Max(subresourceTiling.WidthInTiles, subresourceTiling.HeightInTiles);

                var bufferSize = baseMaxTileDimension * baseMaxTileDimension;
                if (_residencyBuffer == null || _residencyBuffer.Length < bufferSize)
                    _residencyBuffer = new byte[bufferSize];

                // update the data for each texture cube face
                for (var face = 0; face < 6; face++)
                {
                    for (var y = 0; y < baseMaxTileDimension; y++)
                    {
                        var tileY = (y * subresourceTiling.HeightInTiles) / baseMaxTileDimension;
                        for (var x = 0; x < baseMaxTileDimension; x++)
                        {
                            var tileX = (x * subresourceTiling.WidthInTiles) / baseMaxTileDimension;

                            _residencyBuffer[y * baseMaxTileDimension + x] = resource.Residency[face][tileY * subresourceTiling.WidthInTiles + tileX];
                        }
                    }

                    unsafe
                    {
                        fixed (void* pData = &_residencyBuffer[0])
                            context.UpdateSubresource(resource.ResidencyTexture, face, null, new IntPtr(pData), baseMaxTileDimension, 0);
                    }
                }
            }

        }

        /// <summary>
        /// Resets the state of the current residency manager
        /// </summary>
        public void Reset()
        {
            _trackedTiles.Clear();
            _seenTiles.Clear();
            _loadingTiles.Clear();
            _mappedTiles.Clear();

            foreach (var resource in _managedTiledResources)
            {
                var tilings = resource.SubresourceTilings[0];

                for (var face = 0; face < 6; face++)
                {
                    resource.Residency[face] = new byte[tilings.WidthInTiles * tilings.HeightInTiles];
                    Set(resource.Residency[face], (byte)0xff);
                }
            }

            var flags = new[] { SharpDX.Direct3D11.TileRangeFlags.Null };

            var context = _graphicsDeviceManager.GetContext2();

            // mark all tiles as null
            foreach (var resource in _managedTiledResources)
                context.UpdateTileMappings(resource.Texture, 1, null, null, null, 1, flags, null, null, 0);

            //if (_graphicsDeviceManager.IsTier2()) return;

            // On Tier-1 devices, applications must ensure that NULL-mapped tiles are never accessed.
            // Because the mapping heuristic in this sample is only approximate, it is safest to map
            // all tiles to a dummy tile that will serve as the NULL tile.

            var device = _graphicsDeviceManager.GraphicsDevice;

            // Create a temporary buffer to clear the dummy tile to zero.
            var tempBufferDescription = new SharpDX.Direct3D11.BufferDescription
                                      {
                                          SizeInBytes = SampleSettings.TileSizeInBytes,
                                          OptionFlags = SharpDX.Direct3D11.ResourceOptionFlags.Tiled,
                                          Usage = SharpDX.Direct3D11.ResourceUsage.Default
                                      };

            var defaultTileData = new byte[SampleSettings.TileSizeInBytes];
            var coordinate = new[] { new SharpDX.Direct3D11.TiledResourceCoordinate() };
            var regionSize = new[] { new SharpDX.Direct3D11.TileRegionSize { TileCount = 1 } };
            var rangeFlags = new[] { SharpDX.Direct3D11.TileRangeFlags.ReuseSingleTile };

            using (var tempBuffer = SharpDX.Toolkit.Graphics.Buffer.New(device, tempBufferDescription))
            {
                _defaultTileIndex = _reservedTiles++;

                context.UpdateTileMappings(tempBuffer, 1, coordinate, regionSize, _tilePool, 1, rangeFlags, new[] { _defaultTileIndex }, null, 0);

                unsafe
                {
                    // Map the single tile in the buffer to physical tile 0.
                    fixed (void* pdata = &defaultTileData[0])
                        context.UpdateTiles(tempBuffer, coordinate[0], regionSize[0], new IntPtr(pdata), 0);
                }
            }

            // Map all tiles to the dummy physical tile.
            foreach (var resource in _managedTiledResources)
            {
                regionSize[0].TileCount = resource.TotalTiles;
                context.UpdateTileMappings(resource.Texture, 1, coordinate, regionSize, _tilePool, 1, rangeFlags, new[] { _defaultTileIndex }, null, 0);
                context.TiledResourceBarrier(null, (SharpDX.Direct3D11.Resource)resource.Texture);
            }
        }

        /// <summary>
        /// Adds the provided texture to the current manager
        /// </summary>
        /// <param name="texture">The tiled texture that needs to be managed</param>
        /// <param name="filePath">The filepath from where to load the texture data</param>
        /// <returns>The shader resource view for the created residency texture</returns>
        public SharpDX.Toolkit.Graphics.Texture2DBase ManageTexture(SharpDX.Toolkit.Graphics.TextureCube texture, string filePath)
        {
            // assume the diffuse texture has the BC1_Unorm format - store a path to it
            if (texture.Description.Format == Format.BC1_UNorm)
                _diffuseFilepath = filePath;

            var resource = new ManagedTiledResource { Texture = texture };

            var subresourceTilings = texture.Description.MipLevels * texture.Description.ArraySize;

            resource.SubresourceTilings = new SharpDX.Direct3D11.SubResourceTiling[subresourceTilings];

            var device2 = _graphicsDeviceManager.GetDevice2();

            device2.GetResourceTiling(texture,
                                            out resource.TotalTiles,
                                            out resource.PackedMipDescription,
                                            out resource.TileShape,
                                            ref subresourceTilings,
                                            0,
                                            resource.SubresourceTilings);

            if (subresourceTilings != texture.Description.MipLevels * texture.Description.ArraySize)
                throw new ApplicationException("Unexpected value - subresourceTilings has changed");

            resource.Loader = new TileLoader(filePath, resource.SubresourceTilings, false);

            var device = _graphicsDeviceManager.GraphicsDevice;

            var tiling = resource.SubresourceTilings[0];

            var size = Math.Max(tiling.WidthInTiles, tiling.HeightInTiles);

            var description = new SharpDX.Direct3D11.Texture2DDescription
                              {
                                  Width = size,
                                  Height = size,
                                  MipLevels = 1,
                                  ArraySize = 6,
                                  BindFlags = SharpDX.Direct3D11.BindFlags.ShaderResource,
                                  Format = Format.R8_UNorm,
                                  SampleDescription = new SampleDescription(1, 0),
                                  Usage = SharpDX.Direct3D11.ResourceUsage.Default,
                                  OptionFlags = SharpDX.Direct3D11.ResourceOptionFlags.TextureCube
                              };

            resource.ResidencyTexture = ToDispose(SharpDX.Toolkit.Graphics.Texture2D.New(device, description));

            for (var face = 0; face < 6; face++)
            {
                resource.Residency[face] = new byte[tiling.WidthInTiles * tiling.HeightInTiles];
                Set(resource.Residency[face], (byte)0xff);
            }

            _managedTiledResources.Add(resource);

            return resource.ResidencyTexture;
        }

        /// <summary>
        /// Loads all packed mips.
        /// </summary>
        /// <returns>The async loading task</returns>
        public Task InitializeManagedResourcesAsync()
        {
            Reset();

            var tileLoadTasks = new List<Task>();

            var context = _graphicsDeviceManager.GetContext2();

            foreach (var resource in _managedTiledResources)
            {
                var r = resource;

                if (r.PackedMipDescription.PackedMipCount == 0)
                    continue;

                var coordinate = new SharpDX.Direct3D11.TiledResourceCoordinate();

                for (var face = 0; face < 6; face++)
                {
                    for (var i = 0; i < r.PackedMipDescription.PackedMipCount; i++)
                    {
                        var mip = r.Texture.Description.MipLevels - i - 1;
                        coordinate.Subresource = face * r.Texture.Description.MipLevels + mip;

                        var c = coordinate;
                        tileLoadTasks.Add(r.Loader.LoadTileAsync(c)
                                           .ContinueWith(t =>
                                                         {
                                                             var pitch = GetPitch(r.TileShape.WidthInTexels, r.Texture.Description.Format);
                                                             unsafe
                                                             {
                                                                 fixed (void* pdata = &t.Result[0])
                                                                 {
                                                                     context.UpdateSubresource(r.Texture,
                                                                                               c.Subresource,
                                                                                               null,
                                                                                               new IntPtr(pdata),
                                                                                               pitch,
                                                                                               0);
                                                                 }
                                                             }

                                                             r.Loader.CompleteLoading(t.Result);
                                                         }));
                    }
                }
            }

            return Task.Factory.StartNew(() => Task.WaitAll(tileLoadTasks.ToArray()));
        }

        /// <summary>
        /// Enables or disables the drawing of the tile borders
        /// </summary>
        /// <param name="enableBorders">true to enable tile borders, otherwise false</param>
        public void SetBorderMode(bool enableBorders)
        {
            var resource = _managedTiledResources[0];

            resource.Loader = new TileLoader(_diffuseFilepath, resource.SubresourceTilings, enableBorders);
        }

        /// <summary>
        /// Loads asyncronously all needed GPU structures
        /// </summary>
        /// <returns></returns>
        public Task LoadStructuresAsync()
        {
            return Task.Factory.StartNew(LoadStructures);
        }

        /// <summary>
        /// Gets the pitch in bytes of the current format for the provided with in texels
        /// </summary>
        /// <param name="widthInTexels">The with in texels</param>
        /// <param name="format">The current format</param>
        /// <returns>The with in bytes</returns>
        private int GetPitch(int widthInTexels, Format format)
        {
            var result = widthInTexels;

            switch (format)
            {
                case Format.BC1_UNorm:
                    result *= 8 / 4;
                    break;
                case Format.BC5_SNorm:
                    result *= 16 / 4;
                    break;
                case Format.R8G8B8A8_UNorm:
                    result *= 4;
                    break;
                case Format.R16G16_SNorm:
                    result *= 4;
                    break;
                default:
                    throw new NotSupportedException();
            }

            return result;
        }

        /// <summary>
        /// Updates the residency for the provided tile
        /// </summary>
        /// <param name="tile">The tile whose information needs to be updated.</param>
        /// <param name="isMin">Whether to decrease or increase the mip level (for evicted and mapped tiles)</param>
        private static void UpdateResidencyMap(TrackedTile tile, bool isMin)
        {
            var srte1 = tile.ManagedTiledResource.SubresourceTilings[0];
            var srte2 = tile.ManagedTiledResource.SubresourceTilings[tile.Coordinate.Subresource];

            var coveredWidth = srte1.WidthInTiles / srte2.WidthInTiles;
            var coveredHeight = srte1.HeightInTiles / srte2.HeightInTiles;

            for (var y = 0; y < coveredHeight; y++)
            {
                for (var x = 0; x < coveredWidth; x++)
                {
                    var tileX = tile.Coordinate.X * coveredWidth + x;
                    var tileY = tile.Coordinate.Y * coveredHeight + y;

                    var residency = tile.ManagedTiledResource.Residency[tile.Face];

                    var index = tileY * srte1.WidthInTiles + tileX;

                    var v1 = (byte)((tile.MipLevel + 1) * 16);
                    var v2 = residency[index];
                    residency[index] = isMin ? Math.Min(v1, v2) : Math.Max(v1, v2);
                }
            }
        }

        /// <summary>
        /// Loads all needed GPU structures
        /// </summary>
        private void LoadStructures()
        {
            var device = _graphicsDeviceManager.GraphicsDevice;

            var vertexBufferData = new[]
                                   {
                                       new ResidencyVertex(-1.0f, 0.5f, 1.0f, -1.0f, 1.0f, 0.0f),
                                       new ResidencyVertex(-0.5f, 1.0f, -1.0f, -1.0f, 1.0f, 0.0f),
                                       new ResidencyVertex(0.0f, 0.5f, -1.0f, 1.0f, 1.0f, 0.0f),
                                       new ResidencyVertex(0.5f, 1.0f, -1.0f, -1.0f, 1.0f, 0.0f),
                                       new ResidencyVertex(1.0f, 0.5f, 1.0f, -1.0f, 1.0f, 0.0f),
                                       new ResidencyVertex(1.0f, -0.5f, 1.0f, -1.0f, -1.0f, 0.0f),
                                       new ResidencyVertex(0.5f, -1.0f, 1.0f, 1.0f, -1.0f, 0.0f),
                                       new ResidencyVertex(0.0f, -0.5f, -1.0f, 1.0f, -1.0f, 0.0f),
                                       new ResidencyVertex(-0.5f, -1.0f, 1.0f, 1.0f, -1.0f, 0.0f),
                                       new ResidencyVertex(-1.0f, -0.5f, 1.0f, -1.0f, -1.0f, 0.0f),
                                       new ResidencyVertex(-0.5f, 0.0f, 1.0f, 1.0f, 1.0f, 0.0f),
                                       new ResidencyVertex(0.5f, 0.0f, -1.0f, -1.0f, -1.0f, 0.0f)
                                   };

            var indexBufferData = new[]
                                  {
                                      0, 1, 10,
                                      1, 2, 10,
                                      2, 7, 10,
                                      7, 8, 10,
                                      8, 9, 10,
                                      9, 0, 10,
                                      2, 3, 11,
                                      3, 4, 11,
                                      4, 5, 11,
                                      5, 6, 11,
                                      6, 7, 11,
                                      7, 2, 11
                                  };

            _viewerGeometry = ToDispose(new SharpDX.Toolkit.Graphics.GeometricPrimitive<ResidencyVertex>(_graphicsDeviceManager.GraphicsDevice,
                                                                                                         vertexBufferData,
                                                                                                         indexBufferData,
                                                                                                         false));

            _viewerEffect = _contentManager.Load<ResidencyViewerEffect>("ResidencyViewer");

            var tilePoolDescription = new SharpDX.Direct3D11.BufferDescription
                                      {
                                          SizeInBytes = SampleSettings.TileSizeInBytes * SampleSettings.TileResidency.PoolSizeInTiles,
                                          Usage = SharpDX.Direct3D11.ResourceUsage.Default,
                                          OptionFlags = SharpDX.Direct3D11.ResourceOptionFlags.TilePool
                                      };

            _tilePool = ToDispose(SharpDX.Toolkit.Graphics.Buffer.New(device, tilePoolDescription));
        }

        private static TValue GetOrCreate<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key)
            where TValue : new()
        {
            TValue value;
            if (!dictionary.TryGetValue(key, out value))
            {
                value = new TValue();
                dictionary[key] = value;
            }

            return value;
        }

        /// <summary>
        /// Sets all elements from the provided array to the provided value
        /// </summary>
        /// <typeparam name="T">Array type</typeparam>
        /// <param name="array">The destination array</param>
        /// <param name="value">The source value</param>
        private static void Set<T>(T[] array, T value)
        {
            for (var i = 0; i < array.Length; i++)
                array[i] = value;
        }
    }
}