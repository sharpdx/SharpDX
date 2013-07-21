#pragma once

#define __ATTR_SAL

#ifndef _SAL_VERSION
#define _SAL_VERSION 20
#endif

#ifndef __SAL_H_VERSION
#define __SAL_H_VERSION 180000000
#endif

#define _PREFAST_
#define _USE_DECLSPECS_FOR_SAL 1
#define _USE_ATTRIBUTES_FOR_SAL 0

#define _SAL1_Source_(Name, args, annotes) _SA_annotes3(SAL_name, #Name, "", "1") _Group_(annotes _SAL_nop_impl_)
#define _SAL1_1_Source_(Name, args, annotes) _SA_annotes3(SAL_name, #Name, "", "1.1") _Group_(annotes _SAL_nop_impl_)
#define _SAL1_2_Source_(Name, args, annotes) _SA_annotes3(SAL_name, #Name, "", "1.2") _Group_(annotes _SAL_nop_impl_)
#define _SAL2_Source_(Name, args, annotes) _SA_annotes3(SAL_name, #Name, "", "2") _Group_(annotes _SAL_nop_impl_)
#define _SAL_L_Source_(Name, args, annotes) _SA_annotes3(SAL_name, #Name, "", "2") _Group_(annotes _SAL_nop_impl_)

#define _At_(target, annos)            _At_impl_(target, annos _SAL_nop_impl_)

#define _At_buffer_(target, iter, bound, annos)  _At_buffer_impl_(target, iter, bound, annos _SAL_nop_impl_)

#define _When_(expr, annos)            _When_impl_(expr, annos _SAL_nop_impl_)
#define _Group_(annos)                 _Group_impl_(annos _SAL_nop_impl_)
#define _GrouP_(annos)                 _GrouP_impl_(annos _SAL_nop_impl_)

#define _Success_(expr)                  _SAL2_Source_(_Success_, (expr), _Success_impl_(expr))

#define _Return_type_success_(expr)      _SAL2_Source_(_Return_type_success_, (expr), _Success_impl_(expr))

#define _On_failure_(annos)              _On_failure_impl_(annos _SAL_nop_impl_)

#define _Always_(annos)                  _Always_impl_(annos _SAL_nop_impl_)

#define _Use_decl_annotations_         _Use_decl_anno_impl_

#define _Notref_                       _Notref_impl_

#define _Pre_defensive_             _SA_annotes0(SAL_pre_defensive)
#define _Post_defensive_            _SA_annotes0(SAL_post_defensive)

#define _In_defensive_(annotes)     _Pre_defensive_ _Group_(annotes)
#define _Out_defensive_(annotes)    _Post_defensive_ _Group_(annotes)
#define _Inout_defensive_(annotes)  _Pre_defensive_ _Post_defensive_ _Group_(annotes)

#define _Reserved_                      _SAL2_Source_(_Reserved_, (), _Pre1_impl_(__null_impl))

#define _Const_                         _SAL2_Source_(_Const_, (), _Pre1_impl_(__readaccess_impl_notref))


#define _In_                            _SAL2_Source_(_In_, (), _Pre1_impl_(__notnull_impl_notref) _Pre_valid_impl_ _Deref_pre1_impl_(__readaccess_impl_notref))
#define _In_opt_                        _SAL2_Source_(_In_opt_, (), _Pre1_impl_(__maybenull_impl_notref) _Pre_valid_impl_ _Deref_pre_readonly_)

#define _In_z_                          _SAL2_Source_(_In_z_, (),     _In_     _Pre1_impl_(__zterm_impl))
#define _In_opt_z_                      _SAL2_Source_(_In_opt_z_, (), _In_opt_ _Pre1_impl_(__zterm_impl))

#define _In_reads_(size)               _SAL2_Source_(_In_reads_, (size), _Pre_count_(size)          _Deref_pre_readonly_)
#define _In_reads_opt_(size)           _SAL2_Source_(_In_reads_opt_, (size), _Pre_opt_count_(size)      _Deref_pre_readonly_)
#define _In_reads_bytes_(size)         _SAL2_Source_(_In_reads_bytes_, (size), _Pre_bytecount_(size)      _Deref_pre_readonly_)
#define _In_reads_bytes_opt_(size)     _SAL2_Source_(_In_reads_bytes_opt_, (size), _Pre_opt_bytecount_(size)  _Deref_pre_readonly_)
#define _In_reads_z_(size)             _SAL2_Source_(_In_reads_z_, (size), _In_reads_(size)     _Pre_z_)
#define _In_reads_opt_z_(size)         _SAL2_Source_(_In_reads_opt_z_, (size), _Pre_opt_count_(size)      _Deref_pre_readonly_     _Pre_opt_z_)
#define _In_reads_or_z_(size)          _SAL2_Source_(_In_reads_or_z_, (size), _In_ _When_(_String_length_(_Curr_) < (size), _Pre_z_) _When_(_String_length_(_Curr_) >= (size), _Pre1_impl_(__count_impl(size))))
#define _In_reads_or_z_opt_(size)      _SAL2_Source_(_In_reads_or_z_opt_, (size), _In_opt_ _When_(_String_length_(_Curr_) < (size), _Pre_z_) _When_(_String_length_(_Curr_) >= (size), _Pre1_impl_(__count_impl(size))))

#define _In_reads_to_ptr_(ptr)         _SAL2_Source_(_In_reads_to_ptr_, (ptr), _Pre_ptrdiff_count_(ptr)     _Deref_pre_readonly_)
#define _In_reads_to_ptr_opt_(ptr)     _SAL2_Source_(_In_reads_to_ptr_opt_, (ptr), _Pre_opt_ptrdiff_count_(ptr) _Deref_pre_readonly_)
#define _In_reads_to_ptr_z_(ptr)       _SAL2_Source_(_In_reads_to_ptr_z_, (ptr), _In_reads_to_ptr_(ptr) _Pre_z_)
#define _In_reads_to_ptr_opt_z_(ptr)   _SAL2_Source_(_In_reads_to_ptr_opt_z_, (ptr), _Pre_opt_ptrdiff_count_(ptr) _Deref_pre_readonly_  _Pre_opt_z_)

#define _Out_                                  _SAL2_Source_(_Out_, (),     _Out_impl_)
#define _Out_opt_                              _SAL2_Source_(_Out_opt_, (), _Out_opt_impl_)

#define _Out_writes_(size)                     _SAL2_Source_(_Out_writes_, (size), _Pre_cap_(size)            _Post_valid_impl_)
#define _Out_writes_opt_(size)                 _SAL2_Source_(_Out_writes_opt_, (size), _Pre_opt_cap_(size)        _Post_valid_impl_)
#define _Out_writes_bytes_(size)               _SAL2_Source_(_Out_writes_bytes_, (size), _Pre_bytecap_(size)        _Post_valid_impl_)
#define _Out_writes_bytes_opt_(size)           _SAL2_Source_(_Out_writes_bytes_opt_, (size), _Pre_opt_bytecap_(size)    _Post_valid_impl_)
#define _Out_writes_z_(size)                   _SAL2_Source_(_Out_writes_z_, (size), _Pre_cap_(size)            _Post_valid_impl_ _Post_z_)
#define _Out_writes_opt_z_(size)               _SAL2_Source_(_Out_writes_opt_z_, (size), _Pre_opt_cap_(size)        _Post_valid_impl_ _Post_z_)

#define _Out_writes_to_(size,count)            _SAL2_Source_(_Out_writes_to_, (size,count), _Pre_cap_(size)            _Post_valid_impl_ _Post_count_(count))
#define _Out_writes_to_opt_(size,count)        _SAL2_Source_(_Out_writes_to_opt_, (size,count), _Pre_opt_cap_(size)        _Post_valid_impl_ _Post_count_(count))
#define _Out_writes_all_(size)                 _SAL2_Source_(_Out_writes_all_, (size), _Out_writes_to_(_Old_(size), _Old_(size)))
#define _Out_writes_all_opt_(size)             _SAL2_Source_(_Out_writes_all_opt_, (size), _Out_writes_to_opt_(_Old_(size), _Old_(size)))

#define _Out_writes_bytes_to_(size,count)      _SAL2_Source_(_Out_writes_bytes_to_, (size,count), _Pre_bytecap_(size)        _Post_valid_impl_ _Post_bytecount_(count))
#define _Out_writes_bytes_to_opt_(size,count)  _SAL2_Source_(_Out_writes_bytes_to_opt_, (size,count), _Pre_opt_bytecap_(size) _Post_valid_impl_ _Post_bytecount_(count))
#define _Out_writes_bytes_all_(size)           _SAL2_Source_(_Out_writes_bytes_all_, (size), _Out_writes_bytes_to_(_Old_(size), _Old_(size)))
#define _Out_writes_bytes_all_opt_(size)       _SAL2_Source_(_Out_writes_bytes_all_opt_, (size), _Out_writes_bytes_to_opt_(_Old_(size), _Old_(size)))

#define _Out_writes_to_ptr_(ptr)               _SAL2_Source_(_Out_writes_to_ptr_, (ptr), _Pre_ptrdiff_cap_(ptr)     _Post_valid_impl_)
#define _Out_writes_to_ptr_opt_(ptr)           _SAL2_Source_(_Out_writes_to_ptr_opt_, (ptr), _Pre_opt_ptrdiff_cap_(ptr) _Post_valid_impl_)
#define _Out_writes_to_ptr_z_(ptr)             _SAL2_Source_(_Out_writes_to_ptr_z_, (ptr), _Pre_ptrdiff_cap_(ptr)     _Post_valid_impl_ Post_z_)
#define _Out_writes_to_ptr_opt_z_(ptr)         _SAL2_Source_(_Out_writes_to_ptr_opt_z_, (ptr), _Pre_opt_ptrdiff_cap_(ptr) _Post_valid_impl_ Post_z_)

#define _Inout_                                _SAL2_Source_(_Inout_, (), _Prepost_valid_)
#define _Inout_opt_                            _SAL2_Source_(_Inout_opt_, (), _Prepost_opt_valid_)

#define _Inout_z_                              _SAL2_Source_(_Inout_z_, (), _Prepost_z_)
#define _Inout_opt_z_                          _SAL2_Source_(_Inout_opt_z_, (), _Prepost_opt_z_)

#define _Inout_updates_(size)                  _SAL2_Source_(_Inout_updates_, (size), _Pre_cap_(size)         _Pre_valid_impl_ _Post_valid_impl_)
#define _Inout_updates_opt_(size)              _SAL2_Source_(_Inout_updates_opt_, (size), _Pre_opt_cap_(size)     _Pre_valid_impl_ _Post_valid_impl_)
#define _Inout_updates_z_(size)                _SAL2_Source_(_Inout_updates_z_, (size), _Pre_cap_(size)         _Pre_valid_impl_ _Post_valid_impl_ _Pre1_impl_(__zterm_impl) _Post1_impl_(__zterm_impl))
#define _Inout_updates_opt_z_(size)            _SAL2_Source_(_Inout_updates_opt_z_, (size), _Pre_opt_cap_(size)     _Pre_valid_impl_ _Post_valid_impl_ _Pre1_impl_(__zterm_impl) _Post1_impl_(__zterm_impl))

#define _Inout_updates_to_(size,count)         _SAL2_Source_(_Inout_updates_to_, (size,count), _Out_writes_to_(size,count) _Pre_valid_impl_ _Pre1_impl_(__count_impl(size)))
#define _Inout_updates_to_opt_(size,count)     _SAL2_Source_(_Inout_updates_to_opt_, (size,count), _Out_writes_to_opt_(size,count) _Pre_valid_impl_ _Pre1_impl_(__count_impl(size)))

#define _Inout_updates_all_(size)              _SAL2_Source_(_Inout_updates_all_, (size), _Inout_updates_to_(_Old_(size), _Old_(size)))
#define _Inout_updates_all_opt_(size)          _SAL2_Source_(_Inout_updates_all_opt_, (size), _Inout_updates_to_opt_(_Old_(size), _Old_(size)))

#define _Inout_updates_bytes_(size)            _SAL2_Source_(_Inout_updates_bytes_, (size), _Pre_bytecap_(size)     _Pre_valid_impl_ _Post_valid_impl_)
#define _Inout_updates_bytes_opt_(size)        _SAL2_Source_(_Inout_updates_bytes_opt_, (size), _Pre_opt_bytecap_(size) _Pre_valid_impl_ _Post_valid_impl_)

#define _Inout_updates_bytes_to_(size,count)       _SAL2_Source_(_Inout_updates_bytes_to_, (size,count), _Out_writes_bytes_to_(size,count) _Pre_valid_impl_ _Pre1_impl_(__bytecount_impl(size)))
#define _Inout_updates_bytes_to_opt_(size,count)   _SAL2_Source_(_Inout_updates_bytes_to_opt_, (size,count), _Out_writes_bytes_to_opt_(size,count) _Pre_valid_impl_ _Pre1_impl_(__bytecount_impl(size)))

#define _Inout_updates_bytes_all_(size)        _SAL2_Source_(_Inout_updates_bytes_all_, (size), _Inout_updates_bytes_to_(_Old_(size), _Old_(size)))
#define _Inout_updates_bytes_all_opt_(size)    _SAL2_Source_(_Inout_updates_bytes_all_opt_, (size), _Inout_updates_bytes_to_opt_(_Old_(size), _Old_(size)))

#define _Outptr_                         _SAL2_Source_(_Outptr_, (),                      _Out_impl_     _Deref_post2_impl_(__notnull_impl_notref,   __count_impl(1)))
#define _Outptr_result_maybenull_        _SAL2_Source_(_Outptr_result_maybenull_, (),     _Out_impl_     _Deref_post2_impl_(__maybenull_impl_notref, __count_impl(1)))
#define _Outptr_opt_                     _SAL2_Source_(_Outptr_opt_, (),                  _Out_opt_impl_ _Deref_post2_impl_(__notnull_impl_notref,   __count_impl(1)))
#define _Outptr_opt_result_maybenull_    _SAL2_Source_(_Outptr_opt_result_maybenull_, (), _Out_opt_impl_ _Deref_post2_impl_(__maybenull_impl_notref, __count_impl(1)))

#define _Outptr_result_z_                _SAL2_Source_(_Outptr_result_z_, (),               _Out_impl_     _Deref_post_z_)
#define _Outptr_opt_result_z_            _SAL2_Source_(_Outptr_opt_result_z_, (),           _Out_opt_impl_ _Deref_post_z_)
#define _Outptr_result_maybenull_z_      _SAL2_Source_(_Outptr_result_maybenull_z_, (),     _Out_impl_     _Deref_post_opt_z_)
#define _Outptr_opt_result_maybenull_z_  _SAL2_Source_(_Outptr_opt_result_maybenull_z_, (), _Out_opt_impl_ _Deref_post_opt_z_)

#define _Outptr_result_nullonfailure_       _SAL2_Source_(_Outptr_result_nullonfailure_, (),     _Outptr_      _On_failure_(_Deref_post_null_))
#define _Outptr_opt_result_nullonfailure_   _SAL2_Source_(_Outptr_opt_result_nullonfailure_, (), _Outptr_opt_  _On_failure_(_Deref_post_null_))

#define _COM_Outptr_                        _SAL2_Source_(_COM_Outptr_, (),                      _Outptr_                      _On_failure_(_Deref_post_null_))
#define _COM_Outptr_result_maybenull_       _SAL2_Source_(_COM_Outptr_result_maybenull_, (),     _Outptr_result_maybenull_     _On_failure_(_Deref_post_null_))
#define _COM_Outptr_opt_                    _SAL2_Source_(_COM_Outptr_opt_, (),                  _Outptr_opt_                  _On_failure_(_Deref_post_null_))
#define _COM_Outptr_opt_result_maybenull_   _SAL2_Source_(_COM_Outptr_opt_result_maybenull_, (), _Outptr_opt_result_maybenull_ _On_failure_(_Deref_post_null_))

#define _Outptr_result_buffer_(size)                      _SAL2_Source_(_Outptr_result_buffer_, (size),               _Out_impl_     _Deref_post2_impl_(__notnull_impl_notref, __cap_impl(size)))
#define _Outptr_opt_result_buffer_(size)                  _SAL2_Source_(_Outptr_opt_result_buffer_, (size),           _Out_opt_impl_ _Deref_post2_impl_(__notnull_impl_notref, __cap_impl(size)))
#define _Outptr_result_buffer_to_(size, count)            _SAL2_Source_(_Outptr_result_buffer_to_, (size, count),     _Out_impl_     _Deref_post3_impl_(__notnull_impl_notref, __cap_impl(size), __count_impl(count)))
#define _Outptr_opt_result_buffer_to_(size, count)        _SAL2_Source_(_Outptr_opt_result_buffer_to_, (size, count), _Out_opt_impl_ _Deref_post3_impl_(__notnull_impl_notref, __cap_impl(size), __count_impl(count)))

#define _Outptr_result_buffer_all_(size)                  _SAL2_Source_(_Outptr_result_buffer_all_, (size),           _Out_impl_     _Deref_post2_impl_(__notnull_impl_notref, __count_impl(size)))
#define _Outptr_opt_result_buffer_all_(size)              _SAL2_Source_(_Outptr_opt_result_buffer_all_, (size),       _Out_opt_impl_ _Deref_post2_impl_(__notnull_impl_notref, __count_impl(size)))

#define _Outptr_result_buffer_maybenull_(size)               _SAL2_Source_(_Outptr_result_buffer_maybenull_, (size),               _Out_impl_     _Deref_post2_impl_(__maybenull_impl_notref, __cap_impl(size)))
#define _Outptr_opt_result_buffer_maybenull_(size)           _SAL2_Source_(_Outptr_opt_result_buffer_maybenull_, (size),           _Out_opt_impl_ _Deref_post2_impl_(__maybenull_impl_notref, __cap_impl(size)))
#define _Outptr_result_buffer_to_maybenull_(size, count)     _SAL2_Source_(_Outptr_result_buffer_to_maybenull_, (size, count),     _Out_impl_     _Deref_post3_impl_(__maybenull_impl_notref, __cap_impl(size), __count_impl(count)))
#define _Outptr_opt_result_buffer_to_maybenull_(size, count) _SAL2_Source_(_Outptr_opt_result_buffer_to_maybenull_, (size, count), _Out_opt_impl_ _Deref_post3_impl_(__maybenull_impl_notref, __cap_impl(size), __count_impl(count)))

#define _Outptr_result_buffer_all_maybenull_(size)           _SAL2_Source_(_Outptr_result_buffer_all_maybenull_, (size),           _Out_impl_     _Deref_post2_impl_(__maybenull_impl_notref, __count_impl(size)))
#define _Outptr_opt_result_buffer_all_maybenull_(size)       _SAL2_Source_(_Outptr_opt_result_buffer_all_maybenull_, (size),       _Out_opt_impl_ _Deref_post2_impl_(__maybenull_impl_notref, __count_impl(size)))

#define _Outptr_result_bytebuffer_(size)                     _SAL2_Source_(_Outptr_result_bytebuffer_, (size),                     _Out_impl_     _Deref_post2_impl_(__notnull_impl_notref, __bytecap_impl(size)))
#define _Outptr_opt_result_bytebuffer_(size)                 _SAL2_Source_(_Outptr_opt_result_bytebuffer_, (size),                 _Out_opt_impl_ _Deref_post2_impl_(__notnull_impl_notref, __bytecap_impl(size)))
#define _Outptr_result_bytebuffer_to_(size, count)           _SAL2_Source_(_Outptr_result_bytebuffer_to_, (size, count),           _Out_impl_     _Deref_post3_impl_(__notnull_impl_notref, __bytecap_impl(size), __bytecount_impl(count)))
#define _Outptr_opt_result_bytebuffer_to_(size, count)       _SAL2_Source_(_Outptr_opt_result_bytebuffer_to_, (size, count),       _Out_opt_impl_ _Deref_post3_impl_(__notnull_impl_notref, __bytecap_impl(size), __bytecount_impl(count)))

#define _Outptr_result_bytebuffer_all_(size)                 _SAL2_Source_(_Outptr_result_bytebuffer_all_, (size),                 _Out_impl_     _Deref_post2_impl_(__notnull_impl_notref, __bytecount_impl(size)))
#define _Outptr_opt_result_bytebuffer_all_(size)             _SAL2_Source_(_Outptr_opt_result_bytebuffer_all_, (size),             _Out_opt_impl_ _Deref_post2_impl_(__notnull_impl_notref, __bytecount_impl(size)))

#define _Outptr_result_bytebuffer_maybenull_(size)                 _SAL2_Source_(_Outptr_result_bytebuffer_maybenull_, (size),               _Out_impl_     _Deref_post2_impl_(__maybenull_impl_notref, __bytecap_impl(size)))
#define _Outptr_opt_result_bytebuffer_maybenull_(size)             _SAL2_Source_(_Outptr_opt_result_bytebuffer_maybenull_, (size),           _Out_opt_impl_ _Deref_post2_impl_(__maybenull_impl_notref, __bytecap_impl(size)))
#define _Outptr_result_bytebuffer_to_maybenull_(size, count)       _SAL2_Source_(_Outptr_result_bytebuffer_to_maybenull_, (size, count),     _Out_impl_     _Deref_post3_impl_(__maybenull_impl_notref, __bytecap_impl(size), __bytecount_impl(count)))
#define _Outptr_opt_result_bytebuffer_to_maybenull_(size, count)   _SAL2_Source_(_Outptr_opt_result_bytebuffer_to_maybenull_, (size, count), _Out_opt_impl_ _Deref_post3_impl_(__maybenull_impl_notref, __bytecap_impl(size), __bytecount_impl(count)))

#define _Outptr_result_bytebuffer_all_maybenull_(size)         _SAL2_Source_(_Outptr_result_bytebuffer_all_maybenull_, (size),               _Out_impl_     _Deref_post2_impl_(__maybenull_impl_notref, __bytecount_impl(size)))
#define _Outptr_opt_result_bytebuffer_all_maybenull_(size)     _SAL2_Source_(_Outptr_opt_result_bytebuffer_all_maybenull_, (size),           _Out_opt_impl_ _Deref_post2_impl_(__maybenull_impl_notref, __bytecount_impl(size)))

#define _Outref_                                               _SAL2_Source_(_Outref_, (),                  _Out_impl_ _Post_notnull_)
#define _Outref_result_maybenull_                              _SAL2_Source_(_Outref_result_maybenull_, (), _Pre2_impl_(__notnull_impl_notref, __cap_c_one_notref_impl) _Post_maybenull_ _Post_valid_impl_)

#define _Outref_result_buffer_(size)                           _SAL2_Source_(_Outref_result_buffer_, (size),                         _Outref_ _Post1_impl_(__cap_impl(size)))
#define _Outref_result_bytebuffer_(size)                       _SAL2_Source_(_Outref_result_bytebuffer_, (size),                     _Outref_ _Post1_impl_(__bytecap_impl(size)))
#define _Outref_result_buffer_to_(size, count)                 _SAL2_Source_(_Outref_result_buffer_to_, (size, count),               _Outref_result_buffer_(size) _Post1_impl_(__count_impl(count)))
#define _Outref_result_bytebuffer_to_(size, count)             _SAL2_Source_(_Outref_result_bytebuffer_to_, (size, count),           _Outref_result_bytebuffer_(size) _Post1_impl_(__bytecount_impl(count)))
#define _Outref_result_buffer_all_(size)                       _SAL2_Source_(_Outref_result_buffer_all_, (size),                     _Outref_result_buffer_to_(size, _Old_(size)))
#define _Outref_result_bytebuffer_all_(size)                   _SAL2_Source_(_Outref_result_bytebuffer_all_, (size),                 _Outref_result_bytebuffer_to_(size, _Old_(size)))

