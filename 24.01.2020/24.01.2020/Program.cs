/*
 * БПИ1910 - 1
 * Гураевский Максим Дмитриевич
 * 25.01.2020 1:42
 * HomeTask for Seminar 3-2 delegate calculator
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace HW25_01_2020
{
    class Calculator
    {
        public delegate double MathOperation(double a, double b);

        static Dictionary<string, MathOperation> operations;

        // Конструктор Calculator. plus - операция сложения, devide - операция деления, mod - операция остатка от деления.
        static Calculator()
        {
            MathOperation plus = (x, y) => x + y;
            MathOperation devide = (x, y) => x / y;
            MathOperation mod = (x, y) => x % y;
            operations = new Dictionary<string, MathOperation>
            {
                ["+"] = plus,
                ["-"] = (x, y) => x - y,
                ["*"] = (x, y) => x * y,

                ["/"] = devide,
                [":"] = devide,

                ["mod"] = mod,
                ["%"] = mod,

                ["gcd"] = (x, y) =>
                {
                    while (x != 0 && y != 0)
                    {
                        if (x > y)
                            x %= y;
                        else
                            y %= x;
                    }

                    return x == 0 ? y : x;
                },

                ["^"] = (x,y) => Math.Pow(x,y),
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
            string[] arguments = expr.Split(' ');

            double operandA = double.Parse(arguments[0]);
            string operatorExpr = arguments[1];
            double operandB = double.Parse(arguments[2]);

            if (operations.ContainsKey(operatorExpr))
            {
                return operations[operatorExpr](operandA, operandB);
            }
            else
            {
                throw new ArgumentException("Invalid operator");
            }
        }
    }

    class Program
    {
        // Поля для записи в файлы result.txt и answer.txt
        static StreamWriter strAnswer = null;
        static StreamWriter strCheck = null;

        /// <summary>
        /// strAnswer - поток для answer.txt, strCheck - поток для result.txt, operatorExprs - коллекция для чтения выражений, 
        /// checker - коллекция для чтения ответов из файлов, counter - счётчик неверных ответов, someResult - результат нового выражения
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // Поля с инициализацией потоков на запись в result.txt и answer.txt
            strAnswer = new StreamWriter("../../../answer.txt", false);
            strCheck = new StreamWriter("../../../result.txt", false);

            // Повтор решения
            do
            {
                
                string[] operatorExprs = File.ReadAllLines("../../../expressions.txt");
                string[] checker = File.ReadAllLines("../../../expressions_checker.txt");
                int counter = 0;
                string someResult = "";

                // Цикл для записи в файлы ответов
                for (int i = operatorExprs.GetLowerBound(0); i < operatorExprs.GetUpperBound(0) + 1; i++)
                {
                    someResult = String.Format("{0:f3}", Calculator.Calculate(operatorExprs[i]));
                    strAnswer.WriteLine(someResult);
                    if (someResult == checker[i])
                    {
                        strCheck.WriteLine("OK");
                    }
                    else
                    {
                        strCheck.WriteLine("Error");
                        counter++;
                    }
                }
                strCheck.WriteLine("-----------------------");
                strCheck.WriteLine(counter);

                Console.WriteLine("Для выхода нажмите Esc");
            }
            while (Console.ReadKey(true).KeyChar != (char)ConsoleKey.Escape);

            // Goodbye ma lover, goodbye my friend
            Console.WriteLine("GoodBye!");
            Thread.Sleep(1000);

            // Закрытие файлов
            strAnswer.Close();
            strCheck.Close();

        }
    }
}
