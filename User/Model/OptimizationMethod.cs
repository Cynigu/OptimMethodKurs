using System.Collections.Generic;
using System.Collections.ObjectModel;

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
    }
    public class Point3
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }

    internal class MethodOfTheSteepestDescent: IMethod
    {
        public string? Name { get; } = "Метод наискорейшего спуска";
        public Point2 Solve()
        {
            throw new System.NotImplementedException();
        }
        public void RegisterMethod(bool max, double k, double b, string sing, double xmin, double xmax, double ymin, double ymax,
            double ε, task task)
        {
            throw new System.NotImplementedException();
        }
        public ObservableCollection<Point3> GetChartData()
        {
            throw new System.NotImplementedException();
        }
        public ObservableCollection<Point2> GetChartLimitationData()
        {
            throw new System.NotImplementedException();
        }
        public List<List<Point3>> GetChartDataAsTable()
        {
            throw new System.NotImplementedException();
        }


    }

    internal interface IMethod
    {
        public string? Name { get; }
        public Point2 Solve();
        public void RegisterMethod(bool max, double k, double b, string sing,
            double xmin, double xmax, double ymin, double ymax, double ε, task task);
        public ObservableCollection<Point3> GetChartData();
        public ObservableCollection<Point2> GetChartLimitationData();
        public List<List<Point3>> GetChartDataAsTable();
    }

    // для функции 2-х переменных метод Бокса
    internal static class Methodlist
    {
        public static List<IMethod> methods = new()
        {
            new ComplexBoxingMethod(),
            new MethodOfTheSteepestDescent()
        };
    }
}
