namespace Pomodorek.UWP {
    public sealed partial class MainPage {
        public MainPage() {
            InitializeComponent();

            LoadApplication(new Pomodorek.App());
        }
    }
}
