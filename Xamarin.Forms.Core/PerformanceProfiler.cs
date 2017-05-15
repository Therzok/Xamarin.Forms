﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Xamarin.Forms
{
    public class PerformanceProfiler
    {
        static readonly Dictionary<string, PerformanceData> Statistics = 
            new Dictionary<string, PerformanceData>();

        public static void Start([CallerMemberName] string member = null)
        {
            string id = member;

            PerformanceData stats;
            if (!Statistics.TryGetValue(id, out stats))
                Statistics[id] = stats = new PerformanceData();

            stats.CallCount++;
            stats.StartTimes.Push(Stopwatch.GetTimestamp());
        }

        public static void Stop([CallerMemberName] string member = null)
        {
            string id = member;
            long stop = Stopwatch.GetTimestamp();

            PerformanceData stats = Statistics[id];
            long start = stats.StartTimes.Pop();
            if (!stats.StartTimes.Any())
                stats.TotalTime += stop - start;
        }

        public static string GetStats()
        {
            var b = new StringBuilder();
            b.AppendLine();
            foreach (KeyValuePair<string, PerformanceData> kvp in 
                Statistics.OrderBy(kvp => kvp.Key))
            {
                string id = kvp.Key;
                int callCount = kvp.Value.CallCount;
                double time = TimeSpan.FromTicks(kvp.Value.TotalTime).TotalMilliseconds;
                b.Append(string.Format("Call ID: {0}, Call Count: {1}, Time: {2} ms", 
                    id, 
                    callCount,
                    time));
                b.AppendLine();
            }
            return b.ToString();
        }
    }

    internal class PerformanceData
    {
        public readonly Stack<long> StartTimes = new Stack<long>();
        public int CallCount;
        public long TotalTime;
    }
}