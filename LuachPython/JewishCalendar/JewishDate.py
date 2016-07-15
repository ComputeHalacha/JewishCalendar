import datetime
from collections import namedtuple

import JewishCalendar.Utils as Utils


class JewishDate:
    # To save on repeat calculations, a "cache" of years that have had their
    # elapsed days previously calculated
    # by the tDays function is kept in memory.  ("memoizing")
    # Format of each entry is a tuple of (year, elapsed)
    __yearCache = {}

    def __init__(self, year, month, day, ordinal):
        self.year = year
        self.month = month
        self.day = day
        self.ordinal = ordinal

    def __str__(self):
        return self.toString()

    def __repr__(self):
        return 'JewishDate(year=%r, month=%r, day=%r, ordinal=%r)' % (self.year, self.month, self.day, self.ordinal)

    def __int__(self):
        return self.ordinal

    def __gt__(self, other):
        return self.ordinal > other.ordinal

    def __lt__(self, other):
        return self.ordinal < other.ordinal

    def __eq__(self, other):
        return self.ordinal == other.ordinal

    def __ne__(self, other):
        return self.ordinal != other.ordinal

    def __add__(self, other):
        return self.addDays(other)

    def __sub__(self, other):
        return self.addDays(-other)

    def getDayOfWeek(self):
        return self.ordinal % 7

    def addDays(self, num):
        return JewishDate.fromordinal(self.ordinal + num)

    # Gets the number of days separating this Jewish Date and the given one.
    # If the given date is before this one, the number will be negative.
    def diffDays(self, jd):
        return jd.ordinal - self.ordinal

    # Gets the day of the omer for the current Jewish date.
    # If the date is not during sefira, 0 is returned.
    def getDayOfOmer(self):
        dayOfOmer = 0
        if ((self.month == 1 and self.day > 15) or self.month == 2 or (self.month == 3 and self.day < 6)):
            dayOfOmer = JewishDate(self.year, 1, 15).diffDays(self)
        return dayOfOmer

    # Returns the current Jewish date in the format: Thursday Kislev 3 5776
    def toString(self):
        return "{} {} {} {}".format(Utils.dowEng[self.getDayOfWeek()],
                                    Utils.jMonthsEng[self.month],
                                    str(self.day),
                                    str(self.year))

    # Returns the current Jewish date in the format: יום חמישי כ"א כסלו תשע"ו
    def toStringHeb(self):
        return "{} {} {} {}".format(Utils.dowHeb[self.getDayOfWeek()],
                                    Utils.toJNum(self.day),
                                    Utils.jMonthsHeb[self.month],
                                    Utils.toJNum(self.year % 1000))

    # Create a new JewishDate with the given Jewish Year, Month and Day
    @staticmethod
    def create(year, month, day):
        ordinal = JewishDate.toordinal(year, month, day)
        return JewishDate(year, month, day, ordinal)

    # Create a JewishDate for the given ordinal.
    # The ordinal is the number of days elapsed since Teves 17, 3761
    # (12/31/0001 BCE)
    # This is also returned by pythons datetime.date.toordinal().
    @staticmethod
    def fromordinal(ordinal):
        # To save on calculations, start with a few years before date
        year = 3761 + int(ordinal / (366 if ordinal > 0 else 300))
        # Search forward for year from the approximation year.
        while (ordinal >= JewishDate.toordinal(year + 1, 7, 1)):
            year += 1
        # Search forward for month from either Tishrei or Nissan.
        month = (7 if ordinal < JewishDate.toordinal(year, 1, 1) else 1)
        while (ordinal > JewishDate.toordinal(year, month, JewishDate.daysJMonth(year, month))):
            month += 1
            # Calculate the day by subtraction.
        day = (ordinal - JewishDate.toordinal(year, month, 1) + 1)

        return JewishDate(year, month, day, ordinal)

    # Elapsed days since creation of the world until Rosh Hashana of the given
    # year
    @staticmethod
    def tDays(year):
        '''As this function is called many times, often on the same year for all types of calculations,
        we cache a list of years with their elapsed values.'''

        # If this year was already calculated and cached,
        # then we return the cached value.
        if year in JewishDate.__yearCache:
            return JewishDate.__yearCache[year]

        months = int((235 * int((year - 1) / 19)) +  # Leap months this cycle
                     (12 * ((year - 1) % 19)) +  # Regular months in this cycle.
                     (7 * ((year - 1) % 19) + 1) / 19)  # Months in complete cycles so far.
        parts = 204 + 793 * (months % 1080)
        hours = 5 + 12 * months + 793 * int(months / 1080) + int(parts / 1080)
        conjDay = int(1 + 29 * months + hours / 24)
        conjParts = 1080 * (hours % 24) + parts % 1080

        ''' at the end of a leap year -  15 hours, 589 parts or later... -
        ... or is on a Monday at... -  ...of a common year, -
        at 9 hours, 204 parts or later... - ...or is on a Tuesday... -
        If new moon is at or after midday,'''
        if ((conjParts >= 19440) or (
                ((conjDay % 7) == 2) and (conjParts >= 9924) and (not JewishDate.isJdLeapY(year))) or (
                ((conjDay % 7) == 1) and (conjParts >= 16789) and (JewishDate.isJdLeapY(year - 1)))):
            # Then postpone Rosh HaShanah one day
            altDay = (conjDay + 1)
        else:
            altDay = conjDay

        # A day is added if Rosh HaShanah would occur on Sunday, Friday or
        # Wednesday,
        if (altDay % 7) in [0, 3, 5]:
            altDay += 1

        # Add this year to the cache to save on calculations later on
        JewishDate.__yearCache[year] = altDay

        return altDay

    # Number of days in the given Jewish Year
    def daysJYear(year):
        return ((JewishDate.tDays(year + 1)) - (JewishDate.tDays(year)))

    # Number of days in the given Jewish Month.
    # Nissan is 1 and Adar Sheini is 13.
    @staticmethod
    def daysJMonth(year, month):
        if ((month == 2) or (month == 4) or (month == 6) or (
            (month == 8) and (not JewishDate.isLongCheshvan(year))) or (
            (month == 9) and JewishDate.isShortKislev(year)) or (month == 10) or (
            (month == 12) and (not JewishDate.isJdLeapY(year))) or (month == 13)):
            return 29
        else:
            return 30

    # Does Cheshvan for the given Jewish Year have 30 days?
    @staticmethod
    def isLongCheshvan(year):
        return (JewishDate.daysJYear(year) % 10) == 5

    # Does Kislev for the given Jewish Year have 29 days?
    @staticmethod
    def isShortKislev(year):
        return (JewishDate.daysJYear(year) % 10) == 3

    # Does the given Jewish Year have 13 months?
    @staticmethod
    def isJdLeapY(year):
        return (((7 * year) + 1) % 19) < 7

    # Number of months in Jewish Year
    @staticmethod
    def monthsJYear(year):
        return 13 if JewishDate.isJdLeapY(year) else 12

    @staticmethod
    def fromdate(date):
        return JewishDate.fromordinal(date.toordinal())

    def todate(self):
        return datetime.datetime.fromordinal(self.ordinal)

    @staticmethod
    def today():
        return JewishDate.fromordinal(datetime.date.today().toordinal())

    # Return the proleptic ordinal of the JewishDate, where Teves 18, 3761
    # (1/1/0001) has ordinal 1.
    @staticmethod
    def toordinal(year, month, day):
        dayInYear = day  # Days so far this month.
        if (month < 7):  # Before Tishrei, so add days in prior months this year before and after
            # Nissan.
            m = 7
            while (m <= (JewishDate.monthsJYear(year))):
                dayInYear += JewishDate.daysJMonth(year, m)
                m += 1

            m = 1
            while (m < month):
                dayInYear += JewishDate.daysJMonth(year, m)
                m += 1

        else:  # Add days in prior months this year
            m = 7
            while (m < month):
                dayInYear += JewishDate.daysJMonth(year, m)
                m += 1

        # Days elapsed before ordinal date 1.  - Days in prior years.
        return dayInYear + (JewishDate.tDays(year) + (-1373429))

    # Gets an list of holidays, fasts and any other special
    # specifications for the current Jewish date.
    # Each item is a namedtuple instance of type Entry(heb, eng)
    def getHolidays(self, israel):
        Entry = namedtuple('Entry', 'heb eng')
        list = []
        jYear = self.year
        jMonth = self.month
        jDay = self.day
        dayOfWeek = self.getDayOfWeek()
        isLeapYear = JewishDate.isJdLeapY(jYear)
        secDate = self.todate()

        if dayOfWeek == 5:
            list.append(Entry("ערב שבת", "Erev Shabbos"))
        elif (dayOfWeek == 6):
            list.append(Entry("שבת קודש", "Shabbos Kodesh"))

            if (jMonth == 1 and jDay > 7 and jDay < 15):
                list.append(Entry("שבת הגדול", "Shabbos HaGadol"))
            elif (jMonth == 7 and jDay > 2 and jDay < 10):
                list.append(Entry("שבת שובה", "Shabbos Shuva"))
            elif (jMonth == 5 and jDay > 2 and jDay < 10):
                list.append(Entry("שבת חזון", "Shabbos Chazon"))
            elif ((jMonth == (isLeapYear and 12 or 11) and jDay > 23 and jDay < 30) or (
                    jMonth == (isLeapYear and 13 or 12) and jDay == 1)):
                list.append(Entry("פרשת שקלים", "Parshas Shkalim"))
            elif (jMonth == (isLeapYear and 13 or 12) and jDay > 7 and jDay < 14):
                list.append(Entry("פרשת זכור", "Parshas Zachor"))
            elif (jMonth == (isLeapYear and 13 or 12) and jDay > 16 and jDay < 24):
                list.append(Entry("פרשת פרה", "Parshas Parah"))
            elif ((jMonth == (isLeapYear and 13 or 12) and jDay > 23 and jDay < 30) or
                      (jMonth == 1 and jDay == 1)):
                list.append(Entry("פרשת החודש", "Parshas Hachodesh"))

            # All months but Tishrei have Shabbos Mevarchim on the Shabbos
            # before Rosh Chodesh
            if (jMonth != 6 and jDay > 22 and jDay < 30):
                list.append(Entry("מברכים החודש", "Shabbos Mevarchim"))
        if (jDay == 30):
            monthIndex = (1 if (jMonth == 12 and not isLeapYear) or jMonth == 13 else jMonth + 1)
            list.append(Entry("ראש חודש " + Utils.jMonthsHeb[monthIndex],
                              "Rosh Chodesh " + Utils.jMonthsEng[monthIndex]))
        elif (jDay == 1 and jMonth != 7):
            list.append(Entry("ראש חודש " + Utils.jMonthsHeb[jMonth],
                              "Rosh Chodesh " + Utils.jMonthsEng[jMonth]))

        # V'sain Tal U'Matar in Chutz La'aretz is according to the secular date
        if (dayOfWeek != 6 and (not israel) and secDate.month == 12):
            sday = secDate.day
            # The three possible dates for starting vt"u are the 5th, 6th and
            # 7th of December
            if (sday in [5, 6, 7]):
                nextYearIsLeap = JewishDate.isJdLeapY(jYear + 1)
                # If next year is not a leap year, then vst"u starts on the
                # 5th.
                # If next year is a leap year than vst"u starts on the 6th.
                # If the 5th or 6th were shabbos than vst"u starts on Sunday.
                if ((((sday == 5 or (sday == 6 and dayOfWeek == 0)) and (not nextYearIsLeap))) or (
                    (sday == 6 or (sday == 7 and dayOfWeek == 0)) and nextYearIsLeap)):
                    list.append(Entry("ותן טל ומטר", "V'sain Tal U'Matar"))
        if jMonth == 1:  # Nissan
            if (jDay == 12 and dayOfWeek == 4):
                list.append(Entry("בדיקת חמץ", "Bedikas Chametz"))
            elif (jDay == 13 and dayOfWeek != 5):
                list.append(Entry("בדיקת חמץ", "Bedikas Chametz"))
            elif (jDay == 14):
                list.append(Entry("ערב פסח", "Erev Pesach"))
            elif (jDay == 15):
                list.append(Entry("פסח - יום ראשון", "First Day of Pesach"))
            elif (jDay == 16):
                list.append(Entry("פסח - חול המועד", 'Pesach  - Chol HaMoed') if israel else
                            Entry("פסח - יום שני", "Pesach - Second Day"))
            elif (jday in [17, 18, 19]):
                list.append(Entry("פסח - חול המועד", "Pesach - Chol Ha'moed"))
            elif (jDay == 20):
                list.append(Entry("פסח - חול המועד - ערב יו\"ט", "Pesach - Chol Ha'moed - Erev Yomtov"))
            elif (jDay == 21):
                list.append(Entry("שביעי של פסח", "7th Day of Pesach"))
            elif (jDay == 22 and not israel):
                list.append(Entry("אחרון של פסח", "Last Day of Pesach"))
        elif jMonth == 2:  # Iyar
            if (dayOfWeek == 1 and jDay > 2 and jDay < 12):
                list.append(Entry("תענית שני קמא", "Baha\"b"))
            elif (dayOfWeek == 4 and jDay > 5 and jDay < 13):
                list.append(Entry('תענית חמישי', 'Baha\"b'))
            elif (dayOfWeek == 1 and jDay > 9 and jDay < 17):
                list.append(Entry('תענית שני בתרא', 'Baha"b'))
            if (jDay == 14):
                list.append(Entry("פסח שני", "Pesach Sheini"))
            elif (jDay == 18):
                list.append(Entry('ל"ג בעומר', "Lag BaOmer"))
        elif jMonth == 3:  # Sivan
            if (jDay == 5):
                list.append(Entry("ערב שבועות", "Erev Shavuos"))
            elif (jDay == 6):
                list.append(Entry('חג השבועות', 'Shavuos') if israel else
                            Entry('שבועות - יום ראשון', 'Shavuos - First Day'))
            elif (jDay == 7 and not israel):
                list.append(Entry("שבועות - יום שני", "Shavuos - Second Day"))
        elif jMonth == 4:  # Tamuz
            if (jDay == 17 and dayOfWeek != 6):
                list.append(Entry('צום י"ז בתמוז', "Fast - 17th of Tammuz"))
            elif (jDay == 18 and dayOfWeek == 0):
                list.append(Entry('צום י"ז בתמוז', "Fast - 17th of Tammuz"))
        elif jMonth == 5:  # Av
            if (jDay == 9 and dayOfWeek != 6):
                list.append(Entry("תשעה באב", "Tisha B'Av"))
            elif (jDay == 10 and dayOfWeek == 0):
                list.append(Entry("תשעה באב", "Tisha B'Av"))
            elif (jDay == 15):
                list.append(Entry('ט"ו באב', "Tu B'Av"))
        elif jMonth == 6:  # Ellul
            if (jDay == 29):
                list.append(Entry("ערב ראש השנה", "Erev Rosh Hashana"))
        elif jMonth == 7:  # Tishrei
            if (jDay == 1):
                list.append(Entry("ראש השנה", "Rosh Hashana - First Day"))
            elif (jDay == 2):
                list.append(Entry("ראש השנה", "Rosh Hashana - Second Day"))
            elif (jDay == 3 and dayOfWeek != 6):
                list.append(Entry("צום גדליה", "Tzom Gedalia"))
            elif (jDay == 4 and dayOfWeek == 0):
                list.append(Entry("צום גדליה", "Tzom Gedalia"))
            elif (jDay == 9):
                list.append(Entry("ערב יום הכיפורים", "Erev Yom Kippur"))
            elif (jDay == 10):
                list.append(Entry("יום הכיפורים", "Yom Kippur"))
            elif (jDay == 14):
                list.append(Entry("ערב חג הסוכות", "Erev Sukkos"))
            elif (jDay == 15):
                list.append(Entry("חג הסוכות", "First Day of Sukkos"))
            elif (jDay == 16):
                list.append(Entry('סוכות - חול המועד', 'Sukkos - Chol HaMoed') if israel else
                            Entry('חג הסוכות - יום שני', 'Sukkos - Second Day'))
            elif (jDay in [17, 18, 19, 20]):
                list.append(Entry("סוכות - חול המועד", "Sukkos - Chol HaMoed"))
            elif (jDay == 21):
                list.append(Entry('הושענא רבה - ערב יו"ט', "Hoshana Rabba - Erev Yomtov"))
            elif (jDay == 22):
                list.append(Entry("שמיני עצרת", "Shmini Atzeres"))
                if (israel):
                    list.append(Entry("שמחת תורה", "Simchas Torah"))
            elif (jDay == 23 and not israel):
                list.append(Entry("שמחת תורה", "Simchas Torah"))
        elif jMonth == 8:  # Cheshvan
            if (dayOfWeek == 1 and jDay > 2 and jDay < 12):
                list.append(Entry("תענית שני קמא", 'Baha"b'))
            elif (dayOfWeek == 4 and jDay > 5 and jDay < 13):
                list.append(Entry("תענית חמישי", 'Baha"b'))
            elif (dayOfWeek == 1 and jDay > 9 and jDay < 17):
                list.append(Entry("תענית שני בתרא", 'Baha"b'))
            if (jDay == 7 and israel):
                list.append(Entry("ותן טל ומטר", "V'sain Tal U'Matar"))
        elif jMonth == 9:  # Kislev
            if (jDay == 25):
                list.append(Entry("'חנוכה - נר א", "Chanuka - One Candle"))
            elif (jDay == 26):
                list.append(Entry("'חנוכה - נר ב", "Chanuka - Two Candles"))
            elif (jDay == 27):
                list.append(Entry("'חנוכה - נר ג", "Chanuka - Three Candles"))
            elif (jDay == 28):
                list.append(Entry("'חנוכה - נר ד", "Chanuka - Four Candles"))
            elif (jDay == 29):
                list.append(Entry("'חנוכה - נר ה", "Chanuka - Five Candles"))
            elif (jDay == 30):
                list.append(Entry("'חנוכה - נר ו", "Chanuka - Six Candles"))
        elif jMonth == 10:  # Teves
            if (JewishDate.isShortKislev(jYear)):
                if (jDay == 1):
                    list.append(Entry("'חנוכה - נר ו", "Chanuka - Six Candles"))
                elif (jDay == 2):
                    list.append(Entry("'חנוכה - נר ז", "Chanuka - Seven Candles"))
                elif (jDay == 3):
                    list.append(Entry("'חנוכה - נר ח", "Chanuka - Eight Candles"))
            else:
                if (jDay == 1):
                    list.append(Entry("'חנוכה - נר ז", "Chanuka - Seven Candles"))
                elif (jDay == 2):
                    list.append(Entry("'חנוכה - נר ח", "Chanuka - Eight Candles"))
            if (jDay == 10):
                list.append(Entry("צום עשרה בטבת", "Fast - 10th of Teves"))
        elif jMonth == 11:  # Shvat
            if (jDay == 15):
                list.append(Entry("ט\"ו בשבט", "Tu B'Shvat"))
            elif jMonth in [12, 13]:  # Both Adars
                if (jMonth == 12 and isLeapYear):  # Adar Rishon in a leap year
                    if (jDay == 14):
                        list.append(Entry("פורים קטן", "Purim Katan"))
                    elif (jDay == 15):
                        list.append(Entry("שושן פורים קטן", "Shushan Purim Katan"))
                else:  # The "real" Adar: the only one in a non-leap-year or Adar Sheini
                    if (jDay == 11 and dayOfWeek == 4):
                        list.append(Entry("תענית אסתר", "Fast - Taanis Esther"))
                    elif (jDay == 13 and dayOfWeek != 6):
                        list.append(Entry("תענית אסתר", "Fast - Taanis Esther"))
                    elif (jDay == 14):
                        list.append(Entry("פורים", "Purim"))
                    elif (jDay == 15):
                        list.append(Entry("שושן פורים", "Shushan Purim"))
        # If it is during Sefiras Ha'omer
        if ((jMonth == 1 and jDay > 15) or jMonth == 2 or (jMonth == 3 and jDay < 6)):
            dayOfSefirah = self.getDayOfOmer()
            if (dayOfSefirah > 0):
                list.append(Entry("ספירת העומר - יום " + dayOfSefirah.toString(),
                                  "Sefiras Ha'omer - Day " + dayOfSefirah.toString()))

        return list

    # Is the current Jewish Date the day before a yomtov that contains a
    # Friday?
    def hasEiruvTavshilin(self, israel):
        dow = self.getDayOfWeek()
        # No Eiruv Tavshilin ever on Sunday, Monday, Tuesday, Friday or Shabbos
        # If it is Erev Yomtov
        # - but not erev yom kippur
        # Erev rosh hashana on Wednesday OR erev yomtov in Chu"l on wednesday
        # or Thursday OR erev yomtov in Israel on Thursday
        return (dow not in [0, 1, 2, 5, 6]) and \
               self.hasCandleLighting() and \
               self.day != 9 and \
               (self.month == 6 or ((not israel) and dow in [3, 4]) or (israel and dow == 4))

    # Does the current Jewish date have candle lighting before sunset?
    def hasCandleLighting(self):
        dow = self.getDayOfWeek()
        if (dow == 5):
            return True
        elif (dow == 6):
            # there is no "candle lighting time" - even if yom tov is on Motzai
            # Shabbos
            return False

        return (self.month == 1 and self.day in [14, 20] or \
                (self.month == 3 and self.day == 5) or \
                (self.month == 6 and self.day == 29) or \
                (self.month == 7 and self.day in [9, 14, 21]))

    # Gets the candle lighting time for the current Jewish date for the given
    # Location.
    def getCandleLighting(self, location):
        if self.hasCandleLighting():
            from JewishCalendar.Zmanim import Zmanim
            return Zmanim(location, self).getCandleLighting()
        else:
            return None


if __name__ == '__main__':
    # to print todays Jewish Date and any Holidays for today in Hebrew
    jd = JewishDate.today()
    print(jd.toStringHeb(), jd.getHolidays(True))
