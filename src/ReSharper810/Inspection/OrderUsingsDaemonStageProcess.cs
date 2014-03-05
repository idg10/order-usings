namespace OrderUsings.ReSharper.Inspection
{
    using System;
    using System.Collections.Generic;

    using JetBrains.ReSharper.Daemon;
    using JetBrains.ReSharper.Psi.CSharp.Tree;
    using JetBrains.ReSharper.Psi.Tree;

    using OrderUsings.Configuration;
    using OrderUsings.Processing;
    using OrderUsings.ReSharper.CodeModel;
    using OrderUsings.ReSharper.Highlightings;

    /// <summary>
    /// Represents the processing for a particular file in our daemon stage.
    /// </summary>
    internal class OrderUsingsDaemonStageProcess : IDaemonStageProcess
    {
        private readonly ICSharpFile _file;
        private readonly OrderUsingsConfiguration _config;

        /// <summary>
        /// Initializes a <see cref="OrderUsingsDaemonStageProcess"/>.
        /// </summary>
        /// <param name="process">The process object supplied by R# for this work.</param>
        /// <param name="file">The file to process.</param>
        /// <param name="config">The order and spacing configuration to use.</param>
        public OrderUsingsDaemonStageProcess(IDaemonProcess process, ICSharpFile file, OrderUsingsConfiguration config)
        {
            _file = file;
            _config = config;
            DaemonProcess = process;
        }

        /// <summary>
        /// Gets the process object associated with this work.
        /// </summary>
        /// <remarks>
        /// The <see cref="IDaemonStageProcess"/> interface requires this. Quite why R# doesn't 
        /// already know the association is beyond me.
        /// </remarks>
        public IDaemonProcess DaemonProcess { get; private set; }

        /// <summary>
        /// Called by ReSharper when it wants us to execute our work.
        /// </summary>
        /// <param name="committer">A call-back through which we supply the results
        /// of our processing.</param>
        public void Execute(Action<DaemonStageResult> committer)
        {
            DaemonStageResult result = null;
            if (_config != null)
            {
                List<HighlightingInfo> highlights = null;

                // Top-level imports (outside of any namespace blocks) are a singular special
                // case; the other place that imports can be found is in namespace blocks, and
                // since those can be nested, we have to walk them recursively.
                CheckImports(_file, ref highlights);
                WalkNamespaceDeclarations(_file.NamespaceDeclarations, ref highlights);

                if (highlights != null)
                {
                    result = new DaemonStageResult(highlights);
                }
            }

            committer(result);
        }

        /// <summary>
        /// Recursively walk namespace declaration blocks, checking any import lists they
        /// contain.
        /// </summary>
        /// <param name="namespaceDeclarationNodes">The namespace declaration blocks
        /// to check.</param>
        /// <param name="highlights">If any import lists are found that do not meet the
        /// configured order and spacing requirements, they will be returned via this
        /// argument. (To avoid unnecessary allocations in the happy path, we don't allocate
        /// the list of highlights unless we need to generate at least one highlight,
        /// which is why this is a <c>ref</c> parameter - it is initially <c>null</c>,
        /// but gets allocated on demand if needed.)</param>
        private void WalkNamespaceDeclarations(
            TreeNodeCollection<ICSharpNamespaceDeclaration> namespaceDeclarationNodes,
            ref List<HighlightingInfo> highlights)
        {
            foreach (var ns in namespaceDeclarationNodes)
            {
                CheckImports(ns, ref highlights);
                WalkNamespaceDeclarations(ns.NamespaceDeclarations, ref highlights);
            }
        }

        /// <summary>
        /// Checks an import list against the configured ordering and spacing.
        /// </summary>
        /// <param name="holder">The import list container - either a file, or a namespace
        /// declaration block.</param>
        /// <param name="highlights">If the import does not meet the configured requirements,
        /// we allocate a list containing a highlight describing the problem and return
        /// it via this argument. (We allocate the list on demand to avoid allocations
        /// in the happy path in which all the import lists are correctly ordered and
        /// spaced.)</param>
        private void CheckImports(
            ICSharpTypeAndNamespaceHolderDeclaration holder, ref List<HighlightingInfo> highlights)
        {
            List<UsingDirectiveOrSpace> items = ImportReader.ReadImports(holder);
            List<UsingDirective> imports;
            List<List<UsingDirective>> requiredOrderByGroups;
            ImportInspector.FlattenImportsAndDetermineOrderAndSpacing(
                _config, items, out imports, out requiredOrderByGroups);

            bool orderIsCorrect = true;
            if (requiredOrderByGroups != null)
            {
                Relocation nextChange = ImportInspector.GetNextUsingToMove(requiredOrderByGroups, imports);
                if (nextChange != null)
                {
                    orderIsCorrect = false;
                    AddHighlight(holder, ref highlights, new UsingOrderHighlighting(holder, _config));
                }
            }

            // If (and only if) the order is correct, we go on to check the spacing.
            if (orderIsCorrect)
            {
                SpaceChange nextChange = ImportInspector.GetNextSpacingModification(requiredOrderByGroups, items);
                if (nextChange != null)
                {
                    AddHighlight(holder, ref highlights, new UsingSpacingHighlighting(holder, _config));
                }
            }
        }

        private void AddHighlight(
            ICSharpTypeAndNamespaceHolderDeclaration holder,
            ref List<HighlightingInfo> highlights,
            IHighlighting highlight)
        {
            if (highlights == null)
            {
                highlights = new List<HighlightingInfo>();
            }

            highlights.Add(new HighlightingInfo(
                holder.ImportsList.GetHighlightingRange(), highlight));
        }
    }
}
