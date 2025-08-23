using DriverApp.ViewModels;

namespace DriverApp.Views
{
    public partial class EarningsView : ContentView
    {
        private readonly EarningsViewModel _viewModel;

        public EarningsView()
        {
            InitializeComponent();
            BindingContext = _viewModel = new EarningsViewModel();
        }

        private void OnDayClicked(object sender, EventArgs e)
        {
            _viewModel.LoadDayRanges();
        }

        private void OnWeekClicked(object sender, EventArgs e)
        {
            _viewModel.LoadWeekRanges();
        }

        private void OnMonthClicked(object sender, EventArgs e)
        {
            _viewModel.LoadMonthRanges();
        }
    }
}
