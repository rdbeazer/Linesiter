using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nx09SitingTool
{
    class Randomnumber
    {
        Random random = new Random();

        public double RandomNumber()
        {
            return random.NextDouble();
        }
    }
}
