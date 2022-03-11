using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using DeploymentTools.View;
using ISCwebApi.Controllers;

namespace DeploymentTools.View
{
    /// <summary>
    /// TopBar.xaml 的交互逻辑
    /// </summary>
    public partial class TopBar : UserControl
    {
        public TopBar()
        {
            InitializeComponent();
        }


        private void MenuItemIndex2_Click(object sender, RoutedEventArgs e)
        {
            new Synchronization().Show();
        }

        private void MenuItemIndex3_Click(object sender, RoutedEventArgs e)
        {
            new EventSubscriptions().Show();
        }
        private void MenuItemIndex4_Click(object sender, RoutedEventArgs e)
        {
            // new InformationTips().Show();

            //InformationTips F2 = new InformationTips();
            //F2.Init("人脸数据同步提示", "抱歉，人脸数据同步功能暂不开放。如有功能要求可能是软件不是最新版");
            //F2.ShowDialog();
            //if (F2.needChangeUI)
            //{
            //  //OK
            //}
            //new FaceDataFrom().Show();
        }
        /// <summary>
        /// 弹出创建ISCapi界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EstablishIISWEBAPI_Click(object sender, RoutedEventArgs e)
        {
            LnstallWEBAPI F2 = new LnstallWEBAPI();
            F2.Show();
        }

        private void MenuItemIndex1_Click(object sender, RoutedEventArgs e)
        {
            new EquipmentConfig().Show();
        }

        private void MenuItemIndex5_Click(object sender, RoutedEventArgs e)
        {
            new AccessControlUpload().Show();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            InformationTips F2 = new InformationTips();
            F2.Init("关于", "Picture-ISC Deployment Tool 2019 版本 1.1.2 \r\n 本工具由 ©图洋 设计研发保有权利 \r\n\r\n警告：本计算机程序受著作权法和国际条约保护。如未经授权而擅自复制或传播本程序（或其中任何部分），将受到严厉的民事及刑事制裁，并将在法律许可范围内受到最大程度的起诉。");
            F2.ShowDialog();
            if (F2.needChangeUI)
            {
                //OK
            }
        }

        private void MenuItemIndex6_Click(object sender, RoutedEventArgs e)
        {
            new UpdateDeviceName().Show();
        }

        private void ServiceRunningStatus_Click(object sender, RoutedEventArgs e)
        {
            new ServiceRunningStatus().Show();
        }

        private void MenuItemIndex7_Click(object sender, RoutedEventArgs e)
        {
            new SynchronousTalkback().Show();
        }

        private void SQLconfigapp_Click(object sender, RoutedEventArgs e)
        {
            new CareTbview().Show();
        }

        private void WebapiConfig_Click(object sender, RoutedEventArgs e)
        {
            new TreeConfig().Show();
        }

        private void MenuItemIndex8_Click(object sender, RoutedEventArgs e)
        {
            new SynchronousSensor().Show();
        }

        private void MenuItemIndex10_Click(object sender, RoutedEventArgs e)
        {
            new DaHua_Login().Show();
        }

        private void MenuItemIndex19_Click(object sender, RoutedEventArgs e)
        {
            new ZoneSynchronization().Show();
        }

        private void MenuItemIndex3_Click_1(object sender, RoutedEventArgs e)
        {
            new parkList().Show();
        }

        private void MenuItemIndex21_Click(object sender, RoutedEventArgs e)
        {
            new fangqu().Show();
        }

        private void MenuItemIndex22_Click(object sender, RoutedEventArgs e)
        {
           // new SPD().Show();
        }
    }
}
