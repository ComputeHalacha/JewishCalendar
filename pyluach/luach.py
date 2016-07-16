import jcal
from jcal.hourminute import HourMinute
from jcal.jdate import JDate
from jcal.location import Location

__doc__ = '''Display to console a full week of Zmanim for anywhere in the world.
Use - to display the upcoming week for Lakewood NJ:
   import luach
   from jcal.jdate import JDate
   jd = JDate.today()
   luach.display_week(jd, 'lakewood')'''

HEBREW = True
ARMY_TIME = True


def display_week(startjd, search_pattern):
    locations = Location.get_location(r'מוד')
    if locations:
        for location in locations:
            if HEBREW:
                print('** זמני היום {} - {} {:*<15}'.format(startjd.tostring_heb(), location.hebrew.upper(), ''))
            else:
                print('** ZMANIM FOR WEEK STARTING {} IN {} {:*<15}'.format(startjd, location.name.upper(), ''))
            jd = startjd
            nextweek = jd + 7
            while jd < nextweek:
                print('\n--{:-<50}'.format(jd.todate().strftime('%A, %B %d, %Y')))
                infos = jcal.getdailyinfo(jd, location, HEBREW)
                dz = jcal.getdailyzmanim(jd, location, HEBREW)
                for i, v in infos.items():
                    display(i, v)
                for i, v in dz.items():
                    display(i, v)
                jd += 1
    else:
        print('No location found that matches with "%s"' % search_pattern)


def display(title, value):
    if not value:
        print(title)
    else:
        try:
            if isinstance(value, HourMinute):
                value = value.tostring(ARMY_TIME)
            if HEBREW:
                print('{:.<25}{:.>25}'.format(title, str(value)))
            else:
                print('{:.<30} {}'.format(title, str(value)))
        except TypeError:
            print('{}.........{}'.format(title, value))


if __name__ == '__main__':
    display_week(JDate.create(5776, 5, 5), 'Lakewood')
