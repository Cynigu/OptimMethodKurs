using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsView.Mathematic
{
    public enum Quality { BEST, WORST, NEUTRAL }
    public class Point2
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double FunctionValue { get; set; }
        public bool Complex { get; set; } = false;
        public Quality Quality { get; set; } = Quality.NEUTRAL;

        public Point2 Clone()
        {
            return new Point2()
            {
                X = this.X,
                Y = this.Y,
                FunctionValue = this.FunctionValue,
                Complex = this.Complex,
                Quality = this.Quality
            };
        }
    }
    public class Point3
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }
}
