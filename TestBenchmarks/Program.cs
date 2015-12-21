using System;
using JewishCalendar;
using System.Linq;

namespace TestBenchmarks
{
    class Program
    {
        private static DateTime startDate = new DateTime(1584, 1, 1);
        private static DateTime endDate = new DateTime(2239, 1, 1);
        private static int runCount = 10;
        private static double[] jdmt = new double[runCount];
        private static double[] jdt = new double[runCount];
        private static long[] jdmm = new long[runCount];
        private static long[] jdm = new long[runCount];

        static void Main(string[] args)
        {
            for (int i = 0; i < runCount; i++)
            {
                DoJD(i);
                DoJDM(i);

                Console.WriteLine("Iteration {0} complete: JD Time: {1}, JD Mem: {2} JDM Time: {3}, JDM Mem: {4}",
                    i + 1, jdt[i], jdm[i], jdmt[i], jdmm[i]);
            }

            Console.WriteLine("Average Time for JD - {0} Seconds", jdt.Average());
            Console.WriteLine("Average Memory for JD - {0} Bytes", jdm.Average());
            Console.WriteLine("Average Time for JDM - {0} Seconds", jdmt.Average());
            Console.WriteLine("Average Memory for JDM - {0} Bytes", jdmm.Average());
            Console.WriteLine("<Press any key to exit>");
            Console.ReadLine();
        }

        private static void DoJD(int run)
        {
            DateTime start = DateTime.Now;

            DateTime calc = startDate;
            while (calc < endDate)
            {
                var jd = new JewishDate(calc);
                DoStuff(jd);
                jd = jd + 15;
                var other = jd - 15;
                var i = jd > new JewishDate(5776, 2, 1);
                calc = calc.AddDays(1);
            }
            
            jdt[run] = (DateTime.Now - start).TotalSeconds;
            jdm[run] = GC.GetTotalMemory(true);
            GC.Collect(10000, GCCollectionMode.Default, true);
        }

        private static void DoJDM(int run)
        {
            DateTime calc = startDate;
            DateTime start = DateTime.Now;
            while (calc < endDate)
            {
                var jd = new JewishDateMicro(calc);
                DoStuff(jd);
                jd = jd + 15;
                var other = jd - 15;
                var i = jd > new JewishDateMicro(5776, 2, 1);

                calc = calc.AddDays(1);
            }            
            jdmt[run] = (DateTime.Now - start).TotalSeconds;
            jdmm[run] = GC.GetTotalMemory(true);
            GC.Collect(10000, GCCollectionMode.Default, true);
        }

        private static void DoStuff(IJewishDate jd)
        {
            var ds = jd.ToShortDateString();
            var dsh = jd.ToLongDateStringHeb();
            DateTime dt = jd.GregorianDate;
            var dow = jd.DayOfWeek;
            var d = jd.Day;
            var m = jd.Month;
            var y = jd.Year;
            var s = jd.GetDayOfOmer();
            var w = jd.AbsoluteDate;
        }
    }
}
