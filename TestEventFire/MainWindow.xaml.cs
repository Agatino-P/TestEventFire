using System;
using System.Diagnostics;
using System.Windows;

namespace TestEventFire
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public class TestEventArgs : EventArgs
    {
        public string Testo { get; set; }
        public TestEventArgs(string testo)
        {
            Testo = testo;
        }
    }

    public class Subscriber1
    {
        public string Name { get; set; }
        private MainWindow _mainWindow;

        public Subscriber1(MainWindow mainWindow, string name)
        {
            _mainWindow = mainWindow;
            Name = name;
            _mainWindow.TestEvent += Subscriber1OnTestEvent;
        }

        private void Subscriber1OnTestEvent(object sender, TestEventArgs e)
        {
            _mainWindow.txtLog3.Text += $"{DateTime.Now} - {Name} Received: {e.Testo}{Environment.NewLine}";
        }
    }


    public class Subscriber2
    {
        public string Name { get; set; }
        private MainWindow _mainWindow;

        public Subscriber2(MainWindow mainWindow, string name)
        {
            _mainWindow = mainWindow;
            Name = name;
            _mainWindow.TestEvent += Subscriber2OnTestEvent;
        }

        private void Subscriber2OnTestEvent(object sender, TestEventArgs e)
        {
            _mainWindow.txtLog3.Text += $"{DateTime.Now} - {Name} Received: {e.Testo}{Environment.NewLine}";
        }

    }

    public partial class MainWindow : Window
    {

        private bool alreadySubscribed1 = false;
        private bool alreadySubscribed2 = false;

        public event EventHandler<TestEventArgs> TestEvent;// = delegate { };

        public MainWindow()
        {
            InitializeComponent();
            Subscriber1 s1 = new Subscriber1(this, "S-uno");
            Subscriber2 s2 = new Subscriber2(this, "S-due");
        }

        /// 
        /// 0
        /// 

        private void btnRaiseEvent_Click(object sender, RoutedEventArgs e)
        {
            Debug.Write($"Raising Event with arg={txtTesto.Text}{Environment.NewLine}");
            if (TestEvent != null)
            {
                TestEvent(this, new TestEventArgs(txtTesto.Text));
            }
        }

        /// 
        /// 1
        /// 



        private void btnSubscribeEvent_Click1(object sender, RoutedEventArgs e)
        {
            if (alreadySubscribed1)
            {
                return;
            }

            addTimedLogText1("Subscribing");
            alreadySubscribed1 = true;
            TestEvent += OnEventOne1;
        }

        private void OnEventOne1(object sender, TestEventArgs e)
        {
            if (sender == null)
            {
                return;
            }

            System.Diagnostics.Debug.Write($"OnEventOne - {DateTime.Now}{Environment.NewLine}");
            addTimedLogText1($"Received event with arg={e.Testo}");
        }


        private void btnUnSubscribeEvent_Click1(object sender, RoutedEventArgs e)
        {
            if (!alreadySubscribed1)
            {
                return;
            }

            addTimedLogText1("UnSubscribing");
            alreadySubscribed1 = false;
            TestEvent -= OnEventOne1;

        }


        private void addTimedLogText1(string testo)
        {
            txtLog1.Text += $"{Environment.NewLine}{DateTime.Now} - {testo}";
        }

        private void btnResetLog_Click1(object sender, RoutedEventArgs e)
        {
            txtLog1.Text = "";
            addTimedLogText1("ResetLog");
        }

        /// 
        /// 2
        /// 

        private void btnSubscribeEvent_Click2(object sender, RoutedEventArgs e)
        {
            if (alreadySubscribed2)
            {
                return;
            }

            addTimedLogText2("Subscribing");
            alreadySubscribed2 = true;
            TestEvent += OnEventOne2;
        }

        private void OnEventOne2(object sender, TestEventArgs e)
        {
            if (sender == null)
            {
                return;
            }

            System.Diagnostics.Debug.Write($"OnEventOne - {DateTime.Now}{Environment.NewLine}");
            addTimedLogText2($"Received event with arg={e.Testo}");
        }

        private void btnUnSubscribeEvent_Click2(object sender, RoutedEventArgs e)
        {
            if (!alreadySubscribed2)
            {
                return;
            }

            addTimedLogText2("UnSubscribing");
            alreadySubscribed2 = false;
            TestEvent -= OnEventOne2;
        }

        private void addTimedLogText2(string testo)
        {
            txtLog2.Text += $"{Environment.NewLine}{DateTime.Now} - {testo}";
        }

        private void btnResetLog_Click2(object sender, RoutedEventArgs e)
        {
            txtLog1.Text = "";
            addTimedLogText2("ResetLog");
        }

        /// 
        /// 3
        /// 

        private void btnResetLog_Click3(object sender, RoutedEventArgs e)
        {
            txtLog3.Text = "";
            addTimedLogText3("ResetLog");
        }

        private void addTimedLogText3(string testo)
        {
            txtLog3.Text += $"{Environment.NewLine}{DateTime.Now} - {testo}";
        }

    }
}
