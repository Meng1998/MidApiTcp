using DeploymentTools.Logic;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace DeploymentTools.View
{
  
    /// <summary>
    /// KeyAuthorization.xaml 的交互逻辑
    /// </summary>
    public partial class KeyAuthorization : Window
    {
        System.Timers.Timer ServerStatrT = new System.Timers.Timer();
        Int32 count = 0;
        public KeyAuthorization()
        {
            InitializeComponent();

          


           
            AuthorizationStatus.FromStatus = true;
            ServerStatrT.Interval = 100;
            ServerStatrT.Start();
            ServerStatrT.Elapsed += ServerStatrTC;
        }
        /// <summary>
        /// 检测授权
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServerStatrTC(object sender, ElapsedEventArgs e)
        {
            SQTextBox.Dispatcher.Invoke(new Action(() =>
            {
               
                   
                if (SQTextBox.Password == "ty1409ty.." || AuthorizationStatus.Status)
                {
                    count++;
                    SQstate.Content = $"密钥授权状态(成功) {3 - (count / 10)}秒后自动关闭";//CC00FF8B
                    System.Windows.Media.Color color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#CC00FF8B");
                    SQstate.Background = new SolidColorBrush(color);

                    var a = 3 - (count / 10);
                    if (a <= 0) { AuthorizationStatus.Status = true; AuthorizationStatus.FromStatus = false;  Close(); ServerStatrT.Close(); }

                }
                else
                {
                    System.Windows.Media.Color color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFFF3333");
                    SQstate.Background = new SolidColorBrush(color);
                    SQstate.Content = "密钥授权状态(异常)";
                    AuthorizationStatus.Status = false;
                }
            }));

         
        }

        private void DatabaseAPI_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            AuthorizationStatus.FromStatus = false;
        }
    }
}
