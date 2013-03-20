using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Alba.Framework.Serialization.Json
{
    /// <summary>
    /// Formats enum values as strings. Zero values are not formatted as "0" if specific 0 value is present. Combined bit flags are used only as a last resort.
    /// </summary>
    public class EnumFlagsConverter : JsonConverter
    {
        private readonly Dictionary<Type, EnumInfo> _enumInfos = new Dictionary<Type, EnumInfo>();

        public override void WriteJson (JsonWriter writer, object objVal, JsonSerializer serializer)
        {
            if (objVal == null) {
                writer.WriteNull();
                return;
            }

            Type objectType = objVal.GetType(),
                 enumType = GetActualEnumType(objectType);
            EnumInfo enumInfo = GetEnumInfo(enumType);
            ulong val = Convert.ToUInt64(objVal);

            if (!enumType.IsDefined(typeof(FlagsAttribute), false) || val == 0) {
                var valIndex = enumInfo.Values.IndexOf(val);
                writer.WriteValue(valIndex != -1 ? enumInfo.Names[valIndex] : objVal.ToString());
            }
            else {
                var sbVal = new StringBuilder();
                bool isFirstVal = true;

                for (int i = 0; i < enumInfo.Values.Count; i++) {
                    if (enumInfo.Values[i] != 0 && (val & enumInfo.Values[i]) == enumInfo.Values[i]) {
                        val -= enumInfo.Values[i];
                        if (!isFirstVal)
                            sbVal.Append(", ");
                        sbVal.Append(enumInfo.Names[i]);
                        isFirstVal = false;
                    }
                }

                writer.WriteValue(val == 0 ? sbVal.ToString() : objVal.ToString());
            }
        }

        public override object ReadJson (JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var enumType = GetActualEnumType(objectType);

            if (reader.TokenType == JsonToken.Null)
                return null;

            return Enum.Parse(enumType, Convert.ToString(reader.Value, CultureInfo.InvariantCulture));
        }

        public override bool CanConvert (Type objectType)
        {
            return GetActualEnumType(objectType).IsEnum;
        }

        private EnumInfo GetEnumInfo (Type enumType)
        {
            if (!_enumInfos.ContainsKey(enumType)) {
                var nameToVal = Enum.GetNames(enumType).Zip(
                    Enum.GetValues(enumType).OfType<Enum>().Select(Convert.ToUInt64),
                    (name, value) => new { name, value })
                    .OrderBy(p => GetBitCount(p.value)).ThenBy(p => p.value);
                _enumInfos[enumType] = new EnumInfo {
                    Names = nameToVal.Select(p => p.name).ToList(),
                    Values = nameToVal.Select(p => p.value).ToList(),
                };
            }
            return _enumInfos[enumType];
        }

        private static Type GetActualEnumType (Type objectType)
        {
            return objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(Nullable<>)
                ? Nullable.GetUnderlyingType(objectType) : objectType;
        }

        private static int GetBitCount (ulong i)
        {
            i = i - ((i >> 1) & 0x5555555555555555);
            i = (i & 0x3333333333333333) + ((i >> 2) & 0x3333333333333333);
            return (int)((((i + (i >> 4)) & 0xF0F0F0F0F0F0F0F) * 0x101010101010101) >> 56);
        }

        private class EnumInfo
        {
            public List<ulong> Values { get; set; }
            public List<string> Names { get; set; }
        }
    }
}