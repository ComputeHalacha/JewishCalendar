import json

import JewishCalendar
from JewishCalendar.HourMinute import HourMinute
from JewishCalendar.JewishDate import JewishDate
from JewishCalendar.Location import Location

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


def displayWeek(startJd, nameSearch):
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
            infos = JewishCalendar.getDailyInfo(jd, location, HEBREW)
            dz = JewishCalendar.getDailyZmanim(jd, location, HEBREW)
            for i, v in infos.items():
                display(i, v)
            for i, v in dz.items():
                display(i, v)
            jd += 1
    else:
        print('No location found that matches with "%s"' % (nameSearch))


if __name__ == '__main__':
    displayWeek(JewishDate.create(5776, 5, 5), 'Lakewood')
