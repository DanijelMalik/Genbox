using System;
using Genbox.Presentation.Windows.Services;

namespace Genbox.Presentation.Windows.ViewModels
{
    public abstract class ServiceViewModel : ViewModel, IGeneratorService
    {
        private Guid _id;

        public Guid Id
        {
            get { return _id == Guid.Empty ? (_id = Guid.NewGuid()) : _id; }
        }

        public abstract string DisplayName { get; }
        public abstract string Generate();
        public abstract string Generate(int mode, string pattern);

        public virtual void Activate()
        {
        }

        protected virtual void OnModeChanged(string formattedText)
        {
            if (ModeChanged != null)
            {
                ModeChanged(this, new GeneratorEventArgs(formattedText));
            }
        }

        public event EventHandler<GeneratorEventArgs> ModeChanged;
    }
}