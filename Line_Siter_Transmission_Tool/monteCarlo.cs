using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace linesiter.tools
{
    class monteCarlo
    {

        #region Properties
        private int numPasses;
        public int NumPasses
        {
            get { return numPasses; }
            set { numPasses = value; }
        }

        private double _socialWeight;
        public double socialWeight
        {
            get { return _socialWeight; }
            set { _socialWeight = value; }
        }

        private double[] _assignedWeights;
        public double[] assignedWeights
        {
            get { return _assignedWeights; }
            set { _assignedWeights = value; }
        }
        # endregion

        public void calculateWeight(double rv, double[] processArray)
        {
            try
            {
                if (rv >= processArray[0])
                {
                    this.socialWeight = this._assignedWeights[0];
                }
                else if (rv >= processArray[1])
                {
                    this.socialWeight = this._assignedWeights[1];
                }
                else if (rv >= processArray[2])
                {
                    this.socialWeight = this._assignedWeights[2];
                }
                else if (rv >= processArray[3])
                {
                    this.socialWeight = this._assignedWeights[3];
                }
                else if (rv < processArray[4])
                {
                    this.socialWeight = this._assignedWeights[4];
                }
            }
            catch (Exception ex)
            {

            }
        }

    }
}
