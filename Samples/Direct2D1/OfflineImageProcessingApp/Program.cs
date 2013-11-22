namespace OfflineImageProcessingApp
{
    using SharpDX;
    using SharpDX.IO;
    // use namespaces shortcuts to reduce typing and avoid the messing the same class names from different namespaces
    using d2 = SharpDX.Direct2D1;
    using d3d = SharpDX.Direct3D11;
    using dxgi = SharpDX.DXGI;
    using wic = SharpDX.WIC;
    using dw = SharpDX.DirectWrite;

    // This sample represents a method of offline image processing.
    // It requires references to DirectX 11.1, so it will run only on Windows 7 Platform Update or above.
    // It references SharpDX from Win8Desktop-net40 folder.
    // The same method is applicable in Windows 8 modern apps.
    // This code skips all sanity checks to focus on feature demonstration. In production apps do not forget to check
    // for parameters correctness and feature support.

    // The sample does the following:
    // 1. Initializes references to all needed devices and factories
    // 2. Loads an input image and stores its size to be used in the output image
    // 3. Prepares an effect graph to process the image
    // 4. Prepares some text to draw it over image (useful for any kind of annotations, watermarks, etc).
    // 5. Initializes the render target - here can be any compatible surface.
    // 6. Draws the prepared effect graph and text.
    // 7. Saves the image in the output file.
    // 8. Disposes everything.
    // 9. Starts the default associated program to display the result.

    class Program
    {
        static void Main()
        {
            // input and output files are supposed to be in the program folder
            var inputPath = "Input.png";
            var outputPath = "Output.png";

            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            // INITIALIZATION ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            // initialize the D3D device which will allow to render to image any graphics - 3D or 2D
            var defaultDevice = new SharpDX.Direct3D11.Device(SharpDX.Direct3D.DriverType.Hardware,
                                                              d3d.DeviceCreationFlags.VideoSupport
                                                              | d3d.DeviceCreationFlags.BgraSupport
                                                              | d3d.DeviceCreationFlags.Debug); // take out the Debug flag for better performance

            var d3dDevice = defaultDevice.QueryInterface<d3d.Device1>(); // get a reference to the Direct3D 11.1 device
            var dxgiDevice = d3dDevice.QueryInterface<dxgi.Device>(); // get a reference to DXGI device

            var d2dDevice = new d2.Device(dxgiDevice); // initialize the D2D device

            var imagingFactory = new wic.ImagingFactory2(); // initialize the WIC factory

            // initialize the DeviceContext - it will be the D2D render target and will allow all rendering operations
            var d2dContext = new d2.DeviceContext(d2dDevice, d2.DeviceContextOptions.None);

            var dwFactory = new dw.Factory();

            // specify a pixel format that is supported by both D2D and WIC
            var d2PixelFormat = new d2.PixelFormat(dxgi.Format.R8G8B8A8_UNorm, d2.AlphaMode.Premultiplied);
            // if in D2D was specified an R-G-B-A format - use the same for wic
            var wicPixelFormat = wic.PixelFormat.Format32bppPRGBA;

            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            // IMAGE LOADING ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            var decoder = new wic.PngBitmapDecoder(imagingFactory); // we will load a PNG image
            var inputStream = new wic.WICStream(imagingFactory, inputPath, NativeFileAccess.Read); // open the image file for reading
            decoder.Initialize(inputStream, wic.DecodeOptions.CacheOnLoad);

            // decode the loaded image to a format that can be consumed by D2D
            var formatConverter = new wic.FormatConverter(imagingFactory);
            formatConverter.Initialize(decoder.GetFrame(0), wicPixelFormat);

            // load the base image into a D2D Bitmap
            //var inputBitmap = d2.Bitmap1.FromWicBitmap(d2dContext, formatConverter, new d2.BitmapProperties1(d2PixelFormat));

            // store the image size - output will be of the same size
            var inputImageSize = formatConverter.Size;
            var pixelWidth = inputImageSize.Width;
            var pixelHeight = inputImageSize.Height;

            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            // EFFECT SETUP ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            // Effect 1 : BitmapSource - take decoded image data and get a BitmapSource from it
            var bitmapSourceEffect = new d2.Effects.BitmapSource(d2dContext);
            bitmapSourceEffect.WicBitmapSource = formatConverter;

            // Effect 2 : GaussianBlur - give the bitmapsource a gaussian blurred effect
            var gaussianBlurEffect = new d2.Effects.GaussianBlur(d2dContext);
            gaussianBlurEffect.SetInput(0, bitmapSourceEffect.Output, true);
            gaussianBlurEffect.StandardDeviation = 5f;

            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            // OVERLAY TEXT SETUP ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            var textFormat = new dw.TextFormat(dwFactory, "Arial", 15f); // create the text format of specified font configuration

            // draw a long text to show the automatic line wrapping
            var textToDraw = "Some long text to show the drawing of preformatted "
                             + "glyphs using DirectWrite on the Direct2D surface."
                             + " Notice the automatic wrapping of line if it exceeds desired width.";

            // create the text layout - this improves the drawing performance for static text
            // as the glyph positions are precalculated
            var textLayout = new dw.TextLayout(dwFactory, textToDraw, textFormat, 300f, 1000f);

            var textBrush = new d2.SolidColorBrush(d2dContext, Color.LightGreen);

            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            // RENDER TARGET SETUP ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            // create the d2d bitmap description using default flags (from SharpDX samples) and 96 DPI
            var d2dBitmapProps = new d2.BitmapProperties1(d2PixelFormat, 96, 96, d2.BitmapOptions.Target | d2.BitmapOptions.CannotDraw);

            // the render target
            var d2dRenderTarget = new d2.Bitmap1(d2dContext, new Size2(pixelWidth, pixelHeight), d2dBitmapProps);
            d2dContext.Target = d2dRenderTarget; // associate bitmap with the d2d context

            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            // DRAWING ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            // slow preparations - fast drawing:
            d2dContext.BeginDraw();
            d2dContext.DrawImage(gaussianBlurEffect);
            d2dContext.DrawTextLayout(new Vector2(5f, 5f), textLayout, textBrush);
            d2dContext.EndDraw();

            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            // IMAGE SAVING ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            // delete the output file if it already exists
            if (System.IO.File.Exists(outputPath)) System.IO.File.Delete(outputPath);

            // use the appropiate overload to write either to stream or to a file
            var stream = new wic.WICStream(imagingFactory, outputPath, NativeFileAccess.Write);

            // select the image encoding format HERE
            var encoder = new wic.PngBitmapEncoder(imagingFactory);
            encoder.Initialize(stream);

            var bitmapFrameEncode = new wic.BitmapFrameEncode(encoder);
            bitmapFrameEncode.Initialize();
            bitmapFrameEncode.SetSize(pixelWidth, pixelHeight);
            bitmapFrameEncode.SetPixelFormat(ref wicPixelFormat);

            // this is the trick to write D2D1 bitmap to WIC
            var imageEncoder = new wic.ImageEncoder(imagingFactory, d2dDevice);
            imageEncoder.WriteFrame(d2dRenderTarget, bitmapFrameEncode, new wic.ImageParameters(d2PixelFormat, 96, 96, 0, 0, pixelWidth, pixelHeight));

            bitmapFrameEncode.Commit();
            encoder.Commit();

            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            // CLEANUP ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            // dispose everything and free used resources

            bitmapFrameEncode.Dispose();
            encoder.Dispose();
            stream.Dispose();
            textBrush.Dispose();
            textLayout.Dispose();
            textFormat.Dispose();
            formatConverter.Dispose();
            gaussianBlurEffect.Dispose();
            bitmapSourceEffect.Dispose();
            d2dRenderTarget.Dispose();
            inputStream.Dispose();
            decoder.Dispose();
            d2dContext.Dispose();
            dwFactory.Dispose();
            imagingFactory.Dispose();
            d2dDevice.Dispose();
            dxgiDevice.Dispose();
            d3dDevice.Dispose();
            defaultDevice.Dispose();

            // show the result
            System.Diagnostics.Process.Start(outputPath);
        }
    }
}
