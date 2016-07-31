import datetime
from calendar import isleap as is_greg_leap
from collections import namedtuple

from tzlocal import get_localzone

# As months start at 1, -1 is a placeholder for indexing purposes.
_DAYS_IN_GREG_MONTH = [-1, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31]
_DAYS_BEFORE_MONTH = [-1]
dbm = 0
for dim in _DAYS_IN_GREG_MONTH[1:]:
    _DAYS_BEFORE_MONTH.append(dbm)
    dbm += dim
# clean up module
del dbm, dim

# Represents a single day in the Gregorian calendar.
# Primarily used in the jcal library for representing dates
# before the common era - which the built-in datetime.date can't represent.
GregorianDate = namedtuple('GregorianDate', 'year month day')

jMonthsEng = ["", "Nissan", "Iyar", "Sivan", "Tamuz", "Av", "Ellul", "Tishrei", "Cheshvan", "Kislev", "Teves", "Shvat",
              "Adar", "Adar Sheini"]
jMonthsHeb = ["", "ניסן", "אייר", "סיון", "תמוז", "אב", "אלול", "תשרי", "חשון", "כסלו", "טבת", "שבט", "אדר", "אדר שני"]
sMonthsEng = ["", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October",
              "November", "December"]
sMonthsHeb = ["", "ינואר", "פברואר", "מרץ", "אפריל", "מאי", "יוני", "יולי", "אוגוסט", "ספטמבר", "אקטובר",
              "נובמבר", "דצמבר"]
dowEng = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Erev Shabbos", "Shabbos Kodesh"]
dowEngAbbr = ["Sun", "Mon", "Tue", "Wed", "Thur", "Fri", "Shb"]
dowHeb = ["יום ראשון", "יום שני", "יום שלישי", "יום רביעי", "יום חמישי", "ערב שבת קודש", "שבת קודש"]
jsd = ['א', 'ב', 'ג', 'ד', 'ה', 'ו', 'ז', 'ח', 'ט']
jtd = ['י', 'כ', 'ל', 'מ', 'נ', 'ס', 'ע', 'פ', 'צ']
jhd = ['ק', 'ר', 'ש', 'ת']
jsnum = ["", "אחד", "שנים", "שלשה", "ארבעה", "חמשה", "ששה", "שבעה", "שמונה", "תשעה"]
jtnum = ["", "עשר", "עשרים", "שלושים", "ארבעים"]


# Gets the Jewish representation of a number (365 = שס"ה)
# Minimum number is 1 and maximum is 9999.
def to_jnum(number):
    if (number < 1):
        return 'אפס'
    if (number > 9999):
        raise RuntimeError("Max value is 9999")

    n = number
    retval = ''

    if n >= 1000:
        retval += jsd[int((n - (n % 1000)) / 1000) - 1] + "'"
        n = n % 1000

    while n >= 400:
        retval += 'ת'
        n -= 400

    if n >= 100:
        retval += jhd[int((n - (n % 100)) / 100) - 1]
        n = n % 100

    if n == 15:
        retval += "טו"
    elif n == 16:
        retval += "טז"
    else:
        if n > 9:
            retval += jtd[int((n - (n % 10)) / 10) - 1]
        if (n % 10) > 0:
            retval += jsd[(n % 10) - 1]
    if number > 999 and (number % 1000 < 10):
        retval = "'" + retval
    elif len(retval) > 1:
        retval = retval[:-1] + '"' + retval[-1]

    return retval


# Add two character suffix to number. e.g. 21st, 102nd, 93rd, 500th
def to_suffixed(num):
    t = str(num)
    suffix = "th"
    if len(t) == 1 or t[-2] != '1':
        last = t[-1]
        if last == '1':
            suffix = "st"
        elif last == '2':
            suffix = "nd"
        elif last == '3':
            suffix = "rd"
    return t + suffix


# Gets the "proper" name for the given Jewish Month.
# This means for a leap year, labeling each of the the 2 Adars.
def proper_jmonth_name(jYear, jMonth, hebrew=False):
    from jcal.jdate import JDate
    if jMonth == 12 and JDate.isleap_jyear(jYear):
        return "Adar Rishon" if not hebrew else "אדר ראשון"
    else:
        return jMonthsEng[jMonth] if not hebrew else jMonthsHeb[jMonth]


# Gets one less than the ordinal for January 1st of the given year
def ordinal_till_year(year):
    y = year - 1
    ordinal = y * 365 + y // 4 - y // 100 + y // 400
    return ordinal


# Gets the number of days elapsed since the beginning of the Gregorian calendar
def ordinal_from_greg(date_or_year, month=None, day=None):
    """
    date_or_year can be:
          - a datetime.date object
          - a utils.GregorianDate namedtuple
          - an int representing the year
    """
    if isinstance(date_or_year, int):
        date_or_year = GregorianDate(date_or_year, month or 1, day or 1)
    if date_or_year.year > 0:
        # For dates that are representable by the built-in datetime.date class, we use the in-built compiled code.
        if not isinstance(date_or_year, datetime.date):
            date_or_year = datetime.date(date_or_year.year, date_or_year.month, date_or_year.day)
            return date_or_year.toordinal()
    else:
        return ordinal_till_year(date_or_year.year) + days_till_greg_date(date_or_year)


