import datetime
from collections import namedtuple

import jcal.utils as Utils


class JDate:
    # To save on repeat calculations, a "cache" of years that have had their
    # elapsed days previously calculated
    # by the tDays function is kept in memory.  ("memoizing")
    # Format of each entry is a tuple of (year, elapsed)
    __yearCache = {}

    def __init__(self, year, month, day, ordinal=None):
        if year < 0 or year > 5999:
            raise ValueError('year cannot be less than 1 or more than 5999')
        if month < 0 or month > 13:
            raise ValueError('month can only be from 1 to 13')
        if day < 0 or day > 30:
            raise ValueError('day can only be from 1 to 30')
        self.year = year
        # The months_in_jyear calculation is relatively light, so we do a check to make sure the month number makes sense
        months_in_year = JDate.months_in_jyear(year)
        if month > months_in_year:
            raise ValueError('there are only {} months in the year {}'.format(months_in_year, year))
        self.month = month
        # to allow quicker JDate initialization, we don't check the maximum number of days in month
        self.day = day
        self.ordinal = ordinal or JDate.toordinal(year, month, day)

    def __str__(self):
        return self.tostring()

    def __repr__(self):
        return 'JDate(year=%r, month=%r, day=%r, ordinal=%r)' % (self.year, self.month, self.day, self.ordinal)

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
        return self.add_days(other)

    def __sub__(self, other):
        return self.add_days(-other)

    def getdow(self):
        return self.ordinal % 7

    def add_days(self, num):
        return JDate.fromordinal(self.ordinal + num)

    # Gets the number of days separating this Jewish Date and the given one.
    # If the given date is before this one, the number will be negative.
    def diff_days(self, jd):
        return jd.ordinal - self.ordinal

    # Gets the day of the omer for the current Jewish date.
    # If the date is not during sefira, 0 is returned.
    def get_omer(self):
        dayOfOmer = 0
        if ((self.month == 1 and self.day > 15) or self.month == 2 or (self.month == 3 and self.day < 6)):
            dayOfOmer = JDate(self.year, 1, 15).diff_days(self)
        return dayOfOmer

    # Returns the current Jewish date in the format: Thursday Kislev 3 5776
    def tostring(self):
        return "{} {} {} {}".format(Utils.dowEng[self.getdow()],
                                    Utils.jMonthsEng[self.month],
                                    str(self.day),
                                    str(self.year))

    # Returns the current Jewish date in the format: יום חמישי כ"א כסלו תשע"ו
    def tostring_heb(self):
        return "{} {} {} {}".format(Utils.dowHeb[self.getdow()],
                                    Utils.to_jnum(self.day),
                                    Utils.jMonthsHeb[self.month],
                                    Utils.to_jnum(self.year % 1000))

    # Create a new JDate with the given Jewish Year, Month and Day
    @staticmethod
    def create(year, month, day):
        ordinal = JDate.toordinal(year, month, day)
        return JDate(year, month, day, ordinal)

    # Create a JDate for the given ordinal.
    # The ordinal is the number of days elapsed since Teves 17, 3761
    # (12/31/0001 BCE)
    # This is also returned by pythons datetime.date.toordinal().
    @staticmethod
    def fromordinal(ordinal):
        # To save on calculations, start with a few years before date
        year = 3761 + int(ordinal / (366 if ordinal > 0 else 300))
        # Search forward for year from the approximation year.
        while (ordinal >= JDate.toordinal(year + 1, 7, 1)):
            year += 1
        # Search forward for month from either Tishrei or Nissan.
        month = (7 if ordinal < JDate.toordinal(year, 1, 1) else 1)
        while (ordinal > JDate.toordinal(year, month, JDate.days_in_jmonth(year, month))):
            month += 1
            # Calculate the day by subtraction.
        day = (ordinal - JDate.toordinal(year, month, 1) + 1)

        return JDate(year, month, day, ordinal)

    # Elapsed days since creation of the world until Rosh Hashana of the given
    # year
    @staticmethod
    def tdays(year):
        '''As this function is called many times, often on the same year for all types of calculations,
        we cache a list of years with their elapsed values.'''

        # If this year was already calculated and cached,
        # then we return the cached value.
        if year in JDate.__yearCache:
            return JDate.__yearCache[year]

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
                        ((conjDay % 7) == 2) and (conjParts >= 9924) and (not JDate.isleap_jyear(year))) or (
                        ((conjDay % 7) == 1) and (conjParts >= 16789) and (JDate.isleap_jyear(year - 1)))):
            # Then postpone Rosh HaShanah one day
            altDay = (conjDay + 1)
        else:
            altDay = conjDay

        # A day is added if Rosh HaShanah would occur on Sunday, Friday or
        # Wednesday,
        if (altDay % 7) in [0, 3, 5]:
            altDay += 1

        # Add this year to the cache to save on calculations later on
        JDate.__yearCache[year] = altDay

        return altDay

    # Number of days in the given Jewish Year
    def days_in_jyear(year):
        return ((JDate.tdays(year + 1)) - (JDate.tdays(year)))

    # Number of days in the given Jewish Month.
    # Nissan is 1 and Adar Sheini is 13.
    @staticmethod
    def days_in_jmonth(year, month):
        if ((month == 2) or (month == 4) or (month == 6) or (
            (month == 8) and (not JDate.has_long_cheshvan(year))) or (
                    (month == 9) and JDate.has_short_kislev(year)) or (month == 10) or (
                    (month == 12) and (not JDate.isleap_jyear(year))) or (month == 13)):
            return 29
        else:
            return 30

    # Does Cheshvan for the given Jewish Year have 30 days?
    @staticmethod
    def has_long_cheshvan(year):
        return (JDate.days_in_jyear(year) % 10) == 5

    # Does Kislev for the given Jewish Year have 29 days?
    @staticmethod
    def has_short_kislev(year):
        return (JDate.days_in_jyear(year) % 10) == 3

    # Does the given Jewish Year have 13 months?
    @staticmethod
    def isleap_jyear(year):
        return (((7 * year) + 1) % 19) < 7

    # Number of months in Jewish Year
    @staticmethod
    def months_in_jyear(year):
        return 13 if JDate.isleap_jyear(year) else 12

    @staticmethod
    def fromdate(date):
        return JDate.fromordinal(date.toordinal())

    def todate(self):
        return datetime.datetime.fromordinal(self.ordinal)

    @staticmethod
    def today():
        return JDate.fromordinal(datetime.date.today().toordinal())

    # Return the proleptic ordinal of the JDate, where Teves 18, 3761
    # (1/1/0001) has ordinal 1.
    @staticmethod
    def toordinal(year, month, day):
        dayInYear = day  # Days so far this month.
        if (month < 7):  # Before Tishrei, so add days in prior months this year before and after
            # Nissan.
            m = 7
            while (m <= (JDate.months_in_jyear(year))):
                dayInYear += JDate.days_in_jmonth(year, m)
                m += 1

            m = 1
            while (m < month):
                dayInYear += JDate.days_in_jmonth(year, m)
                m += 1

        else:  # Add days in prior months this year
            m = 7
            while (m < month):
                dayInYear += JDate.days_in_jmonth(year, m)
                m += 1

        # Days elapsed before ordinal date 1.  - Days in prior years.
        return dayInYear + (JDate.tdays(year) + (-1373429))

    # Gets an list of holidays, fasts and any other special
    # specifications for the current Jewish date.
    # Each item is a namedtuple instance of type Entry(heb, eng)
    def get_holidays(self, israel):
        Entry = namedtuple('Entry', 'heb eng')
        list = []
        jYear = self.year
        jMonth = self.month
        jDay = self.day
        dayOfWeek = self.getdow()
        isleap_jyear = JDate.isleap_jyear(jYear)
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
            elif ((jMonth == (isleap_jyear and 12 or 11) and jDay > 23 and jDay < 30) or (
                            jMonth == (isleap_jyear and 13 or 12) and jDay == 1)):
                list.append(Entry("פרשת שקלים", "Parshas Shkalim"))
            elif (jMonth == (isleap_jyear and 13 or 12) and jDay > 7 and jDay < 14):
                list.append(Entry("פרשת זכור", "Parshas Zachor"))
            elif (jMonth == (isleap_jyear and 13 or 12) and jDay > 16 and jDay < 24):
                list.append(Entry("פרשת פרה", "Parshas Parah"))
            elif ((jMonth == (isleap_jyear and 13 or 12) and jDay > 23 and jDay < 30) or
                      (jMonth == 1 and jDay == 1)):
                list.append(Entry("פרשת החודש", "Parshas Hachodesh"))

            # All months but Tishrei have Shabbos Mevarchim on the Shabbos
            # before Rosh Chodesh
            if (jMonth != 6 and jDay > 22 and jDay < 30):
                list.append(Entry("מברכים החודש", "Shabbos Mevarchim"))
        if (jDay == 30):
            monthIndex = (1 if (jMonth == 12 and not isleap_jyear) or jMonth == 13 else jMonth + 1)
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
                nextYearIsLeap = JDate.isleap_jyear(jYear + 1)
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
            if (JDate.has_short_kislev(jYear)):
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
                if (jMonth == 12 and isleap_jyear):  # Adar Rishon in a leap year
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
            dayOfSefirah = self.get_omer()
            if (dayOfSefirah > 0):
                list.append(Entry("ספירת העומר - יום " + dayOfSefirah.tostring(),
                                  "Sefiras Ha'omer - Day " + dayOfSefirah.tostring()))

        return list

    # Is the current Jewish Date the day before a yomtov that contains a
    # Friday?
    def has_eiruv_tavshilin(self, israel):
        dow = self.getdow()
        # No Eiruv Tavshilin ever on Sunday, Monday, Tuesday, Friday or Shabbos
        # If it is Erev Yomtov
        # - but not erev yom kippur
        # Erev rosh hashana on Wednesday OR erev yomtov in Chu"l on wednesday
        # or Thursday OR erev yomtov in Israel on Thursday
        return (dow not in [0, 1, 2, 5, 6]) and \
               self.has_candle_lighting() and \
               self.day != 9 and \
               (self.month == 6 or ((not israel) and dow in [3, 4]) or (israel and dow == 4))

    # Does the current Jewish date have candle lighting before sunset?
    def has_candle_lighting(self):
        dow = self.getdow()
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
    def get_candle_lighting(self, location):
        if self.has_candle_lighting():
            from jcal.zmanim import Zmanim
            return Zmanim(location, self).get_candle_lighting()
        else:
            return None


if __name__ == '__main__':
    # to print todays Jewish Date and any Holidays for today in Hebrew
    jd = JDate.today()
    print(jd.tostring_heb(), jd.get_holidays(True))
