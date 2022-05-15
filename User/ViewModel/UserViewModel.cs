using Autofac;
using AutofacDependence;
using ModelsView;
using ReactiveUI;
using Repository.factories;
using Services;
using Services.Interfaces;
using ServicesMVVM;
using System.Collections.ObjectModel;
using System.Linq;
using User.View;
using System.Windows.Input;
using User.Model;
using Xceed.Wpf.Toolkit;
using System.Collections.Generic;
using User.Model.FileServices;
using Syncfusion.XlsIO;
using System;

namespace User.ViewModel
{
    public class UserViewModel : ReactiveObject
    {
        #region поля
        private ObservableCollection<string> listSing;
        private ObservableCollection<string> listExtremum;
        private ObservableCollection<Point2> functionValue;
        private ObservableCollection<Point3> chart3Ddata;
        private ObservableCollection<Point2> chart2Ddata;
        private ObservableCollection<TaskView> tasks;
        private ObservableCollection<OptimizationMethodView> methods;
        private ObservableCollection<TaskParameterValueView> taskParameters;
        private OptimizationMethodView currentMethod;
        private List<List<Point3>> exceldata;
        private TaskView currentTask;
        private DialogService dialogService;
        private FileService fileService;
        private string sing;
        private double k;
        private double b;
        private double xmin;
        private double xmax;
        private double ymin;
        private double ymax;
        private string extremum;
        private string result;
        private double ε;
        private double _stepGraphX;
        private double _stepGraphY;
        private string? descriptionTask;
        private bool isVariableParametersMethod;
        private double pointOfStartX;
        private double pointOfStartY;
        private double stepForMethodY;
        private double stepForMethodX;
        #endregion

        #region get; set
        public double StepForMethodX
        {
            get => stepForMethodX;
            set => this.RaiseAndSetIfChanged(ref stepForMethodX, value);
        }
        public double StepForMethodY
        {
            get => stepForMethodY;
            set => this.RaiseAndSetIfChanged(ref stepForMethodY, value);
        }
        public double PointOfStartX
        {
            get => pointOfStartX;
            set => this.RaiseAndSetIfChanged(ref pointOfStartX, value);
        }
        public double PointOfStartY
        {
            get => pointOfStartY;
            set => this.RaiseAndSetIfChanged(ref pointOfStartY, value);
        }
        public double StepGraphX
        {
            get => _stepGraphX;
            set => this.RaiseAndSetIfChanged(ref _stepGraphX, value);
        }
        public double StepGraphY
        {
            get => _stepGraphY;
            set => this.RaiseAndSetIfChanged(ref _stepGraphY, value);
        }
        public bool IsVariableParametersMethod
        {
            get
            {
                return isVariableParametersMethod;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref isVariableParametersMethod, value);
            }
        }
        public string? DescriptionTask
        {
            get => descriptionTask;
            set => this.RaiseAndSetIfChanged(ref descriptionTask, value);
        }

