"""Calendar printing functions

By default, these calendars have Sunday as the first day of the week.
Use setfirstweekday() to set the first day of the week (0=Monday, 6=Sunday)."""

import locale as _locale
import sys

from jcal import utils
from jcal.jdate import JDate

__all__ = ["IllegalMonthError", "IllegalWeekdayError", "setfirstweekday",
           "firstweekday", "isleap", "leapyears", "weekday", "monthrange",
           "monthcalendar", "prmonth", "month", "prcal", "calendar",
           "timegm", "utils.jMonthsEng", "month_abbr", "utils.dowEng", "utils.dowEngAbbr"]


# Exception raised for bad input (with string parameter for details)
error = ValueError


# Exceptions raised for bad input
class IllegalMonthError(ValueError):
    def __init__(self, month):
        self.month = month

    def __str__(self):
        return "bad month number %r; must be 1-13" % self.month


class IllegalWeekdayError(ValueError):
    def __init__(self, weekday):
        self.weekday = weekday

    def __str__(self):
        return "bad weekday number %r; must be 0 (Sunday) to 6 (Shabbos)" % self.weekday

# Constants for weekdays
SUNDAY, MONDAY, TUESDAY, WEDNESDAY, THURSDAY, FRIDAY, SHABBOS = range(7)


def isleap(year):
    """Return True for leap years, False for non-leap years."""
    return JDate.isleap_jyear(year)

def leapyears(y1, y2):
    """Return number of leap years in range [y1, y2).
       Assume y1 <= y2."""

    return sum(1 for y in range(y1, y2 + 1) if isleap(y))


def weekday(year, month, day):
    """Return weekday (0-6 ~ Sun-Shabbos) for year (5776-...), month (1-12), day (1-30)."""
    return JDate(year, month, day).getdow()


def monthrange(year, month):
    """Return weekday (0-6 ~ Sun-Shabbos) and number of days (29-30) for
       year, month."""
    if not 1 <= month <= 13:
        raise IllegalMonthError(month)
    day1 = weekday(year, month, 1)
    ndays = JDate.days_in_jmonth(year, month)
    return day1, ndays


class Calendar(object):
    """
    Base calendar class. This class doesn't do any formatting. It simply
    provides data to subclasses.
    """

    def __init__(self, firstweekday=0):
        self.firstweekday = firstweekday  # 0 = Sunday, 6 = Shabbos

    def getfirstweekday(self):
        return self._firstweekday % 7

    def setfirstweekday(self, firstweekday):
        self._firstweekday = firstweekday

    firstweekday = property(getfirstweekday, setfirstweekday)

    def iterweekdays(self):
        """
        Return an iterator for one week of weekday numbers starting with the
        configured first one.
        """
        for i in range(self.firstweekday, self.firstweekday + 7):
            yield i % 7

    def itermonthdates(self, year, month):
        """
        Return an iterator for one month. The iterator will yield datetime.date
        values and will always iterate through complete weeks, so it will yield
        dates outside the specified month.
        """
        date = JDate(year, month, 1)
        # Go back to the beginning of the week
        days = (date.getdow() - self.firstweekday) % 7
        date -= days
        oneday = 1
        while True:
            yield date
            try:
                date += oneday
            except OverflowError:
                # Adding one day could fail after datetime.MAXYEAR
                break
            if date.month != month and date.getdow() == self.firstweekday:
                break

    def itermonthdays2(self, year, month):
        """
        Like itermonthdates(), but will yield (day number, weekday number)
        tuples. For days outside the specified month the day number is 0.
        """
        for date in self.itermonthdates(year, month):
            if date.month != month:
                yield (0, date.getdow())
            else:
                yield (date.day, date.getdow())

    def itermonthdays(self, year, month):
        """
        Like itermonthdates(), but will yield day numbers. For days outside
        the specified month the day number is 0.
        """
        for date in self.itermonthdates(year, month):
            if date.month != month:
                yield 0
            else:
                yield date.day

    def monthdatescalendar(self, year, month):
        """
        Return a matrix (list of lists) representing a month's calendar.
        Each row represents a week; week entries are datetime.date values.
        """
        dates = list(self.itermonthdates(year, month))
        return [dates[i:i + 7] for i in range(0, len(dates), 7)]

    def monthdays2calendar(self, year, month):
        """
        Return a matrix representing a month's calendar.
        Each row represents a week; week entries are
        (day number, weekday number) tuples. Day numbers outside this month
        are zero.
        """
        days = list(self.itermonthdays2(year, month))
        return [days[i:i + 7] for i in range(0, len(days), 7)]

    def monthdayscalendar(self, year, month):
        """
        Return a matrix representing a month's calendar.
        Each row represents a week; days outside this month are zero.
        """
        days = list(self.itermonthdays(year, month))
        return [days[i:i + 7] for i in range(0, len(days), 7)]

    def yeardatescalendar(self, year, width=3):
        """
        Return the data for the specified year ready for formatting. The return
        value is a list of month rows. Each month row contains up to width months.
        Each month contains between 4 and 6 weeks and each week contains 1-7
        days. Days are JDate objects.
        """
        months = [
            self.monthdatescalendar(year, i)
            for i in range(7, JDate.months_in_jyear(year) + 1)
            ]
        months += [
            self.monthdatescalendar(year, i)
            for i in range(1, 7)
            ]
        return [months[i:i + width] for i in range(0, len(months), width)]

    def yeardays2calendar(self, year, width=3):
        """
        Return the data for the specified year ready for formatting (similar to
        yeardatescalendar()). Entries in the week lists are
        (day number, weekday number) tuples. Day numbers outside this month are
        zero.
        """
        months = [
            self.monthdays2calendar(year, i)
            for i in range(7, JDate.months_in_jyear(year) + 1)
            ]
        months += [
            self.monthdays2calendar(year, i)
            for i in range(1, 7)
            ]
        return [months[i:i + width] for i in range(0, len(months), width)]

    def yeardayscalendar(self, year, width=3):
        """
        Return the data for the specified year ready for formatting (similar to
        yeardatescalendar()). Entries in the week lists are day numbers.
        Day numbers outside this month are zero.
        """
        months = [
            self.monthdayscalendar(year, i)
            for i in range(7, JDate.months_in_jyear(year) + 1)
            ]
        months += [
            self.monthdayscalendar(year, i)
            for i in range(1, 7)
            ]
        return [months[i:i + width] for i in range(0, len(months), width)]


