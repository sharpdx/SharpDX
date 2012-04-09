using SharpDX.Direct2D1;
using SharpDX;
using System;
using SharpDX.D3DCompiler;
using SharpDX.IO;

namespace CustomPixelShaderEffect
{
    class RippleEffect : CustomEffect
    {

        private Guid GUID_RipplePixelShader = Guid.NewGuid();

        private DrawTransform _drawTransform;

        private float _frequency;
        public float Frequency {
            get { return _frequency; }
            set { _frequency = value; } 
        }

        private float _phase;
        public float Phase
        {
            get { return _phase; }
            set { _phase = value; }
        }

        private float _amplitude;
        public float Amplitude
        {
            get { return _amplitude; }
            set { _amplitude = value; }
        }

        private DrawingPointF _center;
        public DrawingPointF Center
        {
            get { return _center; }
            set { _center = value; }
        }


        public RippleEffect()
        {
            
        }




//        public static void Register(SharpDX.Direct2D1.Factory1 factory){
            
//            string pszXml = @"
//                <?xml version='1.0'?>
//                <Effect>
//                    <!-- System Properties -->
//                    <Property name='DisplayName' type='string' value='Ripple'/>
//                    <Property name='Author' type='string' value='Microsoft Corporation'/>
//                    <Property name='Category' type='string' value='Stylize'/>
//                    <Property name='Description' type='string' value='Adds a ripple effect that can be animated'/>
//
//                    <Inputs>
//                        <Input name='Source'/>
//                    </Inputs>
//
//                    <!-- Custom Properties go here -->
//                    <Property name='Frequency' type='float'>
//                        <Property name='DisplayName' type='string' value='Frequency'/>
//                        <Property name='Min' type='float' value='0.0' />
//                        <Property name='Max' type='float' value='1000.0' />
//                        <Property name='Default' type='float' value='0.0' />
//                    </Property>
//
//                    <Property name='Phase' type='float'>
//                        <Property name='DisplayName' type='string' value='Phase'/>
//                        <Property name='Min' type='float' value='-100.0' />
//                        <Property name='Max' type='float' value='100.0' />
//                        <Property name='Default' type='float' value='0.0' />
//                    </Property>
//
//                    <Property name='Amplitude' type='float'>
//                        <Property name='DisplayName' type='string' value='Amplitude'/>
//                        <Property name='Min' type='float' value='0.0001' />
//                        <Property name='Max' type='float' value='1000.0' />
//                        <Property name='Default' type='float' value='0.0' />
//                    </Property>
//
//                    <Property name='Spread' type='float'>
//                        <Property name='DisplayName' type='string' value='Spread'/>
//                        <Property name='Min' type='float' value='0.0001' />
//                        <Property name='Max' type='float' value='1000.0' />
//                        <Property name='Default' type='float' value='0.0' />
//                    </Property>
//
//                    <Property name='Center' type='vector2'>
//                        <Property name='DisplayName' type='string' value='Center'/>
//                        <Property name='Min' type='vector2' value='(-2000.0, -2000.0)' />
//                        <Property name='Max' type='vector2' value='(2000.0, 2000.0)' />
//                        <Property name='Default' type='vector2' value='(0.0, 0.0)' />
//                    </Property>
//                </Effect>
//                ";

     

//        }






        public void Initialize(EffectContext effectContext, TransformGraph transformGraph)
        {
            byte[] data;


            var path = Windows.ApplicationModel.Package.Current.InstalledLocation.Path;


            using (var stream = new NativeFileStream(path + "\\Ripple.cso", NativeFileMode.Open, NativeFileAccess.Read))
            {
                data = new byte[stream.Length];
                stream.Read(data, 0, (int)stream.Length);
            }



            transformGraph.SingleTransformNode = _drawTransform;

            effectContext.LoadPixelShader(
                GUID_RipplePixelShader, data, data.Length);
        }

        public void PrepareForRender(ChangeType changeType)
        {
            throw new NotImplementedException();
        }

        public void SetGraph(TransformGraph transformGraph)
        {
            throw new NotImplementedException();
        }

        public IDisposable Shadow
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
