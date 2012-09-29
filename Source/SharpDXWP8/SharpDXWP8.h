#pragma once

namespace SharpDXWP8
{
    public ref class Interop sealed
    {
	private:
        Interop();
    public:

        static int D3D11CreateDevice();

		static int CreateDXGIFactory1();
    };
}