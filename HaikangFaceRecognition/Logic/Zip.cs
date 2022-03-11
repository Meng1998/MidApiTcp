using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentTools.Logic
{
    class Zip
    {
        #region 委托
        /// <summary>
        /// 解压zip文件进度回调 单文件进度
        /// </summary>
        public event UnzipFileReadProgress unzipFileReadProgress;
        /// <summary>
        /// 解压zip文件进度回调 总进度
        /// </summary> 
        public event UnzipTotalZipReadProgress unzipTotalReadProgress;
        /// <summary>
        /// 使用文件进度  开启文件进度显示速度会慢 默认关闭
        /// </summary>
        public delegate void UnzipFileReadProgress(string Filename, long Count, long inex);
        public delegate void UnzipTotalZipReadProgress(int Count, int inex);
        #endregion

        #region 进度
        FileProgress fileProgress = new FileProgress();
        //操作结束
        private bool end = false;
        //总数量
        private int totalCount = 0;
        //已执行总数量进度
        private int totalCountProgress = 0;
        #endregion
        /// <summary>
        /// 解压Zip文件
        /// </summary>
        /// <param name="ZipPath">压缩文件路径</param>
        /// <param name="goalDir">解压到目标目录</param>
        public void UnZip(string ZipPath, string goalDir = "./")
        {
            end = false;
            totalCountProgress = 0;
            goalDir = goalDir + @"\";
            goalDir = goalDir.Replace(@"\\", @"\");
            string zipFilePath = ZipPath;
            FileStream zipFileToOpen = new FileStream(zipFilePath, FileMode.Open);
            ZipArchive archive = new ZipArchive(zipFileToOpen, ZipArchiveMode.Read);

            List<DatazipType> Datalist = new List<DatazipType>();
            Task.Factory.StartNew(() => { Upui(); });
            //吧所有文件路径归类存储到list
            foreach (var zipArchiveEntry in archive.Entries)
            {
                if (zipArchiveEntry.Name == "")
                {
                    Datalist.Add(new DatazipType() { Type = 1, FullName = zipArchiveEntry.FullName });
                }
                else
                {
                    Datalist.Add(new DatazipType() { Type = 2, FullName = zipArchiveEntry.FullName });
                }
            }
            totalCount = Datalist.Count();
            //------------------
            //创建所有目录
            foreach (DatazipType v in Datalist)
            {
                if (v.Type == 1)
                {

                    if (!Directory.Exists(goalDir + v.FullName))
                    {
                        Directory.CreateDirectory(goalDir + v.FullName);
                    }
                    unzipTotalReadProgress(totalCount, ++totalCountProgress);
                }
            }
            //解压某个文件
            foreach (DatazipType v in Datalist)
            {
                if (v.Type == 2)
                {
                    ZipArchiveEntry entry = archive.GetEntry(v.FullName);

                    System.IO.Stream stream = entry.Open();
                    System.IO.Stream output = new FileStream(goalDir + v.FullName, FileMode.Create);

                    fileProgress.Filenam = v.FullName;
                    fileProgress.ProgressCount = entry.Length;
                    fileProgress.ProgressIndex = 0;
                    int b = -1;
                    while ((b = stream.ReadByte()) != -1)
                    {
                        output.WriteByte((byte)b);

                        ++fileProgress.ProgressIndex;
                    }
                    output.Close();
                    unzipTotalReadProgress(totalCount, ++totalCountProgress);
                }
            }
            end = true;
            Datalist.Clear();
            archive.Dispose();
            zipFileToOpen.Close();
        }
        //为了不影响解压速度 暂时只想到用这种方法......有没有更好的办法 留言告诉我好不好
        void Upui()
        {
            while (!end)
            {
                unzipFileReadProgress(fileProgress.Filenam, fileProgress.ProgressCount, fileProgress.ProgressIndex);
            }
        }
        public class DatazipType
        {
            /// <summary>
            /// 1 目录 2  文件
            /// </summary>
            public int Type;
            public string FullName;
        }
        //文件进度
        class FileProgress
        {
            public string Filenam;
            public long ProgressCount;
            public long ProgressIndex;
        }
    }
}
