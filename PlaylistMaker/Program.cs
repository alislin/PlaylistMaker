using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistMaker
{
    class Program
    {
        static void Main(string[] args)
        {
            var mf = new MusicFile(Environment.CurrentDirectory);
            var defaultPlaylistName = "myplaylist";

            mf.MakePlayList(null, defaultPlaylistName);
        }
    }
}
