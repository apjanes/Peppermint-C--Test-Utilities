// ReSharper disable InconsistentNaming
using System;
using System.Reflection;
using NUnit.Framework;

namespace Peppermint.Testing.Tests.Unit
{
    [TestFixture]
    public class AccessorTFixture
    {
        /// <summary>
        /// Verifies that when constructing an <see cref="Accessor{T}"/> the
        /// instance parameter must not be null
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullObject_ExceptionThrown()
        {
            const string nullString = null;
            new Accessor<string>(nullString);
        }

        /// <summary>
        /// Verifies that instantiating a <see cref="Accessor{T}"/> with
        /// a reference type instance causes the <see cref="Accessor{T}.Instance"/>
        /// property to be set with that instance and the <see cref="Accessor{T}.Type"/>
        /// property to be set with the type of the instance.
        /// </summary>
        [Test]
        public void Constructor_TypeSet_ReturnsAsExpected()
        {
            var testClass = new TestClass();
            var accessor = new Accessor<TestClass>(testClass);
            Assert.AreSame(testClass, accessor.Instance);
            Assert.IsTrue(typeof(TestClass) == Accessor<TestClass>.Type);
        }

        /// <summary>
        /// Verifies that <see cref="Accessor{T}.Construct"/> creates an instance of
        /// a class with a private constructor using the specified parameters.
        /// </summary>
        [Test]
        public void Construct_ValidTypeWithParameters_ReturnsAsExpected()
        {
            const string parameters = "Snoopy";
            PrivateConstructorClass privateClass = Accessor<PrivateConstructorClass>.Construct(parameters);
            Assert.IsNotNull(privateClass);
            Assert.AreEqual(parameters, privateClass.Data);
        }

        /// <summary>
        /// Verifies that <see cref="Accessor{T}.Construct"/> creates an instance of
        /// a class with a private constructor with no parameters.
        /// </summary>
        [Test]
        public void Construct_ValidTypeNoParameters_ReturnsAsExpected()
        {
            PrivateConstructorClass privateClass = Accessor<PrivateConstructorClass>.Construct();
            Assert.IsNotNull(privateClass);
            Assert.AreEqual(PrivateConstructorClass.CONSTRUCTORPARAMETER_DEFAULT, privateClass.Data);
        }

        /// <summary>
        /// Verifies that the <see cref="Accessor{T}.GetField{ReturnT}"/> method
        /// throws an <see cref="ArgumentNullException"/> when called with a <c>null</c>
        /// field name.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void GetField_NullField_ExceptionThrown()
        {
            var testClass = new TestClass();
            var accessor = new Accessor<TestClass>(testClass);
            accessor.GetField<int>(null);
        }

        /// <summary>
        /// Verifies that the <see cref="Accessor{T}.GetField{ReturnT}"/> method
        /// throws a <see cref="MemberNotFoundException"/> when called with an invalid
        /// field name.
        /// </summary>
        [Test, ExpectedException(typeof(MemberNotFoundException))]
        public void GetField_InvalidField_ExceptionThrown()
        {
            var testClass = new TestClass();
            var accessor = new Accessor<TestClass>(testClass);
            accessor.GetField<int>("Invalid");
        }

        /// <summary>
        /// Verifies that the <see cref="Accessor{T}.GetField{ReturnT}"/> method
        /// returns the correct value when called with a valid field name.
        /// </summary>
        [Test]
        public void GetField_ValidField_ReturnsAsExpected()
        {
            var testClass = new TestClass();
            var accessor = new Accessor<TestClass>(testClass);
            var result = accessor.GetField<int>("_presetField");
            Assert.AreEqual(TestClass.PRESENTFIELD_VALUE, result);
        }

        /// <summary>
        /// Verifies that the <see cref="Accessor{T}.GetStaticField{ReturnT}"/> method
        /// returns the correct value when called with a valid field name.
        /// </summary>
        [Test]
        public void GetStaticField_ValidField_ReturnsAsExpected()
        {
            var result = Accessor<TestClass>.GetStaticField<double>("_staticField");
            Assert.AreEqual(TestClass.STATICFIELD_VALUE, result);
        }

