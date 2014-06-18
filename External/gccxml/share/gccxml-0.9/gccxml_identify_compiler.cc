#if defined(__GNUC__)
GCCXML_SUPPORT="GCC"
#elif defined(__sgi) && defined(_COMPILER_VERSION)
GCCXML_SUPPORT="MIPSpro"
#elif defined(__INTEL_COMPILER) && (__INTEL_COMPILER >= 700)
GCCXML_SUPPORT="Intel"
#elif defined(__SUNPRO_CC)
GCCXML_SUPPORT="Sun"
#elif defined(__IBMCPP__)
GCCXML_SUPPORT="IBM"
#else
GCCXML_SUPPORT=""
#endif
