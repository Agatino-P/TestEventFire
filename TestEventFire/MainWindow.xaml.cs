using System;
using System.ComponentModel;
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

    public class Subscriber
    {
        public string Name { get; set; }
        private MainWindow _mainWindow;

        private static Random rand = new Random();

        public Subscriber(MainWindow mainWindow, string name)
        {
            _mainWindow = mainWindow;
            Name = name;
            _mainWindow.TestEvent += TestEventCaller;
        }

        private void TestEventCaller(object sender, TestEventArgs e)
        {
            //e.Testo = $"{Name}:Event Received";
            Debug.WriteLine( $"{Name} Started working on event at: {DateTime.Now:hh:mm:ss.fff tt}\n ");
            int ms = rand.Next(500, 10000);
            System.Threading.Thread.Sleep(ms);
            Debug.WriteLine($"{Name} Ended working on event at: {DateTime.Now:hh:mm:ss.fff tt} after {ms:N}ms\n ");
        }
    }

    public partial class MainWindow : Window
    {

        private bool alreadySubscribed1 = false;
        private bool alreadySubscribed2 = false;

        public event EventHandler<TestEventArgs> TestEvent = delegate {};
        public delegate string TestEventCaller(string testo);

        public MainWindow()
        {
            InitializeComponent();
            int numSubscribers = 5;
            for (int subscriberIndex=  0; subscriberIndex < numSubscribers; subscriberIndex++)
            {
                new Subscriber(this, subscriberIndex.ToString());
            }
            
        }

        /// 
        /// 0
        /// 

        private void btnRaiseEvent_Click(object sender, RoutedEventArgs e)
        {
            Debug.Write($"Raising Event with arg={txtTesto.Text}{Environment.NewLine}");

            //Works, but Syncronous, just like Invoke
            //TestEvent(this, new TestEventArgs(txtTesto.Text));

            //this Crashes
            //TestEvent.BeginInvoke(this, new TestEventArgs(txtTesto.Text),null,null);

            

            var receivers = TestEvent.GetInvocationList();
            foreach (EventHandler<TestEventArgs> receiver in receivers)
            {
                TestEventArgs tea = new TestEventArgs(txtLog3.Text);
                receiver.BeginInvoke(this, tea , null, null);
                txtLog3.Text += tea.Testo + "\n";
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
            //TestEvent += OnEventOne1;
        }

        private void OnEventOne1(object sender, TestEventArgs e)
        {
            if (sender == null)
            {
                return;
            }

            System.Diagnostics.Debug.Write($"OnEventOne - {DateTime.Now:hh:mm:ss.fff tt}{Environment.NewLine}");
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
            //TestEvent -= OnEventOne1;

        }


        private void addTimedLogText1(string testo)
        {
            txtLog1.Text += $"{Environment.NewLine}{DateTime.Now:hh:mm:ss.fff tt} - {testo}";
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
            //TestEvent += OnEventOne2;
        }

        private void OnEventOne2(object sender, TestEventArgs e)
        {
            if (sender == null)
            {
                return;
            }

            System.Diagnostics.Debug.Write($"OnEventOne - {DateTime.Now:hh:mm:ss.fff tt}{Environment.NewLine}");
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
            //TestEvent -= OnEventOne2;
        }

        private void addTimedLogText2(string testo)
        {
            txtLog2.Text += $"{Environment.NewLine}{DateTime.Now:hh:mm:ss.fff tt} - {testo}";
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
            addTimedLogText3($"ResetLog{Environment.NewLine}");
        }

        private void addTimedLogText3(string testo)
        {
            txtLog3.Text += $"{Environment.NewLine}{DateTime.Now:hh:mm:ss.fff tt} - {testo}";
        }

    }
}