#define _Outref_result_buffer_maybenull_(size)                 _SAL2_Source_(_Outref_result_buffer_maybenull_, (size),               _Outref_result_maybenull_ _Post1_impl_(__cap_impl(size)))
#define _Outref_result_bytebuffer_maybenull_(size)             _SAL2_Source_(_Outref_result_bytebuffer_maybenull_, (size),           _Outref_result_maybenull_ _Post1_impl_(__bytecap_impl(size)))
#define _Outref_result_buffer_to_maybenull_(size, count)       _SAL2_Source_(_Outref_result_buffer_to_maybenull_, (size, count),     _Outref_result_buffer_maybenull_(size) _Post1_impl_(__count_impl(count)))
#define _Outref_result_bytebuffer_to_maybenull_(size, count)   _SAL2_Source_(_Outref_result_bytebuffer_to_maybenull_, (size, count), _Outref_result_bytebuffer_maybenull_(size) _Post1_impl_(__bytecount_impl(count)))
#define _Outref_result_buffer_all_maybenull_(size)             _SAL2_Source_(_Outref_result_buffer_all_maybenull_, (size),           _Outref_result_buffer_to_maybenull_(size, _Old_(size)))
#define _Outref_result_bytebuffer_all_maybenull_(size)         _SAL2_Source_(_Outref_result_bytebuffer_all_maybenull_, (size),       _Outref_result_bytebuffer_to_maybenull_(size, _Old_(size)))

#define _Outref_result_nullonfailure_                          _SAL2_Source_(_Outref_result_nullonfailure_, (), _Outref_    _On_failure_(_Post_null_))

#define _Result_nullonfailure_                                 _SAL2_Source_(_Result_nullonfailure_, (), _On_failure_(_Notref_impl_ _Deref_impl_ _Post_null_))
#define _Result_zeroonfailure_                                 _SAL2_Source_(_Result_zeroonfailure_, (), _On_failure_(_Notref_impl_ _Deref_impl_ _Out_range_(==, 0)))

#define _Ret_z_                             _SAL2_Source_(_Ret_z_, (), _Ret2_impl_(__notnull_impl,  __zterm_impl) _Ret_valid_impl_)
#define _Ret_maybenull_z_                   _SAL2_Source_(_Ret_maybenull_z_, (), _Ret2_impl_(__maybenull_impl,__zterm_impl) _Ret_valid_impl_)

#define _Ret_notnull_                       _SAL2_Source_(_Ret_notnull_, (), _Ret1_impl_(__notnull_impl))
#define _Ret_maybenull_                     _SAL2_Source_(_Ret_maybenull_, (), _Ret1_impl_(__maybenull_impl))
#define _Ret_null_                          _SAL2_Source_(_Ret_null_, (), _Ret1_impl_(__null_impl))

#define _Ret_valid_                         _SAL2_Source_(_Ret_valid_, (), _Ret1_impl_(__notnull_impl_notref)   _Ret_valid_impl_)

#define _Ret_writes_(size)                  _SAL2_Source_(_Ret_writes_, (size), _Ret2_impl_(__notnull_impl,  __count_impl(size))          _Ret_valid_impl_)
#define _Ret_writes_z_(size)                _SAL2_Source_(_Ret_writes_z_, (size), _Ret3_impl_(__notnull_impl,  __count_impl(size), __zterm_impl) _Ret_valid_impl_)
#define _Ret_writes_bytes_(size)            _SAL2_Source_(_Ret_writes_bytes_, (size), _Ret2_impl_(__notnull_impl,  __bytecount_impl(size))      _Ret_valid_impl_)
#define _Ret_writes_maybenull_(size)        _SAL2_Source_(_Ret_writes_maybenull_, (size), _Ret2_impl_(__maybenull_impl,__count_impl(size))          _Ret_valid_impl_)
#define _Ret_writes_maybenull_z_(size)      _SAL2_Source_(_Ret_writes_maybenull_z_, (size), _Ret3_impl_(__maybenull_impl,__count_impl(size),__zterm_impl)  _Ret_valid_impl_)
#define _Ret_writes_bytes_maybenull_(size)  _SAL2_Source_(_Ret_writes_bytes_maybenull_, (size), _Ret2_impl_(__maybenull_impl,__bytecount_impl(size))      _Ret_valid_impl_)

#define _Ret_writes_to_(size,count)                   _SAL2_Source_(_Ret_writes_to_, (size,count), _Ret3_impl_(__notnull_impl,  __cap_impl(size),     __count_impl(count))     _Ret_valid_impl_)
#define _Ret_writes_bytes_to_(size,count)             _SAL2_Source_(_Ret_writes_bytes_to_, (size,count), _Ret3_impl_(__notnull_impl,  __bytecap_impl(size), __bytecount_impl(count)) _Ret_valid_impl_)
#define _Ret_writes_to_maybenull_(size,count)         _SAL2_Source_(_Ret_writes_to_maybenull_, (size,count), _Ret3_impl_(__maybenull_impl,  __cap_impl(size),     __count_impl(count))     _Ret_valid_impl_)
#define _Ret_writes_bytes_to_maybenull_(size,count)   _SAL2_Source_(_Ret_writes_bytes_to_maybenull_, (size,count), _Ret3_impl_(__maybenull_impl,  __bytecap_impl(size), __bytecount_impl(count)) _Ret_valid_impl_)

#define _Points_to_data_        _SAL2_Source_(_Points_to_data_, (), _Pre_ _Points_to_data_impl_)
#define _Literal_               _SAL2_Source_(_Literal_, (), _Pre_ _Literal_impl_)
#define _Notliteral_            _SAL2_Source_(_Notliteral_, (), _Pre_ _Notliteral_impl_)

#define _Must_inspect_result_    _SAL2_Source_(_Must_inspect_result_, (), _Must_inspect_impl_ _Check_return_impl_)

#define _Printf_format_string_  _SAL2_Source_(_Printf_format_string_, (), _Printf_format_string_impl_)
#define _Scanf_format_string_   _SAL2_Source_(_Scanf_format_string_, (), _Scanf_format_string_impl_)
#define _Scanf_s_format_string_  _SAL2_Source_(_Scanf_s_format_string_, (), _Scanf_s_format_string_impl_)

#define _Format_string_impl_(kind,where)  _SA_annotes2(SAL_IsFormatString2, kind, where)
#define _Printf_format_string_params_(x)  _SAL2_Source_(_Printf_format_string_params_, (x), _Format_string_impl_("printf", x))
#define _Scanf_format_string_params_(x)   _SAL2_Source_(_Scanf_format_string_params_, (x), _Format_string_impl_("scanf", x))
#define _Scanf_s_format_string_params_(x) _SAL2_Source_(_Scanf_s_format_string_params_, (x), _Format_string_impl_("scanf_s", x))

#define _In_range_(lb,ub)           _SAL2_Source_(_In_range_, (lb,ub), _In_range_impl_(lb,ub))
#define _Out_range_(lb,ub)          _SAL2_Source_(_Out_range_, (lb,ub), _Out_range_impl_(lb,ub))
#define _Ret_range_(lb,ub)          _SAL2_Source_(_Ret_range_, (lb,ub), _Ret_range_impl_(lb,ub))
#define _Deref_in_range_(lb,ub)     _SAL2_Source_(_Deref_in_range_, (lb,ub), _Deref_in_range_impl_(lb,ub))
#define _Deref_out_range_(lb,ub)    _SAL2_Source_(_Deref_out_range_, (lb,ub), _Deref_out_range_impl_(lb,ub))
#define _Deref_ret_range_(lb,ub)    _SAL2_Source_(_Deref_ret_range_, (lb,ub), _Deref_ret_range_impl_(lb,ub))
#define _Pre_equal_to_(expr)        _SAL2_Source_(_Pre_equal_to_, (expr), _In_range_(==, expr))
#define _Post_equal_to_(expr)       _SAL2_Source_(_Post_equal_to_, (expr), _Out_range_(==, expr))

#define _Unchanged_(e)              _SAL2_Source_(_Unchanged_, (e), _At_(e, _Post_equal_to_(_Old_(e)) _Const_))

#define _Pre_satisfies_(cond)       _SAL2_Source_(_Pre_satisfies_, (cond), _Pre_satisfies_impl_(cond))
#define _Post_satisfies_(cond)      _SAL2_Source_(_Post_satisfies_, (cond), _Post_satisfies_impl_(cond))

#define _Struct_size_bytes_(size)                  _SAL2_Source_(_Struct_size_bytes_, (size), _Writable_bytes_(size))

#define _Field_size_(size)                         _SAL2_Source_(_Field_size_, (size), _Notnull_   _Writable_elements_(size))
#define _Field_size_opt_(size)                     _SAL2_Source_(_Field_size_opt_, (size), _Maybenull_ _Writable_elements_(size))
#define _Field_size_part_(size, count)             _SAL2_Source_(_Field_size_part_, (size, count), _Notnull_   _Writable_elements_(size) _Readable_elements_(count))
#define _Field_size_part_opt_(size, count)         _SAL2_Source_(_Field_size_part_opt_, (size, count), _Maybenull_ _Writable_elements_(size) _Readable_elements_(count))
#define _Field_size_full_(size)                    _SAL2_Source_(_Field_size_full_, (size), _Field_size_part_(size, size))
#define _Field_size_full_opt_(size)                _SAL2_Source_(_Field_size_full_opt_, (size), _Field_size_part_opt_(size, size))

#define _Field_size_bytes_(size)                   _SAL2_Source_(_Field_size_bytes_, (size), _Notnull_   _Writable_bytes_(size))
#define _Field_size_bytes_opt_(size)               _SAL2_Source_(_Field_size_bytes_opt_, (size), _Maybenull_ _Writable_bytes_(size))
#define _Field_size_bytes_part_(size, count)       _SAL2_Source_(_Field_size_bytes_part_, (size, count), _Notnull_   _Writable_bytes_(size) _Readable_bytes_(count))
#define _Field_size_bytes_part_opt_(size, count)   _SAL2_Source_(_Field_size_bytes_part_opt_, (size, count), _Maybenull_ _Writable_bytes_(size) _Readable_bytes_(count))
#define _Field_size_bytes_full_(size)              _SAL2_Source_(_Field_size_bytes_full_, (size), _Field_size_bytes_part_(size, size))
#define _Field_size_bytes_full_opt_(size)          _SAL2_Source_(_Field_size_bytes_full_opt_, (size), _Field_size_bytes_part_opt_(size, size))

#define _Field_z_                                  _SAL2_Source_(_Field_z_, (), _Null_terminated_)

#define _Field_range_(min,max)                     _SAL2_Source_(_Field_range_, (min,max), _Field_range_impl_(min,max))

#define _Pre_                             _Pre_impl_
#define _Post_                            _Post_impl_

#define _Valid_                           _Valid_impl_
#define _Notvalid_                        _Notvalid_impl_
#define _Maybevalid_                      _Maybevalid_impl_

#define _Readable_bytes_(size)            _SAL2_Source_(_Readable_bytes_, (size), _Readable_bytes_impl_(size))
#define _Readable_elements_(size)         _SAL2_Source_(_Readable_elements_, (size), _Readable_elements_impl_(size))
#define _Writable_bytes_(size)            _SAL2_Source_(_Writable_bytes_, (size), _Writable_bytes_impl_(size))
#define _Writable_elements_(size)         _SAL2_Source_(_Writable_elements_, (size), _Writable_elements_impl_(size))

#define _Null_terminated_                 _SAL2_Source_(_Null_terminated_, (), _Null_terminated_impl_)
#define _NullNull_terminated_             _SAL2_Source_(_NullNull_terminated_, (), _NullNull_terminated_impl_)

#define _Pre_readable_size_(size)         _SAL2_Source_(_Pre_readable_size_, (size), _Pre1_impl_(__count_impl(size))      _Pre_valid_impl_)
#define _Pre_writable_size_(size)         _SAL2_Source_(_Pre_writable_size_, (size), _Pre1_impl_(__cap_impl(size)))
#define _Pre_readable_byte_size_(size)    _SAL2_Source_(_Pre_readable_byte_size_, (size), _Pre1_impl_(__bytecount_impl(size))  _Pre_valid_impl_)
#define _Pre_writable_byte_size_(size)    _SAL2_Source_(_Pre_writable_byte_size_, (size), _Pre1_impl_(__bytecap_impl(size)))

#define _Post_readable_size_(size)        _SAL2_Source_(_Post_readable_size_, (size), _Post1_impl_(__count_impl(size))     _Post_valid_impl_)
#define _Post_writable_size_(size)        _SAL2_Source_(_Post_writable_size_, (size), _Post1_impl_(__cap_impl(size)))
#define _Post_readable_byte_size_(size)   _SAL2_Source_(_Post_readable_byte_size_, (size), _Post1_impl_(__bytecount_impl(size)) _Post_valid_impl_)
#define _Post_writable_byte_size_(size)   _SAL2_Source_(_Post_writable_byte_size_, (size), _Post1_impl_(__bytecap_impl(size)))

#define _Null_                            _SAL2_Source_(_Null_, (), _Null_impl_)
#define _Notnull_                         _SAL2_Source_(_Notnull_, (), _Notnull_impl_)
#define _Maybenull_                       _SAL2_Source_(_Maybenull_, (), _Maybenull_impl_)

#define _Pre_z_                           _SAL2_Source_(_Pre_z_, (), _Pre1_impl_(__zterm_impl) _Pre_valid_impl_)

#define _Pre_valid_                       _SAL2_Source_(_Pre_valid_, (), _Pre1_impl_(__notnull_impl_notref)   _Pre_valid_impl_)
#define _Pre_opt_valid_                   _SAL2_Source_(_Pre_opt_valid_, (), _Pre1_impl_(__maybenull_impl_notref) _Pre_valid_impl_)

#define _Pre_invalid_                     _SAL2_Source_(_Pre_invalid_, (), _Deref_pre1_impl_(__notvalid_impl))

#define _Pre_unknown_                     _SAL2_Source_(_Pre_unknown_, (), _Pre1_impl_(__maybevalid_impl))

#define _Pre_notnull_                     _SAL2_Source_(_Pre_notnull_, (), _Pre1_impl_(__notnull_impl_notref))
#define _Pre_maybenull_                   _SAL2_Source_(_Pre_maybenull_, (), _Pre1_impl_(__maybenull_impl_notref))
#define _Pre_null_                        _SAL2_Source_(_Pre_null_, (), _Pre1_impl_(__null_impl_notref))

#define _Post_z_                         _SAL2_Source_(_Post_z_, (), _Post1_impl_(__zterm_impl) _Post_valid_impl_)

#define _Post_valid_                     _SAL2_Source_(_Post_valid_, (), _Post_valid_impl_)
#define _Post_invalid_                   _SAL2_Source_(_Post_invalid_, (), _Deref_post1_impl_(__notvalid_impl))

#define _Post_ptr_invalid_               _SAL2_Source_(_Post_ptr_invalid_, (), _Post1_impl_(__notvalid_impl))

#define _Post_notnull_                   _SAL2_Source_(_Post_notnull_, (), _Post1_impl_(__notnull_impl))

#define _Post_null_                      _SAL2_Source_(_Post_null_, (), _Post1_impl_(__null_impl))

#define _Post_maybenull_                 _SAL2_Source_(_Post_maybenull_, (), _Post1_impl_(__maybenull_impl))

#define _Prepost_z_                      _SAL2_Source_(_Prepost_z_, (), _Pre_z_      _Post_z_)


#pragma region Input Buffer SAL 1 compatibility macros

#define _In_count_(size)               _SAL1_1_Source_(_In_count_, (size), _Pre_count_(size)         _Deref_pre_readonly_)
#define _In_opt_count_(size)           _SAL1_1_Source_(_In_opt_count_, (size), _Pre_opt_count_(size)     _Deref_pre_readonly_)
#define _In_bytecount_(size)           _SAL1_1_Source_(_In_bytecount_, (size), _Pre_bytecount_(size)     _Deref_pre_readonly_)
#define _In_opt_bytecount_(size)       _SAL1_1_Source_(_In_opt_bytecount_, (size), _Pre_opt_bytecount_(size) _Deref_pre_readonly_)

#define _In_count_c_(size)             _SAL1_1_Source_(_In_count_c_, (size), _Pre_count_c_(size)         _Deref_pre_readonly_)
#define _In_opt_count_c_(size)         _SAL1_1_Source_(_In_opt_count_c_, (size), _Pre_opt_count_c_(size)     _Deref_pre_readonly_)
#define _In_bytecount_c_(size)         _SAL1_1_Source_(_In_bytecount_c_, (size), _Pre_bytecount_c_(size)     _Deref_pre_readonly_)
#define _In_opt_bytecount_c_(size)     _SAL1_1_Source_(_In_opt_bytecount_c_, (size), _Pre_opt_bytecount_c_(size) _Deref_pre_readonly_)

#define _In_z_count_(size)               _SAL1_1_Source_(_In_z_count_, (size), _Pre_z_ _Pre_count_(size)         _Deref_pre_readonly_)
#define _In_opt_z_count_(size)           _SAL1_1_Source_(_In_opt_z_count_, (size), _Pre_opt_z_ _Pre_opt_count_(size)     _Deref_pre_readonly_)
#define _In_z_bytecount_(size)           _SAL1_1_Source_(_In_z_bytecount_, (size), _Pre_z_ _Pre_bytecount_(size)     _Deref_pre_readonly_)
#define _In_opt_z_bytecount_(size)       _SAL1_1_Source_(_In_opt_z_bytecount_, (size), _Pre_opt_z_ _Pre_opt_bytecount_(size) _Deref_pre_readonly_)

#define _In_z_count_c_(size)             _SAL1_1_Source_(_In_z_count_c_, (size), _Pre_z_ _Pre_count_c_(size)         _Deref_pre_readonly_)
#define _In_opt_z_count_c_(size)         _SAL1_1_Source_(_In_opt_z_count_c_, (size), _Pre_opt_z_ _Pre_opt_count_c_(size)     _Deref_pre_readonly_)
#define _In_z_bytecount_c_(size)         _SAL1_1_Source_(_In_z_bytecount_c_, (size), _Pre_z_ _Pre_bytecount_c_(size)     _Deref_pre_readonly_)
#define _In_opt_z_bytecount_c_(size)     _SAL1_1_Source_(_In_opt_z_bytecount_c_, (size), _Pre_opt_z_ _Pre_opt_bytecount_c_(size) _Deref_pre_readonly_)

#define _In_ptrdiff_count_(size)       _SAL1_1_Source_(_In_ptrdiff_count_, (size), _Pre_ptrdiff_count_(size)     _Deref_pre_readonly_)
#define _In_opt_ptrdiff_count_(size)   _SAL1_1_Source_(_In_opt_ptrdiff_count_, (size), _Pre_opt_ptrdiff_count_(size) _Deref_pre_readonly_)

#define _In_count_x_(size)             _SAL1_1_Source_(_In_count_x_, (size), _Pre_count_x_(size)         _Deref_pre_readonly_)
#define _In_opt_count_x_(size)         _SAL1_1_Source_(_In_opt_count_x_, (size), _Pre_opt_count_x_(size)     _Deref_pre_readonly_)
#define _In_bytecount_x_(size)         _SAL1_1_Source_(_In_bytecount_x_, (size), _Pre_bytecount_x_(size)     _Deref_pre_readonly_)
#define _In_opt_bytecount_x_(size)     _SAL1_1_Source_(_In_opt_bytecount_x_, (size), _Pre_opt_bytecount_x_(size) _Deref_pre_readonly_)

#define _Out_cap_(size)                   _SAL1_1_Source_(_Out_cap_, (size), _Pre_cap_(size)           _Post_valid_impl_)
#define _Out_opt_cap_(size)               _SAL1_1_Source_(_Out_opt_cap_, (size), _Pre_opt_cap_(size)       _Post_valid_impl_)
#define _Out_bytecap_(size)               _SAL1_1_Source_(_Out_bytecap_, (size), _Pre_bytecap_(size)       _Post_valid_impl_)
#define _Out_opt_bytecap_(size)           _SAL1_1_Source_(_Out_opt_bytecap_, (size), _Pre_opt_bytecap_(size)   _Post_valid_impl_)

#define _Out_cap_c_(size)                 _SAL1_1_Source_(_Out_cap_c_, (size), _Pre_cap_c_(size)         _Post_valid_impl_)
#define _Out_opt_cap_c_(size)             _SAL1_1_Source_(_Out_opt_cap_c_, (size), _Pre_opt_cap_c_(size)     _Post_valid_impl_)
#define _Out_bytecap_c_(size)             _SAL1_1_Source_(_Out_bytecap_c_, (size), _Pre_bytecap_c_(size)     _Post_valid_impl_)
#define _Out_opt_bytecap_c_(size)         _SAL1_1_Source_(_Out_opt_bytecap_c_, (size), _Pre_opt_bytecap_c_(size) _Post_valid_impl_)

#define _Out_cap_m_(mult,size)            _SAL1_1_Source_(_Out_cap_m_, (mult,size), _Pre_cap_m_(mult,size)     _Post_valid_impl_)
#define _Out_opt_cap_m_(mult,size)        _SAL1_1_Source_(_Out_opt_cap_m_, (mult,size), _Pre_opt_cap_m_(mult,size) _Post_valid_impl_)
#define _Out_z_cap_m_(mult,size)          _SAL1_1_Source_(_Out_z_cap_m_, (mult,size), _Pre_cap_m_(mult,size)     _Post_valid_impl_ _Post_z_)
#define _Out_opt_z_cap_m_(mult,size)      _SAL1_1_Source_(_Out_opt_z_cap_m_, (mult,size), _Pre_opt_cap_m_(mult,size) _Post_valid_impl_ _Post_z_)

#define _Out_ptrdiff_cap_(size)           _SAL1_1_Source_(_Out_ptrdiff_cap_, (size), _Pre_ptrdiff_cap_(size)     _Post_valid_impl_)
#define _Out_opt_ptrdiff_cap_(size)       _SAL1_1_Source_(_Out_opt_ptrdiff_cap_, (size), _Pre_opt_ptrdiff_cap_(size) _Post_valid_impl_)

#define _Out_cap_x_(size)                 _SAL1_1_Source_(_Out_cap_x_, (size), _Pre_cap_x_(size)         _Post_valid_impl_)
#define _Out_opt_cap_x_(size)             _SAL1_1_Source_(_Out_opt_cap_x_, (size), _Pre_opt_cap_x_(size)     _Post_valid_impl_)
#define _Out_bytecap_x_(size)             _SAL1_1_Source_(_Out_bytecap_x_, (size), _Pre_bytecap_x_(size)     _Post_valid_impl_)
#define _Out_opt_bytecap_x_(size)         _SAL1_1_Source_(_Out_opt_bytecap_x_, (size), _Pre_opt_bytecap_x_(size) _Post_valid_impl_)

