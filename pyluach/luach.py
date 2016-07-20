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

if __name__ == '__main__':
    display_zmanim('מוד')