class TextCalendar(Calendar):
    """
    Subclass of Calendar that outputs a calendar as a simple plain text
    similar to the UNIX program cal.
    """

    def prweek(self, theweek, width):
        """
        Print a single week (no newline).
        """
        print(self.formatweek(theweek, width), end=' ')

    def formatday(self, day, weekday, width):
        """
        Returns a formatted day.
        """
        if day == 0:
            s = ''
        else:
            s = '%2i' % day  # right-align single-digit days
        return s.center(width)

    def formatweek(self, theweek, width):
        """
        Returns a single week in a string (no newline).
        """
        return ' '.join(self.formatday(d, wd, width) for (d, wd) in theweek)

    def formatweekday(self, day, width):
        """
        Returns a formatted week day name.
        """
        if width >= 9:
            names = utils.dowEng
        else:
            names = utils.dowEngAbbr
        return names[day][:width].center(width)

    def formatweekheader(self, width):
        """
        Return a header for a week.
        """
        return ' '.join(self.formatweekday(i, width) for i in self.iterweekdays())

    def formatmonthname(self, theyear, themonth, width, withyear=True):
        """
        Return a formatted month name.
        """
        s = utils.jMonthsEng[themonth]
        if withyear:
            s = "%s %r" % (s, theyear)
        return s.center(width)

    def prmonth(self, theyear, themonth, w=0, l=0):
        """
        Print a month's calendar.
        """
        print(self.formatmonth(theyear, themonth, w, l), end=' ')

    def formatmonth(self, theyear, themonth, w=0, l=0):
        """
        Return a month's calendar string (multi-line).
        """
        w = max(2, w)
        l = max(1, l)
        s = self.formatmonthname(theyear, themonth, 7 * (w + 1) - 1)
        s = s.rstrip()
        s += '\n' * l
        s += self.formatweekheader(w).rstrip()
        s += '\n' * l
        for week in self.monthdays2calendar(theyear, themonth):
            s += self.formatweek(week, w).rstrip()
            s += '\n' * l
        return s

    def formatyear(self, theyear, w=2, l=1, c=6, m=3):
        """
        Returns a year's calendar as a multi-line string.
        """
        w = max(2, w)
        l = max(1, l)
        c = max(2, c)
        colwidth = (w + 1) * 7 - 1
        v = []
        a = v.append
        a(repr(theyear).center(colwidth * m + c * (m - 1)).rstrip())
        a('\n' * l)
        header = self.formatweekheader(w)

        for (i, row) in enumerate(self.yeardays2calendar(theyear, m)):
            months = range(m * i + 1, min(m * (i + 1) + 1, 14))
            months = _fix_order(months, theyear)
            a('\n' * l)
            names = (self.formatmonthname(theyear, k, colwidth, False)
                     for k in months)
            a(formatstring(names, colwidth, c).rstrip())
            a('\n' * l)
            headers = (header for k in months)
            a(formatstring(headers, colwidth, c).rstrip())
            a('\n' * l)
            # max number of weeks for this row
            height = max(len(cal) for cal in row)
            for j in range(height):
                weeks = []
                for cal in row:
                    if j >= len(cal):
                        weeks.append('')
                    else:
                        weeks.append(self.formatweek(cal[j], w))
                a(formatstring(weeks, colwidth, c).rstrip())
                a('\n' * l)
        return ''.join(v)

    def pryear(self, theyear, w=0, l=0, c=6, m=3):
        """Print a year's calendar."""
        print(self.formatyear(theyear, w, l, c, m))


