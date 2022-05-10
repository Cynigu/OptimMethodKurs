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
        private bool isExtremMax; // true - локальный максимум, false - минимум
        private double ε; // точность
        private task task; // задача
        private string? sing; // знак ограничения второго рода
        private double xmin; // нижнее ограничение по х
        private double xmax; // верхнее ограничение по x
        private double ymin; // нижнее ограничение по y
        private double ymax; // верхнее ограничение по y
        private double k; // k: y=kx+b
        private double b; // b: y=kx+b
        public Point2 Solve()
        {
            throw new System.NotImplementedException();
        }
        public void RegisterMethod(bool max, double k, double b, string sing, double xmin, double xmax, double ymin, double ymax,
            double ε, task task)
        {
            this.isExtremMax = max;
            this.ε = ε;
            this.task = task;
            this.sing = sing;
            this.xmin = xmin;
            this.xmax = xmax;
            this.ymax = ymax;
            this.ymin = ymin;
            this.k = k;
            this.b = b;
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

        /// <summary>
        /// 1) Получить исходную точку
        /// </summary>
        private Point2 GetStartingPoint()
        {
            return new Point2()
            {
                X = k,
                Y = k*0+b
            };
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