        /// <summary>
        /// Verifies that the <see cref="Accessor{T}.SetField{ReturnT}"/> method
        /// throws an <see cref="ArgumentNullException"/> when called with a <c>null</c>
        /// field name.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void SetField_NullField_ExceptionThrown()
        {
            var testClass = new TestClass();
            var accessor = new Accessor<TestClass>(testClass);
            accessor.SetField(null, 0);
        }

        /// <summary>
        /// Verifies that the <see cref="Accessor{T}.SetField{ReturnT}"/> method
        /// throws a <see cref="MemberNotFoundException"/> when called with an invalid
        /// field name.
        /// </summary>
        [Test, ExpectedException(typeof(MemberNotFoundException))]
        public void SetField_InvalidField_ExceptionThrown()
        {
            var testClass = new TestClass();
            var accessor = new Accessor<TestClass>(testClass);
            accessor.SetField("Invalid", 0);
        }

        /// <summary>
        /// Verifies that the <see cref="Accessor{T}.SetField{ReturnT}"/> method
        /// correctly sets the private field with the specified value.
        /// </summary>
        [Test]
        public void SetField_ValidField_ReturnsAsExpected()
        {
            var testClass = new TestClass();
            DateTime now = DateTime.Now;
            var accessor = new Accessor<TestClass>(testClass);
            accessor.SetField("_readOnlyField", now);
            Assert.AreEqual(now, testClass.ReadOnlyField);
        }

        /// <summary>
        /// Verifies that the <see cref="Accessor{T}.SetStaticField{ReturnT}"/> method
        /// correctly sets the private field with the specified value.
        /// </summary>
        [Test]
        public void SetStaticField_ValidField_ReturnsAsExpected()
        {
            const double setTo = 100.0;
            Accessor<TestClass>.SetStaticField("_staticField", setTo);
            Assert.AreEqual(setTo, TestClass.StaticField);
        }

        [Test]
        public void SetStaticField_ValidFieldOnGeneric_ReturnsAsExpected()
        {
            const string setTo = "testing";
            Accessor<GenericTestClass<string>>.SetStaticField("_staticField", setTo);
            Assert.AreEqual(setTo, GenericTestClass<string>.FromStaticField);
        }

        [Test]
        public void SetStaticField_ValidFieldOnGenericSubClass_ReturnsAsExpected()
        {
            var setTo = new SubClassGeneric();
            Accessor<GenericTestClass<SubClassGeneric>>.SetStaticField("_staticField", setTo);
            Assert.AreEqual(setTo, GenericTestClass<SubClassGeneric>.FromStaticField);
        }

        [Test]
        public void SetStaticField_ValidFieldOnGenericSubClassToNull_ReturnsAsExpected()
        {
            var setTo = new SubClassGeneric();
            Accessor<GenericTestClass<SubClassGeneric>>.SetStaticField("_staticField", setTo);
            Assert.AreEqual(setTo, GenericTestClass<SubClassGeneric>.FromStaticField);

            setTo = null;
            Accessor<GenericTestClass<SubClassGeneric>>.SetStaticField("_staticField", setTo);
            Assert.IsNull(GenericTestClass<SubClassGeneric>.FromStaticField);
        }

        /// <summary>
        /// Verifies that when the <see cref="Accessor{T}.Invoke{ReturnT}"/>
        /// method is called with a <c>null</c> method parameter an 
        /// <see cref="ArgumentNullException"/> exception is thrown.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void Invoke_NullMethod_ExceptionThrown()
        {
            var testClass = new TestClass();
            var accessor = new Accessor<TestClass>(testClass);
            accessor.Invoke<string>(null);
        }