#define _Out_z_cap_(size)                 _SAL1_1_Source_(_Out_z_cap_, (size), _Pre_cap_(size)           _Post_valid_impl_ _Post_z_)
#define _Out_opt_z_cap_(size)             _SAL1_1_Source_(_Out_opt_z_cap_, (size), _Pre_opt_cap_(size)       _Post_valid_impl_ _Post_z_)
#define _Out_z_bytecap_(size)             _SAL1_1_Source_(_Out_z_bytecap_, (size), _Pre_bytecap_(size)       _Post_valid_impl_ _Post_z_)
#define _Out_opt_z_bytecap_(size)         _SAL1_1_Source_(_Out_opt_z_bytecap_, (size), _Pre_opt_bytecap_(size)   _Post_valid_impl_ _Post_z_)

#define _Out_z_cap_c_(size)               _SAL1_1_Source_(_Out_z_cap_c_, (size), _Pre_cap_c_(size)         _Post_valid_impl_ _Post_z_)
#define _Out_opt_z_cap_c_(size)           _SAL1_1_Source_(_Out_opt_z_cap_c_, (size), _Pre_opt_cap_c_(size)     _Post_valid_impl_ _Post_z_)
#define _Out_z_bytecap_c_(size)           _SAL1_1_Source_(_Out_z_bytecap_c_, (size), _Pre_bytecap_c_(size)     _Post_valid_impl_ _Post_z_)
#define _Out_opt_z_bytecap_c_(size)       _SAL1_1_Source_(_Out_opt_z_bytecap_c_, (size), _Pre_opt_bytecap_c_(size) _Post_valid_impl_ _Post_z_)

#define _Out_z_cap_x_(size)               _SAL1_1_Source_(_Out_z_cap_x_, (size), _Pre_cap_x_(size)         _Post_valid_impl_ _Post_z_)
#define _Out_opt_z_cap_x_(size)           _SAL1_1_Source_(_Out_opt_z_cap_x_, (size), _Pre_opt_cap_x_(size)     _Post_valid_impl_ _Post_z_)
#define _Out_z_bytecap_x_(size)           _SAL1_1_Source_(_Out_z_bytecap_x_, (size), _Pre_bytecap_x_(size)     _Post_valid_impl_ _Post_z_)
#define _Out_opt_z_bytecap_x_(size)       _SAL1_1_Source_(_Out_opt_z_bytecap_x_, (size), _Pre_opt_bytecap_x_(size) _Post_valid_impl_ _Post_z_)

#define _Out_cap_post_count_(cap,count)                _SAL1_1_Source_(_Out_cap_post_count_, (cap,count), _Pre_cap_(cap)         _Post_valid_impl_ _Post_count_(count))
#define _Out_opt_cap_post_count_(cap,count)            _SAL1_1_Source_(_Out_opt_cap_post_count_, (cap,count), _Pre_opt_cap_(cap)     _Post_valid_impl_ _Post_count_(count))
#define _Out_bytecap_post_bytecount_(cap,count)        _SAL1_1_Source_(_Out_bytecap_post_bytecount_, (cap,count), _Pre_bytecap_(cap)     _Post_valid_impl_ _Post_bytecount_(count))
#define _Out_opt_bytecap_post_bytecount_(cap,count)    _SAL1_1_Source_(_Out_opt_bytecap_post_bytecount_, (cap,count), _Pre_opt_bytecap_(cap) _Post_valid_impl_ _Post_bytecount_(count))

#define _Out_z_cap_post_count_(cap,count)               _SAL1_1_Source_(_Out_z_cap_post_count_, (cap,count), _Pre_cap_(cap)         _Post_valid_impl_ _Post_z_count_(count))
#define _Out_opt_z_cap_post_count_(cap,count)           _SAL1_1_Source_(_Out_opt_z_cap_post_count_, (cap,count), _Pre_opt_cap_(cap)     _Post_valid_impl_ _Post_z_count_(count))
#define _Out_z_bytecap_post_bytecount_(cap,count)       _SAL1_1_Source_(_Out_z_bytecap_post_bytecount_, (cap,count), _Pre_bytecap_(cap)     _Post_valid_impl_ _Post_z_bytecount_(count))
#define _Out_opt_z_bytecap_post_bytecount_(cap,count)   _SAL1_1_Source_(_Out_opt_z_bytecap_post_bytecount_, (cap,count), _Pre_opt_bytecap_(cap) _Post_valid_impl_ _Post_z_bytecount_(count))

#define _Out_capcount_(capcount)             _SAL1_1_Source_(_Out_capcount_, (capcount), _Pre_cap_(capcount)         _Post_valid_impl_ _Post_count_(capcount))
#define _Out_opt_capcount_(capcount)         _SAL1_1_Source_(_Out_opt_capcount_, (capcount), _Pre_opt_cap_(capcount)     _Post_valid_impl_ _Post_count_(capcount))
#define _Out_bytecapcount_(capcount)         _SAL1_1_Source_(_Out_bytecapcount_, (capcount), _Pre_bytecap_(capcount)     _Post_valid_impl_ _Post_bytecount_(capcount))
#define _Out_opt_bytecapcount_(capcount)     _SAL1_1_Source_(_Out_opt_bytecapcount_, (capcount), _Pre_opt_bytecap_(capcount) _Post_valid_impl_ _Post_bytecount_(capcount))

#define _Out_capcount_x_(capcount)           _SAL1_1_Source_(_Out_capcount_x_, (capcount), _Pre_cap_x_(capcount)         _Post_valid_impl_ _Post_count_x_(capcount))
#define _Out_opt_capcount_x_(capcount)       _SAL1_1_Source_(_Out_opt_capcount_x_, (capcount), _Pre_opt_cap_x_(capcount)     _Post_valid_impl_ _Post_count_x_(capcount))
#define _Out_bytecapcount_x_(capcount)       _SAL1_1_Source_(_Out_bytecapcount_x_, (capcount), _Pre_bytecap_x_(capcount)     _Post_valid_impl_ _Post_bytecount_x_(capcount))
#define _Out_opt_bytecapcount_x_(capcount)   _SAL1_1_Source_(_Out_opt_bytecapcount_x_, (capcount), _Pre_opt_bytecap_x_(capcount) _Post_valid_impl_ _Post_bytecount_x_(capcount))

#define _Out_z_capcount_(capcount)           _SAL1_1_Source_(_Out_z_capcount_, (capcount), _Pre_cap_(capcount)         _Post_valid_impl_ _Post_z_count_(capcount))
#define _Out_opt_z_capcount_(capcount)       _SAL1_1_Source_(_Out_opt_z_capcount_, (capcount), _Pre_opt_cap_(capcount)     _Post_valid_impl_ _Post_z_count_(capcount))
#define _Out_z_bytecapcount_(capcount)       _SAL1_1_Source_(_Out_z_bytecapcount_, (capcount), _Pre_bytecap_(capcount)     _Post_valid_impl_ _Post_z_bytecount_(capcount))
#define _Out_opt_z_bytecapcount_(capcount)   _SAL1_1_Source_(_Out_opt_z_bytecapcount_, (capcount), _Pre_opt_bytecap_(capcount) _Post_valid_impl_ _Post_z_bytecount_(capcount))

#define _Inout_count_(size)               _SAL1_1_Source_(_Inout_count_, (size), _Prepost_count_(size))
#define _Inout_opt_count_(size)           _SAL1_1_Source_(_Inout_opt_count_, (size), _Prepost_opt_count_(size))
#define _Inout_bytecount_(size)           _SAL1_1_Source_(_Inout_bytecount_, (size), _Prepost_bytecount_(size))
#define _Inout_opt_bytecount_(size)       _SAL1_1_Source_(_Inout_opt_bytecount_, (size), _Prepost_opt_bytecount_(size))

#define _Inout_count_c_(size)             _SAL1_1_Source_(_Inout_count_c_, (size), _Prepost_count_c_(size))
#define _Inout_opt_count_c_(size)         _SAL1_1_Source_(_Inout_opt_count_c_, (size), _Prepost_opt_count_c_(size))
#define _Inout_bytecount_c_(size)         _SAL1_1_Source_(_Inout_bytecount_c_, (size), _Prepost_bytecount_c_(size))
#define _Inout_opt_bytecount_c_(size)     _SAL1_1_Source_(_Inout_opt_bytecount_c_, (size), _Prepost_opt_bytecount_c_(size))

#define _Inout_z_count_(size)               _SAL1_1_Source_(_Inout_z_count_, (size), _Prepost_z_ _Prepost_count_(size))
#define _Inout_opt_z_count_(size)           _SAL1_1_Source_(_Inout_opt_z_count_, (size), _Prepost_z_ _Prepost_opt_count_(size))
#define _Inout_z_bytecount_(size)           _SAL1_1_Source_(_Inout_z_bytecount_, (size), _Prepost_z_ _Prepost_bytecount_(size))
#define _Inout_opt_z_bytecount_(size)       _SAL1_1_Source_(_Inout_opt_z_bytecount_, (size), _Prepost_z_ _Prepost_opt_bytecount_(size))

#define _Inout_z_count_c_(size)             _SAL1_1_Source_(_Inout_z_count_c_, (size), _Prepost_z_ _Prepost_count_c_(size))
#define _Inout_opt_z_count_c_(size)         _SAL1_1_Source_(_Inout_opt_z_count_c_, (size), _Prepost_z_ _Prepost_opt_count_c_(size))
#define _Inout_z_bytecount_c_(size)         _SAL1_1_Source_(_Inout_z_bytecount_c_, (size), _Prepost_z_ _Prepost_bytecount_c_(size))
#define _Inout_opt_z_bytecount_c_(size)     _SAL1_1_Source_(_Inout_opt_z_bytecount_c_, (size), _Prepost_z_ _Prepost_opt_bytecount_c_(size))

#define _Inout_ptrdiff_count_(size)       _SAL1_1_Source_(_Inout_ptrdiff_count_, (size), _Pre_ptrdiff_count_(size))
#define _Inout_opt_ptrdiff_count_(size)   _SAL1_1_Source_(_Inout_opt_ptrdiff_count_, (size), _Pre_opt_ptrdiff_count_(size))

#define _Inout_count_x_(size)             _SAL1_1_Source_(_Inout_count_x_, (size), _Prepost_count_x_(size))
#define _Inout_opt_count_x_(size)         _SAL1_1_Source_(_Inout_opt_count_x_, (size), _Prepost_opt_count_x_(size))
#define _Inout_bytecount_x_(size)         _SAL1_1_Source_(_Inout_bytecount_x_, (size), _Prepost_bytecount_x_(size))
#define _Inout_opt_bytecount_x_(size)     _SAL1_1_Source_(_Inout_opt_bytecount_x_, (size), _Prepost_opt_bytecount_x_(size))

#define _Inout_cap_(size)                 _SAL1_1_Source_(_Inout_cap_, (size), _Pre_valid_cap_(size)           _Post_valid_)
#define _Inout_opt_cap_(size)             _SAL1_1_Source_(_Inout_opt_cap_, (size), _Pre_opt_valid_cap_(size)       _Post_valid_)
#define _Inout_bytecap_(size)             _SAL1_1_Source_(_Inout_bytecap_, (size), _Pre_valid_bytecap_(size)       _Post_valid_)
#define _Inout_opt_bytecap_(size)         _SAL1_1_Source_(_Inout_opt_bytecap_, (size), _Pre_opt_valid_bytecap_(size)   _Post_valid_)

#define _Inout_cap_c_(size)               _SAL1_1_Source_(_Inout_cap_c_, (size), _Pre_valid_cap_c_(size)         _Post_valid_)
#define _Inout_opt_cap_c_(size)           _SAL1_1_Source_(_Inout_opt_cap_c_, (size), _Pre_opt_valid_cap_c_(size)     _Post_valid_)
#define _Inout_bytecap_c_(size)           _SAL1_1_Source_(_Inout_bytecap_c_, (size), _Pre_valid_bytecap_c_(size)     _Post_valid_)
#define _Inout_opt_bytecap_c_(size)       _SAL1_1_Source_(_Inout_opt_bytecap_c_, (size), _Pre_opt_valid_bytecap_c_(size) _Post_valid_)

#define _Inout_cap_x_(size)               _SAL1_1_Source_(_Inout_cap_x_, (size), _Pre_valid_cap_x_(size)         _Post_valid_)
#define _Inout_opt_cap_x_(size)           _SAL1_1_Source_(_Inout_opt_cap_x_, (size), _Pre_opt_valid_cap_x_(size)     _Post_valid_)
#define _Inout_bytecap_x_(size)           _SAL1_1_Source_(_Inout_bytecap_x_, (size), _Pre_valid_bytecap_x_(size)     _Post_valid_)
#define _Inout_opt_bytecap_x_(size)       _SAL1_1_Source_(_Inout_opt_bytecap_x_, (size), _Pre_opt_valid_bytecap_x_(size) _Post_valid_)

#define _Inout_z_cap_(size)                  _SAL1_1_Source_(_Inout_z_cap_, (size), _Pre_z_cap_(size)            _Post_z_)
#define _Inout_opt_z_cap_(size)              _SAL1_1_Source_(_Inout_opt_z_cap_, (size), _Pre_opt_z_cap_(size)        _Post_z_)
#define _Inout_z_bytecap_(size)              _SAL1_1_Source_(_Inout_z_bytecap_, (size), _Pre_z_bytecap_(size)        _Post_z_)
#define _Inout_opt_z_bytecap_(size)          _SAL1_1_Source_(_Inout_opt_z_bytecap_, (size), _Pre_opt_z_bytecap_(size)    _Post_z_)

#define _Inout_z_cap_c_(size)                _SAL1_1_Source_(_Inout_z_cap_c_, (size), _Pre_z_cap_c_(size)          _Post_z_)
#define _Inout_opt_z_cap_c_(size)            _SAL1_1_Source_(_Inout_opt_z_cap_c_, (size), _Pre_opt_z_cap_c_(size)      _Post_z_)
#define _Inout_z_bytecap_c_(size)            _SAL1_1_Source_(_Inout_z_bytecap_c_, (size), _Pre_z_bytecap_c_(size)      _Post_z_)
#define _Inout_opt_z_bytecap_c_(size)        _SAL1_1_Source_(_Inout_opt_z_bytecap_c_, (size), _Pre_opt_z_bytecap_c_(size)  _Post_z_)

#define _Inout_z_cap_x_(size)                _SAL1_1_Source_(_Inout_z_cap_x_, (size), _Pre_z_cap_x_(size)          _Post_z_)
#define _Inout_opt_z_cap_x_(size)            _SAL1_1_Source_(_Inout_opt_z_cap_x_, (size), _Pre_opt_z_cap_x_(size)      _Post_z_)
#define _Inout_z_bytecap_x_(size)            _SAL1_1_Source_(_Inout_z_bytecap_x_, (size), _Pre_z_bytecap_x_(size)      _Post_z_)
#define _Inout_opt_z_bytecap_x_(size)        _SAL1_1_Source_(_Inout_opt_z_bytecap_x_, (size), _Pre_opt_z_bytecap_x_(size)  _Post_z_)

#define _Ret_                   _SAL1_1_Source_(_Ret_, (), _Ret_valid_)
#define _Ret_opt_               _SAL1_1_Source_(_Ret_opt_, (), _Ret_opt_valid_)

#define _In_bound_           _SAL1_1_Source_(_In_bound_, (), _In_bound_impl_)
#define _Out_bound_          _SAL1_1_Source_(_Out_bound_, (), _Out_bound_impl_)
#define _Ret_bound_          _SAL1_1_Source_(_Ret_bound_, (), _Ret_bound_impl_)
#define _Deref_in_bound_     _SAL1_1_Source_(_Deref_in_bound_, (), _Deref_in_bound_impl_)
#define _Deref_out_bound_    _SAL1_1_Source_(_Deref_out_bound_, (), _Deref_out_bound_impl_)
#define _Deref_inout_bound_  _SAL1_1_Source_(_Deref_inout_bound_, (), _Deref_in_bound_ _Deref_out_bound_)
#define _Deref_ret_bound_    _SAL1_1_Source_(_Deref_ret_bound_, (), _Deref_ret_bound_impl_)

#define _Deref_out_             _SAL1_1_Source_(_Deref_out_, (), _Out_ _Deref_post_valid_)
#define _Deref_out_opt_         _SAL1_1_Source_(_Deref_out_opt_, (), _Out_ _Deref_post_opt_valid_)
#define _Deref_opt_out_         _SAL1_1_Source_(_Deref_opt_out_, (), _Out_opt_ _Deref_post_valid_)
#define _Deref_opt_out_opt_     _SAL1_1_Source_(_Deref_opt_out_opt_, (), _Out_opt_ _Deref_post_opt_valid_)

#define _Deref_out_z_           _SAL1_1_Source_(_Deref_out_z_, (), _Out_ _Deref_post_z_)
#define _Deref_out_opt_z_       _SAL1_1_Source_(_Deref_out_opt_z_, (), _Out_ _Deref_post_opt_z_)
#define _Deref_opt_out_z_       _SAL1_1_Source_(_Deref_opt_out_z_, (), _Out_opt_ _Deref_post_z_)
#define _Deref_opt_out_opt_z_   _SAL1_1_Source_(_Deref_opt_out_opt_z_, (), _Out_opt_ _Deref_post_opt_z_)

#define _Deref_pre_z_                           _SAL1_1_Source_(_Deref_pre_z_, (), _Deref_pre1_impl_(__notnull_impl_notref) _Deref_pre1_impl_(__zterm_impl) _Pre_valid_impl_)
#define _Deref_pre_opt_z_                       _SAL1_1_Source_(_Deref_pre_opt_z_, (), _Deref_pre1_impl_(__maybenull_impl_notref) _Deref_pre1_impl_(__zterm_impl) _Pre_valid_impl_)

#define _Deref_pre_cap_(size)                   _SAL1_1_Source_(_Deref_pre_cap_, (size), _Deref_pre1_impl_(__notnull_impl_notref)   _Deref_pre1_impl_(__cap_impl(size)))
#define _Deref_pre_opt_cap_(size)               _SAL1_1_Source_(_Deref_pre_opt_cap_, (size), _Deref_pre1_impl_(__maybenull_impl_notref) _Deref_pre1_impl_(__cap_impl(size)))
#define _Deref_pre_bytecap_(size)               _SAL1_1_Source_(_Deref_pre_bytecap_, (size), _Deref_pre1_impl_(__notnull_impl_notref)   _Deref_pre1_impl_(__bytecap_impl(size)))
#define _Deref_pre_opt_bytecap_(size)           _SAL1_1_Source_(_Deref_pre_opt_bytecap_, (size), _Deref_pre1_impl_(__maybenull_impl_notref) _Deref_pre1_impl_(__bytecap_impl(size)))

#define _Deref_pre_cap_c_(size)                 _SAL1_1_Source_(_Deref_pre_cap_c_, (size), _Deref_pre1_impl_(__notnull_impl_notref)   _Deref_pre1_impl_(__cap_c_impl(size)))
#define _Deref_pre_opt_cap_c_(size)             _SAL1_1_Source_(_Deref_pre_opt_cap_c_, (size), _Deref_pre1_impl_(__maybenull_impl_notref) _Deref_pre1_impl_(__cap_c_impl(size)))
#define _Deref_pre_bytecap_c_(size)             _SAL1_1_Source_(_Deref_pre_bytecap_c_, (size), _Deref_pre1_impl_(__notnull_impl_notref)   _Deref_pre1_impl_(__bytecap_c_impl(size)))
#define _Deref_pre_opt_bytecap_c_(size)         _SAL1_1_Source_(_Deref_pre_opt_bytecap_c_, (size), _Deref_pre1_impl_(__maybenull_impl_notref) _Deref_pre1_impl_(__bytecap_c_impl(size)))

#define _Deref_pre_cap_x_(size)                 _SAL1_1_Source_(_Deref_pre_cap_x_, (size), _Deref_pre1_impl_(__notnull_impl_notref)   _Deref_pre1_impl_(__cap_x_impl(size)))
#define _Deref_pre_opt_cap_x_(size)             _SAL1_1_Source_(_Deref_pre_opt_cap_x_, (size), _Deref_pre1_impl_(__maybenull_impl_notref) _Deref_pre1_impl_(__cap_x_impl(size)))
#define _Deref_pre_bytecap_x_(size)             _SAL1_1_Source_(_Deref_pre_bytecap_x_, (size), _Deref_pre1_impl_(__notnull_impl_notref)   _Deref_pre1_impl_(__bytecap_x_impl(size)))
#define _Deref_pre_opt_bytecap_x_(size)         _SAL1_1_Source_(_Deref_pre_opt_bytecap_x_, (size), _Deref_pre1_impl_(__maybenull_impl_notref) _Deref_pre1_impl_(__bytecap_x_impl(size)))

#define _Deref_pre_z_cap_(size)                 _SAL1_1_Source_(_Deref_pre_z_cap_, (size), _Deref_pre1_impl_(__notnull_impl_notref)   _Deref_pre2_impl_(__zterm_impl,__cap_impl(size))     _Pre_valid_impl_)
#define _Deref_pre_opt_z_cap_(size)             _SAL1_1_Source_(_Deref_pre_opt_z_cap_, (size), _Deref_pre1_impl_(__maybenull_impl_notref) _Deref_pre2_impl_(__zterm_impl,__cap_impl(size))     _Pre_valid_impl_)
#define _Deref_pre_z_bytecap_(size)             _SAL1_1_Source_(_Deref_pre_z_bytecap_, (size), _Deref_pre1_impl_(__notnull_impl_notref)   _Deref_pre2_impl_(__zterm_impl,__bytecap_impl(size)) _Pre_valid_impl_)
#define _Deref_pre_opt_z_bytecap_(size)         _SAL1_1_Source_(_Deref_pre_opt_z_bytecap_, (size), _Deref_pre1_impl_(__maybenull_impl_notref) _Deref_pre2_impl_(__zterm_impl,__bytecap_impl(size)) _Pre_valid_impl_)

#define _Deref_pre_z_cap_c_(size)               _SAL1_1_Source_(_Deref_pre_z_cap_c_, (size), _Deref_pre1_impl_(__notnull_impl_notref)   _Deref_pre2_impl_(__zterm_impl,__cap_c_impl(size))     _Pre_valid_impl_)
#define _Deref_pre_opt_z_cap_c_(size)           _SAL1_1_Source_(_Deref_pre_opt_z_cap_c_, (size), _Deref_pre1_impl_(__maybenull_impl_notref) _Deref_pre2_impl_(__zterm_impl,__cap_c_impl(size))     _Pre_valid_impl_)
#define _Deref_pre_z_bytecap_c_(size)           _SAL1_1_Source_(_Deref_pre_z_bytecap_c_, (size), _Deref_pre1_impl_(__notnull_impl_notref)   _Deref_pre2_impl_(__zterm_impl,__bytecap_c_impl(size)) _Pre_valid_impl_)
#define _Deref_pre_opt_z_bytecap_c_(size)       _SAL1_1_Source_(_Deref_pre_opt_z_bytecap_c_, (size), _Deref_pre1_impl_(__maybenull_impl_notref) _Deref_pre2_impl_(__zterm_impl,__bytecap_c_impl(size)) _Pre_valid_impl_)

