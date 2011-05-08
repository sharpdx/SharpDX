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
using System.Windows.Forms;
using SharpDX.Direct2D1;
using SharpDX;
using SharpDX.DirectWrite;

namespace CustomLayout
{
    /// <summary>
    /// Demonstrates how to use a custom application-implemented text layout object to display text in a non-rectangular region. 
    /// This sample renders DirectWrite text in two different shapes: circular and funnel.
    ///  
    /// SharpDX Port of DirectWrite Custom Layout Example from Windows 7 SDK samples. 
    /// Thanks to Fadi Alsamman for this port.
    /// http://msdn.microsoft.com/en-us/library/dd941711%28VS.85%29.aspx
    /// </summary>
    public partial class CustomLayoutForm : Form
    {
        public SharpDX.Direct2D1.Factory Factory2D { get; private set; }
        public SharpDX.DirectWrite.Factory FactoryDWrite { get; private set; }
        public WindowRenderTarget RenderTarget2D { get; private set; }
        public SolidColorBrush brush;

        private FlowLayoutSource flowLayoutSource;
        private FlowLayoutSink flowLayoutSink;
        private FlowLayout flowLayout;
        private LayoutText textMode_;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomLayoutForm"/> class.
        /// </summary>
        public CustomLayoutForm()
        {
            InitializeComponent();

            try
            {
                InitDirect2DAndDirectWrite();
            }
            catch (Exception ex)
            {
                LogException(ex);
                Environment.Exit(1);
            }

            flowLayoutSource = new FlowLayoutSource();
            flowLayoutSink = new FlowLayoutSink();
            flowLayout = new FlowLayout(FactoryDWrite);

            SetLayoutText(LayoutText.Latin);
            SetLayoutShape(FlowShape.Circle);

            panel1.Paint += RenderControlPaint;
            panel1.Resize += RenderControlResize;
        }

        protected override void OnCreateControl()
        {
            RenderControlResize(null, null);
            base.OnCreateControl();
        }

        /// <summary>
        /// Inits the direct2D and direct write.
        /// </summary>
        private void InitDirect2DAndDirectWrite()
        {
            Factory2D = new SharpDX.Direct2D1.Factory();
            FactoryDWrite = new SharpDX.DirectWrite.Factory();

            var properties = new HwndRenderTargetProperties { Hwnd = panel1.Handle, PixelSize = panel1.ClientSize, PresentOptions = PresentOptions.None };

            RenderTarget2D = new WindowRenderTarget(Factory2D, new RenderTargetProperties(), properties)
            {
                AntialiasMode = AntialiasMode.PerPrimitive,
                TextAntialiasMode = TextAntialiasMode.Cleartype
            };

            brush = new SolidColorBrush(RenderTarget2D, new Color4(1, 0, 0, 0));
        }

