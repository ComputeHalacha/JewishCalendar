namespace JewishCalendar
{
    /// <summary>
    /// A simpler Time Span. Represents just the hour and the minute.
    /// Explicitly cast-able to and from System.TimeSpan.
    /// </summary>    
    public struct HourMinute
    {
        /// <summary>
        /// The hour
        /// </summary>
        public int Hour;
        /// <summary>
        /// The minute
        /// </summary>
        public int Minute;

        /// <summary>
        /// Returns a TimeSpan representation of this HourMinute
        /// </summary>
        /// <returns></returns>
        public System.TimeSpan ToTimeSpan()
        {
            return new System.TimeSpan(this.Hour, this.Minute, 0);
        }

        /// <summary>
        /// An HourMinute that does not represent a real time.
        /// Use in the place of null or empty etc.
        /// Note: very different from TimeSpan.Zero which represents "zero hour" or midnight.
        /// </summary>
        public static HourMinute NoValue
        {
            get
            {
                //-1 minutes is pretty meaningless. 
                //(negative hours is pretty meaningless too, 
                //but using negative hours can sometimes be useful for calculations)
                return new HourMinute { Minute = -1 };
            }
        }

        /// <summary>
        /// Add minutes
        /// </summary>
        /// <param name="t"></param>
        /// <param name="min"></param>
        /// <returns></returns>
        public static HourMinute operator +(HourMinute t, int min)
        {
            HourMinute result = new HourMinute { Hour = t.Hour, Minute = t.Minute + min };
            while (result.Minute >= 60)
            {
                result.Minute -= 60;
                result.Hour++;
            }
            return result;
        }

        /// <summary>
        /// Add the number of hours and minutes in the given HourMinute to the current HourMinute
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static HourMinute operator +(HourMinute t1, HourMinute t2)
        {
            //Let's cheat and use a real TimeSpan for this.
            return (HourMinute)(t1.ToTimeSpan().Add(t2.ToTimeSpan()));
        }

        /// <summary>
        /// Add a TimeSpan to this HourMinute
        /// </summary>
        /// <param name="t"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static HourMinute operator +(HourMinute t, System.TimeSpan ts)
        {
            return (HourMinute)(t.ToTimeSpan().Add(ts));
        }

        /// <summary>
        /// Subtract minutes.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="min"></param>
        /// <returns></returns>
        public static HourMinute operator -(HourMinute t, int min)
        {
            HourMinute result = new HourMinute { Hour = t.Hour, Minute = t.Minute - min };
            while (result.Minute < 0)
            {
                result.Minute += 60;
                result.Hour--;
            }
            return result;
        }

        /// <summary>
        /// Subtract the number of hours and minutes in the given HourMinute from the current HourMinute
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static HourMinute operator -(HourMinute t1, HourMinute t2)
        {
            return (HourMinute)(t1.ToTimeSpan().Subtract(t2.ToTimeSpan()));
        }

        /// <summary>
        /// Subtract a TimeSpan from this HourMinute
        /// </summary>
        /// <param name="t"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static HourMinute operator -(HourMinute t, System.TimeSpan ts)
        {
            return (HourMinute)(t.ToTimeSpan().Subtract(ts));
        }

        /// <summary>
        /// Compares 2 HourMinute objects
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool operator ==(HourMinute t1, HourMinute t2)
        {
            return (t1.Hour == t2.Hour && t1.Minute == t2.Minute);
        }

        /// <summary>
        /// Compares 2 HourMinute objects for inequality
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool operator !=(HourMinute t1, HourMinute t2)
        {
            return !(t1 == t2);
        }

        /// <summary>
        /// Compare the current HourMinute to a System.TimeSpan
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static bool operator ==(HourMinute t1, System.TimeSpan ts)
        {
            return (t1.Hour * 60 + t1.Minute) == System.Convert.ToInt32(System.Math.Floor(ts.TotalMinutes));
        }

        /// <summary>
        /// Compare the current HourMinute to a System.TimeSpan for inequality
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static bool operator !=(HourMinute t1, System.TimeSpan ts)
        {
            return !(t1 == ts);
        }

        /// <summary>
        /// Returns true if the current HourMinute is after the second one.
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool operator >(HourMinute t1, HourMinute t2)
        {
            return ((t1.Hour * 60 + t1.Minute) > (t2.Hour * 60 + t2.Minute));
        }

        /// <summary>
        /// Returns true if the current HourMinute is before the second one.
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool operator <(HourMinute t1, HourMinute t2)
        {
            return ((t1.Hour * 60 + t1.Minute) < (t2.Hour * 60 + t2.Minute));
        }

        /// <summary>
        /// Returns true if the current HourMinute is after the TimeSpan.
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static bool operator >(HourMinute t1, System.TimeSpan ts)
        {
            return ((t1.Hour * 60 + t1.Minute) > ts.TotalMinutes);
        }

        /// <summary>
        /// Returns true if the current HourMinute is not before the second one.
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool operator >=(HourMinute t1, HourMinute t2)
        {
            return ((t1.Hour * 60 + t1.Minute) >= (t2.Hour * 60 + t2.Minute));
        }

        /// <summary>
        /// Returns true if the current HourMinute is not after the second one.
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool operator <=(HourMinute t1, HourMinute t2)
        {
            return ((t1.Hour * 60 + t1.Minute) <= (t2.Hour * 60 + t2.Minute));
        }

        /// <summary>
        /// Returns true if the current HourMinute is not before the TimeSpan.
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static bool operator >=(HourMinute t1, System.TimeSpan ts)
        {
            return ((t1.Hour * 60 + t1.Minute) >= ts.TotalMinutes);
        }

        /// <summary>
        /// Returns true if the current HourMinute is not after the TimeSpan.
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static bool operator <=(HourMinute t1, System.TimeSpan ts)
        {
            return ((t1.Hour * 60 + t1.Minute) <= ts.TotalMinutes);
        }

        /// <summary>
        /// Returns true if the current HourMinute is before the TimeSpan.
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static bool operator <(HourMinute t1, System.TimeSpan ts)
        {
            return ((t1.Hour * 60 + t1.Minute) < ts.TotalMinutes);
        }

        /// <summary>
        /// Explicitly convert (cast) an HourMinute into a TimeSpan.
        /// </summary>
        /// <param name="hm"></param>
        /// <returns></returns>
        public static explicit operator System.TimeSpan(HourMinute hm)
        {
            return hm.ToTimeSpan();
        }

        /// <summary>
        /// Explicitly convert (cast) a TimeSpan into an HourMinute
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static explicit operator HourMinute(System.TimeSpan ts)
        {
            return new HourMinute { Hour = ts.Hours, Minute = ts.Minutes };
        }

        /// <summary>
        /// The total number of minutes represented by this HourMinute (includes the hours)
        /// </summary>
        public int TotalMinutes
        {
            get
            {
                return this.Hour * 60 + this.Minute;
            }
        }

        /// <summary>
        /// The hour and minute displayed in the format: h:MM tt
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return (Hour <= 12 ? (Hour == 0 ? 12 : Hour) : Hour - 12).ToString() +
                    ":" +
                   (Minute < 10 ? "0" + Minute.ToString() : Minute.ToString()) +
                   (Hour < 12 ? " AM" : " PM");
        }

        /// <summary>
        /// Returns the current time in the format HH:mm
        /// </summary>
        /// <returns></returns>
        public string ToString24H()
        {
            return (Hour.ToString() +
                    ":" +
                   (Minute < 10 ? "0" + Minute.ToString() : Minute.ToString()));
        }

        /// <summary>
        /// Tests 2 HourMinute objects for equality.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is HourMinute)
            {
                return ((HourMinute)obj) == this;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the hashcode for this instance
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Hour.GetHashCode() ^ this.Minute.GetHashCode();
        }
    }
}