        /// <summary>
        /// Verifies that when <see cref="Accessor{T}.Invoke{ReturnT}"/>
        /// is called with a valid method (overloaded) and some parameters the correct
        /// private method is executed on the called class.
        /// </summary>
        [Test]
        public void Invoke_WithParameters_ReturnsAsExpected()
        {
            var testClass = new TestClass();
            var accessor = new Accessor<TestClass>(testClass);
            const string touchValue = "a touch value";
            var result = accessor.Invoke<string>("Touch", touchValue);

            Assert.AreEqual(touchValue, result);
            Assert.AreEqual(touchValue, testClass.TouchValue);
        }

        /// <summary>
        /// Verifies that when <see cref="Accessor{T}.Invoke{ReturnT}"/>
        /// is called with a valid method (overloaded) and no parameters the correct
        /// private method is executed on the called class.
        /// </summary>
        [Test]
        public void Invoke_Parameterless_ReturnsAsExpected()
        {
            var testClass = new TestClass();
            var accessor = new Accessor<TestClass>(testClass);
            var result = accessor.Invoke<string>("Touch");

            Assert.AreEqual(TestClass.TOUCHVALUE_DEFAULT, result);
            Assert.AreEqual(TestClass.TOUCHVALUE_DEFAULT, testClass.TouchValue);
        }

        /// <summary>
        /// Verifies that when the <see cref="Accessor{T}.Invoke"/> method is
        /// called with a valid method and no parameters, the called method
        /// is successfully called.
        /// </summary>
        [Test]
        public void Invoke_ParameterlessVoid_ReturnsAsExpected()
        {
            var testClass = new TestClass();
            var accessor = new Accessor<TestClass>(testClass);
            accessor.Invoke("ReturnsVoid");

            Assert.IsTrue(testClass.ReturnsVoidValue);
        }

        /// <summary>
        /// Verifies that when <see cref="Accessor{T}.Invoke{ReturnT}"/>
        /// is called with a valid method (overloaded) but <c>null</c> as the 
        /// parameter value, the private method is found and executed on the called 
        /// class.
        /// </summary>
        [Test]
        public void Invoke_NullParameters_ReturnsAsExpected()
        {
            var testClass = new TestClass();
            var accessor = new Accessor<TestClass>(testClass);
            var result = accessor.Invoke<string>("Touch", new object[] { null });

            Assert.AreEqual(null, result);
            Assert.AreEqual(null, testClass.TouchValue);
        }

        /// <summary>
        /// Verifies that when a method is invoked with an invalid return type
        /// requested, an exception is thrown.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void Invoke_ReturnTypeMismatch_ExceptionThrown()
        {
            var testClass = new TestClass();
            var accessor = new Accessor<TestClass>(testClass);
            accessor.Invoke<int>("Touch");
        }


        /// <summary>
        /// Verifies that when parameters and return values are a valid
        /// subclass of the parameter or return type (ie. implicit widening
        /// conversion) the <see cref="Accessor{T}.Invoke{ReturnT}"/> method
        /// returns as expected;
        /// </summary>
        public void Invoke_SubClass_ReturnsAsExpected()
        {
            var subClass = new TestSubClass();
            var accessor = new Accessor<TestClass>(subClass);
            var returned = accessor.Invoke<TestSubClass>("SubClass", subClass);
            Assert.AreSame(subClass, returned);
        }

        /// <summary>
        /// Verifies that when <c>null</c> for a parameter is used results
        /// in multiple possible methods an <see cref="InvalidOperationException"/>
        /// is thrown.
        /// </summary>
        [Test, ExpectedException(typeof(AmbiguousMatchException))]
        public void Invoke_NullParameterAmbiguousMethod_ExceptionThrown()
        {
            var testClass = new TestClass();
            var accessor = new Accessor<TestClass>(testClass);
            accessor.Invoke<string>("Ambiguous", new object[] { null });
        }

        /// <summary>
        /// Verifies that when <see cref="Accessor{T}.Invoke{ReturnT}"/> is called with
        /// an invalid method name a <see cref="MethodNotFoundException"/> is thrown
        /// </summary>
        [Test, ExpectedException(typeof(MethodNotFoundException))]
        public void Invoke_InvalidMethodName_ExceptionThrown()
        {
            var testClass = new TestClass();
            var accessor = new Accessor<TestClass>(testClass);
            accessor.Invoke<string>("Invalid");
        }

