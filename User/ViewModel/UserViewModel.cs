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
using System.Threading.Tasks;

namespace User.ViewModel
{
    public class UserViewModel : ReactiveObject
    {
        private readonly ITasksService taskservice;
        private readonly IMethodService methodservice;

        #region поля
        private ObservableCollection<string> listSing;
        private ObservableCollection<string> listExtremum;
        private ObservableCollection<Point2> functionValue;
        private ObservableCollection<Point3> chart3Ddata;
        private ObservableCollection<Point2> chart2Ddata;
        private ObservableCollection<TaskView> tasks;
        private ObservableCollection<OptimizationMethodView> methods;
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
        private ObservableCollection<TaskParameterValueView>? parametersTable;
        private TaskParameterValueView ? selectedParameter;
        private double? parameterByTaskValue;
        private ObservableCollection<ITask>? tasksRealised;
        private ITask? selectedTaskRealised;
        private string? sourceImageFormDesc;
        private string xname;
        private string yname;
        private string cfname;
        #endregion

        #region get; set

        public string XName
        {
            get => xname;
            set => this.RaiseAndSetIfChanged(ref xname, value);
        }
        public string YName
        {
            get => yname;
            set => this.RaiseAndSetIfChanged(ref yname, value);
        }
        public string CFName
        {
            get => cfname;
            set => this.RaiseAndSetIfChanged(ref cfname, value);
        }
        public ObservableCollection<TaskParameterValueView>? ParametersTable
        {
            get => parametersTable;
            set
            {
                this.RaiseAndSetIfChanged(ref parametersTable, value);
                if (parametersTable != null)
                {
                    if (selectedTaskRealised == null)
                    {
                        MessageBox.Show("Задача не найдена");
                    }
                    else
                    {
                        selectedTaskRealised.RegisterTask(parametersTable.ToList());
                        Getε = selectedTaskRealised.ε;
                        Getxmin = selectedTaskRealised.Xmin;
                        Getymin = selectedTaskRealised.Ymin;
                        Getxmax = selectedTaskRealised.Xmax;
                        Getymax = selectedTaskRealised.Ymax;
                        Getk = selectedTaskRealised.k;
                        Getb = selectedTaskRealised.b;
                    }
                }

            }
        }

