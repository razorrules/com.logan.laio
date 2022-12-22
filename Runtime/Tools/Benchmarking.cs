using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laio
{
    /// <summary>
    /// Allows you to run a benchmark on a given method seeing how many milliseconds it takes to complete.
    /// Has options for number if iterations, along with benchmark name and description.
    /// </summary>
    public static class Benchmarking
    {
        public static void Run(Action action)
        {
            long results = RunBenchmark(action);
            PrintResults(action, results);
        }

        public static void Run(Action action, string name)
        {
            long results = RunBenchmark(action);
            PrintResults(action, results, name);
        }

        public static void Run(Action action, string name, string description)
        {
            long results = RunBenchmark(action);
            PrintResults(action, results, name, description);
        }

        public static void Run(Action action, int iterations)
        {
            long results = RunBenchmark(action, iterations);
            PrintResults(action, results, interations: iterations);
        }

        public static void Run(Action action, int iterations, string name)
        {
            long results = RunBenchmark(action, iterations);
            PrintResults(action, results, name: name, interations: iterations);
        }

        public static void Run(Action action, int iterations, string name, string description)
        {
            long results = RunBenchmark(action, iterations);
            PrintResults(action, results, name, description, iterations);
        }

        private static void PrintResults(Action a, long elapsedMs, string name = "", string description = "", int interations = 0)
        {
            string header = (name.Trim().Equals("") == true ?
                /*Empty name*/  $" Took {elapsedMs}ms ({(elapsedMs / 1000.0).ToString("N3")}s) to run. \n[{a.Method.Name}:\n]" :
                /*Filled*/      $" {name} finished. Took {elapsedMs}ms ({(elapsedMs / 1000.0).ToString("N3")}s) to run. \n{description}\n[{a.Method.Name}:\n{a.Method.GetParameters()} ]");
            Debug.Log($"[<color=blue>Benchmark</color>]{(interations != 0 ? (" (Iterations: " + interations + ")") : "")} {header}");
        }

        private static long RunBenchmark(Action action, int iterations = 0)
        {

            var watch = System.Diagnostics.Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                action.Invoke();
            }
            watch.Stop();
            return watch.ElapsedMilliseconds;
        }

    }

}
