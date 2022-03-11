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
using System.Windows.Shapes;

namespace DeploymentTools.View
{
    /// <summary>
    /// InformationTips.xaml 的交互逻辑
    /// </summary>
    public partial class InformationTips : Window
    {
        public InformationTips()
        {

            InitializeComponent();
        }
        public bool needChangeUI = false;
        public void Init(String LabelTitleStr, String ContentStr)
        {
            Content.Text = ContentStr;
            LabelTitle.Content = LabelTitleStr;
            needChangeUI = true;
            this.ResizeMode = ResizeMode.NoResize;
        }


        private void LabelTitle_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
