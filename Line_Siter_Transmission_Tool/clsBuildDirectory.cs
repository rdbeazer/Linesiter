using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace nx09SitingTool
{
    class clsBuildDirectory
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