#define _Deref_pre_z_cap_x_(size)               _SAL1_1_Source_(_Deref_pre_z_cap_x_, (size), _Deref_pre1_impl_(__notnull_impl_notref)   _Deref_pre2_impl_(__zterm_impl,__cap_x_impl(size))     _Pre_valid_impl_)
#define _Deref_pre_opt_z_cap_x_(size)           _SAL1_1_Source_(_Deref_pre_opt_z_cap_x_, (size), _Deref_pre1_impl_(__maybenull_impl_notref) _Deref_pre2_impl_(__zterm_impl,__cap_x_impl(size))     _Pre_valid_impl_)
#define _Deref_pre_z_bytecap_x_(size)           _SAL1_1_Source_(_Deref_pre_z_bytecap_x_, (size), _Deref_pre1_impl_(__notnull_impl_notref)   _Deref_pre2_impl_(__zterm_impl,__bytecap_x_impl(size)) _Pre_valid_impl_)
#define _Deref_pre_opt_z_bytecap_x_(size)       _SAL1_1_Source_(_Deref_pre_opt_z_bytecap_x_, (size), _Deref_pre1_impl_(__maybenull_impl_notref) _Deref_pre2_impl_(__zterm_impl,__bytecap_x_impl(size)) _Pre_valid_impl_)

#define _Deref_pre_valid_cap_(size)             _SAL1_1_Source_(_Deref_pre_valid_cap_, (size), _Deref_pre1_impl_(__notnull_impl_notref)   _Deref_pre1_impl_(__cap_impl(size))     _Pre_valid_impl_)
#define _Deref_pre_opt_valid_cap_(size)         _SAL1_1_Source_(_Deref_pre_opt_valid_cap_, (size), _Deref_pre1_impl_(__maybenull_impl_notref) _Deref_pre1_impl_(__cap_impl(size))     _Pre_valid_impl_)
#define _Deref_pre_valid_bytecap_(size)         _SAL1_1_Source_(_Deref_pre_valid_bytecap_, (size), _Deref_pre1_impl_(__notnull_impl_notref)   _Deref_pre1_impl_(__bytecap_impl(size)) _Pre_valid_impl_)
#define _Deref_pre_opt_valid_bytecap_(size)     _SAL1_1_Source_(_Deref_pre_opt_valid_bytecap_, (size), _Deref_pre1_impl_(__maybenull_impl_notref) _Deref_pre1_impl_(__bytecap_impl(size)) _Pre_valid_impl_)

#define _Deref_pre_valid_cap_c_(size)           _SAL1_1_Source_(_Deref_pre_valid_cap_c_, (size), _Deref_pre1_impl_(__notnull_impl_notref)   _Deref_pre1_impl_(__cap_c_impl(size))     _Pre_valid_impl_)
#define _Deref_pre_opt_valid_cap_c_(size)       _SAL1_1_Source_(_Deref_pre_opt_valid_cap_c_, (size), _Deref_pre1_impl_(__maybenull_impl_notref) _Deref_pre1_impl_(__cap_c_impl(size))     _Pre_valid_impl_)
#define _Deref_pre_valid_bytecap_c_(size)       _SAL1_1_Source_(_Deref_pre_valid_bytecap_c_, (size), _Deref_pre1_impl_(__notnull_impl_notref)   _Deref_pre1_impl_(__bytecap_c_impl(size)) _Pre_valid_impl_)
#define _Deref_pre_opt_valid_bytecap_c_(size)   _SAL1_1_Source_(_Deref_pre_opt_valid_bytecap_c_, (size), _Deref_pre1_impl_(__maybenull_impl_notref) _Deref_pre1_impl_(__bytecap_c_impl(size)) _Pre_valid_impl_)

#define _Deref_pre_valid_cap_x_(size)           _SAL1_1_Source_(_Deref_pre_valid_cap_x_, (size), _Deref_pre1_impl_(__notnull_impl_notref)   _Deref_pre1_impl_(__cap_x_impl(size))     _Pre_valid_impl_)
#define _Deref_pre_opt_valid_cap_x_(size)       _SAL1_1_Source_(_Deref_pre_opt_valid_cap_x_, (size), _Deref_pre1_impl_(__maybenull_impl_notref) _Deref_pre1_impl_(__cap_x_impl(size))     _Pre_valid_impl_)
#define _Deref_pre_valid_bytecap_x_(size)       _SAL1_1_Source_(_Deref_pre_valid_bytecap_x_, (size), _Deref_pre1_impl_(__notnull_impl_notref)   _Deref_pre1_impl_(__bytecap_x_impl(size)) _Pre_valid_impl_)
#define _Deref_pre_opt_valid_bytecap_x_(size)   _SAL1_1_Source_(_Deref_pre_opt_valid_bytecap_x_, (size), _Deref_pre1_impl_(__maybenull_impl_notref) _Deref_pre1_impl_(__bytecap_x_impl(size)) _Pre_valid_impl_)

#define _Deref_pre_count_(size)                 _SAL1_1_Source_(_Deref_pre_count_, (size), _Deref_pre1_impl_(__notnull_impl_notref)   _Deref_pre1_impl_(__count_impl(size))     _Pre_valid_impl_)
#define _Deref_pre_opt_count_(size)             _SAL1_1_Source_(_Deref_pre_opt_count_, (size), _Deref_pre1_impl_(__maybenull_impl_notref) _Deref_pre1_impl_(__count_impl(size))     _Pre_valid_impl_)
#define _Deref_pre_bytecount_(size)             _SAL1_1_Source_(_Deref_pre_bytecount_, (size), _Deref_pre1_impl_(__notnull_impl_notref)   _Deref_pre1_impl_(__bytecount_impl(size)) _Pre_valid_impl_)
#define _Deref_pre_opt_bytecount_(size)         _SAL1_1_Source_(_Deref_pre_opt_bytecount_, (size), _Deref_pre1_impl_(__maybenull_impl_notref) _Deref_pre1_impl_(__bytecount_impl(size)) _Pre_valid_impl_)

#define _Deref_pre_count_c_(size)               _SAL1_1_Source_(_Deref_pre_count_c_, (size), _Deref_pre1_impl_(__notnull_impl_notref)   _Deref_pre1_impl_(__count_c_impl(size))     _Pre_valid_impl_)
#define _Deref_pre_opt_count_c_(size)           _SAL1_1_Source_(_Deref_pre_opt_count_c_, (size), _Deref_pre1_impl_(__maybenull_impl_notref) _Deref_pre1_impl_(__count_c_impl(size))     _Pre_valid_impl_)
#define _Deref_pre_bytecount_c_(size)           _SAL1_1_Source_(_Deref_pre_bytecount_c_, (size), _Deref_pre1_impl_(__notnull_impl_notref)   _Deref_pre1_impl_(__bytecount_c_impl(size)) _Pre_valid_impl_)
#define _Deref_pre_opt_bytecount_c_(size)       _SAL1_1_Source_(_Deref_pre_opt_bytecount_c_, (size), _Deref_pre1_impl_(__maybenull_impl_notref) _Deref_pre1_impl_(__bytecount_c_impl(size)) _Pre_valid_impl_)

#define _Deref_pre_count_x_(size)               _SAL1_1_Source_(_Deref_pre_count_x_, (size), _Deref_pre1_impl_(__notnull_impl_notref)   _Deref_pre1_impl_(__count_x_impl(size))     _Pre_valid_impl_)
#define _Deref_pre_opt_count_x_(size)           _SAL1_1_Source_(_Deref_pre_opt_count_x_, (size), _Deref_pre1_impl_(__maybenull_impl_notref) _Deref_pre1_impl_(__count_x_impl(size))     _Pre_valid_impl_)
#define _Deref_pre_bytecount_x_(size)           _SAL1_1_Source_(_Deref_pre_bytecount_x_, (size), _Deref_pre1_impl_(__notnull_impl_notref)   _Deref_pre1_impl_(__bytecount_x_impl(size)) _Pre_valid_impl_)
#define _Deref_pre_opt_bytecount_x_(size)       _SAL1_1_Source_(_Deref_pre_opt_bytecount_x_, (size), _Deref_pre1_impl_(__maybenull_impl_notref) _Deref_pre1_impl_(__bytecount_x_impl(size)) _Pre_valid_impl_)

#define _Deref_pre_valid_                       _SAL1_1_Source_(_Deref_pre_valid_, (), _Deref_pre1_impl_(__notnull_impl_notref)   _Pre_valid_impl_)
#define _Deref_pre_opt_valid_                   _SAL1_1_Source_(_Deref_pre_opt_valid_, (), _Deref_pre1_impl_(__maybenull_impl_notref) _Pre_valid_impl_)
#define _Deref_pre_invalid_                     _SAL1_1_Source_(_Deref_pre_invalid_, (), _Deref_pre1_impl_(__notvalid_impl))

#define _Deref_pre_notnull_                     _SAL1_1_Source_(_Deref_pre_notnull_, (), _Deref_pre1_impl_(__notnull_impl_notref))
#define _Deref_pre_maybenull_                   _SAL1_1_Source_(_Deref_pre_maybenull_, (), _Deref_pre1_impl_(__maybenull_impl_notref))
#define _Deref_pre_null_                        _SAL1_1_Source_(_Deref_pre_null_, (), _Deref_pre1_impl_(__null_impl_notref))

#define _Deref_pre_readonly_                    _SAL1_1_Source_(_Deref_pre_readonly_, (), _Deref_pre1_impl_(__readaccess_impl_notref))
#define _Deref_pre_writeonly_                   _SAL1_1_Source_(_Deref_pre_writeonly_, (), _Deref_pre1_impl_(__writeaccess_impl_notref))

#define _Deref_post_z_                           _SAL1_1_Source_(_Deref_post_z_, (), _Deref_post1_impl_(__notnull_impl_notref) _Deref_post1_impl_(__zterm_impl) _Post_valid_impl_)
#define _Deref_post_opt_z_                       _SAL1_1_Source_(_Deref_post_opt_z_, (), _Deref_post1_impl_(__maybenull_impl_notref) _Deref_post1_impl_(__zterm_impl) _Post_valid_impl_)

#define _Deref_post_cap_(size)                   _SAL1_1_Source_(_Deref_post_cap_, (size), _Deref_post1_impl_(__notnull_impl_notref) _Deref_post1_impl_(__cap_impl(size)))
#define _Deref_post_opt_cap_(size)               _SAL1_1_Source_(_Deref_post_opt_cap_, (size), _Deref_post1_impl_(__maybenull_impl_notref) _Deref_post1_impl_(__cap_impl(size)))
#define _Deref_post_bytecap_(size)               _SAL1_1_Source_(_Deref_post_bytecap_, (size), _Deref_post1_impl_(__notnull_impl_notref) _Deref_post1_impl_(__bytecap_impl(size)))
#define _Deref_post_opt_bytecap_(size)           _SAL1_1_Source_(_Deref_post_opt_bytecap_, (size), _Deref_post1_impl_(__maybenull_impl_notref) _Deref_post1_impl_(__bytecap_impl(size)))

#define _Deref_post_cap_c_(size)                 _SAL1_1_Source_(_Deref_post_cap_c_, (size), _Deref_post1_impl_(__notnull_impl_notref) _Deref_post1_impl_(__cap_c_impl(size)))
#define _Deref_post_opt_cap_c_(size)             _SAL1_1_Source_(_Deref_post_opt_cap_c_, (size), _Deref_post1_impl_(__maybenull_impl_notref) _Deref_post1_impl_(__cap_c_impl(size)))
#define _Deref_post_bytecap_c_(size)             _SAL1_1_Source_(_Deref_post_bytecap_c_, (size), _Deref_post1_impl_(__notnull_impl_notref) _Deref_post1_impl_(__bytecap_c_impl(size)))
#define _Deref_post_opt_bytecap_c_(size)         _SAL1_1_Source_(_Deref_post_opt_bytecap_c_, (size), _Deref_post1_impl_(__maybenull_impl_notref) _Deref_post1_impl_(__bytecap_c_impl(size)))

#define _Deref_post_cap_x_(size)                 _SAL1_1_Source_(_Deref_post_cap_x_, (size), _Deref_post1_impl_(__notnull_impl_notref) _Deref_post1_impl_(__cap_x_impl(size)))
#define _Deref_post_opt_cap_x_(size)             _SAL1_1_Source_(_Deref_post_opt_cap_x_, (size), _Deref_post1_impl_(__maybenull_impl_notref) _Deref_post1_impl_(__cap_x_impl(size)))
#define _Deref_post_bytecap_x_(size)             _SAL1_1_Source_(_Deref_post_bytecap_x_, (size), _Deref_post1_impl_(__notnull_impl_notref) _Deref_post1_impl_(__bytecap_x_impl(size)))
#define _Deref_post_opt_bytecap_x_(size)         _SAL1_1_Source_(_Deref_post_opt_bytecap_x_, (size), _Deref_post1_impl_(__maybenull_impl_notref) _Deref_post1_impl_(__bytecap_x_impl(size)))

#define _Deref_post_z_cap_(size)                 _SAL1_1_Source_(_Deref_post_z_cap_, (size), _Deref_post1_impl_(__notnull_impl_notref) _Deref_post2_impl_(__zterm_impl,__cap_impl(size))       _Post_valid_impl_)
#define _Deref_post_opt_z_cap_(size)             _SAL1_1_Source_(_Deref_post_opt_z_cap_, (size), _Deref_post1_impl_(__maybenull_impl_notref) _Deref_post2_impl_(__zterm_impl,__cap_impl(size))       _Post_valid_impl_)
#define _Deref_post_z_bytecap_(size)             _SAL1_1_Source_(_Deref_post_z_bytecap_, (size), _Deref_post1_impl_(__notnull_impl_notref) _Deref_post2_impl_(__zterm_impl,__bytecap_impl(size))   _Post_valid_impl_)
#define _Deref_post_opt_z_bytecap_(size)         _SAL1_1_Source_(_Deref_post_opt_z_bytecap_, (size), _Deref_post1_impl_(__maybenull_impl_notref) _Deref_post2_impl_(__zterm_impl,__bytecap_impl(size))   _Post_valid_impl_)

#define _Deref_post_z_cap_c_(size)               _SAL1_1_Source_(_Deref_post_z_cap_c_, (size), _Deref_post1_impl_(__notnull_impl_notref) _Deref_post2_impl_(__zterm_impl,__cap_c_impl(size))     _Post_valid_impl_)
#define _Deref_post_opt_z_cap_c_(size)           _SAL1_1_Source_(_Deref_post_opt_z_cap_c_, (size), _Deref_post1_impl_(__maybenull_impl_notref) _Deref_post2_impl_(__zterm_impl,__cap_c_impl(size))     _Post_valid_impl_)
#define _Deref_post_z_bytecap_c_(size)           _SAL1_1_Source_(_Deref_post_z_bytecap_c_, (size), _Deref_post1_impl_(__notnull_impl_notref) _Deref_post2_impl_(__zterm_impl,__bytecap_c_impl(size)) _Post_valid_impl_)
#define _Deref_post_opt_z_bytecap_c_(size)       _SAL1_1_Source_(_Deref_post_opt_z_bytecap_c_, (size), _Deref_post1_impl_(__maybenull_impl_notref) _Deref_post2_impl_(__zterm_impl,__bytecap_c_impl(size)) _Post_valid_impl_)

#define _Deref_post_z_cap_x_(size)               _SAL1_1_Source_(_Deref_post_z_cap_x_, (size), _Deref_post1_impl_(__notnull_impl_notref) _Deref_post2_impl_(__zterm_impl,__cap_x_impl(size))     _Post_valid_impl_)
#define _Deref_post_opt_z_cap_x_(size)           _SAL1_1_Source_(_Deref_post_opt_z_cap_x_, (size), _Deref_post1_impl_(__maybenull_impl_notref) _Deref_post2_impl_(__zterm_impl,__cap_x_impl(size))     _Post_valid_impl_)
#define _Deref_post_z_bytecap_x_(size)           _SAL1_1_Source_(_Deref_post_z_bytecap_x_, (size), _Deref_post1_impl_(__notnull_impl_notref) _Deref_post2_impl_(__zterm_impl,__bytecap_x_impl(size)) _Post_valid_impl_)
#define _Deref_post_opt_z_bytecap_x_(size)       _SAL1_1_Source_(_Deref_post_opt_z_bytecap_x_, (size), _Deref_post1_impl_(__maybenull_impl_notref) _Deref_post2_impl_(__zterm_impl,__bytecap_x_impl(size)) _Post_valid_impl_)

#define _Deref_post_valid_cap_(size)             _SAL1_1_Source_(_Deref_post_valid_cap_, (size), _Deref_post1_impl_(__notnull_impl_notref) _Deref_post1_impl_(__cap_impl(size))       _Post_valid_impl_)
#define _Deref_post_opt_valid_cap_(size)         _SAL1_1_Source_(_Deref_post_opt_valid_cap_, (size), _Deref_post1_impl_(__maybenull_impl_notref) _Deref_post1_impl_(__cap_impl(size))       _Post_valid_impl_)
#define _Deref_post_valid_bytecap_(size)         _SAL1_1_Source_(_Deref_post_valid_bytecap_, (size), _Deref_post1_impl_(__notnull_impl_notref) _Deref_post1_impl_(__bytecap_impl(size))   _Post_valid_impl_)
#define _Deref_post_opt_valid_bytecap_(size)     _SAL1_1_Source_(_Deref_post_opt_valid_bytecap_, (size), _Deref_post1_impl_(__maybenull_impl_notref) _Deref_post1_impl_(__bytecap_impl(size))   _Post_valid_impl_)
                                                
#define _Deref_post_valid_cap_c_(size)           _SAL1_1_Source_(_Deref_post_valid_cap_c_, (size), _Deref_post1_impl_(__notnull_impl_notref) _Deref_post1_impl_(__cap_c_impl(size))     _Post_valid_impl_)
#define _Deref_post_opt_valid_cap_c_(size)       _SAL1_1_Source_(_Deref_post_opt_valid_cap_c_, (size), _Deref_post1_impl_(__maybenull_impl_notref) _Deref_post1_impl_(__cap_c_impl(size))     _Post_valid_impl_)
#define _Deref_post_valid_bytecap_c_(size)       _SAL1_1_Source_(_Deref_post_valid_bytecap_c_, (size), _Deref_post1_impl_(__notnull_impl_notref) _Deref_post1_impl_(__bytecap_c_impl(size)) _Post_valid_impl_)
#define _Deref_post_opt_valid_bytecap_c_(size)   _SAL1_1_Source_(_Deref_post_opt_valid_bytecap_c_, (size), _Deref_post1_impl_(__maybenull_impl_notref) _Deref_post1_impl_(__bytecap_c_impl(size)) _Post_valid_impl_)
                                                
#define _Deref_post_valid_cap_x_(size)           _SAL1_1_Source_(_Deref_post_valid_cap_x_, (size), _Deref_post1_impl_(__notnull_impl_notref) _Deref_post1_impl_(__cap_x_impl(size))     _Post_valid_impl_)
#define _Deref_post_opt_valid_cap_x_(size)       _SAL1_1_Source_(_Deref_post_opt_valid_cap_x_, (size), _Deref_post1_impl_(__maybenull_impl_notref) _Deref_post1_impl_(__cap_x_impl(size))     _Post_valid_impl_)
#define _Deref_post_valid_bytecap_x_(size)       _SAL1_1_Source_(_Deref_post_valid_bytecap_x_, (size), _Deref_post1_impl_(__notnull_impl_notref) _Deref_post1_impl_(__bytecap_x_impl(size)) _Post_valid_impl_)
#define _Deref_post_opt_valid_bytecap_x_(size)   _SAL1_1_Source_(_Deref_post_opt_valid_bytecap_x_, (size), _Deref_post1_impl_(__maybenull_impl_notref) _Deref_post1_impl_(__bytecap_x_impl(size)) _Post_valid_impl_)

#define _Deref_post_count_(size)                 _SAL1_1_Source_(_Deref_post_count_, (size), _Deref_post1_impl_(__notnull_impl_notref) _Deref_post1_impl_(__count_impl(size))       _Post_valid_impl_)
#define _Deref_post_opt_count_(size)             _SAL1_1_Source_(_Deref_post_opt_count_, (size), _Deref_post1_impl_(__maybenull_impl_notref) _Deref_post1_impl_(__count_impl(size))       _Post_valid_impl_)
#define _Deref_post_bytecount_(size)             _SAL1_1_Source_(_Deref_post_bytecount_, (size), _Deref_post1_impl_(__notnull_impl_notref) _Deref_post1_impl_(__bytecount_impl(size))   _Post_valid_impl_)
#define _Deref_post_opt_bytecount_(size)         _SAL1_1_Source_(_Deref_post_opt_bytecount_, (size), _Deref_post1_impl_(__maybenull_impl_notref) _Deref_post1_impl_(__bytecount_impl(size))   _Post_valid_impl_)

#define _Deref_post_count_c_(size)               _SAL1_1_Source_(_Deref_post_count_c_, (size), _Deref_post1_impl_(__notnull_impl_notref) _Deref_post1_impl_(__count_c_impl(size))     _Post_valid_impl_)
#define _Deref_post_opt_count_c_(size)           _SAL1_1_Source_(_Deref_post_opt_count_c_, (size), _Deref_post1_impl_(__maybenull_impl_notref) _Deref_post1_impl_(__count_c_impl(size))     _Post_valid_impl_)
#define _Deref_post_bytecount_c_(size)           _SAL1_1_Source_(_Deref_post_bytecount_c_, (size), _Deref_post1_impl_(__notnull_impl_notref) _Deref_post1_impl_(__bytecount_c_impl(size)) _Post_valid_impl_)
#define _Deref_post_opt_bytecount_c_(size)       _SAL1_1_Source_(_Deref_post_opt_bytecount_c_, (size), _Deref_post1_impl_(__maybenull_impl_notref) _Deref_post1_impl_(__bytecount_c_impl(size)) _Post_valid_impl_)

#define _Deref_post_count_x_(size)               _SAL1_1_Source_(_Deref_post_count_x_, (size), _Deref_post1_impl_(__notnull_impl_notref) _Deref_post1_impl_(__count_x_impl(size))     _Post_valid_impl_)
#define _Deref_post_opt_count_x_(size)           _SAL1_1_Source_(_Deref_post_opt_count_x_, (size), _Deref_post1_impl_(__maybenull_impl_notref) _Deref_post1_impl_(__count_x_impl(size))     _Post_valid_impl_)
#define _Deref_post_bytecount_x_(size)           _SAL1_1_Source_(_Deref_post_bytecount_x_, (size), _Deref_post1_impl_(__notnull_impl_notref) _Deref_post1_impl_(__bytecount_x_impl(size)) _Post_valid_impl_)
#define _Deref_post_opt_bytecount_x_(size)       _SAL1_1_Source_(_Deref_post_opt_bytecount_x_, (size), _Deref_post1_impl_(__maybenull_impl_notref) _Deref_post1_impl_(__bytecount_x_impl(size)) _Post_valid_impl_)

