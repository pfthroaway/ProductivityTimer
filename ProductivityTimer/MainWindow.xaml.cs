using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

namespace ProductivityTimer
{
    /// <summary>Interaction logic for MainWindow.xaml</summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private TimeSpan _totalWorkTime, _totalBreakTime, _currentWorkTime, _currentBreakTime;
        private readonly TimeSpan _oneSecond = DateTime.MinValue.AddSeconds(1) - DateTime.MinValue;
        private readonly DispatcherTimer _timerBreak = new DispatcherTimer();
        private readonly DispatcherTimer _timerWork = new DispatcherTimer();

        #region Data-Binding

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion Data-Binding

        #region Properties

        /// <summary>The amount of time worked during this current work session.</summary>
        public TimeSpan CurrentWorkTime
        {
            get { return _currentWorkTime; }
            set { _currentWorkTime = value; OnPropertyChanged("CurrentWorkTimeToString"); }
        }

        /// <summary>The amount of time worked during this current work session, formatted.</summary>
        public string CurrentWorkTimeToString => CurrentWorkTime.ToString(@"hh\:mm\:ss");

        /// <summary>The amount of time breaked during this current break session.</summary>
        public TimeSpan CurrentBreakTime
        {
            get { return _currentBreakTime; }
            set { _currentBreakTime = value; OnPropertyChanged("CurrentBreakTimeToString"); }
        }

        /// <summary>The amount of time breaked during this current break session, formatted.</summary>
        public string CurrentBreakTimeToString => CurrentBreakTime.ToString(@"hh\:mm\:ss");

        /// <summary>The total amount of time worked during this session.</summary>
        public TimeSpan TotalWorkTime
        {
            get { return _totalWorkTime; }
            set { _totalWorkTime = value; OnPropertyChanged("TotalWorkTimeToString"); }
        }

        /// <summary>The total amount of time worked during this session, formatted.</summary>
        public string TotalWorkTimeToString => TotalWorkTime.ToString(@"hh\:mm\:ss");

        /// <summary>The total amount of breaked worked during this session.</summary>
        public TimeSpan TotalBreakTime
        {
            get { return _totalBreakTime; }
            set { _totalBreakTime = value; OnPropertyChanged("TotalBreakTimeToString"); }
        }

        /// <summary>The total amount of breaked worked during this session, formatted.</summary>
        public string TotalBreakTimeToString => TotalBreakTime.ToString(@"hh\:mm\:ss");

        #endregion Properties

        #region Button-Click Methods

        private void btnStartWork_Click(object sender, RoutedEventArgs e)
        {
            CurrentWorkTime = new TimeSpan();
            _timerBreak.Stop();
            _timerWork.Start();
            btnStartBreak.IsEnabled = true;
            btnStartWork.IsEnabled = false;
            btnStopWork.IsEnabled = true;
        }

        private void btnStartBreak_Click(object sender, RoutedEventArgs e)
        {
            CurrentBreakTime = new TimeSpan();
            _timerWork.Stop();
            _timerBreak.Start();
            btnStopWork.IsEnabled = true;
            btnStartBreak.IsEnabled = false;
            btnStartWork.IsEnabled = true;
        }

        private void btnStopWork_Click(object sender, RoutedEventArgs e)
        {
            _timerWork.Stop();
            _timerBreak.Stop();
            btnStartBreak.IsEnabled = false;
            btnStartWork.IsEnabled = true;
            btnStopWork.IsEnabled = false;
        }

        #endregion Button-Click Methods

        #region Timer Ticks

        private void timerBreak_Tick(object sender, EventArgs e)
        {
            CurrentBreakTime += _oneSecond;
            TotalBreakTime += _oneSecond;
        }

        private void timerWork_Tick(object sender, EventArgs e)
        {
            CurrentWorkTime += _oneSecond;
            TotalWorkTime += _oneSecond;
        }

        #endregion Timer Ticks

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            _timerBreak.Tick += timerBreak_Tick;
            _timerBreak.Interval = new TimeSpan(0, 0, 1);
            _timerWork.Tick += timerWork_Tick;
            _timerWork.Interval = new TimeSpan(0, 0, 1);
        }
    }
}