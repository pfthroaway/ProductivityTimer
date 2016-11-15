using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

namespace ProductivityTimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private TimeSpan _totalWorkTime = new TimeSpan();
        private TimeSpan _totalBreakTime = new TimeSpan();
        private TimeSpan _currentWorkTime = new TimeSpan();
        private TimeSpan _currentBreakTime = new TimeSpan();
        private TimeSpan oneSecond = DateTime.MinValue.AddSeconds(1) - DateTime.MinValue;
        private DispatcherTimer timerBreak = new DispatcherTimer();
        private DispatcherTimer timerWork = new DispatcherTimer();

        #region Data-Binding

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion Data-Binding

        #region Properties

        public TimeSpan CurrentWorkTime
        {
            get { return _currentWorkTime; }
            set { _currentWorkTime = value; OnPropertyChanged("CurrentWorkTimeToString"); }
        }

        public string CurrentWorkTimeToString
        {
            get { return CurrentWorkTime.ToString(@"hh\:mm\:ss"); }
        }

        public TimeSpan CurrentBreakTime
        {
            get { return _currentBreakTime; }
            set { _currentBreakTime = value; OnPropertyChanged("CurrentBreakTimeToString"); }
        }

        public string CurrentBreakTimeToString
        {
            get { return CurrentBreakTime.ToString(@"hh\:mm\:ss"); }
        }

        public TimeSpan TotalWorkTime
        {
            get { return _totalWorkTime; }
            set { _totalWorkTime = value; OnPropertyChanged("TotalWorkTimeToString"); }
        }

        public string TotalWorkTimeToString
        {
            get { return TotalWorkTime.ToString(@"hh\:mm\:ss"); }
        }

        public TimeSpan TotalBreakTime
        {
            get { return _totalBreakTime; }
            set { _totalBreakTime = value; OnPropertyChanged("TotalBreakTimeToString"); }
        }

        public string TotalBreakTimeToString
        {
            get { return TotalBreakTime.ToString(@"hh\:mm\:ss"); }
        }

        #endregion Properties

        #region Button-Click Methods

        private void btnStartWork_Click(object sender, RoutedEventArgs e)
        {
            CurrentWorkTime = new TimeSpan();
            timerBreak.Stop();
            timerWork.Start();
            btnStartBreak.IsEnabled = true;
            btnStartWork.IsEnabled = false;
            btnStopWork.IsEnabled = true;
        }

        private void btnStartBreak_Click(object sender, RoutedEventArgs e)
        {
            CurrentBreakTime = new TimeSpan();
            timerWork.Stop();
            timerBreak.Start();
            btnStopWork.IsEnabled = true;
            btnStartBreak.IsEnabled = false;
            btnStartWork.IsEnabled = true;
        }

        private void btnStopWork_Click(object sender, RoutedEventArgs e)
        {
            timerWork.Stop();
            timerBreak.Stop();
            btnStartBreak.IsEnabled = false;
            btnStartWork.IsEnabled = true;
            btnStopWork.IsEnabled = false;
        }

        #endregion Button-Click Methods

        #region Timer Ticks

        private void timerBreak_Tick(object sender, EventArgs e)
        {
            CurrentBreakTime += oneSecond;
            TotalBreakTime += oneSecond;
        }

        private void timerWork_Tick(object sender, EventArgs e)
        {
            CurrentWorkTime += oneSecond;
            TotalWorkTime += oneSecond;
        }

        #endregion Timer Ticks

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            timerBreak.Tick += timerBreak_Tick;
            timerBreak.Interval = new TimeSpan(0, 0, 1);
            timerWork.Tick += timerWork_Tick;
            timerWork.Interval = new TimeSpan(0, 0, 1);
        }
    }
}