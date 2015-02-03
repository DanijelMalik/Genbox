using System;

namespace Genbox.Presentation.Windows.Services
{
    public interface IGeneratorService
    {
        Guid Id { get; }
        string DisplayName { get; }
        string Generate();
        string Generate(int mode, string pattern);
        void Activate();
        event EventHandler<GeneratorEventArgs> ModeChanged;
    }

    public class GeneratorEventArgs : EventArgs
    {
        public GeneratorEventArgs(string formattedText)
        {
            FormattedText = formattedText;
        }

        public string FormattedText { get; set; }
    }
}