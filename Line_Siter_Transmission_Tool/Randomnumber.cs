using System;

namespace LineSiterSitingTool
{
    internal class Randomnumber
    {
        private Random random = new Random();

        public double RandomNumber()
        {
            return random.NextDouble();
        }
    }
}