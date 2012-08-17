using System.IO;

namespace LineSiterSitingTool
{
    internal class clsBuildDirectory
    {
        public void buildDirectory(string dirPath)
        {
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
        }
    }
}