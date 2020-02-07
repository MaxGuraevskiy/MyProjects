/*
 * Гураевский Максим Дмитриевич
 * БПИ1910 - 1
 * ДЗ№2 08.02.2020
 * <3 Шадрину
 */
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Media;

namespace HomeWork08._02._20
{
    class Program
    {
        static SoundPlayer simpleSound = new SoundPlayer("../../../bruh.wav");
        
        // Это работает, это само совершенство



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
            strAnswer = new StreamWriter("../../../answer.txt", false, Encoding.UTF8, 300000);
            strCheck = new StreamWriter("../../../result.txt", false, Encoding.UTF8, 300000);
            // Подпись на ивент метода ConsoleErrorHandler
            Calculator.Handle(ConsoleErrorHandler);
            Calculator.Handle(ResultErrorHandler);
            // Повтор решения
            do
            {
                
                string[] operatorExprs = File.ReadAllLines("../../../expressions.txt", Encoding.UTF8);
                string[] checker = File.ReadAllLines("../../../expressions_checker.txt", Encoding.UTF8);
                int counterError = 0;
                //string someResult = "";
                int i = 0;
                int ia = 0;
                // Цикл для записи в файлы ответов
                for (; i < operatorExprs.Length; i++)
                {
                    
                    /*
                    double result;
                    try
                    {
                        result = Calculator.Calculate(operatorExprs[i]);
                    }
                    catch (Exception ex)
                    {
                        result = Double.PositiveInfinity;
                    }
                    //someResult = String.Format("{0:f3}", result);
                    someResult = $"{Math.Round(result, 3, MidpointRounding.ToEven):f3}";

                    if (someResult != "не число" && result != Double.NaN)
                    {

                        strAnswer.WriteLine(someResult);
                        
                    }
                    if (someResult == checker[i] && result != Double.NaN)
                    {
                        strCheck.WriteLine("OK");
                    }
                    else
                    {
                        strCheck.WriteLine("Error");
                        counterError++;
                    }
                    */

                    /*
                    double result = Double.NaN;
                    string someResult = "";
                    try
                    {
                        result = Calculator.Calculate(operatorExprs[i]);
                        someResult = $"{Math.Round(result, 3, MidpointRounding.ToEven):f3}";
                        strAnswer.WriteLine(someResult);
                    }
                    catch (OverflowException)
                    {
                        strAnswer.WriteLine(Double.PositiveInfinity);
                    }
                    catch(Exception ex)
                    {
                        //strAnswer.WriteLine(ex.Message);
                    }

                    if (someResult == checker[i] )
                    {
                        strCheck.WriteLine("OK");
                    }
                    else
                    {
                        strCheck.WriteLine("Error");
                        counterError++;
                    }
                    */
                    string someResult = string.Empty;
                    try
                    {
                        double result = Calculator.Calculate(operatorExprs[i]);
                        someResult = $"{Math.Round(result, 3, MidpointRounding.ToEven):f3}";
                    }
                    catch (Exception ex)
                    {
                        someResult = ex.Message;
                        if (ex.Message == "bruh") // Простите, я не мог удержаться
                            simpleSound.Play(); // Лучше закомментите эту строчку
                    }

                    //if(ia > 9000) Проблема в том, что в файл почему-то не записывается определённого количество строк. 
                    //   Thread.Sleep(3); Это происходит даже на разных пека Я ПОНЯЛ ЭТО БУФЕР ВИНОВАТ ОМГ!!!                        

                    if (double.TryParse(someResult, out double someShit))
                    {

                        strAnswer.WriteLine(someResult);
                        strAnswer.Flush();
                    }

                    if (new StringBuilder(someResult).ToString().Contains(new StringBuilder(checker[i]).ToString()))
                    {

                        strCheck.WriteLine("OK");
                        strCheck.Flush();
                        ia++;
                    }
                    else
                    {
                        strCheck.Flush();
                        strCheck.WriteLine("Error");
                        counterError++;
                        ia++;
                    }

                }
                Console.WriteLine(i);
                Console.WriteLine(ia);

                strCheck.WriteLine("-----------------------");
                strCheck.WriteLine(counterError);
                strCheck.Flush();
                Console.WriteLine(counterError);

                Console.WriteLine("Для выхода нажмите Esc");
            }
            while (Console.ReadKey(true).KeyChar != (char)ConsoleKey.Escape);

            // Goodbye ma lover, goodbye my friend
            Console.WriteLine("GoodBye!");
            Thread.Sleep(1000);

            // Закрытие файлов
            strAnswer.Flush();
            strAnswer.Close();
            strCheck.Flush();
            strCheck.Close();

        }


        public static void ConsoleErrorHandler(string message)
        {
            Console.WriteLine($"{message} {DateTime.Now}");
        }

        public static void ResultErrorHandler(string message)
        {            
            strAnswer.Write($"{message}\r\n");
            strAnswer.Flush();
        }
    }
}
