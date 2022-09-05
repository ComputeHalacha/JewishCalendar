using JewishCalendar;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32.TaskScheduler;

namespace LuachProject
{
    internal static class Program
    {
        internal static Pen DayCellBorderPen = Pens.SteelBlue;
        internal static Brush DayHeadersBGBrush = Brushes.Lavender;
        internal static Color DayHeadersTextColor = Color.SteelBlue;
        internal static Color DayTextColor = Color.DarkBlue;
        internal static System.Globalization.CultureInfo HebrewCultureInfo = new("he-il");
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
        internal static readonly string RemindersTaskName = "LuachSendEmailReminders";

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

            if (args != null && args.Contains("-uoc"))
            {
                int sentReminders = SendUserOccasionEmailReminders();
                Console.WriteLine($"Sent {sentReminders} email reminders");
                Application.Exit();
                return;
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

            JewishDate jd = new();
            var ad = (jd.AbsoluteDate.ToString());
            Application.Run(form ?? new frmMonthlyEnglish());
        }

        public static int SendUserOccasionEmailReminders()
        {
            int count = 0;
            if ((Properties.Settings.Default.SendReminderOnEventDay || Properties.Settings.Default.SendReminderOnDayBeforeEventDay) &&
                !string.IsNullOrWhiteSpace(Properties.Settings.Default.SendToEmailAddress) &&
                !string.IsNullOrWhiteSpace(Properties.Settings.Default.EmailFromAddress) &&
                !string.IsNullOrWhiteSpace(Properties.Settings.Default.EmailServer) &&
                !string.IsNullOrWhiteSpace(Properties.Settings.Default.EmailUserName) &&
                !string.IsNullOrWhiteSpace(Properties.Settings.Default.EmailPassword))
            {
                var body = new StringBuilder();
                string subject = "";
                JewishDate today = GetJewishToday();
                var tommorrow = today + 1;
                var hebrew = Properties.Settings.Default.LastLanguage.Contains("Hebrew");
                var nl = Environment.NewLine;
                var nlt = nl + '\t';
                var hr = nl + new string('-', 100) + nl;
                if (Properties.Settings.Default.SendReminderOnDayBeforeEventDay)
                {
                    UserOccasionColection uoc = UserOccasionColection.FromSettings(tommorrow);

                    if (uoc.Any(o => o.SendEmailReminders))
                    {
                        body.Append(hr + (hebrew
                            ? "מחר " + tommorrow.ToLongDateStringHeb()
                            : "Tommorrow " + tommorrow.ToLongDateString()) + hr);
                        subject += (hebrew ? "מחר הוא " : "Tommorrow is the ");
                        foreach (var oc in uoc.Where(o => o.SendEmailReminders))
                        {
                            count++;
                            string dateDiff = oc.GetAnniversaryString(tommorrow, hebrew);
                            string tot = nlt + oc.Name + nlt +
                               ((!string.IsNullOrWhiteSpace(dateDiff)) ? dateDiff + nlt : "") +
                               ((!string.IsNullOrWhiteSpace(oc.Notes)) ? $"Notes:{nlt}\t" + oc.Notes : "");
                            body.Append(tot);

                            subject += oc.Name + " - " + (!string.IsNullOrWhiteSpace(dateDiff)
                                ? "(" + dateDiff + ")" : "");
                        }
                        body.Append(hr);
                    }
                }
                if (Properties.Settings.Default.SendReminderOnEventDay)
                {
                    UserOccasionColection uoc = UserOccasionColection.FromSettings(today);

                    if (uoc.Any(o => o.SendEmailReminders))
                    {
                        body.Append(hr + (hebrew
                            ? "היום " + tommorrow.ToLongDateStringHeb()
                            : "Today " + today.ToLongDateString()) + hr);
                        subject += (!string.IsNullOrEmpty(subject) ? " - " : "") +
                            (hebrew ? "היום הוא " : "Today is the ");
                        foreach (var oc in uoc.Where(o => o.SendEmailReminders))
                        {
                            count++;
                            string dateDiff = oc.GetAnniversaryString(today, hebrew);
                            string tot = nlt + oc.Name + nlt +
                               ((!string.IsNullOrWhiteSpace(dateDiff)) ? dateDiff + nlt : "") +
                               ((!string.IsNullOrWhiteSpace(oc.Notes)) ? $"Notes:{nlt}\t" + oc.Notes : "");
                            body.Append(tot);

                            subject += oc.Name + " - " + (!string.IsNullOrWhiteSpace(dateDiff)
                              ? "(" + dateDiff + ")" : "");
                        }
                        body.Append(hr);
                    }
                }
                if (body.Length > 0 && subject.Length > 0)
                {
                    body.Insert(0, (hebrew
                        ? "תזכורת אירוע מתוכנת לוח"
                        : "Occasion Reminder Message from Luach") + nl);
                    try
                    {
                        MailMessage message = new();
                        SmtpClient smtp = new();
                        message.To.Add(new MailAddress(Properties.Settings.Default.SendToEmailAddress));
                        message.Subject = subject;
                        message.IsBodyHtml = false;
                        message.Body = body.ToString();
                        message.From = new MailAddress(Properties.Settings.Default.EmailFromAddress, Properties.Settings.Default.EmailFromName);
                        smtp.Port = int.Parse(Properties.Settings.Default.SmtpPort);
                        smtp.Host = Properties.Settings.Default.EmailServer;
                        smtp.EnableSsl = true;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(Properties.Settings.Default.EmailUserName, Properties.Settings.Default.EmailPassword);
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.Send(message);
                    }
                    catch (Exception) { }
                }
            }
            return count;
        }

