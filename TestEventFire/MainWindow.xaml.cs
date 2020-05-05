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

    public class Subscriber1
    {
        public string Name { get; set; }
        private MainWindow _mainWindow;

        public Subscriber1(MainWindow mainWindow, string name)
        {
            _mainWindow = mainWindow;
            Name = name;
            _mainWindow.TestEvent += TestEventCaller;
        }

        public void TestEventCaller(object sender, TestEventArgs e)
        {
            Debug.WriteLine( $"{DateTime.Now:hh:mm:ss.fff tt} - {Name} Start: {e.Testo}{Environment.NewLine}");
            Random rand = new Random();
            int ms = rand.Next(5000, 10000);
            System.Threading.Thread.Sleep(ms);
            Debug.WriteLine($"{DateTime.Now:hh:mm:ss.fff tt} - {Name} End: {e.Testo}{Environment.NewLine}");
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
            _mainWindow.TestEvent += TestEventCaller;
        }

        public void TestEventCaller(object sender, TestEventArgs e)
        {

            Debug.WriteLine($"{DateTime.Now:hh:mm:ss.fff tt} - {Name} Start: {e.Testo}{Environment.NewLine}");
            Random rand = new Random();
            int ms = rand.Next(1000, 3000);
            System.Threading.Thread.Sleep(ms);
            Debug.WriteLine($"{DateTime.Now:hh:mm:ss.fff tt} - {Name} End: {e.Testo}{Environment.NewLine}");
        }

    }

    public partial class MainWindow : Window
    {

        private bool alreadySubscribed1 = false;
        private bool alreadySubscribed2 = false;

        public event EventHandler<TestEventArgs> TestEvent;
        public delegate string TestEventCaller(string testo);

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

            //this Crashes
            //TestEvent.BeginInvoke(this, new TestEventArgs(txtTesto.Text),null,null);

            //Works, but Syncronous, just like Invoke
            //TestEvent(this, new TestEventArgs(txtTesto.Text));




            var args = new TestEventArgs(txtLog3.Text);

            var receivers = TestEvent.GetInvocationList();
            foreach (EventHandler<TestEventArgs> receiver in receivers)
            {
                receiver.BeginInvoke(this, args, null, null);
            }


            
         //   foreach (Delegate singleCast in TestEvent.GetInvocationList())
            //{
            
            //    try
            //    {
            //        ISynchronizeInvoke syncInvoke = (ISynchronizeInvoke)singleCast.Target;
            //        if (syncInvoke != null && syncInvoke.InvokeRequired)
            //        {
            //            syncInvoke.BeginInvoke(singleCast, new string[] { txtTesto.Text });
            //        }
            //        else
            //        {
            //            singleCast.DynamicInvoke(new string[] { txtTesto.Text });
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //    }
            //}
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
