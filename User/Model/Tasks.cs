using ModelsView;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Model
{
    internal interface ITask
    {
        public string? Name { get; }
        public string ? ByObj { get; }
        public double GetTask(Point2 point);
        public double t { get; set; }
        public void RegisterTask(List<TaskParameterValueView> parameter);
        public double GetVByT(Point2 point);
    }

    internal class RegisterTask15: ITask
    {
        public string? Name { get; } = "Вариант 15";
        public string? ByObj { get; } = $"за время (ч)";
        private double a;
        private double β;
        private double y;
        private double p1;
        private double p2;
        private double N;
        public double t { get; set; }
        public void RegisterTask(List<TaskParameterValueView> parameter)
        {
            this.a = parameter.Where(x=>x.Notation == "α" && x.TaskName == Name)
                .Select(el=>el.Value).Single();
            this.β = parameter.Where(x => x.Notation == "β" && x.TaskName == Name)
                .Select(el => el.Value).Single();
            this.y = parameter.Where(x => x.Notation == "γ" && x.TaskName == Name)
                .Select(el => el.Value).Single();
            this.p1 = parameter.Where(x => x.Notation == "∆р1" && x.TaskName == Name)
                .Select(el => el.Value).Single();
            this.p2 = parameter.Where(x => x.Notation == "∆р2" && x.TaskName == Name)
                .Select(el => el.Value).Single();
            this.N = parameter.Where(x => x.Notation == "N" && x.TaskName == Name)
                .Select(el => el.Value).Single();
            this.t = parameter.Where(x => x.Notation == "t" && x.TaskName == Name)
                .Select(el => el.Value).Single();
        }

        public double GetVByT(Point2 point)
        {
            return GetTask(point) * t;
        }

        public double GetTask(Point2 point)
        {
            double sqrt = Math.Sqrt(Math.Pow(point.X, N) + Math.Pow(point.Y, N));
            double FunctionValue = a * (point.X - β * p1) * Math.Cos(y * p2 * sqrt);
            return FunctionValue;
        }
    }

    internal class RegisterTask12 : ITask
    {
        public string? Name { get; } = "Вариант 12";
        public string? ByObj { get; } = "";
        private double a;
        private double β;
        private double y;
        private double A1;
        private double A2;
        private double V1;
        private double V2;
        public double t { get; set; }
        public void RegisterTask(List<TaskParameterValueView> parameter)
        {
            this.a = parameter.Where(x => x.Notation == "α" && x.TaskName == Name)
                .Select(el => el.Value).Single();
            this.β = parameter.Where(x => x.Notation == "β" && x.TaskName == Name)
                .Select(el => el.Value).Single();
            this.y = parameter.Where(x => x.Notation == "γ" && x.TaskName == Name)
                .Select(el => el.Value).Single();
            this.A1 = parameter.Where(x => x.Notation == "A1" && x.TaskName == Name)
                .Select(el => el.Value).Single();
            this.A2 = parameter.Where(x => x.Notation == "A2" && x.TaskName == Name)
                .Select(el => el.Value).Single();
            this.V1 = parameter.Where(x => x.Notation == "V1" && x.TaskName == Name)
                .Select(el => el.Value).Single();
            this.V2 = parameter.Where(x => x.Notation == "V2" && x.TaskName == Name)
                .Select(el => el.Value).Single();
        }

        public double GetVByT(Point2 point)
        {
            return GetTask(point) * t;
        }
        public double GetTask(Point2 point)
        {
            //С  =  α * (Т2– Т1)^А1 + β * 1 /  V1 * (Т1+Т2 -  γ *V2)^A2
            double FunctionValue = a * Math.Pow(point.Y - point.X, A1) 
                                   + β * 1 / V1 
                                   * Math.Pow(point.X + point.Y - y * V2, A2 );
            return FunctionValue;
        }
    }

    internal class RegisterTask18: ITask
    {
        public string? Name { get; } = "Вариант 18";
        public string? ByObj { get; } = "";
        private double a;
        private double β;
        private double μ;
        private double A;
        private double G;
        private double N;
        public double t { get; set; }
        public double GetVByT(Point2 point)
        {
            return GetTask(point) * t;
        }
        public void RegisterTask(List<TaskParameterValueView> parameter)
        {
            this.a = parameter.Where(x => x.Notation == "α" && x.TaskName == Name)
                .Select(el => el.Value).Single();
            this.β = parameter.Where(x => x.Notation == "β" && x.TaskName == Name)
                .Select(el => el.Value).Single();
            this.μ = parameter.Where(x => x.Notation == "γ" && x.TaskName == Name)
                .Select(el => el.Value).Single();
            this.A = parameter.Where(x => x.Notation == "A" && x.TaskName == Name)
                .Select(el => el.Value).Single();
            this.G = parameter.Where(x => x.Notation == "G" && x.TaskName == Name)
                .Select(el => el.Value).Single();
            this.N = parameter.Where(x => x.Notation == "N" && x.TaskName == Name)
                .Select(el => el.Value).Single();
        }
        public double GetTask(Point2 point)
        {
            double FunctionValue = a * (G * μ * (Math.Pow(point.Y - point.X, N) + Math.Pow(β * A - point.X, N)));
            return FunctionValue;
        }
    }

    internal class RegisterTask13: ITask
    {
        private double a;
        private double β;
        private double μ;
        private double A;
        private double V;
        public double t { get; set; } // G
        public double GetVByT(Point2 point)
        {
            return GetTask(point) * t * 1000;
        }
        public string? Name { get; } = "Вариант 13";
        public string? ByObj { get; } = "при количестве реакционной массы (т)";

        public double GetTask(Point2 point)
        {
            double FunctionValue = a * (t * μ * (Math.Pow(point.Y - point.X, V) + Math.Pow(β * A - point.X, V)));
            return FunctionValue;
        }

        public void RegisterTask(List<TaskParameterValueView> parameter)
        {
            this.a = parameter.Where(x => x.Notation == "α" && x.TaskName == Name)
                .Select(el => el.Value).Single();
            this.β = parameter.Where(x => x.Notation == "β" && x.TaskName == Name)
                .Select(el => el.Value).Single();
            this.μ = parameter.Where(x => x.Notation == "γ" && x.TaskName == Name)
                .Select(el => el.Value).Single();
            this.A = parameter.Where(x => x.Notation == "A" && x.TaskName == Name)
                .Select(el => el.Value).Single();
            this.t = parameter.Where(x => x.Notation == "G" && x.TaskName == Name)
                .Select(el => el.Value).Single();
            this.V = parameter.Where(x => x.Notation == "V" && x.TaskName == Name)
                .Select(el => el.Value).Single();
        }
    }
    
    internal static class Tasklist
    {
        public static List<ITask> tasks = new List<ITask> 
        { 
            new RegisterTask15(), 
            new RegisterTask18(),
            new RegisterTask13(),
            new RegisterTask12()
        };
    }
}