class HTMLCalendar(Calendar):
    """
    This calendar returns complete HTML pages.
    """

    # CSS classes for the day <td>s
    cssclasses = ["sun", "mon", "tue", "wed", "thu", "fri", "shb"]

    def formatday(self, day, weekday):
        """
        Return a day as a table cell.
        """
        if day == 0:
            return '<td class="noday">&nbsp;</td>'  # day outside month
        else:
            return '<td class="%s">%d</td>' % (self.cssclasses[weekday], day)

    def formatweek(self, theweek):
        """
        Return a complete week as a table row.
        """
        s = ''.join(self.formatday(d, wd) for (d, wd) in theweek)
        return '<tr>%s</tr>' % s

    def formatweekday(self, day):
        """
        Return a weekday name as a table header.
        """
        return '<th class="%s">%s</th>' % (self.cssclasses[day], utils.dowEngAbbr[day])

    def formatweekheader(self):
        """
        Return a header for a week as a table row.
        """
        s = ''.join(self.formatweekday(i) for i in self.iterweekdays())
        return '<tr>%s</tr>' % s

    def formatmonthname(self, theyear, themonth, withyear=True):
        """
        Return a month name as a table row.
        """
        if withyear:
            s = '%s %s' % (utils.jMonthsEng[themonth], theyear)
        else:
            s = '%s' % utils.jMonthsEng[themonth]
        return '<tr><th colspan="7" class="month">%s</th></tr>' % s

    def formatmonth(self, theyear, themonth, withyear=True):
        """
        Return a formatted month as a table.
        """
        v = []
        a = v.append
        a('<table border="0" cellpadding="0" cellspacing="0" class="month">')
        a('\n')
        a(self.formatmonthname(theyear, themonth, withyear=withyear))
        a('\n')
        a(self.formatweekheader())
        a('\n')
        for week in self.monthdays2calendar(theyear, themonth):
            a(self.formatweek(week))
            a('\n')
        a('</table>')
        a('\n')
        return ''.join(v)

    def formatyear(self, theyear, width=3):
        """
        Return a formatted year as a table of tables.
        """
        v = []
        a = v.append
        width = max(width, 1)
        a('<table border="0" cellpadding="0" cellspacing="0" class="year">')
        a('\n')
        a('<tr><th colspan="%d" class="year">%s</th></tr>' % (width, theyear))
        for i in range(1, JDate.months_in_jyear(theyear) + 1, width):
            # months in this row
            months = range(i, min(i + width, 14))
            months = _fix_order(months)
            a('<tr>')
            for m in months:
                a('<td>')
                a(self.formatmonth(theyear, m, withyear=False))
                a('</td>')
            a('</tr>')
        a('</table>')
        return ''.join(v)

    def formatyearpage(self, theyear, width=3, css='calendar.css', encoding=None):
        """
        Return a formatted year as a complete HTML page.
        """
        if encoding is None:
            encoding = sys.getdefaultencoding()
        v = []
        a = v.append
        a('<?xml version="1.0" encoding="%s"?>\n' % encoding)
        a(
            '<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">\n')
        a('<html>\n')
        a('<head>\n')
        a('<meta http-equiv="Content-Type" content="text/html; charset=%s" />\n' % encoding)
        if css is not None:
            a('<link rel="stylesheet" type="text/css" href="%s" />\n' % css)
        a('<title>Calendar for %d</title>\n' % theyear)
        a('</head>\n')
        a('<body>\n')
        a(self.formatyear(theyear, width))
        a('</body>\n')
        a('</html>\n')
        return ''.join(v).encode(encoding, "xmlcharrefreplace")