        public double Getk
        {
            get { return k; }
            set { this.RaiseAndSetIfChanged(ref k, value); }
        }
        public double Getb
        {
            get { return b; }
            set { this.RaiseAndSetIfChanged(ref b, value); }
        }
        public double Getxmin
        {
            get { return xmin; }
            set { this.RaiseAndSetIfChanged(ref xmin, value); }
        }
        public double Getxmax
        {
            get { return xmax; }
            set { this.RaiseAndSetIfChanged(ref xmax, value); }
        }
        public double Getymin
        {
            get { return ymin; }
            set { this.RaiseAndSetIfChanged(ref ymin, value); }
        }
        public double Getymax
        {
            get { return ymax; }
            set { this.RaiseAndSetIfChanged(ref ymax, value); }
        }
        public ObservableCollection<string> GetlistSing
        {
            get { return listSing; }
            set { this.RaiseAndSetIfChanged(ref listSing, value); }
        }
        public ObservableCollection<string> GetlistExtremum
        {
            get { return listExtremum; }
            set { this.RaiseAndSetIfChanged(ref listExtremum, value); }
        }
        public ObservableCollection<Point3> Getchart3Ddata
        {
            get { return chart3Ddata; }
            set { this.RaiseAndSetIfChanged(ref chart3Ddata, value); }
        }
        public ObservableCollection<Point2> Getchart2Ddata
        {
            get { return chart2Ddata; }
            set { this.RaiseAndSetIfChanged(ref chart2Ddata, value); }
        }
        public ObservableCollection<Point2> GetfunctionValue
        {
            get { return functionValue; }
            set { this.RaiseAndSetIfChanged(ref functionValue, value); }
        }
        public ObservableCollection<TaskView> Gettasks
        {
            get { return tasks; }
            set { this.RaiseAndSetIfChanged(ref tasks, value); }
        }
        public ObservableCollection<OptimizationMethodView> Getmethods
        {
            get { return methods; }
            set { this.RaiseAndSetIfChanged(ref methods, value); }
        }
        public OptimizationMethodView GetcurrentMethod
        {
            get { return currentMethod; }
            set
            {
                this.RaiseAndSetIfChanged(ref currentMethod, value);
                if (currentMethod.Name == "Метод поочередного варьирования переменных")
                {
                    IsVariableParametersMethod = true;
                }
                else
                {
                    IsVariableParametersMethod = false;
                }
            }
        }
        public TaskView GetcurrentTask
        {
            get { return currentTask; }
            set
            {
                this.RaiseAndSetIfChanged(ref currentTask, value);
                if (currentTask != null)
                {
                    DescriptionTask = currentTask.Description;
                }
                else
                {
                    DescriptionTask = null;
                }
            }
        }
        public string Getsing
        {
            get { return sing; }
            set { this.RaiseAndSetIfChanged(ref sing, value); }
        }
        public string Getextremum
        {
            get { return extremum; }
            set { this.RaiseAndSetIfChanged(ref extremum, value); }
        }
        public string Getresult
        {
            get { return result; }
            set { this.RaiseAndSetIfChanged(ref result, value); }
        }
        public double Getε
        {
            get { return ε; }
            set { this.RaiseAndSetIfChanged(ref ε, value); }
        }
        #endregion

        #region конструкторы
        public UserViewModel()
        {
            GetlistSing = new() { ">", "<", "⩾", "⩽" };
            GetlistExtremum = new() { "локальный максимум", "локальный минимум" };
            Getextremum = "локальный максимум";
            Getsing = "⩾";
            Getk = 1;
            Getb = 2;
            Getxmin = -3;
            Getxmax = 0;
            Getymin = -0.5;
            Getymax = 3;
            Getε = 0.01;
            GetfunctionValue = new();
            ContainerBuilder builderBase = new();
            builderBase.RegisterModule(new RepositoryModule());
            var sql = builderBase.Build().Resolve<ISqlLiteRepositoryContextFactory>();
            ITasksService taskservice = new TasksService(sql);
            IMethodService methodservice = new MethodService(sql);
            Gettasks = new (taskservice.GetAllTask());
            GetcurrentTask = Gettasks[0];
            Getmethods = new (methodservice.GetAllOptimizationMethods().Where(x=>x.IsRealized == true).Select(el=>el));
            GetcurrentMethod = Getmethods[0];
            taskParameters = new(taskservice.GetAllParametersValues());
            StepGraphX = 0.05;
            StepGraphY = 0.05;
            PointOfStartX = -2.5;
            PointOfStartY = 3;
            StepForMethodX = 0.05;
            StepForMethodY = 0.05;
        }
        #endregion

