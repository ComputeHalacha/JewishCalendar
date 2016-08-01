import datetime
import sys
from collections import namedtuple

import jcal.utils as utils

if "jcal.conversions" not in sys.modules.keys():
    # prevent circular referencing
    from jcal import conversions


class JDate:
    '''
    This class represents a single day in the Jewish Calendar and is the basic date unit for the pyluach library.

    To create an instance from a civil/secular/gregorian date, use the JDate.fromdate function.
    To get a civil/secular/gregorian date from a Jewish Date use the todate function.
    To get he current Jewish Date, use the JDate.today function.

    Addition and subtraction of days can be done on Jewish Dates using regular operators.
    Comparison operators can also be used.

    This implementation of Jewish Dates is based on the "ordinal" -
    which is the number of days that have passed since the Gregorian Date 12/31/0001 BCE.
    As there is no Gregorian year zero, the date 1/1/0001 CE is ordinal day number 1.
    Conveniently, Pythons datetime.date also uses the ordinal for it's underlying property.
    This facilitates quick conversions and an underlying similarity to Pythons built-in date classes.
    The one caveat of this is that the built-in date classes do not allow years prior to Gregorian year number 1,
    while the Jewish calendar begins some 3,760 years earlier.

    Some of the calculations used are based on the algorithms in "Calendrical Calculations"
    by Nachum Dershowitz and Edward M. Reingold
    '''

    # To save on repeat calculations, a "cache" of years that have had their elapsed days previously calculated
    # by the tdays function is kept in memory.  ("memoizing").
    _yearCache = {}

    def __init__(self, year, month, day, ordinal=None):
        """
        :param year: The 4 digit Jewish Year
        :type year: int
        :param month: The Jewish Month. Nissan is 1 and Adar Sheini is 13
        :type month: int
        :param day: The day of he Jewish Month
        :type day: int
        :param ordinal: The number of days elapsed since the beginning of the Gregorian Calendar
        :type ordinal: int
        """
        if not 0 < year < 6000:
            raise ValueError('year cannot be less than 1 or more than 5999')
        if not 0 < month <= 13:
            raise ValueError('month can only be from 1 to 13')
        if not 0 < day <= 30:
            raise ValueError('day can only be from 1 to 30')
        self._year = year
        # The months_in_jyear calculation is relatively light, so it shouldn't hit performance
        # if we do a check to make sure the month number makes sense
        months_in_year = JDate.months_in_jyear(self._year)
        if month > months_in_year:
            raise ValueError('there are only {} months in the year {}'.format(months_in_year, year))
        self._month = month
        # to allow quicker JDate initialization, we don't check the maximum number of days in month
        self._day = day
        self._ordinal = ordinal or JDate.toordinal(self._year, self._month, self._day)

    def __str__(self):
        return self.tostring()

    def __repr__(self):
        return 'JDate(year=%r, month=%r, day=%r, ordinal=%r)' % (self._year, self._month, self._day, self._ordinal)

    def __int__(self):
        return self._ordinal

    def __gt__(self, other):
        return self._ordinal > other._ordinal

    def __lt__(self, other):
        return self._ordinal < other._ordinal

    def __eq__(self, other):
        return self._ordinal == other._ordinal

    def __ne__(self, other):
        return self._ordinal != other._ordinal

    def __add__(self, other):
        return self.add_days(other)

    def __sub__(self, other):
        return self.add_days(-other)

    # Read-only field accessors
    @property
    def year(self):
        return self._year

    @property
    def month(self):
        return self._month

    @property
    def day(self):
        return self._day

    @property
    def ordinal(self):
        return self._ordinal

    def getdow(self):
        return self._ordinal % 7

    def add_days(self, num):
        return JDate.fromordinal(self._ordinal + num)

    # Gets the number of days separating this Jewish Date and the given one.
    # If the given date is before this one, the number will be negative.
    def diff_days(self, jd):
        return jd._ordinal - self._ordinal

    # Gets the day of the omer for the current Jewish date.
    # If the date is not during sefira, 0 is returned.
    def get_omer(self):
        dayOfOmer = 0
        if ((self._month == 1 and self._day > 15) or self._month == 2 or (self._month == 3 and self._day < 6)):
            dayOfOmer = JDate(self._year, 1, 15).diff_days(self)
        return dayOfOmer

    # Returns the current Jewish date in the format: Thursday Kislev 3 5776
    def tostring(self):
        return "{} {} {} {}".format(utils.dow_eng[self.getdow()],
                                    utils.jmonths_eng[self._month],
                                    str(self._day),
                                    str(self._year))

    # Returns the current Jewish date in the format: יום חמישי כ"א כסלו תשע"ו
    def tostring_heb(self):
        hundreds = divmod(self._year, 1000)
        return "{} {} {} {} אלפים {}".format(utils.dowHeb[self.getdow()],
                                             utils.to_jnum(self._day),
                                             utils.jmonths_heb[self._month],
                                             utils.to_jnum(hundreds[0]),
                                             utils.to_jnum(hundreds[1] if hundreds[1] else ''))

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
        """
        Creates a JDate from the given ordinal.
        :param ordinal: The number of days since the day before the beginning of the Gregorian calendar.
        Which is December 31, 1 BCE. Note, there is no year 0 so, ordinal number 1 is January 1st 0001 CE
        :type ordinal: int
        :return: The jewish date on the day of with the given ordinal
        :rtype: JDate
        """
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
        if year in JDate._yearCache:
            return JDate._yearCache[year]

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
        JDate._yearCache[year] = altDay

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

    # Converts a gregorian date to a Jewish date.
    @staticmethod
    def fromdate(date_or_year, month=None, day=None):
        """
        date_or_year can be:
          - a datetime.date object
          - a utils.GregorianDate namedtuple
          - an int representing the yearl
         """

        return conversions.greg_to_jdate(date_or_year, month, day)

    # Returns the civil/secular/Gregorian date of this Jewish Date
    def todate(self):
        """
        :return: the civil/secular/Gregorian date of this Jewish Date
        :rtype: utils.GregorianDate namedtuple of (year, month, day)

        By not returning a Python datetime.date, we can allow return dates before the common era -
        which the built-in datetime classes do not allow.
        """

        return conversions.jdate_to_greg(self)

    # Return the current Jewish Date
    @staticmethod
    def today():
        """
        :return: The current Jewish Date. Based on the system clock.
        :rtype: JDate

        We use datetime.date.today to get current ordinal even though the built-in date class
        is restricted to the common era.
        I think that it's pretty safe to assume that the current system date is after 12/31/0001 BCE.
        Anyone sophisticated enough to be doing time traveling, will probably have better tools than this code.
        """
        return JDate.fromordinal(datetime.date.today().toordinal())

    # Return the proleptic ordinal of the JDate, where Teves 18, 3761 (1/1/0001 CE) is day 1.
    # As there is no year 0 in the theoretical early Gregorian calendar, day 0 is 12/31/0001 BCE
    @staticmethod
    def toordinal(year, month, day):
        day_in_year = day  # Days so far this month.
        if month < 7:  # Before Tishrei, so add days in prior months this year before and after Nissan.
            m = 7
            while m <= JDate.months_in_jyear(year):
                day_in_year += JDate.days_in_jmonth(year, m)
                m += 1
            m = 1
            while m < month:
                day_in_year += JDate.days_in_jmonth(year, m)
                m += 1
        else:  # Add days in prior months this year
            m = 7
            while m < month:
                day_in_year += JDate.days_in_jmonth(year, m)
                m += 1
        # Days elapsed before ordinal date 1.  - Days in prior years.
        return day_in_year + (JDate.tdays(year) + (-1373429))

    # Gets an list of holidays, fasts and any other special
    # specifications for the current Jewish date.
    # Each item is a namedtuple instance of type Entry(heb, eng)
    def get_holidays(self, israel):
        Entry = namedtuple('Entry', 'heb eng')
        list = []
        j_year = self._year
        j_month = self._month
        j_day = self._day
        day_of_week = self.getdow()
        isleap_jyear = JDate.isleap_jyear(j_year)
        sec_date = self.todate()

        if day_of_week == 5:
            list.append(Entry("ערב שבת", "Erev Shabbos"))
        elif (day_of_week == 6):
            list.append(Entry("שבת קודש", "Shabbos Kodesh"))

            if (j_month == 1 and j_day > 7 and j_day < 15):
                list.append(Entry("שבת הגדול", "Shabbos HaGadol"))
            elif (j_month == 7 and j_day > 2 and j_day < 10):
                list.append(Entry("שבת שובה", "Shabbos Shuva"))
            elif (j_month == 5 and j_day > 2 and j_day < 10):
                list.append(Entry("שבת חזון", "Shabbos Chazon"))
            elif ((j_month == (isleap_jyear and 12 or 11) and j_day > 23 and j_day < 30) or (
                            j_month == (isleap_jyear and 13 or 12) and j_day == 1)):
                list.append(Entry("פרשת שקלים", "Parshas Shkalim"))
            elif (j_month == (isleap_jyear and 13 or 12) and j_day > 7 and j_day < 14):
                list.append(Entry("פרשת זכור", "Parshas Zachor"))
            elif (j_month == (isleap_jyear and 13 or 12) and j_day > 16 and j_day < 24):
                list.append(Entry("פרשת פרה", "Parshas Parah"))
            elif ((j_month == (isleap_jyear and 13 or 12) and j_day > 23 and j_day < 30) or
                      (j_month == 1 and j_day == 1)):
                list.append(Entry("פרשת החודש", "Parshas Hachodesh"))

            # All months but Tishrei have Shabbos Mevarchim on the Shabbos
            # before Rosh Chodesh
            if (j_month != 6 and j_day > 22 and j_day < 30):
                list.append(Entry("מברכים החודש", "Shabbos Mevarchim"))
        if (j_day == 30):
            month_index = (1 if (j_month == 12 and not isleap_jyear) or j_month == 13 else j_month + 1)
            list.append(Entry("ראש חודש " + utils.jmonths_heb[month_index],
                              "Rosh Chodesh " + utils.jmonths_eng[month_index]))
        elif (j_day == 1 and j_month != 7):
            list.append(Entry("ראש חודש " + utils.jmonths_heb[j_month],
                              "Rosh Chodesh " + utils.jmonths_eng[j_month]))

        # V'sain Tal U'Matar in Chutz La'aretz is according to the secular date
        if (day_of_week != 6 and (not israel) and sec_date.month == 12):
            sday = sec_date.day
            # The three possible dates for starting vt"u are the 5th, 6th and
            # 7th of December
            if (sday in [5, 6, 7]):
                nextYearIsLeap = JDate.isleap_jyear(j_year + 1)
                # If next year is not a leap year, then vst"u starts on the
                # 5th.
                # If next year is a leap year than vst"u starts on the 6th.
                # If the 5th or 6th were shabbos than vst"u starts on Sunday.
                if ((((sday == 5 or (sday == 6 and day_of_week == 0)) and (not nextYearIsLeap))) or (
                            (sday == 6 or (sday == 7 and day_of_week == 0)) and nextYearIsLeap)):
                    list.append(Entry("ותן טל ומטר", "V'sain Tal U'Matar"))
        if j_month == 1:  # Nissan
            if (j_day == 12 and day_of_week == 4):
                list.append(Entry("בדיקת חמץ", "Bedikas Chametz"))
            elif (j_day == 13 and day_of_week != 5):
                list.append(Entry("בדיקת חמץ", "Bedikas Chametz"))
            elif (j_day == 14):
                list.append(Entry("ערב פסח", "Erev Pesach"))
            elif (j_day == 15):
                list.append(Entry("פסח - יום ראשון", "First Day of Pesach"))
            elif (j_day == 16):
                list.append(Entry("פסח - חול המועד", 'Pesach  - Chol HaMoed') if israel else
                            Entry("פסח - יום שני", "Pesach - Second Day"))
            elif (j_day in [17, 18, 19]):
                list.append(Entry("פסח - חול המועד", "Pesach - Chol Ha'moed"))
            elif (j_day == 20):
                list.append(Entry("פסח - חול המועד - ערב יו\"ט", "Pesach - Chol Ha'moed - Erev Yomtov"))
            elif (j_day == 21):
                list.append(Entry("שביעי של פסח", "7th Day of Pesach"))
            elif (j_day == 22 and not israel):
                list.append(Entry("אחרון של פסח", "Last Day of Pesach"))
        elif j_month == 2:  # Iyar
            if (day_of_week == 1 and j_day > 2 and j_day < 12):
                list.append(Entry("תענית שני קמא", "Baha\"b"))
            elif (day_of_week == 4 and j_day > 5 and j_day < 13):
                list.append(Entry('תענית חמישי', 'Baha\"b'))
            elif (day_of_week == 1 and j_day > 9 and j_day < 17):
                list.append(Entry('תענית שני בתרא', 'Baha"b'))
            if (j_day == 14):
                list.append(Entry("פסח שני", "Pesach Sheini"))
            elif (j_day == 18):
                list.append(Entry('ל"ג בעומר', "Lag BaOmer"))
        elif j_month == 3:  # Sivan
            if (j_day == 5):
                list.append(Entry("ערב שבועות", "Erev Shavuos"))
            elif (j_day == 6):
                list.append(Entry('חג השבועות', 'Shavuos') if israel else
                            Entry('שבועות - יום ראשון', 'Shavuos - First Day'))
            elif (j_day == 7 and not israel):
                list.append(Entry("שבועות - יום שני", "Shavuos - Second Day"))
        elif j_month == 4:  # Tamuz
            if (j_day == 17 and day_of_week != 6):
                list.append(Entry('צום י"ז בתמוז', "Fast - 17th of Tammuz"))
            elif (j_day == 18 and day_of_week == 0):
                list.append(Entry('צום י"ז בתמוז', "Fast - 17th of Tammuz"))
        elif j_month == 5:  # Av
            if (j_day == 9 and day_of_week != 6):
                list.append(Entry("תשעה באב", "Tisha B'Av"))
            elif (j_day == 10 and day_of_week == 0):
                list.append(Entry("תשעה באב", "Tisha B'Av"))
            elif (j_day == 15):
                list.append(Entry('ט"ו באב', "Tu B'Av"))
        elif j_month == 6:  # Ellul
            if (j_day == 29):
                list.append(Entry("ערב ראש השנה", "Erev Rosh Hashana"))
        elif j_month == 7:  # Tishrei
            if (j_day == 1):
                list.append(Entry("ראש השנה", "Rosh Hashana - First Day"))
            elif (j_day == 2):
                list.append(Entry("ראש השנה", "Rosh Hashana - Second Day"))
            elif (j_day == 3 and day_of_week != 6):
                list.append(Entry("צום גדליה", "Tzom Gedalia"))
            elif (j_day == 4 and day_of_week == 0):
                list.append(Entry("צום גדליה", "Tzom Gedalia"))
            elif (j_day == 9):
                list.append(Entry("ערב יום הכיפורים", "Erev Yom Kippur"))
            elif (j_day == 10):
                list.append(Entry("יום הכיפורים", "Yom Kippur"))
            elif (j_day == 14):
                list.append(Entry("ערב חג הסוכות", "Erev Sukkos"))
            elif (j_day == 15):
                list.append(Entry("חג הסוכות", "First Day of Sukkos"))
            elif (j_day == 16):
                list.append(Entry('סוכות - חול המועד', 'Sukkos - Chol HaMoed') if israel else
                            Entry('חג הסוכות - יום שני', 'Sukkos - Second Day'))
            elif (j_day in [17, 18, 19, 20]):
                list.append(Entry("סוכות - חול המועד", "Sukkos - Chol HaMoed"))
            elif (j_day == 21):
                list.append(Entry('הושענא רבה - ערב יו"ט', "Hoshana Rabba - Erev Yomtov"))
            elif (j_day == 22):
                list.append(Entry("שמיני עצרת", "Shmini Atzeres"))
                if (israel):
                    list.append(Entry("שמחת תורה", "Simchas Torah"))
            elif (j_day == 23 and not israel):
                list.append(Entry("שמחת תורה", "Simchas Torah"))
        elif j_month == 8:  # Cheshvan
            if (day_of_week == 1 and j_day > 2 and j_day < 12):
                list.append(Entry("תענית שני קמא", 'Baha"b'))
            elif (day_of_week == 4 and j_day > 5 and j_day < 13):
                list.append(Entry("תענית חמישי", 'Baha"b'))
            elif (day_of_week == 1 and j_day > 9 and j_day < 17):
                list.append(Entry("תענית שני בתרא", 'Baha"b'))
            if (j_day == 7 and israel):
                list.append(Entry("ותן טל ומטר", "V'sain Tal U'Matar"))
        elif j_month == 9:  # Kislev
            if (j_day == 25):
                list.append(Entry("'חנוכה - נר א", "Chanuka - One Candle"))
            elif (j_day == 26):
                list.append(Entry("'חנוכה - נר ב", "Chanuka - Two Candles"))
            elif (j_day == 27):
                list.append(Entry("'חנוכה - נר ג", "Chanuka - Three Candles"))
            elif (j_day == 28):
                list.append(Entry("'חנוכה - נר ד", "Chanuka - Four Candles"))
            elif (j_day == 29):
                list.append(Entry("'חנוכה - נר ה", "Chanuka - Five Candles"))
            elif (j_day == 30):
                list.append(Entry("'חנוכה - נר ו", "Chanuka - Six Candles"))
        elif j_month == 10:  # Teves
            if (JDate.has_short_kislev(j_year)):
                if (j_day == 1):
                    list.append(Entry("'חנוכה - נר ו", "Chanuka - Six Candles"))
                elif (j_day == 2):
                    list.append(Entry("'חנוכה - נר ז", "Chanuka - Seven Candles"))
                elif (j_day == 3):
                    list.append(Entry("'חנוכה - נר ח", "Chanuka - Eight Candles"))
            else:
                if (j_day == 1):
                    list.append(Entry("'חנוכה - נר ז", "Chanuka - Seven Candles"))
                elif (j_day == 2):
                    list.append(Entry("'חנוכה - נר ח", "Chanuka - Eight Candles"))
            if (j_day == 10):
                list.append(Entry("צום עשרה בטבת", "Fast - 10th of Teves"))
        elif j_month == 11:  # Shvat
            if (j_day == 15):
                list.append(Entry("ט\"ו בשבט", "Tu B'Shvat"))
            elif j_month in [12, 13]:  # Both Adars
                if (j_month == 12 and isleap_jyear):  # Adar Rishon in a leap year
                    if (j_day == 14):
                        list.append(Entry("פורים קטן", "Purim Katan"))
                    elif (j_day == 15):
                        list.append(Entry("שושן פורים קטן", "Shushan Purim Katan"))
                else:  # The "real" Adar: the only one in a non-leap-year or Adar Sheini
                    if (j_day == 11 and day_of_week == 4):
                        list.append(Entry("תענית אסתר", "Fast - Taanis Esther"))
                    elif (j_day == 13 and day_of_week != 6):
                        list.append(Entry("תענית אסתר", "Fast - Taanis Esther"))
                    elif (j_day == 14):
                        list.append(Entry("פורים", "Purim"))
                    elif (j_day == 15):
                        list.append(Entry("שושן פורים", "Shushan Purim"))
        # If it is during Sefiras Ha'omer
        if ((j_month == 1 and j_day > 15) or j_month == 2 or (j_month == 3 and j_day < 6)):
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
               self._day != 9 and \
               (self._month == 6 or ((not israel) and dow in [3, 4]) or (israel and dow == 4))

    # Does the current Jewish date have candle lighting before sunset?
    def has_candle_lighting(self):
        dow = self.getdow()
        if (dow == 5):
            return True
        elif (dow == 6):
            # there is no "candle lighting time" - even if yom tov is on Motzai
            # Shabbos
            return False

        return (self._month == 1 and self._day in [14, 20] or
                (self._month == 3 and self._day == 5) or
                (self._month == 6 and self._day == 29) or
                (self._month == 7 and self._day in [9, 14, 21]))

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
