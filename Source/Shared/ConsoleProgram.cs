// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace SharpDX
{
    /// <summary>Reusable, reflection based helper for parsing command line options.
    /// Greetings to Shawn Hargreaves, original code http://blogs.msdn.com/b/shawnhar/archive/2012/04/20/a-reusable-reflection-based-command-line-parser.aspx
    /// This is a modified version of command line parser that adds:
    /// - .NET 2.0 compatible
    /// - Allow inheritance to simplify declaration
    /// - Print exe banner, using AssemblyTitle and AssemblyCopyright.
    /// - Better padding of options, add descriptor and value text overrides.
    /// - Add support for - and / starting options.
    /// - Remove usage of ":" to separate option from parsed option
    /// - Add "&lt;options&gt;" to the Usage when options are defined
    /// - Add Console Color handling</summary>
    /// <remarks>This single file is intended to be directly included in the project that needs to handle command line without requiring any SharpDX assembly dependencies.</remarks>
    class ConsoleProgram
    {
        /// <summary>The constant d_ outpu t_ handle.</summary>
        private const int StdOutputHandle = -11;
        /// <summary>The authentication console handle.</summary>
        private static readonly int hConsoleHandle;
        /// <summary>The console output location.</summary>
        //private static COORD ConsoleOutputLocation;
        /// <summary>The console information.</summary>
        private static CONSOLE_SCREEN_BUFFER_INFO ConsoleInfo;
        /// <summary>The original colors.</summary>
        private static readonly int OriginalColors;
        /// <summary>The optional options.</summary>
        private readonly Dictionary<string, FieldInfo> optionalOptions;
        /// <summary>The optional usage help.</summary>
        private readonly List<string> optionalUsageHelp;
        /// <summary>The options object.</summary>
        private readonly object optionsObject;
        /// <summary>The option names.</summary>
        private readonly string[] optionNames;

        /// <summary>The required options.</summary>
        private readonly Queue<FieldInfo> requiredOptions;

        /// <summary>The required usage help.</summary>
        private readonly List<string> requiredUsageHelp;

        /// <summary>Initializes static members of the <see cref="ConsoleProgram"/> class.</summary>
        static ConsoleProgram()
        {
            ConsoleInfo = new CONSOLE_SCREEN_BUFFER_INFO();
            //ConsoleOutputLocation = new COORD();
            hConsoleHandle = GetStdHandle(StdOutputHandle);
            GetConsoleScreenBufferInfo(hConsoleHandle, ref ConsoleInfo);
            OriginalColors = ConsoleInfo.wAttributes;
        }

        /// <summary>Initializes a new instance of the <see cref="ConsoleProgram"/> class.</summary>
        /// <param name="padOptions">The pad options.</param>
        protected ConsoleProgram(int padOptions = 16) : this(null, padOptions)
        {
        }

        // Constructor.
        /// <summary>Initializes a new instance of the <see cref="ConsoleProgram"/> class.</summary>
        /// <param name="optionsObjectArg">The options object argument.</param>
        /// <param name="padOptions">The pad options.</param>
        private ConsoleProgram(object optionsObjectArg, int padOptions = 16)
        {
            this.requiredUsageHelp = new List<string>();
            this.requiredOptions = new Queue<FieldInfo>();
            this.optionalUsageHelp = new List<string>();
            this.optionalOptions = new Dictionary<string, FieldInfo>();
            this.optionsObject = optionsObjectArg ?? this;

            // Reflect to find what command line options are available.
            foreach (FieldInfo field in optionsObject.GetType().GetFields())
            {
                var option = GetOptionName(field);

                var optionName = option.Name;

                if (option.Required)
                {
                    // Record a required option.
                    requiredOptions.Enqueue(field);

                    requiredUsageHelp.Add(string.Format("<{0}>", option.Name));
                }
                else
                {
                    // Record an optional option.
                    optionalOptions.Add(optionName, field);

                    if (field.FieldType == typeof (bool))
                    {
                        optionalUsageHelp.Add(string.Format("/{0,-" + padOptions + "}{1}", optionName, option.Description ?? string.Empty));
                    }
                    else
                    {
                        optionalUsageHelp.Add(string.Format("/{0,-" + padOptions + "}{1}", string.Format("{0}{1}", optionName, option.Value ?? "<value>"), option.Description ?? string.Empty));
                    }
                }
            }

            optionNames = new string[optionalOptions.Count];
            optionalOptions.Keys.CopyTo(optionNames, 0);
            Array.Sort(optionNames, (left, right) => -string.Compare(left, right, StringComparison.Ordinal));

            if (optionalOptions.Count > 0)
            {
                requiredUsageHelp.Insert(0, "<options>");
            }
        }

        /// <summary>Prints the header.</summary>
        public static void PrintHeader()
        {
            Console.WriteLine("{0} - {1}", GetAssemblyTitle(), Assembly.GetEntryAssembly().GetName().Version);
            Console.WriteLine("{0}", GetAssemblyCopyright());
            Console.WriteLine();            
        }

        /// <summary>Parses the command line.</summary>
        /// <param name="options">The options.</param>
        /// <param name="args">The arguments.</param>
        /// <param name="padOptions">The pad options.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool ParseCommandLine(object options, string[] args, int padOptions = 16)
        {
            PrintHeader();

            var cmdParser = new ConsoleProgram(options, padOptions);
            return cmdParser.ParseCommandLine(args);
        }

        /// <summary>Parses the command line.</summary>
        /// <param name="args">The arguments.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected bool ParseCommandLine(string[] args)
        {
            // Parse each argument in turn.
            foreach (string arg in args)
            {
                if (!ParseArgument(arg.Trim()))
                {
                    return false;
                }
            }

            // Make sure we got all the required options.
            FieldInfo missingRequiredOption = null;

            foreach (var field in requiredOptions)
            {
                if (!IsList(field) || GetList(field).Count == 0)
                {
                    missingRequiredOption = field;
                    break;
                }
            }

            if (missingRequiredOption != null)
            {
                ShowError("Missing argument '{0}'", GetOptionName(missingRequiredOption).Name);
                return false;
            }

            return true;
        }

        /// <summary>Parses the argument.</summary>
        /// <param name="arg">The argument.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ParseArgument(string arg)
        {
            if (arg.StartsWith("/") || arg.StartsWith("-"))
            {
                string name = arg.Substring(1);

                string value = null;

                FieldInfo field = null;



                foreach (var registerName in optionNames)
                {
                    if (name.StartsWith(registerName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        field = optionalOptions[registerName];
                        value = name.Substring(registerName.Length);
                        break;
                    }
                }

                if (field == null)
                {
                    ShowError("Unknown option '{0}'", name);
                    return false;
                }

                if (string.IsNullOrEmpty(value))
                {
                    value = "true";
                }

                return SetOption(field, value);
            }
            else
            {
                // Parse a required argument.
                if (requiredOptions.Count == 0)
                {
                    ShowError("Too many arguments");
                    return false;
                }

                FieldInfo field = requiredOptions.Peek();

                if (!IsList(field))
                {
                    requiredOptions.Dequeue();
                }

                return SetOption(field, arg);
            }
        }

        /// <summary>Sets the option.</summary>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool SetOption(FieldInfo field, string value)
        {
            try
            {
                if (IsList(field))
                {
                    // Append this value to a list of options.
                    GetList(field).Add(ChangeType(value, ListElementType(field)));
                }
                else
                {
                    // Set the value of a single option.
                    field.SetValue(optionsObject, ChangeType(value, field.FieldType));
                }

                return true;
            }
            catch
            {
                ShowError("Invalid value '{0}' for option '{1}'", value, GetOptionName(field).Name);
                return false;
            }
        }


        /// <summary>Changes the type.</summary>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        /// <returns>System.Object.</returns>
        private static object ChangeType(string value, Type type)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(type);

            return converter.ConvertFromInvariantString(value);
        }


        /// <summary>Determines whether the specified field is list.</summary>
        /// <param name="field">The field.</param>
        /// <returns><see langword="true" /> if the specified field is list; otherwise, <see langword="false" />.</returns>
        private static bool IsList(FieldInfo field)
        {
            return typeof (IList).IsAssignableFrom(field.FieldType);
        }


        /// <summary>Gets the list.</summary>
        /// <param name="field">The field.</param>
        /// <returns>IList.</returns>
        private IList GetList(FieldInfo field)
        {
            return (IList) field.GetValue(optionsObject);
        }


        /// <summary>Lists the type of the element.</summary>
        /// <param name="field">The field.</param>
        /// <returns>Type.</returns>
        private static Type ListElementType(FieldInfo field)
        {
            foreach (var fieldInterface in field.FieldType.GetInterfaces())
            {
                if (fieldInterface.IsGenericType && fieldInterface.GetGenericTypeDefinition() == typeof (IEnumerable<>))
                {
                    return fieldInterface.GetGenericArguments()[0];
                }
            }

            return null;
        }

        /// <summary>Gets the name of the option.</summary>
        /// <param name="field">The field.</param>
        /// <returns>OptionAttribute.</returns>
        private static OptionAttribute GetOptionName(FieldInfo field)
        {
            return GetAttribute<OptionAttribute>(field) ?? new OptionAttribute(field.Name);
        }

        /// <summary>Shows the error.</summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        public void ShowError(string message, params object[] args)
        {
            string name = Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().ProcessName);

            ErrorColor();
            System.Console.Error.WriteLine(message, args);
            ResetColor();
            System.Console.Error.WriteLine();
            System.Console.Out.WriteLine("Usage: {0} {1}", name, string.Join(" ", requiredUsageHelp.ToArray()));

            if (optionalUsageHelp.Count > 0)
            {
                System.Console.Out.WriteLine();
                System.Console.Out.WriteLine("Options:");

                foreach (string optional in optionalUsageHelp)
                {
                    System.Console.Out.WriteLine("    {0}", optional);
                }
            }
        }

        /// <summary>Gets the attribute.</summary>
        /// <typeparam name="T">The <see langword="Type" /> of attribute.</typeparam>
        /// <param name="provider">The provider.</param>
        /// <returns>The T.</returns>
        private static T GetAttribute<T>(ICustomAttributeProvider provider) where T : Attribute
        {
            var attributes = provider.GetCustomAttributes(typeof (T), false);
            if (attributes.Length > 0)
            {
                return (T)attributes[0];
            }
            return null;
        }

        /// <summary>
        /// Gets the assembly title.
        /// </summary>
        /// <value>The assembly title.</value>
        private static string GetAssemblyTitle()
        {
            var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
            if (attributes.Length > 0)
            {
                var titleAttribute = (AssemblyTitleAttribute)attributes[0];
                if (!string.IsNullOrEmpty(titleAttribute.Title))
                    return titleAttribute.Title;
            }
            return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
        }

        /// <summary>
        /// Gets the assembly title.
        /// </summary>
        /// <value>The assembly title.</value>
        private static string GetAssemblyCopyright()
        {
            var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
            if (attributes.Length > 0)
            {
                var titleAttribute = (AssemblyCopyrightAttribute)attributes[0];
                if (!string.IsNullOrEmpty(titleAttribute.Copyright))
                    return titleAttribute.Copyright;
            }
            return string.Empty;
        }

        // Used on optionsObject fields to indicate which options are required.

        [DllImport("kernel32.dll", EntryPoint = "GetStdHandle", SetLastError = true,
            CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
        private static extern int GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", EntryPoint = "GetConsoleScreenBufferInfo",
            SetLastError = true, CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
        private static extern int GetConsoleScreenBufferInfo(int hConsoleOutput,
                                                             ref CONSOLE_SCREEN_BUFFER_INFO lpConsoleScreenBufferInfo);

        [DllImport("kernel32.dll", EntryPoint = "SetConsoleTextAttribute",
            SetLastError = true, CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
        private static extern int SetConsoleTextAttribute(int hConsoleOutput,
                                                          int wAttributes);

        public static void ErrorColor()
        {
            Color(ConsoleColor.Red | ConsoleColor.Intensity);
        }

        public static void Color(ConsoleColor color)
        {
            SetConsoleTextAttribute(hConsoleHandle, (int) color);
        }

        public static void ResetColor()
        {
            SetConsoleTextAttribute(hConsoleHandle, OriginalColors);
        }

        #region Nested type: CONSOLE_SCREEN_BUFFER_INFO

        [StructLayout(LayoutKind.Sequential)]
        private struct CONSOLE_SCREEN_BUFFER_INFO
        {
            public COORD dwSize;
            public COORD dwCursorPosition;
            public int wAttributes;
            public SMALL_RECT srWindow;
            public COORD dwMaximumWindowSize;
        }

        #endregion

        #region Nested type: COORD

        [StructLayout(LayoutKind.Sequential)]
        private struct COORD
        {
            private short X;
            private short Y;
        }

        #endregion

        #region Nested type: OptionAttribute

        [AttributeUsage(AttributeTargets.Field)]
        public sealed class OptionAttribute : Attribute
        {
            public OptionAttribute(string name)
            {
                this.Name = name;
            }

            public string Name { get; private set; }

            public string Description { get; set; }

            public string Value { get; set; }

            public bool Required { get; set; }
        }

        #endregion

        #region Nested type: SMALL_RECT

        [StructLayout(LayoutKind.Sequential)]
        private struct SMALL_RECT
        {
            private short Left;
            private short Top;
            private short Right;
            private short Bottom;
        }

        #endregion
    }

    /// <summary>
    /// Colors used by <see cref="ConsoleProgram.Color"/>
    /// </summary>
    [Flags]
    enum ConsoleColor
    {
        /// <summary>
        /// Blue foreground color.
        /// </summary>
        Blue = 0x00000001,

        /// <summary>
        /// Green foreground color.
        /// </summary>
        Green = 0x00000002,

        /// <summary>
        /// Red foreground color.
        /// </summary>
        Red = 0x00000004,

        /// <summary>
        /// Intensity foreground color modifier.
        /// </summary>
        Intensity = 0x00000008,

        /// <summary>
        /// Blue background color.
        /// </summary>
        BlueBackground = 0x00000010,

        /// <summary>
        /// Green background color.
        /// </summary>
        GreenBackground = 0x00000020,

        /// <summary>
        /// Red background color.
        /// </summary>
        RedBackground = 0x00000040,

        /// <summary>
        /// Intensity background color modifier.
        /// </summary>
        IntensityBackground = 0x00000080,


        Black = 0,

        Cyan = Green | Blue,

        Magenta = Red | Blue,

        Yellow = Red | Green,

        DarkGrey = Red | Green | Blue,

        LightGrey = Intensity,

        LightRed = Intensity | Red,

        LightGreen = Intensity | Green,

        LightBlue = Intensity | Blue,

        LightCyan = Intensity | Green | Blue,

        LightMagenta = Intensity | Red | Blue,

        LightYellow = Intensity | Red | Green,

        White = Intensity | Red | Green | Blue,
    }
}