namespace TiledResources
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Loads the tile data from the provided file
    /// </summary>
    internal sealed class TileLoader
    {
        private readonly string _filePath;
        private readonly SharpDX.Direct3D11.SubResourceTiling[] _tilingInfo;
        private readonly bool _border;

        private readonly List<long> _subresourceTileOffsets = new List<long>();
        private readonly int _subresourcesPerFaceInResource;
        private readonly int _subresourcesPerFaceInFile;

        // a pool of byte arrays used to load tiles. Used to avoid permanent allocations and garbage generation
        private readonly HashSet<byte[]> _buffersPool = new HashSet<byte[]>();

        // border colors
        private static readonly ushort[] colors = new[]
                                                  {
                                                      (ushort)0xF800,
                                                      (ushort)0xFFB0,
                                                      (ushort)0x07B0,
                                                      (ushort)0x07FF,
                                                      (ushort)0x001F,
                                                      (ushort)0xF81F
                                                  };

        public TileLoader(string filePath, SharpDX.Direct3D11.SubResourceTiling[] tilingInfo, bool border)
        {
            _filePath = filePath;
            _tilingInfo = tilingInfo;
            _border = border;

            var fileInfo = new FileInfo(_filePath);

            // precompute the tile offsets in the file for each cube face
            var numTilesPerFace = (fileInfo.Length / SampleSettings.TileSizeInBytes / 6);
            var tilesForSingleFaceMostDetailedMip = tilingInfo[1].StartTileIndexInOverallResource;
            _subresourcesPerFaceInResource = tilingInfo.Length / 6;

            for (var face = 0; face < 6; face++)
            {
                var tileIndexInFace = 0;
                var tilesInSubresource = tilesForSingleFaceMostDetailedMip;
                _subresourcesPerFaceInFile = 0;
                while (tileIndexInFace < numTilesPerFace)
                {
                    var offset = (face * numTilesPerFace) + tileIndexInFace;
                    _subresourceTileOffsets.Add(offset);
                    tileIndexInFace += tilesInSubresource;
                    tilesInSubresource = Math.Max(1, tilesInSubresource / 4);
                    _subresourcesPerFaceInFile++;
                }
            }
        }

        /// <summary>
        /// Launches tile loading as async task.
        /// </summary>
        /// <param name="coordinate">The coordinate of the tile to load.</param>
        /// <returns>A task which will load the tile data asyncronously</returns>
        public Task<byte[]> LoadTileAsync(SharpDX.Direct3D11.TiledResourceCoordinate coordinate)
        {
            return Task.Factory.StartNew(() => LoadTile(coordinate));
        }

        /// <summary>
        /// Returns the byte array to the pool to be reused for other tile loading.
        /// </summary>
        /// <param name="buffer">The buffer which is not used anymore and can be reused safely.</param>
        public void CompleteLoading(byte[] buffer)
        {
            ReturnBufferToPool(buffer);
        }

        private byte[] LoadTile(SharpDX.Direct3D11.TiledResourceCoordinate coordinate)
        {
            var subresourceInFile = (coordinate.Subresource / _subresourcesPerFaceInResource) * _subresourcesPerFaceInFile
                                    + coordinate.Subresource % _subresourcesPerFaceInResource;

            var fileOffset = (_subresourceTileOffsets[subresourceInFile] + (coordinate.Y * _tilingInfo[coordinate.Subresource].WidthInTiles + coordinate.X)) * SampleSettings.TileSizeInBytes;

            //var data = new byte[SampleSettings.TileSizeInBytes];
            var data = GetOrCreateBuffer();

            using (var stream = File.Open(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                stream.Seek(fileOffset, SeekOrigin.Begin);
                var bytesRead = stream.Read(data, 0, data.Length);
                Debug.Assert(bytesRead == data.Length);
            }

            // set a border around the loaded tile with a color depending on the mip level
            if (_border)
            {
                const int blockRows = 256 / 4;
                const int blockCols = 512 / 4;
                const int borderWidth = 2;

                var colorSelect = (17 - subresourceInFile % 15);

                for (var blockRow = 0; blockRow < blockRows; blockRow++)
                {
                    for (var blockCol = 0; blockCol < blockCols; blockCol++)
                    {
                        var shouldAddBorder = blockRow < borderWidth
                                              || blockRow >= (blockRows - borderWidth)
                                              || blockCol < borderWidth
                                              || blockCol > (blockCols - borderWidth);

                        if (shouldAddBorder)
                        {
                            var color = colors[colorSelect % colors.Length];

                            var baseIndex = (blockRow * blockCols + blockCol) * 8;

                            data[baseIndex] = (byte)((color & 0x00FF));
                            data[baseIndex + 1] = (byte)((color & 0xFF00) >> 8);

                            for (var i = 1; i < 4; i++)
                            {
                                data[baseIndex + i * sizeof(ushort)] = 0;
                                data[baseIndex + i * sizeof(ushort) + 1] = 0;
                            }
                        }
                    }
                }
            }

            return data;
        }

        private byte[] GetOrCreateBuffer()
        {
            byte[] buffer;
            lock (_buffersPool)
            {
                if (_buffersPool.Count != 0)
                {
                    buffer = _buffersPool.First();
                    _buffersPool.Remove(buffer);
                }
                else
                {
                    buffer = new byte[SampleSettings.TileSizeInBytes];
                }
            }

            return buffer;
        }

        private void ReturnBufferToPool(byte[] buffer)
        {
            lock (_buffersPool)
                _buffersPool.Add(buffer);
        }
    }
}