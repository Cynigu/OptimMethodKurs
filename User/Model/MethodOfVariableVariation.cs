using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace User.Model;
/// <summary>
/// Метод варьирования переменных
/// </summary>
internal class MethodOfVariableVariation: IMethod
{
    public string? Name { get; } = "Метод поочередного варьирования переменных";
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
    private double x0;
    private double y0;
    private double stepX;
    private double stepY;
    public void RegisterMethod(bool max, double k, double b, string sing, double xmin, double xmax, double ymin, double ymax,
        double ε, task task, 
        double? x0 = null, double? y0 = null,
        double? stepX = null, double? stepY = null)
    {
        this.stepX = stepX != null && stepX > 0 
            ? stepX ?? throw new ArgumentException("Введите шаг")
            : throw new ArgumentException("Шаг должен быть больше 0");
        this.stepY = stepY != null && stepY > 0
            ? stepY ?? throw new ArgumentException("Введите шаг")
            : throw new ArgumentException("Шаг должен быть больше 0");
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
        this.x0 = x0 ?? throw new ArgumentException("Должна быть исходная точка!");
        this.y0 = y0 ?? throw new ArgumentException("Должна быть исходная точка!");
        if (!CheckConditionFirstKind(this.x0, this.y0) || !CheckConditionSecondKind(this.x0, this.y0, sing))
            throw new ArgumentException("Исходная точка не соответсвует ограничениям 1ого и(или) 2ого рода!");
    }
    private bool CheckConditionSecondKind(double x, double y, string sing)  // проверить выполнение условия 2-ого рода
    {
        bool flag = false;
        switch (sing)
        {
            case "⩽": flag = (y <= k * x + b) ? true : false; break;
            case "⩾": flag = (y >= k * x + b) ? true : false; break;
            case ">": flag = (y > k * x + b) ? true : false; break;
            case "<": flag = (y < k * x + b) ? true : false; break;
        }
        return flag;
    }

    private bool CheckConditionFirstKind(double x, double y)  // проверить выполнение условия 1-ого рода
    {
        if (x < xmin || x > xmax || y < ymin || y > ymax)
            return false;
        return  true;
    }

