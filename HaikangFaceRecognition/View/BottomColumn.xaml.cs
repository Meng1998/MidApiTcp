using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DeploymentTools.Logic;

namespace DeploymentTools.View
{
    /// <summary>
    /// BottomColumn.xaml 的交互逻辑
    /// </summary>
    public partial class BottomColumn : UserControl
    {
        Timer EarthRotation = new Timer();
        public BottomColumn()
        {
            InitializeComponent();
            EarthRotation.Interval = 1000;
            EarthRotation.Elapsed += Timer_Elapsed;
            EarthRotation.Start();
        }
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (LoadingState.GetLoadingState() == "就绪")
            {
                LoadingName.Dispatcher.Invoke(new Action(() =>
                {
                    LoadingName.Visibility = Visibility.Hidden;
                    CurrentState.Content = LoadingState.GetLoadingState();
                }));
            }
            else
            {
                try
                {
                    CurrentState.Dispatcher.Invoke(new Action(() =>
                    {
                        CurrentState.Content = LoadingState.GetLoadingState();
                    }));
                }
                catch (Exception)
                {
                    Process.GetCurrentProcess().Kill();
                }
               
            }
          
           
        }
    }
}
