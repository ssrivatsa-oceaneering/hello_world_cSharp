using System;

namespace CSVConsoleWriter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var exec = new ThreadExecutor(0.5);
            exec.RunExecutor();
        }
    }
}
