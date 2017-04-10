// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
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
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Linq;

namespace SharpDX.DirectInput
{
    public abstract class CustomDevice<T, TRaw, TUpdate> : Device
        where T : class, IDeviceState<TRaw, TUpdate>, new()
        where TRaw : struct
        where TUpdate : struct, IStateUpdate
    {
        private DataFormat _dataFormat;
        private readonly Dictionary<string, DataObjectFormat> _mapNameToObjectFormat = new Dictionary<string, DataObjectFormat>();

        protected CustomDevice(IntPtr nativePtr) : base(nativePtr)
        {
        }

        protected CustomDevice(DirectInput directInput, Guid deviceGuid) : base(directInput, deviceGuid)
        {
            var dataFormat = GetDataFormat();
            SetDataFormat(dataFormat);
        }

        private static readonly TUpdate[] singletonEmptyArray = new TUpdate[0];

        /// <summary>
        /// Retrieves buffered data from the device.
        /// </summary>
        /// <returns>A collection of buffered input events.</returns>
        public TUpdate[] GetBufferedData()
        {
            TUpdate[] updates = singletonEmptyArray;
            unsafe
            {
                var sizeOfObjectData = Utilities.SizeOf<ObjectData>();
                int size = -1;
                // 1 for peek
                GetDeviceData(sizeOfObjectData, IntPtr.Zero, ref size, 1);

                if (size == 0)
                    return updates;

                var pData = stackalloc ObjectData[size];
                GetDeviceData(sizeOfObjectData, (IntPtr)pData, ref size, 0);

                if (size == 0)
                    return updates;

                updates = new TUpdate[size];
                for (int i = 0; i < size; i++)
                {
                    var update = new TUpdate();
                    update.RawOffset = pData[i].Offset;
                    update.Value = pData[i].Data;
                    update.Timestamp = pData[i].TimeStamp;
                    update.Sequence = pData[i].Sequence;
                    updates[i] = update;
                }
            }

            return updates;
        }

        public T GetCurrentState()
        {
            var value = new T();
            GetCurrentState(ref value);
            return value;
        }

        public void GetCurrentState(ref T data)
        {
            unsafe
            {
                int size = Utilities.SizeOf<TRaw>();
                byte* pTemp = stackalloc byte[size*2];
                TRaw temp = default(TRaw);
                GetDeviceState(size, (IntPtr)pTemp);
                Utilities.Read((IntPtr)pTemp, ref temp);
                data.MarshalFrom(ref temp);
            }
        }

        public DeviceObjectInstance GetObjectInfoByName(string name)
        {
            return GetObjectInfo(GetFromName(name).Offset, PropertyHowType.Byoffset);
        }
        
        public DeviceObjectInstance GetObjectInfoByOffset(int offset)
        {
            return GetObjectInfo(offset, PropertyHowType.Byoffset);
        }

        public ObjectProperties GetObjectPropertiesByName(string name)
        {
            return new ObjectProperties(this, GetFromName(name).Offset, PropertyHowType.Byoffset);
        }

        private DataObjectFormat GetFromName(string name)
        {
            DataObjectFormat objectFormat;
            if (!_mapNameToObjectFormat.TryGetValue(name, out objectFormat))
                throw new ArgumentException(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Invalid name [{0}]. Must be in [{1}]", name, Utilities.Join(";", _mapNameToObjectFormat.Keys)));
            return objectFormat;
        }
        
        private DataFormat GetDataFormat()
        {
            if (_dataFormat == null)
            {
                // Build DataFormat from IDataFormatProvider
                if (typeof (IDataFormatProvider).GetTypeInfo().IsAssignableFrom(typeof (TRaw).GetTypeInfo()))
                {
                    var provider = (IDataFormatProvider) (new TRaw());
                    _dataFormat = new DataFormat(provider.Flags) {DataSize = Utilities.SizeOf<TRaw>(), ObjectsFormat = provider.ObjectsFormat};
                }
                else
                {
                    // Build DataFormat from DataFormat and DataObjectFormat attributes
                    IEnumerable<DataFormatAttribute> dataFormatAttributes = typeof(TRaw).GetTypeInfo().GetCustomAttributes<DataFormatAttribute>(false);
                    if (dataFormatAttributes.Count() != 1)
                        throw new InvalidOperationException(
                            string.Format(System.Globalization.CultureInfo.InvariantCulture, "The structure [{0}] must be marked with DataFormatAttribute or provide a IDataFormatProvider",
                                            typeof (TRaw).FullName));

                    _dataFormat = new DataFormat(((DataFormatAttribute) dataFormatAttributes.First()).Flags) {DataSize = Utilities.SizeOf<TRaw>()};

                    var dataObjects = new List<DataObjectFormat>();

                    IEnumerable<FieldInfo> fields;
#if NET45 || BEFORE_NET45
                    fields = typeof(TRaw).GetFields(BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance);
#elif NETCOREAPP1_0
                    fields = typeof(TRaw).GetTypeInfo().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
#else
                    fields = typeof(TRaw).GetTypeInfo().DeclaredFields;
#endif
                    // Iterates on fields
                    foreach (var field in fields)
                    {
                        IEnumerable<DataObjectFormatAttribute> dataObjectAttributes = field.GetCustomAttributes<DataObjectFormatAttribute>(false);
                        if (dataObjectAttributes.Count() > 0)
                        {
                            int fieldOffset = Marshal.OffsetOf(typeof (TRaw), field.Name).ToInt32();
                            int totalSizeOfField = Marshal.SizeOf(field.FieldType);
                            int offset = fieldOffset;
                            int numberOfDataObjects = 0;

                            // Count the number of effective sub-field for a field
                            // A field that contains a fixed array should have sub-field
                            for (int i = 0; i < dataObjectAttributes.Count(); i++)
                            {
                                var attr = dataObjectAttributes.ElementAt(i);
                                numberOfDataObjects += attr.ArrayCount == 0 ? 1 : attr.ArrayCount;
                            }

                            // Check that the size of the field is compatible with the number of sub-field
                            // For a simple field without any array element, sub-field = field
                            int sizeOfField = totalSizeOfField / numberOfDataObjects;
                            if ((sizeOfField * numberOfDataObjects) != totalSizeOfField)
                                throw new InvalidOperationException(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Field [{0}] has incompatible size [{1}] and number of DataObjectAttributes [{2}]", field.Name, (double)totalSizeOfField / numberOfDataObjects, numberOfDataObjects));

                            int subFieldIndex = 0;

                            // Iterates on attributes
                            for (int i = 0; i < dataObjectAttributes.Count(); i++)
                            {

                                var attr = dataObjectAttributes.ElementAt(i);
                                numberOfDataObjects = attr.ArrayCount == 0 ? 1 : attr.ArrayCount;

                                // Add DataObjectFormat
                                for (int j = 0; j < numberOfDataObjects; j++)
                                {
                                    var dataObject = new DataObjectFormat(
                                        string.IsNullOrEmpty(attr.Guid) ? Guid.Empty : new Guid(attr.Guid), offset,
                                        attr.TypeFlags, attr.Flags, attr.InstanceNumber);

                                    // Use attribute name or fallback to field's name
                                    string name = (string.IsNullOrEmpty(attr.Name)) ? field.Name : attr.Name;
                                    name = numberOfDataObjects == 1 ? name : name + subFieldIndex;

                                    dataObject.Name = name;
                                    dataObjects.Add(dataObject);

                                    offset += sizeOfField;
                                    subFieldIndex++;
                                }
                            }
                        }
                    }
                    _dataFormat.ObjectsFormat = dataObjects.ToArray();
                }

                for (int i = 0; i < _dataFormat.ObjectsFormat.Length; i++)
                {
                    var dataObject = _dataFormat.ObjectsFormat[i];

                    // Map field name to object
                    if (_mapNameToObjectFormat.ContainsKey(dataObject.Name))
                        throw new InvalidOperationException(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Incorrect field name [{0}]. Field name must be unique", dataObject.Name));
                    _mapNameToObjectFormat.Add(dataObject.Name, dataObject);                    
                }

                // DumpDataFormat(_dataFormat);
            }
            return _dataFormat;
        }

        /// <summary>
        /// Dumps the DataFormat in native form in order to verify it against the unmanaged version.
        /// </summary>
        /// <param name="format">The format.</param>
        private void DumpDataFormat(DataFormat format)
        {
            unsafe
            {
                var data = new DataFormat.__Native();
                format.__MarshalTo(ref data);

                string name = typeof (TRaw).Name;
                Console.WriteLine("{0}.dwSize     {1}", name, data.Size);
                Console.WriteLine("{0}.dwObjSize  {1}", name, data.ObjectSize);
                Console.WriteLine("{0}.dwFlags    {1} ({2})", name, (int) data.Flags, data.Flags);
                Console.WriteLine("{0}.dwDataSize {1}", name, data.DataSize);
                Console.WriteLine("{0}.dwNumObjs  {1}", name, data.ObjectArrayCount);

                Console.WriteLine("{4,32};{0,38};{1,8},{2,8};{3,8}", "Guid", "Offset", "Type", "Flags","Name");
                var currentObject = (DataObjectFormat.__Native*)data.ObjectArrayPointer;
                for (int i = 0; i < data.ObjectArrayCount; i++)
                {
                    var objFormat = currentObject[i];
                    string guid = objFormat.GuidPointer == IntPtr.Zero ? "" : (*((Guid*) objFormat.GuidPointer)).ToString();
                    Console.WriteLine("{5,32};{0,38};{1,8},{2:X8};{3:X8} ({4})", guid, objFormat.Offset, objFormat.Type, (int)objFormat.Flags, objFormat.Flags, format.ObjectsFormat[i].Name);
                    //Console.WriteLine("{0,32} = {1,8},", format.ObjectsFormat[i].Name, objFormat.Offset);
                }
                Console.WriteLine();
            }
        }
    }
}
