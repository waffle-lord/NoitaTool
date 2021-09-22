using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NoitaTool.Outputs
{
    public static class AdditionalInfo
    {
        public static string GamePath = $@"{new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)).Parent}\LocalLow\Nolla_Games_Noita";
        public static string MainSavePath = $@"{new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)).Parent}\LocalLow\Nolla_Games_Noita\save00";
    }
}
