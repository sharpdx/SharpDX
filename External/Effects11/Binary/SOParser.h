//////////////////////////////////////////////////////////////////////////////
//
//  Copyright (C) Microsoft Corporation.  All Rights Reserved.
//
//  File:       SOParser.h
//  Content:    D3DX11 Effects Stream Out Decl Parser
//
//////////////////////////////////////////////////////////////////////////////

#pragma once

namespace D3DX11Effects
{


//////////////////////////////////////////////////////////////////////////
// CSOParser
//////////////////////////////////////////////////////////////////////////

class CSOParser
{

    CEffectVector<D3D11_SO_DECLARATION_ENTRY>   m_vDecls;                                       // Set of parsed decl entries
    D3D11_SO_DECLARATION_ENTRY                  m_newEntry;                                     // Currently parsing entry
    LPSTR                                       m_SemanticString[D3D11_SO_BUFFER_SLOT_COUNT];   // Copy of strings

    static const UINT MAX_ERROR_SIZE = 254;
    char                                        m_pError[ MAX_ERROR_SIZE + 1 ];                 // Error buffer

public:
    CSOParser()
    {
        ZeroMemory(&m_newEntry, sizeof(m_newEntry));
        ZeroMemory(m_SemanticString, sizeof(m_SemanticString));
        m_pError[0] = 0;
    }

    ~CSOParser()
    {
        for( UINT Stream = 0; Stream < D3D11_SO_STREAM_COUNT; Stream++ )
        {
            SAFE_DELETE_ARRAY( m_SemanticString[Stream] );
        }
    }

    // Parse a single string, assuming stream 0
    HRESULT Parse( __in_z LPCSTR pString )
    {
        m_vDecls.Clear();
        return Parse( 0, pString );
    }

    // Parse all 4 streams
    HRESULT Parse( __in_z LPSTR pStreams[D3D11_SO_STREAM_COUNT] )
    {
        HRESULT hr = S_OK;
        m_vDecls.Clear();
        for( UINT iDecl=0; iDecl < D3D11_SO_STREAM_COUNT; ++iDecl )
        {
            hr = Parse( iDecl, pStreams[iDecl] );
            if( FAILED(hr) )
            {
                char pStream[16];
                StringCchPrintfA( pStream, 16, " in stream %d.", iDecl );
                pStream[15] = 0;
                StringCchCatA( m_pError, MAX_ERROR_SIZE, pStream );
                return hr;
            }
        }
        return hr;
    }

    // Return resulting declarations
    D3D11_SO_DECLARATION_ENTRY *GetDeclArray()
    {
        return &m_vDecls[0];
    }

    char* GetErrorString()
    {
        return m_pError;
    }

    UINT GetDeclCount() const
    {
        return m_vDecls.GetSize();
    }

    // Return resulting buffer strides
    void GetStrides( UINT strides[4] )
    {
        UINT len = GetDeclCount();
        strides[0] = strides[1] = strides[2] = strides[3] = 0;

        for( UINT i=0; i < len; i++ )
        {
            strides[m_vDecls[i].OutputSlot] += m_vDecls[i].ComponentCount * sizeof(float);
        }
    }

protected:

    // Parse a single string "[<slot> :] <semantic>[<index>][.<mask>]; [[<slot> :] <semantic>[<index>][.<mask>][;]]"
    HRESULT Parse( UINT Stream, __in_z LPCSTR pString )
    {
        HRESULT hr = S_OK;

        m_pError[0] = 0;

        if( pString == NULL )
            return S_OK;

        UINT len = (UINT)strlen( pString );
        if( len == 0 )
            return S_OK;

        SAFE_DELETE_ARRAY( m_SemanticString[Stream] );
        VN( m_SemanticString[Stream] = NEW char[len + 1] );
        StringCchCopyA( m_SemanticString[Stream], len + 1, pString );

        LPSTR pSemantic = m_SemanticString[Stream];

        while( TRUE )
        {
            // Each decl entry is delimited by a semi-colon
            LPSTR pSemi = strchr( pSemantic, ';' );

            // strip leading and trailing spaces
            LPSTR pEnd;
            if( pSemi != NULL )
            {
                *pSemi = '\0';
                pEnd = pSemi - 1;
            }
            else
            {
                pEnd = pSemantic + strlen( pSemantic );
            }
            while( isspace( (unsigned char)*pSemantic ) )
                pSemantic++;
            while( pEnd > pSemantic && isspace( (unsigned char)*pEnd ) )
            {
                *pEnd = '\0';
                pEnd--;
            }

            if( *pSemantic != '\0' )
            {
                VH( AddSemantic( pSemantic ) );
                m_newEntry.Stream = Stream;

                VH( m_vDecls.Add( m_newEntry ) );
            }
            if( pSemi == NULL )
                break;
            pSemantic = pSemi + 1;
        }

lExit:
        return hr;
    }

