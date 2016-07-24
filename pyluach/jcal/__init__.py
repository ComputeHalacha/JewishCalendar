from collections import OrderedDict, namedtuple

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
        dy =  Dafyomi.tostring_heb(jd)
        if dy:
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


def getdailyzmanim(jd, location):
    from jcal.zmanim import Zmanim
    from jcal.hourminute import HourMinute

    infos = []
    z = Zmanim(location, jd)
    feet = int(location.elevation * 3.28084)
    netz, shkia = z.get_sun_times(considerElevation=True)
    st_mishor = z.get_sun_times(considerElevation=False)
    netz_mishor, shkia_mishor = st_mishor
    shaa_zmanis = z.get_shaa_zmanis(netzshkia=st_mishor)
    shaa_zmanis_90 = z.get_shaa_zmanis(offset=90, netzshkia=st_mishor)
    chatzos = z.get_chatzos(suntimes=st_mishor)
    chatzos_night = HourMinute(chatzos.hour % 12 + (12 if chatzos.hour < 12 else 0), chatzos.minute)
    alos90 = netz_mishor - 90
    alos72 = netz_mishor - 72
    ks_mga = (netz_mishor - 90) + int(shaa_zmanis_90 * 3)
    ks_gra = netz_mishor + int(shaa_zmanis * 3)
    zt_mga = (netz_mishor - 90) + int(shaa_zmanis_90 * 4)
    zt_gra = netz_mishor + int(shaa_zmanis * 4)
    mincha_gd = (chatzos + int(shaa_zmanis * 0.5))
    mincha_ktn = netz_mishor + int(shaa_zmanis * 9.5)
    plag_mincha = netz_mishor + int(shaa_zmanis * 10.75)
    tzais_45 = shkia + 45
    tzais_72 = shkia + 72
    tzais_72_zmanios = shkia + int(shaa_zmanis * 1.2)
    tzais_72_zmanios_chmr = shkia + int(shaa_zmanis_90 * 1.2)
    no_value = HourMinute(hour=0, minute=0)
    OneZman = namedtuple('OneZman', 'eng heb time')

    if jd.month == 1 and jd.day == 14:
        infos.append(
            OneZman(eng="Stop eating Chometz by", heb='סו"ז אכילת חמץ', time=(netz - 90) + int(shaa_zmanis_90 * 4)))
        infos.append(OneZman(eng='Burn Chometz by', heb='סו"ז שריפת חמץ', time=(netz - 90) + int(shaa_zmanis_90 * 5)))
    if netz == no_value or netz > chatzos:
        infos.append(OneZman(eng=' The sun does not rise', heb='השמש אינו עולה', time=None))
    else:
        # Doesn't make sense to show alos if it is before chatzos.
        # If chatzos is before midnight, then chatzos.hour will be > 12 -
        # so we then check if alos is after midnight - in other words has an hour less than 12
        if alos90 > chatzos_night or (chatzos_night.hour > 12 and alos90.hour < 12):
            infos.append(OneZman(eng='Alos Hashachar (90)', heb='עלות השחר 90', time=alos90))
        if alos72 > chatzos_night or (chatzos_night.hour > 12 and alos72.hour < 12):
            infos.append(OneZman(eng='Alos Hashachar (72)', heb='עלות השחר 72', time=alos72))
        if netz == netz_mishor:
            infos.append(OneZman(eng='Netz Hachama', heb='הנץ החמה', time=netz))
        else:
            infos.append(OneZman(eng='Netz Hachama ({} feet)'.format(feet),
                                 heb="הנץ החמה מגובה " + str(location.elevation) + " מטר",
                                 time=netz))
            infos.append(OneZman(eng='Netz Hachama (Sea Level)', heb='הנץ החמה מגובה פני הים', time=netz_mishor))

        infos.append(OneZman(eng='Zman Krias Shma - MG"A', heb='סוזק\"ש - מג\"א', time=ks_mga))
        infos.append(OneZman(eng='Zman Krias Shma - GR"A', heb='סוזק\"ש - הגר\"א', time=ks_gra))
        infos.append(OneZman(eng='Zman Tefillah - MG"A', heb='סוז\"ת - מג\"א', time=zt_mga))
        infos.append(OneZman(eng='Zman Tefillah - GR"A', heb='סוז\"ת - הגר\"א', time=zt_gra))

    if netz != no_value and shkia != no_value:
        infos.append(OneZman(eng='Chatzos (day and night)', heb='חצות היום והלילה', time=chatzos))
        infos.append(OneZman(eng='Mincha Gedola', heb='מנחה גדולה', time=mincha_gd))
        infos.append(OneZman(eng='Mincha Ketana', heb='מנחה קטנה', time=mincha_ktn))
        infos.append(OneZman(eng='Plag Hamincha', heb='פלג המנחה', time=plag_mincha))

    if shkia == no_value:
        infos.append(OneZman(eng='Shkiah - Sun does not set', heb='שקיעת החמה - השמש אינו שוקעת', time=None))
    else:
        if shkia == shkia_mishor:
            infos.append(OneZman(eng='Shkiah - Sunset', heb='שקיעת החמה', time=shkia))
        else:
            infos.append(OneZman(eng='Shkia (Sea Level)', heb='שקיעת החמה מגובה פני הים', time=shkia_mishor))
            infos.append(OneZman(eng='Shkia (from ({} feet)'.format(feet),
                                 heb="שקיעת החמה מגובה " + str(location.elevation) + " מטר", time=shkia))
            if tzais_45 < chatzos_night or (chatzos_night.hour < 12 and tzais_45.hour > 12):
                infos.append(OneZman(eng='Tzais (45)', heb='צאת הכוכבים 45', time=tzais_45))
            if tzais_72 < chatzos_night or (chatzos_night.hour < 12 and tzais_72.hour > 12):
                infos.append(OneZman(eng='Rabbeinu Tam (72)', heb='רבינו תם (72 שוות)', time=tzais_72))
            if tzais_72_zmanios < chatzos_night or (chatzos_night.hour < 12 and tzais_72_zmanios.hour > 12):
                infos.append(OneZman(eng='Rabbeinu Tam (Zmanios)', heb='72 דקות זמניות', time=tzais_72_zmanios))
            if tzais_72_zmanios_chmr < chatzos_night or (chatzos_night.hour < 12 and tzais_72_zmanios_chmr.hour > 12):
                infos.append(OneZman(eng='Rabbeinu Tam (Zmanios Lechumra)', heb='72 דקות זמניות לחומרה',
                                     time=tzais_72_zmanios_chmr))
    return infos
