namespace OrderUsings.Configuration
{
    /// <summary>
    /// Determines how using alias directives are ordered within a group.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If a single group ends up containing multiple using alias directives, we can
    /// sort them either by the alias itself or the namespace. E.g., if you choose
    /// <see cref="Alias"/>, you get:
    /// </para>
    /// <code>
    /// using B = Quux;
    /// using D = Foo;
    /// </code>
    /// <para>
    /// But with <see cref="Namespace"/>, the same directives would (if they end up matching
    /// the same group) be sorted so that the namespaces are in order, i.e.:
    /// </para>
    /// <code>
    /// using D = Foo;
    /// using B = Quux;
    /// </code>
    /// <para>
    /// If a single <see cref="Alias"/> includes both using alias directives and
    /// ordinary using directives, these will be intermingled. (You use separate rules
    /// if you want them separated.) So with <see cref="Namespace"/> you would get this
    /// sort of thing:
    /// </para>
    /// <code>
    /// using A;
    /// using A.Something;
    /// using B = Quux;
    /// using C.G;
    /// using D = Foo;
    /// using Faz;
    /// using Foz;
    /// using P;
    /// using Z;
    /// </code>
    /// <para>
    /// With <see cref="GroupRule"/>, the same directives would go in this order:
    /// </para>
    /// <code>
    /// using A;
    /// using A.Something;
    /// using C.G;
    /// using Faz;
    /// using D = Foo;
    /// using Foz;
    /// using P;
    /// using B = Quux;
    /// using Z;
    /// </code>
    /// </remarks>
    public enum OrderAliasBy
    {
        /// <summary>
        /// The order is based on the alias defined by the directive.
        /// </summary>
        Alias,

        /// <summary>
        /// The order is based on the directive's namespace.
        /// </summary>
        Namespace
    }
}
