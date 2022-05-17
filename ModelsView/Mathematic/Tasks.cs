namespace ModelsView.Mathematic
{
    public interface ITask
    {
        public ICollection<string> Names { get; set; }
        public string UnitOfMeasCF { get; }
        public string SourceImageFormDesc { get; }
        public string CF { get; }
        public string X { get; }
        public string Y { get; }
        public double Xmin { get;}
        public double Xmax { get;}
        public double Ymin { get; }
        public double Ymax { get; }
        public double k { get; }
        public double b { get;}
        public double ε { get; }
        public bool IsExtremMax { get; set; }
        public string sing { get; set; }
        public double GetValueCF(Point2 point);
        public void RegisterTask(List<TaskParameterValueView> parameters);
    }

    internal class RegisterTask15: ITask
    {
        public ICollection<string> Names { get; set; } = new []{"Вариант 15"};
        public string UnitOfMeasCF { get; } = "м^3";
        public string SourceImageFormDesc { get; } = "var15.png";
        public string CF { get; } = "S(T1, T2)";
        public string X { get; } = "T1";
        public string Y { get; } = "T2";
        public double Xmin { get; private set; }
        public double Xmax { get; private set; }
        public double Ymin { get; private set; }
        public double Ymax { get; private set; }
        public double k { get; private set; }
        public double b { get; private set; }
        public double ε { get; private set; }
        public bool IsExtremMax { get; set; } = true;
        public string sing { get; set; } = "⩾";

        private double a;
        private double β;
        private double y;
        private double p1;
        private double p2;
        private double N;
        private double t;

        public void RegisterTask(List<TaskParameterValueView> parameters)
        {
            if (parameters.Count == 0)
            {
                throw new ArgumentException("В базе данных нет параметров для этой задачи!");
            }
            this.a = parameters.Where(x=>x.Notation == "α")
                .Select(el=>el.Value).Single();
            this.β = parameters.Where(x => x.Notation == "β")
                .Select(el => el.Value).Single();
            this.y = parameters.Where(x => x.Notation == "γ")
                .Select(el => el.Value).Single();
            this.p1 = parameters.Where(x => x.Notation == "∆р1")
                .Select(el => el.Value).Single();
            this.p2 = parameters.Where(x => x.Notation == "∆р2")
                .Select(el => el.Value).Single();
            this.N = parameters.Where(x => x.Notation == "N")
                .Select(el => el.Value).Single();
            this.t = parameters.Where(x => x.Notation == "t")
                .Select(el => el.Value).Single();

            this.Xmin = parameters.Where(x => x.Notation == "T1min")
                .Select(el => el.Value).Single();
            this.Xmax = parameters.Where(x => x.Notation == "T1max")
                .Select(el => el.Value).Single();
            this.Ymin = parameters.Where(x => x.Notation == "T2min")
                .Select(el => el.Value).Single();
            this.Ymax = parameters.Where(x => x.Notation == "T2max")
                .Select(el => el.Value).Single();
            this.k = parameters.Where(x => x.Notation == "k")
                .Select(el => el.Value).Single();
            this.b = parameters.Where(x => x.Notation == "b")
                .Select(el => el.Value).Single();
            this.ε = parameters.Where(x => x.Notation == "ε")
                .Select(el => el.Value).Single();
        }

        public double GetValueCF(Point2 point)
        {
            double sqrt = Math.Sqrt(Math.Pow(point.X, N) + Math.Pow(point.Y, N));
            double FunctionValue = a * (point.X - β * p1) * Math.Cos(y * p2 * sqrt);
            return FunctionValue * t;
        }
    }
    internal class RegisterTask12 : ITask
    {
        public ICollection<string> Names { get; set; } = new []{"Вариант 12"};
        public string UnitOfMeasCF { get; } = "кг";
        public string SourceImageFormDesc { get; } = "var15.png";
        public string CF { get; } = "S(T1, T2)";
        public string X { get; } = "T1";
        public string Y { get; } = "T2";
        public double Xmin { get; private set; }
        public double Xmax { get; private set; }
        public double Ymin { get; private set; }
        public double Ymax { get; private set; }
        public double k { get; private set; }
        public double b { get; private set; }
        public double ε { get; private set; }
        public bool IsExtremMax { get; set; } = true;
        public string sing { get; set; } = "⩽";

        private double a;
        private double β;
        private double y;
        private double A1;
        private double A2;
        private double V1;
        private double V2;
        private double t;
        public void RegisterTask(List<TaskParameterValueView> parameters)
        {
            if (parameters.Count == 0)
            {
                throw new ArgumentException("В базе данных нет параметров для этой задачи!");
            }
            this.a = parameters.Where(x => x.Notation == "α")
                .Select(el => el.Value).Single();
            this.β = parameters.Where(x => x.Notation == "β")
                .Select(el => el.Value).Single();
            this.y = parameters.Where(x => x.Notation == "γ")
                .Select(el => el.Value).Single();
            this.A1 = parameters.Where(x => x.Notation == "A1")
                .Select(el => el.Value).Single();
            this.A2 = parameters.Where(x => x.Notation == "A2")
                .Select(el => el.Value).Single();
            this.V1 = parameters.Where(x => x.Notation == "V1")
                .Select(el => el.Value).Single();
            this.V2 = parameters.Where(x => x.Notation == "V2")
                .Select(el => el.Value).Single();

            this.t = parameters.Where(x => x.Notation == "t")
                .Select(el => el.Value).Single();
            this.Xmin = parameters.Where(x => x.Notation == "T1min")
                .Select(el => el.Value).Single();
            this.Xmax = parameters.Where(x => x.Notation == "T1max")
                .Select(el => el.Value).Single();
            this.Ymin = parameters.Where(x => x.Notation == "T2min")
                .Select(el => el.Value).Single();
            this.Ymax = parameters.Where(x => x.Notation == "T2max")
                .Select(el => el.Value).Single();
            this.k = parameters.Where(x => x.Notation == "k")
                .Select(el => el.Value).Single();
            this.b = parameters.Where(x => x.Notation == "b")
                .Select(el => el.Value).Single();
            this.ε = parameters.Where(x => x.Notation == "ε")
                .Select(el => el.Value).Single();
        }
        public double GetValueCF(Point2 point)
        {
            //С  =  α * (Т2– Т1)^А1 + β * 1 /  V1 * (Т1+Т2 -  γ *V2)^A2
            double FunctionValue = a * Math.Pow(point.Y - point.X, A1) 
                                   + β * 1 / V1 
                                   * Math.Pow(point.X + point.Y - y * V2, A2 );
            return FunctionValue * t;
        }
    }
    internal class RegisterTask13: ITask
    {
        private double a;
        private double β;
        private double μ;
        private double A;
        private double V;
        private double z;
        public double G { get; set; }
        public ICollection<string> Names { get; set; } = new []{"Вариант 13"};
        public string UnitOfMeasCF { get; } = "у.е.";
        public string SourceImageFormDesc { get; } = "var15.png";
        public string CF { get; } = "S(T1, T2)";
        public string X { get; } = "T1";
        public string Y { get; } = "T2";
        public double Xmin { get; private set; }
        public double Xmax { get; private set; }
        public double Ymin { get; private set; }
        public double Ymax { get; private set; }
        public double k { get; private set; }
        public double b { get; private set; }
        public double ε { get; private set; }
        public bool IsExtremMax { get; set; } = false;
        public string sing { get; set; } = "⩾";

        public double GetValueCF(Point2 point)
        {
            double FunctionValue = a * (G * μ * (Math.Pow(point.Y - point.X, V) + Math.Pow(β * A - point.X, V)));
            return FunctionValue * z;
        }

        public void RegisterTask(List<TaskParameterValueView> parameters)
        {
            if (parameters.Count == 0)
            {
                throw new ArgumentException("В базе данных нет параметров для этой задачи!");
            }
            this.a = parameters.Where(x => x.Notation == "α")
                .Select(el => el.Value).Single();
            this.β = parameters.Where(x => x.Notation == "β")
                .Select(el => el.Value).Single();
            this.μ = parameters.Where(x => x.Notation == "γ")
                .Select(el => el.Value).Single();
            this.A = parameters.Where(x => x.Notation == "A")
                .Select(el => el.Value).Single();
            this.V = parameters.Where(x => x.Notation == "V")
                .Select(el => el.Value).Single();
            this.G = parameters.Where(x => x.Notation == "G")
                .Select(el => el.Value).Single();

            this.z = parameters.Where(x => x.Notation == "z")
                .Select(el => el.Value).Single();

            this.Xmin = parameters.Where(x => x.Notation == "T1min")
                .Select(el => el.Value).Single();
            this.Xmax = parameters.Where(x => x.Notation == "T1max")
                .Select(el => el.Value).Single();
            this.Ymin = parameters.Where(x => x.Notation == "T2min")
                .Select(el => el.Value).Single();
            this.Ymax = parameters.Where(x => x.Notation == "T2max")
                .Select(el => el.Value).Single();
            this.k = parameters.Where(x => x.Notation == "k")
                .Select(el => el.Value).Single();
            this.b = parameters.Where(x => x.Notation == "b")
                .Select(el => el.Value).Single();
            this.ε = parameters.Where(x => x.Notation == "ε")
                .Select(el => el.Value).Single();
        }
    }
    internal class RegisterTask14 : ITask
    {
        public ICollection<string> Names { get; set; } = new []{"Вариант 14"};
        public string UnitOfMeasCF { get; } = "кг";
        public string SourceImageFormDesc { get; } = "var15.png";
        public string CF { get; } = "S(A1, A2)";
        public string X { get; } = "A1";
        public string Y { get; } = "A2";
        public double Xmin { get; private set; }
        public double Xmax { get; private set; }
        public double Ymin { get; private set; }
        public double Ymax { get; private set; }
        public double k { get; private set; }
        public double b { get; private set; }
        public double ε { get; private set; }
        public bool IsExtremMax { get; set; } = true;
        public string sing { get; set; } = "⩽";

        private double a;
        private double a1;
        private double β;
        private double β1;
        private double y;
        private double y1;
        private double V1;
        private double V2;
        private double N;
        private double t;
        public void RegisterTask(List<TaskParameterValueView> parameters)
        {
            if (parameters.Count == 0)
            {
                throw new ArgumentException("В базе данных нет параметров для этой задачи!");
            }
            this.a = parameters.Where(x => x.Notation == "α")
                .Select(el => el.Value).Single();
            this.β = parameters.Where(x => x.Notation == "β")
                .Select(el => el.Value).Single();
            this.y = parameters.Where(x => x.Notation == "γ")
                .Select(el => el.Value).Single();
            this.a = parameters.Where(x => x.Notation == "α1")
                .Select(el => el.Value).Single();
            this.β = parameters.Where(x => x.Notation == "β1")
                .Select(el => el.Value).Single();
            this.y = parameters.Where(x => x.Notation == "γ1")
                .Select(el => el.Value).Single();
            this.V1 = parameters.Where(x => x.Notation == "V1")
                .Select(el => el.Value).Single();
            this.V2 = parameters.Where(x => x.Notation == "V2")
                .Select(el => el.Value).Single();
            this.N = parameters.Where(x => x.Notation == "N")
                .Select(el => el.Value).Single();

            this.t = parameters.Where(x => x.Notation == "t")
                .Select(el => el.Value).Single();
            this.Xmin = parameters.Where(x => x.Notation == "A1min")
                .Select(el => el.Value).Single();
            this.Xmax = parameters.Where(x => x.Notation == "A1max")
                .Select(el => el.Value).Single();
            this.Ymin = parameters.Where(x => x.Notation == "A2min")
                .Select(el => el.Value).Single();
            this.Ymax = parameters.Where(x => x.Notation == "A2max")
                .Select(el => el.Value).Single();
            this.k = parameters.Where(x => x.Notation == "k")
                .Select(el => el.Value).Single();
            this.b = parameters.Where(x => x.Notation == "b")
                .Select(el => el.Value).Single();
            this.ε = parameters.Where(x => x.Notation == "ε")
                .Select(el => el.Value).Single();
        }

        public double GetValueCF(Point2 point)
        {
            //С  = α (A1^2 +β A2 – µV1)^ N + α1(β1A1 + A2^2 – µ1V2)^N,
            var px = point.X;
            var py = point.Y;
            double FunctionValue = a * Math.Pow( (Math.Pow(px,2) - β*py- y*V1), N)
                                   + a1* Math.Pow((β1*px + Math.Pow(py, 2) - y1*V2), N);
            return FunctionValue * t;
        }
    }
    public class Tasklist
    {
        private readonly List<ITask> tasks;
        public List<ITask> Tasks => tasks;
        public Tasklist()
        {
            tasks = new List<ITask>();
            tasks.Add(new RegisterTask15());
            tasks.Add(new RegisterTask13());
            tasks.Add(new RegisterTask12());
            tasks.Add(new RegisterTask14());
        }
    }
}
