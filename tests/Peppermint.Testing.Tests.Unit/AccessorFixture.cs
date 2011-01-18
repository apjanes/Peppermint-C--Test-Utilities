// ReSharper disable InconsistentNaming
using System;
using NUnit.Framework;

namespace Peppermint.Testing.Tests.Unit
{
    [TestFixture]
    public class AccessorFixture
    {
        [Test]
        public void SetStaticField_ValidFieldOnStaticClass_ReturnsAsExpected()
        {
            var expectedValue = Guid.NewGuid().ToString();
            Accessor.SetStaticField(typeof(StaticClass), "_setField", expectedValue);
            var value = StaticClass.SetField;
            Assert.That(value, Is.EqualTo(expectedValue));
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void SetStaticField_NullType_ExceptionThrown()
        {
            Accessor.SetStaticField(null, "_setField", "");
        }

        [Test]
        public void GetStaticField_ValidField_ReturnsAsExpected()
        {
            var value = Accessor.GetStaticField<string>(typeof(StaticClass), "_getField");
            Assert.That(value, Is.EqualTo(StaticClass.GetFieldValue));
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void GetStaticField_NullType_ExceptionThrown()
        {
            Accessor.GetStaticField<string>(null, "_getField");
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void InvokeStatic_WithReturnNullType_ExceptionThrown()
        {
            Accessor.InvokeStatic<string>(null, "InvokeWithReturn");
        }
        [Test, ExpectedException(typeof(ArgumentNullException))]

        public void InvokeStatic_WithReturnNullName_ExceptionThrown()

        {
            Accessor.InvokeStatic<string>(typeof(string), null);
        }

        [Test]
        public void InvokeStatic_MethodWithReturnNoParameters_ReturnsAsExpected()
        {
            var value = Accessor.InvokeStatic<string>(typeof(StaticClass), "InvokeWithReturn");
            Assert.That(value, Is.EqualTo(StaticClass.InvokeWithReturnValue));
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void InvokeStatic_VoidNullType_ExceptionThrown()
        {
            Accessor.InvokeStatic(null, "InvokeWithReturn");
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void InvokeStatic_VoidNullName_ExceptionThrown()
        {
            Accessor.InvokeStatic(typeof(string), null);
        }

        [Test]
        public void InvokeStatic_MethodVoidNoParameters_ReturnsAsExpected()
        {
            var value = Accessor.InvokeStatic<string>(typeof(StaticClass), "InvokeWithReturn");
            Assert.That(value, Is.EqualTo(StaticClass.InvokeWithReturnValue));
        }

        [Test]
        public void InvokeStatic_MethodWithReturnAndParameters_ReturnsAsExpected()
        {
            StaticClass.InvokeVoidCalled = false;

            Accessor.InvokeStatic(typeof(StaticClass), "InvokeVoid");
            Assert.That(StaticClass.InvokeVoidCalled);
        }

        public static class StaticClass
        {
#pragma warning disable 169
#pragma warning disable 649
            // ReSharper disable UnusedMember.Local
            public const string GetFieldValue = "Test get field";
            public const string GetPropertyValue = "Test get property";
            public const string InvokeWithReturnValue = "return value";

            private static string _setField;

            private static string _getField = GetFieldValue;



            public static string SetField
            {
                get { return _setField; }
            }


            private static string InvokeWithReturn()
            {
                return InvokeWithReturnValue;
            }

            private static string InvokeWithReturn(string value)
            {
                return value;
            }


            private static void InvokeVoid()

            {
                InvokeVoidCalled = true;
            }

            public static bool InvokeVoidCalled { get; set; }
        }
// ReSharper restore UnusedMember.Local
#pragma warning restore 649
#pragma warning restore 169
    }
}
// ReSharper restore InconsistentNaming