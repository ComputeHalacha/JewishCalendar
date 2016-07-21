import argparse
import re
import sys

import jcal
from jcal.hourminute import HourMinute
from jcal.jdate import JDate
from jcal.location import Location

__doc__ = '''This file was  meant to be en example of use for the pyluach package.
It displays to console the full zmanim anywhere in the world for any number of days.
Use:
   import luach
   from jcal.jdate import JDate

   # Use the following to display todays zmanim for Lakewood NJ in English:
   luach.display_zmanim('lakewood')

   # Use the following to display the entire 8 days of Sukkos 5777 for Ashdod, in Hebrew:
   sukkos_day_one = JDate.create(5777, 7, 15)
   luach.display_zmanim(
        location_search_pattern="ashdod",
        startjd=sukkos_day_one,
        number_of_days=8,
        hebrew=True)
   '''


def display_zmanim(location_search_pattern, startjd=JDate.today(), number_of_days=1, hebrew=False, army_time=False):
    locations = Location.get_location(location_search_pattern)
    if locations:
        for location in locations:
            if hebrew:
                print('** זמני היום עבור {} {:*<15}'.format(location.hebrew.upper(), ''))
            else:
                print('** ZMANIM FOR {} {:*<15}'.format(location.name.upper(), ''))
            jd = startjd
            for i in range(number_of_days):
                print('\n--{:-<50}'.format(jd.todate().strftime('%A, %B %d, %Y')))

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
    if len(sys.argv) > 1:
        parser = argparse.ArgumentParser(description='Display all the days Zmanim for anywhere in the world')
        parser.add_argument('location',
                            help='''The city or location name or a part of it.\r\n
                                The search is not case sensitive.\r\n
                                If the supplied value matches more than one location,
                                the displayed Zmanim will be reapeated for each match.\r\n
                                Regular expressions can also be used.\r\n
                                For example: "new" will display the zmanim for New Bedfort, New Brunswick, New Delhi, New York, New Square etc. ''')
        parser.add_argument('-jd', '--jewishdate', default=JDate.today(), type=parse_jewish_date,
                            help='''The Jewish Date to start from in the format: day-month-year.\r\n
                                For the month, keep in mind that Nissan is month number 1.\r\n
                                For example: "13-4-5777" will get the 13th day of Tammuz 5777.\r\n
                                You can also separate the date parts with a forward-slash (/), comma or period.\r\n
                                If this argument is not supplied, the current date is used.''')
        parser.add_argument('-d', '--days', type=int, default=1,
                            help='''The number of days to display.\r\n
                                If this is not supplied, a single day will be displayed.''')
        parser.add_argument('-heb', '--hebrew', action="store_true", help='Display in Hebrew.')
        parser.add_argument('-a', '--army', action="store_true", help='Display times as army time. Such as 22:10 instead of 10:10 PM')
        parser.add_argument('-l', '--locations', action="store_true",
                            help='''Instead of displaying the Zmanim, display the list of locations matching the "location" argument.\n\r
                                To display all the locations in the list, use: luach.py . -l''')
        args = parser.parse_args()

        if not args.locations:
            display_zmanim(location_search_pattern=args.location,
                           startjd=args.jewishdate,
                           number_of_days=args.days,
                           hebrew=args.hebrew,
                           army_time=args.army)
        else:
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
        # just showing off
        display_zmanim('מודיעין עילית')


if __name__ == '__main__':
    main()
