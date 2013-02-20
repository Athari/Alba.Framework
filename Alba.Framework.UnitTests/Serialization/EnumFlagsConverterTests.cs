using System;
using Alba.Framework.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Alba.Framework.UnitTests.Serialization
{
    [TestClass]
    public class EnumFlagsConverterTests
    {
        [TestInitialize]
        public void Init ()
        {}

        [TestMethod]
        public void ReadJson_SimpleEnum ()
        {
            AssertReadJson(SimpleEnum.A, "'A'");
            AssertReadJson(SimpleEnum.A, "'1'");
            AssertReadJson(SimpleEnum.A, "1");
            AssertReadJson(SimpleEnum.B, "'B'");
            AssertReadJson((SimpleEnum)0, "'0'");
            AssertReadJson((SimpleEnum)0, "0");
            AssertReadJson((SimpleEnum)10, "'10'");
            AssertReadJson((SimpleEnum)10, "10");
        }

        [TestMethod]
        public void ReadJson_SimpleEnumWithZero ()
        {
            AssertReadJson(SimpleEnumWithZero.A, "'A'");
            AssertReadJson(SimpleEnumWithZero.A, "'1'");
            AssertReadJson(SimpleEnumWithZero.A, "1");
            AssertReadJson(SimpleEnumWithZero.B, "'B'");
            AssertReadJson(SimpleEnumWithZero.Z, "'0'");
            AssertReadJson(SimpleEnumWithZero.Z, "0");
            AssertReadJson(SimpleEnumWithZero.Z, "'Z'");
            AssertReadJson((SimpleEnumWithZero)10, "'10'");
            AssertReadJson((SimpleEnumWithZero)10, "10");
        }

        [TestMethod]
        public void ReadJson_SimpleEnumWithFlagsAttr ()
        {
            AssertReadJson(SimpleEnumWithFlagsAttr.A, "'A'");
            AssertReadJson(SimpleEnumWithFlagsAttr.A, "'1'");
            AssertReadJson(SimpleEnumWithFlagsAttr.A, "1");
            AssertReadJson(SimpleEnumWithFlagsAttr.B, "'B'");
            AssertReadJson((SimpleEnumWithFlagsAttr)0, "'0'");
            AssertReadJson((SimpleEnumWithFlagsAttr)0, "0");
            AssertReadJson((SimpleEnumWithFlagsAttr)10, "'10'");
            AssertReadJson((SimpleEnumWithFlagsAttr)10, "10");
        }

        [TestMethod]
        public void ReadJson_SimpleEnumWithZeroWithFlagsAttr ()
        {
            AssertReadJson(SimpleEnumWithZeroWithFlagsAttr.A, "'A'");
            AssertReadJson(SimpleEnumWithZeroWithFlagsAttr.A, "'1'");
            AssertReadJson(SimpleEnumWithZeroWithFlagsAttr.A, "1");
            AssertReadJson(SimpleEnumWithZeroWithFlagsAttr.B, "'B'");
            AssertReadJson(SimpleEnumWithZeroWithFlagsAttr.Z, "'0'");
            AssertReadJson(SimpleEnumWithZeroWithFlagsAttr.Z, "0");
            AssertReadJson(SimpleEnumWithZeroWithFlagsAttr.Z, "'Z'");
            AssertReadJson((SimpleEnumWithZeroWithFlagsAttr)10, "'10'");
            AssertReadJson((SimpleEnumWithZeroWithFlagsAttr)10, "10");
        }

        [TestMethod]
        public void ReadJson_FlagsEnum ()
        {
            AssertReadJson(FlagsEnum.A, "'A'");
            AssertReadJson(FlagsEnum.A, "'1'");
            AssertReadJson(FlagsEnum.A, "1");
            AssertReadJson(FlagsEnum.B, "'B'");
            AssertReadJson(FlagsEnum.C, "'C'");
            AssertReadJson(FlagsEnum.A | FlagsEnum.B, "'A,B'");
            AssertReadJson(FlagsEnum.A | FlagsEnum.B, "'A, B'");
            AssertReadJson(FlagsEnum.A | FlagsEnum.B, "'3'");
            AssertReadJson(FlagsEnum.A | FlagsEnum.B, "3");
            AssertReadJson(FlagsEnum.B | FlagsEnum.C, "'C,B'");
            AssertReadJson(FlagsEnum.B | FlagsEnum.C, "'6'");
            AssertReadJson(FlagsEnum.B | FlagsEnum.C, "6");
            AssertReadJson(FlagsEnum.A | FlagsEnum.B | FlagsEnum.C, "'C,A,B'");
            AssertReadJson(FlagsEnum.A | FlagsEnum.B | FlagsEnum.C, "' C, A,B '");
            AssertReadJson(FlagsEnum.A | FlagsEnum.B | FlagsEnum.C, "'7'");
            AssertReadJson(FlagsEnum.A | FlagsEnum.B | FlagsEnum.C, "7");
            AssertReadJson((FlagsEnum)0, "'0'");
            AssertReadJson((FlagsEnum)0, "0");
            AssertReadJson((FlagsEnum)10, "'10'");
            AssertReadJson((FlagsEnum)10, "10");
        }

        [TestMethod]
        public void ReadJson_FlagsEnumWithZero ()
        {
            AssertReadJson(FlagsEnumWithZero.A, "'A'");
            AssertReadJson(FlagsEnumWithZero.A, "'1'");
            AssertReadJson(FlagsEnumWithZero.A, "1");
            AssertReadJson(FlagsEnumWithZero.B, "'B'");
            AssertReadJson(FlagsEnumWithZero.C, "'C'");
            AssertReadJson(FlagsEnumWithZero.A | FlagsEnumWithZero.B, "'A,B'");
            AssertReadJson(FlagsEnumWithZero.A | FlagsEnumWithZero.B, "'A, B'");
            AssertReadJson(FlagsEnumWithZero.A | FlagsEnumWithZero.B, "'3'");
            AssertReadJson(FlagsEnumWithZero.A | FlagsEnumWithZero.B, "3");
            AssertReadJson(FlagsEnumWithZero.B | FlagsEnumWithZero.C, "'C,B'");
            AssertReadJson(FlagsEnumWithZero.B | FlagsEnumWithZero.C, "'6'");
            AssertReadJson(FlagsEnumWithZero.B | FlagsEnumWithZero.C, "6");
            AssertReadJson(FlagsEnumWithZero.A | FlagsEnumWithZero.B | FlagsEnumWithZero.C, "'C,A,B'");
            AssertReadJson(FlagsEnumWithZero.A | FlagsEnumWithZero.B | FlagsEnumWithZero.C, "' C, A,B '");
            AssertReadJson(FlagsEnumWithZero.A | FlagsEnumWithZero.B | FlagsEnumWithZero.C, "'7'");
            AssertReadJson(FlagsEnumWithZero.A | FlagsEnumWithZero.B | FlagsEnumWithZero.C, "7");
            AssertReadJson((FlagsEnumWithZero)0, "'0'");
            AssertReadJson((FlagsEnumWithZero)0, "0");
            AssertReadJson((FlagsEnumWithZero)0, "'Z'");
            AssertReadJson((FlagsEnumWithZero)10, "'10'");
            AssertReadJson((FlagsEnumWithZero)10, "10");
        }

        [TestMethod]
        public void WriteJson_SimpleEnum ()
        {
            AssertWriteJson("\"A\"", SimpleEnum.A);
            AssertWriteJson("\"B\"", SimpleEnum.B);
            AssertWriteJson("\"0\"", (SimpleEnum)0);
            AssertWriteJson("\"10\"", (SimpleEnum)10);
        }

        [TestMethod]
        public void WriteJson_SimpleEnumWithZero ()
        {
            AssertWriteJson("\"A\"", SimpleEnumWithZero.A);
            AssertWriteJson("\"B\"", SimpleEnumWithZero.B);
            AssertWriteJson("\"Z\"", SimpleEnumWithZero.Z);
            AssertWriteJson("\"10\"", (SimpleEnumWithZero)10);
        }

        [TestMethod]
        public void WriteJson_SimpleEnumWithFlagsAttr ()
        {
            AssertWriteJson("\"A\"", SimpleEnumWithFlagsAttr.A);
            AssertWriteJson("\"B\"", SimpleEnumWithFlagsAttr.B);
            AssertWriteJson("\"0\"", (SimpleEnumWithFlagsAttr)0);
            AssertWriteJson("\"10\"", (SimpleEnumWithFlagsAttr)10);
        }

        [TestMethod]
        public void WriteJson_SimpleEnumWithZeroWithFlagsAttr ()
        {
            AssertWriteJson("\"A\"", SimpleEnumWithZeroWithFlagsAttr.A);
            AssertWriteJson("\"B\"", SimpleEnumWithZeroWithFlagsAttr.B);
            AssertWriteJson("\"Z\"", SimpleEnumWithZeroWithFlagsAttr.Z);
            AssertWriteJson("\"10\"", (SimpleEnumWithZeroWithFlagsAttr)10);
        }

        [TestMethod]
        public void WriteJson_FlagsEnum ()
        {
            AssertWriteJson("\"A\"", FlagsEnum.A);
            AssertWriteJson("\"B\"", FlagsEnum.B);
            AssertWriteJson("\"A, B\"", FlagsEnum.A | FlagsEnum.B);
            AssertWriteJson("\"B, C\"", FlagsEnum.B | FlagsEnum.C);
            AssertWriteJson("\"A, B, C\"", FlagsEnum.A | FlagsEnum.B | FlagsEnum.C);
            AssertWriteJson("\"0\"", (FlagsEnum)0);
            AssertWriteJson("\"10\"", (FlagsEnum)10);
        }

        [TestMethod]
        public void WriteJson_FlagsEnumWithZero ()
        {
            AssertWriteJson("\"A\"", FlagsEnumWithZero.A);
            AssertWriteJson("\"B\"", FlagsEnumWithZero.B);
            AssertWriteJson("\"A, B\"", FlagsEnumWithZero.A | FlagsEnumWithZero.B);
            AssertWriteJson("\"B, C\"", FlagsEnumWithZero.B | FlagsEnumWithZero.C);
            AssertWriteJson("\"A, B, C\"", FlagsEnumWithZero.A | FlagsEnumWithZero.B | FlagsEnumWithZero.C);
            AssertWriteJson("\"Z\"", (FlagsEnumWithZero)0);
            AssertWriteJson("\"10\"", (FlagsEnumWithZero)10);
        }

        private void AssertReadJson<T> (T expectedValue, string json)
        {
            var settings = new JsonSerializerSettings {
                Converters = { new EnumFlagsConverter() }
            };
            Assert.AreEqual(expectedValue, JsonConvert.DeserializeObject<T>(json, settings));
        }

        private void AssertWriteJson<T> (string expectedJson, T value)
        {
            var settings = new JsonSerializerSettings {
                Converters = { new EnumFlagsConverter() },
            };
            Assert.AreEqual(expectedJson, JsonConvert.SerializeObject(value, settings));
        }

        private enum SimpleEnum
        {
            A = 1,
            B,
        }

        private enum SimpleEnumWithZero
        {
            Z = 0,
            A,
            B,
        }

        [Flags]
        private enum SimpleEnumWithFlagsAttr
        {
            A = 1,
            B,
        }

        [Flags]
        private enum SimpleEnumWithZeroWithFlagsAttr
        {
            Z = 0,
            A,
            B,
        }

        [Flags]
        private enum FlagsEnum
        {
            A = 1 << 0,
            B = 1 << 1,
            C = 1 << 2,
        }

        [Flags]
        private enum FlagsEnumWithZero
        {
            Z = 0,
            A = 1 << 0,
            B = 1 << 1,
            C = 1 << 2,
        }
    }
}