    public Point2 Solve()
    {
        // 1) Выбор очередности варьирования переменных в цикле
        var startPoint = new Point2() {X = x0, Y = y0};
        startPoint.FunctionValue = task(startPoint);

        if (!CheckConditionSecondKind(startPoint.X, startPoint.Y, sing) || !CheckConditionFirstKind(startPoint.X, startPoint.Y))
        {
            throw new Exception("Начальная точка не удовлетворяет условиям!");
        }

        Point2 extremPoint = startPoint.Clone();
        Point2 prewExtrPoint = startPoint.Clone();
        Point2? prewPrewPoinr = null;
        do
        {
            if (prewPrewPoinr != null && Math.Abs(prewPrewPoinr.FunctionValue - extremPoint.FunctionValue) < this.ε)
            {
                stepX = stepX > 0.1 ? stepX - 0.1: stepX = 0.5;
                stepY = stepY > 0.1 ? stepY - 0.1 : stepY = 0.5;
            }
            prewPrewPoinr = prewExtrPoint.Clone();
            // Предыдущая точка экстремума
            prewExtrPoint = extremPoint.Clone();
            
            // 2) Получение интервала переменной, подозрительного на экстремум по Х
            var (intervalPointByX1, intervalPointByX2) = GetIntervalExtrByChangeX(extremPoint);

            // 3) выбор метода локализации экстремума в найденном интервале
            // (метода одномерного поиска - золотое сечение);

            var pointLocalExtrByX = GoldenRatioMethod(intervalPointByX2, intervalPointByX1);

            // 2) Получение интервала переменной, подозрительного на экстремум по Y
            var (intervalPointByY1, intervalPointByY2) = GetIntervalExtrByChangeY(pointLocalExtrByX);

            // 3) выбор метода локализации экстремума в найденном интервале
            // (метода одномерного поиска - золотое сечение);
            extremPoint = GoldenRatioMethod(intervalPointByY2, intervalPointByY1);
            // Провекрка что не вышли за ограничения 1ого рода
            if (!CheckConditionFirstKind(extremPoint.X, extremPoint.Y))
            {
                if(extremPoint.X > xmax)
                    extremPoint.X = xmax;
                else if(extremPoint.X < xmin)
                    extremPoint.X = xmin;

                if (extremPoint.Y > ymax)
                    extremPoint.Y = ymax;
                else if (extremPoint.Y < ymin)
                    extremPoint.Y = ymin;
                extremPoint.FunctionValue = task(extremPoint);
            }

            // Если точка находится на границе 2ого рода, то искать екстремум нужно на этой границе
            if (Math.Abs(extremPoint.Y - (k * extremPoint.X + b)) < 0.02 || !CheckConditionSecondKind(extremPoint.X, extremPoint.Y, sing))
            {
                var upPoint = new Point2();
                upPoint.X = extremPoint.X + stepX;
                upPoint.Y = k * upPoint.X + b;
                upPoint.FunctionValue = task(upPoint);

                if ((upPoint.FunctionValue > extremPoint.FunctionValue) == isExtremMax)
                {
                    while ((upPoint.FunctionValue > extremPoint.FunctionValue) == isExtremMax
                           && Math.Abs(upPoint.FunctionValue - extremPoint.FunctionValue) > this.ε)
                    {
                        extremPoint = upPoint.Clone();
                        upPoint.X = extremPoint.X + stepX;
                        upPoint.Y = k * upPoint.X + b;
                        upPoint.FunctionValue = task(upPoint);

                        // Провекрка что не вышли за ограничения 1ого рода
                        if (!CheckConditionFirstKind(extremPoint.X, extremPoint.Y))
                        {
                            break;
                        }
                    }

                    var intervalLimitSecond1 = extremPoint.Clone();
                    var intervalLimitSecond2 = upPoint.Clone();
                    extremPoint = GoldenRatioMethod(intervalLimitSecond1, intervalLimitSecond2);
                }

                var downPoint = new Point2();
                downPoint.X = extremPoint.X - stepX;
                downPoint.Y = k * downPoint.X + b;
                downPoint.FunctionValue = task(downPoint);

                if ((downPoint.FunctionValue > extremPoint.FunctionValue) == isExtremMax)
                {
                    while ((downPoint.FunctionValue > extremPoint.FunctionValue) == isExtremMax
                           && Math.Abs(downPoint.FunctionValue - extremPoint.FunctionValue) > this.ε)
                    {
                        extremPoint = downPoint.Clone();
                        downPoint.X = extremPoint.X + stepX;
                        downPoint.Y = k * downPoint.X + b;
                        downPoint.FunctionValue = task(downPoint);
                        // Провекрка что не вышли за ограничения 1ого рода
                        if (!CheckConditionFirstKind(extremPoint.X, extremPoint.Y))
                        {
                            break;
                        }
                    }

                    var intervalLimitSecond1 = extremPoint.Clone();
                    var intervalLimitSecond2 = downPoint.Clone();
                    extremPoint = GoldenRatioMethod(intervalLimitSecond1, intervalLimitSecond2);
                }

                if (!CheckConditionSecondKind(extremPoint.X, extremPoint.Y, sing))
                {
                    extremPoint.Y = k * extremPoint.X + b;
                    extremPoint.FunctionValue = task(extremPoint);
                }
                // Провекрка что не вышли за ограничения 1ого рода
                if (!CheckConditionFirstKind(extremPoint.X, extremPoint.Y))
                {
                    if (extremPoint.X > xmax)
                        extremPoint.X = xmax;
                    else if (extremPoint.X < xmin)
                        extremPoint.X = xmin;

                    if (extremPoint.Y > ymax)
                        extremPoint.Y = ymax;
                    else if (extremPoint.Y < ymin)
                        extremPoint.Y = ymin;
                    extremPoint.FunctionValue = task(extremPoint);
                }

            }

        } while (Math.Abs(extremPoint.FunctionValue-prewExtrPoint.FunctionValue) > ε);

        return extremPoint;
    }

    /// <summary>
    /// 3) Метод золотого сечения
    /// </summary>
    /// <param name="intervalPoint2">правая граница</param>
    /// <param name="intervalPoint1">левая граница</param>
    /// <returns></returns>
    private Point2 GoldenRatioMethod(Point2 intervalPoint2, Point2 intervalPoint1)
    {
        // 3.1) Заданы начальные границы [a, b] = [intervalPoint1, intervalPoint2] и эпсилон
        // Phi  — пропорция золотого сечения, равная 1,618
        double Phi = 1.618;

        do
        {
            // 3.2) Рассчитыввем начальные точки деления и значения ЦФ
            // p1 = b - (b-a)/Phi; 
            var pointDel1 = new Point2()
            {
                X = intervalPoint2.X - (intervalPoint2.X - intervalPoint1.X) / Phi,
                Y = intervalPoint2.Y - (intervalPoint2.Y - intervalPoint1.Y) / Phi
            };
            pointDel1.FunctionValue = task(pointDel1);

            //p2 = a + (b-a)/Phi
            var pointDel2 = new Point2()
            {
                X = intervalPoint1.X + (intervalPoint2.X - intervalPoint1.X) / Phi,
                Y = intervalPoint1.Y + (intervalPoint2.Y - intervalPoint1.Y) / Phi
            };
            pointDel2.FunctionValue = task(pointDel2);

            if ((pointDel2.FunctionValue >= pointDel1.FunctionValue) == isExtremMax)
            {
                intervalPoint1 = pointDel1.Clone();
            }
            else
            {
                intervalPoint2 = pointDel2.Clone();
            }
            // 3.3) если |b-a|<e, то переходит к расчету xэкстр, иначе к пункту 3.2
        } while (Math.Abs(intervalPoint2.X - intervalPoint1.X) >= ε
                 && Math.Abs(intervalPoint2.Y - intervalPoint1.Y) >= ε);

        var xExtr = new Point2()
        {
            X = (intervalPoint1.X + intervalPoint2.X) / 2,
            Y = (intervalPoint1.Y + intervalPoint2.Y) / 2
        };
        xExtr.FunctionValue = task(xExtr);
        return xExtr;
    }

