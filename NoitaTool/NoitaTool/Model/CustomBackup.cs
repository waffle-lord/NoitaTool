using System.IO;

namespace NoitaTool.Model
{
    public class CustomBackup
    {
        public bool IsEmpty;
        public string Name;
        public int Index;
        public string FullPath;

        public CustomBackup(string Name, int Index, string FullPath, bool IsEmpty = false)
        {
            this.IsEmpty = IsEmpty;
            this.Name = Name;
            this.Index = Index;
            this.FullPath = FullPath;
        }
    }
}