# Number of days in year preceding first day of the given Gregorian Month.
def days_till_greg_month(year, month):
    return _DAYS_BEFORE_MONTH[month] + (month > 2 and is_greg_leap(year))


# Number of days in year preceding the given Gregorian Date.
def days_till_greg_date(date_or_year, month=None, day=None):
    """
    date_or_year can be:
          - a datetime.date object
          - a utils.GregorianDate namedtuple
          - an int representing the year
    """
    if isinstance(date_or_year, int):
        date_or_year = GregorianDate(date_or_year, month or 1, day or 1)
    year, month, day = date_or_year.year, date_or_year.month, date_or_year.day
    return days_till_greg_month(year, month) + day


# Get day of week for a gregorian date.
# As opposed to Pythons function, this function returns Sunday as day 0
def greg_dow(date_or_year, month=None, day=None):
    """
    date_or_year can be:
          - a datetime.date object
          - a utils.GregorianDate namedtuple
          - an int representing the year
    """
    if isinstance(date_or_year, datetime.date):
        return date_or_year.isoweekday() % 7
    elif isinstance(date_or_year, int):
        date_or_year = GregorianDate(date_or_year, month or 1, day or 1)
    return ordinal_from_greg(date_or_year) % 7


def monthcalendar(year, month):
    start_weekday = JDate(year, month, 1).getdow()
    month_length = JDate.days_in_jmonth(year, month)
    end_weekday = start_weekday + (month_length - 1) % 7
    lpad = start_weekday
    rpad = 6 - end_weekday
    days = [None] * lpad + list(range(1, 1 + month_length)) + rpad * [None]
    return [days[i:i + 7] for i in range(0, len(days), 7)]


# Gets the UTC offset in whole hours for the users current time zone
# Note: this is not affected by DST
def curr_utc_offset():
    td = datetime.datetime.today()
    tz = get_localzone()  # local timezone
    return (tz.utcoffset(td) - tz.dst(td)).total_seconds() // 3600


# Determines if the users system is currently set to DST
def curr_dst():
    return is_sd_dst(datetime.datetime.today())


# Determines if the given date is within DST on the users system
def is_sd_dst(dt):
    if dt.year < 1:
        return None
    if not isinstance(dt, datetime.datetime):
        dt = datetime.datetime(dt.year, dt.month, dt.day)
    tz = get_localzone()  # local timezone
    return tz.dst(dt).total_seconds() > 0


# Determines if the given date and time are during DST according to the USA rules
def is_usa_dst(dt, hour):
    year = dt.year
    month = dt.month
    day = dt.day

    if month < 3 or month == 12:
        return False
    elif 3 < month < 11:
        return True
    # DST starts at 2 AM on the second Sunday in March
    elif month == 3:  # March
        # Gets day of week on March 1st
        first_dow = greg_dow(year, 3, 1)
        # Gets date of second Sunday
        target_date = 8 if first_dow == 0 else (7 - (first_dow + 7) % 7) + 8
        return day > target_date or (day == target_date and hour >= 2)
        # DST ends at 2 AM on the first Sunday in November
    else:  # dt.Month == 11 / November
        # Gets day of week on November 1st
        first_dow = greg_dow(year, 11, 1)
        # Gets date of first Sunday
        target_date = 1 if first_dow == 0 else (7 - (first_dow + 7) % 7) + 1
        return day < target_date or (day == target_date and hour < 2)


# Determines if the given date and time is during DST according to the current (5776) Israeli rules
def is_il_dst(dt):
    year = dt.year
    month = dt.month
    day = dt.day
    hour = dt.hour if 'hour' in dt else 0

    if month > 10 or month < 3:
        return False
    elif month > 3 and month < 10:
        return True
    # DST starts at 2 AM on the Friday before the last Sunday in March
    elif month == 3:  # March
        # Gets date of the Friday before the last Sunday
        last_friday = (31 - greg_dow(year, 3, 31)) - 2
        return day > last_friday or (day == last_friday and hour >= 2)
        # DST ends at 2 AM on the last Sunday in October
    else:  # dt.Month === 10 / October
        # Gets date of last Sunday in October
        last_sunday = 31 - greg_dow(year, 10, 31)
        return day < last_sunday or (day == last_sunday and hour < 2)


if __name__ == '__main__':
    from jcal.jdate import JDate

    # orig = GregorianDate(-2, 1, 1)
    # jd = JDate.fromdate(orig)
    # print('orig', orig)
    # print('orig - jdate', jd)
    # back = jd.todate()
    # print('back', back)
    td = JDate.today()
    from jcal.zmanim import Zmanim

    zm = Zmanim(dt=td)
    print(monthcalendar(td.year, td.month))
    print(zm.get_sun_times())
