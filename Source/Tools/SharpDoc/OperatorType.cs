// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.


namespace SharpDoc
{
    #region OperatorType enumeration
    public enum OperatorType
    {
        None,
        op_Implicit,
        op_Explicit,

        // overloadable unary operators
        op_Decrement,                   // --
        op_Increment,                   // ++
        op_UnaryNegation,               // -
        op_UnaryPlus,                   // +
        op_LogicalNot,                  // !
        op_True,                        // true
        op_False,                       // false
        op_OnesComplement,              // ~
        op_Like,                        // Like (Visual Basic)


        // overloadable binary operators
        op_Addition,                    // +
        op_Subtraction,                 // -
        op_Division,                    // /
        op_Multiply,                    // *
        op_Modulus,                     // %
        op_BitwiseAnd,                  // &
        op_ExclusiveOr,                 // ^
        op_LeftShift,                   // <<
        op_RightShift,                  // >>
        op_BitwiseOr,                   // |

        // overloadable comparision operators
        op_Equality,                    // ==
        op_Inequality,                  // != 
        op_LessThanOrEqual,             // <=
        op_LessThan,                    // <
        op_GreaterThanOrEqual,          // >=
        op_GreaterThan,                 // >

        // not overloadable operators
        op_AddressOf,                       // &
        op_PointerDereference,              // *
        op_LogicalAnd,                      // &&
        op_LogicalOr,                       // ||
        op_Assign,                          // Not defined (= is not the same)
        op_SignedRightShift,                // Not defined
        op_UnsignedRightShift,              // Not defined
        op_UnsignedRightShiftAssignment,    // Not defined
        op_MemberSelection,                 // ->
        op_RightShiftAssignment,            // >>=
        op_MultiplicationAssignment,        // *=
        op_PointerToMemberSelection,        // ->*
        op_SubtractionAssignment,           // -=
        op_ExclusiveOrAssignment,           // ^=
        op_LeftShiftAssignment,             // <<=
        op_ModulusAssignment,               // %=
        op_AdditionAssignment,              // +=
        op_BitwiseAndAssignment,            // &=
        op_BitwiseOrAssignment,             // |=
        op_Comma,                           // ,
        op_DivisionAssignment               // /=
    }
    #endregion
}
