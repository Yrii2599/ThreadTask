using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using EquationSolver;
using System.Diagnostics;
using System.Threading;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ThreadSquareEquation
{
    class FileWrite
    {
        private readonly List<string> explanations;
        string path = @"C:\Users\Юра\source\repos\ThreadTask\FileForRead.txt";
        private readonly int _length;


        public FileWrite(int length)
        {
            _length = length;
            explanations = new List<string>();

            Write();

        }

        public void Write()
        {

            Random random = new Random();
            Equation equation = new Equation();
            Equation eq = new Equation();
            using StreamWriter sw =
                new StreamWriter(path: path, append: false, encoding: System.Text.Encoding.Default);
            for (int i = 0; i < _length; i++)
            {
                double A = random.Next(minValue: 1, maxValue: 5);
                double B = random.Next(minValue: -1, maxValue: 12);
                double C = random.Next(minValue: -12, maxValue: 12);

                sw.WriteLine($"{A} {B} {C}");
            }

        }


        public async Task Read()
        {
            List<Task> listTasks = new List<Task>();
            using StreamReader sr = new StreamReader(path: path);
            Task lastTask = null;
            for (int i = 0; i < _length; i++)
            {
                var newTask = new Task(() =>
                {
                    var res = TryRead(sr);

                });
                listTasks.Add(newTask);
                if (lastTask == null)
                {
                    lastTask = newTask;
                    lastTask.Start();
                }
                else
                {
                    await lastTask.ContinueWith(u => newTask.Start());
                }
            }

            Task.WaitAll(listTasks.ToArray());


        }

        public async Task SolveAndWrite()
        {

            await Read();
            List<Task> listTasks = new List<Task>();
            string pathResult = @"C:\Users\Юра\source\repos\ThreadTask\result.txt";
            using StreamWriter sw =
                new StreamWriter(path: pathResult, append: false, encoding: System.Text.Encoding.Default);

            foreach (var item in explanations)
            {
                // listTasks.Add(Task.Run(() => { TryWrite(sw,item); }));
                await TryWrite(sw, item);
            }

            // Task.WaitAll(listTasks.ToArray());








            //stopWatch.Start();
            //EqSolver solver = new EqSolver();
            //string pathResult = @"C:\Users\Юра\source\repos\ThreadSquareEquation\result.txt";
            //using StreamWriter sw = new StreamWriter(pathResult, false, System.Text.Encoding.Default);
            //foreach (var eq in equationArray)
            //{
            //    var result = solver.ResolveEquation(eq);
            //    sw.WriteLine(result.Result.Explanation);
            //}
            //stopWatch.Stop();
            //Console.WriteLine($" seconds {stopWatch.Elapsed.Seconds}  ms {stopWatch.Elapsed.Milliseconds}");


        }

        private async Task<string> TryRead(StreamReader sr, int number = 0)
        {
            List<Task> listTasks = new List<Task>();
            if (number > 20)
            {
                return null;
            }

            try
            {
                var str = sr.ReadLineAsync();
                listTasks.Add(str);
                if (str.Result == null)
                {
                    return str.Result;
                }

                listTasks.Add(Task.Run(() =>
                {
                    var res = str.Result;
                    if (res != null)
                    {
                        var abc = res.Split();
                        if (double.TryParse(abc[0], out double a)
                            && double.TryParse(abc[1], out double b)
                            && double.TryParse(abc[2], out double c))
                        {
                            EqSolver solver = new EqSolver();
                            var equ = new Equation()
                            {
                                A = a,
                                B = b,
                                C = c
                            };
                            var result = solver.ResolveEquation(equ).Result.Explanation;
                            explanations.Add(result);
                            Console.WriteLine(result);
                        }
                    }
                }));
                Task.WaitAll(listTasks.ToArray());
                Thread.Sleep(1000);
                return str.Result;

            }
            catch
            {
                return await TryRead(sr, ++number);

            }

        }

        private async Task TryWrite(StreamWriter sw, string s, int number = 0)
        {
            if (number > 10)
            {
                return;
            }

            try
            {
                await sw.WriteLineAsync(s);
            }
            catch
            {
                await TryWrite(sw, s, ++number);

            }
        }

    }
}
