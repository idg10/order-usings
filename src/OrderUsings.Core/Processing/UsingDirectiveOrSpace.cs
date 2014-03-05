namespace OrderUsings.Processing
{
    using System;

    /// <summary>
    /// Represents an entry in a list of using directives - either a directive or
    /// a blank line.
    /// </summary>
    public struct UsingDirectiveOrSpace
    {
        private readonly UsingDirective _directive;

        /// <summary>
        /// Initialises a <see cref="UsingDirectiveOrSpace"/> representing
        /// a using directive. (Use the no-arguments constructor to represent
        /// a blank line.)
        /// </summary>
        /// <param name="directive">The directive.</param>
        public UsingDirectiveOrSpace(UsingDirective directive)
        {
            _directive = directive;
        }

        /// <summary>
        /// Gets a value indicating whether this entry represents a blank line.
        /// </summary>
        public bool IsBlankLine
        {
            get { return _directive == null; }
        }

        /// <summary>
        /// Gets the directive represented by this entry. (Throws if this entry
        /// represents a blank line.)
        /// </summary>
        public UsingDirective Directive
        {
            get
            {
                if (_directive == null)
                {
                    throw new InvalidOperationException("Cannot use Directive property on a blank line");
                }

                return _directive;
            }
        }
    }
}
