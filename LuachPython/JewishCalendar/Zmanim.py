import datetime
import math
from .JewishDate import JewishDate
from .HourMinute import HourMinute
from .Location import Location
from .Utils import Utils

'''Computes the daily Zmanim for any single date at any location.
 The astronomical and mathematical calculations were directly adapted from the excellent
 Jewish calendar calculation in C# Copyright © by Ulrich and Ziporah Greve (2005) '''


class Zmanim:
    def __init__(self, location, date = datetime.datetime.today()):
        self.location = location or Location.getJerusalem()
        if isinstance(date, datetime.datetime):
            self.seculardate = date
        elif isinstance(date, JewishDate):
            self.seculardate = date.todate()
        else:
            raise ValueError('date must be either a python or Jewish date')

    '''Gets sunrise and sunset time for the current date.
    Returns a tuple of HourMinute objects (sunrise, sunset)'''
    def getSunTimes(self, considerElevation):
        sunrise = HourMinute(0, 0)
        sunset = HourMinute(0, 0)
        day = Zmanim.dayOfYear(self.seculardate)
        zeninthDeg = 90 
        zenithMin = 50
        earthRadius = 6356900
        zenithAtElevation = Zmanim.__degToDec(zeninthDeg, zenithMin) + Zmanim.__radToDeg(math.acos(earthRadius / (earthRadius + ((self.location.elevation or 0) if considerElevation else 0))))
        zeninthDeg = math.floor(zenithAtElevation)
        zenithMin = (zenithAtElevation - math.floor(zenithAtElevation)) * 60
        cosZen = math.cos(0.01745 * Zmanim.__degToDec(zeninthDeg, zenithMin))
        longitude = self.location.longitude
        lonHour = longitude / 15
        latitude = self.location.latitude
        cosLat = math.cos(0.01745 * latitude)
        sinLat = math.sin(0.01745 * latitude)
        tRise = day + (6 + lonHour) / 24
        tSet = day + (18 + lonHour) / 24
        xmRise = Zmanim.__m(tRise)
        xlRise = Zmanim.__l(xmRise)
        xmSet = Zmanim.__m(tSet)
        xlSet = Zmanim.__l(xmSet)
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
            utRise = ((360 - hRise) / 15) + ahrRise + Zmanim.__adj(tRise) + lonHour
            Zmanim.__set_time(sunrise, utRise + self.location.utcOffset, self.seculardate, self.location)
            while (sunrise.hour > 12):
                sunrise.hour -= 12

        if abs(hSet) <= 1:
            hSet = 57.29578 * math.acos(hSet)
            utSet = (hRise / 15) + ahrSet + Zmanim.__adj(tSet) + lonHour
            Zmanim.__set_time(sunset, utSet + self.location.utcOffset, self.seculardate, self.location)
            while (sunset.hour < 12):
                sunset.hour += 12
    
        return (sunrise, sunset)

    @staticmethod
    def isSecularLeapYear(year):
        if (year % 400 == 0):
            return True
        if (year % 100 != 0):
            if (year % 4 == 0):
                return True
        return False

    @staticmethod
    def dayOfYear(dt):
        monCount = [0, 1, 32, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335, 366]
        if ((dt.month > 2) and (Zmanim.isSecularLeapYear(dt.year))):
            return monCount[dt.month] + dt.day + 1
        else:
            return monCount[dt.month] + dt.day

    @staticmethod
    def __degToDec (deg, min):
        return (deg + min / 60.0)

    @staticmethod
    def __m (x):
        return 0.9856 * x - 3.251

    @staticmethod
    def __l (x):
        return x + 1.916 * math.sin(0.01745 * x) + 0.02 * math.sin(2 * 0.01745 * x) + 282.565

    @staticmethod
    def __adj(x):
        return -0.06571 * x - 6.62

    @staticmethod
    def __radToDeg(rad):
        return 57.29578 * rad

    @staticmethod
    def __set_time(hm, time, date, location):
        if (time < 0):
            time += 24
        hour = int(time)
        min = int(int((time - hour) * 60 + 0.5))
        
        inCurrTZ = location.utcOffset == Utils.currUtcOffset()
        if (inCurrTZ and Utils.isDateDST(date)):
            hour += 1
        elif ((not inCurrTZ) and
            ((location.israel and Utils.isIsrael_DST(date)) or Utils.isUSA_DST(date, hour))):
            hour += 1
        hm.hour = hour
        hm.minute = min





