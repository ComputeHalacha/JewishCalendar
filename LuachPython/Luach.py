import datetime
from JewishCalendar.JewishDate import JewishDate
from JewishCalendar.Utils import Utils
from JewishCalendar.Zmanim import Zmanim
from JewishCalendar.Location import Location
if __name__ == '__main__':
    jd = JewishDate.today()
    print(jd, Zmanim(Location.getJerusalem(), jd).getSunTimes(True))

