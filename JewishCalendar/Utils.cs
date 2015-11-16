using System;
using System.Collections;
using System.Text;

namespace JewishCalendar
{
    /// <summary>
    /// Contains general useful utils and pitchifkes
    /// </summary>
    public static class Utils
    {
        private static char[] _sings = new char[] { '�', '�', '�', '�', '�', '�', '�', '�', '�' };
        private static char[] _tens = new char[] { '�', '�', '�', '�', '�', '�', '�', '�', '�' };
        private static char[] _hundreds = new char[] { '�', '�', '�', '�' };
        private static string[] _hebsingles = { "", "���", "����", "����", "�����", "����", "���", "����", "�����", "����" };
        private static string[] _hebTens = { "", "���", "�����", "������", "������" };

        /// <summary>
        /// Array of name of the Jewish Months. Month numbers correspond to the array index, so  Nissan is JewishMonthNamesEnglish[1] etc.
        /// </summary>
        public static string[] JewishMonthNamesEnglish = { "", "Nissan", "Iyar", "Sivan", "Tamuz", "Av", "Ellul", "Tishrei", "Cheshvan", "Kislev", "Teves", "Shvat", "Adar", "Adar Sheini" };
        /// <summary>
        /// Array of Hebrew names of the Jewish Months. Month numbers correspond to the array index, so  ���� is JewishMonthNamesHebrew[1] etc.
        /// </summary>
        public static string[] JewishMonthNamesHebrew = { "", "����", "����", "����", "����", "��", "����", "����", "����", "����", "���", "���", "���", "��� ���" };
        /// <summary>
        /// Names of days of week in Hebrew. ��� ����� is JewishDOWNames[0].
        /// </summary>
        public static string[] JewishDOWNames = { "��� �����", "��� ���", "��� �����", "��� �����", "��� �����", "��� ��� ����", "��� ����" };
        /// <summary>
        /// The Jewish names for the days of the week in English as an array. For example, DaysOfWeek[5] is Erev Shabbos
        /// </summary>
        public static string[] DaysOfWeek = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Erev Shabbos", "Shabbos Kodesh" };

        /// <summary>
        /// Determine if this object is contained in a list of objects
        /// </summary>
        /// <param name="obj">The object to test</param>
        /// <param name="list">Params list of objects to look for object</param>
        /// <returns></returns>
        public static bool In(this object obj, params Object[] list)
        {
            return Array.IndexOf(list, obj) > -1;
        }

        /// <summary>
        /// Add two character suffix to number. e.g. 21st, 102nd, 93rd, 500th
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string ToSuffixedString(this int num)
        {
            string t = num.ToString();
            string suffix = "th";
            if (t.Length == 1 || (t[t.Length - 2] != '1'))
            {
                switch (t[t.Length - 1])
                {
                    case '1':
                        suffix = "st";
                        break;
                    case '2':
                        suffix = "nd";
                        break;
                    case '3':
                        suffix = "rd";
                        break;
                    default:
                        suffix = "th";
                        break;
                }
            }
            return t + suffix;
        }

        /// <summary>
        /// Determines if the given Gregorian date and time is within the rules for DST.
        /// If no time zone info is available, the US rules are used.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public static bool IsDateTimeDST(DateTime date, Location location)
        {
            if (location != null && location.TimeZoneInfo != null)
            {
                return location.TimeZoneInfo.IsDaylightSavingTime(date);
            }

            int year = date.Year,
                month = date.Month,
                day = date.Year,
                hour = date.Hour;

            if (month < 3 || month == 12)
            {
                return false;
            }
            else if (month > 3 && month < 11)
            {
                return true;
            }
            //DST starts at 2:00 AM on the second Sunday in March                
            else if (month == 3)
            {
                //Gets day of week on March 1st
                int firstDOW = getDOW(year, 3, 1);
                //Gets date of second Sunday
                int targetDate = firstDOW == 0 ? 8 : ((7 - (firstDOW + 7) % 7)) + 8;

                return (day > targetDate || (day == targetDate && hour >= 2));
            }
            //DST ends at 2:00 AM on the first Sunday in November
            else //dt.Month == 11
            {
                //Gets day of week on November 1st
                int firstDOW = getDOW(year, 11, 1);
                //Gets date of first Sunday
                int targetDate = firstDOW == 0 ? 1 : ((7 - (firstDOW + 7) % 7)) + 1;

                return (day < targetDate || (day == targetDate && hour < 2));
            }
        }

