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
    /// Helper source/sink class for text analysis.
    /// </summary>
    internal class TextAnalysis : TextAnalysisSource, TextAnalysisSink
    {
        protected String text_;
        protected String localeName_;
        protected NumberSubstitution numberSubstitution_;
        protected ReadingDirection readingDirection_;

        // Current processing state
        protected int currentPosition_;
        protected int currentRunIndex_;

        //output
        protected List<LinkedRun> runs_;
        protected List<LineBreakpoint> breakpoints_;

        public TextAnalysis(String Text, String localName, ReadingDirection readingDirection, NumberSubstitution numberSubstitution)
        {
            text_ = Text;
            localeName_ = localName;
            readingDirection_ = readingDirection;
            numberSubstitution_ = numberSubstitution;
        }

        /// <summary>
        /// Analyzes the text using each of the analyzers and returns 
        /// their results as a series of runs.
        /// </summary>
        public void GenerateResults(TextAnalyzer textAnalyzer, out Run[] runs, out LineBreakpoint[] breakpoints)
        {
            // Initially start out with one result that covers the entire range.
            // This result will be subdivided by the analysis processes.
            LinkedRun initialRun = new LinkedRun()
            {
                nextRunIndex = 0,
                textStart = 0,
                textLength = text_.Length,
                bidiLevel = (readingDirection_ == ReadingDirection.RightToLeft) ? 1 : 0
            };
            runs_ = new List<LinkedRun>();
            runs_.Add(initialRun);

            breakpoints_ = new List<LineBreakpoint>();

             //Call each of the analyzers in sequence, recording their results.
            if ((textAnalyzer.AnalyzeLineBreakpoints(this, 0, text_.Length, this) == Result.Ok) &&
                (textAnalyzer.AnalyzeBidi(this, 0, text_.Length, this) == Result.Ok) &&
                (textAnalyzer.AnalyzeScript(this, 0, text_.Length, this) == Result.Ok) &&
                (textAnalyzer.AnalyzeNumberSubstitution(this, 0, text_.Length, this) == Result.Ok))
            {

                breakpoints = new LineBreakpoint[breakpoints_.Count];
                breakpoints_.CopyTo(breakpoints);

                // Resequence the resulting runs in order before returning to caller.
                runs = new Run[runs_.Count];
                int nextRunIndex = 0;
                for (int i = 0; i < runs_.Count; i++)
                {
                    runs[i] = runs_[nextRunIndex].AsRun;
                    nextRunIndex = runs_[nextRunIndex].nextRunIndex;
                }
            }
            else
            {
                runs = new Run[0];
                breakpoints = new LineBreakpoint[0];
            }
        }


        #region TextAnalysisSource Members

        public string GetTextAtPosition(int textPosition)
        {
            if (textPosition > text_.Length)
            {
                // Return no text if a query comes for a position at the end of
                // the range. Note that is not an error and we should not return
                // a failing HRESULT. Although the application is not expected
                // to return any text that is outside of the given range, the
                // analyzers may probe the ends to see if text exists.
                return null;
            }
            else
            {
                return text_.Substring(textPosition, text_.Length - textPosition);
            }
        }
        public string GetTextBeforePosition(int textPosition)
        {
            if (textPosition == 0 || textPosition > text_.Length)
            {
                // Return no text if a query comes for a position at the end of
                // the range. Note that is not an error and we should not return
                // a failing HRESULT. Although the application is not expected
                // to return any text that is outside of the given range, the
                // analyzers may probe the ends to see if text exists.
                return null;
            }
            else
            {
                // text length is valid from current position backward
                return text_.Substring(0, textPosition);
            }
        }
        public ReadingDirection ReadingDirection
        {
            get
            {
                return readingDirection_;
            }
        }
        public string GetLocaleName(int textPosition)
        {
            // The pointer returned should remain valid until the next call,
            // or until analysis ends. Since only one locale name is supported,
            // the text length is valid from the current position forward to
            // the end of the string.
            return localeName_;
        }
        public NumberSubstitution GetNumberSubstitution(int textPosition, out int textLength)
        {
            textLength = text_.Length - textPosition;
            return numberSubstitution_;
        }

        #endregion

        #region TextAnalysisSink Members

        public void SetLineBreakpoints(int textPosition, int textLength, LineBreakpoint[] lineBreakpoints)
        {
            if (textLength > 0)
            {
                breakpoints_.AddRange(lineBreakpoints);
            }
        }
        public void SetScriptAnalysis(int textPosition, int textLength, ScriptAnalysis scriptAnalysis)
        {
            SetCurrentRun(textPosition);
            SplitCurrentRun(textPosition);
            while (textLength > 0)
            {
                int nextRunIndex = FetchNextRunIndex(ref textLength);
                LinkedRun run = runs_[nextRunIndex];
                run.script = scriptAnalysis;
                runs_[nextRunIndex] = run;
            }
        }
        public void SetBidiLevel(int textPosition, int textLength, byte explicitLevel, byte resolvedLevel)
        {
            SetCurrentRun(textPosition);
            SplitCurrentRun(textPosition);
            while (textLength > 0)
            {
                int nextRunIndex = FetchNextRunIndex(ref textLength);
                LinkedRun run = runs_[nextRunIndex];
                run.bidiLevel = resolvedLevel;
                runs_[nextRunIndex] = run;
            }
        }
        public void SetNumberSubstitution(int textPosition, int textLength, NumberSubstitution numberSubstitution)
        {
            SetCurrentRun(textPosition);
            SplitCurrentRun(textPosition);
            while (textLength > 0)
            {
                int nextRunIndex = FetchNextRunIndex(ref textLength);
                LinkedRun run = runs_[nextRunIndex];
                run.isNumberSubstituted = (numberSubstitution != null);
                runs_[nextRunIndex] = run;
            }
        }

        #endregion

        #region Run modification.
        private int FetchNextRunIndex(ref int textLength)
        {
            // Used by the sink setters, this returns a reference to the next run.
            // Position and length are adjusted to now point after the current run
            // being returned.

            int runIndex = currentRunIndex_;
            int runTextLength = runs_[currentRunIndex_].textLength;

            // Split the tail if needed (the length remaining is less than the
            // current run's size).
            if (textLength < runTextLength)
            {
                runTextLength = textLength;// Limit to what's actually left.
                int runTextStart = runs_[currentRunIndex_].textStart;

                SplitCurrentRun(runTextStart + runTextLength);
            }
            else
            {
                // Just advance the current run.
                currentRunIndex_ = runs_[currentRunIndex_].nextRunIndex;
            }
            textLength -= runTextLength;
            return runIndex;
        }
        private void SetCurrentRun(int textPosition)
        {
            // Move the current run to the given position.
            // Since the analyzers generally return results in a forward manner,
            // this will usually just return early. If not, find the
            // corresponding run for the text position.

            if (currentRunIndex_ < runs_.Count && runs_[currentRunIndex_].ContainsTextPosition(textPosition))
            {
                return;
            }

            for (int i = 0; i < runs_.Count; i++)
                if (runs_[i].ContainsTextPosition(textPosition))
                {
                    currentRunIndex_ = i;
                    break;
                }
        }
        private void SplitCurrentRun(int splitPosition)
        {
            // Splits the current run and adjusts the run values accordingly.
            int runTextStart = runs_[currentRunIndex_].textStart;

            if (splitPosition <= runTextStart)
                return; // no change

            LinkedRun frontHalf = runs_[currentRunIndex_];
            LinkedRun backHalf = frontHalf;

            // Adjust runs' text positions and lengths.
            int splitPoint = splitPosition - runTextStart;
            backHalf.textStart += splitPoint;
            backHalf.textLength -= splitPoint;
            runs_.Add(backHalf);

            frontHalf.textLength = splitPoint;
            frontHalf.nextRunIndex = runs_.Count - 1;
            runs_[currentRunIndex_] = frontHalf;

            currentRunIndex_ = runs_.Count - 1;
        }

        #endregion
    }

    /// <summary>
    /// A single contiguous run of characters containing the same analysis results.
    /// </summary>
    internal struct Run
    {
        public int textStart;   // starting text position of this run
        public int textLength;  // number of contiguous code units covered
        public int glyphStart;  // starting glyph in the glyphs array
        public int glyphCount;  // number of glyphs associated with this run of text
        public ScriptAnalysis script;
        public int bidiLevel;
        public bool isNumberSubstituted;
        public bool isSideways;

        public bool ContainsTextPosition(int desiredTextPosition)
        {
            return desiredTextPosition >= textStart && desiredTextPosition < textStart + textLength;
        }
        public override string ToString()
        {
            return String.Format("Text:{0} - {1} , Glyph:{2} - {3}", new object[] { textStart, textLength, glyphStart, glyphCount });
        }
    }

    internal struct LinkedRun
    {
        public int textStart;   // starting text position of this run
        public int textLength;  // number of contiguous code units covered
        public int glyphStart;  // starting glyph in the glyphs array
        public int glyphCount;  // number of glyphs associated with this run of text
        public ScriptAnalysis script;
        public int bidiLevel;
        public bool isNumberSubstituted;
        public bool isSideways;
        public int nextRunIndex;

        public bool ContainsTextPosition(int desiredTextPosition)
        {
            return desiredTextPosition >= textStart && desiredTextPosition < textStart + textLength;
        }

        public Run AsRun
        {
            get
            {
                return new Run()
                {
                    textStart = this.textStart,
                    textLength = this.textLength,
                    glyphStart = this.glyphStart,
                    glyphCount = this.glyphCount,
                    script = this.script,
                    bidiLevel = this.bidiLevel,
                    isNumberSubstituted = this.isNumberSubstituted,
                    isSideways = this.isSideways
                };
            }
        }
        public override string ToString()
        {
            return String.Format("Text:{0} - {1} , Glyph:{2} - {3}", new object[] { textStart, textLength, glyphStart, glyphCount });
        }
    }
}
