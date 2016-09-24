using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private delegate void mainWindowIsEnabledGate(bool flag);
        private void mainWindowIsEnabled(bool flag)
        {
            if (listBoxLog.Dispatcher.Thread != Thread.CurrentThread)
            {
                mainWindowIsEnabledGate gate = new mainWindowIsEnabledGate(mainWindowIsEnabled);
                Dispatcher.Invoke(gate, new object[] { flag });
            }
            else
            {
                this.IsEnabled = flag;
            }
        }
        private delegate void buttonAbortIsEnabledGate(bool flag);
        private void buttonAbortIsEnabled(bool flag)
        {
            if (buttonAbort.Dispatcher.Thread != Thread.CurrentThread)
            {
                buttonAbortIsEnabledGate gate = new buttonAbortIsEnabledGate(buttonAbortIsEnabled);
                Dispatcher.Invoke(gate, new object[] { flag });
            }
            else
            {
                buttonAbort.IsEnabled = flag;
            }
        }
        private delegate void buttonStartIsEnabledGate(bool flag);
        private void buttonStartIsEnabled(bool flag)
        {
            if (buttonStart.Dispatcher.Thread != Thread.CurrentThread)
            {
                buttonStartIsEnabledGate gate = new buttonStartIsEnabledGate(buttonStartIsEnabled);
                Dispatcher.Invoke(gate, new object[] { flag });
            }
            else
            {
                buttonStart.IsEnabled = flag;
            }
        }
        private delegate void listBoxLogItemsAddGate(string item);
        private void listBoxLogItemsAdd(string item)
        {
            if (listBoxLog.Dispatcher.Thread != Thread.CurrentThread)
            {
                listBoxLogItemsAddGate gate = new listBoxLogItemsAddGate(listBoxLogItemsAdd);
                Dispatcher.Invoke(gate, new object[] { item });
            }
            else
            {
                listBoxLog.Items.Add(item);
            }
        }
        private Thread threadStart = null;
        private void threadStartImpl()
        {
            buttonStartIsEnabled(false);
            buttonAbortIsEnabled(true);
            for(int i = 0;i < 50;i++){
                listBoxLogItemsAdd(i.ToString());
                Thread.Sleep(1000);
            }
            buttonStartIsEnabled(true);
            buttonAbortIsEnabled(false);
        }
        private void threadStartStart()
        {
            threadStart = new Thread(threadStartImpl);
            threadStart.IsBackground = true;
            threadStart.Start();
        }
        private void threadStartAbort()
        {
            threadStart.Abort();
            threadStart.Join();
            buttonAbortIsEnabled(false);
            buttonStartIsEnabled(true);
        }
        public MainWindow()
        {
            InitializeComponent();
            buttonAbortIsEnabled(false);
            buttonStartIsEnabled(true);
        }
        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            threadStartStart();
        }

        private void buttonAbort_Click(object sender, RoutedEventArgs e)
        {
            threadStartAbort();
        }
    }
}
