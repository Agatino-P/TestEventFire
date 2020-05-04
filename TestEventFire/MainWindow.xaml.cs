using System;
using System.Diagnostics;
using System.Windows;

namespace TestEventFire
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    internal class EventTestArgs : EventArgs
    {
        public string Testo { get; set; }
        public EventTestArgs(string testo)
        {
            Testo = testo;
        }
    }

    public partial class MainWindow : Window
    {

        private bool alreadySubscribed1 = false;
        private bool alreadySubscribed2 = false;

        private event EventHandler<EventTestArgs> EventTest;// = delegate { };

        public MainWindow()
        {
            InitializeComponent();
        }

        /// 
        /// 0
        /// 

        private void btnRaiseEvent_Click(object sender, RoutedEventArgs e)
        {
            Debug.Write($"Raising Event with arg={txtTesto.Text}{Environment.NewLine}");
            if (EventTest != null)
            {
                EventTest(this, new EventTestArgs(txtTesto.Text));
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
            EventTest += OnEventOne1;
        }

        private void OnEventOne1(object sender, EventTestArgs e)
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
            EventTest -= OnEventOne1;

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
            EventTest += OnEventOne2;
        }

        private void OnEventOne2(object sender, EventTestArgs e)
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
            EventTest -= OnEventOne2;
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



    }
}