        /// <summary>
        /// Gets todays Jewish date
        /// If the location is set in the user settings, gets the current Jewish Date based on the current location.
        /// </summary>
        /// <returns></returns>
        public static JewishDate GetJewishToday()
        {
            var locationName = Properties.Settings.Default.LocationName;
            if (locationName != null)
            {
                Location location = Program.LocationsList.FirstOrDefault(l =>
                    l.Name == locationName);
                if (location != null && Program.WeAreHere(location))
                {
                    return new JewishDate(location);
                }
            }

            return new JewishDate();
        }

        public static bool SendTestEmail(out string outMessage)
        {
            var body = new StringBuilder();
            var hebrew = Properties.Settings.Default.LastLanguage.Contains("Hebrew");
            string subject = hebrew ? "מייל דמה מתוכנת לוח!" : "Tesr email from the Luach Application";
            var nl = Environment.NewLine;
            var hr = nl + new string('-', 100) + nl;

            body.Append(hr + (hebrew
                 ? "היום עשיתם שליחת מייל דמה מתוכנת לוח"
                 : "Today you have sent a test email from the Luach Application") + hr);

            body.Insert(0, (hebrew
                ? "תזכורת אירוע מתוכנת לוח"
                : "Occasion Reminder Message from Luach") + nl);
            try
            {
                MailMessage message = new();
                SmtpClient smtp = new();
                message.To.Add(new MailAddress(Properties.Settings.Default.SendToEmailAddress));
                message.Subject = subject;
                message.IsBodyHtml = false;
                message.Body = body.ToString();
                message.From = new MailAddress(Properties.Settings.Default.EmailFromAddress, Properties.Settings.Default.EmailFromName);
                smtp.Port = int.Parse(Properties.Settings.Default.SmtpPort);
                smtp.Host = Properties.Settings.Default.EmailServer;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(Properties.Settings.Default.EmailUserName, Properties.Settings.Default.EmailPassword);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
                outMessage = "Success";
                return true;
            }
            catch (Exception ex)
            {
                outMessage = ex.Message;
                return false;
            }
        }

        public static void SetDailyRemindersTask()
        {
            using TaskService ts = new();
            //Always delete the previous one.
            ts.RootFolder.DeleteTask(Program.RemindersTaskName, false);

            if (Properties.Settings.Default.SendReminderOnEventDay || Properties.Settings.Default.SendReminderOnDayBeforeEventDay)
            {
                try
                {
                    bool isVistaPlus = Environment.OSVersion.Version.Major >= 6;
                    string path = Application.ExecutablePath;
                    string folder = Application.StartupPath;
                    using TaskDefinition td = ts.NewTask();
                    using DailyTrigger dt = new();

                    td.Actions.Add(path, "-uoc", folder);
                    dt.StartBoundary = (DateTime.Now.Date + Properties.Settings.Default.ReminderTimeOfDay.TimeOfDay);
                    dt.EndBoundary = dt.StartBoundary.AddYears(75);
                    dt.DaysInterval = 1;
                    td.Triggers.Add(dt);

                    if (isVistaPlus)
                    {
                        td.Principal.LogonType = TaskLogonType.InteractiveToken;
                        td.Principal.UserId = System.Threading.Thread.CurrentPrincipal.Identity.Name;
                        td.RegistrationInfo.Date = DateTime.Now;
                        td.RegistrationInfo.Author = "Compute Software Solutions, LLC.";
                        td.RegistrationInfo.Version = new Version("6.1.3");
                        td.RegistrationInfo.Description = "This task was created by the Luach application. " +
                            "It runs each day at the time specified and sends an reminder email for any Events and Occasions that are set for this.";
                        td.Settings.AllowDemandStart = true;
                        td.Settings.AllowHardTerminate = true;
                        td.Settings.StartWhenAvailable = true;
                        td.Settings.DeleteExpiredTaskAfter = new TimeSpan(0, 0, 0, 1);
                        td.Settings.DisallowStartIfOnBatteries = false;
                        td.Settings.DisallowStartOnRemoteAppSession = false;
                        td.Settings.ExecutionTimeLimit = new TimeSpan(1, 0, 0, 0, 0);
                        td.Settings.RunOnlyIfNetworkAvailable = true;
                        td.Settings.StopIfGoingOnBatteries = false;
                        td.Settings.WakeToRun = true;
                    }
                    ts.RootFolder.RegisterTaskDefinition(Program.RemindersTaskName, td);
                }
                catch (TSNotSupportedException nse)
                {
                    throw nse;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
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
            using var ms = new System.IO.StringReader(Properties.Resources.LocationsList);
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