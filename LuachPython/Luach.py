from json import load
from JewishCalendar.JewishDate import JewishDate
from JewishCalendar.Zmanim import Zmanim
from JewishCalendar.Location import Location

if __name__ == '__main__':
    jd = JewishDate.today()
    file = open('Files/LocationsList.json', 'r', encoding='utf-8')
    b = load(file)
    mi=[Location.parse(m) for m in b['locations'] if m['n'].startswith('Modi')]
    mi.sort(key=lambda i: i.name)
    for lo in mi:
        z = Zmanim(lo, jd)
        try:
            st = z.getSunTimes(False)
            print('{} - sunrise: {} sunset: {}'.format(lo.name, st[0], st[1]))
        except:
            print('Could not determine ', lo.name)
