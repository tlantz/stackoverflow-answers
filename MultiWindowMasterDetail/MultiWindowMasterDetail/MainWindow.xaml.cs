using MultiWindowMasterDetail.ViewModels;
using System.Windows;

namespace MultiWindowMasterDetail
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += OnMainWindowLoaded;
        }

        private void OnMainWindowLoaded(object sender, RoutedEventArgs e) // have to wait until load to show a child window
        {
            var otherwin = new Window();
            otherwin.Owner = this;
            otherwin.Title = "Banking Details";
            otherwin.Content = new View.DetailView();
            otherwin.SizeToContent = SizeToContent.WidthAndHeight;
            otherwin.Show();
            var vm = new BankingViewModel();
            this.DataContext = vm;
            otherwin.DataContext = vm;
        }
    }
}
