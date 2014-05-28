using SharpDX.Serialization;

namespace SharpDX.Toolkit
{
    public struct SrtTransform : IDataSerializable
    {
        public Vector3 Scale;

        public Quaternion Rotation;

        public Vector3 Translation;

        public static readonly SrtTransform Identity = new SrtTransform(Vector3.One, Quaternion.Identity, Vector3.Zero);

        public SrtTransform(Vector3 scale, Quaternion rotation, Vector3 translation)
        {
            Scale = scale;
            Rotation = rotation;
            Translation = translation;
        }

        public SrtTransform(Matrix transform)
        {
            transform.Decompose(out Scale, out Rotation, out Translation);
        }

        public static void Slerp(ref SrtTransform start, ref SrtTransform end, float amount, out SrtTransform result)
        {
            Vector3.Lerp(ref start.Scale, ref end.Scale, amount, out result.Scale);
            Quaternion.Slerp(ref start.Rotation, ref end.Rotation, amount, out result.Rotation);
            Vector3.Lerp(ref start.Translation, ref end.Translation, amount, out result.Translation);
        }

        public static SrtTransform Slerp(SrtTransform start, SrtTransform end, float amount)
        {
            SrtTransform result;
            Slerp(ref start, ref end, amount, out result);
            return result;
        }

        public static explicit operator Matrix(SrtTransform value)
        {
            return Matrix.Scaling(value.Scale) * Matrix.RotationQuaternion(value.Rotation) * Matrix.Translation(value.Translation);
        }

        public void Serialize(BinarySerializer serializer)
        {
            serializer.Serialize(ref Scale);
            serializer.Serialize(ref Rotation);
            serializer.Serialize(ref Translation);
        }

        public bool Equals(SrtTransform other)
        {
            return Scale.Equals(other.Scale) && Rotation.Equals(other.Rotation) && Translation.Equals(other.Translation);
        }

        public override bool Equals(object value)
        {
            if (value == null)
                return false;

            if (!ReferenceEquals(value.GetType(), typeof(SrtTransform)))
                return false;

            return Equals((SrtTransform)value);
        }
    }
}
