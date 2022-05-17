using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Office.Interop.Excel;
using ModelsView.Mathematic;

namespace User.Model
{
    
    public  delegate double task(Point2 point);

    

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
