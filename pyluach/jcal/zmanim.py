import datetime
import math

import jcal.utils as utils
from jcal.hourminute import HourMinute
from jcal.jdate import JDate
from jcal.location import Location

'''Computes the daily Zmanim for any single date at any location.
 The astronomical and mathematical calculations were directly adapted from the excellent
 Jewish calendar calculation in C# Copyright © by Ulrich and Ziporah Greve (2005)
 To get the zmanim for today in Jerusalem use:
    from jcal.jdate import JDate
    from jcal.Zmanim import Zmanim
    convertdate = JDate.today()
    zm = Zmanim(date=convertdate)
    print(zm.get_sun_times())
'''

EARTH_RADIUS = 6356900


class Zmanim:
    def __init__(self, location=Location.get_jerusalem(), dt=datetime.datetime.today()):
        self.location = location or Location.get_jerusalem()
        if isinstance(dt, datetime.datetime):
            self.seculardate = dt
        elif isinstance(dt, JDate):
            self.seculardate = dt.todate()
        else:
            raise ValueError('date must be either a python or Jewish date')

    '''Gets sunrise and sunset time for the current date.
    Returns a tuple of HourMinute objects (sunrise, sunset)'''

    def get_sun_times(self, considerElevation=True):
        sunrise = HourMinute(0, 0)
        sunset = HourMinute(0, 0)
        day = utils.days_till_greg_date(self.seculardate) + 1
        zen_deg = 90
        zen_min = 50
        zen_at_elv = Zmanim._degtodec(zen_deg, zen_min) + Zmanim._radtodeg(
            math.acos(EARTH_RADIUS / (EARTH_RADIUS + ((self.location.elevation or 0) if considerElevation else 0))))
        zen_deg = math.floor(zen_at_elv)
        zen_min = (zen_at_elv - math.floor(zen_at_elv)) * 60
        cos_zen = math.cos(0.01745 * Zmanim._degtodec(zen_deg, zen_min))
        longitude = self.location.longitude
        lon_h = longitude / 15
        latitude = self.location.latitude
        cos_lat = math.cos(0.01745 * latitude)
        sinLat = math.sin(0.01745 * latitude)
        t_rise = day + (6 + lon_h) / 24
        t_set = day + (18 + lon_h) / 24
        xm_rise = Zmanim._m(t_rise)
        xl_rise = Zmanim._l(xm_rise)
        xm_set = Zmanim._m(t_set)
        xl_set = Zmanim._l(xm_set)
        a_rise = 57.29578 * math.atan(0.91746 * math.tan(0.01745 * xl_rise))
        a_set = 57.29578 * math.atan(0.91746 * math.tan(0.01745 * xl_set))
        if abs(a_rise + 360 - xl_rise) > 90:
            a_rise += 180
        if (a_rise > 360):
            a_rise -= 360
        if (abs(a_set + 360 - xl_set) > 90):
            a_set += 180
        if (a_set > 360):
            a_set -= 360
        ahr_rise = a_rise / 15
        sin_dec = 0.39782 * math.sin(0.01745 * xl_rise)
        cos_dec = math.sqrt(1 - sin_dec * sin_dec)
        h_rise = (cos_zen - sin_dec * sinLat) / (cos_dec * cos_lat)
        ahrSet = a_set / 15
        sin_dec = 0.39782 * math.sin(0.01745 * xl_set)
        cos_dec = math.sqrt(1 - sin_dec * sin_dec)
        h_set = (cos_zen - sin_dec * sinLat) / (cos_dec * cos_lat)
        if abs(h_rise) <= 1:
            h_rise = 57.29578 * math.acos(h_rise)
            ut_rise = ((360 - h_rise) / 15) + ahr_rise + Zmanim._adjust(t_rise) + lon_h
            Zmanim._set_time(sunrise, ut_rise + self.location.utcoffset, self.seculardate, self.location)
            while (sunrise.hour > 12):
                sunrise.hour -= 12

        if abs(h_set) <= 1:
            h_set = 57.29578 * math.acos(h_set)
            ut_set = (h_rise / 15) + ahrSet + Zmanim._adjust(t_set) + lon_h
            Zmanim._set_time(sunset, ut_set + self.location.utcoffset, self.seculardate, self.location)
            while (sunset.hour < 12):
                sunset.hour += 12

        return (sunrise, sunset)

    def get_chatzos(self, suntimes=None):
        netz, shkia = suntimes or self.get_sun_times(False)
        no_value = HourMinute(0, 0)

        if netz == no_value or shkia == no_value:
            return None
        else:
            chatzi = int((shkia.total_minutes() - netz.total_minutes()) / 2)
            return netz + chatzi

    def get_shaa_zmanis(self, offset=0, netzshkia=None):
        if netzshkia is None:
            netzshkia = self.get_sun_times(True)
        netz = netzshkia[0] - offset
        shkia = netzshkia[1] + offset
        no_value = HourMinute(0, 0)

        if netz == no_value or shkia == no_value:
            return None
        else:
            return (shkia.total_minutes() - netz.total_minutes()) / 12

    def get_candle_lighting(self):
        shkiah = self.get_sun_times()[1]
        if self.location.candles:
            return shkiah - self.location.candles
        elif not self.location.israel:
            return shkiah - 18
        else:
            loclc = self.location.name.lower()
            if loclc in ['jerusalem', 'yerush', 'petach', 'petah', 'petak']:
                return shkiah - 40
            elif loclc in ['haifa', 'chaifa', 'be\'er sheva', 'beersheba']:
                return shkiah - 22
            else:
                return shkiah - 30

    # convert degrees to decimal
    @staticmethod
    def _degtodec(deg, min):
        return (deg + min / 60.0)

    @staticmethod
    def _m(x):
        return 0.9856 * x - 3.251

    @staticmethod
    def _l(x):
        return x + 1.916 * math.sin(0.01745 * x) + 0.02 * math.sin(2 * 0.01745 * x) + 282.565

    @staticmethod
    def _adjust(x):
        return -0.06571 * x - 6.62

    # convert radians to degress
    @staticmethod
    def _radtodeg(rad):
        return 57.29578 * rad

    @staticmethod
    def _set_time(hm, time, date, location):
        if (time < 0):
            time += 24
        hour = int(time)
        min = int(int((time - hour) * 60 + 0.5))

        while min >= 60:
            hour += 1
            min -= 60

        inCurrTZ = location.utcoffset == utils.curr_utc_offset()
        if (inCurrTZ and utils.is_sd_dst(date)):
            hour += 1
        elif ((not inCurrTZ) and ((location.israel and utils.is_il_dst(date)) or utils.is_usa_dst(date, hour))):
            hour += 1

        hm.hour = hour
        hm.minute = min


if __name__ == '__main__'"":
    jd = JDate.today()
    zm = Zmanim(dt=jd)
    print(zm.get_sun_times())
    print(zm.get_chatzos())
