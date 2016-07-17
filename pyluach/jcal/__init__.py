from collections import OrderedDict

__version__ = '0.0.3'
__all__ = [
    'dafyomi', 'hourminute', 'jdate', 'location',
    'molad', 'pirkeiavos', 'sedra', 'utils', 'zmanim'
]

__author__ = 'CB Sommers <cb@compute.co.il>'


# Returns an OrderedDict of information about the given day.
# Entries include: "Date", "Parshas Hashavua", any holidays or fasts,
# "Eruv Tavshilin", "Candle Lighting" and "Daf Yomi".
def getdailyinfo(jd, location, hebrew):
    from jcal.jdate import JDate
    import jcal.utils
    from jcal.molad import Molad
    from jcal.hourminute import HourMinute
    from jcal.sedra import Sedra
    from jcal.dafyomi import Dafyomi
    import jcal.pirkeiavos

    infos = OrderedDict()
    sedras = Sedra.get_sedra(jd, location.israel)
    holidays = jd.get_holidays(location.israel)

    if hebrew:
        infos['תאריך'] = jd.tostring_heb()
        infos['פרשת השבוע'] = ' - '.join([s[1] for s in sedras])
        for h in holidays:
            htext = h.heb
            if 'מברכים' in htext:
                nextMonth = jd.add_days(12)
                htext += '- חודש ' + utils.proper_jmonth_name(nextMonth.year, nextMonth.month)
                htext += '\nהמולד: ' + Molad.molad_string_heb(nextMonth.month, nextMonth.year)
                dim = JDate.days_in_jmonth(jd.year, jd.month)
                dow = dim - jd.getdow() - (1 if dim == 30 else 0)
                htext += '\nראש חודש: ' + utils.dowHeb[dow]
                if dim == 30:
                    htext += ", " + utils.dowHeb[(dow + 1) % 7]
            infos[htext] = ''
        if jd.has_eiruv_tavshilin(location.israel):
            infos['עירוב תבשילין'] = ''
        if jd.has_candle_lighting():
            infos['הדלקת נרות'] = jd.get_candle_lighting(location)
        infos['דף יומי'] = Dafyomi.tostring_heb(jd)
        if jd.getdow() == 6:
            prakim = pirkeiavos.get_pirkeiavos(jd, location.israel)
            if prakim:
                infos['פרקי אבות'] = ' פרק' + ' ופרק '.join([utils.jsd[p - 1] for p in prakim])
    else:
        infos["Date"] = jd.tostring()
        infos['Parshas Hashavua'] = ' - '.join([s[0] for s in sedras])
        for h in holidays:
            htext = h.eng
            if 'Mevarchim' in htext:
                nextMonth = jd.add_days(12)
                htext += '- Chodesh ' + utils.proper_jmonth_name(nextMonth.year, nextMonth.month)
                htext += '\nThe Molad: ' + Molad.molad_string_heb(nextMonth.month, nextMonth.year)
                dim = JDate.days_in_jmonth(jd.year, jd.month)
                dow = dim - jd.getdow() - (1 if dim == 30 else 0)
                htext += '\nRosh Chodesh: ' + utils.dowHeb[dow]
                if dim == 30:
                    htext += ", " + utils.dowEng[(dow + 1) % 7]
            infos[htext] = ''
        if jd.has_eiruv_tavshilin(location.israel):
            infos['Eruv Tavshilin'] = ''
        if jd.has_candle_lighting():
            infos['Candle Lighting'] = jd.get_candle_lighting(location)
        infos['Daf Yomi'] = Dafyomi.tostring(jd)
        if jd.getdow() == 6:
            prakim = pirkeiavos.get_pirkeiavos(jd, location.israel)
            if prakim:
                infos['Pirkei Avos'] = ' and '.join([utils.to_suffixed(p) + ' Perek' for p in prakim])
    return infos