    /// <summary>
    /// 2) Получение интервала переменной, подозрительного на экстремум по X
    /// </summary>
    /// <param name="startPoint"></param>
    /// <returns></returns>
    private (Point2 intervalPoint1, Point2 intervalPoint2) GetIntervalExtrByChangeX(Point2 startPoint)
    {
        // Выбор величины шага поиска
        var point = startPoint.Clone();

        if ((point.FunctionValue < task(new Point2()
            {
                X = point.X + stepX,
                Y = point.Y
            })) == isExtremMax
            && Math.Abs(point.FunctionValue - task(new Point2() { X = point.X + stepX, Y = point.Y })) > this.ε
            && CheckConditionSecondKind(point.X, point.Y, sing))
        {
            //  Движение осуществляется до тех пор, пока значение функции изменяется желательным образом.
            while ((point.FunctionValue < task(new Point2()
                   {
                       X = point.X + stepX,
                       Y = point.Y
                   })) == isExtremMax
                   && Math.Abs(point.FunctionValue - task(new Point2() { X = point.X + stepX, Y = point.Y })) > this.ε)
            {
                point.X += stepX;
                point.FunctionValue = task(point);
                if (!CheckConditionFirstKind(point.X, point.Y)
                    || !CheckConditionSecondKind(point.X, point.Y, sing))
                {
                    // Провекрка что не вышли за ограничения 2ого рода
                    if (!CheckConditionSecondKind(point.X, point.Y, sing))
                    {
                        point.X = (b - point.Y) / k;
                        point.FunctionValue = task(point);
                    }
                    break;
                }
            }

            point.X += stepX;
            point.FunctionValue = task(point);

            // При нарушении этого условия координата последней точки фиксируется, а за интервал,
            // «подозрительный» на экстремум, принимается интервал в два шага в направлении,
            // противоположном движению изображающей точки.

            // Левая границы интервала (в два шага в противоположнос направлении) 
            var intervalPoint1 = point.Clone();
            intervalPoint1.X -= 2 * stepX;
            intervalPoint1.FunctionValue = task(intervalPoint1);

            // правая граница
            var intervalPoint2 = point.Clone();
            return (intervalPoint1, intervalPoint2);
        }
        else if((point.FunctionValue < task(new Point2()
                {
                    X = point.X - stepX,
                    Y = point.Y
                })) == isExtremMax
                && Math.Abs(point.FunctionValue - task(new Point2() { X = point.X - stepX, Y = point.Y })) > this.ε
                && CheckConditionSecondKind(point.X, point.Y, sing))
        {
            while ((point.FunctionValue < task(new Point2()
                   {
                       X = point.X - stepX,
                       Y = point.Y
                   })) == isExtremMax
                   && Math.Abs(point.FunctionValue - task(new Point2() { X = point.X - stepX, Y = point.Y })) > this.ε)
            {
                point.X -= stepX;
                point.FunctionValue = task(point);
                if (!CheckConditionFirstKind(point.X, point.Y)
                    || !CheckConditionSecondKind(point.X, point.Y, sing))
                {
                    // Провекрка что не вышли за ограничения 2ого рода
                    if (!CheckConditionSecondKind(point.X, point.Y, sing))
                    {
                        point.X = (b - point.Y) / k;
                        point.FunctionValue = task(point);
                    }
                    break;
                }
            }

            point.X -= stepX;
            point.FunctionValue = task(point);
            
            // При нарушении этого условия координата последней точки фиксируется, а за интервал,
            // «подозрительный» на экстремум, принимается интервал в два шага в направлении,
            // противоположном движению изображающей точки.

            // Левая границы интервала (в два шага в противоположнос направлении) 
            var intervalPoint2 = point.Clone();
            intervalPoint2.X += 2 * stepX;
            intervalPoint2.FunctionValue = task(intervalPoint2);

            // правая граница
            var intervalPoint1 = point.Clone();
            return (intervalPoint2, intervalPoint1);
        }
        var point1 = point.Clone();
        point1.X -= stepX;
        var point2 = point.Clone();
        point2.X += stepX;
        return (point1, point2);
    }

