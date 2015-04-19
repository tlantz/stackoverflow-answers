
namespace VisifireDatabinding
{
    public sealed partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new ViewModels.ExampleViewModel();
        }
    }
}
