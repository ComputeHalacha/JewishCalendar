import argparse
import datetime
import re

import jcal
import jcal.utils as Utils
from jcal.hourminute import HourMinute
from jcal.jdate import JDate
from jcal.location import Location

__doc__ = """This module is meant to be the command line interface to the pyluach package.

usage: luach.py [-h] [-convertdate JEWISHDATE] [-d DAYS] [-heb] [-a] [-l] location

Outputs a formatted list of Zmanim for anywhere in the world for any Jewish Date and for any number of days.

positional arguments:
  location              The city or location name. Doesn't need the full name,
                        the beginning of the name or a regular expression
                        search can be used. The search is not case sensitive.
                        For locations in Israel, the Hebrew name can be used as well as the English name.
                        If the supplied value matches more than one location,
                        the displayed Zmanim will be repeated for each match.
                        For example, if the supplied value is ".+wood", the
                        Zmanim of both Lakewood NJ and Hollywood California
                        will be displayed.

optional arguments:
  -h, --help            show the help message and exit

  -convertdate JEWISHDATE, --jewishdate JEWISHDATE
                        The Jewish Date to display the Zmanim for.
                        If this argument is not supplied, the current system date is converted to a Jewish Date and used.
                        The Jewish Date should be formatted: DAY-MONTH-YEAR.
                        DAY is the day of the month.
                        MONTH is the Jewish month number, Nissan is month number 1 and Adar Sheini is 13.
                        YEAR is the full 4 digit Jewish year.
                        For example, "1-7-5778" will get the first day of Rosh Hashana 5778.
                        "13-4-5777" will get the 13th day of Tammuz 5777.
                        Alternatively, the date parts can be separated with a forward-slash (/), comma or period.

  -d DAYS, --days DAYS  The number of days forward to display.
                        If this is not supplied, a single day will be displayed.

  -heb, --hebrew        Display the Zmanim in Hebrew.

  -a, --army            Display the Zmanim time in army-time/24 hour format.
                        For example, for a Zman of 10:10 PM, 22:10 will be displayed.

  -l, --locations       Instead of displaying the Zmanim, display the list of
                        locations returned from the "location" argument search.
                        Shows each locations name, latitude, longitude, elevation, utcoffset and hebrew name.
                        To show the full list of all the locations, use: luach.py .+ -l

For example, to show all the Zmanim for all the days of Sukkos 5777 for both Lakewood NJ and Brooklyn NY,
Use: luach.py "lakewood|brooklyn" -convertdate 15-7-5777 -d 9

To show the Zmanim in Hebrew for Tisha B'av in Jerusalem in the year 3248 (the year the Beis Hamikdash was destroyed),
 Use: luach.py "ירושלים" -convertdate 9-5-3248 -h
"""


def display_zmanim(location_search_pattern, startjd=JDate.today(), number_of_days=1, hebrew=False, army_time=False):
    locations = Location.get_location(location_search_pattern)
    if locations:
        for location in locations:
            if hebrew:
                print('זמני היום עבור {} {:*<45}\n'.format(location.hebrew.upper(), ''))
            else:
                print('\nZMANIM FOR {} {:*<45}'.format(location.name.upper(), ''))
            jd = startjd
            for i in range(number_of_days):
                gd = jd.todate()
                if isinstance(gd, datetime.date):
                    print('\n--{:-<50}'.format(jd.todate().strftime('%A, %B %d, %Y')))
                elif isinstance(gd, Utils.GregorianDate):
                    if not hebrew:
                        print('\n--{}, {} {}, {}{:->40}'.format(Utils.dowEng[jd.getdow()],
                                                                Utils.sMonthsEng[gd.month],
                                                                Utils.to_suffixed(gd.day),
                                                                abs(gd.year),
                                                                ' BCE' if gd.year < 1 else ''))
                    else:
                        print('\n--{} {} {} {} {:-<28}'.format(Utils.dowHeb[jd.getdow()],
                                                              gd.day,
                                                              'ל' + Utils.sMonthsHeb[gd.month],
                                                              abs(gd.year),
                                                              'לפה"ס' if gd.year < 1 else ''))

                # daily information is an OrderedDict of {title:value}
                for title, value in jcal.getdailyinfo(jd, location, hebrew).items():
                    display_info(title, value, hebrew, army_time)

                # daily zmanim is a list of namedtuple('OneZman', 'eng heb time')
                for one_zman in jcal.getdailyzmanim(jd, location):
                    display_zman(one_zman, hebrew, army_time)

                jd += 1
    else:
        print('No location found that matches with "%s"' % location_search_pattern)