#define _Deref_post_valid_                       _SAL1_1_Source_(_Deref_post_valid_, (), _Deref_post1_impl_(__notnull_impl_notref)   _Post_valid_impl_)
#define _Deref_post_opt_valid_                   _SAL1_1_Source_(_Deref_post_opt_valid_, (), _Deref_post1_impl_(__maybenull_impl_notref) _Post_valid_impl_)

#define _Deref_post_notnull_                     _SAL1_1_Source_(_Deref_post_notnull_, (), _Deref_post1_impl_(__notnull_impl_notref))
#define _Deref_post_maybenull_                   _SAL1_1_Source_(_Deref_post_maybenull_, (), _Deref_post1_impl_(__maybenull_impl_notref))
#define _Deref_post_null_                        _SAL1_1_Source_(_Deref_post_null_, (), _Deref_post1_impl_(__null_impl_notref))

#define _Deref_ret_z_                            _SAL1_1_Source_(_Deref_ret_z_, (), _Deref_ret1_impl_(__notnull_impl_notref) _Deref_ret1_impl_(__zterm_impl))
#define _Deref_ret_opt_z_                        _SAL1_1_Source_(_Deref_ret_opt_z_, (), _Deref_ret1_impl_(__maybenull_impl_notref) _Ret1_impl_(__zterm_impl))

#define _Deref2_pre_readonly_                    _SAL1_1_Source_(_Deref2_pre_readonly_, (), _Deref2_pre1_impl_(__readaccess_impl_notref))

#define _Ret_opt_valid_                   _SAL1_1_Source_(_Ret_opt_valid_, (), _Ret1_impl_(__maybenull_impl_notref) _Ret_valid_impl_)
#define _Ret_opt_z_                       _SAL1_1_Source_(_Ret_opt_z_, (), _Ret2_impl_(__maybenull_impl,__zterm_impl) _Ret_valid_impl_)

#define _Ret_cap_(size)                   _SAL1_1_Source_(_Ret_cap_, (size), _Ret1_impl_(__notnull_impl_notref) _Ret1_impl_(__cap_impl(size)))
#define _Ret_opt_cap_(size)               _SAL1_1_Source_(_Ret_opt_cap_, (size), _Ret1_impl_(__maybenull_impl_notref) _Ret1_impl_(__cap_impl(size)))
#define _Ret_bytecap_(size)               _SAL1_1_Source_(_Ret_bytecap_, (size), _Ret1_impl_(__notnull_impl_notref) _Ret1_impl_(__bytecap_impl(size)))
#define _Ret_opt_bytecap_(size)           _SAL1_1_Source_(_Ret_opt_bytecap_, (size), _Ret1_impl_(__maybenull_impl_notref) _Ret1_impl_(__bytecap_impl(size)))

#define _Ret_cap_c_(size)                 _SAL1_1_Source_(_Ret_cap_c_, (size), _Ret1_impl_(__notnull_impl_notref) _Ret1_impl_(__cap_c_impl(size)))
#define _Ret_opt_cap_c_(size)             _SAL1_1_Source_(_Ret_opt_cap_c_, (size), _Ret1_impl_(__maybenull_impl_notref) _Ret1_impl_(__cap_c_impl(size)))
#define _Ret_bytecap_c_(size)             _SAL1_1_Source_(_Ret_bytecap_c_, (size), _Ret1_impl_(__notnull_impl_notref) _Ret1_impl_(__bytecap_c_impl(size)))
#define _Ret_opt_bytecap_c_(size)         _SAL1_1_Source_(_Ret_opt_bytecap_c_, (size), _Ret1_impl_(__maybenull_impl_notref) _Ret1_impl_(__bytecap_c_impl(size)))

#define _Ret_cap_x_(size)                 _SAL1_1_Source_(_Ret_cap_x_, (size), _Ret1_impl_(__notnull_impl_notref) _Ret1_impl_(__cap_x_impl(size)))
#define _Ret_opt_cap_x_(size)             _SAL1_1_Source_(_Ret_opt_cap_x_, (size), _Ret1_impl_(__maybenull_impl_notref) _Ret1_impl_(__cap_x_impl(size)))
#define _Ret_bytecap_x_(size)             _SAL1_1_Source_(_Ret_bytecap_x_, (size), _Ret1_impl_(__notnull_impl_notref) _Ret1_impl_(__bytecap_x_impl(size)))
#define _Ret_opt_bytecap_x_(size)         _SAL1_1_Source_(_Ret_opt_bytecap_x_, (size), _Ret1_impl_(__maybenull_impl_notref) _Ret1_impl_(__bytecap_x_impl(size)))

#define _Ret_z_cap_(size)                 _SAL1_1_Source_(_Ret_z_cap_, (size), _Ret1_impl_(__notnull_impl_notref) _Ret2_impl_(__zterm_impl,__cap_impl(size))     _Ret_valid_impl_)
#define _Ret_opt_z_cap_(size)             _SAL1_1_Source_(_Ret_opt_z_cap_, (size), _Ret1_impl_(__maybenull_impl_notref) _Ret2_impl_(__zterm_impl,__cap_impl(size))     _Ret_valid_impl_)
#define _Ret_z_bytecap_(size)             _SAL1_1_Source_(_Ret_z_bytecap_, (size), _Ret1_impl_(__notnull_impl_notref) _Ret2_impl_(__zterm_impl,__bytecap_impl(size)) _Ret_valid_impl_)
#define _Ret_opt_z_bytecap_(size)         _SAL1_1_Source_(_Ret_opt_z_bytecap_, (size), _Ret1_impl_(__maybenull_impl_notref) _Ret2_impl_(__zterm_impl,__bytecap_impl(size)) _Ret_valid_impl_)

#define _Ret_count_(size)                 _SAL1_1_Source_(_Ret_count_, (size), _Ret1_impl_(__notnull_impl_notref) _Ret1_impl_(__count_impl(size))     _Ret_valid_impl_)
#define _Ret_opt_count_(size)             _SAL1_1_Source_(_Ret_opt_count_, (size), _Ret1_impl_(__maybenull_impl_notref) _Ret1_impl_(__count_impl(size))     _Ret_valid_impl_)
#define _Ret_bytecount_(size)             _SAL1_1_Source_(_Ret_bytecount_, (size), _Ret1_impl_(__notnull_impl_notref) _Ret1_impl_(__bytecount_impl(size)) _Ret_valid_impl_)
#define _Ret_opt_bytecount_(size)         _SAL1_1_Source_(_Ret_opt_bytecount_, (size), _Ret1_impl_(__maybenull_impl_notref) _Ret1_impl_(__bytecount_impl(size)) _Ret_valid_impl_)

#define _Ret_count_c_(size)               _SAL1_1_Source_(_Ret_count_c_, (size), _Ret1_impl_(__notnull_impl_notref) _Ret1_impl_(__count_c_impl(size))     _Ret_valid_impl_)
#define _Ret_opt_count_c_(size)           _SAL1_1_Source_(_Ret_opt_count_c_, (size), _Ret1_impl_(__maybenull_impl_notref) _Ret1_impl_(__count_c_impl(size))     _Ret_valid_impl_)
#define _Ret_bytecount_c_(size)           _SAL1_1_Source_(_Ret_bytecount_c_, (size), _Ret1_impl_(__notnull_impl_notref) _Ret1_impl_(__bytecount_c_impl(size)) _Ret_valid_impl_)
#define _Ret_opt_bytecount_c_(size)       _SAL1_1_Source_(_Ret_opt_bytecount_c_, (size), _Ret1_impl_(__maybenull_impl_notref) _Ret1_impl_(__bytecount_c_impl(size)) _Ret_valid_impl_)

#define _Ret_count_x_(size)               _SAL1_1_Source_(_Ret_count_x_, (size), _Ret1_impl_(__notnull_impl_notref) _Ret1_impl_(__count_x_impl(size))     _Ret_valid_impl_)
#define _Ret_opt_count_x_(size)           _SAL1_1_Source_(_Ret_opt_count_x_, (size), _Ret1_impl_(__maybenull_impl_notref) _Ret1_impl_(__count_x_impl(size))     _Ret_valid_impl_)
#define _Ret_bytecount_x_(size)           _SAL1_1_Source_(_Ret_bytecount_x_, (size), _Ret1_impl_(__notnull_impl_notref) _Ret1_impl_(__bytecount_x_impl(size)) _Ret_valid_impl_)
#define _Ret_opt_bytecount_x_(size)       _SAL1_1_Source_(_Ret_opt_bytecount_x_, (size), _Ret1_impl_(__maybenull_impl_notref) _Ret1_impl_(__bytecount_x_impl(size)) _Ret_valid_impl_)

#define _Ret_z_count_(size)               _SAL1_1_Source_(_Ret_z_count_, (size), _Ret1_impl_(__notnull_impl_notref) _Ret2_impl_(__zterm_impl,__count_impl(size))     _Ret_valid_impl_)
#define _Ret_opt_z_count_(size)           _SAL1_1_Source_(_Ret_opt_z_count_, (size), _Ret1_impl_(__maybenull_impl_notref) _Ret2_impl_(__zterm_impl,__count_impl(size))     _Ret_valid_impl_)
#define _Ret_z_bytecount_(size)           _SAL1_1_Source_(_Ret_z_bytecount_, (size), _Ret1_impl_(__notnull_impl_notref) _Ret2_impl_(__zterm_impl,__bytecount_impl(size)) _Ret_valid_impl_)
#define _Ret_opt_z_bytecount_(size)       _SAL1_1_Source_(_Ret_opt_z_bytecount_, (size), _Ret1_impl_(__maybenull_impl_notref) _Ret2_impl_(__zterm_impl,__bytecount_impl(size)) _Ret_valid_impl_)


#define _Pre_opt_z_                       _SAL1_1_Source_(_Pre_opt_z_, (), _Pre1_impl_(__maybenull_impl_notref) _Pre1_impl_(__zterm_impl) _Pre_valid_impl_)

#define _Pre_readonly_                    _SAL1_1_Source_(_Pre_readonly_, (), _Pre1_impl_(__readaccess_impl_notref))
#define _Pre_writeonly_                   _SAL1_1_Source_(_Pre_writeonly_, (), _Pre1_impl_(__writeaccess_impl_notref))

#define _Pre_cap_(size)                   _SAL1_1_Source_(_Pre_cap_, (size), _Pre1_impl_(__notnull_impl_notref) _Pre1_impl_(__cap_impl(size)))
#define _Pre_opt_cap_(size)               _SAL1_1_Source_(_Pre_opt_cap_, (size), _Pre1_impl_(__maybenull_impl_notref) _Pre1_impl_(__cap_impl(size)))
#define _Pre_bytecap_(size)               _SAL1_1_Source_(_Pre_bytecap_, (size), _Pre1_impl_(__notnull_impl_notref) _Pre1_impl_(__bytecap_impl(size)))
#define _Pre_opt_bytecap_(size)           _SAL1_1_Source_(_Pre_opt_bytecap_, (size), _Pre1_impl_(__maybenull_impl_notref) _Pre1_impl_(__bytecap_impl(size)))

#define _Pre_cap_c_(size)                 _SAL1_1_Source_(_Pre_cap_c_, (size), _Pre1_impl_(__notnull_impl_notref) _Pre1_impl_(__cap_c_impl(size)))
#define _Pre_opt_cap_c_(size)             _SAL1_1_Source_(_Pre_opt_cap_c_, (size), _Pre1_impl_(__maybenull_impl_notref) _Pre1_impl_(__cap_c_impl(size)))
#define _Pre_bytecap_c_(size)             _SAL1_1_Source_(_Pre_bytecap_c_, (size), _Pre1_impl_(__notnull_impl_notref) _Pre1_impl_(__bytecap_c_impl(size)))
#define _Pre_opt_bytecap_c_(size)         _SAL1_1_Source_(_Pre_opt_bytecap_c_, (size), _Pre1_impl_(__maybenull_impl_notref) _Pre1_impl_(__bytecap_c_impl(size)))
#define _Pre_cap_c_one_                   _SAL1_1_Source_(_Pre_cap_c_one_, (), _Pre1_impl_(__notnull_impl_notref) _Pre1_impl_(__cap_c_one_notref_impl))
#define _Pre_opt_cap_c_one_               _SAL1_1_Source_(_Pre_opt_cap_c_one_, (), _Pre1_impl_(__maybenull_impl_notref) _Pre1_impl_(__cap_c_one_notref_impl))

#define _Pre_cap_m_(mult,size)            _SAL1_1_Source_(_Pre_cap_m_, (mult,size), _Pre1_impl_(__notnull_impl_notref) _Pre1_impl_(__mult_impl(mult,size)))
#define _Pre_opt_cap_m_(mult,size)        _SAL1_1_Source_(_Pre_opt_cap_m_, (mult,size), _Pre1_impl_(__maybenull_impl_notref) _Pre1_impl_(__mult_impl(mult,size)))

#define _Pre_cap_for_(param)              _SAL1_1_Source_(_Pre_cap_for_, (param), _Pre1_impl_(__notnull_impl_notref) _Pre1_impl_(__cap_for_impl(param)))
#define _Pre_opt_cap_for_(param)          _SAL1_1_Source_(_Pre_opt_cap_for_, (param), _Pre1_impl_(__maybenull_impl_notref) _Pre1_impl_(__cap_for_impl(param)))

#define _Pre_cap_x_(size)                 _SAL1_1_Source_(_Pre_cap_x_, (size), _Pre1_impl_(__notnull_impl_notref) _Pre1_impl_(__cap_x_impl(size)))
#define _Pre_opt_cap_x_(size)             _SAL1_1_Source_(_Pre_opt_cap_x_, (size), _Pre1_impl_(__maybenull_impl_notref) _Pre1_impl_(__cap_x_impl(size)))
#define _Pre_bytecap_x_(size)             _SAL1_1_Source_(_Pre_bytecap_x_, (size), _Pre1_impl_(__notnull_impl_notref) _Pre1_impl_(__bytecap_x_impl(size)))
#define _Pre_opt_bytecap_x_(size)         _SAL1_1_Source_(_Pre_opt_bytecap_x_, (size), _Pre1_impl_(__maybenull_impl_notref) _Pre1_impl_(__bytecap_x_impl(size)))

#define _Pre_ptrdiff_cap_(ptr)            _SAL1_1_Source_(_Pre_ptrdiff_cap_, (ptr), _Pre1_impl_(__notnull_impl_notref) _Pre1_impl_(__cap_x_impl(__ptrdiff(ptr))))
#define _Pre_opt_ptrdiff_cap_(ptr)        _SAL1_1_Source_(_Pre_opt_ptrdiff_cap_, (ptr), _Pre1_impl_(__maybenull_impl_notref) _Pre1_impl_(__cap_x_impl(__ptrdiff(ptr))))

#define _Pre_z_cap_(size)                 _SAL1_1_Source_(_Pre_z_cap_, (size), _Pre1_impl_(__notnull_impl_notref) _Pre2_impl_(__zterm_impl,__cap_impl(size))       _Pre_valid_impl_)
#define _Pre_opt_z_cap_(size)             _SAL1_1_Source_(_Pre_opt_z_cap_, (size), _Pre1_impl_(__maybenull_impl_notref) _Pre2_impl_(__zterm_impl,__cap_impl(size))       _Pre_valid_impl_)
#define _Pre_z_bytecap_(size)             _SAL1_1_Source_(_Pre_z_bytecap_, (size), _Pre1_impl_(__notnull_impl_notref) _Pre2_impl_(__zterm_impl,__bytecap_impl(size))   _Pre_valid_impl_)
#define _Pre_opt_z_bytecap_(size)         _SAL1_1_Source_(_Pre_opt_z_bytecap_, (size), _Pre1_impl_(__maybenull_impl_notref) _Pre2_impl_(__zterm_impl,__bytecap_impl(size))   _Pre_valid_impl_)

#define _Pre_z_cap_c_(size)               _SAL1_1_Source_(_Pre_z_cap_c_, (size), _Pre1_impl_(__notnull_impl_notref) _Pre2_impl_(__zterm_impl,__cap_c_impl(size))     _Pre_valid_impl_)
#define _Pre_opt_z_cap_c_(size)           _SAL1_1_Source_(_Pre_opt_z_cap_c_, (size), _Pre1_impl_(__maybenull_impl_notref) _Pre2_impl_(__zterm_impl,__cap_c_impl(size))     _Pre_valid_impl_)
#define _Pre_z_bytecap_c_(size)           _SAL1_1_Source_(_Pre_z_bytecap_c_, (size), _Pre1_impl_(__notnull_impl_notref) _Pre2_impl_(__zterm_impl,__bytecap_c_impl(size)) _Pre_valid_impl_)
#define _Pre_opt_z_bytecap_c_(size)       _SAL1_1_Source_(_Pre_opt_z_bytecap_c_, (size), _Pre1_impl_(__maybenull_impl_notref) _Pre2_impl_(__zterm_impl,__bytecap_c_impl(size)) _Pre_valid_impl_)

#define _Pre_z_cap_x_(size)               _SAL1_1_Source_(_Pre_z_cap_x_, (size), _Pre1_impl_(__notnull_impl_notref) _Pre2_impl_(__zterm_impl,__cap_x_impl(size))     _Pre_valid_impl_)
#define _Pre_opt_z_cap_x_(size)           _SAL1_1_Source_(_Pre_opt_z_cap_x_, (size), _Pre1_impl_(__maybenull_impl_notref) _Pre2_impl_(__zterm_impl,__cap_x_impl(size))     _Pre_valid_impl_)
#define _Pre_z_bytecap_x_(size)           _SAL1_1_Source_(_Pre_z_bytecap_x_, (size), _Pre1_impl_(__notnull_impl_notref) _Pre2_impl_(__zterm_impl,__bytecap_x_impl(size)) _Pre_valid_impl_)
#define _Pre_opt_z_bytecap_x_(size)       _SAL1_1_Source_(_Pre_opt_z_bytecap_x_, (size), _Pre1_impl_(__maybenull_impl_notref) _Pre2_impl_(__zterm_impl,__bytecap_x_impl(size)) _Pre_valid_impl_)

#define _Pre_valid_cap_(size)             _SAL1_1_Source_(_Pre_valid_cap_, (size), _Pre1_impl_(__notnull_impl_notref) _Pre1_impl_(__cap_impl(size))       _Pre_valid_impl_)
#define _Pre_opt_valid_cap_(size)         _SAL1_1_Source_(_Pre_opt_valid_cap_, (size), _Pre1_impl_(__maybenull_impl_notref) _Pre1_impl_(__cap_impl(size))       _Pre_valid_impl_)
#define _Pre_valid_bytecap_(size)         _SAL1_1_Source_(_Pre_valid_bytecap_, (size), _Pre1_impl_(__notnull_impl_notref) _Pre1_impl_(__bytecap_impl(size))   _Pre_valid_impl_)
#define _Pre_opt_valid_bytecap_(size)     _SAL1_1_Source_(_Pre_opt_valid_bytecap_, (size), _Pre1_impl_(__maybenull_impl_notref) _Pre1_impl_(__bytecap_impl(size))   _Pre_valid_impl_)

#define _Pre_valid_cap_c_(size)           _SAL1_1_Source_(_Pre_valid_cap_c_, (size), _Pre1_impl_(__notnull_impl_notref) _Pre1_impl_(__cap_c_impl(size))     _Pre_valid_impl_)
#define _Pre_opt_valid_cap_c_(size)       _SAL1_1_Source_(_Pre_opt_valid_cap_c_, (size), _Pre1_impl_(__maybenull_impl_notref) _Pre1_impl_(__cap_c_impl(size))     _Pre_valid_impl_)
#define _Pre_valid_bytecap_c_(size)       _SAL1_1_Source_(_Pre_valid_bytecap_c_, (size), _Pre1_impl_(__notnull_impl_notref) _Pre1_impl_(__bytecap_c_impl(size)) _Pre_valid_impl_)
#define _Pre_opt_valid_bytecap_c_(size)   _SAL1_1_Source_(_Pre_opt_valid_bytecap_c_, (size), _Pre1_impl_(__maybenull_impl_notref) _Pre1_impl_(__bytecap_c_impl(size)) _Pre_valid_impl_)

#define _Pre_valid_cap_x_(size)           _SAL1_1_Source_(_Pre_valid_cap_x_, (size), _Pre1_impl_(__notnull_impl_notref) _Pre1_impl_(__cap_x_impl(size))     _Pre_valid_impl_)
#define _Pre_opt_valid_cap_x_(size)       _SAL1_1_Source_(_Pre_opt_valid_cap_x_, (size), _Pre1_impl_(__maybenull_impl_notref) _Pre1_impl_(__cap_x_impl(size))     _Pre_valid_impl_)
#define _Pre_valid_bytecap_x_(size)       _SAL1_1_Source_(_Pre_valid_bytecap_x_, (size), _Pre1_impl_(__notnull_impl_notref) _Pre1_impl_(__bytecap_x_impl(size)) _Pre_valid_impl_)
#define _Pre_opt_valid_bytecap_x_(size)   _SAL1_1_Source_(_Pre_opt_valid_bytecap_x_, (size), _Pre1_impl_(__maybenull_impl_notref) _Pre1_impl_(__bytecap_x_impl(size)) _Pre_valid_impl_)

#define _Pre_count_(size)                 _SAL1_1_Source_(_Pre_count_, (size), _Pre1_impl_(__notnull_impl_notref) _Pre1_impl_(__count_impl(size))       _Pre_valid_impl_)
#define _Pre_opt_count_(size)             _SAL1_1_Source_(_Pre_opt_count_, (size), _Pre1_impl_(__maybenull_impl_notref) _Pre1_impl_(__count_impl(size))       _Pre_valid_impl_)
#define _Pre_bytecount_(size)             _SAL1_1_Source_(_Pre_bytecount_, (size), _Pre1_impl_(__notnull_impl_notref) _Pre1_impl_(__bytecount_impl(size))   _Pre_valid_impl_)
#define _Pre_opt_bytecount_(size)         _SAL1_1_Source_(_Pre_opt_bytecount_, (size), _Pre1_impl_(__maybenull_impl_notref) _Pre1_impl_(__bytecount_impl(size))   _Pre_valid_impl_)

#define _Pre_count_c_(size)               _SAL1_1_Source_(_Pre_count_c_, (size), _Pre1_impl_(__notnull_impl_notref) _Pre1_impl_(__count_c_impl(size))     _Pre_valid_impl_)
#define _Pre_opt_count_c_(size)           _SAL1_1_Source_(_Pre_opt_count_c_, (size), _Pre1_impl_(__maybenull_impl_notref) _Pre1_impl_(__count_c_impl(size))     _Pre_valid_impl_)
#define _Pre_bytecount_c_(size)           _SAL1_1_Source_(_Pre_bytecount_c_, (size), _Pre1_impl_(__notnull_impl_notref) _Pre1_impl_(__bytecount_c_impl(size)) _Pre_valid_impl_)
#define _Pre_opt_bytecount_c_(size)       _SAL1_1_Source_(_Pre_opt_bytecount_c_, (size), _Pre1_impl_(__maybenull_impl_notref) _Pre1_impl_(__bytecount_c_impl(size)) _Pre_valid_impl_)

