using System;
using System.Reflection;

namespace Peppermint.Testing
{
    /// <summary>
    /// This class provides access to private members of the instance.
    /// </summary>
    /// <typeparam name="T">The type of the instance to be used</typeparam>
    public class Accessor<T> where T : class
    {
// ReSharper disable InconsistentNaming
        static readonly private Type _type = typeof(T);
// ReSharper restore InconsistentNaming
        readonly private T _instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="Accessor&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="instance">The instance whose private members are to be accessible.</param>
        public Accessor(T instance)
        {
            if (instance == null) throw new ArgumentNullException("instance");
            _instance = instance;
        }

        /// <summary>
        /// Gets the type of the instance.
        /// </summary>
        /// <value>The type.</value>
        public static Type Type
        {
            get { return _type; }
        }


        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public T Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Gets the static field.
        /// </summary>
        /// <typeparam name="TReturn">The type of the eturn T.</typeparam>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        public static TReturn GetStaticField<TReturn>(string field)
        {
            return (TReturn)Accessor.DoGetField(typeof(TReturn), Type, field, null);
        }

        /// <summary>
        /// Sets the field specified by the field name with the specified
        /// value.
        /// </summary>
        /// <typeparam name="TField">The type of field.</typeparam>
        /// <param name="field">The name of the field to set.</param>
        /// <param name="value">The value to set the field to.</param>
        public void SetField<TField>(string field, TField value)
        {
            Accessor.DoSetField(typeof(TField), Type, field, value, Instance);
        }

        /// <summary>
        /// Sets the static field.
        /// </summary>
        /// <typeparam name="TField">The type of the ield T.</typeparam>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        public static void SetStaticField<TField>(string field, TField value)
        {
            Accessor.DoSetField(typeof(TField), Type, field, value, null);
        }

        /// <summary>
        /// Constructs an instance of the specified type using the parameters
        /// specified.
        /// </summary>
        /// <param name="parameters">The parameters to use during construction.</param>
        /// <returns>An instance of the class.</returns>
        public static T Construct(params object[] parameters)
        {
            Type[] types = Accessor.CreateTypes(parameters);
            ConstructorInfo constructorInfo = Accessor.GetConstructorInfo(typeof(T), types);
            if (constructorInfo == null) throw new InvalidOperationException("Could not find a constructor with the correct parameters.");

            return (T)constructorInfo.Invoke(parameters);
        }



        /// <summary>
        /// Invokes the specified private method identified by the specified 
        /// method name and parameters.
        /// </summary>
        /// <typeparam name="TReturn">The type of the return value for this method.</typeparam>
        /// <param name="method">The name of the method to invoke.</param>
        /// <param name="parameters">The parameters to pass to the method on invokation.</param>
        /// <returns>Any value returned by the method.</returns>
        public TReturn Invoke<TReturn>(string method, params object[] parameters)
        {
            return (TReturn)Accessor.DoInvoke(method, typeof(TReturn), typeof(T), Instance, false, parameters);
        }

        /// <summary>
        /// Invokes the specified private method identified by the specified 
        /// method name and parameters.
        /// </summary>
        /// <param name="method">The name of the method to invoke.</param>
        /// <param name="parameters">The parameters to pass to the method on invokation.</param>
        public void Invoke(string method, params object[] parameters)
        {
            Accessor.DoInvoke(method, typeof(void), typeof(T), Instance, false, parameters);
        }

        /// <summary>
        /// Invokes the static method specifed by the "method" and "parameters" parameters.
        /// </summary>
        /// <typeparam name="TReturn">The type of return value.</typeparam>
        /// <param name="method">The name of the method to invoke.</param>
        /// <param name="parameters">The parameters to use when invoing the method.</param>
        /// <returns></returns>
        public static TReturn InvokeStatic<TReturn>(string method, params object[] parameters)
        {
            return (TReturn)Accessor.DoInvoke(method, typeof(TReturn), Type, null, true, parameters);
        }

        /// <summary>
        /// Invokes the static method specifed by the "method" and "parameters" parameters.
        /// </summary>
        /// <param name="method">The name of the method to invoke.</param>
        /// <param name="parameters">The parameters to use when invoing the method.</param>
        public static void InvokeStatic(string method, params object[] parameters)
        {
            Accessor.DoInvoke(method, typeof(void), Type, null, true, parameters);
        }



        /// <summary>
        /// Gets the value of the private property specified by the property name
        /// and parameters
        /// </summary>
        /// <typeparam name="TReturn">The type of the value to be returned.</typeparam>
        /// <param name="property">The name of the property to get the value of.</param>
        /// <param name="parameters">Any parameters passed to the property as indexers.</param>
        /// <returns>The property value.</returns>
        public TReturn GetProperty<TReturn>(string property, params object[] parameters)
        {
            return (TReturn)Accessor.DoGetProperty(typeof(TReturn), Type, property, Instance, parameters);
        }

        /// <summary>
        /// Gets the value field specified by the field name.
        /// </summary>
        /// <typeparam name="TReturn">The type of the value to be returned.</typeparam>
        /// <param name="field">The name of the field to get the value of.</param>
        /// <returns>The field value.</returns>
        public TReturn GetField<TReturn>(string field)
        {
            return (TReturn)Accessor.DoGetField(typeof(TReturn), Type, field, Instance);
        }


    }
}