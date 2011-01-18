using System;

namespace Peppermint.Testing
{
    /// <summary>
    /// Exception that is thrown when a method with the required signature cannot be found
    /// in the type.
    /// </summary>
    /// <revisionHistory>
    ///     <revisionItem revisionDate="29 Jan 2009" author="Aaron Janes">
    ///         Created.
    ///     </revisionItem>
    /// </revisionHistory>
    public class MethodNotFoundException : MemberNotFoundException
    {
        private readonly Type[] _methodTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodNotFoundException"/> class.
        /// </summary>
        /// <param name="methodName">Name of the method which could not be found.</param>
        /// <param name="methodTypes">The method types on the method which could not be found.</param>
        public MethodNotFoundException(string methodName, Type[] methodTypes)
            : base(methodName)
        {
            _methodTypes = methodTypes;
        }

        /// <summary>
        /// Gets the method types.
        /// </summary>
        /// <value>The method types on the method which could not be found.</value>
        public Type[] MethodTypes
        {
            get { return _methodTypes; }
        }
    }
}
