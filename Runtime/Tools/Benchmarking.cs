using UnityEngine;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Debug = UnityEngine.Debug;

namespace Laio.Tools
{
    /// <summary>
    /// Allows you to run a benchmark on a given method seeing how many milliseconds it takes to complete.
    /// Has options for number if iterations, along with benchmark name and description.
    /// </summary>
    public static class Benchmarking
    {
        internal static float average = 0;
        internal static float accumulative = 0;
        internal static uint framesCounted = 0;

        /// <summary>
        /// Get current FPS (smoothed)
        /// </summary>
        public static float RawFPS
        {
            get
            {
                return (1.0f / Time.deltaTime);
            }
        }

        /// <summary>
        /// Get current FPS (smoothed)
        /// </summary>
        public static float FPS
        {
            get
            {
                return (1.0f / Time.smoothDeltaTime);
            }
        }

        /// <summary>
        /// Uses secondsToUpdate as smoothing, and will calculate the average FPS 
        /// in that time frame.
        /// </summary>
        /// <param name="secondsToUpate">How many seconds before it updates?</param>
        /// <param name="decimalPlaces">How many decimal places to use</param>
        /// <returns></returns>
        public static string GetFPS(float secondsToUpate, uint decimalPlaces = 0)
        {
            //If no value is set for this, just grab immedate delta time
            if (average == 0)
            {
                average = Time.smoothDeltaTime;
                accumulative = 0;
                framesCounted = 0;
            }

            if (accumulative > secondsToUpate)
            {
                average = accumulative / framesCounted;
                accumulative = 0;
                framesCounted = 0;
                return (1.0f / average).ToString("N" + decimalPlaces);
            }
            else
            {
                accumulative = accumulative + Time.smoothDeltaTime;
                framesCounted++;
                return (1.0f / average).ToString("N" + decimalPlaces);
            }
        }

        /// <summary>
        /// Run an action and calculate the time in MS it took to complete that action.
        /// </summary>
        /// <param name="action">Action to run</param>
        public static void RunAction(Action action)
        {
            long results = RunActionBenchmark(action);
            PrintActionResults(action, results);
        }

        /// <summary>
        /// Run an action and calculate the time in MS it took to complete that action.
        /// </summary>
        /// <param name="action">Action to run</param>
        /// <param name="name">Name of the action</param>
        public static void RunAction(Action action, string name)
        {
            long results = RunActionBenchmark(action);
            PrintActionResults(action, results, name);
        }

        /// <summary>
        /// Run an action and calculate the time in MS it took to complete that action.
        /// </summary>
        /// <param name="action">Action to run</param>
        /// <param name="name">Name of the action</param>
        /// <param name="description">Description of the action</param>
        public static void RunAction(Action action, string name, string description)
        {
            long results = RunActionBenchmark(action);
            PrintActionResults(action, results, name, description);
        }

        /// <summary>
        /// Run an action multiple times and calculate the time in MS it took to complete that action.
        /// </summary>
        /// <param name="action">Action to run</param>
        /// <param name="iterations">Iterations to run</param>
        public static void RunAction(Action action, int iterations)
        {
            long results = RunActionBenchmark(action, iterations);
            PrintActionResults(action, results, interations: iterations);
        }

        /// <summary>
        /// Run an action and calculate the time in MS it took to complete that action.
        /// </summary>
        /// <param name="action">Action to run</param>
        /// <param name="iterations">Iterations to run</param>
        /// <param name="name">Name of the action</param>
        public static void RunAction(Action action, int iterations, string name)
        {
            long results = RunActionBenchmark(action, iterations);
            PrintActionResults(action, results, name: name, interations: iterations);
        }

        /// <summary>
        /// Run an action and calculate the time in MS it took to complete that action.
        /// </summary>
        /// <param name="action">Action to run</param>
        /// <param name="iterations">Iterations to run</param>
        /// <param name="name">Name of the action</param>
        /// <param name="description">Description of the action</param>
        public static void RunAction(Action action, int iterations, string name, string description)
        {
            long results = RunActionBenchmark(action, iterations);
            PrintActionResults(action, results, name, description, iterations);
        }

        /// <summary>
        /// Prints an actions results.
        /// </summary>
        /// <param name="a">Action to print</param>
        /// <param name="elapsedMs">Time it took to complete an action</param>
        /// <param name="name">Name of the action</param>
        /// <param name="description">Description of the action</param>
        /// <param name="interations">Iterations ran</param>
        internal static void PrintActionResults(Action a, long elapsedMs, string name = "", string description = "", int interations = 0)
        {
            string header = (name.Trim().Equals("") == true ?
                /*Empty name*/  $" Took {elapsedMs}ms ({(elapsedMs / 1000.0).ToString("N3")}s) to run. \n[{a.Method.Name}:\n]" :
                /*Filled*/      $" {name} finished. Took {elapsedMs}ms ({(elapsedMs / 1000.0).ToString("N3")}s) to run. \n{description}\n[{a.Method.Name}:\n{a.Method.GetParameters()} ]");
            UnityEngine.Debug.Log($"[<color=blue>Benchmark</color>]{(interations != 0 ? (" (Iterations: " + interations + ")") : "")} {header}");
        }

        /// <summary>
        /// Runs an action and returns how long it took to run (in milliseconds)
        /// </summary>
        /// <param name="action">Action to run</param>
        /// <param name="iterations">Iterations to run</param>
        /// <returns>Time to complete in milliseconds</returns>
        private static long RunActionBenchmark(Action action, int iterations = 1)
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
