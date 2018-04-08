using JewishCalendar;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Configuration;
using System.Diagnostics;

namespace LuachProject
{
    internal static class Program
    {
        internal static Pen DayCellBorderPen = Pens.SteelBlue;
        internal static Brush DayHeadersBGBrush = Brushes.Lavender;
        internal static Color DayHeadersTextColor = Color.SteelBlue;
        internal static Color DayTextColor = Color.DarkBlue;
        internal static System.Globalization.CultureInfo HebrewCultureInfo = new System.Globalization.CultureInfo("he-il");
        internal static List<Location> LocationsList;
        internal static Color SecularDayColor = Color.DarkBlue;
        internal static Brush SelectedDayBackgroundBrush = new SolidBrush(Color.FromArgb(100, 100, 200, 255));
        internal static Brush ShabbosBrush = Brushes.LightSteelBlue;
        internal static Brush MinorYomtovBrush = Brushes.GhostWhite;
        internal static Brush MajorYomtovBrush = new SolidBrush(Color.FromArgb(75, Color.RoyalBlue));
        internal static Color ZmanimColor = Color.RoyalBlue;
        internal static TextFormatFlags TextFormatFlags =
            TextFormatFlags.HorizontalCenter |
            TextFormatFlags.VerticalCenter |
            TextFormatFlags.NoPrefix;

        static Program()
        {
            if (Properties.Settings.Default.UserOccasions == null)
            {
                Properties.Settings.Default.UserOccasions = new UserOccasionColection();
                Properties.Settings.Default.Save();
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            if (Properties.Settings.Default.NeedsUpdate)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.NeedsUpdate = false;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            LoadLocations();
            Form form = null;
            if (args.Length > 0)
            {
                switch (args[0])
                {
                    case "English":
                        form = new frmMonthlyEnglish();
                        break;

                    case "Hebrew":
                        form = new frmMonthlyHebrew();
                        break;

                    case "SecularEnglish":
                        form = new frmMonthlySecular();
                        break;

                    case "SecularHebrew":
                        form = new frmMonthlySecular { DisplayHebrew = true };
                        break;
                }
            }
            else if (!string.IsNullOrEmpty(Properties.Settings.Default.LastLanguage))
            {
                switch (Properties.Settings.Default.LastLanguage)
                {
                    case "English":
                        form = new frmMonthlyEnglish();
                        break;

                    case "Hebrew":
                        form = new frmMonthlyHebrew();
                        break;

                    case "SecularEnglish":
                        form = new frmMonthlySecular();
                        break;

                    case "SecularHebrew":
                        form = new frmMonthlySecular { DisplayHebrew = true };
                        break;
                }
            }

            JewishDate jd = new JewishDate();
            var ad = (jd.AbsoluteDate.ToString());
            Application.Run(form ?? new frmMonthlyEnglish());
        }

        /// <summary>
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

        public static bool WeAreHere(Location location) => location.TimeZoneInfo != null && TimeZoneInfo.Local.Id == location.TimeZoneInfo.Id;

        public static void SetDoubleBuffered(Control c)
        {
            //Taxes: Remote Desktop Connection and painting
            //http://blogs.msdn.com/oldnewthing/archive/2006/01/03/508694.aspx
            if (SystemInformation.TerminalServerSession)
                return;

            PropertyInfo aProp =
                  typeof(Control).GetProperty(
                        "DoubleBuffered",
                        BindingFlags.NonPublic |
                        BindingFlags.Instance);

            aProp.SetValue(c, true, null);
        }
    }
}