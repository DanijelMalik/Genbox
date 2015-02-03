using Genbox.Presentation.Windows.ViewModels;

namespace Genbox.Presentation.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Shell
    {
        public Shell()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}