        #region команды
        private RelayCommand start;
        private RelayCommand build3DChart;
        private RelayCommand buildGraphs;
        private RelayCommand clear;
        private RelayCommand taskDescription;
        private RelayCommand reference;
        private RelayCommand saveresult;
        public ICommand Start
        {
            get
            {
                return start ??= new RelayCommand(obj =>
                {
                    if (xmax <= xmin || ymax <= ymin) 
                    {
                        MessageBox.Show("Неверно заданы ограничения области", "Ошибка");
                        return;
                    }
                    foreach(ITask task in Tasklist.tasks)
                    {
                        if(task.Name == currentTask.Name)
                        {
                            foreach(IMethod method in Methodlist.methods)
                            {
                                if (method.Name == currentMethod.Name) 
                                {
                                    GetfunctionValue.Clear();
                                    var parameters = taskParameters.Where(x => x.TaskId == currentTask.IdTask).Select(el => el).ToList();
                                    task.RegisterTask(parameters);
                                    try
                                    {
                                        if (Getextremum == "локальный максимум")
                                        {
                                            method.RegisterMethod(true, k, b, sing, xmin, xmax, ymin, ymax, ε, 
                                                task.GetTask, PointOfStartX, PointOfStartY, StepForMethodX, StepForMethodY);
                                        }
                                        if (Getextremum == "локальный минимум")
                                        {
                                            method.RegisterMethod(false, k, b, sing, xmin, xmax, ymin, ymax, ε, 
                                                task.GetTask, PointOfStartX, PointOfStartY, StepForMethodX, StepForMethodY);
                                        }
                                    }
                                    catch (ArgumentException ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                        return;
                                    }

                                    int count = BitConverter.GetBytes(decimal.GetBits((decimal)ε)[3])[2];
                                    GetfunctionValue.Add(method.Solve());
                                    GetfunctionValue[0].X = Math.Round(GetfunctionValue[0].X, count);
                                    GetfunctionValue[0].Y = Math.Round(GetfunctionValue[0].Y, count);
                                    GetfunctionValue[0].FunctionValue = Math.Round(GetfunctionValue[0].FunctionValue, count);
                                    Getresult = $"X = {GetfunctionValue[0].X}\n" +
                                                $"Y = {GetfunctionValue[0].Y}\n" +
                                                $"Значение целевой функции в точке\n" +
                                                $"F(X, Y) = {GetfunctionValue[0].FunctionValue}";
                                    try
                                    {
                                        Getchart3Ddata = GetChartData(task.GetTask, StepGraphX, StepGraphY);
                                        Getchart2Ddata = GetChartLimitationData();
                                        exceldata = GetChartDataAsTable(task.GetTask, StepGraphX, StepGraphY);
                                    }
                                    catch (ArgumentException ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                        return;
                                    }
                                    return;
                                }
                            }
                            break;
                        }
                    }
                    MessageBox.Show("Задача или метод оптимизации не найдены", "Ошибка");
                });
            }
        }
        public ICommand Clear
        {
            get
            {
                return clear ??= new RelayCommand(obj =>
                {
                    GetfunctionValue.Clear();
                    Getchart3Ddata.Clear();
                    Getchart2Ddata.Clear();
                    Getresult = "";
                });
            }
        }
        public ICommand Build3DChart
        {
            get
            {
                return build3DChart ??
                  (build3DChart = new RelayCommand(obj =>
                  {
                      var builderBase = Container.GetBuilder().Build();
                      Chart3D Chart3D = new() {
                          DataContext = builderBase.Resolve<Chart3DViewModel>(new NamedParameter("p1", chart3Ddata))
                      };
                      Chart3D.ShowDialog();
                  }));
            }
        }
        
        public ICommand BuildGraphs
        {
            get
            {
                return buildGraphs ??
                       (build3DChart = new RelayCommand(obj =>
                       {
                           BuildGraphsMethod();
                       }));
            }
        }
        public ICommand TaskDescription
        {
            get
            {
                return taskDescription ??
                  (taskDescription = new RelayCommand(obj =>
                  {
                      var builderBase = Container.GetBuilder().Build();
                      TaskDescription TaskDescription = new()
                      {
                          DataContext = builderBase.Resolve<TaskDescriptionViewModel>(new NamedParameter("p1", currentTask.Description))
                      };
                      TaskDescription.Show();
                  }));
            }
        }
        public ICommand Reference
        {
            get
            {
                return reference ??
                  (reference = new RelayCommand(obj =>
                  {
                      string message = "Курсовая работа по дисциплине \"Методы оптимизации\"" +
                                       "\nТема: Разработка программного комплекса для решения задачи оптимизации " +
                                       "\nзаданного объекта исследования" +
                                       "\nВариант: 15" +
                                       "\nВыполнила студентка 3 курса: Тюлькина И.П." +
                                       "\nГруппа: 494";
                      MessageBox.Show(message, "Справка");
                  }));
            }
        }
        public ICommand Saveresult
        {
            get
            {
                return saveresult ??
                  (saveresult = new RelayCommand(obj =>
                  {
                      IApplication application = (new ExcelEngine()).Excel;
                      if (application == null)
                      {
                          MessageBox.Show("Excel не установлен на вашем устройстве", "Ошибка");
                          return;
                      }
                      if (exceldata == null)
                      {
                          MessageBox.Show("Недостаточно данных", "Ошибка");
                          return;
                      }
                      var builderBase = Container.GetBuilder().Build();
                      dialogService = builderBase.Resolve<DialogService>();
                      fileService = builderBase.Resolve<FileService>();
                      string data =
                      $"Задача: {currentTask.Name}\n" +
                      $"Метод: {currentMethod.Name}\n" +
                      $"Значение целевой функции в точке\n" +
                      $"X = {GetfunctionValue[0].X}\n" +
                      $"Y = {GetfunctionValue[0].Y}\n" +
                      $"F(X, Y) = {GetfunctionValue[0].FunctionValue}";
                      SaveFile.SaveXls(exceldata, data, application);
                      try
                      {
                          if (!dialogService.SaveFileDialog()) { return; }
                          fileService.Save(dialogService.FilePath, application);
                      }
                      catch
                      {
                          string message = "Не удалось сохранить файл.";
                          MessageBox.Show(message, "Ошибка");
                      }

                  }));
            }
        }
        #endregion