#define _Pre_count_x_(size)               _SAL1_1_Source_(_Pre_count_x_, (size), _Pre1_impl_(__notnull_impl_notref) _Pre1_impl_(__count_x_impl(size))     _Pre_valid_impl_)
#define _Pre_opt_count_x_(size)           _SAL1_1_Source_(_Pre_opt_count_x_, (size), _Pre1_impl_(__maybenull_impl_notref) _Pre1_impl_(__count_x_impl(size))     _Pre_valid_impl_)
#define _Pre_bytecount_x_(size)           _SAL1_1_Source_(_Pre_bytecount_x_, (size), _Pre1_impl_(__notnull_impl_notref) _Pre1_impl_(__bytecount_x_impl(size)) _Pre_valid_impl_)
#define _Pre_opt_bytecount_x_(size)       _SAL1_1_Source_(_Pre_opt_bytecount_x_, (size), _Pre1_impl_(__maybenull_impl_notref) _Pre1_impl_(__bytecount_x_impl(size)) _Pre_valid_impl_)

#define _Pre_ptrdiff_count_(ptr)          _SAL1_1_Source_(_Pre_ptrdiff_count_, (ptr), _Pre1_impl_(__notnull_impl_notref) _Pre1_impl_(__count_x_impl(__ptrdiff(ptr))) _Pre_valid_impl_)
#define _Pre_opt_ptrdiff_count_(ptr)      _SAL1_1_Source_(_Pre_opt_ptrdiff_count_, (ptr), _Pre1_impl_(__maybenull_impl_notref) _Pre1_impl_(__count_x_impl(__ptrdiff(ptr))) _Pre_valid_impl_)

#define _Post_maybez_                    _SAL_L_Source_(_Post_maybez_, (), _Post1_impl_(__maybezterm_impl))

#define _Post_cap_(size)                 _SAL1_1_Source_(_Post_cap_, (size), _Post1_impl_(__cap_impl(size)))
#define _Post_bytecap_(size)             _SAL1_1_Source_(_Post_bytecap_, (size), _Post1_impl_(__bytecap_impl(size)))

#define _Post_count_(size)               _SAL1_1_Source_(_Post_count_, (size), _Post1_impl_(__count_impl(size))       _Post_valid_impl_)
#define _Post_bytecount_(size)           _SAL1_1_Source_(_Post_bytecount_, (size), _Post1_impl_(__bytecount_impl(size))   _Post_valid_impl_)
#define _Post_count_c_(size)             _SAL1_1_Source_(_Post_count_c_, (size), _Post1_impl_(__count_c_impl(size))     _Post_valid_impl_)
#define _Post_bytecount_c_(size)         _SAL1_1_Source_(_Post_bytecount_c_, (size), _Post1_impl_(__bytecount_c_impl(size)) _Post_valid_impl_)
#define _Post_count_x_(size)             _SAL1_1_Source_(_Post_count_x_, (size), _Post1_impl_(__count_x_impl(size))     _Post_valid_impl_)
#define _Post_bytecount_x_(size)         _SAL1_1_Source_(_Post_bytecount_x_, (size), _Post1_impl_(__bytecount_x_impl(size)) _Post_valid_impl_)

#define _Post_z_count_(size)             _SAL1_1_Source_(_Post_z_count_, (size), _Post2_impl_(__zterm_impl,__count_impl(size))       _Post_valid_impl_)
#define _Post_z_bytecount_(size)         _SAL1_1_Source_(_Post_z_bytecount_, (size), _Post2_impl_(__zterm_impl,__bytecount_impl(size))   _Post_valid_impl_)
#define _Post_z_count_c_(size)           _SAL1_1_Source_(_Post_z_count_c_, (size), _Post2_impl_(__zterm_impl,__count_c_impl(size))     _Post_valid_impl_)
#define _Post_z_bytecount_c_(size)       _SAL1_1_Source_(_Post_z_bytecount_c_, (size), _Post2_impl_(__zterm_impl,__bytecount_c_impl(size)) _Post_valid_impl_)
#define _Post_z_count_x_(size)           _SAL1_1_Source_(_Post_z_count_x_, (size), _Post2_impl_(__zterm_impl,__count_x_impl(size))     _Post_valid_impl_)
#define _Post_z_bytecount_x_(size)       _SAL1_1_Source_(_Post_z_bytecount_x_, (size), _Post2_impl_(__zterm_impl,__bytecount_x_impl(size)) _Post_valid_impl_)

#define _Prepost_opt_z_                  _SAL1_1_Source_(_Prepost_opt_z_, (), _Pre_opt_z_  _Post_z_)

#define _Prepost_count_(size)            _SAL1_1_Source_(_Prepost_count_, (size), _Pre_count_(size)           _Post_count_(size))
#define _Prepost_opt_count_(size)        _SAL1_1_Source_(_Prepost_opt_count_, (size), _Pre_opt_count_(size)       _Post_count_(size))
#define _Prepost_bytecount_(size)        _SAL1_1_Source_(_Prepost_bytecount_, (size), _Pre_bytecount_(size)       _Post_bytecount_(size))
#define _Prepost_opt_bytecount_(size)    _SAL1_1_Source_(_Prepost_opt_bytecount_, (size), _Pre_opt_bytecount_(size)   _Post_bytecount_(size))
#define _Prepost_count_c_(size)          _SAL1_1_Source_(_Prepost_count_c_, (size), _Pre_count_c_(size)         _Post_count_c_(size))
#define _Prepost_opt_count_c_(size)      _SAL1_1_Source_(_Prepost_opt_count_c_, (size), _Pre_opt_count_c_(size)     _Post_count_c_(size))
#define _Prepost_bytecount_c_(size)      _SAL1_1_Source_(_Prepost_bytecount_c_, (size), _Pre_bytecount_c_(size)     _Post_bytecount_c_(size))
#define _Prepost_opt_bytecount_c_(size)  _SAL1_1_Source_(_Prepost_opt_bytecount_c_, (size), _Pre_opt_bytecount_c_(size) _Post_bytecount_c_(size))
#define _Prepost_count_x_(size)          _SAL1_1_Source_(_Prepost_count_x_, (size), _Pre_count_x_(size)         _Post_count_x_(size))
#define _Prepost_opt_count_x_(size)      _SAL1_1_Source_(_Prepost_opt_count_x_, (size), _Pre_opt_count_x_(size)     _Post_count_x_(size))
#define _Prepost_bytecount_x_(size)      _SAL1_1_Source_(_Prepost_bytecount_x_, (size), _Pre_bytecount_x_(size)     _Post_bytecount_x_(size))
#define _Prepost_opt_bytecount_x_(size)  _SAL1_1_Source_(_Prepost_opt_bytecount_x_, (size), _Pre_opt_bytecount_x_(size) _Post_bytecount_x_(size))

#define _Prepost_valid_                   _SAL1_1_Source_(_Prepost_valid_, (), _Pre_valid_     _Post_valid_)
#define _Prepost_opt_valid_               _SAL1_1_Source_(_Prepost_opt_valid_, (), _Pre_opt_valid_ _Post_valid_)

#define _Deref_prepost_z_                         _SAL1_1_Source_(_Deref_prepost_z_, (), _Deref_pre_z_      _Deref_post_z_)
#define _Deref_prepost_opt_z_                     _SAL1_1_Source_(_Deref_prepost_opt_z_, (), _Deref_pre_opt_z_  _Deref_post_opt_z_)

#define _Deref_prepost_cap_(size)                 _SAL1_1_Source_(_Deref_prepost_cap_, (size), _Deref_pre_cap_(size)                _Deref_post_cap_(size))
#define _Deref_prepost_opt_cap_(size)             _SAL1_1_Source_(_Deref_prepost_opt_cap_, (size), _Deref_pre_opt_cap_(size)            _Deref_post_opt_cap_(size))
#define _Deref_prepost_bytecap_(size)             _SAL1_1_Source_(_Deref_prepost_bytecap_, (size), _Deref_pre_bytecap_(size)            _Deref_post_bytecap_(size))
#define _Deref_prepost_opt_bytecap_(size)         _SAL1_1_Source_(_Deref_prepost_opt_bytecap_, (size), _Deref_pre_opt_bytecap_(size)        _Deref_post_opt_bytecap_(size))

#define _Deref_prepost_cap_x_(size)               _SAL1_1_Source_(_Deref_prepost_cap_x_, (size), _Deref_pre_cap_x_(size)              _Deref_post_cap_x_(size))
#define _Deref_prepost_opt_cap_x_(size)           _SAL1_1_Source_(_Deref_prepost_opt_cap_x_, (size), _Deref_pre_opt_cap_x_(size)          _Deref_post_opt_cap_x_(size))
#define _Deref_prepost_bytecap_x_(size)           _SAL1_1_Source_(_Deref_prepost_bytecap_x_, (size), _Deref_pre_bytecap_x_(size)          _Deref_post_bytecap_x_(size))
#define _Deref_prepost_opt_bytecap_x_(size)       _SAL1_1_Source_(_Deref_prepost_opt_bytecap_x_, (size), _Deref_pre_opt_bytecap_x_(size)      _Deref_post_opt_bytecap_x_(size))

#define _Deref_prepost_z_cap_(size)               _SAL1_1_Source_(_Deref_prepost_z_cap_, (size), _Deref_pre_z_cap_(size)              _Deref_post_z_cap_(size))
#define _Deref_prepost_opt_z_cap_(size)           _SAL1_1_Source_(_Deref_prepost_opt_z_cap_, (size), _Deref_pre_opt_z_cap_(size)          _Deref_post_opt_z_cap_(size))
#define _Deref_prepost_z_bytecap_(size)           _SAL1_1_Source_(_Deref_prepost_z_bytecap_, (size), _Deref_pre_z_bytecap_(size)          _Deref_post_z_bytecap_(size))
#define _Deref_prepost_opt_z_bytecap_(size)       _SAL1_1_Source_(_Deref_prepost_opt_z_bytecap_, (size), _Deref_pre_opt_z_bytecap_(size)      _Deref_post_opt_z_bytecap_(size))

#define _Deref_prepost_valid_cap_(size)           _SAL1_1_Source_(_Deref_prepost_valid_cap_, (size), _Deref_pre_valid_cap_(size)          _Deref_post_valid_cap_(size))
#define _Deref_prepost_opt_valid_cap_(size)       _SAL1_1_Source_(_Deref_prepost_opt_valid_cap_, (size), _Deref_pre_opt_valid_cap_(size)      _Deref_post_opt_valid_cap_(size))
#define _Deref_prepost_valid_bytecap_(size)       _SAL1_1_Source_(_Deref_prepost_valid_bytecap_, (size), _Deref_pre_valid_bytecap_(size)      _Deref_post_valid_bytecap_(size))
#define _Deref_prepost_opt_valid_bytecap_(size)   _SAL1_1_Source_(_Deref_prepost_opt_valid_bytecap_, (size), _Deref_pre_opt_valid_bytecap_(size)  _Deref_post_opt_valid_bytecap_(size))

#define _Deref_prepost_valid_cap_x_(size)           _SAL1_1_Source_(_Deref_prepost_valid_cap_x_, (size), _Deref_pre_valid_cap_x_(size)          _Deref_post_valid_cap_x_(size))
#define _Deref_prepost_opt_valid_cap_x_(size)       _SAL1_1_Source_(_Deref_prepost_opt_valid_cap_x_, (size), _Deref_pre_opt_valid_cap_x_(size)      _Deref_post_opt_valid_cap_x_(size))
#define _Deref_prepost_valid_bytecap_x_(size)       _SAL1_1_Source_(_Deref_prepost_valid_bytecap_x_, (size), _Deref_pre_valid_bytecap_x_(size)      _Deref_post_valid_bytecap_x_(size))
#define _Deref_prepost_opt_valid_bytecap_x_(size)   _SAL1_1_Source_(_Deref_prepost_opt_valid_bytecap_x_, (size), _Deref_pre_opt_valid_bytecap_x_(size)  _Deref_post_opt_valid_bytecap_x_(size))

#define _Deref_prepost_count_(size)             _SAL1_1_Source_(_Deref_prepost_count_, (size), _Deref_pre_count_(size)            _Deref_post_count_(size))
#define _Deref_prepost_opt_count_(size)         _SAL1_1_Source_(_Deref_prepost_opt_count_, (size), _Deref_pre_opt_count_(size)        _Deref_post_opt_count_(size))
#define _Deref_prepost_bytecount_(size)         _SAL1_1_Source_(_Deref_prepost_bytecount_, (size), _Deref_pre_bytecount_(size)        _Deref_post_bytecount_(size))
#define _Deref_prepost_opt_bytecount_(size)     _SAL1_1_Source_(_Deref_prepost_opt_bytecount_, (size), _Deref_pre_opt_bytecount_(size)    _Deref_post_opt_bytecount_(size))

#define _Deref_prepost_count_x_(size)           _SAL1_1_Source_(_Deref_prepost_count_x_, (size), _Deref_pre_count_x_(size)          _Deref_post_count_x_(size))
#define _Deref_prepost_opt_count_x_(size)       _SAL1_1_Source_(_Deref_prepost_opt_count_x_, (size), _Deref_pre_opt_count_x_(size)      _Deref_post_opt_count_x_(size))
#define _Deref_prepost_bytecount_x_(size)       _SAL1_1_Source_(_Deref_prepost_bytecount_x_, (size), _Deref_pre_bytecount_x_(size)      _Deref_post_bytecount_x_(size))
#define _Deref_prepost_opt_bytecount_x_(size)   _SAL1_1_Source_(_Deref_prepost_opt_bytecount_x_, (size), _Deref_pre_opt_bytecount_x_(size)  _Deref_post_opt_bytecount_x_(size))

#define _Deref_prepost_valid_                    _SAL1_1_Source_(_Deref_prepost_valid_, (), _Deref_pre_valid_     _Deref_post_valid_)
#define _Deref_prepost_opt_valid_                _SAL1_1_Source_(_Deref_prepost_opt_valid_, (), _Deref_pre_opt_valid_ _Deref_post_opt_valid_)

#define _Deref_out_z_cap_c_(size)  _SAL1_1_Source_(_Deref_out_z_cap_c_, (size), _Deref_pre_cap_c_(size) _Deref_post_z_)
#define _Deref_inout_z_cap_c_(size)  _SAL1_1_Source_(_Deref_inout_z_cap_c_, (size), _Deref_pre_z_cap_c_(size) _Deref_post_z_)
#define _Deref_out_z_bytecap_c_(size)  _SAL1_1_Source_(_Deref_out_z_bytecap_c_, (size), _Deref_pre_bytecap_c_(size) _Deref_post_z_)
#define _Deref_inout_z_bytecap_c_(size)  _SAL1_1_Source_(_Deref_inout_z_bytecap_c_, (size), _Deref_pre_z_bytecap_c_(size) _Deref_post_z_)
#define _Deref_inout_z_  _SAL1_1_Source_(_Deref_inout_z_, (), _Deref_prepost_z_)

#pragma endregion Input Buffer SAL 1 compatibility macros

#define _Always_impl_(annos)            _Group_(annos _SAL_nop_impl_) _On_failure_impl_(annos _SAL_nop_impl_)
#define _Bound_impl_                    _SA_annotes0(SAL_bound)
#define _Field_range_impl_(min,max)     _Range_impl_(min,max)
#define _Literal_impl_                  _SA_annotes1(SAL_constant, __yes)
#define _Maybenull_impl_                _SA_annotes1(SAL_null, __maybe)
#define _Maybevalid_impl_               _SA_annotes1(SAL_valid, __maybe)
#define _Must_inspect_impl_ _Post_impl_ _SA_annotes0(SAL_mustInspect)
#define _Notliteral_impl_               _SA_annotes1(SAL_constant, __no)
#define _Notnull_impl_                  _SA_annotes1(SAL_null, __no)
#define _Notvalid_impl_                 _SA_annotes1(SAL_valid, __no)
#define _NullNull_terminated_impl_      _Group_(_SA_annotes1(SAL_nullTerminated, __yes) _SA_annotes1(SAL_readableTo,inexpressibleCount("NullNull terminated string")))
#define _Null_impl_                     _SA_annotes1(SAL_null, __yes)
#define _Null_terminated_impl_          _SA_annotes1(SAL_nullTerminated, __yes)
#define _Out_impl_                      _Pre1_impl_(__notnull_impl_notref) _Pre1_impl_(__cap_c_one_notref_impl) _Post_valid_impl_
#define _Out_opt_impl_                  _Pre1_impl_(__maybenull_impl_notref) _Pre1_impl_(__cap_c_one_notref_impl) _Post_valid_impl_
#define _Points_to_data_impl_           _At_(*_Curr_, _SA_annotes1(SAL_mayBePointer, __no))
#define _Post_satisfies_impl_(cond)     _Post_impl_ _Satisfies_impl_(cond)
#define _Post_valid_impl_               _Post1_impl_(__valid_impl)
#define _Pre_satisfies_impl_(cond)      _Pre_impl_ _Satisfies_impl_(cond)
#define _Pre_valid_impl_                _Pre1_impl_(__valid_impl)
#define _Range_impl_(min,max)           _SA_annotes2(SAL_range, min, max)
#define _Readable_bytes_impl_(size)     _SA_annotes1(SAL_readableTo, byteCount(size))
#define _Readable_elements_impl_(size)  _SA_annotes1(SAL_readableTo, elementCount(size))
#define _Ret_valid_impl_                _Ret1_impl_(__valid_impl)
#define _Satisfies_impl_(cond)          _SA_annotes1(SAL_satisfies, cond)
#define _Valid_impl_                    _SA_annotes1(SAL_valid, __yes)
#define _Writable_bytes_impl_(size)     _SA_annotes1(SAL_writableTo, byteCount(size))
#define _Writable_elements_impl_(size)  _SA_annotes1(SAL_writableTo, elementCount(size))

#define _In_range_impl_(min,max)        _Pre_impl_ _Range_impl_(min,max)
#define _Out_range_impl_(min,max)       _Post_impl_ _Range_impl_(min,max)
#define _Ret_range_impl_(min,max)       _Post_impl_ _Range_impl_(min,max)
#define _Deref_in_range_impl_(min,max)  _Deref_pre_impl_ _Range_impl_(min,max)
#define _Deref_out_range_impl_(min,max) _Deref_post_impl_ _Range_impl_(min,max)
#define _Deref_ret_range_impl_(min,max) _Deref_post_impl_ _Range_impl_(min,max)

#define _Deref_pre_impl_                _Pre_impl_  _Notref_impl_ _Deref_impl_
#define _Deref_post_impl_               _Post_impl_ _Notref_impl_ _Deref_impl_

#define __AuToQuOtE                     _SA_annotes0(SAL_AuToQuOtE)

#define __deferTypecheck                _SA_annotes0(SAL_deferTypecheck)

#define _SA_SPECSTRIZE( x ) #x
#define _SAL_nop_impl_       /* nothing */
#define __nop_impl(x)            x

#define _SA_annotes0(n)                __declspec(#n)
#define _SA_annotes1(n,pp1)            __declspec(#n "(" _SA_SPECSTRIZE(pp1) ")" )
#define _SA_annotes2(n,pp1,pp2)        __declspec(#n "(" _SA_SPECSTRIZE(pp1) "," _SA_SPECSTRIZE(pp2) ")")
#define _SA_annotes3(n,pp1,pp2,pp3)    __declspec(#n "(" _SA_SPECSTRIZE(pp1) "," _SA_SPECSTRIZE(pp2) "," _SA_SPECSTRIZE(pp3) ")")

#define _Pre_impl_                     _SA_annotes0(SAL_pre)
#define _Post_impl_                    _SA_annotes0(SAL_post)
#define _Deref_impl_                   _SA_annotes0(SAL_deref)
#define _Notref_impl_                  _SA_annotes0(SAL_notref)

#define __ANNOTATION(fun)              _SA_annotes0(SAL_annotation) void __SA_##fun
 
#define __PRIMOP(type, fun)            _SA_annotes0(SAL_primop) type __SA_##fun

#define __QUALIFIER(fun)               _SA_annotes0(SAL_qualifier)  void __SA_##fun;

#define __In_impl_ _Pre_impl_ _SA_annotes0(SAL_valid) _Pre_impl_ _Deref_impl_ _Notref_impl_ _SA_annotes0(SAL_readonly)


__ANNOTATION(SAL_useHeader(void));
__ANNOTATION(SAL_bound(void));
__ANNOTATION(SAL_allocator(void));   //??? resolve with PFD
__ANNOTATION(SAL_file_parser(__AuToQuOtE __In_impl_ char *, __In_impl_ char *));
__ANNOTATION(SAL_source_code_content(__In_impl_ char *));
__ANNOTATION(SAL_analysisHint(__AuToQuOtE __In_impl_ char *));
__ANNOTATION(SAL_untrusted_data_source(__AuToQuOtE __In_impl_ char *));
__ANNOTATION(SAL_untrusted_data_source_this(__AuToQuOtE __In_impl_ char *));
__ANNOTATION(SAL_validated(__AuToQuOtE __In_impl_ char *));
__ANNOTATION(SAL_validated_this(__AuToQuOtE __In_impl_ char *));
__ANNOTATION(SAL_encoded(void));
__ANNOTATION(SAL_adt(__AuToQuOtE __In_impl_ char *, __AuToQuOtE __In_impl_ char *));
__ANNOTATION(SAL_add_adt_property(__AuToQuOtE __In_impl_ char *, __AuToQuOtE __In_impl_ char *));
__ANNOTATION(SAL_remove_adt_property(__AuToQuOtE __In_impl_ char *, __AuToQuOtE __In_impl_ char *));
__ANNOTATION(SAL_transfer_adt_property_from(__AuToQuOtE __In_impl_ char *));
__ANNOTATION(SAL_post_type(__AuToQuOtE __In_impl_ char *));
__ANNOTATION(SAL_volatile(void));
__ANNOTATION(SAL_nonvolatile(void));
__ANNOTATION(SAL_entrypoint(__AuToQuOtE __In_impl_ char *, __AuToQuOtE __In_impl_ char *));
__ANNOTATION(SAL_blocksOn(__In_impl_ void*));
__ANNOTATION(SAL_mustInspect(void));

__ANNOTATION(SAL_TypeName(__AuToQuOtE __In_impl_ char *));

__ANNOTATION(SAL_interlocked(void);)

__QUALIFIER(SAL_name(__In_impl_ char *, __In_impl_ char *, __In_impl_ char *);)

__PRIMOP(char *, _Macro_value_(__In_impl_ char *));
__PRIMOP(int, _Macro_defined_(__In_impl_ char *));
__PRIMOP(char *, _Strstr_(__In_impl_ char *, __In_impl_ char *));

//#define _Check_return_impl_ __post      _SA_annotes0(SAL_checkReturn)
#define _Check_return_impl_

#define _Success_impl_(expr)            _SA_annotes1(SAL_success, expr)
#define _On_failure_impl_(annos)        _SA_annotes1(SAL_context, SAL_failed) _Group_(_Post_impl_ _Group_(_SAL_nop_impl_ annos))

