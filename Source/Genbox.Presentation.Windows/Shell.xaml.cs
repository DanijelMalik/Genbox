using Generator4Developers.Presentation.Windows.ViewModels;

namespace Generator4Developers.Presentation.Windows
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