        public TaskParameterValueView? SelectedParameter
        {
            get => selectedParameter;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedParameter, value);
                if (selectedParameter != null)
                {
                    ParameterByTaskValue = selectedParameter.Value;
                }
                else
                {
                    ParameterByTaskValue = null;
                }
            }
        }
        public double? ParameterByTaskValue
        {
            get => parameterByTaskValue;
            set => this.RaiseAndSetIfChanged(ref parameterByTaskValue, value);
        }
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
        public string? SourceImageFormDesc
        {
            get => sourceImageFormDesc;
            set => this.RaiseAndSetIfChanged(ref sourceImageFormDesc, value);
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
        } // !!
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
                    var isTask = false;
                    foreach (var task in tasksRealised)
                    {
                        if (task.Name == currentTask.Name)
                        {
                            selectedTaskRealised = task;
                            isTask = true;
                            break;
                        }
                    }

                    if (!isTask)
                    {
                        MessageBox.Show("Эта задача не реализована!");
                        return;
                    }
                    SourceImageFormDesc = selectedTaskRealised?.SourceImageFormDesc;
                    DescriptionTask = currentTask.Description;
                    ParametersTable = new ObservableCollection<TaskParameterValueView>(taskservice.GetParametersByTaskId(currentTask.IdTask));
                    XName = selectedTaskRealised.X;
                    YName = selectedTaskRealised.Y;
                    CFName = selectedTaskRealised.CF;
                    Getsing = selectedTaskRealised.sing;
                }
                else
                {
                    ParametersTable = null;
                    DescriptionTask = null;
                    selectedTaskRealised = null;
                }
            }
        }
        public string Getsing
        {
            get { return sing; }
            set
            {
                this.RaiseAndSetIfChanged(ref sing, value);
                selectedTaskRealised.sing = sing;
            }
        }
        public string Getextremum
        {
            get { return extremum; }
            set
            {
                this.RaiseAndSetIfChanged(ref extremum, value);
                if (selectedTaskRealised != null)
                    selectedTaskRealised.IsExtremMax = extremum == "локальный максимум" ? true : false;
            }
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
        public UserViewModel(ITasksService taskService, IMethodService metService)
        {
            this.taskservice = taskService;
            this.methodservice = metService;

            Getmethods = new(methodservice.GetAllOptimizationMethods()
                .Where(x => x.IsRealized == true)
                .Select(el => el));
            tasksRealised = new ObservableCollection<ITask>((new Tasklist()).Tasks);
            Gettasks = new(taskservice.GetAllTask());
            GetlistSing = new() { "⩾", "⩽" };
            GetlistExtremum = new() {"локальный максимум", "локальный минимум"};
            GetfunctionValue = new();

            GetcurrentTask = Gettasks[0];
            GetcurrentMethod = Getmethods[0];
            Getsing = selectedTaskRealised?.sing ?? "⩾"; 
            Getextremum = selectedTaskRealised?.IsExtremMax ?? true ? "локальный максимум" : "локальный минимум";
            if (ParametersTable != null) selectedTaskRealised?.RegisterTask(ParametersTable.ToList());

            Getk = selectedTaskRealised?.k ?? 1;
            Getb = selectedTaskRealised?.b ?? 2;
            Getxmin = selectedTaskRealised?.Xmin ?? -3;
            Getxmax = selectedTaskRealised?.Xmax ?? 0;
            Getymin = selectedTaskRealised?.Ymin ?? - 0.5;
            Getymax = selectedTaskRealised?.Ymax ?? 3;
            Getε = selectedTaskRealised?.ε ?? 0.01;

            StepGraphX = 0.05;
            StepGraphY = 0.05;
            PointOfStartX = -2.5;
            PointOfStartY = 3;
            StepForMethodX = 0.05;
            StepForMethodY = 0.05;
            
            ChangeValueCommand = new RelayCommand(obj => ChangeValue(), 
                obj => SelectedParameter != null && ParameterByTaskValue != null
                );
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
        public RelayCommand ChangeValueCommand { get; set; }
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

                    if (selectedTaskRealised == null)
                    {
                        MessageBox.Show("Не найдено решение задачи!");
                    }
                    foreach(IMethod method in Methodlist.methods)
                    {
                        if (method.Name == currentMethod.Name) 
                        {
                            GetfunctionValue.Clear();
                            try
                            {
                                if (ParametersTable != null && selectedTaskRealised != null)
                                    selectedTaskRealised.RegisterTask(ParametersTable.ToList());
                                else
                                    return;
                            }
                            catch
                            {
                                MessageBox.Show("Значения праметров не могут быть присвоены данной функции");
                                return;
                            }

                            try
                            {
                                method.RegisterMethod(selectedTaskRealised.IsExtremMax, selectedTaskRealised.k, selectedTaskRealised.b, selectedTaskRealised.sing,
                                        selectedTaskRealised.Xmin,  selectedTaskRealised.Xmax,
                                        selectedTaskRealised.Ymin, selectedTaskRealised.Ymax, selectedTaskRealised.ε, 
                                        selectedTaskRealised.GetValueCF, PointOfStartX, PointOfStartY, StepForMethodX, StepForMethodY);

                            }
                            catch (ArgumentException ex)
                            {
                                MessageBox.Show(ex.Message);
                                return;
                            }

                            int count = BitConverter.GetBytes(decimal.GetBits((decimal)ε)[3])[2];
                            var point = method.Solve();
                            point.X = Math.Round(point.X, count);
                            point.Y = Math.Round(point.Y, count);
                            point.FunctionValue = Math.Round(point.FunctionValue, count);
                            GetfunctionValue.Add(point);
                            Getresult = $"Точки:\n" + 
                                        $"{selectedTaskRealised.X} = {GetfunctionValue[0].X}\n" +
                                        $"{selectedTaskRealised.Y}  = {GetfunctionValue[0].Y}\n" +
                                        $"ЦФ:\n" +
                                        $"{selectedTaskRealised.CF}  = {GetfunctionValue[0].FunctionValue} " +
                                        $"({selectedTaskRealised.UnitOfMeasCF})";
                            try
                            {
                                Getchart3Ddata = GetChartData(selectedTaskRealised.GetValueCF, StepGraphX, StepGraphY);
                                Getchart2Ddata = GetChartLimitationData();
                                exceldata = GetChartDataAsTable(selectedTaskRealised.GetValueCF, StepGraphX, StepGraphY);
                            }
                            catch (ArgumentException ex)
                            {
                                MessageBox.Show(ex.Message);
                                return;
                            }
                            return;
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

        private void ChangeValue()
        {
            try
            {
                if (SelectedParameter != null)
                    SelectedParameter.Value =
                        ParameterByTaskValue ?? throw new ArgumentException("Значение не введено!");
                ParametersTable = new ObservableCollection<TaskParameterValueView>(ParametersTable);
            }
            catch (ArgumentException e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private ObservableCollection<Point3> GetChartData(task task, double stepX, double stepY)
        {
            if (stepX <= 0 || stepY <= 0)
            {
                throw new ArgumentException("Шаг не может быть 0 или меньше 0");
            }
            //float step = 0.1f;
            ObservableCollection<Point3> chart3dData = new();
            if (selectedTaskRealised != null)
                for (double i = (selectedTaskRealised.Xmin); i < selectedTaskRealised.Xmax; i += stepX)
                {
                    for (double j = (selectedTaskRealised.Ymin); j < selectedTaskRealised.Ymax; j += stepY)
                    {
                        chart3dData.Add(new Point3 {X = i, Y = j, Z = (float) task(new Point2 {X = i, Y = j})});
                    }
                }

            return chart3dData;
        }
        private ObservableCollection<Point2> GetChartLimitationData()
        {
            ObservableCollection<Point2> limitationData = new();
            ObservableCollection<Point2> points = new();
            if (selectedTaskRealised != null)
            {
                double yXmin = selectedTaskRealised.k * selectedTaskRealised.Xmin + selectedTaskRealised.b;
                double yXmax = selectedTaskRealised.k * selectedTaskRealised.Xmax + selectedTaskRealised.b;
                double xYmin = (selectedTaskRealised.Ymin - selectedTaskRealised.b) / selectedTaskRealised.k;
                double xYmax = (selectedTaskRealised.Ymax - selectedTaskRealised.b) / selectedTaskRealised.k;

                if (selectedTaskRealised.Ymin <= yXmax && yXmax <= selectedTaskRealised.Ymax)
                {
                    limitationData.Add(new Point2 { X = selectedTaskRealised.Xmax, Y = yXmax });
                }
                if (selectedTaskRealised.Xmin <= xYmax && xYmax <= selectedTaskRealised.Xmax)
                {
                    limitationData.Add(new Point2 { X = xYmax, Y = selectedTaskRealised.Ymax });
                }
                if (selectedTaskRealised.Ymin <= yXmin && yXmin <= selectedTaskRealised.Ymax)
                {
                    limitationData.Add(new Point2 { X = selectedTaskRealised.Xmin, Y = yXmin });
                }
                if (selectedTaskRealised.Xmin <= xYmin && xYmin <= selectedTaskRealised.Xmax)
                {
                    limitationData.Add(new Point2 { X = xYmin, Y = selectedTaskRealised.Ymin });
                }
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
            if (selectedTaskRealised != null)
                for (double i = (selectedTaskRealised.Xmin); i < selectedTaskRealised.Xmax; i += stepX, row++)
                {
                    data.Add(new());
                    for (double j = (selectedTaskRealised.Ymin); j < selectedTaskRealised.Ymax; j += stepY)
                    {
                        data[row].Add(new Point3 {X = i, Y = j, Z = (float) task(new Point2 {X = i, Y = j})});
                    }
                }

            return data;
        }

        private void BuildGraphsMethod()
        {
            if (ParametersTable != null && selectedTaskRealised != null)
            {
                var parameters = ParametersTable.ToList();
                selectedTaskRealised.RegisterTask(parameters);
            }
            else
            {
                MessageBox.Show("Задача описана неккоректно!");
                return;
            }
            try
            {
                if (selectedTaskRealised != null)
                {
                    Getchart3Ddata = GetChartData(selectedTaskRealised.GetValueCF, StepGraphX, StepGraphY);
                    Getchart2Ddata = GetChartLimitationData();
                    exceldata = GetChartDataAsTable(selectedTaskRealised.GetValueCF, StepGraphX, StepGraphY);
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        #endregion
    }
}