        #region Methods

        private ObservableCollection<Point3> GetChartData(task task, double stepX, double stepY)
        {
            if (stepX <= 0 || stepY <= 0)
            {
                throw new ArgumentException("Шаг не может быть 0 или меньше 0");
            }
            //float step = 0.1f;
            ObservableCollection<Point3> chart3dData = new();
            for (double i = (xmin); i < xmax; i += stepX)
            {
                for (double j = (ymin); j < ymax; j += stepY)
                {
                    chart3dData.Add(new Point3 { X = i, Y = j, Z = (float)task(new Point2 { X = i, Y = j }) });
                }
            }
            return chart3dData;
        }
        private ObservableCollection<Point2> GetChartLimitationData()
        {
            ObservableCollection<Point2> limitationData = new();
            ObservableCollection<Point2> points = new();
            double yXmin = k * xmin + b;
            double yXmax = k * xmax + b;
            double xYmin = (ymin - b) / k;
            double xYmax = (ymax - b) / k;

            if (ymin <= yXmax && yXmax <= ymax)
            {
                limitationData.Add(new Point2 { X = xmax, Y = yXmax });
            }
            if (xmin <= xYmax && xYmax <= xmax)
            {
                limitationData.Add(new Point2 { X = xYmax, Y = ymax });
            }
            if (ymin <= yXmin && yXmin <= ymax)
            {
                limitationData.Add(new Point2 { X = xmin, Y = yXmin });
            }
            if (xmin <= xYmin && xYmin <= xmax)
            {
                limitationData.Add(new Point2 { X = xYmin, Y = ymin });
            }
            return limitationData;
        }
        private List<List<Point3>> GetChartDataAsTable(task task, double stepX, double stepY)
        {
            if (stepX <= 0 || stepY <= 0)
            {
                throw new ArgumentException("Шаг не может быть 0 или меньше 0");
            }
            //float step = 0.5f;
            List<List<Point3>> data = new(); int row = 0;
            for (double i = (xmin); i < xmax; i += stepX, row++)
            {
                data.Add(new());
                for (double j = (ymin); j < ymax; j += stepY)
                {
                    data[row].Add(new Point3 { X = i, Y = j, Z = (float)task(new Point2 { X = i, Y = j }) });
                }
            }
            return data;
        }

        private void BuildGraphsMethod()
        {
            foreach (ITask task in Tasklist.tasks)
            {
                if (task.Name == currentTask.Name)
                {
                    var parameters = taskParameters.Where(x => x.TaskId == currentTask.IdTask).Select(el => el).ToList();
                    task.RegisterTask(parameters);
                    try
                    {
                        Getchart3Ddata = GetChartData(task.GetTask, StepGraphX, StepGraphY);
                        Getchart2Ddata = GetChartLimitationData();
                        exceldata = GetChartDataAsTable(task.GetTask, StepGraphX, StepGraphY);
                    }
                    catch (ArgumentException ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }
                }
            }
        }

        #endregion
    }
}
