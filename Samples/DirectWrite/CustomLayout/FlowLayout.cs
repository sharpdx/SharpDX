// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
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
using SharpDX.DirectWrite;
using SharpDX;

namespace CustomLayout
{
    /// <summary>
    /// Custom layout, demonstrating usage of shaping and glyph results.
    /// </summary>
    internal class FlowLayout
    {
        // This custom layout processes layout in two stages.
        //
        // 1. Analyze the text, given the current font and size
        //      a. Bidirectional analysis
        //      b. Script analysis
        //      c. Number substitution analysis
        //      d. Shape glyphs
        //      e. Intersect run results
        //
        // 2. Fill the text to the given shape
        //      a. Pull next rect from flow source
        //      b. Fit as much text as will go in
        //      c. Push text to flow sink

        // Input information.
        protected Factory dwriteFactory_;
        protected String text_, localName_;
        protected ReadingDirection readingDirection_;
        protected FontFace fontFace_;
        protected NumberSubstitution numberSubstitution_;
        protected float fontEmSize_;

        // Output text analysis results
        protected Run[] runs_;
        protected LineBreakpoint[] breakpoints_;
        protected GlyphOffset[] glyphOffsets_;
        protected short[] glyphClusters_;
        protected short[] glyphIndices_;
        protected float[] glyphAdvances_;

        protected float maxSpaceWidth_;           // maximum stretch of space allowed for justification
        protected bool isTextAnalysisComplete_;   // text analysis was done.

        public FlowLayout(Factory dwriteFactory)
        {
            dwriteFactory_ = dwriteFactory;
            readingDirection_ = ReadingDirection.LeftToRight;
            fontEmSize_ = 12;
            maxSpaceWidth_ = 8;
            isTextAnalysisComplete_ = false;
        }
        public void Dispose()
        {
            fontFace_.Dispose();
        }

        /// <summary>
        /// Estimates the maximum number of glyph indices needed to hold a string of 
        /// a given length.  This is the formula given in the Uniscribe SDK and should
        /// cover most cases. Degenerate cases will require a reallocation.
        /// </summary>
        public static int EstimateGlyphCount(int textLength)
        {
            return 3 * textLength / 2 + 16;
        }

