using DeploymentTools.Logic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DeploymentTools
{
  
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Fluent.RibbonWindow
    {
        private void Rotation3DUIC(object sender, ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(delegate
            {
                if (Rotation3D.Angle >= 360)
                    Rotation3D.Angle = 0;
                Rotation3D.Angle++;
            }));
            //settings.Locale = "zh-CN";
            //settings.BrowserSubprocessPath = @"x86\CefSharp.BrowserSubprocess.exe";

            //Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);
            //var browser = new BrowserForm();
            //System.Windows.Application.Run(browser);
            //var browser = new BrowserForm();
            //Application.Run(browser);

        }
        NotifyIcon notifyIcon = new NotifyIcon();
        System.Timers.Timer Rotation3DUI = new System.Timers.Timer();
        public MainWindow()
        {
           
            InitializeComponent();

            //Rotation3DUI.AutoReset = false;
            Rotation3DUI.Interval = 10;
            Rotation3DUI.Elapsed += Rotation3DUIC;
            Rotation3DUI.Start();

        
            //初始化主窗体控件操作模型
            InitializeOperationModel();
            //初始化主窗体控件数据或
            InitUIdata();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //System.Windows.Application.Exit();
            Rotation3DUI.Close();
            notifyIcon.Dispose();
            Process.GetCurrentProcess().Kill();
            Close();
        }
        private void InitUIdata() {
            String LookMonitoringPoints = Config.GetConfigValue("LookMonitoringPoints");
            String SetupStatus = Config.GetConfigValue("SetupStatus");
            if (LookMonitoringPoints == true.ToString())
            {
                SetupStatusBT_Copy.IsChecked = true;
            }
            else
            {
                SetupStatusBT_Copy.IsChecked = false;
                IFseeBUT.IsChecked = false;
                MessGrid.Visibility = Visibility.Hidden;
            }
            if (SetupStatus == true.ToString())
            {
                IFseeBUT.IsChecked = true;
            }

            
           notifyIcon.BalloonTipText = "欢迎使用图洋，摄像机配置工具!";
           notifyIcon.Text = "图洋数据对ISC配置工具";
           notifyIcon.Icon  = Properties.Resources.Ico;
           notifyIcon.Visible = true;
           notifyIcon.ShowBalloonTip(1000);
        }

        private void FaceLogin_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("按钮测试");
        }

        private void OnPlayButtonClick(object sender, RoutedEventArgs e)
        {
            
        }
        private void InitializeOperationModel()
        {
           //new ControlOperation().InitializeOperationModel(ref vlcPlayer);//.SourceProvider.MediaPlayer;
        }

        private void SetupStatusBT_Copy_Click(object sender, RoutedEventArgs e)
        {
            if (SetupStatusBT_Copy.IsChecked == true)
                MessGrid.Visibility = Visibility.Visible;
            else
                MessGrid.Visibility = Visibility.Hidden;
            Config.SetConfigValue("LookMonitoringPoints", SetupStatusBT_Copy.IsChecked.ToString());
            Config.SetConfigValue("SetupStatus", IFseeBUT.IsChecked.ToString());
        }

        private void IFseeBUT_Click(object sender, RoutedEventArgs e)
        {
            Config.SetConfigValue("LookMonitoringPoints", SetupStatusBT_Copy.IsChecked.ToString());
            Config.SetConfigValue("SetupStatus", IFseeBUT.IsChecked.ToString());
        }
        private void SliderOpacity_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //vlcPlayer.Opacity = (SliderOpacity.Value / 100);
        }
    }
}
