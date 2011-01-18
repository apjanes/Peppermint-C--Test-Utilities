using System;
using System.Collections.Generic;
using System.Reflection;

namespace Peppermint.Testing
{

    /// <summary>
    /// This class provides access to private members of the instance.  Provided in non-generic form
    /// so that it works with static classes.
    /// </summary>
    public static class Accessor
    {

        /// <summary>
        /// Gets the static field.
        /// </summary>
        /// <typeparam name="TReturn">The type of the eturn T.</typeparam>
        /// <param name="classType">Type of the static class.</param>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        public static TReturn GetStaticField<TReturn>(Type classType, string field)
        {
            if (classType == null) throw new ArgumentNullException("classType");
            return (TReturn)DoGetField(typeof(TReturn), classType, field, null);
        }

        /// <summary>
        /// Sets the static field.
        /// </summary>
        /// <param name="classType">Type of the static class.</param>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        public static void SetStaticField(Type classType, string field, object value)
        {
            if (classType == null) throw new ArgumentNullException("classType");
            var type = value != null ? value.GetType() : null;
            DoSetField(type, classType, field, value, null);
        }

        /// <summary>
        /// Invokes the static method specifed by the "method" and "parameters" parameters.
        /// </summary>
        /// <typeparam name="TReturn">The type of return value.</typeparam>
        /// <param name="classType">Type of the class.</param>
        /// <param name="method">The name of the method to invoke.</param>
        /// <param name="parameters">The parameters to use when invoing the method.</param>
        /// <returns></returns>
        public static TReturn InvokeStatic<TReturn>(Type classType, string method, params object[] parameters)
        {
            return (TReturn)DoInvoke(method, typeof(TReturn), classType, null, true, parameters);
        }

        /// <summary>
        /// Invokes the static method specifed by the "method" and "parameters" parameters.
        /// </summary>
        /// <param name="classType">Type of the class.</param>
        /// <param name="method">The name of the method to invoke.</param>
        /// <param name="parameters">The parameters to use when invoing the method.</param>
        public static void InvokeStatic(Type classType, string method, params object[] parameters)
        {
            DoInvoke(method, typeof(void), classType, classType, true, parameters);
        }


        internal static object DoInvoke(string method, Type returnType, Type classType, object instance, bool isStatic, params object[] parameters)
        {
            if (classType == null) throw new ArgumentNullException("classType");
            if (method == null) throw new ArgumentNullException("method");
            if (parameters == null) parameters = new object[0];
            Type[] types = CreateTypes(parameters);

            MethodInfo methodInfo = GetMethodInfo(method, classType, returnType, types, isStatic);
            if (methodInfo == null) throw new InvalidOperationException("Could not find method with the correct name and parameters to invoke.");
            return methodInfo.Invoke(instance, parameters);
        }

        internal static object DoGetField(Type returnType, Type instanceType, string field, object instance)
        {
            if (field == null) throw new ArgumentNullException("field");
            bool isStatic = instance == null ? true : false;
            BindingFlags flags = (isStatic ? BindingFlags.Static : BindingFlags.Instance) | BindingFlags.NonPublic;
            FieldInfo fieldInfo = (instanceType).GetField(field, flags);

            if (fieldInfo != null)
            {
                Type fieldType = fieldInfo.FieldType;
                if (ReflectionUtilities.IsType(fieldType, returnType))
// ReSharper disable AssignNullToNotNullAttribute
                    return fieldInfo.GetValue(instance);
// ReSharper restore AssignNullToNotNullAttribute
            }

            throw new MemberNotFoundException(field);
        }

        internal static object DoGetProperty(Type returnType, Type instanceType, string property, object instance, params object[] parameters)
        {
            if (property == null) throw new ArgumentNullException("property");
            bool isStatic = instance == null ? true : false;
            BindingFlags flags = (isStatic ? BindingFlags.Static : BindingFlags.Instance) | BindingFlags.NonPublic;
            PropertyInfo propertyInfo = (instanceType).GetProperty(property, flags);

            if (propertyInfo != null)
            {
                Type propertyType = propertyInfo.PropertyType;
                if (ReflectionUtilities.IsType(propertyType, returnType))
                    return propertyInfo.GetValue(instance, parameters);
            }

