﻿using JewishCalendar;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ZmanimChart
{
    public enum DayOfWeekFormat
    {
        Full,
        JewishNum,
        Number,
        English,
        None
    } 

    static class Program
    {
        internal static List<Location> LocationsList;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if(Properties.Settings.Default.NeedsUpdate)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.NeedsUpdate = false;
                Properties.Settings.Default.Save();
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            LoadLocations();
            Application.Run(new Form1());
        }

        /// Loads the locations from the settings.
        /* Note: the xml format for each locations is:
            <L N="Ofakim" H="אופקים" I="Y">    <!--N = name of location, H = hebrew name (optional), I = is the location in Israel? [Y = yes] (optional)-->
                <T>2</T>    <!--Time zone: hour offset from UTC (AKA GMT) -->
                <E>170</E>    <!--Elevation in meters (optional)-->
                <LT>31.32</LT>    <!--Latitude-->
                <LN>-34.62</LN>   <!--Longitude-->
                <CL>30</CL>    <!--Candle-lighting: minutes before sunset (optional)-->
                <TZN>Israel Standard Time</TZN>    <!--Time zone name (optional)-->
            </L>     */
        /// </summary>
        public static void LoadLocations()
        {
            LocationsList = new List<Location>();
            using (var ms = new System.IO.StringReader(Properties.Resources.LocationsList))
            {
                var settings = new System.Xml.XmlReaderSettings() { IgnoreWhitespace = true };
                using (var xr = System.Xml.XmlReader.Create(ms, settings))
                {
                    while (xr.ReadToFollowing("L"))
                    {
                        string name = xr.GetAttribute("N").Trim();
                        string heb = xr.GetAttribute("H") ?? name;
                        bool inIsrael = xr.GetAttribute("I") == "Y";
                        int timeZone;
                        int elevation = 0;
                        double latitude;
                        double longitute;
                        int candleLighting;
                        string timeZoneName = null;

                        xr.ReadToDescendant("T");
                        timeZone = xr.ReadElementContentAsInt("T", "");
                        if (xr.Name == "E")
                        {
                            elevation = xr.ReadElementContentAsInt("E", "");
                        }

                        latitude = xr.ReadElementContentAsDouble("LT", "");
                        longitute = xr.ReadElementContentAsDouble("LN", "");

                        if (xr.Name == "CL")
                        {
                            candleLighting = xr.ReadElementContentAsInt("CL", "");
                        }
                        else
                        {
                            candleLighting = inIsrael ? 30 : 18;
                        }

                        if (xr.Name == "TZN")
                        {
                            timeZoneName = xr.ReadElementContentAsString("TZN", "");
                        }
                        else if (inIsrael)
                        {
                            timeZoneName = "Israel Standard Time";
                        }

                        LocationsList.Add(new Location(name, timeZone, latitude, longitute)
                        {
                            NameHebrew = heb,
                            Elevation = elevation,
                            IsInIsrael = inIsrael,
                            TimeZoneName = timeZoneName,
                            CandleLighting = candleLighting
                        });
                    }
                    xr.Close();
                }
                ms.Close();
            }
        }

        public static string[] ZmanTypesList = {
            "Alos Hashachar - 90 / עלות השחר - 90",
            "Alos Hashachar - 72 / עלות השחר - 72",            
            "Sunrise / הנץ החמה",
            "Sunrise - sea level / הנץ החמה - מישור",
            "Shma - MG\"A / סוזק\"ש מג\"א",
            "Shma - GR\"A / סוזק\"ש גר\"א",
            "Tefilla - MG\"A / סוז\"ת מג\"א",
            "Tefilla - GR\"A / סוז\"ת גר\"א",
            "Midday and Midnight / חצות היום והלילה",
            "Mincha Gedolah / מנחה גדולה",
            "Mincha Ketana / מנחה קטנה",
            "Plag HaMincha / פלג המנחה",
            "Sunset / שקיעת החמה",
            "Sunset - sea level / שקיעת החמה - מישור",
            "Night - 45 / צה\"כ - 45",
            "Night - Rabbeinu Tam / צה\"כ - ר\"ת",
            "Night - 72 Zmaniyos / צה\"כ - 72 זמניות",
            "Daf Yomi / דף היומי"
        };
    }
}
