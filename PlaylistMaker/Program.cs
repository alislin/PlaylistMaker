using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PlaylistMaker
{
    class Program
    {
        static void Main(string[] args)
        {
            var argAction = new ArgAction(args);
            argAction.DefaultAction()
                .Help();
        }
    }

    public class ArgAction
    {
        public string Key { get; set; }
        public bool SkipAction { get; set; }
        public ArgAction(string[] args)
        {
            Args = args;
        }

        public string[] Args { get; set; }

        private bool FindKey(string key,string[] args)
        {
            var regex = new Regex(key);
            foreach (var item in args)
            {
                var m = regex.Match(item);
                if (m.Success)
                {
                    return true;
                }
            }
            return false;
        }

        public ArgAction DefaultAction()
        {
            if (SkipAction || Args.Length>0)
            {
                return this;
            }
            var mf = new MusicFile(Environment.CurrentDirectory);
            var defaultPlaylistName = "myplaylist";
            mf.MakePlayList(null, defaultPlaylistName);
            Console.WriteLine("Create playlist file: myplaylist.m3u");
            SkipAction = true;
            return this;
        }

        public ArgAction Help()
        {
            var key = @"^help$|^h$|^\?$";
            if (SkipAction || !FindKey(key,Args))
            {
                return this;
            }
            Console.WriteLine(@"Put the application in the music catalog and generate random playlists by default without parameters.");
            SkipAction = true;
            return this;
        }
    }
}