    /// <summary>
    /// 2) Получение интервала переменной, подозрительного на экстремум по Y
    /// </summary>
    /// <param name="startPoint"></param>
    /// <returns></returns>
    private (Point2 intervalPoint1, Point2 intervalPoint2) GetIntervalExtrByChangeY(Point2 startPoint)
    {
        var point = startPoint.Clone();

        if ((point.FunctionValue < task(new Point2()
            {
                X = point.X,
                Y = point.Y + stepY
        })) == isExtremMax
            && Math.Abs(point.FunctionValue - task(new Point2() { X = point.X, Y = point.Y + stepY })) > this.ε
            && CheckConditionSecondKind(point.X, point.Y, sing))
        {
            //  Движение осуществляется до тех пор, пока значение функции изменяется желательным образом.
            while ((point.FunctionValue < task(new Point2() {X = point.X , Y = point.Y + stepY})) 
                   == isExtremMax 
                   && Math.Abs(point.FunctionValue - task(new Point2() {X = point.X, Y = point.Y + stepY})) > this.ε)
            {

                point.Y += stepY;
                point.FunctionValue = task(point);
                if (!CheckConditionFirstKind(point.X, point.Y)
                    || !CheckConditionSecondKind(point.X, point.Y, sing))
                {
                    //// Провекрка что не вышли за ограничения 2ого рода
                    if (!CheckConditionSecondKind(point.X, point.Y, sing))
                    {
                        point.Y = k * point.X + b;
                        point.FunctionValue = task(point);
                    }
                    break;
                }
            }

            point.Y += stepY;
            point.FunctionValue = task(point);
            
            // При нарушении этого условия координата последней точки фиксируется, а за интервал,
            // «подозрительный» на экстремум, принимается интервал в два шага в направлении,
            // противоположном движению изображающей точки.

            // Левая границы интервала (в два шага в противоположнос направлении) 
            var intervalPoint1 = point.Clone();
            intervalPoint1.Y -= 2 * stepY;
            intervalPoint1.FunctionValue = task(intervalPoint1);

            // правая граница
            var intervalPoint2 = point.Clone();
            return (intervalPoint1, intervalPoint2);
        }
        else if((point.FunctionValue < task(new Point2()
                {
                    X = point.X ,
                    Y = point.Y - stepY
        })) == isExtremMax
                && Math.Abs(point.FunctionValue - task(new Point2() { X = point.X, Y = point.Y - stepY })) > this.ε
                && CheckConditionSecondKind(point.X, point.Y, sing))
        {
            //  Движение осуществляется до тех пор, пока значение функции изменяется желательным образом.
            while ((point.FunctionValue < task(new Point2()
                   {
                       X = point.X ,
                       Y = point.Y - stepY
            })) == isExtremMax
                   && Math.Abs(point.FunctionValue - task(new Point2() { X = point.X, Y = point.Y - stepY })) > this.ε)
            {
                point.Y -= stepY;
                point.FunctionValue = task(point);
                if (!CheckConditionFirstKind(point.X, point.Y)
                    || !CheckConditionSecondKind(point.X, point.Y, sing))
                {
                    // Провекрка что не вышли за ограничения 2ого рода
                    if (!CheckConditionSecondKind(point.X, point.Y, sing))
                    {
                        point.Y = k * point.X + b;
                        point.FunctionValue = task(point);
                    }
                    break;
                }
            }
            point.Y -= stepY;
            point.FunctionValue = task(point);
            // При нарушении этого условия координата последней точки фиксируется, а за интервал,
            // «подозрительный» на экстремум, принимается интервал в два шага в направлении,
            // противоположном движению изображающей точки.

            // Левая границы интервала (в два шага в противоположнос направлении) 
            var intervalPoint2 = point.Clone();
            intervalPoint2.Y += 2 * stepY;
            intervalPoint2.FunctionValue = task(intervalPoint2);

            // правая граница
            var intervalPoint1 = point.Clone();
            return (intervalPoint2, intervalPoint1);
        }
        else
        {
            var point1 = point.Clone();
            point1.Y -= stepY;
            var point2 = point.Clone();
            point2.Y += stepY;
            return (point1, point2);
        }
    }
}