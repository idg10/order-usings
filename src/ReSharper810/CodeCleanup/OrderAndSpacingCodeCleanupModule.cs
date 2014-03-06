namespace OrderUsings.ReSharper.CodeCleanup
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using JetBrains.Application;
    using JetBrains.Application.Progress;
    using JetBrains.Application.Settings;
    using JetBrains.DocumentModel;
    using JetBrains.ReSharper.Feature.Services.CodeCleanup;
    using JetBrains.ReSharper.Psi;
    using JetBrains.ReSharper.Psi.CSharp;
    using JetBrains.ReSharper.Psi.CSharp.Tree;
    using JetBrains.ReSharper.Psi.Files;
    using JetBrains.ReSharper.Psi.Tree;

    using OrderUsings.Configuration;
    using OrderUsings.ReSharper.Settings;

    /// <summary>
    /// Enables the order and spacing of using directives to be fixed en masse through
    /// ReSharper's code cleanup feature.
    /// </summary>
    [CodeCleanupModule]
    public class OrderAndSpacingCodeCleanupModule : ICodeCleanupModule
    {
        private static readonly OrderAndSpaceCleanupDescriptor DescriptorInstance = new OrderAndSpaceCleanupDescriptor();
        private readonly IShellLocks _shellLocks;

        /// <summary>
        /// Initializes a <see cref="OrderAndSpacingCodeCleanupModule"/>.
        /// </summary>
        /// <param name="shellLocks">Passed by ReSharper - enables us to lock files for
        /// exclusive write access while we perform cleanup.</param>
        public OrderAndSpacingCodeCleanupModule(IShellLocks shellLocks)
        {
            _shellLocks = shellLocks;
        }

        /// <inheritdoc/>
        public PsiLanguageType LanguageType
        {
            get { return CSharpLanguage.Instance; }
        }

        /// <inheritdoc/>
        public ICollection<CodeCleanupOptionDescriptor> Descriptors
        {
            get { return new CodeCleanupOptionDescriptor[] { DescriptorInstance }; }
        }

        /// <inheritdoc/>
        public bool IsAvailableOnSelection
        {
            get { return false; }
        }

        /// <inheritdoc/>
        public void SetDefaultSetting(CodeCleanupProfile profile, CodeCleanup.DefaultProfileType profileType)
        {
            switch (profileType)
            {
                case CodeCleanup.DefaultProfileType.FULL:
                    profile.SetSetting(DescriptorInstance, true);
                    break;
                default:
                    profile.SetSetting(DescriptorInstance, false);
                    break;
            }
        }

        /// <inheritdoc/>
        public bool IsAvailable(IPsiSourceFile sourceFile)
        {
            return sourceFile.GetPsiFiles<CSharpLanguage>().Any();
        }

        /// <inheritdoc/>
        public void Process(
            IPsiSourceFile sourceFile,
            IRangeMarker rangeMarker,
            CodeCleanupProfile profile,
            IProgressIndicator progressIndicator)
        {
            IPsiServices psiServices = sourceFile.GetPsiServices();
            IPsiFiles psiFiles = psiServices.Files;
            IContextBoundSettingsStore settings = sourceFile.GetSettingsStore();
            var orderUsingSettings =
                settings.GetKey<OrderUsingsSettings>(SettingsOptimization.DoMeSlowly);
            OrderUsingsConfiguration config = null;
            if (!string.IsNullOrWhiteSpace(orderUsingSettings.OrderSpecificationXml))
            {
                config = ConfigurationSerializer.FromXml(new StringReader(orderUsingSettings.OrderSpecificationXml));
            }

            if (config == null)
            {
                return;
            }

            var file = psiFiles.GetDominantPsiFile<CSharpLanguage>(sourceFile) as ICSharpFile;
            if (file == null)
            {
                return;
            }

            if (!profile.GetSetting(DescriptorInstance))
            {
                return;
            }

            file.GetPsiServices().Transactions.Execute(
                "Code cleanup",
                () =>
                {
                    using (_shellLocks.UsingWriteLock())
                    {
                        CleanUsings(file, config);
                        WalkNamespaceDeclarations(file.NamespaceDeclarations, config);
                    }
                });
        }

        /// <summary>
        /// Recursively walk namespace declaration blocks, cleaning any import lists they
        /// contain.
        /// </summary>
        /// <param name="namespaceDeclarationNodes">The namespace declaration blocks
        /// to check.</param>
        /// <param name="configuration">The configuration defining the correct order
        /// and spacing.</param>
        private void WalkNamespaceDeclarations(
            TreeNodeCollection<ICSharpNamespaceDeclaration> namespaceDeclarationNodes,
            OrderUsingsConfiguration configuration)
        {
            foreach (var ns in namespaceDeclarationNodes)
            {
                CleanUsings(ns, configuration);
                WalkNamespaceDeclarations(ns.NamespaceDeclarations, configuration);
            }
        }

        /// <summary>
        /// Fixes the order and spacing for the using blocks in a file or namespace declaration block.
        /// </summary>
        /// <param name="holder">The file or namespace declaration blocks to check.</param>
        /// <param name="configuration">The configuration defining the correct order
        /// and spacing.</param>
        private void CleanUsings(
            ICSharpTypeAndNamespaceHolderDeclaration holder, OrderUsingsConfiguration configuration)
        {
            Fixes.FixOrder(holder, configuration);
            Fixes.FixSpacing(holder, configuration);
        }
    }
}
