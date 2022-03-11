using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentTools.Logic
{
    class ControlOperation
    {
       

        private class OperationModel
        {
            /// <summary>
            /// VLC操作模型
            /// </summary>
            public static Vlc.DotNet.Wpf.VlcControl  OperationModelVLC { get; set; }

        }
        public void InitializeOperationModel(ref Vlc.DotNet.Wpf.VlcControl vlcMod)
        {

            OperationModel.OperationModelVLC = vlcMod;
        }
        public void VlcControlPlay(String url) {

            //OperationModel.OperationModelVLC
            OperationModel.OperationModelVLC.SourceProvider.MediaPlayer.Play(new Uri(url));

        }
    }
}
