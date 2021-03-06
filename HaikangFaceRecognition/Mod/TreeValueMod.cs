using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using DeploymentTools.Logic;

namespace DeploymentTools.Mod
{
   
    public static class Data
    {
        public static IEnumerable<Folder> Folders
        {
            get
            {
                return new List<Folder>()
                {
                    new Folder()
                    {
                        Name = TreeValue.GetName(),//二级名称
                        Value = TreeValue.GetValue(),//二级值
                        SubFolders = EnumerateDirectories()
                    }
                };
            }
        }
       
        private static IEnumerable<Folder> EnumerateDirectories()
        {
            var folders = new List<Folder>();
            try
            {
                //二级
                //var folder = new Folder() { Name = 1.ToString(), Value = "999"};
                //folders.Add(folder);
                return folders;
            }
            catch (UnauthorizedAccessException)
            {
                return null;
            }
        }
    }

    public class Folder
    {
        public IEnumerable<Folder> SubFolders { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

    }
}
