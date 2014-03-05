namespace OrderUsings.Configuration
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;

    /// <summary>
    /// Loads and saves configuration settings.
    /// </summary>
    public class ConfigurationSerializer
    {
        private const string Ns = "http://schemas.interact-sw.co.uk/OrderUsings/2014";
        private static readonly XName GroupsName = XName.Get("Groups", Ns);
        private static readonly XName GroupName = XName.Get("Group", Ns);
        private static readonly XName SpaceName = XName.Get("Space", Ns);

        /// <summary>
        /// Loads configuration settings from XML.
        /// </summary>
        /// <param name="xmlConfiguration">A stream containing settings in XML.</param>
        /// <returns>A <see cref="OrderUsingsConfiguration"/> representing the settings
        /// in the file.</returns>
        public static OrderUsingsConfiguration FromXml(TextReader xmlConfiguration)
        {
            var xml = XDocument.Load(xmlConfiguration);
            var groupsElement = xml.Element(GroupsName);
            if (groupsElement == null)
            {
                throw new ArgumentException("Root element must be Groups");
            }

            return new OrderUsingsConfiguration
            {
                GroupsAndSpaces = xml
                    .Elements(XName.Get("Groups", Ns))
                    .Elements()
                    .Select(RuleFromElement)
                    .ToList()
            };
        }

        /// <summary>
        /// Produces a <see cref="ConfigurationRule"/> from an XML element, which must be
        /// either a <c>Group</c> or a <c>Space</c> element.
        /// </summary>
        /// <param name="elem">The XML element describing the rule.</param>
        /// <returns>A <see cref="ConfigurationRule"/>.</returns>
        private static ConfigurationRule RuleFromElement(XElement elem)
        {
            if (elem.Name == GroupName)
            {
                var aliasText = (string) elem.Attribute("Type");
                var type = MatchType.Import;
                switch (aliasText)
                {
                    case "Alias":
                        type = MatchType.Alias;
                        break;

                    case "ImportOrAlias":
                        type = MatchType.ImportOrAlias;
                        break;
                }

                return ConfigurationRule.ForGroupRule(new GroupRule
                {
                    Priority = ((int?) elem.Attribute("Priority")) ?? 1,
                    NamespacePattern = (string) elem.Attribute("NamespacePattern"),
                    AliasPattern = (string) elem.Attribute("AliasPattern"),
                    Type = type,
                    OrderAliasesBy = ((string) elem.Attribute("AliasOrderKey")) == "Namespace" ?
                        OrderAliasBy.Namespace : OrderAliasBy.Alias
                });
            }

            if (elem.Name == SpaceName)
            {
                return ConfigurationRule.ForSpace();
            }

            throw new ArgumentException("Elements in <Groups> must be either <Group> or <Space>");
        }
    }
}
