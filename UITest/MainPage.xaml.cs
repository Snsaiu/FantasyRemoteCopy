using CommunityToolkit.Mvvm.ComponentModel;

using System.Windows.Input;

namespace UITest
{
    public partial class MainPage : ContentPage
    {
        private int count = 0;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel();
        }
    }

    public partial class MainPageViewModel : ObservableObject
    {
        public ICommand IconClickCommand { get; set; }

        [ObservableProperty] private double progress;

        public MainPageViewModel()
        {
            IconClickCommand = new Command(x => { });

            Progress = 0;
            StartProgressAnimation();
        }

        private void StartProgressAnimation()
        {
            var timer = Application.Current.Dispatcher.CreateTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) =>
            {
                Progress += 0.1;
                if (Progress > 1)
                    timer.Stop();
            };
            timer.Start();
        }
    }
}