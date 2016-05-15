using System;

namespace JewishCalendar
{
    public static class PirkeiAvos
    {
        public static int[] GetPirkeiAvos(IJewishDate jDate, bool inIsrael)
        {
            int jYear = jDate.Year,
                jMonth = jDate.Month,
                jDay = jDate.Day;

            //Pirkei Avos is from after Pesach until Rosh Hashana
            if ((jMonth == 1 && jDay > (inIsrael ? 21 : 22)) ||
                //All Shabbosim through Iyar, Sivan, Tamuz, Av - besides for the day/s of Shavuos and Tisha B'Av
                ((jMonth > 1 && jMonth < 6 &&
                    (!((jMonth == 3 && jDay == 6) || (!inIsrael && jMonth == 3 && jDay == 7))) &&
                    (!(jMonth == 5 && jDay == 9)))))
            {
                return new int[] { GetPerek(jDate, inIsrael) };
            }
            //Ellul can have multiple prakim
            else if (jMonth == 6)
            {
                return GetEllulPrakim(jDate, inIsrael);
            }
            //No Pirkei Avos
            else
            {
                return new int[] { };
            }
        }

        private static int[] GetEllulPrakim(IJewishDate jDate, bool inIsrael)
        {
            int[] prakim = null;
            int jYear = jDate.Year,
                jMonth = jDate.Month,
                jDay = jDate.Day;
            //The year/month/day/absoluteDay constructor for JewishDateMicro is used for efficiency.
            JewishDateMicro day1 = new JewishDateMicro(jYear, 6, 1, jDate.AbsoluteDate - jDate.Day + 1);
            int day1DOW = day1.DayInWeek;
            int shabbos1Day = day1DOW == 6 ? 1 : ((6 - (day1DOW + 6) % 6) + 1);
            JewishDateMicro shabbos1Date = new JewishDateMicro(jYear, 6, shabbos1Day, day1.AbsoluteDate + shabbos1Day - 1);
            //Which shabbos in Ellul are we working out now?
            int currentShabbosNumber = jDay == shabbos1Day ? 1 : ((jDay - shabbos1Day) / 7) + 1;
            
            switch (GetPerek(shabbos1Date, inIsrael))
            {
                case 1:
                    switch (currentShabbosNumber)
                    {
                        case 1:
                            prakim = new int[] { 1 };
                            break;
                        case 2:
                            prakim = new int[] { 2 };
                            break;
                        case 3:
                            prakim = new int[] { 3, 4 };
                            break;
                        case 4:
                            prakim = new int[] { 5, 6 };
                            break;
                    }
                    break;
                case 2:
                    switch (currentShabbosNumber)
                    {
                        case 1:
                            prakim = new int[] { 2 };
                            break;
                        case 2:
                            prakim = new int[] { 3 };
                            break;
                        case 3:
                            prakim = new int[] { 4 };
                            break;
                        case 4:
                            prakim = new int[] { 5, 6 };
                            break;
                    }
                    break;
                case 3:
                    switch (currentShabbosNumber)
                    {
                        case 1:
                            prakim = new int[] { 3 };
                            break;
                        case 2:
                            prakim = new int[] { 4 };
                            break;
                        case 3:
                            prakim = new int[] { 5 };
                            break;
                        case 4:
                            prakim = new int[] { 6 };
                            break;
                    }
                    break;
                case 4:
                    //This can only happen in Chutz La'aretz
                    switch (currentShabbosNumber)
                    {
                        case 1:
                            prakim = new int[] { 4, 5 };
                            break;
                        case 2:
                            prakim = new int[] { 6, 1 };
                            break;
                        case 3:
                            prakim = new int[] { 2, 3 };
                            break;
                        case 4:
                            prakim = new int[] { 4, 5, 6 };
                            break;
                    }
                    break;
                case 5:
                    switch (currentShabbosNumber)
                    {
                        case 1:
                            prakim = new int[] { 5, 6 };
                            break;
                        case 2:
                            prakim = new int[] { 1, 2 };
                            break;
                        case 3:
                            prakim = new int[] { 3, 4 };
                            break;
                        case 4:
                            prakim = new int[] { 5, 6 };
                            break;
                    }
                    break;
                case 6:
                    switch (currentShabbosNumber)
                    {
                        case 1:
                            prakim = new int[] { 6 };
                            break;
                        case 2:
                            prakim = new int[] { 1, 2 };
                            break;
                        case 3:
                            prakim = new int[] { 3, 4 };
                            break;
                        case 4:
                            prakim = new int[] { 5, 6 };
                            break;
                    }
                    break;
            }

            return prakim;
        }

        private static int GetPerek(IJewishDate jDate, bool inIsrael)
        {
            int jYear = jDate.Year,
               jMonth = jDate.Month,
               jDay = jDate.Day;

            //Get the week number of since Pesach
            var perekAvos = ((jDate.AbsoluteDate - (new JewishDateMicro(jYear, 1, (inIsrael ? 22 : 23))).AbsoluteDate) % 6) + 1;
            //If Shavuos was on Shabbos, we miss a week
            if ((jMonth > 3 || (jMonth == 3 && jDay > 6)) && new JewishDateMicro(jYear, 3, 6).DayOfWeek == DayOfWeek.Saturday)
            {
                perekAvos--;
                if (perekAvos == 0)
                {
                    perekAvos = 6;
                }
            }
            //If Tisha B'Av was on Shabbos, we miss a week
            if ((jMonth > 5 || (jMonth == 5 && jDay > 9)) && new JewishDateMicro(jYear, 5, 9).DayOfWeek == DayOfWeek.Saturday)
            {
                perekAvos--;
                if (perekAvos == 0)
                {
                    perekAvos = 6;
                }
            }

            return perekAvos;
        }
    }
}