#define _Printf_format_string_impl_     _SA_annotes1(SAL_IsFormatString, "printf")
#define _Scanf_format_string_impl_      _SA_annotes1(SAL_IsFormatString, "scanf")
#define _Scanf_s_format_string_impl_    _SA_annotes1(SAL_IsFormatString, "scanf_s")

#define _In_bound_impl_                 _Pre_impl_ _Bound_impl_
#define _Out_bound_impl_                _Post_impl_ _Bound_impl_
#define _Ret_bound_impl_                _Post_impl_ _Bound_impl_
#define _Deref_in_bound_impl_           _Deref_pre_impl_ _Bound_impl_
#define _Deref_out_bound_impl_          _Deref_post_impl_ _Bound_impl_
#define _Deref_ret_bound_impl_          _Deref_post_impl_ _Bound_impl_


#define __null_impl              _SA_annotes0(SAL_null) // _SA_annotes1(SAL_null, __yes)
#define __notnull_impl           _SA_annotes0(SAL_notnull) // _SA_annotes1(SAL_null, __no)
#define __maybenull_impl         _SA_annotes0(SAL_maybenull) // _SA_annotes1(SAL_null, __maybe)

#define __valid_impl             _SA_annotes0(SAL_valid) // _SA_annotes1(SAL_valid, __yes)
#define __notvalid_impl          _SA_annotes0(SAL_notvalid) // _SA_annotes1(SAL_valid, __no)
#define __maybevalid_impl        _SA_annotes0(SAL_maybevalid) // _SA_annotes1(SAL_valid, __maybe)

#define __null_impl_notref       _Notref_ _Null_impl_
#define __maybenull_impl_notref  _Notref_ _Maybenull_impl_
#define __notnull_impl_notref    _Notref_ _Notnull_impl_

#define __zterm_impl             _SA_annotes1(SAL_nullTerminated, __yes)
#define __maybezterm_impl        _SA_annotes1(SAL_nullTerminated, __maybe)
#define __maybzterm_impl         _SA_annotes1(SAL_nullTerminated, __maybe)
#define __notzterm_impl          _SA_annotes1(SAL_nullTerminated, __no)

#define __readaccess_impl        _SA_annotes1(SAL_access, 0x1)
#define __writeaccess_impl       _SA_annotes1(SAL_access, 0x2)
#define __allaccess_impl         _SA_annotes1(SAL_access, 0x3)

#define __readaccess_impl_notref  _Notref_ _SA_annotes1(SAL_access, 0x1)
#define __writeaccess_impl_notref _Notref_ _SA_annotes1(SAL_access, 0x2)
#define __allaccess_impl_notref   _Notref_ _SA_annotes1(SAL_access, 0x3)

#define __cap_impl(size)         _SA_annotes1(SAL_writableTo,elementCount(size))
#define __cap_c_impl(size)       _SA_annotes1(SAL_writableTo,elementCount(size))
#define __cap_c_one_notref_impl  _Notref_ _SA_annotes1(SAL_writableTo,elementCount(1))
#define __cap_for_impl(param)    _SA_annotes1(SAL_writableTo,inexpressibleCount(sizeof(param)))
#define __cap_x_impl(size)       _SA_annotes1(SAL_writableTo,inexpressibleCount(#size))

#define __bytecap_impl(size)     _SA_annotes1(SAL_writableTo,byteCount(size))
#define __bytecap_c_impl(size)   _SA_annotes1(SAL_writableTo,byteCount(size))
#define __bytecap_x_impl(size)   _SA_annotes1(SAL_writableTo,inexpressibleCount(#size))

#define __mult_impl(mult,size)   _SA_annotes1(SAL_writableTo,(mult)*(size))

#define __count_impl(size)       _SA_annotes1(SAL_readableTo,elementCount(size))
#define __count_c_impl(size)     _SA_annotes1(SAL_readableTo,elementCount(size))
#define __count_x_impl(size)     _SA_annotes1(SAL_readableTo,inexpressibleCount(#size))

#define __bytecount_impl(size)   _SA_annotes1(SAL_readableTo,byteCount(size))
#define __bytecount_c_impl(size) _SA_annotes1(SAL_readableTo,byteCount(size))
#define __bytecount_x_impl(size) _SA_annotes1(SAL_readableTo,inexpressibleCount(#size))

#define _At_impl_(target, annos)     _SA_annotes0(SAL_at(target)) _Group_(annos)
#define _At_buffer_impl_(target, iter, bound, annos)  _SA_annotes3(SAL_at_buffer, target, iter, bound) _Group_(annos)
#define _Group_impl_(annos)          _SA_annotes0(SAL_begin) annos _SA_annotes0(SAL_end)
#define _GrouP_impl_(annos)          _SA_annotes0(SAL_BEGIN) annos _SA_annotes0(SAL_END)
#define _When_impl_(expr, annos)     _SA_annotes0(SAL_when(expr)) _Group_(annos)

#define _Use_decl_anno_impl_         __declspec("SAL_useHeader()") // this is a special case!

#define _Pre1_impl_(p1)              _Pre_impl_ p1
#define _Pre2_impl_(p1,p2)           _Pre_impl_ p1 _Pre_impl_ p2
#define _Pre3_impl_(p1,p2,p3)        _Pre_impl_ p1 _Pre_impl_ p2 _Pre_impl_ p3

#define _Post1_impl_(p1)             _Post_impl_ p1
#define _Post2_impl_(p1,p2)          _Post_impl_ p1 _Post_impl_ p2
#define _Post3_impl_(p1,p2,p3)       _Post_impl_ p1 _Post_impl_ p2 _Post_impl_ p3

#define _Ret1_impl_(p1)              _Post_impl_ p1
#define _Ret2_impl_(p1,p2)           _Post_impl_ p1 _Post_impl_ p2
#define _Ret3_impl_(p1,p2,p3)        _Post_impl_ p1 _Post_impl_ p2 _Post_impl_ p3

#define _Deref_pre1_impl_(p1)        _Deref_pre_impl_ p1
#define _Deref_pre2_impl_(p1,p2)     _Deref_pre_impl_ p1 _Deref_pre_impl_ p2
#define _Deref_pre3_impl_(p1,p2,p3)  _Deref_pre_impl_ p1 _Deref_pre_impl_ p2 _Deref_pre_impl_ p3

#define _Deref_post1_impl_(p1)       _Deref_post_impl_ p1
#define _Deref_post2_impl_(p1,p2)    _Deref_post_impl_ p1 _Deref_post_impl_ p2
#define _Deref_post3_impl_(p1,p2,p3) _Deref_post_impl_ p1 _Deref_post_impl_ p2 _Deref_post_impl_ p3

#define _Deref_ret1_impl_(p1)        _Deref_post_impl_ p1
#define _Deref_ret2_impl_(p1,p2)     _Deref_post_impl_ p1 _Deref_post_impl_ p2
#define _Deref_ret3_impl_(p1,p2,p3)  _Deref_post_impl_ p1 _Deref_post_impl_ p2 _Deref_post_impl_ p3

#define _Deref2_pre1_impl_(p1)       _Deref_pre_impl_ _Notref_impl_ _Deref_impl_ p1
#define _Deref2_post1_impl_(p1)      _Deref_post_impl_ _Notref_impl_ _Deref_impl_ p1
#define _Deref2_ret1_impl_(p1)       _Deref_post_impl_ _Notref_impl_ _Deref_impl_ p1

#define __inner_typefix(ctype)             _SA_annotes1(SAL_typefix, ctype)
#define __inner_exceptthat                 _SA_annotes0(SAL_except)

#define __specstrings

