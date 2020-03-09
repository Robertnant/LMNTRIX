using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace LMNTRIX_gLauncher
{
    class LauncherEvents
    {
        public static void LaunchGame()
        {
            try
            {
                Process.Start("MyFirstGame.exe");
                //string should later be changed to brainbyte.exe
                Environment.Exit(0);
            }
            catch(Exception e)
            {
                Console.WriteLine("Executable failed to open");
                throw e;
            }
        }

        public static void ViewWebsite(string url)
        {
            Process.Start(url);
        }
    }
}
