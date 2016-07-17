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
                infos = jcal.getdailyinfo(jd, location, hebrew)
                dz = jcal.getdailyzmanim(jd, location, hebrew)
                for i, v in infos.items():
                    display(i, v, hebrew, army_time)
                for i, v in dz.items():
                    display(i, v, hebrew, army_time)
                jd += 1
    else:
        print('No location found that matches with "%s"' % location_search_pattern)


def display(title, value, hebrew, army_time):
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


if __name__ == '__main__':
    display_zmanim('brooklyn')
