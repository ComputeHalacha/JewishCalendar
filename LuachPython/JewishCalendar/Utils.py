from datetime import date
import datetime
import math
from pytz import tzinfo


class Utils:
    jMonthsEng = ["", "Nissan", "Iyar", "Sivan", "Tamuz", "Av", "Ellul", "Tishrei", "Cheshvan", "Kislev", "Teves", "Shvat", "Adar", "Adar Sheini"]
    jMonthsHeb = ["", "ניסן", "אייר", "סיון", "תמוז", "אב", "אלול", "תשרי", "חשון", "כסלו", "טבת", "שבט", "אדר", "אדר שני"]
    sMonthsEng = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"]
    dowEng = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Erev Shabbos", "Shabbos Kodesh"]
    dowHeb = ["יום ראשון", "יום שני", "יום שלישי", "יום רביעי", "יום חמישי", "ערב שבת קודש", "שבת קודש"]
    jsd = ['א', 'ב', 'ג', 'ד', 'ה', 'ו', 'ז', 'ח', 'ט']
    jtd = ['י', 'כ', 'ל', 'מ', 'נ', 'ס', 'ע', 'פ', 'צ']
    jhd = ['ק', 'ר', 'ש', 'ת']
    jsnum = ["", "אחד", "שנים", "שלשה", "ארבעה", "חמשה", "ששה", "שבעה", "שמונה", "תשעה"]
    jtnum = ["", "עשר", "עשרים", "שלושים", "ארבעים"]


    # Gets the Jewish representation of a number (365 = שס"ה)
    # Minimum number is 1 and maximum is 9999.
    @staticmethod
    def toJNum(number):
        if (number < 1):
            raise RuntimeError("Min value is 1")
        if (number > 9999):
            raise RuntimeError("Max value is 9999")

        n = number
        retval = ''

        if n >= 1000:
            retval += Utils.jsd[int((n - (n % 1000)) / 1000) - 1] + "'"
            n = n % 1000

        while n >= 400:
            retval += 'ת'
            n -= 400

        if n >= 100:
            retval += Utils.jhd[int((n - (n % 100)) / 100) - 1]
            n = n % 100

        if n == 15:
            retval += "טו"
        elif n == 16:
            retval += "טז"
        else:
            if n > 9:
                retval += Utils.jtd[int((n - (n % 10)) / 10) - 1]
            if (n % 10) > 0:
                retval += Utils.jsd[(n % 10) - 1]
        if number > 999 and (number % 1000 < 10):
            retval = "'" + retval
        elif len(retval) > 1:
            retval = retval[:-1] + '"' + retval[-1]

        return retval

    # Add two character suffix to number. e.g. 21st, 102nd, 93rd, 500th
    @staticmethod
    def toSuffixed(num):
        t = str(num)
        suffix = "th"
        if len(t) == 1 or t[:-2] != '1':
            last = t[:-1]
            if last == '1':
                suffix = "st"
            elif last == '2':
                suffix = "nd"
            elif last == '3':
                suffix = "rd"
        return t + suffix


    #  Get day of week using Pythons datetime.date.isoweekday function.
    # As opposed to Pythons function, this function returns Sunday as 0
    @staticmethod
    def getSdDOW(year, month, day):
        return date(year, month, day).isoweekday() % 7

    # Gets the UTC offset in whole hours for the users time zone
    # Note: this is not affected by DST - unlike javascripts getTimezoneOffset() function which gives you the current offset.
    @staticmethod
    def currUtcOffset():
        date = datetime.today()
        jan = datetime(date.getFullYear(), 0, 1)
        jul = datetime(date.getFullYear(), 6, 1)
        return -int(math.max(jan.getTimezoneOffset(), jul.getTimezoneOffset()) / 60)

    # Determines if the given date is within DST on the users system
    @staticmethod
    def isDateDST(date):
        return (-int(date.getTimezoneOffset() / 60)) != Utils.currUtcOffset()

    # Determines if the users system is currently set to DST
    @staticmethod
    def isDST():
        return Utils.isDateDST(datetime.datetime.today())

    # Determines if the given date and time are during DST according to the USA rules
    @staticmethod
    def isUSA_DST(dt):
        year = dt.year
        month = dt.month
        day = dt.day
        hour = dt.hour

        if (month < 3 or month == 12):
            return False
        elif (month > 3 and month < 11):
            return True
        # DST starts at 2 AM on the second Sunday in March
        elif (month == 3): # March
            # Gets day of week on March 1st
            firstDOW = Utils.getSdDOW(year, 3, 1)
            # Gets date of second Sunday
            targetDate = 8 if firstDOW == 0 else ((7 - (firstDOW + 7) % 7)) + 8
            return (day > targetDate or (day == targetDate and hour >= 2))
            # DST ends at 2 AM on the first Sunday in November
        else: # dt.Month == 11 / November
            # Gets day of week on November 1st
            firstDOW = Utils.getSdDOW(year, 11, 1)
            # Gets date of first Sunday
            targetDate = 1 if firstDOW == 0 else ((7 - (firstDOW + 7) % 7)) + 1
            return (day < targetDate or (day == targetDate and hour < 2))

    # Determines if the given date and time is during DST according to the current (5776) Israeli rules
    @staticmethod
    def isIsrael_DST(dt):
        year = dt.getFullYear()
        month = dt.getMonth() + 1
        day = dt.getDate()
        hour = dt.hour

        if (month > 10 or month < 3):
            return False
        elif (month > 3 and month < 10):
            return True
        # DST starts at 2 AM on the Friday before the last Sunday in March
        elif (month == 3): # March
            # Gets date of the Friday before the last Sunday
            lastFriday = (31 - Utils.getSdDOW(year, 3, 31)) - 2
            return (day > lastFriday or (day == lastFriday and hour >= 2))
            # DST ends at 2 AM on the last Sunday in October
        else: # dt.Month === 10 / October
            # Gets date of last Sunday in October
            lastSunday = 31 - Utils.getSdDOW(year, 10, 31)
            return (day < lastSunday or (day == lastSunday and hour < 2))
