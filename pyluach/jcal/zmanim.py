import datetime
import math

import jcal.utils as Utils
from jcal.hourminute import HourMinute
from jcal.jdate import JDate
from jcal.location import Location

'''Computes the daily Zmanim for any single date at any location.
 The astronomical and mathematical calculations were directly adapted from the excellent
 Jewish calendar calculation in C# Copyright © by Ulrich and Ziporah Greve (2005)
 To get the zmanim for today in Jerusalem use:
    from jcal.jdate import JDate
    from jcal.Zmanim import Zmanim
    jd = JDate.today()
    zm = Zmanim(date=jd)
    print(zm.get_sun_times())
'''


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
        day = Zmanim.day_of_syear(self.seculardate)
        zeninthDeg = 90
        zenithMin = 50
        earthRadius = 6356900
        zenithAtElevation = Zmanim._degtodec(zeninthDeg, zenithMin) + Zmanim._radtodeg(
            math.acos(earthRadius / (earthRadius + ((self.location.elevation or 0) if considerElevation else 0))))
        zeninthDeg = math.floor(zenithAtElevation)
        zenithMin = (zenithAtElevation - math.floor(zenithAtElevation)) * 60
        cosZen = math.cos(0.01745 * Zmanim._degtodec(zeninthDeg, zenithMin))
        longitude = self.location.longitude
        lonHour = longitude / 15
        latitude = self.location.latitude
        cosLat = math.cos(0.01745 * latitude)
        sinLat = math.sin(0.01745 * latitude)
        tRise = day + (6 + lonHour) / 24
        tSet = day + (18 + lonHour) / 24
        xmRise = Zmanim._m(tRise)
        xlRise = Zmanim._l(xmRise)
        xmSet = Zmanim._m(tSet)
        xlSet = Zmanim._l(xmSet)
        aRise = 57.29578 * math.atan(0.91746 * math.tan(0.01745 * xlRise))
        aSet = 57.29578 * math.atan(0.91746 * math.tan(0.01745 * xlSet))
        if (abs(aRise + 360 - xlRise) > 90):
            aRise += 180
        if (aRise > 360):
            aRise -= 360
        if (abs(aSet + 360 - xlSet) > 90):
            aSet += 180
        if (aSet > 360):
            aSet -= 360
        ahrRise = aRise / 15
        sinDec = 0.39782 * math.sin(0.01745 * xlRise)
        cosDec = math.sqrt(1 - sinDec * sinDec)
        hRise = (cosZen - sinDec * sinLat) / (cosDec * cosLat)
        ahrSet = aSet / 15
        sinDec = 0.39782 * math.sin(0.01745 * xlSet)
        cosDec = math.sqrt(1 - sinDec * sinDec)
        hSet = (cosZen - sinDec * sinLat) / (cosDec * cosLat)
        if abs(hRise) <= 1:
            hRise = 57.29578 * math.acos(hRise)
            utRise = ((360 - hRise) / 15) + ahrRise + Zmanim._adjust(tRise) + lonHour
            Zmanim._set_time(sunrise, utRise + self.location.utcOffset, self.seculardate, self.location)
            while (sunrise.hour > 12):
                sunrise.hour -= 12

        if abs(hSet) <= 1:
            hSet = 57.29578 * math.acos(hSet)
            utSet = (hRise / 15) + ahrSet + Zmanim._adjust(tSet) + lonHour
            Zmanim._set_time(sunset, utSet + self.location.utcOffset, self.seculardate, self.location)
            while (sunset.hour < 12):
                sunset.hour += 12

        return (sunrise, sunset)

    def get_chatzos(self, suntimes=None):
        netzShkia = suntimes or self.get_sun_times(False)
        netz = netzShkia[0]
        shkia = netzShkia[1]
        noValue = HourMinute(0, 0)

        if netz == noValue or shkia == noValue:
            return None
        else:
            chatzi = int((shkia.total_minutes() - netz.total_minutes()) / 2)
            return netz + chatzi

    def get_shaa_zmanis(self, offset=0, netzshkia=None):
        if netzshkia is None:
            netzshkia = self.get_sun_times(True)
        netz = netzshkia[0] - offset
        shkia = netzshkia[1] + offset
        noValue = HourMinute(0, 0)

        if netz == noValue or shkia == noValue:
            return None
        else:
            return (shkia.total_minutes() - netz.total_minutes()) / 12.0

    def get_candle_lighting(self):
        shkiah = self.get_sun_times()[1]
        if self.location.candles:
            return shkiah.addtime(0, -(self.location.candles))
        elif not self.location.israel:
            return shkiah.addtime(0, -18)
        else:
            loclc = self.location.name.lower()
            if loclc in ['jerusalem', 'yerush', 'petach', 'petah', 'petak']:
                return shkiah.addtime(0, -40)
            elif loclc in ['haifa', 'chaifa', 'be\'er sheva', 'beersheba']:
                return shkiah.addtime(0, -22)
            else:
                return shkiah.addtime(0, -30)

    @staticmethod
    def is_syear_leap(year):
        if (year % 400 == 0):
            return True
        if (year % 100 != 0):
            if (year % 4 == 0):
                return True
        return False

    @staticmethod
    def day_of_syear(dt):
        monCount = [0, 1, 32, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335, 366]
        if ((dt.month > 2) and (Zmanim.is_syear_leap(dt.year))):
            return monCount[dt.month] + dt.day + 1
        else:
            return monCount[dt.month] + dt.day

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

        inCurrTZ = location.utcOffset == Utils.curr_utc_offset()
        if (inCurrTZ and Utils.is_sd_dst(date)):
            hour += 1
        elif ((not inCurrTZ) and ((location.israel and Utils.is_il_dst(date)) or Utils.is_usa_dst(date, hour))):
            hour += 1

        hm.hour = hour
        hm.minute = min


if __name__ == '__main__'"":
    jd = JDate.today()
    zm = Zmanim(date=jd)
    print(zm.get_sun_times())
    print(zm.get_chatzos())