        /// <summary>
        /// Verifies that when <see cref="Accessor{T}.Invoke{ReturnT}"/> is called with
        /// a valid method name but invalid parameters a <see cref="MethodNotFoundException"/> 
        /// is thrown.
        /// </summary>
        [Test, ExpectedException(typeof(MethodNotFoundException))]
        public void Invoke_InvalidMethodParameters_ExceptionThrown()
        {
            var testClass = new TestClass();
            var accessor = new Accessor<TestClass>(testClass);
            accessor.Invoke<string>("Touch", new object[] { DateTime.Now });
        }

        [Test]
        public void InvokeStatic_VoidResultNonStaticType_OperatesAsExpected()
        {
            var expectedValue = Guid.NewGuid().ToString();
            Accessor<TestClass>.InvokeStatic("Fill", expectedValue);
            Assert.That(TestClass.FillTestProperty, Is.EqualTo(expectedValue));

        }

        [Test]
        public void InvokeStatic_StringResultNonStaticType_OperatesAsExpected()
        {
            const string expectedName = "Aaron";
            var name = Accessor<TestClass>.InvokeStatic<string>("GetName");
            Assert.That(name, Is.EqualTo(expectedName));

        }


#pragma warning disable 169
#pragma warning disable 649
        // ReSharper disable UnusedMember.Local
        // ReSharper disable UnusedParameter.Local
        public class TestClass
        {
            public const string TOUCHVALUE_DEFAULT = "Default class value";
            public const string AMBIGUOUS_STRING = "Parameter is string";
            public const string AMBIGUOUS_INT = "Parameter is int";
            public const int PRESENTFIELD_VALUE = 2;
            public const double STATICFIELD_VALUE = 20.0;

            public string TouchValue;
            public bool ReturnsVoidValue;

            private static double _staticField = STATICFIELD_VALUE;
            private int _presetField = PRESENTFIELD_VALUE;
            private DateTime _readOnlyField;

            private string Touch(string touchValue)
            {
                TouchValue = touchValue;
                return touchValue;
            }

            private string Touch()
            {
                TouchValue = TOUCHVALUE_DEFAULT;
                return TOUCHVALUE_DEFAULT;
            }

            private string Ambiguous(string parameter)
            {
                return AMBIGUOUS_STRING;
            }

            private string Ambiguous(int parameter)
            {
                return AMBIGUOUS_INT;
            }

            private void ReturnsVoid()
            {
                ReturnsVoidValue = true;
            }

            public DateTime ReadOnlyField
            {
                get { return _readOnlyField; }
            }

            public static double StaticField
            {
                get { return _staticField; }
            }

            public static string FillTestProperty { get; set; }

            private static void Fill(string testValue)
            {
                FillTestProperty = testValue;
            }

            private static string GetName()
            {
                return "Aaron";
            }
        }

        public class TestSubClass : TestClass
        {
            private TestClass SubClass(TestClass parameter)
            {
                return parameter;
            }
        }

        public class PrivateConstructorClass
        {
            public const string CONSTRUCTORPARAMETER_DEFAULT = "Parameterless";
            public string Data;

            private PrivateConstructorClass(string data)
            {
                Data = data;
            }

            private PrivateConstructorClass()
                : this(CONSTRUCTORPARAMETER_DEFAULT)
            {

            }
        }

        public class GenericTestClass<T>
        {
            private static T _staticField;

            public static T FromStaticField
            {
                get { return _staticField; }
            }
        }

        public class SubClassGeneric : GenericTestClass<SubClassGeneric>
        {

        }
        // ReSharper restore UnusedParameter.Local
        // ReSharper restore UnusedMember.Local
#pragma warning restore 649
#pragma warning restore 169
    }
}
// ReSharper restore InconsistentNaming
