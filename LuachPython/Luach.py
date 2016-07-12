import math
from json import load
from JewishCalendar.JewishDate import JewishDate
from JewishCalendar.Zmanim import Zmanim
from JewishCalendar.Location import Location
from JewishCalendar.Utils import Utils
from JewishCalendar.Molad import Molad
from JewishCalendar.HourMinute import HourMinute
from JewishCalendar.Sedra import Sedra
from JewishCalendar.Dafyomi import Dafyomi


def display(title, value=''):
    print('{:>20} {}'.format(title + (':' if value else ''), value))


if __name__ == '__main__':
    cityNameStart = 'Modi'.lower()
    file = open('Files/LocationsList.json', 'r', encoding='utf-8')
    b = load(file)
    mi = [Location.parse(m) for m in b['locations'] if m['n'].lower().startswith(cityNameStart)]

    if len(mi):
        lo = mi[0]

        jd = JewishDate.today()
        display(jd.toStringHeb())

        sedras = Sedra.getsedra(jd, lo.israel)
        display('פרשת השבוע', ' - '.join([s[1 if lo.israel else 0] for s in sedras]))

        holidays = jd.getHolidays(lo.israel, lo.israel)

        for h in holidays:
            hText = h
            if 'מברכים' in h:
                nextMonth = jd.addDays(12)
                hText += '- חודש ' + Utils.properMonthName(nextMonth.year, nextMonth.month)
                hText += '\nהמולד: ' + Molad.getStringHeb(nextMonth.month, nextMonth.year)
                dim = JewishDate.daysJMonth(jd.year, jd.month)
                dow = dim - jd.getDayOfWeek() - (1 if dim == 30 else 0)
                hText += '\nראש חודש: ' + Utils.dowHeb[dow]
                if dim == 30:
                    hText += ", " + Utils.dowHeb[(dow + 1) % 7]
            display(hText)

        if jd.hasEiruvTavshilin(lo.israel):
            display('עירוב תבשילין')

        if jd.hasCandleLighting():
            display('הדלקת נרות', jd.getCandleLighting(lo))

        display('דף יומי', Dafyomi.toStringHeb(jd))

        z = Zmanim(lo, jd)
        st = z.getSunTimes(True)
        netz = st[0]
        shkia = st[1]
        stMishor = z.getSunTimes(False)
        netzMishor = stMishor[0]
        shkiaMishor = stMishor[1]
        shaaZmanis = z.getShaaZmanis(netzshkia=stMishor)
        shaaZmanis90 = z.getShaaZmanis(90, stMishor)
        chatzos = z.getChatzos(stMishor)
        noValue = HourMinute(0, 0)

        if jd.month == 1 and jd.day == 14:
            display("סו\"ז אכילת חמץ", (netz - 90) + int(math.floor(shaaZmanis90 * 4)))
            display("סו\"ז שריפת חמץ", (netz - 90) + int(math.floor(shaaZmanis90 * 5)))

        if netz == noValue:
            display("הנץ החמה", "השמש אינו עולה")
        else:
            display("עלות השחר 90", netzMishor - 90)
            display("עלות השחר 72", netzMishor - 72)

            if netz == netzMishor:
                display("הנץ החמה", netz)
            else:
                display("הנץ החמה מגובה " + str(lo.elevation) + " מטר", netz)
                display("הנץ החמה מגובה פני הים", netzMishor)
                display('סוזק\"ש - מג\"א', (netzMishor - 90) + int(math.floor(shaaZmanis90 * 3)))
                display('סוזק\"ש - הגר\"א', netzMishor + int(math.floor(shaaZmanis * 3)))
                display('סוז\"ת - מג\"א', (netzMishor - 90) + int(math.floor(shaaZmanis90 * 4)))
                display('סוז\"ת - הגר\"א', netzMishor + int(math.floor(shaaZmanis * 4)))
        if netz != noValue and shkia != noValue:
            display('חצות היום והלילה', chatzos)
            display('מנחה גדולה', (chatzos + int(shaaZmanis * 0.5)))
            display('מנחה קטנה', netzMishor + int(shaaZmanis * 9.5))
            display('פלג המנחה', netzMishor + int(shaaZmanis * 10.75))
        if shkia == noValue:
            display("שקיעת החמה", "השמש אינו שוקעת")
        else:
            if shkia == shkiaMishor:
                display("שקיעת החמה", shkia)
            else:
                display("שקיעת החמה מגובה פני הים", shkiaMishor)
                display("שקיעת החמה מגובה " + str(lo.elevation) + " מטר", shkia)
            display('צאת הכוכבים 45', shkia + 45)
            display('רבינו תם', shkia + 72)
            display('72 דקות זמניות', shkia + int(shaaZmanis * 1.2))
            display('72 דקות זמניות לחומרה', shkia + int(shaaZmanis90 * 1.2))
            # except:
            #    print('Could not determine for ' + lo.name)
