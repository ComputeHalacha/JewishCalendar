from json import load
import JewishCalendar
from JewishCalendar.JewishDate import JewishDate
from JewishCalendar.Location import Location
from JewishCalendar.HourMinute import HourMinute

HEBREW = True
ARMYTIME = True


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

if __name__ == '__main__':
    cityNameStart = 'Modi'.lower()
    file = open('Files/LocationsList.json', 'r', encoding='utf-8')
    b = load(file)
    mi = [Location.parse(m) for m in b['locations'] if m['n'].lower().startswith(cityNameStart)]
    if len(mi):
        lo = mi[0]
        print('** ZMANIM FOR UPCOMING WEEK IN {} {:*<15}'.format(lo.name.upper(), ''))
        jd = JewishDate.today()
        nextweek = jd + 7
        while jd < nextweek:
            print('\n--{:-<50}'.format(jd.todate().strftime('%A, %B %d, %Y')))
            infos = JewishCalendar.getDailyInfo(jd, lo, HEBREW)
            dz = JewishCalendar.getDailyZmanim(jd, lo, HEBREW)
            for i, v in infos.items():
                display(i, v)
            for i, v in dz.items():
                display(i, v)
            jd += 1

