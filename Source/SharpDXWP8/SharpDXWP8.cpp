// SharpDXWP8.cpp
#include "pch.h"
#include "SharpDXWP8.h"

using namespace SharpDXWP8;
using namespace Platform;

Interop::Interop()
{
}
int Interop::D3D11CreateDevice() 
{
	return (int)::D3D11CreateDevice;
}

int Interop::CreateDXGIFactory1()
{
	return (int)::CreateDXGIFactory1;
}
