using System;

namespace Peppermint.Testing
{
    /// <summary>
    /// Exception that is thrown when a constructor with the required signature cannot be found.
    /// </summary>
    /// <revisionHistory>
    ///     <revisionItem revisionDate="29 Jan 2009" author="Aaron Janes">
    ///         Created.
    ///     </revisionItem>
    /// </revisionHistory>
    public class ConstructorNotFoundException : Exception
    {
        private readonly Type _classType;
        private readonly Type[] _methodTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorNotFoundException"/> class.
        /// </summary>
        /// <param name="classType">Name of the method which could not be found.</param>
        /// <param name="methodTypes">The method types on the method which could not be found.</param>
        public ConstructorNotFoundException(Type classType, Type[] methodTypes)
            : base(String.Format("Could not find a constructor with the required parameters for the specified type."))
        {
            _classType = classType;
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

        /// <summary>
        /// Gets the <see cref="Type"/> of the class that was trying to be constructed
        /// </summary>
        /// <value>The method type of the class.</value>
        public Type ClassType
        {
            get { return _classType; }
        }
    }
}
