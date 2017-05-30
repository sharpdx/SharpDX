using System.Diagnostics;
using System.Text;
using SharpDX.Toolkit.Graphics;

namespace SharpDX.Toolkit.Diagnostics
{
    /// <summary>
    /// A class used to draw debugging information to the screen.
    /// </summary>
    public class TextConsole : Component
    {
        private SpriteFont font;                    // used for drawing the messages
        private SpriteBatch spriteBatch;            // used for rendering
        private readonly StringBuilder textBuffer;  // used to store the messages
        private LogMessageType currentState;        // the current logging state
        private Vector2 cursor;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextConsole"/> class.
        /// </summary>
        /// <param name="Font">The font used for rendering.</param>
        public TextConsole(SpriteFont Font) {
            font = Font;
            spriteBatch = new SpriteBatch(font.GraphicsDevice);
            textBuffer = new StringBuilder(64);
            Location = Vector2.Zero;

            InfoForegroundColor    = Color.White;
            WarningForegroundColor = Color.Yellow;
            ErrorForegroundColor   = Color.Red;
        }


        /// <summary>
        /// The font used for rendering.
        /// </summary>
        public SpriteFont Font {
            get {
                return font;
            }
            set {
                if (font.GraphicsDevice != value.GraphicsDevice) {
                    spriteBatch.Dispose();
                    spriteBatch = new SpriteBatch(font.GraphicsDevice);
                }
                font = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the location
        /// </summary>
        public Vector2 Location { get; set; }
        
        /// <summary>
        /// Gets or sets the color for info messages. Default: SharpDX.Color.White
        /// </summary>
        public Color InfoForegroundColor { get; set; }
        
        /// <summary>
        /// Gets or sets the color for warning messages. Default: SharpDX.Color.Yellow
        /// </summary>
        public Color WarningForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets the color for error messages. Default: SharpDX.Color.Red
        /// </summary>
        public Color ErrorForegroundColor { get; set; }


        /// <summary>
        /// Prints a info message.
        /// </summary>
        /// <param name="Message"></param>
        [ConditionalAttribute("DEBUG")]
        public void Info(string Message) {
            if (currentState != LogMessageType.Info) {
                Flush();
                currentState = LogMessageType.Info;
            }

            textBuffer.AppendLine(Message);
        }

        /// <summary>
        /// Prints a warning message.
        /// </summary>
        /// <param name="Message"></param>
        [ConditionalAttribute("DEBUG")]
        public void Warning(string Message) {
            if (currentState != LogMessageType.Warning) {
                Flush();
                currentState = LogMessageType.Warning;
            }

            textBuffer.AppendLine(Message);
        }

        /// <summary>
        /// Prints a error message.
        /// </summary>
        /// <param name="Message"></param>
        [ConditionalAttribute("DEBUG")]
        public void Error(string Message) {
            if (currentState != LogMessageType.Error) {
                Flush();
                currentState = LogMessageType.Error;
            }

            textBuffer.AppendLine(Message);
        }

        /// <summary>
        /// Clears the buffer and writes the text to the batch.
        /// </summary>
        [ConditionalAttribute("DEBUG")]
        public void Flush() {
            // if the buffer is empty do nothing
            if (textBuffer.Length < 0)
                return;

            // add the text to the batch
            spriteBatch.DrawString(Font, textBuffer.ToString(), cursor, getStateColor());

            // get the size of the text
            Vector2 bounds = Font.MeasureString(textBuffer);
             
            // check if the text fits in
            if (bounds.X > spriteBatch.GraphicsDevice.Viewport.Width + Location.X) {
                // TODO: add some magic clipping
            }

            // new line
            cursor.X = Location.X;
            cursor.Y += bounds.Y;
            
            // empty the buffer
            textBuffer.Length = 0;
        }

        /// <summary>
        /// Starts a frame of text
        /// </summary>
        [ConditionalAttribute("DEBUG")]
        public void Begin() {
            spriteBatch.Begin();
            cursor = Location;
            currentState = LogMessageType.Info;
        }

        /// <summary>
        /// Ends a frame of text
        /// </summary>
        [ConditionalAttribute("DEBUG")]
        public void End() {
            // add everything that is still in the buffer to the batch
            Flush();
            textBuffer.Capacity = 64;
            spriteBatch.End();
        }


        protected override void Dispose(bool disposeManagedResources) {
            base.Dispose(disposeManagedResources);
            if (disposeManagedResources) {
                Font.Dispose();
                spriteBatch.Dispose();
                textBuffer.Capacity = 0;
            }
        }

        Color getStateColor() {
            switch (currentState) {
                case LogMessageType.Error:
                    return ErrorForegroundColor;
                case LogMessageType.Warning:
                    return WarningForegroundColor;
                case LogMessageType.Info:
                default:
                    return InfoForegroundColor;
            }
        }
    }
}