        /// <summary>
        /// Get day of week using Zellers algorithm.
        /// </summary>
        /// <remarks>Day zero is Sunday</remarks>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        private static int getDOW(int year, int month, int day)
        {
            int adjustment = (14 - month) / 12,
                mm = month + 12 * adjustment - 2,
                yy = year - adjustment;
            return (day + (13 * mm - 1) / 5 + yy + yy / 4 - yy / 100 + yy / 400) % 7;
        }

        /// <summary>
        /// Converts a number into its jewish number equivalent. I.E. 254 is ��"�
        /// NOTE: The exact thousands numbers (1000, 2000, 3000 etc.) 
        /// will look awfully similar to the single digits, but will be formatted with an apostrophe I.E. 2000 = "'�"        
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToNumberHeb(this int number)
        {
            if (number < 1)
            {
                throw new ArgumentOutOfRangeException("number", "Min value is 1");
            }

            if (number > 9999)
            {
                throw new ArgumentOutOfRangeException("number", "Max value is 9999");
            }

            int n = number;
            StringBuilder retval = new StringBuilder();

            if (n >= 1000)
            {
                retval.AppendFormat("{0}'", _sings[((n - (n % 1000)) / 1000) - 1]);
                n = n % 1000;
            }

            while (n >= 400)
            {
                retval.Append('�');
                n -= 400;
            }

            if (n >= 100)
            {
                retval.Append(_hundreds[((n - (n % 100)) / 100) - 1]);
                n = n % 100;
            }

            if (n == 15)
            {
                retval.Append("��");
            }
            else if (n == 16)
            {
                retval.Append("��");
            }
            else
            {
                if (n > 9)
                {
                    retval.Append(_tens[((n - (n % 10)) / 10) - 1]);
                }
                if ((n % 10) > 0)
                {
                    retval.Append(_sings[(n % 10) - 1]);
                }
            }
            if (number > 999 && (number % 1000 < 10))
            {
                retval.Insert(0, "'");
            }
            else if (retval.Length > 1)
            {
                retval = retval.Insert(retval.Length - 1, "\"");
            }
            return retval.ToString();
        }

        /// <summary>
        /// Returns the nusach for Sefiras Ha'omer for the given day and minhag
        /// </summary>
        /// <param name="dayOfOmer"></param>
        /// <param name="laOmer"></param>
        /// <param name="sfardi"></param>
        /// <returns></returns>
        public static string GetOmerNusach(int dayOfOmer, bool laOmer, bool sfardi)
        {
            int weeks = Convert.ToInt32(dayOfOmer / 7),
                days = dayOfOmer % 7;
            string nusach = "���� ";

            if (dayOfOmer == 1)
            {
                nusach += "��� ��� ";
            }
            else
            {
                if (dayOfOmer == 2)
                {
                    nusach += "��� ";
                }
                else
                {
                    if (dayOfOmer == 10)
                    {
                        nusach += "���� ";
                    }
                    else
                    {
                        nusach += _hebsingles[(dayOfOmer % 10)] + " ";
                        if (dayOfOmer > 10)
                        {
                            if (dayOfOmer > 20 && ((dayOfOmer % 10) > 0))
                            {
                                nusach += "�";
                            }
                            nusach += _hebTens[dayOfOmer / 10] + " ";
                        }
                    }
                }
                nusach += (dayOfOmer >= 11 ? "���" : "����") + " ";

                if (sfardi)
                    nusach += "�����" + " ";

                if (dayOfOmer >= 7)
                {
                    nusach += "��� ";
                    if (weeks == 1)
                    {
                        nusach += "���� ��� ";
                    }
                    else if (weeks == 2)
                    {
                        nusach += "��� ������ ";
                    }
                    else if (weeks > 0)
                    {
                        nusach += _hebsingles[weeks] + " ������ ";
                    }
                    if (days == 1)
                    {
                        nusach += "���� ��� ";
                    }
                    else if (days == 2)
                    {
                        nusach += "���� ���� ";
                    }
                    else if (days > 0)
                    {
                        nusach += "�" + _hebsingles[days] + " ���� ";
                    }
                }
            }
            if (!sfardi)
            {
                nusach += (laOmer ? "�" : "�") + "����";
            }
            return nusach;
        } 
    }
}