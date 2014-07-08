// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using SharpDX.Direct3D11;
using SharpDX.Toolkit.Diagnostics;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Contains rendering state for drawing with an effect; an effect can contain one or more passes. 
    /// </summary>
    public sealed class EffectPass : ComponentBase
    {
        private const int StageCount = 6;

        internal const int MaximumResourceCountPerStage =
            Direct3D11.CommonShaderStage.ConstantBufferApiSlotCount + // Constant buffer
            Direct3D11.CommonShaderStage.InputResourceSlotCount + // ShaderResourceView 
            Direct3D11.ComputeShaderStage.UnorderedAccessViewSlotCount + // UnorderedAccessView
            Direct3D11.CommonShaderStage.SamplerSlotCount; // SamplerStates;

        /// <summary>
        ///   Declared 128 UAV counters for SetUnordere
        /// </summary>
        private static readonly int[] UnchangedUAVCounters =
            {
                -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1
                , -1, -1, -1,
                -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1
                , -1, -1, -1,
            };

        /// <summary>
        ///   Gets the attributes associated with this pass.
        /// </summary>
        /// <value> The attributes. </value>
        public readonly PropertyCollection Properties;

        /// <summary>
        /// The parent effect of this pass.
        /// </summary>
        public readonly Effect Effect;

        private readonly EffectData.Pass pass;
        private readonly GraphicsDevice graphicsDevice;

        private PipelineBlock pipeline;

        private BlendState blendState;
        private bool hasBlendState = false;
        private Color4 blendStateColor;
        private bool hasBlendStateColor;
        private uint blendStateSampleMask;

        private DepthStencilState depthStencilState;
        private bool hasDepthStencilState = false;
        private int depthStencilReference;
        private bool hasDepthStencilReference = false;

        private bool hasRasterizerState = false;
        private RasterizerState rasterizerState;

        private InputSignatureManager inputSignatureManager;
        private InputLayoutPair currentInputLayoutPair;

        internal EffectTechnique Technique;

        private const bool EnableDebug = false;
        internal TextWriter DebugLog = new StringWriter();

        /// <summary>
        ///   Initializes a new instance of the <see cref="EffectPass" /> class.
        /// </summary>
        /// <param name="logger">The logger used to log errors.</param>
        /// <param name="effect"> The effect. </param>
        /// <param name="technique">The technique. </param>
        /// <param name="pass"> The pass. </param>
        /// <param name="name"> The name. </param>
        internal EffectPass(Logger logger, Effect effect, EffectTechnique technique, EffectData.Pass pass, string name)
            : base(name)
        {
            this.Technique = technique;
            this.pass = pass;
            this.Effect = effect;
            this.graphicsDevice = effect.GraphicsDevice;
            pipeline = new PipelineBlock()
                           {
                               Stages = new StageBlock[EffectPass.StageCount],
                           };

            Properties = PrepareProperties(logger, pass.Properties);
            IsSubPass = pass.IsSubPass;
            // Don't create SubPasses collection for subpass.
            if (!IsSubPass)
                SubPasses = new EffectPassCollection();
        }

        /// <summary>
        /// Gets the sub-pass attached to a global pass.
        /// </summary>
        /// <remarks>
        /// As a subpass cannot have subpass, if this pass is already a subpass, this field is null.
        /// </remarks>
        public readonly EffectPassCollection SubPasses;

        /// <summary>
        /// Gets a boolean indicating if this pass is a subpass.
        /// </summary>
        public readonly bool IsSubPass;

        /// <summary>
        ///   Gets or sets the state of the blend.
        /// </summary>
        /// <value> The state of the blend. </value>
        public BlendState BlendState
        {
            get { return blendState; }
            set
            {
                blendState = value;
                hasBlendState = true;
            }
        }

        /// <summary>
        ///   Gets or sets the color of the blend state.
        /// </summary>
        /// <value> The color of the blend state. </value>
        public Color4 BlendStateColor
        {
            get { return blendStateColor; }
            set
            {
                blendStateColor = value;
                hasBlendStateColor = true;
            }
        }

        /// <summary>
        ///   Gets or sets the blend state sample mask.
        /// </summary>
        /// <value> The blend state sample mask. </value>
        public uint BlendStateSampleMask
        {
            get { return blendStateSampleMask; }
            set { blendStateSampleMask = value; }
        }

        /// <summary>
        ///   Gets or sets the state of the depth stencil.
        /// </summary>
        /// <value> The state of the depth stencil. </value>
        public DepthStencilState DepthStencilState
        {
            get { return depthStencilState; }
            set
            {
                depthStencilState = value;
                hasDepthStencilState = true;
            }
        }

        /// <summary>
        ///   Gets or sets the depth stencil reference.
        /// </summary>
        /// <value> The depth stencil reference. </value>
        public int DepthStencilReference
        {
            get { return depthStencilReference; }
            set
            {
                depthStencilReference = value;
                hasDepthStencilReference = true;
            }
        }

        /// <summary>
        ///   Gets or sets the state of the rasterizer.
        /// </summary>
        /// <value> The state of the rasterizer. </value>
        public RasterizerState RasterizerState
        {
            get { return rasterizerState; }
            set
            {
                rasterizerState = value;
                hasRasterizerState = true;
            }
        }
        /// <summary>
        ///   Applies this pass to the device pipeline.
        /// </summary>
        /// <remarks>
        ///   This method is responsible to:
        ///   <ul>
        ///     <li>Setup the shader on each stage.</li>
        ///     <li>Upload constant buffers with dirty flag</li>
        ///     <li>Set all input constant buffers, shader resource view, unordered access views and sampler states to the stage.</li>
        ///   </ul>
        /// </remarks>
        public void Apply()
        {
            // Give a chance to the effect callback to prepare this pass before it is actually applied (the OnApply can completely
            // change the pass and use for example a subpass).
            var realPass = Effect.OnApply(this);
            realPass.ApplyInternal();

            // Applies global state if we have applied a subpass (that will eventually override states setup by realPass)
            if (realPass != this)
            {
                ApplyStates();
            }
        }

        /// <summary>
        /// Un-Applies this pass to the device pipeline by unbinding all resources/views previously bound by this pass. This is not mandatory to call this method, unless you want to explicitly unbind
        /// resource views that were bound by this pass.
        /// </summary>
        /// <param name="fullUnApply">if set to <c>true</c> this will unbind all resources; otherwise <c>false</c> will unbind only ShaderResourceView and UnorderedAccessView. Default is false.</param>
        public unsafe void UnApply(bool fullUnApply = false)
        {
            // If nothing to clear, return immediately
            if (graphicsDevice.CurrentPass == null)
            {
                return;
            }

            // Sets the current pass on the graphics device
            graphicsDevice.CurrentPass = null;

            // ----------------------------------------------
            // Iterate on each stage to setup all inputs
            // ----------------------------------------------
            for (int stageIndex = 0; stageIndex < pipeline.Stages.Length; stageIndex++)
            {
                var stageBlock = pipeline.Stages[stageIndex];
                if (stageBlock == null)
                {
                    continue;
                }

                var shaderStage = stageBlock.ShaderStage;

                // ----------------------------------------------
                // Setup the shader for this stage.
                // ----------------------------------------------               
                if (fullUnApply)
                {
                    shaderStage.SetShader(null, null, 0);
                }

                // If Shader is a null shader, then skip further processing
                if (stageBlock.Index < 0)
                {
                    continue;
                }

                var mergerStage = pipeline.OutputMergerStage;

                // ----------------------------------------------
                // Reset ShaderResourceView
                // ----------------------------------------------
                RawSlotLinkSet localLink = stageBlock.ShaderResourceViewSlotLinks;
                SlotLink* pLinks = localLink.Links;
                for (int i = 0; i < localLink.Count; i++)
                {
                    shaderStage.SetShaderResources(pLinks->SlotIndex, pLinks->SlotCount, graphicsDevice.ResetSlotsPointers);
                    pLinks++;
                }

                // ----------------------------------------------
                // Reset UnorderedAccessView
                // ----------------------------------------------
                localLink = stageBlock.UnorderedAccessViewSlotLinks;
                pLinks = localLink.Links;

                if (stageBlock.Type == EffectShaderType.Compute)
                {
                    for (int i = 0; i < localLink.Count; i++)
                    {
                        shaderStage.SetUnorderedAccessViews(pLinks->SlotIndex, pLinks->SlotCount, graphicsDevice.ResetSlotsPointers, pLinks->UavInitialCount);
                        pLinks++;
                    }
                }
                else
                {
                    // Otherwise, for OutputMergerStage.
                    for (int i = 0; i < localLink.Count; i++)
                    {
                        mergerStage.SetUnorderedAccessViewsKeepRTV(pLinks->SlotIndex, pLinks->SlotCount, graphicsDevice.ResetSlotsPointers, pLinks->UavInitialCount);
                        pLinks++;
                    }
                }

                if (fullUnApply)
                {
                    // ----------------------------------------------
                    // Reset Constant Buffers
                    // ----------------------------------------------
                    localLink = stageBlock.ConstantBufferSlotLinks;
                    pLinks = localLink.Links;
                    for (int i = 0; i < localLink.Count; i++)
                    {
                        shaderStage.SetConstantBuffers(pLinks->SlotIndex, pLinks->SlotCount, graphicsDevice.ResetSlotsPointers);
                        pLinks++;
                    }
                }
            }

            if (fullUnApply)
            {
                // ----------------------------------------------
                // Set the blend state
                // ----------------------------------------------
                if (hasBlendState)
                {
                    graphicsDevice.SetBlendState(null);
                }

                // ----------------------------------------------
                // Set the depth stencil state
                // ----------------------------------------------
                if (hasDepthStencilState)
                {
                    graphicsDevice.SetDepthStencilState(null);
                }

                // ----------------------------------------------
                // Set the rasterizer state
                // ----------------------------------------------
                if (hasRasterizerState)
                {
                    graphicsDevice.SetRasterizerState(null);
                }
            }
        }

        /// <summary>
        /// Internal apply.
        /// </summary>
        private unsafe void ApplyInternal()
        {
            // By default, we set the Current technique 
            Effect.CurrentTechnique = Technique;

            // Sets the current pass on the graphics device
            graphicsDevice.CurrentPass = this;

            var pLinks = pipeline.CopySlotLinks.Links;
            var pPointers = pipeline.PointersBuffer;
            var pUAVs = pipeline.UAVBuffer;
            var constantBuffers = Effect.ResourceLinker.ConstantBuffers;

            // ---------------------------------------------------------------------
            // Handle sparse input resources and update their continuous counterpart.
            // ---------------------------------------------------------------------
            var resourceLinkerPointers = Effect.ResourceLinker.Pointers;
            var resourceLinkerUAVCounts  = Effect.ResourceLinker.UAVCounts;
            for (int i = 0; i < pipeline.CopySlotLinks.Count; i++)
            {
                var pWritePtr = pPointers + pLinks->SlotIndex;
                int slotIndex = pLinks->GlobalIndex;
                if (pLinks->UavInitialCount == IntPtr.Zero)
                {
                    for (int j = 0; j < pLinks->SlotCount; j++, slotIndex++)
                    {
                        *pWritePtr++ = resourceLinkerPointers[slotIndex];
                    }
                }
                else
                {
                    var pWriteUAVPtr = pUAVs + pLinks->SlotIndex;
                    for (int j = 0; j < pLinks->SlotCount; j++, slotIndex++)
                    {
                        *pWritePtr++ = resourceLinkerPointers[slotIndex];
                        *pWriteUAVPtr++ = resourceLinkerUAVCounts[slotIndex];
                    }
                }
                pLinks++;
            }

            // ----------------------------------------------
            // Iterate on each stage to setup all inputs
            // ----------------------------------------------
            for (int stageIndex = 0; stageIndex < pipeline.Stages.Length; stageIndex++)
            {
                var stageBlock = pipeline.Stages[stageIndex];
                if (stageBlock == null)
                {
                    continue;
                }

                var shaderStage = stageBlock.ShaderStage;

                // ----------------------------------------------
                // Setup the shader for this stage.
                // ----------------------------------------------               
                shaderStage.SetShader(stageBlock.Shader, null, 0);

                // If Shader is a null shader, then skip further processing
                if (stageBlock.Index < 0)
                {
                    continue;
                }

                var mergerStage = pipeline.OutputMergerStage;

                // ----------------------------------------------
                // Setup Constant Buffers
                // ----------------------------------------------

                // Upload all constant buffers to the GPU they have been modified.
                for (int i = 0; i < stageBlock.ConstantBufferLinks.Length; i++)
                {
                    var constantBufferLink = stageBlock.ConstantBufferLinks[i];
                    var constantBuffer = constantBufferLink.ConstantBuffer;
                    if (constantBuffer.IsDirty)
                    {
                        constantBuffers[constantBufferLink.ResourceIndex].SetData(Effect.GraphicsDevice, new DataPointer(constantBuffer.DataPointer, constantBuffer.Size));
                        constantBuffer.IsDirty = false;
                    }
                }

                var localLink = stageBlock.ConstantBufferSlotLinks;
                pLinks = localLink.Links;
                for (int i = 0; i < localLink.Count; i++)
                {
                    shaderStage.SetConstantBuffers(pLinks->SlotIndex, pLinks->SlotCount, pLinks->Pointer);
                    pLinks++;
                }

                // ----------------------------------------------
                // Setup ShaderResourceView
                // ----------------------------------------------
                localLink = stageBlock.ShaderResourceViewSlotLinks;
                pLinks = localLink.Links;
                for (int i = 0; i < localLink.Count; i++)
                {
                    shaderStage.SetShaderResources(pLinks->SlotIndex, pLinks->SlotCount, pLinks->Pointer);
                    pLinks++;
                }

                // ----------------------------------------------
                // Setup UnorderedAccessView
                // ----------------------------------------------
                localLink = stageBlock.UnorderedAccessViewSlotLinks;
                pLinks = localLink.Links;

                if (stageBlock.Type == EffectShaderType.Compute)
                {
                    for (int i = 0; i < localLink.Count; i++)
                    {
                        shaderStage.SetUnorderedAccessViews(pLinks->SlotIndex, pLinks->SlotCount, pLinks->Pointer, pLinks->UavInitialCount);
                        pLinks++;
                    }
                }
                else
                {
                    // Otherwise, for OutputMergerStage.
                    for (int i = 0; i < localLink.Count; i++)
                    {
                        mergerStage.SetUnorderedAccessViewsKeepRTV(pLinks->SlotIndex, pLinks->SlotCount, pLinks->Pointer, pLinks->UavInitialCount);
                        pLinks++;
                    }
                }

                // ----------------------------------------------
                // Setup SamplerStates
                // ----------------------------------------------
                localLink = stageBlock.SamplerStateSlotLinks;
                pLinks = localLink.Links;
                for (int i = 0; i < localLink.Count; i++)
                {
                    shaderStage.SetSamplers(pLinks->SlotIndex, pLinks->SlotCount, pLinks->Pointer);
                    pLinks++;
                }
            }

            ApplyStates();
        }

        private void ApplyStates()
        {
            // ----------------------------------------------
            // Set the blend state
            // ----------------------------------------------
            if (hasBlendState)
            {
                if (hasBlendStateColor)
                {
                    graphicsDevice.SetBlendState(blendState, BlendStateColor, blendStateSampleMask);
                }
                else
                {
                    graphicsDevice.SetBlendState(blendState);
                }
            }

            // ----------------------------------------------
            // Set the depth stencil state
            // ----------------------------------------------
            if (hasDepthStencilState)
            {
                if (hasDepthStencilReference)
                {
                    graphicsDevice.SetDepthStencilState(depthStencilState, DepthStencilReference);
                }
                else
                {
                    graphicsDevice.SetDepthStencilState(depthStencilState);
                }
            }

            // ----------------------------------------------
            // Set the rasterizer state
            // ----------------------------------------------
            if (hasRasterizerState)
            {
                graphicsDevice.SetRasterizerState(rasterizerState);
            }
        }

        /// <summary>
        /// Initializes this pass.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <exception cref="System.InvalidOperationException"></exception>
        internal void Initialize(Logger logger)
        {
            // Gets the output merger stage.
            pipeline.OutputMergerStage = ((Direct3D11.DeviceContext) Effect.GraphicsDevice).OutputMerger;

            for (int i = 0; i < StageCount; i++)
            {
                var shaderType = (EffectShaderType) i;
                var link = pass.Pipeline[shaderType];
                if (link == null)
                    continue;

                if (link.IsImport)
                {
                    throw new InvalidOperationException(string.Format("Unable to resolve imported shader [{0}] for stage [{1}]", link.ImportName, shaderType));
                }

                var stageBlock = new StageBlock(shaderType);
                pipeline.Stages[i] = stageBlock;

                stageBlock.Index = link.Index;
                stageBlock.ShaderStage = Effect.GraphicsDevice.ShaderStages[i];
                stageBlock.StreamOutputElements = link.StreamOutputElements;
                stageBlock.StreamOutputRasterizedStream = link.StreamOutputRasterizedStream;

                InitStageBlock(stageBlock, logger);
            }
        }

        /// <summary>
        /// Initializes the stage block.
        /// </summary>
        /// <param name="stageBlock">The stage block.</param>
        /// <param name="logger">The logger.</param>
        private void InitStageBlock(StageBlock stageBlock, Logger logger)
        {
            // If null shader, then skip init
            var shaderIndex = stageBlock.Index;
            if (shaderIndex < 0)
            {
                return;
            }

            string errorProfile;
            stageBlock.Shader = Effect.Pool.GetOrCompileShader(stageBlock.Type, shaderIndex, stageBlock.StreamOutputRasterizedStream, stageBlock.StreamOutputElements, out errorProfile);

            if (stageBlock.Shader == null)
            {
                logger.Error(
                    "Unsupported shader profile [{0} / {1}] on current GraphicsDevice [{2}] in (effect [{3}] Technique [{4}] Pass: [{5}])",
                    stageBlock.Type,
                    errorProfile,
                    graphicsDevice.Features.Level,
                    Effect.Name,
                    Technique.Name,
                    Name);
                return;
            }

            var shaderRaw = Effect.Pool.RegisteredShaders[shaderIndex];

            // Cache the input signature
            if (shaderRaw.Type == EffectShaderType.Vertex)
            {
                inputSignatureManager = graphicsDevice.GetOrCreateInputSignatureManager(shaderRaw.InputSignature.Bytecode, shaderRaw.InputSignature.Hashcode);
            }

            for (int i = 0; i < shaderRaw.ConstantBuffers.Count; i++)
            {
                var constantBufferRaw = shaderRaw.ConstantBuffers[i];

                // Constant buffers with a null size are skipped
                if (constantBufferRaw.Size == 0)
                    continue;

                var constantBuffer = Effect.GetOrCreateConstantBuffer(Effect.GraphicsDevice, constantBufferRaw);
                // IF constant buffer is null, it means that there is a conflict
                if (constantBuffer == null)
                {
                    logger.Error("Constant buffer [{0}] cannot have multiple size or different content declaration inside the same effect pool", constantBufferRaw.Name);
                    continue;
                }
                
                // Test if this constant buffer is not already part of the effect
                if (Effect.ConstantBuffers[constantBufferRaw.Name] == null)
                {
                    // Add the declared constant buffer to the effect shader.
                    Effect.ConstantBuffers.Add(constantBuffer);

                    // Declare all parameter from constant buffer at the effect level.
                    foreach (var parameter in constantBuffer.Parameters)
                    {
                        var previousParameter = Effect.Parameters[parameter.Name];
                        if (previousParameter == null)
                        {
                            // Add an effect parameter linked to the appropriate constant buffer at the effect level.
                            Effect.Parameters.Add(new EffectParameter((EffectData.ValueTypeParameter) parameter.ParameterDescription, constantBuffer));
                        }
                        else if (parameter.ParameterDescription != previousParameter.ParameterDescription || parameter.buffer != previousParameter.buffer)
                        {
                            // If registered parameters is different
                            logger.Error("Parameter [{0}] defined in Constant buffer [{0}] is already defined by another constant buffer with the definition [{2}]", parameter, constantBuffer.Name, previousParameter);
                        }
                    }
                }
            }

            var constantBufferLinks = new List<ConstantBufferLink>();

            // Declare all resource parameters at the effect level.
            foreach (var parameterRaw in shaderRaw.ResourceParameters)
            {
                EffectParameter parameter;
                var previousParameter = Effect.Parameters[parameterRaw.Name];

                // Skip empty constant buffers.
                if (parameterRaw.Type == EffectParameterType.ConstantBuffer && Effect.ConstantBuffers[parameterRaw.Name] == null)
                {
                    continue;
                }

                int resourceIndex = Effect.ResourceLinker.Count;

                if (previousParameter == null)
                {
                    parameter = new EffectParameter(parameterRaw, EffectResourceTypeHelper.ConvertFromParameterType(parameterRaw.Type), Effect.ResourceLinker.Count, Effect.ResourceLinker);
                    Effect.Parameters.Add(parameter);

                    Effect.ResourceLinker.Count += parameterRaw.Count;
                }
                else
                {
                    if (CompareResourceParameter(parameterRaw, (EffectData.ResourceParameter) previousParameter.ParameterDescription))
                    {
                        // If registered parameters is different
                        logger.Error("Resource Parameter [{0}] is already defined with a different definition [{1}]", parameterRaw, previousParameter.ParameterDescription);
                    }
                    parameter = previousParameter;
                }

                // For constant buffers, we need to store explicit link
                if (parameter.ResourceType == EffectResourceType.ConstantBuffer)
                {
                    constantBufferLinks.Add(new ConstantBufferLink(Effect.ConstantBuffers[parameter.Name], parameter));
                }

                if (stageBlock.Parameters == null)
                {
                    stageBlock.Parameters = new List<ParameterBinding>(shaderRaw.ResourceParameters.Count);
                }

                stageBlock.Parameters.Add(new ParameterBinding(parameter, parameterRaw.Slot));
            }

            stageBlock.ConstantBufferLinks = constantBufferLinks.ToArray();
        }

        /// <summary>
        /// Optimizes the slot links.
        /// </summary>
        /// <param name="stageBlock">The stage block.</param>
        private void PrepareSlotLinks(ref StageBlock stageBlock)
        {
            // Allocate slots only when needed
            stageBlock.Slots = new List<SlotLinkSet>[1 + (int)EffectResourceType.UnorderedAccessView];

            // Retrieve Constant buffer resource index as It has been updated by the reordering of resources
            for (int i = 0; i < stageBlock.ConstantBufferLinks.Length; i++)
            {
                stageBlock.ConstantBufferLinks[i].ResourceIndex = stageBlock.ConstantBufferLinks[i].Parameter.Offset;
            }

            // Compute default slot links link
            foreach (var parameterBinding in stageBlock.Parameters)
            {
                var parameter = parameterBinding.Parameter;

                var slots = stageBlock.Slots[(int)parameter.ResourceType];
                if (slots == null)
                {
                    slots = new List<SlotLinkSet>();
                    stageBlock.Slots[(int)parameter.ResourceType] = slots;
                }

                var parameterRaw = (EffectData.ResourceParameter)parameter.ParameterDescription;

                var range = new SlotLinkSet() { SlotCount = parameterRaw.Count, SlotIndex = parameterBinding.Slot };
                slots.Add(range);
                range.Links.Add(new SlotLink(parameter.Offset, 0, parameterRaw.Count));
            }

            if (EnableDebug)
            {
                DebugLog.WriteLine("*** Before OptimizeSlotLinks ****");
                PrintLinks(ref stageBlock);
            }

            // Optimize all slots
            foreach (var slotRangePerResourceType in stageBlock.Slots)
            {
                if (slotRangePerResourceType == null)
                    continue;

                var previousRange = slotRangePerResourceType[0];

                for (int i = 1; i < slotRangePerResourceType.Count; i++)
                {
                    var currentRange = slotRangePerResourceType[i];
                    int endIndex = previousRange.SlotIndex + previousRange.SlotCount;

                    var delta = (currentRange.SlotIndex - endIndex);

                    // If there is at maximum a 1 
                    if (delta <= 1)
                    {
                        foreach (var slotLink in currentRange.Links)
                        {
                            var previousLink = previousRange.Links[previousRange.Links.Count - 1];
                            // Merge consecutive individual slot link
                            if ((previousLink.GlobalIndex + previousLink.SlotCount) == slotLink.GlobalIndex && (previousLink.SlotIndex + previousLink.SlotCount) == (currentRange.SlotIndex + slotLink.SlotIndex))
                            {
                                previousLink.SlotCount += slotLink.SlotCount;
                                previousRange.Links[previousRange.Links.Count - 1] = previousLink;
                            }
                            else
                            {
                                previousRange.Links.Add(new SlotLink(slotLink.GlobalIndex, (slotLink.SlotIndex + previousRange.SlotCount + delta), slotLink.SlotCount));
                            }
                        }

                        // Update the total slot count
                        previousRange.SlotCount += (short)(delta + currentRange.SlotCount);

                        slotRangePerResourceType.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        previousRange = currentRange;
                    }
                }
            }

            if (EnableDebug)
            {
                DebugLog.WriteLine("*** After OptimizeSlotLinks ****");
                PrintLinks(ref stageBlock);
            }
        }

        /// <summary>
        /// Computes the slot links.
        /// </summary>
        internal unsafe void ComputeSlotLinks()
        {
            // Total size allocated for the unmanaged buffers.
            int slotTotalMemory = 0;

            // Offset in bytes to the all SlotLinks to resource
            int singleSlotLinksOffset = 0;

            // Offset in bytes to the begin of buffer pointers
            int pointerBuffersOffset = 0;

            // ----------------------------------------------------------------------------------
            // 0 pass: Prepare all slot links
            // ----------------------------------------------------------------------------------
            foreach (var stageBlockVar in pipeline.Stages)
            {
                var stageBlock = stageBlockVar;

                if (stageBlock == null || stageBlock.Parameters == null)
                    continue;

                PrepareSlotLinks(ref stageBlock);
            }

            // ----------------------------------------------------------------------------------
            // 1st pass: calculate memory for all SlotLinks and buffer of pointers for each stage
            // ----------------------------------------------------------------------------------
            int totalPointerCount = 0;
            foreach (var stageBlockVar in pipeline.Stages)
            {
                var stageBlock = stageBlockVar;

                if (stageBlock == null || stageBlock.Slots == null)
                    continue;

                for (int resourceType = 0; resourceType < stageBlock.Slots.Length; resourceType++)
                {
                    var slotLinkSetList = stageBlock.Slots[resourceType];
                    if (slotLinkSetList == null)
                    {
                        continue;
                    }

                    // Allocate memory for slotlinks per type
                    singleSlotLinksOffset += slotLinkSetList.Count * Utilities.SizeOf<SlotLink>();

                    // Allocate memory for each single slot links
                    foreach (var slotLinkSet in slotLinkSetList)
                    {
                        // Optimization: If there is only 1 slot link set, than we can use direct reference to EffectResourceLinker
                        // else we need to use an intermediate buffer
                        if (!slotLinkSet.IsDirectSlot)
                        {
                            pointerBuffersOffset += slotLinkSet.Links.Count * Utilities.SizeOf<SlotLink>();
                            slotTotalMemory += slotLinkSet.SlotCount * Utilities.SizeOf<IntPtr>(); // +PointerResources
                            slotTotalMemory += slotLinkSet.SlotCount * sizeof(int); // + UAVCounts
                            totalPointerCount += slotLinkSet.SlotCount;
                        }
                    }
                }
            }

            slotTotalMemory += singleSlotLinksOffset;
            slotTotalMemory += pointerBuffersOffset;

            pointerBuffersOffset += singleSlotLinksOffset;

            // Allocate all memory
            pipeline.GlobalSlotPointer = Effect.DisposeCollector.Collect(Utilities.AllocateMemory(slotTotalMemory));

            // Clear this memory
            Utilities.ClearMemory(pipeline.GlobalSlotPointer, 0, slotTotalMemory);

            // Calculate address of slotlinks
            pipeline.CopySlotLinks.Links = (SlotLink*) ((byte*) pipeline.GlobalSlotPointer + singleSlotLinksOffset);

            // Calculate address of buffer pointers
            pipeline.PointersBuffer = (IntPtr*) ((byte*) pipeline.GlobalSlotPointer + pointerBuffersOffset);
            pipeline.UAVBuffer = (int*)((byte*)pipeline.PointersBuffer + totalPointerCount * Utilities.SizeOf<IntPtr>());

            // Initialize default UAVCounts to -1
            for (int i = 0; i < totalPointerCount; i++)
            {
                pipeline.UAVBuffer[i] = -1;
            }

            if (EnableDebug)
            {
                DebugLog.WriteLine("Memory Layout for Effect [{0}] Pass [{1}]", Effect.Name, Name);
                DebugLog.WriteLine("Global SlotLinks Native Buffer [0x{0:X} - 0x{1:X}] ({2} bytes)", pipeline.GlobalSlotPointer.ToInt64(), new IntPtr((byte*)pipeline.GlobalSlotPointer + slotTotalMemory).ToInt64(), slotTotalMemory);

                DebugLog.WriteLine("SlotLinks Per Type [0x{0:X} - 0x{1:X}] ({2} bytes)", pipeline.GlobalSlotPointer.ToInt64(), new IntPtr(pipeline.CopySlotLinks.Links).ToInt64(), singleSlotLinksOffset);

                DebugLog.WriteLine("SlotLinks Copy [0x{0:X} - 0x{1:X}] ({2} bytes)", new IntPtr(pipeline.CopySlotLinks.Links).ToInt64(), new IntPtr(pipeline.PointersBuffer).ToInt64(), pointerBuffersOffset - singleSlotLinksOffset);

                DebugLog.WriteLine("Slot Copy Pointers [0x{0:X} - 0x{1:X}] ({2} bytes)", new IntPtr(pipeline.PointersBuffer).ToInt64(), new IntPtr((byte*)pipeline.GlobalSlotPointer + slotTotalMemory).ToInt64(), slotTotalMemory - pointerBuffersOffset);
                DebugLog.WriteLine("");
                DebugLog.Flush();
            }

            // ----------------------------------------------------------------------------------
            // 2nd pass: calculate memory for all SlotLinks and buffer of pointers for each stage
            // ----------------------------------------------------------------------------------
            var globalSoftLink = (SlotLink*) pipeline.GlobalSlotPointer;

            var slotLinks = pipeline.CopySlotLinks.Links;

            int currentPointerIndex = 0;

            foreach (var stageBlock in pipeline.Stages)
            {
                if (stageBlock == null)
                    continue;

                if (stageBlock.Slots == null)
                    continue;

                for (int resourceType = 0; resourceType < stageBlock.Slots.Length; resourceType++)
                {
                    var slotLinkSetList = stageBlock.Slots[resourceType];
                    if (slotLinkSetList == null)
                        continue;

                    var slotLinksPerType = globalSoftLink;

                    bool isUAV = false;
                    switch ((EffectResourceType) resourceType)
                    {
                        case EffectResourceType.ConstantBuffer:
                            stageBlock.ConstantBufferSlotLinks.Count = slotLinkSetList.Count;
                            stageBlock.ConstantBufferSlotLinks.Links = slotLinksPerType;
                            break;
                        case EffectResourceType.ShaderResourceView:
                            stageBlock.ShaderResourceViewSlotLinks.Count = slotLinkSetList.Count;
                            stageBlock.ShaderResourceViewSlotLinks.Links = slotLinksPerType;
                            break;
                        case EffectResourceType.UnorderedAccessView:
                            stageBlock.UnorderedAccessViewSlotLinks.Count = slotLinkSetList.Count;
                            stageBlock.UnorderedAccessViewSlotLinks.Links = slotLinksPerType;
                            isUAV = true;
                            break;
                        case EffectResourceType.SamplerState:
                            stageBlock.SamplerStateSlotLinks.Count = slotLinkSetList.Count;
                            stageBlock.SamplerStateSlotLinks.Links = slotLinksPerType;
                            break;
                    }

                    globalSoftLink += slotLinkSetList.Count;

                    // Allocate memory for each single slot links
                    foreach (var slotLinkSet in slotLinkSetList)
                    {
                        // Calculate the memory offset from the beginning of the 

                        slotLinksPerType->SlotIndex = slotLinkSet.SlotIndex;
                        slotLinksPerType->SlotCount = slotLinkSet.SlotCount;
                        if (slotLinkSet.IsDirectSlot)
                        {
                            slotLinksPerType->Pointer = (IntPtr) (Effect.ResourceLinker.Pointers + slotLinkSet.Links[0].GlobalIndex);

                            // Special case for UAV initial counters
                            if (isUAV)
                            {
                                slotLinksPerType->UavInitialCount = (IntPtr)(Effect.ResourceLinker.UAVCounts + slotLinkSet.Links[0].GlobalIndex);
                            }
                        }
                        else
                        {
                            slotLinksPerType->Pointer = (IntPtr)((byte*)pipeline.PointersBuffer + currentPointerIndex * Utilities.SizeOf<IntPtr>());

                            // Special case for UAV initial counters
                            if (isUAV)
                            {
                                slotLinksPerType->UavInitialCount = (IntPtr)((byte*)pipeline.UAVBuffer + currentPointerIndex * Utilities.SizeOf<int>());
                            }
                            
                            foreach (SlotLink localLink in slotLinkSet.Links)
                            {
                                var slotLink = localLink;

                                // Make slotIndex absolute
                                slotLink.SlotIndex = slotLink.SlotIndex + currentPointerIndex;
                                *slotLinks++ = slotLink;
                                pipeline.CopySlotLinks.Count++;
                            }

                            currentPointerIndex += slotLinkSet.SlotCount;
                        }

                        slotLinksPerType++;
                    }
                }
            }
        }

        private void PrintLinks(ref StageBlock stageBlock)
        {
            DebugLog.WriteLine("  |- Stage [{0}]", stageBlock.Shader.GetType().Name);
            for (int resourceTypeIndex = 0; resourceTypeIndex < stageBlock.Slots.Length; resourceTypeIndex++)
            {
                var resourceType = (EffectResourceType)resourceTypeIndex;
                var slotRangePerResourceType = stageBlock.Slots[resourceTypeIndex];
                if (slotRangePerResourceType == null)
                {
                    continue;
                }

                DebugLog.WriteLine("     |- ResourceType [{0}]", resourceType);
                foreach (var slotLinkSet in slotRangePerResourceType)
                {
                    DebugLog.WriteLine("        |- SlotRange Slot [{0}] Count [{1}] (IsDirect: {2})", slotLinkSet.SlotIndex, slotLinkSet.SlotCount, slotLinkSet.IsDirectSlot);
                    foreach (var slotLink in slotLinkSet.Links)
                    {
                        DebugLog.WriteLine("           |- Resource [{0}] Index [{1}] Count[{2}]", slotLink.GlobalIndex, slotLink.SlotIndex, slotLink.SlotCount);
                    }
                }
            }
        }

        private bool CompareResourceParameter(EffectData.ResourceParameter left, EffectData.ResourceParameter right)
        {
            return (left.Class != right.Class || left.Type != right.Type || left.Count != right.Count);
        }

        private PropertyCollection PrepareProperties(Logger logger, CommonData.PropertyCollection properties)
        {
            var passProperties = new PropertyCollection();

            foreach (var property in properties)
            {
                switch (property.Key)
                {
                    case EffectData.PropertyKeys.Blending:
                        BlendState = graphicsDevice.BlendStates[(string)property.Value];
                        if (BlendState == null)
                            logger.Error("Unable to find registered BlendState [{0}]", (string)property.Value);
                        break;
                    case EffectData.PropertyKeys.BlendingColor:
                        BlendStateColor = (Color4)(Vector4)property.Value;
                        break;
                    case EffectData.PropertyKeys.BlendingSampleMask:
                        BlendStateSampleMask = (uint)property.Value;
                        break;

                    case EffectData.PropertyKeys.DepthStencil:
                        DepthStencilState = graphicsDevice.DepthStencilStates[(string)property.Value];
                        if (DepthStencilState == null)
                            logger.Error("Unable to find registered DepthStencilState [{0}]", (string)property.Value);
                        break;
                    case EffectData.PropertyKeys.DepthStencilReference:
                        DepthStencilReference = (int)property.Value;
                        break;

                    case EffectData.PropertyKeys.Rasterizer:
                        RasterizerState = graphicsDevice.RasterizerStates[(string)property.Value];
                        if (RasterizerState == null)
                            logger.Error("Unable to find registered RasterizerState [{0}]", (string)property.Value);
                        break;
                    default:
                        passProperties[new PropertyKey(property.Key)] = property.Value;
                        break;
                }
            }

            return passProperties;
        }


        private readonly Dictionary<VertexInputLayout, InputLayoutPair> LocalInputLayoutCache = new Dictionary<VertexInputLayout, InputLayoutPair>();

        internal InputLayout GetInputLayout(VertexInputLayout layout)
        {
            if (layout == null)
                return null;

            if (!ReferenceEquals(currentInputLayoutPair.VertexInputLayout, layout))
            {
                // Use a local cache to speed up retrieval
                if (!LocalInputLayoutCache.TryGetValue(layout, out currentInputLayoutPair))
                {
                    inputSignatureManager.GetOrCreate(layout, out currentInputLayoutPair);
                    LocalInputLayoutCache.Add(layout, currentInputLayoutPair);
                }
            }
            return currentInputLayoutPair.InputLayout;
        }

        #region Nested type: PipelineBlock

        private struct PipelineBlock
        {
            public IntPtr GlobalSlotPointer;

            public OutputMergerStage OutputMergerStage;
            public unsafe IntPtr* PointersBuffer;
            public unsafe int* UAVBuffer;
            public RawSlotLinkSet CopySlotLinks;
            public StageBlock[] Stages;
        }

        private struct ParameterBinding
        {
            public ParameterBinding(EffectParameter parameter, int slot)
            {
                Parameter = parameter;
                Slot = slot;
            }

            public readonly EffectParameter Parameter;

            public readonly int Slot;
        }

        #endregion

        #region Nested type: RawSlotLinkSet

        private struct RawSlotLinkSet
        {
            /// <summary>
            ///   Number of SlotLinks.
            /// </summary>
            public int Count;

            /// <summary>
            ///   SlotLink* ptr;
            /// </summary>
            public unsafe SlotLink* Links;
        }

        #endregion

        #region Nested type: SlotLink

        // The CLR seems to have a bug when using an explicit layout and adding this object
        // to a list. The object is not correctly copied!
        [StructLayout(LayoutKind.Sequential)]
        private struct SlotLink
        {
            public SlotLink(int globalIndex, int slotIndex, int slotCount)
            {
                Pointer = new IntPtr(globalIndex);
                SlotIndex = slotIndex;
                SlotCount = slotCount;
                UavInitialCount = IntPtr.Zero;
            }

            public IntPtr Pointer;

            public int SlotIndex;

            public int SlotCount;

            public IntPtr UavInitialCount;

            public int GlobalIndex
            {
                get
                {
                    return Pointer.ToInt32();
                }
            }
        }

        #endregion

        #region Nested type: SlotLinkSet

        private class SlotLinkSet
        {
            public List<SlotLink> Links;
            public short SlotCount;
            public int SlotIndex;

            public SlotLinkSet()
            {
                Links = new List<SlotLink>();
            }

            public bool IsDirectSlot
            {
                get { return Links.Count == 1; }
            }
        }

        #endregion

        #region Nested type: StageBlock

        private class StageBlock
        {
            public List<ParameterBinding> Parameters;

            public RawSlotLinkSet ConstantBufferSlotLinks;
            public ConstantBufferLink[] ConstantBufferLinks;
            public int Index;
            public RawSlotLinkSet SamplerStateSlotLinks;

            public DeviceChild Shader;

            public RawSlotLinkSet ShaderResourceViewSlotLinks;

            public CommonShaderStage ShaderStage;
            public List<SlotLinkSet>[] Slots;
            public EffectShaderType Type;
            public RawSlotLinkSet UnorderedAccessViewSlotLinks;

            public StreamOutputElement[] StreamOutputElements;

            public int StreamOutputRasterizedStream;

            public StageBlock(EffectShaderType type)
            {
                Type = type;
            }
        }

        private struct ConstantBufferLink
        {
            public ConstantBufferLink(EffectConstantBuffer constantBuffer, EffectParameter parameter)
            {
                ConstantBuffer = constantBuffer;
                Parameter = parameter;
                ResourceIndex = 0;
            }

            public readonly EffectConstantBuffer ConstantBuffer;

            public readonly EffectParameter Parameter;

            public int ResourceIndex;
        }

        #endregion
    }
}