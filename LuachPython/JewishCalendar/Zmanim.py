from Location import Location
import datetime
import math

'''Computes the daily Zmanim for any single date at any location.
 The astronomical and mathematical calculations were directly adapted from the excellent
 Jewish calendar calculation in C# Copyright © by Ulrich and Ziporah Greve (2005) '''
class Zmanim:
    def __init__(self, location, date):
        self.location = location or Location.getJerusalem()
        self.seculardate = date if date is datetime.date else date.todate()
        
    '''Gets sunrise and sunset time for given date.
    Accepts a javascript Date object, a string for creating a javascript date object or a jDate object.
    Returns: sunrise:: hour: 6, minute: 18, sunset:: hour: 19, minute: 41
    Location object is required.'''
    def getSunTimes(self, considerElevation):
            day = Zmanim.dayOfYear(date)
                zeninthDeg = 90 
                zenithMin = 50
                lonHour = 0
                longitude = 0
                latitude = 0
                cosLat = 0 
                sinLat = 0
                cosZen = 0
                sinDec = 0
                cosDec = 0
                xmRise = 0
                xmSet = 0
                xlRise = 0
                xlSet = 0
                aRise = 0
                aSet = 0
                ahrRise = 0
                ahrSet = 0
                hRise = 0
                hSet = 0
                tRise = 0
                tSet = 0
                utRise = 0
                utSet = 0
                earthRadius = 6356900
                zenithAtElevation = Zmanim.degToDec(zeninthDeg, zenithMin) +
                                    Zmanim.radToDeg(math.acos(earthRadius / (earthRadius + ((self.location.Elevation or 0) if considerElevation else 0))))
        
            zeninthDeg = math.floor(zenithAtElevation)
            zenithMin = (zenithAtElevation - math.floor(zenithAtElevation)) * 60
            cosZen = math.cos(0.01745 * Zmanim.degToDec(zeninthDeg, zenithMin))
            longitude = self.location.Longitude
            lonHour = longitude / 15
            latitude = self.location.Latitude
            cosLat = math.cos(0.01745 * latitude)
            sinLat = math.sin(0.01745 * latitude)
            tRise = day + (6 + lonHour) / 24
            tSet = day + (18 + lonHour) / 24
            xmRise = Zmanim.M(tRise)
            xlRise = Zmanim.L(xmRise)
            xmSet = Zmanim.M(tSet)
            xlSet = Zmanim.L(xmSet)
            aRise = 57.29578 * math.atan(0.91746 * math.tan(0.01745 * xlRise))
            aSet = 57.29578 * math.atan(0.91746 * math.tan(0.01745 * xlSet))
            if (math.abs(aRise + 360 - xlRise) > 90):
                aRise += 180
            if (aRise > 360):
                aRise -= 360
            if (math.abs(aSet + 360 - xlSet) > 90):
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
            if (math.abs(hRise) <= 1):
                hRise = 57.29578 * math.acos(hRise)
                utRise = ((360 - hRise) / 15) + ahrRise + Zmanim.adj(tRise) + lonHour
                sunrise = Zmanim.timeAdj(utRise + self.location.UTCOffset, self.date, self.location)
                while (sunrise.hour > 12):
                    sunrise.hour -= 12
        
            if (math.abs(hSet) <= 1):
                hSet = 57.29578 * math.acos(hSet)
                utSet = (hRise / 15) + ahrSet + Zmanim.adj(tSet) + lonHour
                sunset = Zmanim.timeAdj(utSet + self.location.UTCOffset, self.date, self.location)
                while (sunset.hour < 12):
                    sunset.hour += 12
        
            return: {'sunrise': sunrise, 'sunset': sunset}






