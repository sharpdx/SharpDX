namespace TiledResources
{
    using SharpDX;

    /// <summary>
    /// Base implementation for a camera
    /// </summary>
    /// <remarks>Due to prebaked geometry in this sample - all matrices are Right-Handed.</remarks>
    internal abstract class Camera
    {
        public const float NearClippingPlane = 0.001f; // z-near
        public const float FarClippingPlane = 1000f;  // z-far
        public const float DefaultAspectRatio = 1f;
        public const float DefaultFov = MathUtil.PiOverTwo * 7f / 9f;

        protected static readonly Vector3 _upDirection = Vector3.UnitZ;

        private Matrix _view;
        private Matrix _projection;

        private Vector3 _eyePosition = Vector3.UnitY;
        private Vector3 _targetPosition = Vector3.Zero;

        private float _aspectRatio = DefaultAspectRatio;
        private float _fov = DefaultFov;

        // by default matrices are dirty, to recompute them on first call
        private bool _isViewDirty = true;
        private bool _isProjectionDirty = true;

        /// <summary>
        /// Gets the camera view matrix
        /// </summary>
        public Matrix View
        {
            get
            {
                if (_isViewDirty)
                    RecreateView();
                return _view;
            }
        }

        /// <summary>
        /// Gets the camera projection matrix
        /// </summary>
        public Matrix Projection
        {
            get
            {
                if (_isProjectionDirty)
                    RecreateProjection();
                return _projection;
            }
        }

        /// <summary>
        /// Gets or sets the eye position for the view matrix
        /// </summary>
        protected Vector3 EyePosition
        {
            get { return _eyePosition; }
            set
            {
                _eyePosition = value;
                _isViewDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the target position for the view matrix
        /// </summary>
        protected Vector3 TargetPosition
        {
            get { return _targetPosition; }
            set
            {
                _targetPosition = value;
                _isViewDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the aspect ration (width divided by height) for the projection matrix
        /// </summary>
        protected float AspectRatio
        {
            get { return _aspectRatio; }
            set
            {
                _aspectRatio = value;
                _isProjectionDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the field-of-view for the projection matrix, default value is 90 degrees
        /// </summary>
        protected float Fov
        {
            get { return _fov; }
            set
            {
                _fov = value;
                _isProjectionDirty = true;
            }
        }

        private void RecreateView()
        {
            _view = Matrix.LookAtRH(_eyePosition, _targetPosition, _upDirection);
            _isViewDirty = false;
        }

        private void RecreateProjection()
        {
            _projection = Matrix.PerspectiveFovRH(_fov, _aspectRatio, NearClippingPlane, FarClippingPlane);
            _isProjectionDirty = false;
        }
    }
}