        public void SetTextFormat(TextFormat textFormat)
        {
            // Initializes properties using a text format, like font family, font size,
            // and reading direction. For simplicity, this custom layout supports
            // minimal formatting. No mixed formatting or alignment modes are supported.
            readingDirection_ = textFormat.ReadingDirection;
            fontEmSize_ = textFormat.FontSize;
            localName_ = textFormat.LocaleName;

            // Map font and style to fontFace.
            FontCollection fontCollection = textFormat.FontCollection;// Need the font collection to map from font name to actual font.
            if (fontCollection == null)
                fontCollection = dwriteFactory_.GetSystemFontCollection(false);// No font collection was set in the format, so use the system default.

            // Find matching family name in collection.
            String fontFamilyName = textFormat.FontFamilyName;

            int fontIndex;
            // If the given font does not exist, take what we can get
            // (displaying something instead nothing), choosing the foremost
            // font in the collection.
            if (!fontCollection.FindFamilyName(fontFamilyName, out fontIndex))
                fontIndex = 0;

            FontFamily fontFamily = fontCollection.GetFontFamily(fontIndex);
            Font font = fontFamily.GetFirstMatchingFont(textFormat.FontWeight, textFormat.FontStretch, textFormat.FontStyle);
            fontFace_ = new FontFace(font);

            font.Dispose();
            fontFamily.Dispose();
            fontCollection.Dispose();
        }
        public void SetNumberSubstitution(NumberSubstitution numberSubstitution)
        {
            numberSubstitution_ = numberSubstitution;
        }
        public void AnalyzeText(String text)
        {
            // Perform analysis on the given text, converting text to glyphs.

            // Analyzes the given text and keeps the results for later reflow.
            isTextAnalysisComplete_ = false;

            // Need a font face to determine metrics.
            if (fontFace_ == null)
                throw new Exception("FlowLayout: Need a font face to determine metrics.");

            text_ = text;

            // Query for the text analyzer's interface.
            TextAnalyzer textAnalyzer = new TextAnalyzer(dwriteFactory_);

            // Record the analyzer's results.
            TextAnalysis textAnalysis = new TextAnalysis(text_, localName_, readingDirection_, numberSubstitution_);
            textAnalysis.GenerateResults(textAnalyzer, out runs_, out breakpoints_);

            // Convert the entire text to glyphs.
            ShapeGlyphRuns(textAnalyzer);

            isTextAnalysisComplete_ = true;

            textAnalyzer.Dispose();
        }
        protected void ShapeGlyphRuns(TextAnalyzer textAnalyzer)
        {
            // Shapes all the glyph runs in the layout.

            // Estimate the maximum number of glyph indices needed to hold a string.
            int estimatedGlyphCount = EstimateGlyphCount(text_.Length);

            glyphIndices_ = new short[estimatedGlyphCount];
            glyphOffsets_ = new GlyphOffset[estimatedGlyphCount];
            glyphAdvances_ = new float[estimatedGlyphCount];
            glyphClusters_ = new short[text_.Length];

            int glyphStart = 0;

            // Shape each run separately. This is needed whenever script, locale,
            // or reading direction changes.
            for (int runIndex = 0; runIndex < runs_.Length; runIndex++)
            {
                ShapeGlyphRun(textAnalyzer, runIndex, ref glyphStart);
            }

            short[] resized_glyphIndices_ = new short[glyphStart];
            Array.Copy(glyphIndices_, 0, resized_glyphIndices_, 0, glyphStart);
            glyphIndices_ = resized_glyphIndices_;

            GlyphOffset[] resized_glyphOffsets_ = new GlyphOffset[glyphStart];
            Array.Copy(glyphOffsets_, 0, resized_glyphOffsets_, 0, glyphStart);
            glyphOffsets_ = resized_glyphOffsets_;

            float[] resized_glyphAdvances_ = new float[glyphStart];
            Array.Copy(glyphAdvances_, 0, resized_glyphAdvances_, 0, glyphStart);
            glyphAdvances_ = resized_glyphAdvances_;

        }
        protected void ShapeGlyphRun(TextAnalyzer textAnalyzer, int runIndex, ref int glyphStart)
        {
            // Shapes a single run of text into glyphs.
            // Alternately, you could iteratively interleave shaping and line
            // breaking to reduce the number glyphs held onto at once. It's simpler
            // for this demostration to just do shaping and line breaking as two
            // separate processes, but realize that this does have the consequence that
            // certain advanced fonts containing line specific features (like Gabriola)
            // will shape as if the line is not broken.

            Run run = runs_[runIndex];
            int textStart = run.textStart;
            int textLength = run.textLength;
            int maxGlyphCount = glyphIndices_.Length - glyphStart;
            int actualGlyphCount = 0;

            run.glyphStart = glyphStart;
            run.glyphCount = 0;

            if (textLength == 0)
                return;// Nothing to do..

            // Allocate space for shaping to fill with glyphs and other information,
            // with about as many glyphs as there are text characters. We'll actually
            // need more glyphs than codepoints if they are decomposed into separate
            // glyphs, or fewer glyphs than codepoints if multiple are substituted
            // into a single glyph. In any case, the shaping process will need some
            // room to apply those rules to even make that determintation.

            if (textLength > maxGlyphCount)
            {
                maxGlyphCount = EstimateGlyphCount(textLength);
                int totalGlyphsArrayCount = glyphStart + maxGlyphCount;
                short[] Resized_glyphIndices_ = new short[totalGlyphsArrayCount];
                glyphIndices_.CopyTo(Resized_glyphIndices_, 0);
                glyphIndices_ = Resized_glyphIndices_;
            }


            ShapingTextProperties[] textProps = new ShapingTextProperties[textLength];
            ShapingGlyphProperties[] glyphProps = new ShapingGlyphProperties[maxGlyphCount];

            // Get the glyphs from the text, retrying if needed.
            int tries = 0;
            do
            {
                short[] call_glyphClusters_ = new short[glyphClusters_.Length - textStart];
                short[] call_glyphIndices_ = new short[glyphIndices_.Length - glyphStart];

                var result = textAnalyzer.GetGlyphs(
                    text_.Substring(textStart, textLength),
                    textLength,
                    fontFace_,
                    run.isSideways,
                    (run.bidiLevel % 2 == 1),
                    run.script,
                    localName_,
                    run.isNumberSubstituted ? numberSubstitution_ : null,
                    null,
                    null,
                    0,
                    maxGlyphCount,
                    call_glyphClusters_,
                    textProps,
                    call_glyphIndices_,
                    glyphProps,
                    out actualGlyphCount);
                Array.Copy(call_glyphClusters_, 0, glyphClusters_, textStart, call_glyphClusters_.Length);
                Array.Copy(call_glyphIndices_, 0, glyphIndices_, glyphStart, call_glyphIndices_.Length);
                tries++;
                //if (result!=SharpDX.Result.OutOfMemory)
                if (result != SharpDX.Result.Ok)
                {
                    // Try again using a larger buffer.
                    maxGlyphCount = EstimateGlyphCount(maxGlyphCount);
                    int totalGlyphsArrayCount = glyphStart + maxGlyphCount;

                    glyphProps = new ShapingGlyphProperties[maxGlyphCount];
                    glyphIndices_ = new short[totalGlyphsArrayCount];
                }
                else
                    break;
            }
            while (tries < 2);// We'll give it two chances.

            // Get the placement of the all the glyphs.
            if (glyphAdvances_.Length < glyphStart + actualGlyphCount)
            {
                float[] Resized_glyphAdvances_ = new float[glyphStart + actualGlyphCount];
                glyphAdvances_.CopyTo(Resized_glyphAdvances_, 0);
                glyphAdvances_ = Resized_glyphAdvances_;
            }
            if (glyphOffsets_.Length < glyphStart + actualGlyphCount)
            {
                GlyphOffset[] Resized_glyphOffsets_ = new GlyphOffset[glyphStart + actualGlyphCount];
                glyphOffsets_.CopyTo(Resized_glyphOffsets_, 0);
                glyphOffsets_ = Resized_glyphOffsets_;
            }

            short[] call2_glyphClusters_ = new short[glyphClusters_.Length - textStart];
            Array.Copy(glyphClusters_, textStart, call2_glyphClusters_, 0, call2_glyphClusters_.Length);
            short[] call2_glyphIndices_ = new short[glyphIndices_.Length - glyphStart];
            Array.Copy(glyphIndices_, glyphStart, call2_glyphIndices_, 0, call2_glyphIndices_.Length);
            float[] call2_glyphAdvances_ = new float[glyphAdvances_.Length - glyphStart];
            Array.Copy(glyphAdvances_, glyphStart, call2_glyphAdvances_, 0, call2_glyphAdvances_.Length);
            GlyphOffset[] call2_glyphOffsets_ = new GlyphOffset[glyphOffsets_.Length - glyphStart];
            Array.Copy(glyphOffsets_, glyphStart, call2_glyphOffsets_, 0, call2_glyphOffsets_.Length);

            var result2 = textAnalyzer.GetGlyphPlacements(
                text_.Substring(textStart, textLength),
                call2_glyphClusters_,
                textProps,
                textLength,
                call2_glyphIndices_,
                glyphProps,
                actualGlyphCount,
                fontFace_,
                fontEmSize_,
                run.isSideways,
                run.bidiLevel % 2 == 1,
                run.script,
                localName_,
                null,
                null,
                0,
                call2_glyphAdvances_,
                call2_glyphOffsets_);
            //call2_glyphClusters_.CopyTo(glyphClusters_, textStart);
            call2_glyphAdvances_.CopyTo(glyphAdvances_, glyphStart);
            call2_glyphOffsets_.CopyTo(glyphOffsets_, glyphStart);

            // Certain fonts, like Batang, contain glyphs for hidden control
            // and formatting characters. So we'll want to explicitly force their
            // advance to zero.
            if (run.script.Shapes == ScriptShapes.NoVisual)
            {
                for (int i = glyphStart; i < glyphStart + actualGlyphCount; i++)
                    glyphAdvances_[i] = 0;
            }

            // Set the final glyph count of this run and advance the starting glyph.
            run.glyphCount = actualGlyphCount;
            runs_[runIndex] = run;
            glyphStart += actualGlyphCount;
        }
        public void FlowText(FlowLayoutSource flowSource, FlowLayoutSink flowSink)
        {
            // Reflow all the text, from source to sink.
            if (!isTextAnalysisComplete_)
                throw new Exception("FlowLayout: Text analysis in not complete");

            // Determine the font line height, needed by the flow source.
            FontMetrics fontMetrics = fontFace_.Metrics;
            float fontHeight = (fontMetrics.Ascent + fontMetrics.Descent + fontMetrics.LineGap) * fontEmSize_ / fontMetrics.DesignUnitsPerEm;

            // Set initial cluster position to beginning of text.
            ClusterPosition cluster = new ClusterPosition();
            SetClusterPosition(ref cluster, 0);
            RectangleF rect;
            ClusterPosition nextCluster;

            // Iteratively pull rect's from the source,
            // and push as much text will fit to the sink.
            while (cluster.textPosition < text_.Length)
            {
                // Pull the next rect from the source.
                if (!flowSource.GetNextRect(fontHeight, out rect))
                    break;

                if (rect.Right - rect.Left <= 0)
                    break; // Stop upon reaching zero sized rects.

                // Fit as many clusters between breakpoints that will go in.
                if (!FitText(ref cluster, text_.Length, rect.Right - rect.Left, out nextCluster))
                    break;

                // Push the glyph runs to the sink.
                if (!ProduceGlyphRuns(flowSink, ref rect, ref cluster, ref nextCluster))
                    break;

                cluster = nextCluster;
            }
        }
        protected bool FitText(ref ClusterPosition clusterStart, int textEnd, float maxWidth, out ClusterPosition clusterEnd)
        {
            // Fits as much text as possible into the given width,
            // using the clusters and advances returned by DWrite.

            ////////////////////////////////////////
            // Set the starting cluster to the starting text position,
            // and continue until we exceed the maximum width or hit
            // a hard break.
            ClusterPosition cluster = clusterStart;
            ClusterPosition nextCluster = clusterStart;
            int validBreakPosition = cluster.textPosition;
            int bestBreakPosition = cluster.textPosition;
            float textWidth = 0;

            while (cluster.textPosition < textEnd)
            {
                // Use breakpoint information to find where we can safely break words.
                AdvanceClusterPosition(ref nextCluster);
                LineBreakpoint breakpoint = breakpoints_[nextCluster.textPosition - 1];

                // Check whether we exceeded the amount of text that can fit,
                // unless it's whitespace, which we allow to flow beyond the end.

                textWidth += GetClusterRangeWidth(ref cluster, ref nextCluster);
                if (textWidth > maxWidth && !breakpoint.IsWhitespace)
                {
                    // Want a minimum of one cluster.
                    if (validBreakPosition > clusterStart.textPosition)
                        break;
                }

                validBreakPosition = nextCluster.textPosition;

                // See if we can break after this character cluster, and if so,
                // mark it as the new potential break point.
                if (breakpoint.BreakConditionAfter != BreakCondition.MayNotBreak)
                {
                    bestBreakPosition = validBreakPosition;
                    if (breakpoint.BreakConditionAfter == BreakCondition.MustBreak)
                        break; // we have a hard return, so we've fit all we can.
                }
                cluster = nextCluster;
            }

            ////////////////////////////////////////
            // Want last best position that didn't break a word, but if that's not available,
            // fit at least one cluster (emergency line breaking).
            if (bestBreakPosition == clusterStart.textPosition)
                bestBreakPosition = validBreakPosition;

            SetClusterPosition(ref cluster, bestBreakPosition);

            clusterEnd = cluster;

            return true;
        }
        protected bool ProduceGlyphRuns(FlowLayoutSink flowSink, ref RectangleF rect, ref ClusterPosition clusterStart, ref ClusterPosition clusterEnd)
        {
            // Produce a series of glyph runs from the given range
            // and send them to the sink. If the entire text fit
            // into the rect, then we'll only pass on a single glyph
            // run.

            ////////////////////////////////////////
            // Figure out how many runs we cross, because this is the number
            // of distinct glyph runs we'll need to reorder visually.

            int runIndexEnd = clusterEnd.runIndex;
            if (clusterEnd.textPosition > runs_[runIndexEnd].textStart)
                ++runIndexEnd; // Only partially cover the run, so round up.

            int[] bidiOrdering = new int[100];
            int totalRuns = Math.Min(bidiOrdering.Length, runIndexEnd - clusterStart.runIndex);
            ProduceBidiOrdering(clusterStart.runIndex, totalRuns, bidiOrdering);

            ////////////////////////////////////////
            // Ignore any trailing whitespace

            // Look backward from end until we find non-space.
            int trailingWsPosition;
            for (trailingWsPosition = clusterEnd.textPosition; trailingWsPosition > clusterStart.textPosition; --trailingWsPosition)
            {
                if (!breakpoints_[trailingWsPosition - 1].IsWhitespace)
                    break; // Encountered last significant character.
            }
            // Set the glyph run's ending cluster to the last whitespace.
            ClusterPosition clusterWsEnd = clusterStart;
            SetClusterPosition(ref clusterWsEnd, trailingWsPosition);

            ////////////////////////////////////////
            // Produce justified advances to reduce the jagged edge.
            List<float> justifiedAdvances;
            ProduceJustifiedAdvances(ref rect, ref clusterStart, ref clusterWsEnd, out justifiedAdvances);
            int justificationGlyphStart = GetClusterGlyphStart(ref clusterStart);
            ////////////////////////////////////////
            // Determine starting point for alignment.
            float x = rect.Left;
            float y = rect.Bottom;

            FontMetrics fontMetrics = fontFace_.Metrics;

            float descent = (fontMetrics.Descent * fontEmSize_ / fontMetrics.DesignUnitsPerEm);
            y -= descent;

            if (readingDirection_ == ReadingDirection.RightToLeft)
            {
                // For RTL, we neeed the run width to adjust the origin
                // so it starts on the right side.
                int glyphStart = GetClusterGlyphStart(ref clusterStart);
                int glyphEnd = GetClusterGlyphStart(ref clusterWsEnd);

                if (glyphStart < glyphEnd)
                {
                    float lineWidth = GetClusterRangeWidth(
                        glyphStart - justificationGlyphStart,
                        glyphEnd - justificationGlyphStart,
                        justifiedAdvances.ToArray()
                        );
                    x = rect.Right - lineWidth;
                }
            }

            ////////////////////////////////////////
            // Send each glyph run to the sink.
            for (int i = 0; i < totalRuns; ++i)
            {
                Run run = runs_[bidiOrdering[i]];
                int glyphStart = run.glyphStart;
                int glyphEnd = glyphStart + run.glyphCount;

                // If the run is only partially covered, we'll need to find
                // the subsection of glyphs that were fit.
                if (clusterStart.textPosition > run.textStart)
                {
                    glyphStart = GetClusterGlyphStart(ref clusterStart);
                }
                if (clusterWsEnd.textPosition < run.textStart + run.textLength)
                {
                    glyphEnd = GetClusterGlyphStart(ref clusterWsEnd);
                }
                if ((glyphStart >= glyphEnd) || (run.script.Shapes == ScriptShapes.NoVisual))
                {
                    // The itemizer told us not to draw this character,
                    // either because it was a formatting, control, or other hidden character.
                    continue;
                }

                // The run width is needed to know how far to move forward,
                // and to flip the origin for right-to-left text.
                float runWidth = GetClusterRangeWidth(
                                    glyphStart - justificationGlyphStart,
                                    glyphEnd - justificationGlyphStart,
                                    justifiedAdvances.ToArray()
                                    );

                // Flush this glyph run.
                //int glyphCount = glyphEnd - glyphStart;
                int glyphCount = justifiedAdvances.Count;
                short[] call_glyphIndices_ = new short[glyphCount];
                Array.Copy(glyphIndices_, glyphStart, call_glyphIndices_, 0, call_glyphIndices_.Length);
                float[] call_justifiedAdvances = new float[justifiedAdvances.Count - (glyphStart - justificationGlyphStart)];
                justifiedAdvances.CopyTo(glyphStart - justificationGlyphStart, call_justifiedAdvances, 0, call_justifiedAdvances.Length);
                GlyphOffset[] call_glyphOffsets_ = new GlyphOffset[glyphCount];
                Array.Copy(glyphOffsets_, glyphStart, call_glyphOffsets_, 0, call_glyphOffsets_.Length);
                flowSink.SetGlyphRun(
                    (run.bidiLevel % 2 != 0) ? (x + runWidth) : (x), // origin starts from right if RTL
                    y,
                    glyphCount,
                    call_glyphIndices_,
                    call_justifiedAdvances,
                    call_glyphOffsets_,
                    fontFace_,
                    fontEmSize_,
                    run.bidiLevel,
                    run.isSideways
                    );

                x += runWidth;
            }
            return true;
        }
        protected void ProduceBidiOrdering(int spanStart, int spanCount, int[] spanIndices)
        {
            // Produces an index mapping from sequential order to visual bidi order.
            // The function progresses forward, checking the bidi level of each
            // pair of spans, reversing when needed.
            //
            // See the Unicode technical report 9 for an explanation.
            // http://www.unicode.org/reports/tr9/tr9-17.html 

            // Fill all entries with initial indices
            for (int i = 0; i < spanCount; ++i)
            {
                spanIndices[i] = spanStart + i;
            }
            
            if (spanCount <= 1)
                return;

            int runStart = 0;
            int currentLevel = runs_[spanStart].bidiLevel;

            // Rearrange each run to produced reordered spans.
            for (int i = 0; i < spanCount; ++i)
            {
                int runEnd = i + 1;
                int nextLevel = (runEnd < spanCount)
                                    ? runs_[spanIndices[runEnd]].bidiLevel
                                    : 0; // past last element

                // We only care about transitions, particularly high to low,
                // because that means we have a run behind us where we can
                // do something.

                if (currentLevel <= nextLevel) // This is now the beginning of the next run.
                {
                    if (currentLevel < nextLevel)
                    {
                        currentLevel = nextLevel;
                        runStart = i + 1;
                    }
                    continue; // Skip past equal levels, or increasing stairsteps.
                }

                do // currentLevel > nextLevel
                {
                    // Recede to find start of the run and previous level.
                    int previousLevel;
                    while (true)
                    {
                        if (runStart <= 0) // reached front of index list
                        {
                            previousLevel = 0; // position before string has bidi level of 0
                            break;
                        }
                        if (runs_[spanIndices[--runStart]].bidiLevel < currentLevel)
                        {
                            previousLevel = runs_[spanIndices[runStart]].bidiLevel;
                            ++runStart; // compensate for going one element past
                            break;
                        }
                    }

                    // Reverse the indices, if the difference between the current and
                    // next/previous levels is odd. Otherwise, it would be a no-op, so
                    // don't bother.
                    if ((Math.Min(currentLevel - nextLevel, currentLevel - previousLevel) & 1) != 0)
                    {
                        Array.Reverse(spanIndices, runStart, runEnd - runStart);
                    }

                    // Descend to the next lower level, the greater of previous and next
                    currentLevel = Math.Max(previousLevel, nextLevel);
                }
                while (currentLevel > nextLevel); // Continue until completely flattened.
            }
        }
        protected void ProduceJustifiedAdvances(ref RectangleF rect, ref ClusterPosition clusterStart, ref ClusterPosition clusterEnd, out List<float> justifiedAdvances)
        {
            // Performs simple inter-word justification
            // using the breakpoint analysis whitespace property.

            // Copy out default, unjustified advances.
            int glyphStart = GetClusterGlyphStart(ref clusterStart);
            int glyphEnd = GetClusterGlyphStart(ref clusterEnd);

            justifiedAdvances = new List<float>(glyphEnd - glyphStart + 1);
            for (int i = glyphStart; i < glyphEnd; i++)
                justifiedAdvances.Add(glyphAdvances_[i]);

            if (glyphEnd - glyphStart == 0)
                return; // No glyphs to modify.

            float maxWidth = rect.Right - rect.Left;
            if (maxWidth <= 0)
                return; // Text can't fit anyway.

            ////////////////////////////////////////
            // First, count how many spaces there are in the text range.

            ClusterPosition cluster = clusterStart;
            int whitespaceCount = 0;

            while (cluster.textPosition < clusterEnd.textPosition)
            {
                if (breakpoints_[cluster.textPosition].IsWhitespace)
                    ++whitespaceCount;
                AdvanceClusterPosition(ref cluster);
            }
            if (whitespaceCount <= 0)
                return; // Can't justify using spaces, since none exist.

            ////////////////////////////////////////
            // Second, determine the needed contribution to each space.

            float lineWidth = GetClusterRangeWidth(glyphStart, glyphEnd, glyphAdvances_);
            float justificationPerSpace = (maxWidth - lineWidth) / whitespaceCount;

            if (justificationPerSpace <= 0)
                return; // Either already justified or would be negative justification.

            if (justificationPerSpace > maxSpaceWidth_)
                return; // Avoid justification if it would space the line out awkwardly far.

            ////////////////////////////////////////
            // Lastly, adjust the advance widths, adding the difference to each space character.

            cluster = clusterStart;
            while (cluster.textPosition < clusterEnd.textPosition)
            {
                if (breakpoints_[cluster.textPosition].IsWhitespace)
                    justifiedAdvances[GetClusterGlyphStart(ref cluster) - glyphStart] += justificationPerSpace;

                AdvanceClusterPosition(ref cluster);
            }
        }