        private void LogException(Exception ex)
        {
            //MessageBox.Show(this, string.Format("Unable to use the font {0}. Reason : {1}", FontFamilyName, ex), "Error while setting text layout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Handles the Resize event of the renderControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void RenderControlResize(object sender, System.EventArgs e)
        {
            try
            {
                RenderTarget2D.Resize(panel1.ClientRectangle.Size);
                flowLayoutSource.SetSize(panel1.ClientSize.Width, panel1.ClientSize.Height);
                ReflowLayout();
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        /// <summary>
        /// Handles the Paint event of the renderControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
        void RenderControlPaint(object sender, PaintEventArgs e)
        {
            try
            {
                RenderTarget2D.BeginDraw();

                RenderTarget2D.Clear(new Color4(1, 1, 1, 1));

                flowLayoutSink.DrawGlyphRuns(RenderTarget2D, brush);


                RenderTarget2D.EndDraw();
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }


        private void ReflowLayout()
        {
            flowLayoutSource.Reset();
            flowLayoutSink.Reset();
            flowLayout.FlowText(flowLayoutSource, flowLayoutSink);
        }
        private void SetLayoutText(LayoutText layoutText)
        {
            // Selects a different text sample.

            ReadingDirection readingDirection = ReadingDirection.LeftToRight;
            String text = "";
            String fontName = "";
            String localeName = "";

            switch (layoutText)
            {
                case LayoutText.Latin:
                    fontName = "Segoe UI";
                    localeName = "en-us";
                    readingDirection = ReadingDirection.LeftToRight;
                    text =
                          "DirectWrite provides factored layers of functionality, with each layer interacting seamlessly with the next. "
                        + "The API design gives an application the freedom and flexibility to adopt individual layers depending on their needs and schedule.\n"
                        + "\n"
                        + "The text layout API provides the highest level functionality available from DirectWrite. "
                        + "It provides services for the application to measure, display, and interact with richly formatted text strings. "
                        + "This text API can be used in applications that currently use Win32 DrawText to build a modern UI with richly formatted text.\n"
                        + "\n"
                        + "* Text-intensive applications that implement their own layout engine may use the next layer down: the script processor. "
                        + "The script processor segments text into runs of similar properties and handles the mapping from Unicode codepoints "
                        + "to the appropriate glyph in the font. "
                        + "DirectWrite's own layout is built upon this same font and script processing system. "
                        + "This sample demonstrates how a custom layout can utilize the information from script itemization, bidi analysis, line breaking analysis, and shaping, "
                        + "to accomplish text measurement/fitting, line breaking, basic justification, and drawing.\n"
                        + "\n"
                        + "The glyph-rendering layer is the lowest layer and provides glyph-rendering functionality for applications "
                        + "that implement their own complete text layout engine. The glyph rendering layer is also useful for applications that implement a custom "
                        + "renderer to modify the glyph-drawing behavior through the callback function in the DirectWrite text-formatting API.\n"
                        + "\n"
                        + "The DirectWrite font system is available to all the functional layers, and enables an application to access font and glyph information. "
                        + "It is designed to handle common font technologies and data formats. The DirectWrite font model follows the common typographic practice of "
                        + "supporting any number of weights, styles, and stretches in the same font family. This model, the same model followed by WPF and CSS, "
                        + "specifies that fonts differing only in weight (bold, light, etc.), style (upright, italic, or oblique) or stretch (narrow, condensed, wide, etc.) "
                        + "are considered to be members of a single font family.\n"
                        + "\n"
                        + "Text in DirectWrite is rendered using Microsoft ClearType, which enhances the clarity and readability of text. "
                        + "ClearType takes advantage of the fact that modern LCD displays have RGB stripes for each pixel that can be controlled individually. "
                        + "DirectWrite uses the latest enhancements to ClearType, first included with Windows Vista with Windows Presentation Foundation, "
                        + "that enables it to evaluate not just the individual letters but also the spacing between letters. "
                        + "Before these ClearType enhancements, text with a “reading” size of 10 or 12 points was difficult to display: "
                        + "we could place either 1 pixel in between letters, which was often too little, or 2 pixels, which was often too much. "
                        + "Using the extra resolution in the subpixels provides us with fractional spacing, which improves the evenness and symmetry of the entire page.\n"
                        + "\n"
                        + "The subpixel ClearType positioning offers the most accurate spacing of characters on screen, "
                        + "especially at small sizes where the difference between a sub-pixel and a whole pixel represents a significant proportion of glyph width. "
                        + "It allows text to be measured in ideal resolution space and rendered at its natural position at the LCD color stripe, subpixel granularity. "
                        + "Text measured and rendered using this technology is, by definition, "
                        + "resolution-independent—meaning the exact same layout of text is achieved across the range of various display resolutions.\n"
                        + "\n"
                        + "Unlike either flavor of GDI's ClearType rendering, sub-pixel ClearType offers the most accurate width of characters. "
                        + "The Text String API adopts sub-pixel text rendering by default, which means it measures text at its ideal resolution independent "
                        + "to the current display resolution, and produces the glyph positioning result based on the truly scaled glyph advance widths and positioning offsets.";
                    break;
                case LayoutText.Arabic:
                    fontName = "Arabic Typesetting";
                    localeName = "ar-eg";
                    readingDirection = ReadingDirection.RightToLeft;
                    text =
                        "الديباجة\n"
                        + "لمّا كان الاعتراف بالكرامة المتأصلة في جميع أعضاء الأسرة البشرية وبحقوقهم المتساوية الثابتة هو أساس الحرية والعدل والسلام في العالم.\n"
                        + "\n"
                        + "ولما كان تناسي حقوق الإنسان وازدراؤها قد أفضيا إلى أعمال همجية آذت الضمير الإنساني. وكان غاية ما يرنو إليه عامة البشر انبثاق عالم يتمتع فيه الفرد بحرية القول والعقيدة ويتحرر من الفزع والفاقة.\n"
                        + "\n"
                        + "ولما كان من الضروري أن يتولى القانون حماية حقوق الإنسان لكيلا يضطر المرء آخر الأمر إلى التمرد على الاستبداد والظلم.\n"
                        + "\n"
                        + "ولما كانت شعوب الأمم المتحدة قد أكدت في الميثاق من جديد إيمانها بحقوق الإنسان الأساسية وبكرامة الفرد وقدره وبما للرجال والنساء من حقوق متساوية وحزمت أمرها على أن تدفع بالرقي الاجتماعي قدمًا وأن ترفع مستوى الحياة في جو من الحرية أفسح.\n"
                        + "\n"
                        + "ولما كانت الدول الأعضاء قد تعهدت بالتعاون مع الأمم المتحدة على ضمان إطراد مراعاة حقوق الإنسان والحريات الأساسية واحترامها.\n"
                        + "\n"
                        + "ولما كان للإدراك العام لهذه الحقوق والحريات الأهمية الكبرى للوفاء التام بهذا التعهد.\n"
                        + "\n"
                        + "فإن الجمعية العامة\n"
                        + "\n"
                        + "تنادي بهذا الإعلان العالمي لحقوق الإنسان\n"
                        + "\n"
                        + "على أنه المستوى المشترك الذي ينبغي أن تستهدفه كافة الشعوب والأمم حتى يسعى كل فرد وهيئة في المجتمع، واضعين على الدوام هذا الإعلان نصب أعينهم، إلى توطيد احترام هذه الحقوق والحريات عن طريق التعليم والتربية واتخاذ إجراءات مطردة، قومية وعالمية، لضمان الإعتراف بها ومراعاتها بصورة عالمية فعالة بين الدول الأعضاء ذاتها وشعوب البقاع الخاضعة لسلطانها.\n"
                        + "\n"
                        + "المادة 1\n"
                        + "\n"
                        + "يولد جميع الناس أحرارًا متساوين في الكرامة والحقوق. وقد وهبوا عقلاً وضميرًا وعليهم أن يعامل بعضهم بعضًا بروح الإخاء.\n"
                        + "\n"
                        + "المادة 2\n"
                        + "\n"
                        + "لكل إنسان حق التمتع بكافة الحقوق والحريات الواردة في هذا الإعلان، دون أي تمييز، كالتمييز بسبب العنصر أو اللون أو الجنس أو اللغة أو الدين أو الرأي السياسي أو أي رأي آخر، أو الأصل الوطني أو الإجتماعي أو الثروة أو الميلاد أو أي وضع آخر، دون أية تفرقة بين الرجال والنساء.\n"
                        + "\n"
                        + "وفضلاً عما تقدم فلن يكون هناك أي تمييز أساسه الوضع السياسي أو القانوني أو الدولي لبلد أو البقعة التي ينتمي إليها الفرد سواء كان هذا البلد أو تلك البقعة مستقلاً أو تحت الوصاية أو غير متمتع بالحكم الذاتي أو كانت سيادته خاضعة لأي قيد من القيود.\n";
                    break;
                case LayoutText.Japanese:
                    fontName = "Meiryo";
                    localeName = "jp-jp";
                    readingDirection = ReadingDirection.LeftToRight;
                    text =
                        "『世界人権宣言』\n"
                        + "（1948.12.10 第３回国連総会採択）〈前文〉\n"
                        + "\n"
                        + "人類社会のすべての構成員の固有の尊厳と平等で譲ることのできない権利とを承認することは、世界における自由、正義及び平和の基礎であるので、\n"
                        + "\n"
                        + "人権の無視及び軽侮が、人類の良心を踏みにじった野蛮行為をもたらし、言論及び信仰の自由が受けられ、恐怖及び欠乏のない世界の到来が、一般の人々の最高の願望として宣言されたので、\n"
                        + "\n"
                        + "人間が専制と圧迫とに対する最後の手段として反逆に訴えることがないようにするためには、法の支配によって人権を保護することが肝要であるので、\n"
                        + "\n"
                        + "諸国間の友好関係の発展を促進することが肝要であるので、\n"
                        + "\n"
                        + "国際連合の諸国民は、国連憲章において、基本的人権、人間の尊厳及び価値並びに男女の同権についての信念を再確認し、かつ、一層大きな自由のうちで社会的進歩と生活水準の向上とを促進することを決意したので、\n"
                        + "\n"
                        + "加盟国は、国際連合と協力して、人権及び基本的自由の普遍的な尊重及び遵守の促進を達成することを誓約したので、\n"
                        + "\n"
                        + "これらの権利及び自由に対する共通の理解は、この誓約を完全にするためにもっとも重要であるので、\n"
                        + "\n"
                        + "よって、ここに、国連総会は、\n"
                        + "\n"
                        + "\n"
                        + "社会の各個人及び各機関が、この世界人権宣言を常に念頭に置きながら、加盟国自身の人民の間にも、また、加盟国の管轄下にある地域の人民の間にも、これらの権利と自由との尊重を指導及び教育によって促進すること並びにそれらの普遍的措置によって確保することに努力するように、すべての人民とすべての国とが達成すべき共通の基準として、この人権宣言を公布する。\n"
                        + "\n"
                        + "第１条\n"
                        + "すべての人間は、生まれながらにして自由であり、かつ、尊厳と権利と について平等である。人間は、理性と良心とを授けられており、互いに同 胞の精神をもって行動しなければならない。\n"
                        + "\n"
                        + "第２条"
                        + "すべて人は、人種、皮膚の色、性、言語、宗教、政治上その他の意見、\n"
                        + "\n"
                        + "国民的もしくは社会的出身、財産、門地その他の地位又はこれに類するい\n"
                        + "\n"
                        + "かなる自由による差別をも受けることなく、この宣言に掲げるすべての権\n"
                        + "\n"
                        + "利と自由とを享有することができる。\n"
                        + "\n"
                        + "さらに、個人の属する国又は地域が独立国であると、信託統治地域で\n"
                        + "\n"
                        + "あると、非自治地域であると、又は他のなんらかの主権制限の下にあると\n"
                        + "\n"
                        + "を問わず、その国又は地域の政治上、管轄上又は国際上の地位に基ずくい\n"
                        + "\n"
                        + "かなる差別もしてはならない。\n"
                        + "\n"
                        + "第３条\n"
                        + "すべての人は、生命、自由及び身体の安全に対する権利を有する。\n"
                        + "\n"
                        + "第４条\n"
                        + "何人も、奴隷にされ、又は苦役に服することはない。奴隷制度及び奴隷\n"
                        + "\n"
                        + "売買は、いかなる形においても禁止する。\n"
                        + "\n"
                        + "第５条\n"
                        + "何人も、拷問又は残虐な、非人道的なもしくは屈辱的な取扱もしくは刑\n"
                        + "\n"
                        + "罰を受けることはない。\n";

                    break;
            }
            textMode_ = layoutText;

            TextFormat textFormat = null;
            try
            {
                textFormat = new TextFormat(FactoryDWrite, fontName, null, FontWeight.Normal, SharpDX.DirectWrite.FontStyle.Normal, FontStretch.Normal, 14, localeName);
                textFormat.ReadingDirection = readingDirection;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not create text format for custom layout! CreateTextFormat()" + Environment.NewLine + ex.Message, "CutomLayout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                flowLayout.SetTextFormat(textFormat);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not set text format on custom layout! SetTextFormat()" + Environment.NewLine + ex.Message, "CutomLayout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                flowLayout.AnalyzeText(text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Text analysis failed! FlowLayout::AnalyzeText()" + Environment.NewLine + ex.Message, "CutomLayout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SetLayoutShape(FlowShape flowShape)
        {
            flowLayoutSource.FlowShape = flowShape;
            flowLayoutSource.Reset();
        }
        private void SetLayoutNumbers(LayoutNumbers layoutNumbers)
        {
            // Creates a number substitution to select which digits are displayed.
            String localName = "en-us";
            if (layoutNumbers == LayoutNumbers.Arabic)
                localName = "ar-eg";
            NumberSubstitution numberSubstitution = new NumberSubstitution(FactoryDWrite, NumberSubstitutionMethod.Contextual, localName, true);
            flowLayout.SetNumberSubstitution(numberSubstitution);

            SetLayoutText(textMode_);
            numberSubstitution.Dispose();
        }

        private void miLatinText_Click(object sender, EventArgs e)
        {
            SetLayoutText(LayoutText.Latin);
            ReflowLayout();
            panel1.Invalidate();
        }

        private void miArabicText_Click(object sender, EventArgs e)
        {
            SetLayoutText(LayoutText.Arabic);
            ReflowLayout();
            panel1.Invalidate();
        }

        private void miJapaneesText_Click(object sender, EventArgs e)
        {
            SetLayoutText(LayoutText.Japanese);
            ReflowLayout();
            panel1.Invalidate();
        }

        private void miCircleShape_Click(object sender, EventArgs e)
        {
            SetLayoutShape(FlowShape.Circle);
            ReflowLayout();
            panel1.Invalidate();
        }

        private void miFunnelShape_Click(object sender, EventArgs e)
        {
            SetLayoutShape(FlowShape.Funnel);
            ReflowLayout();
            panel1.Invalidate();
        }

        private void miNominalNumbers_Click(object sender, EventArgs e)
        {
            SetLayoutNumbers(LayoutNumbers.Nominal);
            ReflowLayout();
            panel1.Invalidate();
        }

        private void miArabicContextual_Click(object sender, EventArgs e)
        {
            SetLayoutNumbers(LayoutNumbers.Arabic);
            ReflowLayout();
            panel1.Invalidate();
        }

        private void miExit_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}

public enum LayoutText { Latin, Arabic, Japanese };
public enum LayoutNumbers { Nominal, Arabic };
