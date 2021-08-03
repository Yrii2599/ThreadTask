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
        private  List<string> _explanations;
        private readonly List<Equation> _equations = new List<Equation>();
        private List<string> _inputList = new List<string>();
        string path = @"C:\Users\Юра\source\repos\ThreadTask\FileForRead.txt";
        private readonly int _length;


        public FileWrite(int length)
        {
            _length = length;
            _explanations = new List<string>();

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
        public void SolveAndWrite()
        {
            TryRead();

            List<Task<EqSolution>> listTasks = new List<Task<EqSolution>>();
            EqSolver solver = new EqSolver();
            string pathResult = @"C:\Users\Юра\source\repos\ThreadTask\result.txt";
            _equations.ForEach(item=>listTasks.Add(Task.Factory.StartNew(()=>solver.ResolveEquation(item).Result)));
            Task.WaitAll(listTasks.ToArray());
            _explanations=listTasks.Select(e=>e.Result.Explanation).ToList();
        
                File.WriteAllLines(pathResult,_explanations);
                

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

        private void TryRead()
        {


            
                var str = File.ReadAllLines(path);
                _inputList = str.ToList();
                foreach (var item in _inputList)
                {
                    Equation equation = new Equation();
                    string[] abc = new string[2];
                    abc = item.Split();
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
                        _equations.Add(equ);
                    }
                }
        }

        

    }
}
