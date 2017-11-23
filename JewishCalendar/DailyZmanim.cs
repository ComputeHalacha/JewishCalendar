using System;

namespace JewishCalendar
{
    /// <summary>
    /// The type of the single Zman
    /// </summary>
    public enum ZmanType
    {
        /// <summary>
        /// Sunrise at sea level
        /// </summary>
        NetzMishor,
        /// <summary>
        /// Sunrise at current Locations elevation
        /// </summary>
        Netz,
        /// <summary>
        /// Say Shma by... according to the Magen Avraham
        /// </summary>
        KShmMga,
        /// <summary>
        /// Say Shma by... according to the Gr"a
        /// </summary>
        KshmGra,
        /// <summary>
        /// Say Shmonah Esray of Shacharis by... according to the Magen Avraham
        /// </summary>
        TflMga,
        /// <summary>
        /// Say Shmonah Esray of Shacharis by... according to the Gr"a
        /// </summary>
        TflGra,
        /// <summary>
        /// Chatzos of the night and the day
        /// </summary>
        Chatzos,
        /// <summary>
        /// Mincha Gedolah
        /// </summary>
        MinchaG,
        /// <summary>
        /// Mincha Ketana
        /// </summary>
        MinchaK,
        /// <summary>
        /// Plag Hamincha
        /// </summary>
        MinchaPlg,
        /// <summary>
        /// Sunset at current Locations elevation
        /// </summary>
        Shkia,
        /// <summary>
        /// Sunset at sea level
        /// </summary>
        ShkiaMishor
    }
    /// <summary>
    /// Gives efficient access to the daily Zmanim for a single day at the given location
    /// </summary>
    public class DailyZmanim
    {
        private Zmanim _zmanim;
        private HourMinute[] _netzshkia = null;
        private HourMinute[] _netzshkiaMishor = null;
        private HourMinute _chatzos = HourMinute.NoValue;
        private double _shaaZmanis;
        private double _shaaZmanis90;

        /// <summary>
        /// Create a new DailyZmanim object for the given date and location
        /// </summary>
        /// <param name="sd">The Gregorian Date</param>
        /// <param name="location">The location</param>
        public DailyZmanim(DateTime sd, Location location)
        {
            this._zmanim = new Zmanim(sd, location);
        }

        /// <summary>
        /// Access the underlying Zmanim object
        /// </summary>
        public Zmanim Zmanim => this._zmanim;
        /// <summary>
        /// The Gregorian Date
        /// </summary>
        public DateTime SecularDate
        {
            get { return this._zmanim.SecularDate; }
            set { this._zmanim.SecularDate = value; }
        }
        /// <summary>
        /// The Location
        /// </summary>
        public Location Location
        {
            get { return this._zmanim.Location; }
            set { this._zmanim.Location = value; }
        }
        /// <summary>
        /// Gets an array of two HourMinute structures for the current Date and Location. 
        /// The first is the time of Netz for the current date at the elevation 
        /// and coordinates of the current Location and the second is the time of shkia.        
        /// </summary>
        public HourMinute[] NetzShkia
        {
            get
            {
                if (this._netzshkia == null)
                {
                    this._netzshkia = this._zmanim.GetNetzShkia(true);
                }
                return this._netzshkia;
            }
        }
        /// <summary>
        /// Gets an array of two HourMinute structures at sea level for the current Date and Location. 
        /// The first is the time of Netz for the current date and location and the second is the time of shkia.
        /// The elevation is NOT kept into account.
        /// </summary>
        public HourMinute[] NetzShkiaMishor
        {
            get
            {
                if (this._netzshkiaMishor == null)
                {
                    this._netzshkiaMishor = this._zmanim.GetNetzShkia(false);
                }
                return this._netzshkiaMishor;
            }
        }
        /// <summary>
        /// Sunrise for the current Date at the elevation and coordinates of the current Location. 
        /// </summary>
        public HourMinute Netz => this.NetzShkia[0];
        /// <summary>
        /// Sunset for the current Date at the elevation and coordinates of the current Location.
        /// </summary>
        public HourMinute Shkia => this.NetzShkia[1];
        /// <summary>
        /// Sunrise at sea level for the current Date at the coordinates of the current Location.
        /// </summary>
        public HourMinute NetzMishor => this.NetzShkiaMishor[0];
        /// <summary>
        /// Sunset at sea level for the current Date at the coordinates of the current Location.
        /// </summary>
        public HourMinute ShkiaMishor => this.NetzShkiaMishor[1];
        /// <summary>
        /// Chatzos of the day and the night
        /// </summary>
        public HourMinute Chatzos
        {
            get
            {
                if (this._chatzos == HourMinute.NoValue)
                {
                    this._chatzos = Zmanim.GetChatzos(this.NetzShkiaMishor);
                }
                return this._chatzos;
            }
        }
        /// <summary>
        /// The length of Shaa zmanis in minutes for current date and location.
        /// Configured from netz to shkia at sea level.
        /// </summary>
        public Double ShaaZmanis
        {
            get
            {
                if (this._shaaZmanis == 0)
                {
                    this._shaaZmanis = Zmanim.GetShaaZmanis(this.NetzShkiaMishor);
                }
                return this._shaaZmanis;
            }
        }
        /// <summary>
        /// The length of Shaa zmanis in minutes for current date and location.
        /// Configured from 90 minutes before netz to 90 minutes after shkia at sea level.
        /// </summary>
        public Double ShaaZmanis90
        {
            get
            {
                if (this._shaaZmanis90 == 0)
                {
                    this._shaaZmanis90 = Zmanim.GetShaaZmanis(this.NetzShkiaMishor, 90);
                }
                return this._shaaZmanis90;
            }
        }

        /// <summary>
        /// Gets a single Zman for the current Date and Location
        /// </summary>
        /// <param name="type">The type of Zman to return</param>
        /// <param name="offset">Optional. The number of minutes to offset the zman by.</param>
        /// <returns></returns>
        public HourMinute GetZman(ZmanType type, int offset = 0)
        {
            HourMinute hm = HourMinute.NoValue;
            switch (type)
            {
                case ZmanType.NetzMishor:
                    hm = this.NetzMishor;
                    break;

                case ZmanType.Netz:
                    hm = this.Netz;
                    break;

                case ZmanType.KShmMga:
                    hm = (this.NetzMishor - 90) + (int)Math.Floor(this.ShaaZmanis90 * 3D);
                    break;

                case ZmanType.KshmGra:
                    hm = this.NetzMishor + (int)Math.Floor(this.ShaaZmanis * 3D);
                    break;

                case ZmanType.TflMga:
                    hm = (this.NetzMishor - 90) + (int)Math.Floor(this.ShaaZmanis90 * 4D);
                    break;

                case ZmanType.TflGra:
                    hm = this.NetzMishor + (int)Math.Floor(this.ShaaZmanis * 4D);
                    break;

                case ZmanType.Chatzos:
                    hm = this.Chatzos;
                    break;

                case ZmanType.MinchaG:
                    hm = this.Chatzos + (int)(this.ShaaZmanis * 0.5);
                    break;

                case ZmanType.MinchaK:
                    hm = this.NetzMishor + (int)(this.ShaaZmanis * 9.5);
                    break;

                case ZmanType.MinchaPlg:
                    hm = this.NetzMishor + (int)(this.ShaaZmanis * 10.75);
                    break;

                case ZmanType.Shkia:
                    hm = this.Shkia;
                    break;

                case ZmanType.ShkiaMishor:
                    hm = this.ShkiaMishor;
                    break;

            }
            if (offset != 0)
            {
                hm += offset;
            }
            return hm;
        }
    }
}