def display_info(title, value, hebrew, army_time):
    if not value:
        if hebrew:
            print('{:<50}'.format(title))
        else:
            print(title)
    else:
        try:
            if isinstance(value, HourMinute):
                value = value.tostring(army_time)
            if hebrew:
                print('{:.<25}{:.>25}'.format(title, str(value)))
            else:
                print('{:.<30} {}'.format(title, str(value)))
        except TypeError:
            print('{}.........{}'.format(title, value))


def display_zman(one_zman, hebrew, army_time):
    try:
        value = one_zman.time.tostring(army_time) if one_zman.time else ''
        if hebrew:
            print('{:.<25}{:.>25}'.format(one_zman.heb, str(value)))
        else:
            print('{:.<30} {}'.format(one_zman.eng, str(value)))
    except TypeError:
        print('{}/{}.........{}'.format(one_zman.eng, one_zman.heb, value))


def parse_jewish_date(string):
    match = re.match(pattern=r'^(?P<day>\d{1,2})[-\/,\.](?P<month>\d{1,2})[-\/,\.](?P<year>\d{4})$', string=string)
    if not match:
        raise argparse.ArgumentTypeError("%r is not a valid jewish date" % string)
    else:
        try:
            return JDate(year=int(match.group('year')),
                         month=int(match.group('month')),
                         day=int(match.group('day')))
        except ValueError as ve:
            msg = '"{}" is not a valid jewish date as {}'.format(string, ve)
            raise argparse.ArgumentTypeError(msg)


def main():
    parser = argparse.ArgumentParser(description='Outputs a formatted list of Zmanim for anywhere in the world '
                                                 'for any Jewish Date and for any number of days',
                                     epilog='''For example, to show all the Zmanim for all the days of Sukkos 5777
                                               for both Lakewood NJ and Brooklyn NY,
                                               use: luach.py "lakewood|brooklyn" -convertdate 15-7-5777 -d 9''')
    parser.add_argument('location',
                        help='''The city or location name. Doesn't need the full name,
                                the beginning of the name or a regular expression
                                search can be used. The search is not case sensitive.
                                For locations in Israel, the Hebrew name can be used as well as the English name.
                                If the supplied value matches more than one location,
                                the displayed Zmanim will be repeated for each match.
                                For example, if the supplied value is ".+wood", the
                                Zmanim of both Lakewood NJ and Hollywood California
                                will be displayed.''')
    parser.add_argument('-convertdate', '--jewishdate', default=JDate.today(), type=parse_jewish_date,
                        help='''The Jewish Date to display the Zmanim for.
                                If this argument is not supplied, the current system date is converted to a Jewish Date and used.
                                The Jewish Date should be formatted: DAY-MONTH-YEAR.
                                DAY is the day of the month.
                                MONTH is the Jewish month number, Nissan is month number 1 and Adar Sheini is 13.
                                YEAR is the full 4 digit Jewish year.
                                For example, "1-7-5778" will get the first day of Rosh Hashana 5778.
                                "13-4-5777" will get the 13th day of Tammuz 5777.
                                Alternatively, the date parts can be separated with a forward-slash (/), comma or period.''')
    parser.add_argument('-d', '--days', type=int, default=1,
                        help='''The number of days forward to display.
                                If this is not supplied, a single day will be displayed.''')
    parser.add_argument('-heb', '--hebrew', action="store_true", help='Display the Zmanim in Hebrew.')
    parser.add_argument('-a', '--army', action="store_true",
                        help='''Display the Zmanim time in army-time/24 hour format.
                                For example, for a Zman of 10:10 PM, 22:10 will be displayed.''')
    parser.add_argument('-l', '--locations', action="store_true",
                        help='''Instead of displaying the Zmanim, display the list of
                                locations returned from the "location" argument search.
                                Shows each locations name, latitude, longitude, elevation, utcoffset and hebrew name.
                                To show the ful list of all the locations, use: luach.py .+ -l''')
    args = parser.parse_args()

    if args.locations:
        locs = Location.get_location(args.location)
        if locs:
            locs.sort(key=lambda loc: loc.name)
            for i in locs:
                try:
                    print(i)
                except:
                    pass
        else:
            print('No locations were found that matched ', args.location)
    else:
        display_zmanim(location_search_pattern=args.location,
                       startjd=args.jewishdate,
                       number_of_days=args.days,
                       hebrew=args.hebrew,
                       army_time=args.army)


if __name__ == '__main__':
    display_zmanim('Modi')