# Change months to Tishrei first
def _fix_order(months, year):
    """
    Even though the month numbers for the Jewish year start from Nissan,
    the year starts from Tishrei.
    This function takes a list of month numbers used to display a yearly calendar,
    ordered as month 1 in year, 2 in year etc.
    and replaces the number with the correct real month number for that month.
    So for example, if the number is 1 , meaning the first month in the year,
    it is replaced with 7 which is the correct month number for Tishrei.
    """
    ly = JDate.months_in_jyear(year)
    # list of month numbers of this Jewish Year starting from Tishrei through Ellul.
    monthprog = [i for i in range(7, ly + 1)] + [i for i in range(1, 7)]
    return [monthprog[i - 1] for i in months]

# Support for old module level interface
c = TextCalendar()

firstweekday = c.getfirstweekday


def setfirstweekday(firstweekday):
    if not SUNDAY <= firstweekday <= SHABBOS:
        raise IllegalWeekdayError(firstweekday)
    c.firstweekday = firstweekday


monthcalendar = c.monthdayscalendar
prweek = c.prweek
week = c.formatweek
weekheader = c.formatweekheader
prmonth = c.prmonth
month = c.formatmonth
calendar = c.formatyear
prcal = c.pryear

# Spacing of month columns for multi-column year calendar
_colwidth = 7 * 3 - 1  # Amount printed by prweek()
_spacing = 6  # Number of spaces between columns


def format(cols, colwidth=_colwidth, spacing=_spacing):
    """Prints multi-column formatting for year calendars"""
    print(formatstring(cols, colwidth, spacing))


def formatstring(cols, colwidth=_colwidth, spacing=_spacing):
    """Returns a string formatted from n strings, centered within n columns."""
    spacing *= ' '
    return spacing.join(c.center(colwidth) for c in cols)

def main(args):
    import optparse
    parser = optparse.OptionParser(usage="usage: %prog [options] [year [month]]")
    parser.add_option(
        "-w", "--width",
        dest="width", type="int", default=2,
        help="width of date column (default 2, text only)"
    )
    parser.add_option(
        "-l", "--lines",
        dest="lines", type="int", default=1,
        help="number of lines for each week (default 1, text only)"
    )
    parser.add_option(
        "-s", "--spacing",
        dest="spacing", type="int", default=6,
        help="spacing between months (default 6, text only)"
    )
    parser.add_option(
        "-m", "--months",
        dest="months", type="int", default=3,
        help="months per row (default 3, text only)"
    )
    parser.add_option(
        "-c", "--css",
        dest="css", default="calendar.css",
        help="CSS to use for page (html only)"
    )
    parser.add_option(
        "-L", "--locale",
        dest="locale", default=None,
        help="locale to be used from month and weekday names"
    )
    parser.add_option(
        "-e", "--encoding",
        dest="encoding", default=None,
        help="Encoding to use for output."
    )
    parser.add_option(
        "-t", "--type",
        dest="type", default="text",
        choices=("text", "html"),
        help="output type (text or html)"
    )

    (options, args) = parser.parse_args(args)

    if options.locale and not options.encoding:
        parser.error("if --locale is specified --encoding is required")
        sys.exit(1)

    locale = options.locale, options.encoding

    if options.type == "html":
        cal = HTMLCalendar()
        encoding = options.encoding
        if encoding is None:
            encoding = sys.getdefaultencoding()
        optdict = dict(encoding=encoding, css=options.css)
        write = sys.stdout.buffer.write
        if len(args) == 1:
            write(cal.formatyearpage(JDate.today().year, **optdict))
        elif len(args) == 2:
            write(cal.formatyearpage(int(args[1]), **optdict))
        else:
            parser.error("incorrect number of arguments")
            sys.exit(1)
    else:
        cal = TextCalendar()
        optdict = dict(w=options.width, l=options.lines)
        if len(args) != 3:
            optdict["c"] = options.spacing
            optdict["m"] = options.months
        if len(args) == 1:
            result = cal.formatyear(JDate.today().year, **optdict)
        elif len(args) == 2:
            result = cal.formatyear(int(args[1]), **optdict)
        elif len(args) == 3:
            result = cal.formatmonth(int(args[1]), int(args[2]), **optdict)
        else:
            parser.error("incorrect number of arguments")
            sys.exit(1)
        write = sys.stdout.write
        if options.encoding:
            result = result.encode(options.encoding)
            write = sys.stdout.buffer.write
        write(result)


if __name__ == "__main__":
    main(sys.argv)
