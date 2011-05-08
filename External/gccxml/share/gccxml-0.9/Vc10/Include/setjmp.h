/***
*setjmp.h - definitions/declarations for setjmp/longjmp routines
*
*       Copyright (c) Microsoft Corporation. All rights reserved.
*
*Purpose:
*       This file defines the machine-dependent buffer used by
*       setjmp/longjmp to save and restore the program state, and
*       declarations for those routines.
*       [ANSI/System V]
*
*       [Public]
*
****/

#pragma once

#ifndef _INC_SETJMP
#define _INC_SETJMP

#include <crtdefs.h>

#if defined(_M_CEE)
/*
 * The reason why simple setjmp won't work here is that there may
 * be case when CLR stubs are on the stack e.g. function call just
 * after jitting, and not unwinding CLR will result in bad state of
 * CLR which then can AV or do something very bad.
 */
#include <setjmpex.h>
#endif  /* defined(_M_CEE) */

/*
 * Currently, all MS C compilers for Win32 platforms default to 8 byte
 * alignment.
 */
#pragma pack(push,_CRT_PACKING)

#ifdef  __cplusplus
extern "C" {
#endif

/*
 * Definitions specific to particular setjmp implementations.
 */

#if     defined(_M_IX86)

/*
 * MS compiler for x86
 */

#ifndef _INC_SETJMPEX
#define setjmp  _setjmp
#endif

#define _JBLEN  16
#define _JBTYPE int

/*
 * Define jump buffer layout for x86 setjmp/longjmp.
 */
typedef struct __JUMP_BUFFER {
    unsigned long Ebp;
    unsigned long Ebx;
    unsigned long Edi;
    unsigned long Esi;
    unsigned long Esp;
    unsigned long Eip;
    unsigned long Registration;
    unsigned long TryLevel;
    unsigned long Cookie;
    unsigned long UnwindFunc;
    unsigned long UnwindData[6];
} _JUMP_BUFFER;


#elif defined(_M_IA64)

/*
 * Minimum length is 528 bytes
 * Since this is allocated as an array of "SETJMP_FLOAT128", the
 * number of entries required is 33 (16-byte aligned).
 */

/* Avoid conflicts with winnt.h FLOAT128 by giving the typedef another name. */
typedef _CRT_ALIGN(16) struct _SETJMP_FLOAT128 {
    __int64 LowPart;
    __int64 HighPart;
} SETJMP_FLOAT128;
                                  
#define _JBLEN  33
typedef SETJMP_FLOAT128 _JBTYPE;
#ifndef _INC_SETJMPEX
#define setjmp  _setjmp
#endif
/*
 * Define jump buffer layout for IA64 setjmp/longjmp.
 */

typedef struct __JUMP_BUFFER {

    /*
     * x86 reserved.
     */

    unsigned long iAReserved[6];

    /*
     * x86 C9.0 compatibility
     */

    unsigned long Registration;  /* point to the UnwindData field. */
    unsigned long TryLevel;      /* ignored by setjmp */
    unsigned long Cookie;        /* set to "VC20" by setjmp */
    unsigned long UnwindFunc;    /* set to EM longjmp() by setjmp */

    /*
     * First dword is zero to indicate it's an exception registration
     * record prepared by EM setjmp function.
     * Second dword is set to 0 for unsafe EM setjmp, and 1 for safe
     * EM setjmp.
     * Third dword is set to the setjmp site memory stack frame pointer.
     * Fourth dword is set to the setjmp site backing store frame pointer.
     */

    unsigned long UnwindData[6];

    /*
     * floating point status register,
     * and preserved floating point registers fs0 - fs19
     */

    SETJMP_FLOAT128 FltS0;
    SETJMP_FLOAT128 FltS1;
    SETJMP_FLOAT128 FltS2;
    SETJMP_FLOAT128 FltS3;
    SETJMP_FLOAT128 FltS4;
    SETJMP_FLOAT128 FltS5;
    SETJMP_FLOAT128 FltS6;
    SETJMP_FLOAT128 FltS7;
    SETJMP_FLOAT128 FltS8;
    SETJMP_FLOAT128 FltS9;
    SETJMP_FLOAT128 FltS10;
    SETJMP_FLOAT128 FltS11;
    SETJMP_FLOAT128 FltS12;
    SETJMP_FLOAT128 FltS13;
    SETJMP_FLOAT128 FltS14;
    SETJMP_FLOAT128 FltS15;
    SETJMP_FLOAT128 FltS16;
    SETJMP_FLOAT128 FltS17;
    SETJMP_FLOAT128 FltS18;
    SETJMP_FLOAT128 FltS19;

    __int64 FPSR;

    /*
     * return link and preserved branch registers bs0 - bs4
     */

    __int64 StIIP;     /* continuation address */
    __int64 BrS0;
    __int64 BrS1;
    __int64 BrS2;
    __int64 BrS3;
    __int64 BrS4;

    /*
     * preserved general registers s0 - s3, sp, nats
     */

    __int64 IntS0;
    __int64 IntS1;
    __int64 IntS2;
    __int64 IntS3;

    /*
     * bsp, pfs, unat, lc
     */

    __int64 RsBSP;
    __int64 RsPFS;     /* previous frame marker (cfm of setjmp's caller) */
    __int64 ApUNAT;    /* User Nat collection register (preserved) */
    __int64 ApLC;      /* loop counter */

    __int64 IntSp;     /* memory stack pointer */
    __int64 IntNats;   /* Nat bits of preserved integer regs s0 - s3 */
    __int64 Preds;     /* predicates */

} _JUMP_BUFFER;
#elif defined(_M_AMD64)

typedef _CRT_ALIGN(16) struct _SETJMP_FLOAT128 {
    unsigned __int64 Part[2];
} SETJMP_FLOAT128;

#define _JBLEN  16
typedef SETJMP_FLOAT128 _JBTYPE;

#ifndef _INC_SETJMPEX
#define setjmp  _setjmp
#endif

typedef struct _JUMP_BUFFER {
    unsigned __int64 Frame;
    unsigned __int64 Rbx;
    unsigned __int64 Rsp;
    unsigned __int64 Rbp;
    unsigned __int64 Rsi;
    unsigned __int64 Rdi;
    unsigned __int64 R12;
    unsigned __int64 R13;
    unsigned __int64 R14;
    unsigned __int64 R15;
    unsigned __int64 Rip;
    unsigned __int64 Spare;
    
    SETJMP_FLOAT128 Xmm6;
    SETJMP_FLOAT128 Xmm7;
    SETJMP_FLOAT128 Xmm8;
    SETJMP_FLOAT128 Xmm9;
    SETJMP_FLOAT128 Xmm10;
    SETJMP_FLOAT128 Xmm11;
    SETJMP_FLOAT128 Xmm12;
    SETJMP_FLOAT128 Xmm13;
    SETJMP_FLOAT128 Xmm14;
    SETJMP_FLOAT128 Xmm15;
} _JUMP_BUFFER;

#endif


/* Define the buffer type for holding the state information */

#ifndef _JMP_BUF_DEFINED
typedef _JBTYPE jmp_buf[_JBLEN];
#define _JMP_BUF_DEFINED
#endif


/* Function prototypes */

int __cdecl setjmp(_Out_ jmp_buf _Buf);

#ifdef  __cplusplus
}
#endif

#ifdef  __cplusplus
extern "C"
{
_CRTIMP __declspec(noreturn) void __cdecl longjmp(_In_ jmp_buf _Buf, _In_ int _Value);
}
#else
_CRTIMP __declspec(noreturn) void __cdecl longjmp(_In_ jmp_buf _Buf, _In_ int _Value);
#endif

#pragma pack(pop)

#endif  /* _INC_SETJMP */
