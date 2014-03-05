namespace OrderUsings.Configuration
{
    using System;

    /// <summary>
    /// Represents a single entry from the configuration - either a <see cref="GroupRule"/>
    /// or an entry indicating that a blank line is required.
    /// </summary>
    public struct ConfigurationRule
    {
        private readonly GroupRule _rule;

        /// <summary>
        /// Initializes a <see cref="ConfigurationRule"/> representing a <see cref="GroupRule"/>.
        /// </summary>
        /// <param name="rule">The rule.</param>
        private ConfigurationRule(GroupRule rule)
        {
            _rule = rule;
        }

        /// <summary>
        /// Gets a value indicating whether this rule represents a space or a group rule.
        /// </summary>
        public bool IsSpace
        {
            get { return _rule == null; }
        }

        /// <summary>
        /// Gets this entry's group rule. (Throws if you use this on an entry representing
        /// a space.)
        /// </summary>
        public GroupRule Rule
        {
            get
            {
                if (_rule == null)
                {
                    throw new InvalidOperationException("Cannot fetch Rule for entry representing space");
                }

                return _rule;
            }
        }

        /// <summary>
        /// Creates a <see cref="ConfigurationRule"/> representing a <see cref="GroupRule"/>.
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <returns>The configuration rule.</returns>
        public static ConfigurationRule ForGroupRule(GroupRule rule)
        {
            return new ConfigurationRule(rule);
        }

        /// <summary>
        /// Creates a <see cref="ConfigurationRule"/> representing a space between group rules.
        /// </summary>
        /// <returns>The configuration rule.</returns>
        public static ConfigurationRule ForSpace()
        {
            return new ConfigurationRule();
        }
    }
}
