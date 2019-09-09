/* Ceated by Ya Lin. 2019/9/6 13:22:00 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistMaker
{
    public class Mp3List
    {
        public Mp3List(string name, string rootPath,Func<List<string>, List<string>> sortAction=null)
        {
            Name = name;
            RootPath = rootPath;
            if (RootPath.Last()!='\\')
            {
                RootPath += "\\";
            }
            SortAction = sortAction;
            Init();
        }

        private string MusicExt = "*.mp3";
        public Func<List<string>, List<string>> SortAction { get; set; }
        public string Name { get; set; }
        public string RootPath { get; set; }
        protected List<FileInfo> Files { get; set; } = new List<FileInfo>();
        public List<string> PlayList => GetPlayList();

        private List<string> GetPlayList()
        {
            var rootLen = RootPath.Length;
            var list = Files.Select(x => x.FullName.Substring(rootLen, x.FullName.Length - rootLen)).ToList();
            if (SortAction != null)
            {
                list = SortAction(list);
            }
            return list;
        }

        private void Init()
        {
            try
            {
                var dinfo = new DirectoryInfo(Name);
                Files.AddRange(dinfo.GetFiles(MusicExt));
            }
            catch (Exception ex)
            {
            }
        }

    }

    public class MusicFile
    {
        public List<Mp3List> Mp3Lists { get; set; } = new List<Mp3List>();
        public MusicFile(string rootPath)
        {
            RootPath = rootPath;
            Init();
        }

        public string RootPath { get; set; }
        public string PlayListExt { get; set; } = "m3u";

        private void Init()
        {
            
            try
            {
                var dinfo = new DirectoryInfo(RootPath);
                var list = GetDirInfo(dinfo);
                foreach (var item in list)
                {
                    var mlist = new Mp3List(item.FullName, RootPath);
                    Mp3Lists.Add(mlist);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private DirectoryInfo[] GetDirInfo(DirectoryInfo di)
        {
            var dlist = new List<DirectoryInfo>();
            try
            {
                var list = di.GetDirectories();
                
                foreach (var item in list)
                {
                    dlist.AddRange(GetDirInfo(item));
                }

                dlist.AddRange(list);
            }
            catch (Exception ex)
            {

            }
            return dlist.ToArray();
        }

        public List<string> PlayList(string key = null)
        {
            var list = new List<string>();
            var result = Mp3Lists.Where(x => string.IsNullOrWhiteSpace(key) || x.Name.ToLower().IndexOf(key.ToLower()) >= 0).ToList();
            foreach (var item in result)
            {
                list.AddRange(item.PlayList);
            }
            return list;
        }

        private List<string> RandomSort(List<string> list)
        {
            var result = new List<string>();
            var rnd = new Random(DateTime.Now.Millisecond);
            var len = list.Count;
            for (int i = 0; i < len; i++)
            {
                var index = rnd.Next(list.Count);
                var item = list[index];
                result.Add(item);
                list.Remove(item);
            }
            //result.Add(list[0]);
            return result;
        }

        public void MakePlayList(string key=null,string fileName=null)
        {
            var fname = $"{fileName}.{PlayListExt}";
            var list = PlayList(key);
            list = RandomSort(list);
            Console.WriteLine($"Total music files: {list.Count}");
            try
            {
                var s = "#EXTM3U\r\n";
                s += string.Join("\r\n", list);
                File.WriteAllText(fname, s);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
