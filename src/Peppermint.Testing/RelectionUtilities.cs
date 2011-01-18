using System;

namespace Peppermint.Testing
{
    /// <summary>
    /// Various utilities that can be used in when using .NET reflection.
    /// </summary>
    /// <revisionHistory>
    ///     <revisionItem revisionDate="29 Jan 2009" author="Aaron Janes">
    ///         Created.
    ///     </revisionItem>
    /// </revisionHistory>
    public class ReflectionUtilities
    {
        /// <summary>
        /// Determines whether the specified type to check is the same as
        /// or a subclass of or impelements the base type.
        /// </summary>
        /// <param name="toCheck">The type to check.</param>
        /// <param name="baseType">The base type to compare.</param>
        /// <returns>
        /// 	<c>true</c> if the type to check is the same as or a sub class 
        ///     of the base type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsType(Type toCheck, Type baseType)
        {
            return (toCheck == baseType || toCheck.IsSubclassOf(baseType) || Implements(toCheck, baseType));
        }

        /// <summary>
        /// Determines whether the specified type implements the other type.
        /// </summary>
        /// <param name="toCheck">The type which is being checked.</param>
        /// <param name="implemented">The type to check if it is implemented.</param>
        /// <returns></returns>
        public static bool Implements(Type toCheck, Type implemented)
        {
            if (toCheck == null) throw new ArgumentNullException("toCheck");
            if (implemented == null) throw new ArgumentNullException("implemented");
            Type[] interfaces = toCheck.GetInterfaces();
            foreach (Type type in interfaces)
            {
                if (type == implemented) return true;
            }

            return false;
        }
    }
}
