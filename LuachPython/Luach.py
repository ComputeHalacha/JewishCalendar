from json import load
import LuachPython.JewishCalendar
import JewishDate
from Location import Location

HEBREW = False


def display(title, value):
    if not value:
        print(title)
    else:
        try:
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
        jd = JewishDate.JewishDate.today()
        nextweek = jd.addDays(7)
        while jd < nextweek:
            print('\n--{:-<50}'.format(jd.todate().strftime('%A, %B %d, %Y')))
            infos = LuachPython.JewishCalendar.getDailyInfo(jd, lo, HEBREW)
            dz = LuachPython.JewishCalendar.getDailyZmanim(jd, lo, HEBREW)
            for i, v in infos.items():
                display(i, v)
            for i, v in dz.items():
                display(i, v)
            jd = jd.addDays(1)

