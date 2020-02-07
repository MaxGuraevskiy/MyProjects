using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork08._02._20
{
    public delegate void ErrorNotificationType(string message);
    class Calculator
    {
        
        public delegate double MathOperation(double a, double b);
        public delegate double CheckerUncheck(MathOperation operation, double a, double b);

        static Dictionary<string, MathOperation> operations;

        private static event ErrorNotificationType ErrorNotification; // Можно сделать поле паблик и подписать из мейна

        // Конструктор Calculator. plus - операция сложения, divide - операция деления, mod - операция остатка от деления.
        static Calculator()
        {
            CheckerUncheck check = (MathOperation operation, double a, double b) =>
            {
                try
                {
                    double answer = checked(operation(a, b));

                    return answer;
                }
                catch (Exception)
                {
                    throw;
                }
            };

            MathOperation plus = (x, y) => x + y;
            MathOperation divide = (x, y) => x / y;
            MathOperation mod = (x, y) => x % y;
            MathOperation minus = (x, y) => x - y;
            MathOperation multiply = (x, y) => x * y;
            MathOperation pow = (x, y) => Math.Pow(x, y);
            MathOperation gcd = (x, y) =>
            {
                while (x != 0 && y != 0)
                {
                    if (x > y)
                        x %= y;
                    else
                        y %= x;
                }

                return x == 0 ? y : x;
            };

            operations = new Dictionary<string, MathOperation>
            {

                ["+"] = (x, y) => check(plus, x, y),
                ["-"] = (x, y) => check(minus, x, y),
                ["*"] = (x, y) => check(multiply, x, y),

                ["/"] = (x, y) => check(divide, x, y),
                [":"] = (x, y) => check(divide, x, y),

                ["mod"] = (x, y) => check(mod, x, y),
                ["%"] = (x, y) => check(mod, x, y),

                ["gcd"] = (x, y) => check(gcd, x, y),

                ["^"] = (x, y) => check(pow, x, y),
                /*
                ["+"] = (x, y) => checked(plus(x,y)),
                ["-"] = (x, y) => checked(minus(x, y)),
                ["*"] = (x, y) => checked(multiply( x, y)),

                ["/"] = (x, y) => checked(divide( x, y)),
                [":"] = (x, y) => checked(divide( x, y)),

                ["mod"] = (x, y) => checked(mod( x, y)),
                ["%"] = (x, y) => checked(mod( x, y)),

                ["gcd"] = (x, y) => checked(gcd( x, y)),

                ["^"] = (x, y) => checked(pow( x, y)),
                */
            };

        }

        /// <summary>
        /// arguments - коллекция для разбиения на части выражения, operandA - первая часть выражения (число А), 
        /// operatorExpr - вторая часть выражения (операция), operandB -  третья часть выражения (число В)
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static double Calculate(string expr)
        {
            /*
            string[] arguments = expr.Split(' ');

            if (!double.TryParse(arguments[0], out double operandA))
            {
                throw new Exception();
            }

            if (!double.TryParse(arguments[2], out double operandB))
            {
                throw new Exception();
            }

            //char[] someShit = arguments[1].ToCharArray();
            string operatorExpr = arguments[1];           

            double answer = Double.NaN;

            if (operandB == 0 && operatorExpr == "/")
            {
                ErrorNotification(new StringBuilder("bruh").ToString());
                return answer;
            }
            if (!(operations.Keys.Contains($"{operatorExpr}")))
            {
                ErrorNotification(new StringBuilder("неверный оператор").ToString());
                return answer;
            }            
            if (Double.IsNaN(operations[operatorExpr](operandA, operandB)))
            {
                ErrorNotification(new StringBuilder("не число").ToString());
                return answer;
            }
            
            //if ( Double.IsNaN(answer) )
            //    throw new Exception("не число");
            return operations[operatorExpr](operandA, operandB);
            */
            /*
            try
            {
                string[] arguments = expr.Split(' ');
                double operandA = double.Parse(arguments[0]);
                double operandB = double.Parse(arguments[2]);
                string operatorExpr = arguments[1];
                if (operandB == 0 && operatorExpr == "/")
                {                    
                    throw new DivideByZeroException("bruh");
                }
                if (!(operations.Keys.Contains($"{operatorExpr}")))
                {
                    throw new KeyNotFoundException("неверный оператор");
                }
                return operations[operatorExpr](operandA, operandB);

            }
            catch (OverflowException)
            {
                //ErrorNotification(Double.PositiveInfinity.ToString());
                throw;
            }
            catch(Exception ex)
            {
                ErrorNotification(ex.Message);
                throw;
            }
            */

            string[] arguments = expr.Split(' ');

            if (!double.TryParse(arguments[0], out double operandA) || !double.TryParse(arguments[2], out double operandB))
            {
                return Double.PositiveInfinity;
            }

            string operatorExpr = arguments[1];

            /*
            if (operandB == 0 && operatorExpr == "/")
            {
                ErrorNotification("bruh");
                throw new Exception();
            }
               
            if (!(operations.Keys.Contains($"{operatorExpr}")))
            {
                ErrorNotification("неверный оператор");
                throw new Exception();
            }
            */
            try
            {
                if (operandB == 0.0 && operatorExpr == "/")
                {
                    throw new Exception(new StringBuilder("bruh").ToString());
                }

                if (!(operations.Keys.Contains($"{operatorExpr}")))
                {                    
                    throw new Exception(new StringBuilder("неверный оператор").ToString());
                }

                if (Double.IsNaN(operations[operatorExpr](operandA, operandB)))
                {
                    //throw new Exception(new StringBuilder("не число").ToString()); Если выбрасывать это исключение, то в файл записывается верно,
                    // но при сравнение одиновых строк он даёт false, что они одинаковые. Проверка с помощью ==, Equals, Contains и т.д. не помогает
                    // поэтому в консоль не будет выводиться "не число". А разбивать строку на чары и сравнивать. Я думаю, это говнокод (мб я ошибаюсь)
                }
            }
            catch(Exception ex)
            {
                ErrorNotification(ex.Message);
                throw new Exception(ex.Message);
            }
              
            return operations[operatorExpr](operandA, operandB);


        }


        public static void Handle(ErrorNotificationType e)
        {
            ErrorNotification += e;
        }

    }
}
