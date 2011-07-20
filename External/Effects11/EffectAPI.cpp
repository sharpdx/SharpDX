//////////////////////////////////////////////////////////////////////////////
//
//  Copyright (C) Microsoft Corporation.  All Rights Reserved.
//
//  File:       EffectAPI.cpp
//  Content:    D3DX11 Effect DLL entry points
//
//////////////////////////////////////////////////////////////////////////////

#include "pchfx.h"

using namespace D3DX11Effects;

HRESULT WINAPI D3DX11CreateEffectFromMemory(CONST void *pData, SIZE_T DataLength, UINT FXFlags, ID3D11Device *pDevice, ID3DX11Effect **ppEffect)
{
    HRESULT hr = S_OK;

    // Note that pData must point to a compiled effect, not HLSL
    VN( *ppEffect = NEW CEffect( FXFlags & D3DX11_EFFECT_RUNTIME_VALID_FLAGS) );
    VH( ((CEffect*)(*ppEffect))->LoadEffect(pData, static_cast<UINT>(DataLength)) );
    VH( ((CEffect*)(*ppEffect))->BindToDevice(pDevice) );

lExit:
    if (FAILED(hr))
    {
        SAFE_RELEASE(*ppEffect);
    }
    return hr;
}
