using System.Windows.Input;

namespace UITest
{
    public partial class MainPage : ContentPage
    {
        private int count = 0;

        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = new MainPageViewModel();
        }
    }

    public class MainPageViewModel
    {
        public ICommand IconClickCommand { get; set; }

        public MainPageViewModel()
        {
            this.IconClickCommand = new Command(x => { });
        }
    }
}