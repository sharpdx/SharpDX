namespace WPFHost
{
    using System;
    using SharpDX.Direct3D10;

    public interface ISceneHost
    {
        Device Device { get; }
    }

    public interface IScene
    {
        void Attach(ISceneHost host);
        void Detach();
        void Update(TimeSpan timeSpan);
        void Render();
    }
}
