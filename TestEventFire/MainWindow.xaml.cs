using System;
using System.Windows;

namespace TestEventFire
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    internal class EventOneArgs : EventArgs
    {
        public string Testo { get; set; }
        public EventOneArgs(string testo)
        {
            Testo = testo;
        }
    }

    public partial class MainWindow : Window
    {

        private bool alreadySubscribed = false;

        private event EventHandler<EventOneArgs> EventOne;// = delegate { };

        public MainWindow()
        {
            InitializeComponent();
            //EventOne += OnEventOne;
        }

        private void OnEventOne(object sender, EventOneArgs e)
        {
            if (sender == null)
            {
                return;
            }

            System.Diagnostics.Debug.Write($"OnEventOne - {DateTime.Now}{Environment.NewLine}");
            addTimedLogText($"Received event with arg={e.Testo}");
        }

        private void btnRaiseEvent_Click(object sender, RoutedEventArgs e)
        {
            addTimedLogText("Raising Event with arg={txtTesto.Text}");
            if (EventOne != null)
            {
                EventOne(this, new EventOneArgs(txtTesto.Text));
            }
        }

        private void btnSubscribeEvent_Click(object sender, RoutedEventArgs e)
        {
            subscribeOnce();
        }
        private void subscribeOnce()
        {
            if (alreadySubscribed)
            {
                return;
            }

            addTimedLogText("Subscribing");
            alreadySubscribed = true;
            EventOne += OnEventOne;
        }

        private void btnUnSubscribeEvent_Click(object sender, RoutedEventArgs e)
        {
            if (!alreadySubscribed)
            {
                return;
            }

            addTimedLogText("UnSubscribing");
            alreadySubscribed = false;
            EventOne -= OnEventOne;

        }

        private void btnResetLog_Click(object sender, RoutedEventArgs e)
        {
            txtLog.Text = "";
            addTimedLogText("ResetLog");
        }

        private void addTimedLogText(string testo)
        {
            txtLog.Text += $"{Environment.NewLine}{DateTime.Now} - {testo}";
        }
    }
}