    // Parse a single decl  "[<slot> :] <semantic>[<index>][.<mask>]"
    HRESULT AddSemantic( __inout_z LPSTR pSemantic )
    {
        HRESULT hr = S_OK;

        D3DXASSERT( pSemantic );

        ZeroMemory( &m_newEntry, sizeof(m_newEntry) );
        VH( ConsumeOutputSlot( &pSemantic ) );
        VH( ConsumeRegisterMask( pSemantic ) );
        VH( ConsumeSemanticIndex( pSemantic ) );

        // pSenantic now contains only the SemanticName (all other fields were consumed)
        if( strcmp( "$SKIP", pSemantic ) != 0 )
        {
            m_newEntry.SemanticName = pSemantic;
        }

lExit:
        return hr;
    }

    // Parse optional mask "[.<mask>]"
    HRESULT ConsumeRegisterMask( __inout_z LPSTR pSemantic )
    {
        HRESULT hr = S_OK;
        const char *pFullMask1 = "xyzw";
        const char *pFullMask2 = "rgba";
        SIZE_T stringLength;
        SIZE_T startComponent = 0;
        LPCSTR p;

        D3DXASSERT( pSemantic );

        pSemantic = strchr( pSemantic, '.' ); 

        if( pSemantic == NULL )
        {
            m_newEntry.ComponentCount = 4;
            return S_OK;
        }

        *pSemantic = '\0';
        pSemantic++;

        stringLength = strlen( pSemantic );
        p = strstr(pFullMask1, pSemantic );
        if( p )
        {
            startComponent = (UINT)( p - pFullMask1 );
        }
        else
        {
            p = strstr( pFullMask2, pSemantic );
            if( p )
                startComponent = (UINT)( p - pFullMask2 );
            else
            {
                StringCchPrintfA( m_pError, MAX_ERROR_SIZE, "ID3D11Effect::ParseSODecl - invalid mask declaration '%s'", pSemantic );
                VH( E_FAIL );
            }

        }

        if( stringLength == 0 )
            stringLength = 4;

        m_newEntry.StartComponent = (BYTE)startComponent;
        m_newEntry.ComponentCount = (BYTE)stringLength;

lExit:
        return hr;
    }

    // Parse optional output slot "[<slot> :]"
    HRESULT ConsumeOutputSlot( __deref_inout_z LPSTR* ppSemantic )
    {
        D3DXASSERT( ppSemantic && *ppSemantic );

        HRESULT hr = S_OK;
        LPSTR pColon = strchr( *ppSemantic, ':' ); 

        if( pColon == NULL )
            return S_OK;

        if( pColon == *ppSemantic )
        {
            StringCchCopyA( m_pError, MAX_ERROR_SIZE,
                           "ID3D11Effect::ParseSODecl - Invalid output slot" );
            VH( E_FAIL );
        }

        *pColon = '\0';
        int outputSlot = atoi( *ppSemantic );
        if( outputSlot < 0 || outputSlot > 255 )
        {
            StringCchCopyA( m_pError, MAX_ERROR_SIZE,
                           "ID3D11Effect::ParseSODecl - Invalid output slot" );
            VH( E_FAIL );
        }
        m_newEntry.OutputSlot = (BYTE)outputSlot;

        while( *ppSemantic < pColon )
        {
            if( !isdigit( (unsigned char)**ppSemantic ) )
            {
                StringCchPrintfA( m_pError, MAX_ERROR_SIZE, "ID3D11Effect::ParseSODecl - Non-digit '%c' in output slot", **ppSemantic );
                VH( E_FAIL );
            }
            (*ppSemantic)++;
        }

        // skip the colon (which is now '\0')
        (*ppSemantic)++;

        while( isspace( (unsigned char)**ppSemantic ) )
            (*ppSemantic)++;

lExit:
        return hr;
    }

    // Parse optional index "[<index>]"
    HRESULT ConsumeSemanticIndex( __inout_z LPSTR pSemantic )
    {
        D3DXASSERT( pSemantic );

        UINT uLen = (UINT)strlen( pSemantic );

        // Grab semantic index
        while( uLen > 0 && isdigit( (unsigned char)pSemantic[uLen - 1] ) )
            uLen--;

        if( isdigit( (unsigned char)pSemantic[uLen] ) )
        {
            m_newEntry.SemanticIndex = atoi( pSemantic + uLen );
            pSemantic[uLen] = '\0';
        } 
        else
        {
            m_newEntry.SemanticIndex = 0;
        }

        return S_OK;
    }
};


} // end namespace D3DX11Effects
