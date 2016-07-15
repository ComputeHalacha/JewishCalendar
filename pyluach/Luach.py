import json

import jcal
from jcal.hourminute import HourMinute
from jcal.jdate import JDate
from jcal.location import Location

HEBREW = False
ARMYTIME = False


def display(title, value):
    if not value:
        print(title)
    else:
        try:
            if isinstance(value, HourMinute):
                value = value.tostring(ARMYTIME)
            if HEBREW:
                print('{:.<25}{:.>25}'.format(title, str(value)))
            else:
                print('{:.<30} {}'.format(title, str(value)))
        except TypeError:
            print('{}.........{}'.format(title, value))


def display_week(startJd, nameSearch):
    file = open('Files/LocationsList.json', 'r', encoding='utf-8')
    b = json.load(file)
    loc_raw = next(iter([m for m in b['locations']
                         if nameSearch.lower() in m['n'].lower()]), None)
    if loc_raw:
        location = Location.parse(loc_raw)
        print('** ZMANIM FOR WEEK STARTING {} IN {} {:*<15}'.format(startJd, location.name.upper(), ''))
        jd = startJd
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
        print('No location found that matches with "%s"' % (nameSearch))


if __name__ == '__main__':
    display_week(JDate.create(5776, 5, 5), 'Lakewood')
