import jcal.utils as Utils
from jcal.hourminute import HourMinute
from jcal.jdate import JDate
from jcal.zmanim import Zmanim

''' Returns the molad for the given jewish month and year.
 Algorithm was adapted from Hebcal by Danny Sadinoff.
 
  Example of use:
  moladString = Molad.getString(5776, 10)
 '''


class Molad:
    @staticmethod
    def getMolad(month, year):
        monthAdj = month - 7

        if (monthAdj < 0):
            monthAdj += JDate.monthsJYear(year)

        totalMonths = int(
            monthAdj + 235 * int((year - 1) / 19) + 12 * ((year - 1) % 19) + ((((year - 1) % 19) * 7) + 1) / 19)
        partsElapsed = 204 + (793 * (totalMonths % 1080))
        hoursElapsed = 5 + (12 * totalMonths) + 793 * int(totalMonths / 1080) + int(partsElapsed / 1080) - 6
        parts = int((partsElapsed % 1080) + 1080 * (hoursElapsed % 24))

        return dict(JDate=JDate.fromordinal((1 + (29 * int(totalMonths))) + int((hoursElapsed / 24))),
                    time=HourMinute(int(hoursElapsed) % 24, int((parts % 1080) / 18)), chalakim=parts % 18)

    #  Returns the time of the molad as a string in the format: Monday Night, 8:33 PM and 12 Chalakim
    #  The molad is always in Jerusalem so we use the Jerusalem sunset times
    #  to determine whether to display "Night" or "Motzai Shabbos" etc. (check this...)
    @staticmethod
    def getString(year, month):
        molad = Molad.getMolad(month, year)
        zmanim = Zmanim(date=molad['JDate'])
        _, nightfall = zmanim.getSunTimes()
        isNight = molad['time'].totalMinutes() >= nightfall.totalMinutes()
        dow = molad['JDate'].getdow()
        text = ''

        if (dow == 6 and isNight):
            text += "Motzai Shabbos,"
        elif (dow == 5 and isNight):
            text += "Shabbos Night,"
        else:
            text += Utils.dowEng[dow] + (" Night" if isNight else "")

        text += " " + str(molad['time']) + " and " + str(molad['chalakim']) + " Chalakim"

        return text

    #  Returns the time of the molad as a string in the format: ליל שני 20:33 12 חלקים
    #  The molad is always in Jerusalem so we use the Jerusalem sunset times
    #  to determine whether to display "ליל/יום" or "מוצאי שב"ק" etc.
    def getStringHeb(year, month):
        molad = Molad.getMolad(month, year)
        nightfall = molad.JDate.getSunriseSunset(Location.getJerusalem()).sunset
        isNight = Utils.totalMinutes(Utils.timeDiff(molad.time, nightfall)) >= 0
        dow = molad.JDate.getdow()
        text = ''

        if (dow == 6):
            text += ('מוצאי שב״ק' if isNight else 'יום שב״ק')
        elif (dow == 5):
            text += ('ליל שב״ק' if isNight else 'ערב שב״ק')
        else:
            text += ('ליל' if isNight else 'יום') + Utils.dowHeb[dow].replace("יום", "")
        str += " " + Utils.getTimeString(molad.time, True) + " " + molad.chalakim.tostring() + " חלקים"

        return str


if __name__ == '__main__':
    print(Molad.getString(5776, 4))
