using System;

namespace Peppermint.Testing
{
    /// <summary>
    /// Thrown if a particular member cannot be found on a particular type.
    /// </summary>
    /// <revisionHistory>
    ///     <revisionItem revisionDate="29 Jan 2009" author="Aaron Janes">
    ///         Created.
    ///     </revisionItem>
    /// </revisionHistory>
    public class MemberNotFoundException : Exception
    {
        private readonly string _name;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberNotFoundException"/> class.
        /// </summary>
        /// <param name="name">Name of the member which could not be found.</param>
        public MemberNotFoundException(string name)
        {
            _name = name;
        }

        /// <summary>
        /// Gets the name of the member which could not be found..
        /// </summary>
        /// <value>The name of the member which could not be found.</value>
        public string Name
        {
            get { return _name; }
        }
    }
}
