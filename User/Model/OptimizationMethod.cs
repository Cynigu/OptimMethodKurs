using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Office.Interop.Excel;

namespace User.Model
{
    public enum Quality { BEST, WORST, NEUTRAL}
    public  delegate double task(Point2 point);
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

    internal interface IMethod
    {
        public string? Name { get; }
        public Point2 Solve();
        public void RegisterMethod(bool max, double k, double b, string sing,
            double xmin, double xmax, double ymin, double ymax, double ε, task task,
            double? x0 = null, double? y0 = null,
            double? stepX = null, double? stepY = null);
    }

    internal static class Methodlist
    {
        public static List<IMethod> methods = new()
        {
            new ComplexBoxingMethod(),
            new MethodOfVariableVariation()
        };
    }
}