        // Text/cluster navigation.

        protected void SetClusterPosition(ref ClusterPosition cluster, int textPosition)
        {
            // Since layout should never split text clusters, we want to move ahead whole
            // clusters at a time.

            // Updates the current position and seeks its matching text analysis run.
            cluster.textPosition = textPosition;

            // If the new text position is outside the previous analysis run,
            // find the right one.

            if (textPosition >= cluster.runEndPosition || !runs_[cluster.runIndex].ContainsTextPosition(textPosition))
            {
                // If we can resume the search from the previous run index,
                // (meaning the new text position comes after the previously
                // seeked one), we can save some time. Otherwise restart from
                // the beginning.
                int newRunIndex = 0;
                if (textPosition >= runs_[cluster.runIndex].textStart)
                {
                    newRunIndex = cluster.runIndex;
                }

                // Find new run that contains the text position.
                for (int i = 0; i < runs_.Length; i++)
                    if (runs_[i].ContainsTextPosition(textPosition))
                    {
                        newRunIndex = i;
                        break;
                    }

                // Keep run index within the list, rather than pointing off the end.
                if (newRunIndex >= runs_.Length)
                {
                    newRunIndex = runs_.Length - 1;
                }

                // Cache the position of the next analysis run to efficiently
                // move forward in the clustermap.
                cluster.runIndex = newRunIndex;
                cluster.runEndPosition = runs_[newRunIndex].textStart + runs_[newRunIndex].textLength;
            }
        }
        protected void AdvanceClusterPosition(ref ClusterPosition cluster)
        {
            // Looks forward in the cluster map until finding a new cluster,
            // or until we reach the end of a cluster run returned by shaping.
            //
            // Glyph shaping can produce a clustermap where a:
            //  - A single codepoint maps to a single glyph (simple Latin and precomposed CJK)
            //  - A single codepoint to several glyphs (diacritics decomposed into distinct glyphs)
            //  - Multiple codepoints are coalesced into a single glyph.
            //
            int textPosition = cluster.textPosition;
            int clusterId = glyphClusters_[textPosition];

            for (++textPosition; textPosition < cluster.runEndPosition; ++textPosition)
            {
                if (glyphClusters_[textPosition] != clusterId)
                {
                    // Now pointing to the next cluster.
                    cluster.textPosition = textPosition;
                    return;
                }
            }
            if (textPosition == cluster.runEndPosition)
            {
                // We crossed a text analysis run.
                SetClusterPosition(ref cluster, textPosition);
            }
        }
        protected int GetClusterGlyphStart(ref ClusterPosition cluster)
        {
            // Maps from text position to corresponding starting index in the glyph array.
            // This is needed because there isn't a 1:1 correspondence between text and
            // glyphs produced.

            int glyphStart = runs_[cluster.runIndex].glyphStart;

            return (cluster.textPosition < glyphClusters_.Length)
                ? glyphStart + glyphClusters_[cluster.textPosition]
                : glyphStart + runs_[cluster.runIndex].glyphCount;
        }
        protected float GetClusterRangeWidth(ref ClusterPosition clusterStart, ref  ClusterPosition clusterEnd)
        {
            // Sums the glyph advances between two cluster positions,
            // useful for determining how long a line or word is.
            return GetClusterRangeWidth(GetClusterGlyphStart(ref clusterStart), GetClusterGlyphStart(ref clusterEnd), glyphAdvances_);
        }
        protected float GetClusterRangeWidth(int glyphStart, int glyphEnd, float[] glyphAdvances)
        {
            // Sums the glyph advances between two glyph offsets, given an explicit
            // advances array - useful for determining how long a line or word is.
            float width = 0;
            for (int i = glyphStart; i < glyphEnd; i++)
                width += glyphAdvances[i];
            return width;
        }
    }

    internal struct ClusterPosition
    {
        public int textPosition;    // Current text position
        public int runIndex;        // Associated analysis run covering this position
        public int runEndPosition;  // Text position where this run ends
    }
}
