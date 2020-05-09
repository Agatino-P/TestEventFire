using System;
using System.Diagnostics;
using System.Threading;
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
            _mainWindow.TestEvent += ReceivedTestEvent;
        }

        private void ReceivedTestEvent(object sender, TestEventArgs e)
        {

            Debug.WriteLine($"{Name} Started working on event at: {DateTime.Now:hh:mm:ss.fff tt} Thread{System.Threading.Thread.CurrentThread.ManagedThreadId}\n ");
            int ms = rand.Next(50, 2000);
            System.Threading.Thread.Sleep(ms);
            Debug.WriteLine($"{Name} Ended working on event at: {DateTime.Now:hh:mm:ss.fff tt} after {ms:N}ms Thread{System.Threading.Thread.CurrentThread.ManagedThreadId}\n ");
        }
    }

    public partial class MainWindow : Window
    {

        private static object lockHandle = new object();

        public int MyProperty
        {
            get => (int)GetValue(MyPropertyProperty);
            set => SetValue(MyPropertyProperty, value);
        }
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("MyProperty", typeof(int), typeof(MainWindow), new PropertyMetadata(0));

        public int NumCallBacks { get; set; } = 0;

        public event EventHandler<TestEventArgs> TestEvent;// = delegate { };
        public delegate string TestEventCaller(string testo);

        public MainWindow()
        {
            InitializeComponent();
            int numSubscribers = 15;
            for (int subscriberIndex = 0; subscriberIndex < numSubscribers; subscriberIndex++)
            {
                new Subscriber(this, subscriberIndex.ToString());
            }

        }

        private void finishedCallback(IAsyncResult ar)
        {
            var dispatcher = Application.Current.Dispatcher;
            if (dispatcher.CheckAccess())
            {
                updateCounter();
            }
            else
            {
                dispatcher.Invoke(new Action(() => updateCounter()));
            }
        }

        private void updateCounter()
        {
            int thID = System.Threading.Thread.CurrentThread.ManagedThreadId;
            Debug.WriteLine($"{thID} Entering CallBack\n");


            thID = System.Threading.Thread.CurrentThread.ManagedThreadId;
          //  lock (lockHandle) //Lock is needed if not going through Dispatcher
            {
                Debug.WriteLine($"{thID} inside (Dispatched) Lock\n");
                int a = MyProperty;
                Thread.Sleep(1000);
                MyProperty = a + 1;
            }
            Debug.WriteLine($"{thID} Exiting CallBack Lock\n");
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

            MyProperty = 0;

            var receivers = TestEvent.GetInvocationList();
            foreach (EventHandler<TestEventArgs> receiver in receivers)
            {
                TestEventArgs tea = new TestEventArgs(txtLog3.Text);

                IAsyncResult ar = receiver.BeginInvoke(this, tea, finishedCallback, null);
            }
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Callbacks_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(NumCallBacks.ToString(), "CallBacks");
        }
    }
}
