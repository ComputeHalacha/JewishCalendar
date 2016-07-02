import datetime
from JewishCalendar.JewishDate import JewishDate
from JewishCalendar.Utils import Utils
if __name__ == '__main__':
    jd = JewishDate.today()
    jd = jd.addDays(1)
    print(jd , jd.getHolidays(True, False))