            throw new MemberNotFoundException(property);
        }

        internal static void DoSetField(Type setType, Type instanceType, string field, object value, object instance)
        {
            if (field == null) throw new ArgumentNullException("field");
            bool isStatic = instance == null ? true : false;
            BindingFlags flags = (isStatic ? BindingFlags.Static : BindingFlags.Instance) | BindingFlags.NonPublic;
            FieldInfo fieldInfo = GetField(instanceType, field, flags);
            if (fieldInfo != null)
            {
                Type fieldType = fieldInfo.FieldType;
                if (fieldType == setType || ReflectionUtilities.IsType(setType, fieldType))
                {
                    fieldInfo.SetValue(instance, value);
                    return;
                }
            }

            throw new MemberNotFoundException(field);
        }

        /// <summary>
        /// Traverses up the heirarchy to find a field with the requested name.
        /// </summary>
        /// <param name="type">The type to get the fieldName from.</param>
        /// <param name="fieldName">The name of the field to get.</param>
        /// <param name="flags">The flags used to identify the field.</param>
        /// <returns>The field, if found, or <c>null</c></returns>
        internal static FieldInfo GetField(Type type, string fieldName, BindingFlags flags)
        {
            FieldInfo fieldInfo = type.GetField(fieldName, flags);
            if (fieldInfo == null && type.BaseType != null)
            {
                fieldInfo = GetField(type.BaseType, fieldName, flags);
            }

            return fieldInfo;
        }

        internal static MethodInfo GetMethodInfo(string methodName, Type type, Type returnType, Type[] parameterTypes, bool isStatic)
        {
            MethodInfo info = GetMethodInfo(methodName, type, parameterTypes, isStatic);

            if (info != null && !ReflectionUtilities.IsType(returnType, info.ReturnType))
                info = null;

            return info;
        }

        internal static MethodInfo GetMethodInfo(string name, Type type, Type[] types, bool isStatic)
        {
            var possible = new List<MethodInfo>();
            var flags = (isStatic ? BindingFlags.Static : BindingFlags.Instance) | BindingFlags.NonPublic;
            var allMethods = type.GetMethods(flags);

            foreach (MethodInfo methodInfo in allMethods)
            {
                if (methodInfo.Name != name) continue;

                ParameterInfo[] parameters = methodInfo.GetParameters();
                bool matched = MatchParameters(parameters, types);
                if (matched) possible.Add(methodInfo);
            }

            if (possible.Count == 0)
                throw new MethodNotFoundException(name, types);

            if (possible.Count > 1)
                throw new AmbiguousMatchException();

            return possible[0];
        }

        internal static ConstructorInfo GetConstructorInfo(Type type, Type[] types)
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var possible = new List<ConstructorInfo>();
            var allConstructors = type.GetConstructors(flags);

            foreach (var constructorInfo in allConstructors)
            {
                var parameters = constructorInfo.GetParameters();
                var matched = MatchParameters(parameters, types);
                if (matched) possible.Add(constructorInfo);
            }

            if (possible.Count == 0)
                throw new ConstructorNotFoundException(type, types);

            if (possible.Count > 1)
                throw new AmbiguousMatchException();

            return possible[0];
        }

        internal static bool MatchParameters(ParameterInfo[] parameters, Type[] types)
        {
            bool matched = false;
            if (parameters.Length == types.Length)
            {
                matched = true;
                for (int index = 0; index < types.Length; index++)
                {
                    ParameterInfo parameter = parameters[index];
                    Type currentType = types[index];

                    if (currentType == null || parameter.ParameterType == currentType ||
                        ReflectionUtilities.IsType(currentType, parameter.ParameterType))
                    {
                        // is valid for this parameter, continue
                    }
                    else
                    {
                        matched = false;
                        break;
                    }

                }
            }

            return matched;
        }

        internal static Type[] CreateTypes(object[] parameters)
        {
            var types = new Type[parameters.Length];
            for (int index = 0; index < parameters.Length; index++)
            {
                object parameter = parameters[index];
                if (parameter == null)
                {
                    types[index] = null;
                }
                else types[index] = parameter.GetType();
            }

            return types;
        }
    }
}