#ifdef  __cplusplus // [
#ifndef __nothrow // [
# define __nothrow __declspec(nothrow)
#endif // ]
extern "C" {
#else // ][
#ifndef __nothrow // [
# define __nothrow
#endif // ]
#endif // ]

    #define _SA_SPECSTRIZE( x ) #x

    #define __null                  _Null_impl_
    #define __notnull               _Notnull_impl_
    #define __maybenull             _Maybenull_impl_

    #define __readonly              _Pre1_impl_(__readaccess_impl)
    #define __notreadonly           _Pre1_impl_(__allaccess_impl)
    #define __maybereadonly         _Pre1_impl_(__readaccess_impl)

    #define __valid                 _Valid_impl_
    #define __notvalid              _Notvalid_impl_
    #define __maybevalid            _Maybevalid_impl_

    #define __readableTo(extent)    _SA_annotes1(SAL_readableTo, extent)
    #define __elem_readableTo(size)   _SA_annotes1(SAL_readableTo, elementCount( size ))
    #define __byte_readableTo(size)   _SA_annotes1(SAL_readableTo, byteCount(size))
    #define __writableTo(size)   _SA_annotes1(SAL_writableTo, size)
    #define __elem_writableTo(size)   _SA_annotes1(SAL_writableTo, elementCount( size ))
    #define __byte_writableTo(size)   _SA_annotes1(SAL_writableTo, byteCount( size))
    #define __deref                 _Deref_impl_
    #define __pre                   _Pre_impl_
    #define __post                  _Post_impl_
    #define __precond(expr)         __pre
    #define __postcond(expr)        __post
    #define __exceptthat                __inner_exceptthat
    #define __refparam                  _Notref_ __deref __notreadonly
    #define __inner_control_entrypoint(category) _SA_annotes2(SAL_entrypoint, controlEntry, category)
    #define __inner_data_entrypoint(category)    _SA_annotes2(SAL_entrypoint, dataEntry, category)

    #define __inner_override                    _SA_annotes0(__override)
    #define __inner_callback                    _SA_annotes0(__callback)
    #define __inner_blocksOn(resource)          _SA_annotes1(SAL_blocksOn, resource)
    #define __inner_fallthrough_dec             __inline __nothrow void __FallThrough() {}
    #define __inner_fallthrough                 __FallThrough();

    #define __post_except_maybenull     __post __inner_exceptthat _Maybenull_impl_
    #define __pre_except_maybenull      __pre  __inner_exceptthat _Maybenull_impl_

    #define __post_deref_except_maybenull       __post __deref __inner_exceptthat _Maybenull_impl_
    #define __pre_deref_except_maybenull    __pre  __deref __inner_exceptthat _Maybenull_impl_

    #define __inexpressible_readableTo(size)  _Readable_elements_impl_(_Inexpressible_(size))
    #define __inexpressible_writableTo(size)  _Writable_elements_impl_(_Inexpressible_(size))

#define __ecount(size)                                           _SAL1_Source_(__ecount, (size), __notnull __elem_writableTo(size))
#define __bcount(size)                                           _SAL1_Source_(__bcount, (size), __notnull __byte_writableTo(size))
#define __in                                                     _SAL1_Source_(__in, (), _In_)
#define __in_ecount(size)                                        _SAL1_Source_(__in_ecount, (size), _In_reads_(size))
#define __in_bcount(size)                                        _SAL1_Source_(__in_bcount, (size), _In_reads_bytes_(size))
#define __in_z                                                   _SAL1_Source_(__in_z, (), _In_z_)
#define __in_ecount_z(size)                                      _SAL1_Source_(__in_ecount_z, (size), _In_reads_z_(size))
#define __in_bcount_z(size)                                      _SAL1_Source_(__in_bcount_z, (size), __in_bcount(size) __pre __nullterminated)
#define __in_nz                                                  _SAL1_Source_(__in_nz, (), __in)
#define __in_ecount_nz(size)                                     _SAL1_Source_(__in_ecount_nz, (size), __in_ecount(size))
#define __in_bcount_nz(size)                                     _SAL1_Source_(__in_bcount_nz, (size), __in_bcount(size))
#define __out                                                    _SAL1_Source_(__out, (), _Out_)
#define __out_ecount(size)                                       _SAL1_Source_(__out_ecount, (size), _Out_writes_(size))
#define __out_bcount(size)                                       _SAL1_Source_(__out_bcount, (size), _Out_writes_bytes_(size))
#define __out_ecount_part(size,length)                           _SAL1_Source_(__out_ecount_part, (size,length), _Out_writes_to_(size,length))
#define __out_bcount_part(size,length)                           _SAL1_Source_(__out_bcount_part, (size,length), _Out_writes_bytes_to_(size,length))
#define __out_ecount_full(size)                                  _SAL1_Source_(__out_ecount_full, (size), _Out_writes_all_(size))
#define __out_bcount_full(size)                                  _SAL1_Source_(__out_bcount_full, (size), _Out_writes_bytes_all_(size))
#define __out_z                                                  _SAL1_Source_(__out_z, (), __post __valid __refparam __post __nullterminated)
#define __out_z_opt                                              _SAL1_Source_(__out_z_opt, (), __post __valid __refparam __post __nullterminated __pre_except_maybenull)
#define __out_ecount_z(size)                                     _SAL1_Source_(__out_ecount_z, (size), __ecount(size) __post __valid __refparam __post __nullterminated)
#define __out_bcount_z(size)                                     _SAL1_Source_(__out_bcount_z, (size), __bcount(size) __post __valid __refparam __post __nullterminated)
#define __out_ecount_part_z(size,length)                         _SAL1_Source_(__out_ecount_part_z, (size,length), __out_ecount_part(size,length) __post __nullterminated)
#define __out_bcount_part_z(size,length)                         _SAL1_Source_(__out_bcount_part_z, (size,length), __out_bcount_part(size,length) __post __nullterminated)
#define __out_ecount_full_z(size)                                _SAL1_Source_(__out_ecount_full_z, (size), __out_ecount_full(size) __post __nullterminated)
#define __out_bcount_full_z(size)                                _SAL1_Source_(__out_bcount_full_z, (size), __out_bcount_full(size) __post __nullterminated)
#define __out_nz                                                 _SAL1_Source_(__out_nz, (), __post __valid __refparam)
#define __out_nz_opt                                             _SAL1_Source_(__out_nz_opt, (), __post __valid __refparam __post_except_maybenull_)
#define __out_ecount_nz(size)                                    _SAL1_Source_(__out_ecount_nz, (size), __ecount(size) __post __valid __refparam)
#define __out_bcount_nz(size)                                    _SAL1_Source_(__out_bcount_nz, (size), __bcount(size) __post __valid __refparam)
#define __inout                                                  _SAL1_Source_(__inout, (), _Inout_)
#define __inout_ecount(size)                                     _SAL1_Source_(__inout_ecount, (size), _Inout_updates_(size))
#define __inout_bcount(size)                                     _SAL1_Source_(__inout_bcount, (size), _Inout_updates_bytes_(size))
#define __inout_ecount_part(size,length)                         _SAL1_Source_(__inout_ecount_part, (size,length), _Inout_updates_to_(size,length))
#define __inout_bcount_part(size,length)                         _SAL1_Source_(__inout_bcount_part, (size,length), _Inout_updates_bytes_to_(size,length))
#define __inout_ecount_full(size)                                _SAL1_Source_(__inout_ecount_full, (size), _Inout_updates_all_(size))
#define __inout_bcount_full(size)                                _SAL1_Source_(__inout_bcount_full, (size), _Inout_updates_bytes_all_(size))
#define __inout_z                                                _SAL1_Source_(__inout_z, (), _Inout_z_)
#define __inout_ecount_z(size)                                   _SAL1_Source_(__inout_ecount_z, (size), _Inout_updates_z_(size))
#define __inout_bcount_z(size)                                   _SAL1_Source_(__inout_bcount_z, (size), __inout_bcount(size) __pre __nullterminated __post __nullterminated)
#define __inout_nz                                               _SAL1_Source_(__inout_nz, (), __inout)
#define __inout_ecount_nz(size)                                  _SAL1_Source_(__inout_ecount_nz, (size), __inout_ecount(size))
#define __inout_bcount_nz(size)                                  _SAL1_Source_(__inout_bcount_nz, (size), __inout_bcount(size))
#define __ecount_opt(size)                                       _SAL1_Source_(__ecount_opt, (size), __ecount(size)                              __pre_except_maybenull)
#define __bcount_opt(size)                                       _SAL1_Source_(__bcount_opt, (size), __bcount(size)                              __pre_except_maybenull)
#define __in_opt                                                 _SAL1_Source_(__in_opt, (), _In_opt_)
#define __in_ecount_opt(size)                                    _SAL1_Source_(__in_ecount_opt, (size), _In_reads_opt_(size))
#define __in_bcount_opt(size)                                    _SAL1_Source_(__in_bcount_opt, (size), _In_reads_bytes_opt_(size))
#define __in_z_opt                                               _SAL1_Source_(__in_z_opt, (), _In_opt_z_)
#define __in_ecount_z_opt(size)                                  _SAL1_Source_(__in_ecount_z_opt, (size), __in_ecount_opt(size) __pre __nullterminated)
#define __in_bcount_z_opt(size)                                  _SAL1_Source_(__in_bcount_z_opt, (size), __in_bcount_opt(size) __pre __nullterminated)
#define __in_nz_opt                                              _SAL1_Source_(__in_nz_opt, (), __in_opt)
#define __in_ecount_nz_opt(size)                                 _SAL1_Source_(__in_ecount_nz_opt, (size), __in_ecount_opt(size))
#define __in_bcount_nz_opt(size)                                 _SAL1_Source_(__in_bcount_nz_opt, (size), __in_bcount_opt(size))
#define __out_opt                                                _SAL1_Source_(__out_opt, (), _Out_opt_)
#define __out_ecount_opt(size)                                   _SAL1_Source_(__out_ecount_opt, (size), _Out_writes_opt_(size))
#define __out_bcount_opt(size)                                   _SAL1_Source_(__out_bcount_opt, (size), _Out_writes_bytes_opt_(size))
#define __out_ecount_part_opt(size,length)                       _SAL1_Source_(__out_ecount_part_opt, (size,length), __out_ecount_part(size,length)              __pre_except_maybenull)
#define __out_bcount_part_opt(size,length)                       _SAL1_Source_(__out_bcount_part_opt, (size,length), __out_bcount_part(size,length)              __pre_except_maybenull)
#define __out_ecount_full_opt(size)                              _SAL1_Source_(__out_ecount_full_opt, (size), __out_ecount_full(size)                     __pre_except_maybenull)
#define __out_bcount_full_opt(size)                              _SAL1_Source_(__out_bcount_full_opt, (size), __out_bcount_full(size)                     __pre_except_maybenull)
#define __out_ecount_z_opt(size)                                 _SAL1_Source_(__out_ecount_z_opt, (size), __out_ecount_opt(size) __post __nullterminated)
#define __out_bcount_z_opt(size)                                 _SAL1_Source_(__out_bcount_z_opt, (size), __out_bcount_opt(size) __post __nullterminated)
#define __out_ecount_part_z_opt(size,length)                     _SAL1_Source_(__out_ecount_part_z_opt, (size,length), __out_ecount_part_opt(size,length) __post __nullterminated)
#define __out_bcount_part_z_opt(size,length)                     _SAL1_Source_(__out_bcount_part_z_opt, (size,length), __out_bcount_part_opt(size,length) __post __nullterminated)
#define __out_ecount_full_z_opt(size)                            _SAL1_Source_(__out_ecount_full_z_opt, (size), __out_ecount_full_opt(size) __post __nullterminated)
#define __out_bcount_full_z_opt(size)                            _SAL1_Source_(__out_bcount_full_z_opt, (size), __out_bcount_full_opt(size) __post __nullterminated)
#define __out_ecount_nz_opt(size)                                _SAL1_Source_(__out_ecount_nz_opt, (size), __out_ecount_opt(size) __post __nullterminated)
#define __out_bcount_nz_opt(size)                                _SAL1_Source_(__out_bcount_nz_opt, (size), __out_bcount_opt(size) __post __nullterminated)
#define __inout_opt                                              _SAL1_Source_(__inout_opt, (), _Inout_opt_)
#define __inout_ecount_opt(size)                                 _SAL1_Source_(__inout_ecount_opt, (size), __inout_ecount(size)                        __pre_except_maybenull)
#define __inout_bcount_opt(size)                                 _SAL1_Source_(__inout_bcount_opt, (size), __inout_bcount(size)                        __pre_except_maybenull)
#define __inout_ecount_part_opt(size,length)                     _SAL1_Source_(__inout_ecount_part_opt, (size,length), __inout_ecount_part(size,length)            __pre_except_maybenull)
#define __inout_bcount_part_opt(size,length)                     _SAL1_Source_(__inout_bcount_part_opt, (size,length), __inout_bcount_part(size,length)            __pre_except_maybenull)
#define __inout_ecount_full_opt(size)                            _SAL1_Source_(__inout_ecount_full_opt, (size), __inout_ecount_full(size)                   __pre_except_maybenull)
#define __inout_bcount_full_opt(size)                            _SAL1_Source_(__inout_bcount_full_opt, (size), __inout_bcount_full(size)                   __pre_except_maybenull)
#define __inout_z_opt                                            _SAL1_Source_(__inout_z_opt, (), __inout_opt __pre __nullterminated __post __nullterminated)
#define __inout_ecount_z_opt(size)                               _SAL1_Source_(__inout_ecount_z_opt, (size), __inout_ecount_opt(size) __pre __nullterminated __post __nullterminated)
#define __inout_ecount_z_opt(size)                               _SAL1_Source_(__inout_ecount_z_opt, (size), __inout_ecount_opt(size) __pre __nullterminated __post __nullterminated)
#define __inout_bcount_z_opt(size)                               _SAL1_Source_(__inout_bcount_z_opt, (size), __inout_bcount_opt(size))
#define __inout_nz_opt                                           _SAL1_Source_(__inout_nz_opt, (), __inout_opt)
#define __inout_ecount_nz_opt(size)                              _SAL1_Source_(__inout_ecount_nz_opt, (size), __inout_ecount_opt(size))
#define __inout_bcount_nz_opt(size)                              _SAL1_Source_(__inout_bcount_nz_opt, (size), __inout_bcount_opt(size))
#define __deref_ecount(size)                                     _SAL1_Source_(__deref_ecount, (size), _Notref_ __ecount(1) __post _Notref_ __elem_readableTo(1) __post _Notref_ __deref _Notref_ __notnull __post __deref __elem_writableTo(size))
#define __deref_bcount(size)                                     _SAL1_Source_(__deref_bcount, (size), _Notref_ __ecount(1) __post _Notref_ __elem_readableTo(1) __post _Notref_ __deref _Notref_ __notnull __post __deref __byte_writableTo(size))
#define __deref_out                                              _SAL1_Source_(__deref_out, (), _Outptr_)
#define __deref_out_ecount(size)                                 _SAL1_Source_(__deref_out_ecount, (size), _Outptr_result_buffer_(size))
#define __deref_out_bcount(size)                                 _SAL1_Source_(__deref_out_bcount, (size), _Outptr_result_bytebuffer_(size))
#define __deref_out_ecount_part(size,length)                     _SAL1_Source_(__deref_out_ecount_part, (size,length), _Outptr_result_buffer_to_(size,length))
#define __deref_out_bcount_part(size,length)                     _SAL1_Source_(__deref_out_bcount_part, (size,length), _Outptr_result_bytebuffer_to_(size,length))
#define __deref_out_ecount_full(size)                            _SAL1_Source_(__deref_out_ecount_full, (size), __deref_out_ecount_part(size,size))
#define __deref_out_bcount_full(size)                            _SAL1_Source_(__deref_out_bcount_full, (size), __deref_out_bcount_part(size,size))
#define __deref_out_z                                            _SAL1_Source_(__deref_out_z, (), _Outptr_result_z_)
#define __deref_out_ecount_z(size)                               _SAL1_Source_(__deref_out_ecount_z, (size), __deref_out_ecount(size) __post __deref __nullterminated)
#define __deref_out_bcount_z(size)                               _SAL1_Source_(__deref_out_bcount_z, (size), __deref_out_bcount(size) __post __deref __nullterminated)
#define __deref_out_nz                                           _SAL1_Source_(__deref_out_nz, (), __deref_out)
#define __deref_out_ecount_nz(size)                              _SAL1_Source_(__deref_out_ecount_nz, (size), __deref_out_ecount(size))
#define __deref_out_bcount_nz(size)                              _SAL1_Source_(__deref_out_bcount_nz, (size), __deref_out_ecount(size))
#define __deref_inout                                            _SAL1_Source_(__deref_inout, (), _Notref_ __notnull _Notref_ __elem_readableTo(1) __pre __deref __valid __post _Notref_ __deref __valid __refparam)
#define __deref_inout_z                                          _SAL1_Source_(__deref_inout_z, (), __deref_inout __pre __deref __nullterminated __post _Notref_ __deref __nullterminated)
#define __deref_inout_ecount(size)                               _SAL1_Source_(__deref_inout_ecount, (size), __deref_inout __pre __deref __elem_writableTo(size) __post _Notref_ __deref __elem_writableTo(size))
#define __deref_inout_bcount(size)                               _SAL1_Source_(__deref_inout_bcount, (size), __deref_inout __pre __deref __byte_writableTo(size) __post _Notref_ __deref __byte_writableTo(size))
#define __deref_inout_ecount_part(size,length)                   _SAL1_Source_(__deref_inout_ecount_part, (size,length), __deref_inout_ecount(size) __pre __deref __elem_readableTo(length) __post __deref __elem_readableTo(length))
#define __deref_inout_bcount_part(size,length)                   _SAL1_Source_(__deref_inout_bcount_part, (size,length), __deref_inout_bcount(size) __pre __deref __byte_readableTo(length) __post __deref __byte_readableTo(length))
#define __deref_inout_ecount_full(size)                          _SAL1_Source_(__deref_inout_ecount_full, (size), __deref_inout_ecount_part(size,size))
#define __deref_inout_bcount_full(size)                          _SAL1_Source_(__deref_inout_bcount_full, (size), __deref_inout_bcount_part(size,size))
#define __deref_inout_ecount_z(size)                             _SAL1_Source_(__deref_inout_ecount_z, (size), __deref_inout_ecount(size) __pre __deref __nullterminated __post __deref __nullterminated)
#define __deref_inout_bcount_z(size)                             _SAL1_Source_(__deref_inout_bcount_z, (size), __deref_inout_bcount(size) __pre __deref __nullterminated __post __deref __nullterminated)
#define __deref_inout_nz                                         _SAL1_Source_(__deref_inout_nz, (), __deref_inout)
#define __deref_inout_ecount_nz(size)                            _SAL1_Source_(__deref_inout_ecount_nz, (size), __deref_inout_ecount(size))
#define __deref_inout_bcount_nz(size)                            _SAL1_Source_(__deref_inout_bcount_nz, (size), __deref_inout_ecount(size))
#define __deref_ecount_opt(size)                                 _SAL1_Source_(__deref_ecount_opt, (size), __deref_ecount(size)                        __post_deref_except_maybenull)
#define __deref_bcount_opt(size)                                 _SAL1_Source_(__deref_bcount_opt, (size), __deref_bcount(size)                        __post_deref_except_maybenull)
#define __deref_out_opt                                          _SAL1_Source_(__deref_out_opt, (), __deref_out                                 __post_deref_except_maybenull)
#define __deref_out_ecount_opt(size)                             _SAL1_Source_(__deref_out_ecount_opt, (size), __deref_out_ecount(size)                    __post_deref_except_maybenull)
#define __deref_out_bcount_opt(size)                             _SAL1_Source_(__deref_out_bcount_opt, (size), __deref_out_bcount(size)                    __post_deref_except_maybenull)
#define __deref_out_ecount_part_opt(size,length)                 _SAL1_Source_(__deref_out_ecount_part_opt, (size,length), __deref_out_ecount_part(size,length)        __post_deref_except_maybenull)
#define __deref_out_bcount_part_opt(size,length)                 _SAL1_Source_(__deref_out_bcount_part_opt, (size,length), __deref_out_bcount_part(size,length)        __post_deref_except_maybenull)
#define __deref_out_ecount_full_opt(size)                        _SAL1_Source_(__deref_out_ecount_full_opt, (size), __deref_out_ecount_full(size)               __post_deref_except_maybenull)
#define __deref_out_bcount_full_opt(size)                        _SAL1_Source_(__deref_out_bcount_full_opt, (size), __deref_out_bcount_full(size)               __post_deref_except_maybenull)
#define __deref_out_z_opt                                        _SAL1_Source_(__deref_out_z_opt, (), _Outptr_result_maybenull_z_)
#define __deref_out_ecount_z_opt(size)                           _SAL1_Source_(__deref_out_ecount_z_opt, (size), __deref_out_ecount_opt(size) __post __deref __nullterminated)
#define __deref_out_bcount_z_opt(size)                           _SAL1_Source_(__deref_out_bcount_z_opt, (size), __deref_out_bcount_opt(size) __post __deref __nullterminated)
#define __deref_out_nz_opt                                       _SAL1_Source_(__deref_out_nz_opt, (), __deref_out_opt)
#define __deref_out_ecount_nz_opt(size)                          _SAL1_Source_(__deref_out_ecount_nz_opt, (size), __deref_out_ecount_opt(size))
#define __deref_out_bcount_nz_opt(size)                          _SAL1_Source_(__deref_out_bcount_nz_opt, (size), __deref_out_bcount_opt(size))
#define __deref_inout_opt                                        _SAL1_Source_(__deref_inout_opt, (), __deref_inout                               __pre_deref_except_maybenull __post_deref_except_maybenull)
#define __deref_inout_ecount_opt(size)                           _SAL1_Source_(__deref_inout_ecount_opt, (size), __deref_inout_ecount(size)                  __pre_deref_except_maybenull __post_deref_except_maybenull)
#define __deref_inout_bcount_opt(size)                           _SAL1_Source_(__deref_inout_bcount_opt, (size), __deref_inout_bcount(size)                  __pre_deref_except_maybenull __post_deref_except_maybenull)
#define __deref_inout_ecount_part_opt(size,length)               _SAL1_Source_(__deref_inout_ecount_part_opt, (size,length), __deref_inout_ecount_part(size,length)      __pre_deref_except_maybenull __post_deref_except_maybenull)
#define __deref_inout_bcount_part_opt(size,length)               _SAL1_Source_(__deref_inout_bcount_part_opt, (size,length), __deref_inout_bcount_part(size,length)      __pre_deref_except_maybenull __post_deref_except_maybenull)
#define __deref_inout_ecount_full_opt(size)                      _SAL1_Source_(__deref_inout_ecount_full_opt, (size), __deref_inout_ecount_full(size)             __pre_deref_except_maybenull __post_deref_except_maybenull)
#define __deref_inout_bcount_full_opt(size)                      _SAL1_Source_(__deref_inout_bcount_full_opt, (size), __deref_inout_bcount_full(size)             __pre_deref_except_maybenull __post_deref_except_maybenull)
#define __deref_inout_z_opt                                      _SAL1_Source_(__deref_inout_z_opt, (), __deref_inout_opt __pre __deref __nullterminated __post __deref __nullterminated)
#define __deref_inout_ecount_z_opt(size)                         _SAL1_Source_(__deref_inout_ecount_z_opt, (size), __deref_inout_ecount_opt(size) __pre __deref __nullterminated __post __deref __nullterminated)
#define __deref_inout_bcount_z_opt(size)                         _SAL1_Source_(__deref_inout_bcount_z_opt, (size), __deref_inout_bcount_opt(size) __pre __deref __nullterminated __post __deref __nullterminated)
#define __deref_inout_nz_opt                                     _SAL1_Source_(__deref_inout_nz_opt, (), __deref_inout_opt)
#define __deref_inout_ecount_nz_opt(size)                        _SAL1_Source_(__deref_inout_ecount_nz_opt, (size), __deref_inout_ecount_opt(size))
#define __deref_inout_bcount_nz_opt(size)                        _SAL1_Source_(__deref_inout_bcount_nz_opt, (size), __deref_inout_bcount_opt(size))
#define __deref_opt_ecount(size)                                 _SAL1_Source_(__deref_opt_ecount, (size), __deref_ecount(size)                        __pre_except_maybenull)
#define __deref_opt_bcount(size)                                 _SAL1_Source_(__deref_opt_bcount, (size), __deref_bcount(size)                        __pre_except_maybenull)
#define __deref_opt_out                                          _SAL1_Source_(__deref_opt_out, (), _Outptr_opt_)
#define __deref_opt_out_z                                        _SAL1_Source_(__deref_opt_out_z, (), _Outptr_opt_result_z_)
#define __deref_opt_out_ecount(size)                             _SAL1_Source_(__deref_opt_out_ecount, (size), __deref_out_ecount(size)                    __pre_except_maybenull)
#define __deref_opt_out_bcount(size)                             _SAL1_Source_(__deref_opt_out_bcount, (size), __deref_out_bcount(size)                    __pre_except_maybenull)
#define __deref_opt_out_ecount_part(size,length)                 _SAL1_Source_(__deref_opt_out_ecount_part, (size,length), __deref_out_ecount_part(size,length)        __pre_except_maybenull)
#define __deref_opt_out_bcount_part(size,length)                 _SAL1_Source_(__deref_opt_out_bcount_part, (size,length), __deref_out_bcount_part(size,length)        __pre_except_maybenull)
#define __deref_opt_out_ecount_full(size)                        _SAL1_Source_(__deref_opt_out_ecount_full, (size), __deref_out_ecount_full(size)               __pre_except_maybenull)
#define __deref_opt_out_bcount_full(size)                        _SAL1_Source_(__deref_opt_out_bcount_full, (size), __deref_out_bcount_full(size)               __pre_except_maybenull)
#define __deref_opt_inout                                        _SAL1_Source_(__deref_opt_inout, (), _Inout_opt_)
#define __deref_opt_inout_ecount(size)                           _SAL1_Source_(__deref_opt_inout_ecount, (size), __deref_inout_ecount(size)                  __pre_except_maybenull)
#define __deref_opt_inout_bcount(size)                           _SAL1_Source_(__deref_opt_inout_bcount, (size), __deref_inout_bcount(size)                  __pre_except_maybenull)
#define __deref_opt_inout_ecount_part(size,length)               _SAL1_Source_(__deref_opt_inout_ecount_part, (size,length), __deref_inout_ecount_part(size,length)      __pre_except_maybenull)
#define __deref_opt_inout_bcount_part(size,length)               _SAL1_Source_(__deref_opt_inout_bcount_part, (size,length), __deref_inout_bcount_part(size,length)      __pre_except_maybenull)
#define __deref_opt_inout_ecount_full(size)                      _SAL1_Source_(__deref_opt_inout_ecount_full, (size), __deref_inout_ecount_full(size)             __pre_except_maybenull)
#define __deref_opt_inout_bcount_full(size)                      _SAL1_Source_(__deref_opt_inout_bcount_full, (size), __deref_inout_bcount_full(size)             __pre_except_maybenull)
#define __deref_opt_inout_z                                      _SAL1_Source_(__deref_opt_inout_z, (), __deref_opt_inout __pre __deref __nullterminated __post __deref __nullterminated)
#define __deref_opt_inout_ecount_z(size)                         _SAL1_Source_(__deref_opt_inout_ecount_z, (size), __deref_opt_inout_ecount(size) __pre __deref __nullterminated __post __deref __nullterminated)
#define __deref_opt_inout_bcount_z(size)                         _SAL1_Source_(__deref_opt_inout_bcount_z, (size), __deref_opt_inout_bcount(size) __pre __deref __nullterminated __post __deref __nullterminated)
#define __deref_opt_inout_nz                                     _SAL1_Source_(__deref_opt_inout_nz, (), __deref_opt_inout)
#define __deref_opt_inout_ecount_nz(size)                        _SAL1_Source_(__deref_opt_inout_ecount_nz, (size), __deref_opt_inout_ecount(size))
#define __deref_opt_inout_bcount_nz(size)                        _SAL1_Source_(__deref_opt_inout_bcount_nz, (size), __deref_opt_inout_bcount(size))
#define __deref_opt_ecount_opt(size)                             _SAL1_Source_(__deref_opt_ecount_opt, (size), __deref_ecount_opt(size)                    __pre_except_maybenull)
#define __deref_opt_bcount_opt(size)                             _SAL1_Source_(__deref_opt_bcount_opt, (size), __deref_bcount_opt(size)                    __pre_except_maybenull)
#define __deref_opt_out_opt                                      _SAL1_Source_(__deref_opt_out_opt, (), _Outptr_opt_result_maybenull_)
#define __deref_opt_out_ecount_opt(size)                         _SAL1_Source_(__deref_opt_out_ecount_opt, (size), __deref_out_ecount_opt(size)                __pre_except_maybenull)
#define __deref_opt_out_bcount_opt(size)                         _SAL1_Source_(__deref_opt_out_bcount_opt, (size), __deref_out_bcount_opt(size)                __pre_except_maybenull)
#define __deref_opt_out_ecount_part_opt(size,length)             _SAL1_Source_(__deref_opt_out_ecount_part_opt, (size,length), __deref_out_ecount_part_opt(size,length)    __pre_except_maybenull)
#define __deref_opt_out_bcount_part_opt(size,length)             _SAL1_Source_(__deref_opt_out_bcount_part_opt, (size,length), __deref_out_bcount_part_opt(size,length)    __pre_except_maybenull)
#define __deref_opt_out_ecount_full_opt(size)                    _SAL1_Source_(__deref_opt_out_ecount_full_opt, (size), __deref_out_ecount_full_opt(size)           __pre_except_maybenull)
#define __deref_opt_out_bcount_full_opt(size)                    _SAL1_Source_(__deref_opt_out_bcount_full_opt, (size), __deref_out_bcount_full_opt(size)           __pre_except_maybenull)
#define __deref_opt_out_z_opt                                    _SAL1_Source_(__deref_opt_out_z_opt, (), __post __deref __valid __refparam __pre_except_maybenull __pre_deref_except_maybenull __post_deref_except_maybenull __post __deref __nullterminated)
#define __deref_opt_out_ecount_z_opt(size)                       _SAL1_Source_(__deref_opt_out_ecount_z_opt, (size), __deref_opt_out_ecount_opt(size) __post __deref __nullterminated)
#define __deref_opt_out_bcount_z_opt(size)                       _SAL1_Source_(__deref_opt_out_bcount_z_opt, (size), __deref_opt_out_bcount_opt(size) __post __deref __nullterminated)
#define __deref_opt_out_nz_opt                                   _SAL1_Source_(__deref_opt_out_nz_opt, (), __deref_opt_out_opt)
#define __deref_opt_out_ecount_nz_opt(size)                      _SAL1_Source_(__deref_opt_out_ecount_nz_opt, (size), __deref_opt_out_ecount_opt(size))
#define __deref_opt_out_bcount_nz_opt(size)                      _SAL1_Source_(__deref_opt_out_bcount_nz_opt, (size), __deref_opt_out_bcount_opt(size))
#define __deref_opt_inout_opt                                    _SAL1_Source_(__deref_opt_inout_opt, (), __deref_inout_opt                           __pre_except_maybenull)
#define __deref_opt_inout_ecount_opt(size)                       _SAL1_Source_(__deref_opt_inout_ecount_opt, (size), __deref_inout_ecount_opt(size)              __pre_except_maybenull)
#define __deref_opt_inout_bcount_opt(size)                       _SAL1_Source_(__deref_opt_inout_bcount_opt, (size), __deref_inout_bcount_opt(size)              __pre_except_maybenull)
#define __deref_opt_inout_ecount_part_opt(size,length)           _SAL1_Source_(__deref_opt_inout_ecount_part_opt, (size,length), __deref_inout_ecount_part_opt(size,length)  __pre_except_maybenull)
#define __deref_opt_inout_bcount_part_opt(size,length)           _SAL1_Source_(__deref_opt_inout_bcount_part_opt, (size,length), __deref_inout_bcount_part_opt(size,length)  __pre_except_maybenull)
#define __deref_opt_inout_ecount_full_opt(size)                  _SAL1_Source_(__deref_opt_inout_ecount_full_opt, (size), __deref_inout_ecount_full_opt(size)         __pre_except_maybenull)
#define __deref_opt_inout_bcount_full_opt(size)                  _SAL1_Source_(__deref_opt_inout_bcount_full_opt, (size), __deref_inout_bcount_full_opt(size)         __pre_except_maybenull)
#define __deref_opt_inout_z_opt                                  _SAL1_Source_(__deref_opt_inout_z_opt, (), __deref_opt_inout_opt  __pre __deref __nullterminated __post __deref __nullterminated)
#define __deref_opt_inout_ecount_z_opt(size)                     _SAL1_Source_(__deref_opt_inout_ecount_z_opt, (size), __deref_opt_inout_ecount_opt(size)  __pre __deref __nullterminated __post __deref __nullterminated)
#define __deref_opt_inout_bcount_z_opt(size)                     _SAL1_Source_(__deref_opt_inout_bcount_z_opt, (size), __deref_opt_inout_bcount_opt(size)  __pre __deref __nullterminated __post __deref __nullterminated)
#define __deref_opt_inout_nz_opt                                 _SAL1_Source_(__deref_opt_inout_nz_opt, (), __deref_opt_inout_opt)
#define __deref_opt_inout_ecount_nz_opt(size)                    _SAL1_Source_(__deref_opt_inout_ecount_nz_opt, (size), __deref_opt_inout_ecount_opt(size))
#define __deref_opt_inout_bcount_nz_opt(size)                    _SAL1_Source_(__deref_opt_inout_bcount_nz_opt, (size), __deref_opt_inout_bcount_opt(size))

#define __success(expr)                      _SAL1_1_Source_(__success, (expr), _Success_(expr))
#define __nullterminated                     _SAL1_Source_(__nullterminated, (), _Null_terminated_)
#define __nullnullterminated                 _SAL1_Source_(__nullnulltermiated, (), _SAL_nop_impl_)
#define __reserved                           _SAL1_Source_(__reserved, (), _Reserved_)
#define __checkReturn                        _SAL1_Source_(__checkReturn, (), _Check_return_)
#define __typefix(ctype)                     _SAL1_Source_(__typefix, (ctype), __inner_typefix(ctype))
#define __override                           __inner_override
#define __callback                           __inner_callback
#define __format_string                      _SAL1_1_Source_(__format_string, (), _Printf_format_string_)
#define __blocksOn(resource)                 _SAL_L_Source_(__blocksOn, (resource), __inner_blocksOn(resource))
#define __control_entrypoint(category)       _SAL_L_Source_(__control_entrypoint, (category), __inner_control_entrypoint(category))
#define __data_entrypoint(category)          _SAL_L_Source_(__data_entrypoint, (category), __inner_data_entrypoint(category))

#ifdef _USING_V110_SDK71_ // [
#ifndef _PREFAST_ // [
#define __useHeader
#else // ][
#error Code analysis is not supported when using Visual C++ 11.0/12.0 with the Windows 7.1 SDK.
#endif // ]
#else // ][
#define __useHeader                          _Use_decl_anno_impl_
#endif // ]

#ifdef _USING_V110_SDK71_ // [
#ifndef _PREFAST_ // [
#define __on_failure(annotes)
#else // ][
#error Code analysis is not supported when using Visual C++ 11.0/12.0 with the Windows 7.1 SDK.
#endif // ]
#else // ][
#define __on_failure(annotes)                _SAL1_1_Source_(__on_failure, (annotes), _On_failure_impl_(annotes _SAL_nop_impl_))
#endif // ]

#ifndef __fallthrough // [
    __inner_fallthrough_dec
    #define __fallthrough __inner_fallthrough
#endif // ]

#ifndef __analysis_assume // [
#ifdef _PREFAST_ // [
#define __analysis_assume(expr) __assume(expr)
#else // ][
#define __analysis_assume(expr) 
#endif // ]
#endif // ]

#ifndef _Analysis_assume_ // [
#ifdef _PREFAST_ // [
#define _Analysis_assume_(expr) __assume(expr)
#else // ][
#define _Analysis_assume_(expr) 
#endif // ]
#endif // ]

#define _Analysis_noreturn_    _SAL2_Source_(_Analysis_noreturn_, (), _SA_annotes0(SAL_terminates))

__inline __nothrow 
void __AnalysisAssumeNullterminated(_Post_ _Null_terminated_ void *p);

#define _Analysis_assume_nullterminated_(x) __AnalysisAssumeNullterminated(x)

#define ___MKID(x, y) x ## y
#define __MKID(x, y) ___MKID(x, y)
#define __GENSYM(x) __MKID(x, __COUNTER__)

__ANNOTATION(SAL_analysisMode(__AuToQuOtE __In_impl_ char *mode);)

#define _Analysis_mode_impl_(mode) _SAL2_Source_(_Analysis_mode_impl_, (mode), _SA_annotes1(SAL_analysisMode, #mode))

#ifndef _M_IX86 // [

#define _Analysis_mode_(mode)                                                 \
    __pragma(warning(disable: 28110 28111 28161 28162))                       \
    typedef _Analysis_mode_impl_(mode) int                                    \
        __GENSYM(__prefast_analysis_mode_flag);

#else // ][

#define _Analysis_mode_(mode)                                                 \
    typedef _Analysis_mode_impl_(mode) int                                    \
        __GENSYM(__prefast_analysis_mode_flag);

#endif // ]

__ANNOTATION(SAL_functionClassNew(__In_impl_ char*);)
__PRIMOP(int, _In_function_class_(__In_impl_ char*);)
#define _In_function_class_(x)  _In_function_class_(#x)

#define _Function_class_(x)  _SAL2_Source_(_Function_class_, (x), _SA_annotes1(SAL_functionClassNew, _SA_SPECSTRIZE(x)))

#define _Enum_is_bitflag_    _SAL2_Source_(_Enum_is_bitflag_, (), _SA_annotes0(SAL_enumIsBitflag))
#define _Strict_type_match_  _SAL2_Source_(_Strict_type_match, (), _SA_annotes0(SAL_strictType2))

#define _Maybe_raises_SEH_exception_   _SAL2_Source_(_Maybe_raises_SEH_exception_, (x), _Pre_ _SA_annotes1(SAL_inTry,__yes))
#define _Raises_SEH_exception_         _SAL2_Source_(_Raises_SEH_exception_, (x), _Maybe_raises_SEH_exception_ _Analysis_noreturn_)

#ifdef  __cplusplus // [
}
#endif // ]

#include <ConcurrencySal.h>