def getdailyzmanim(jd, location, hebrew):
    from jcal.zmanim import Zmanim
    from jcal.hourminute import HourMinute

    infos = OrderedDict()
    z = Zmanim(location, jd)
    netz, shkia = z.get_sun_times(considerElevation=True)
    stMishor = z.get_sun_times(considerElevation=False)
    netzMishor, shkiaMishor = stMishor
    shaaZmanis = z.get_shaa_zmanis(netzshkia=stMishor)
    shaaZmanis90 = z.get_shaa_zmanis(offset=90,netzshkia=stMishor)
    chatzos = z.get_chatzos(suntimes=stMishor)
    noValue = HourMinute(hour=0, minute=0)

    if hebrew:
        if jd.month == 1 and jd.day == 14:
            infos["סו\"ז אכילת חמץ"] = (netz - 90) + int(shaaZmanis90 * 4)
            infos["סו\"ז שריפת חמץ"] = (netz - 90) + int(shaaZmanis90 * 5)
        if netz == noValue:
            infos["הנץ החמה"] = "השמש אינו עולה"
        else:
            infos["עלות השחר 90"] = netzMishor - 90
            infos["עלות השחר 72"] = netzMishor - 72

            if netz == netzMishor:
                infos["הנץ החמה"] = netz
            else:
                infos["הנץ החמה מגובה " + str(location.elevation) + " מטר"] = netz
                infos["הנץ החמה מגובה פני הים"] = netzMishor

            infos['סוזק\"ש - מג\"א'] = (netzMishor - 90) + int(shaaZmanis90 * 3)
            infos['סוזק\"ש - הגר\"א'] = netzMishor + int(shaaZmanis * 3)
            infos['סוז\"ת - מג\"א'] = (netzMishor - 90) + int(shaaZmanis90 * 4)
            infos['סוז\"ת - הגר\"א'] = netzMishor + int(shaaZmanis * 4)

        if netz != noValue and shkia != noValue:
            infos['חצות היום והלילה'] = chatzos
            infos['מנחה גדולה'] = (chatzos + int(shaaZmanis * 0.5))
            infos['מנחה קטנה'] = netzMishor + int(shaaZmanis * 9.5)
            infos['פלג המנחה'] = netzMishor + int(shaaZmanis * 10.75)

        if shkia == noValue:
            infos["שקיעת החמה"] = "השמש אינו שוקעת"
        else:
            if shkia == shkiaMishor:
                infos["שקיעת החמה"] = shkia
            else:
                infos["שקיעת החמה מגובה פני הים"] = shkiaMishor
                infos["שקיעת החמה מגובה " + str(location.elevation) + " מטר"] = shkia
            infos['צאת הכוכבים 45'] = shkia + 45
            infos['רבינו תם'] = shkia + 72
            infos['72 דקות זמניות'] = shkia + int(shaaZmanis * 1.2)
            infos['72 דקות זמניות לחומרה'] = shkia + int(shaaZmanis90 * 1.2)
    else:
        feet = int(location.elevation * 3.28084)
        if jd.month == 1 and jd.day == 14:
            infos["Stop eating Chometz by"] = (netz - 90) + int(shaaZmanis90 * 4)
            infos["Burn Chometz by"] = (netz - 90) + int(shaaZmanis90 * 5)
        if netz == noValue:
            infos["Netz Hachama"] = "The sun does not rise"
        else:
            infos["Alos Hashachar (90)"] = netzMishor - 90
            infos["Alos Hashachar (72)"] = netzMishor - 72
            if netz == netzMishor:
                infos["Netz Hachama"] = netz
            else:
                infos['Netz Hachama ({} feet)'.format(feet)] = netz
                infos["Netz Hachama (Sea Level)"] = netzMishor

            infos['Zman Krias Shma - MG"A'] = (netzMishor - 90) + int(shaaZmanis90 * 3)
            infos['Zman Krias Shma - GR"A'] = netzMishor + int(shaaZmanis * 3)
            infos['Zman Tefillah - MG"A'] = (netzMishor - 90) + int(shaaZmanis90 * 4)
            infos['Zman Tefillah - GR"A'] = netzMishor + int(shaaZmanis * 4)

        if netz != noValue and shkia != noValue:
            infos['Chatzos (day and night)'] = chatzos
            infos['Mincha Gedola'] = (chatzos + int(shaaZmanis * 0.5))
            infos['Mincha Ketana'] = netzMishor + int(shaaZmanis * 9.5)
            infos['Plag Hamincha'] = netzMishor + int(shaaZmanis * 10.75)
        if shkia == noValue:
            infos["Shkia"] = "Sun does not set"
        else:
            if shkia == shkiaMishor:
                infos["Shkia"] = shkia
            else:
                infos["Shkia (Sea Level)"] = shkiaMishor
                infos['Shkia (from ({} feet)'.format(feet)] = shkia
            infos['Tzais (45)'] = shkia + 45
            infos['Rabbeinu Tam (72)'] = shkia + 72
            infos['Rabbeinu Tam (Zmanios)'] = shkia + int(shaaZmanis * 1.2)
            infos['Rabbeinu Tam (Zmanios Lechumra)'] = shkia + int(shaaZmanis90 * 1.2)
    return infos
