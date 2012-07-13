using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LineSiterSitingTool
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
