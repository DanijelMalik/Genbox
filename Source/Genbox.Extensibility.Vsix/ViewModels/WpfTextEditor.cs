using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace Genbox.Extensibility.Vsix.ViewModels
{
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    public sealed class WpfTextEditor : IWpfTextViewCreationListener
    {
        public IWpfTextView TextView { get; set; }

        public void TextViewCreated(IWpfTextView textView)
        {
            TextView = textView;
        }
    }
}