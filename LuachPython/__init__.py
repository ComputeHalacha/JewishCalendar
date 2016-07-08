from json import load
from JewishCalendar.JewishDate import JewishDate
from JewishCalendar.Zmanim import Zmanim
from JewishCalendar.Location import Location

if __name__ == '__main__':
    jd = JewishDate.today()
    file = open('Files/LocationsList.json', 'r', encoding='utf-8')
    b = load(file)
    mi=[Location.parse(m) for m in b['locations'] if m['n'].lower().startswith('modi')]
    mi.sort(key=lambda i: i.name)
    for lo in mi:
        #try:
        z = Zmanim(lo, jd)
        st = z.getSunTimes(True)
        print('{} - Candles: {} sunset: {}'.format(lo.name, st[1], z.getCandleLighting()))
        #except:
        #    print('Could not determine for ' + lo.name)