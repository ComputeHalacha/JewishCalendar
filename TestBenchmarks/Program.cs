using System;
using JewishCalendar;
using System.Linq;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace TestBenchmarks
{
    class Program
    {
        private static DateTime startDate = new DateTime(1915, 1, 1);
        private static DateTime endDate = new DateTime(2015, 1, 10);
        
        static void Main(string[] args)
        {
            Benchmark("JewishDateMicro", 50, 50, new Action(DoJDM));
            Benchmark("JewishDate", 50, 50, new Action(DoJD));            
            Console.WriteLine("<Press any key to exit>");
            Console.ReadLine();
        }

        static double Benchmark(string name, int runCount, int subRunCount, Action action)
        {
            Console.WriteLine("{0}: warming up...", name);

            // warm up.
            action();

            Console.WriteLine("{0}: finding ballpark speed...", name);

            // find an average amount of calls it fill up two seconds.

            Stopwatch sw = Stopwatch.StartNew();

            int count = 0;
            do
            {
                ++count;
                action();
            }
            while (sw.ElapsedTicks < (Stopwatch.Frequency * 2));

            sw.Stop();

            Console.WriteLine("{0}: ballpark speed is {1} runs/sec", name, MulMulDiv(count, subRunCount, Stopwatch.Frequency, sw.ElapsedTicks));

            // The benchmark will run the Action in a loop 'count' times.

            count = Math.Max(count / 2, 1);

            // Start the benchmark.

            Console.Write("{0}: benchmarking", name);
            Console.Out.Flush();

            long minticks = long.MaxValue;
            int runs = 0;

            while (runs < runCount)
            {
                sw.Restart();

                for (int i = 0; i < count; ++i)
                {
                    action();
                }

                sw.Stop();

                long ticks = sw.ElapsedTicks;

                if (ticks < minticks)
                {
                    // Found a new smallest execution time. Reset.

                    minticks = ticks;
                    runs = 0;

                    Console.Write('+');
                    Console.Out.Flush();
                    continue;
                }
                else
                {
                    ++runs;
                    Console.Write('.');
                    Console.Out.Flush();
                }
            }

            Console.WriteLine("done");
            Console.WriteLine("{0}: speed is {1} runs/sec", name, MulMulDiv(count, subRunCount, Stopwatch.Frequency, minticks));

            return (double)count * subRunCount * Stopwatch.Frequency / minticks;
        }

        static long MulMulDiv(long count, long subRunCount, long freq, long ticks)
        {
            return count * subRunCount * freq / ticks;
        }

        private static void DoJD()
        {
            DateTime calc = startDate;
            while (calc < endDate)
            {                
                var jd = new JewishDate(calc);
                var a = jd.ToShortDateString();
                var b = jd.ToLongDateStringHeb();
                var c = jd.GregorianDate;
                var d = jd.DayOfWeek;
                var e = jd.Day;
                var f = jd.Month;
                var g = jd.Year;
                var h = jd.GetDayOfOmer();
                var i = jd.AbsoluteDate;
                var j = jd + 1500;
                var k = jd - 2000;
                var l = new JewishDate(5776, 2, 1);
                var m = j > l;
                var n = l < k;
                
                calc = calc.AddDays(1);
            }           
        }

        private static void DoJDM()
        {
            DateTime calc = startDate;
            while (calc < endDate)
            {
                var jd = new JewishDateMicro(calc);
                var a = jd.ToShortDateString();
                var b = jd.ToLongDateStringHeb();
                var c = jd.GregorianDate;
                var d = jd.DayOfWeek;
                var e = jd.Day;
                var f = jd.Month;
                var g = jd.Year;
                var h = jd.GetDayOfOmer();
                var i = jd.AbsoluteDate;
                var j = jd + 1500;
                var k = jd - 2000;
                var l = new JewishDateMicro(5776, 2, 1);
                var m = j > l;
                var n = l < k;

                calc = calc.AddDays(1);
            }            
        }        
    }
}
