import jcal.utils as utils
from jcal.hourminute import HourMinute
from jcal.jdate import JDate
from jcal.zmanim import Zmanim
from jcal.location import Location

''' Returns the molad for the given jewish month and year.
 Algorithm was adapted from Hebcal by Danny Sadinoff.

  Example of use:
  moladString = Molad.molad_string(5776, 10)
 '''


class Molad:
    @staticmethod
    def get_molad(month, year):
        month_adj = month - 7

        if month_adj < 0:
            month_adj += JDate.months_in_jyear(year)

        total_months = int(
            month_adj + 235 * int((year - 1) / 19) + 12 * ((year - 1) % 19) + ((((year - 1) % 19) * 7) + 1) / 19)
        parts_elapsed = 204 + (793 * (total_months % 1080))
        hours_elapsed = 5 + (12 * total_months) + 793 * int(total_months / 1080) + int(parts_elapsed / 1080) - 6
        parts = int((parts_elapsed % 1080) + 1080 * (hours_elapsed % 24))

        return dict(JDate=JDate.fromordinal((1 + (29 * int(total_months))) + int((hours_elapsed / 24))),
                    time=HourMinute(int(hours_elapsed) % 24, int((parts % 1080) / 18)), chalakim=parts % 18)

    #  Returns the time of the molad as a string in the format: Monday Night, 8:33 PM and 12 Chalakim
    #  The molad is always in Jerusalem so we use the Jerusalem sunset times
    #  to determine whether to display "Night" or "Motzai Shabbos" etc. (check this...)
    @staticmethod
    def molad_string(year, month):
        molad = Molad.get_molad(month, year)
        zmanim = Zmanim(dt=molad['JDate'])
        _, nightfall = zmanim.get_sun_times()
        is_night = molad['time'].total_minutes() >= nightfall.total_minutes()
        dow = molad['JDate'].getdow()
        text = ''

        if dow == 6 and is_night:
            text += "Motzai Shabbos,"
        elif dow == 5 and is_night:
            text += "Shabbos Night,"
        else:
            text += utils.dow_eng[dow] + (" Night" if is_night else "")

        text += " " + str(molad['time']) + " and " + str(molad['chalakim']) + " Chalakim"

        return text

    #  Returns the time of the molad as a string in the format: ליל שני 20:33 12 חלקים
    #  The molad is always in Jerusalem so we use the Jerusalem sunset times
    #  to determine whether to display "ליל/יום" or "מוצאי שב"ק" etc.
    @staticmethod
    def molad_string_heb(year, month):
        molad = Molad.get_molad(month, year)
        nightfall = molad['JDate'].getSunriseSunset(Location.get_jerusalem()).sunset
        is_night = molad['time'] > nightfall
        dow = molad['JDate'].getdow()
        text = ''

        if dow == 6:
            text += ('מוצאי שב״ק' if is_night else 'יום שב״ק')
        elif dow == 5:
            text += ('ליל שב״ק' if is_night else 'ערב שב״ק')
        else:
            text += ('ליל' if is_night else 'יום') + utils.dowHeb[dow].replace("יום", "")
        text += " " + molad['time'].tostring(army=True) + " " + str(molad['chalakim']) + " חלקים"

        return text


if __name__ == '__main__':
    print(Molad.molad_string(5776, 4))
