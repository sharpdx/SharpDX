namespace TiledResources
{
    using System;
    using SharpDX;

    /// <summary>
    /// Simple orbit camera that rotates around an fixed point
    /// </summary>
    internal sealed class OrbitCamera : Camera
    {
        private float _radius;
        private float _rotation;
        private float _upDown;

        public OrbitCamera()
        {
            Fov = MathUtil.PiOverTwo;
            Speed = 1f;
            ZoomSpeed = 0.5f;
        }

        /// <summary>
        /// The distance to the orbited point
        /// </summary>
        public float Radius { get { return _radius; } set { _radius = value; RecomputeEye(); } }

        /// <summary>
        /// The rotation angle around vertical axis
        /// </summary>
        public float Rotation { get { return _rotation; } set { _rotation = value; RecomputeEye(); } }

        /// <summary>
        /// The up-down rotation angle
        /// </summary>
        public float UpDown { get { return _upDown; } set { _upDown = value; RecomputeEye(); } }

        /// <summary>
        /// Gets or sets the target position
        /// </summary>
        public Vector3 Target { get { return TargetPosition; } set { TargetPosition = value; RecomputeEye(); } }

        /// <summary>
        /// Rotation speed
        /// </summary>
        public float Speed { get; set; }

        /// <summary>
        /// Zoom speed
        /// </summary>
        public float ZoomSpeed { get; set; }

        /// <summary>
        /// Sets the aspect ratio of this camera, usually it is the With divided by Height of the viewport
        /// </summary>
        /// <param name="aspectRatio">The apect ratio value</param>
        /// <exception cref="ArgumentOutOfRangeException">Is thrown when <paramref name="aspectRatio"/> is less than or equal to zero.</exception>
        public void SetAspectRatio(float aspectRatio)
        {
            if (aspectRatio <= 0f)
                throw new ArgumentOutOfRangeException("aspectRatio");

            AspectRatio = aspectRatio;
        }

        public void Left(float timeDelta)
        {
            Rotation -= GetMoveSpeed() * timeDelta;
        }

        public void Right(float timeDelta)
        {
            Rotation += GetMoveSpeed() * timeDelta;
        }

        public void Up(float timeDelta)
        {
            UpDown += GetMoveSpeed() * timeDelta;
        }

        public void Down(float timeDelta)
        {
            UpDown -= GetMoveSpeed() * timeDelta;
        }

        public void ZoomIn(float timeDelta)
        {
            Fov -= timeDelta * ZoomSpeed * 5f;

            const float minFov = MathUtil.Pi / 60f;
            if (Fov < minFov) Fov = minFov;
        }

        public void ZoomOut(float timeDelta)
        {
            Fov += timeDelta * ZoomSpeed * 5f;

            const float maxFov = MathUtil.Pi * 2f / 3f;
            if (Fov > maxFov) Fov = maxFov;
        }

        private float GetMoveSpeed()
        {
            return Speed * Fov / DefaultFov;
        }

        private void RecomputeEye()
        {
            if (_upDown > MathUtil.PiOverTwo - 0.001f) _upDown = MathUtil.PiOverTwo - 0.001f;
            else if (_upDown < -MathUtil.PiOverTwo + 0.001f) _upDown = -MathUtil.PiOverTwo + 0.001f;

            if (_radius < 0f) _radius = 0f;

            var matrix = Matrix.RotationX(_upDown) * Matrix.RotationZ(_rotation);
            var x = Vector3.Transform(Vector3.UnitY * Radius, matrix);

            EyePosition = Target + new Vector3(x.X, x.Y, x.Z);
        }
    }
}