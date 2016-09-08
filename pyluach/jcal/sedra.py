from jcal.jdate import JDate

'''************************************************************************************
 * Computes the Sedra/Sedras of the week for the given day.
 * Returns an array of sedras (either one or two) for the given Jewish Date
 * Sample of use to get todays sedra in Israel:
 *     sedras = Sedra.get_sedra(JDate.today(), True)
 *     text = (' - ').join([s[0] for s in sedras])
 * The code was converted to python and tweaked by CBS.
 * It is directly based on the C code in Danny Sadinoff's HebCal - Copyright (C) 1994.
 * Portions of that code are Copyright (c) 2002 Michael J. Radwin. All Rights Reserved.
 * Many of the algorithms were taken from hebrew calendar routines
 * implemented by Nachum Dershowitz
 * ***********************************************************************************'''


class Sedra:
    _lastcalculatedyear = None
    _sedralist = [("Bereshis", "בראשית"), ("Noach", "נח"), ("Lech-Lecha", "לך לך"), ("Vayera", "וירא"),
                  ("Chayei Sara", "חיי שרה"), ("Toldos", "תולדות"), ("Vayetzei", "ויצא"), ("Vayishlach", "וישלח"),
                  ("Vayeishev", "וישב"), ("Mikeitz", "מקץ"), ("Vayigash", "ויגש"), ("Vayechi", "ויחי"),
                  ("Shemos", "שמות"), ("Va'era", "וארא"), ("Bo", "בא"), ("Beshalach", "בשלח"), ("Yisro", "יתרו"),
                  ("Mishpatim", "משפטים"), ("Terumah", "תרומה"), ("Tetzaveh", "תצוה"), ("Ki Sisa", "כי תשא"),
                  ("Vayakhel", "ויקהל"), ("Pekudei", "פקודי"), ("Vayikra", "ויקרא"), ("Tzav", "צו"),
                  ("Shmini", "שמיני"), ("Tazria", "תזריע"), ("Metzora", "מצורע"), ("Achrei Mos", "אחרי מות"),
                  ("Kedoshim", "קדושים"), ("Emor", "אמור"), ("Behar", "בהר"), ("Bechukosai", "בחקותי"),
                  ("Bamidbar", "במדבר"), ("Nasso", "נשא"), ("Beha'aloscha", "בהעלתך"), ("Sh'lach", "שלח"),
                  ("Korach", "קרח"), ("Chukas", "חקת"), ("Balak", "בלק"), ("Pinchas", "פינחס"), ("Matos", "מטות"),
                  ("Masei", "מסעי"), ("Devarim", "דברים"), ("Va'eschanan", "ואתחנן"), ("Eikev", "עקב"),
                  ("Re'eh", "ראה"), ("Shoftim", "שופטים"), ("Ki Seitzei", "כי תצא"), ("Ki Savo", "כי תבא"),
                  ("Nitzavim", "נצבים"), ("Vayeilech", "וילך"), ("Ha'Azinu", "האזינו"),
                  ("Vezos Habracha", "וזאת הברכה")]
    _shabbos_short = (
        52, 52, 53, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, -21, 23, 24, 25, 25,
        -26,
        -28, 30, -31, 33, 34, 35, 36, 37, 38, 39, 40, -41, 43, 44, 45, 46, 47, 48, 49, 50)
    _shabbos_long = (
        52, 52, 53, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, -21, 23, 24, 25, 25,
        -26,
        -28, 30, -31, 33, 34, 35, 36, 37, 38, 39, 40, -41, 43, 44, 45, 46, 47, 48, 49, -50)
    _mon_short = (
        51, 52, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, -21, 23, 24, 25, 25, -26,
        -28,
        30, -31, 33, 34, 35, 36, 37, 38, 39, 40, -41, 43, 44, 45, 46, 47, 48, 49, -50)
    _mon_long = (
        51, 52, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, -21, 23, 24, 25, 25, -26,
        -28,
        30, -31, 33, 34, 34, 35, 36, 37, -38, 40, -41, 43, 44, 45, 46, 47, 48, 49, -50)
    _thu_normal = (
        52, 53, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, -21, 23, 24, 25, 25, 25,
        -26,
        -28, 30, -31, 33, 34, 35, 36, 37, 38, 39, 40, -41, 43, 44, 45, 46, 47, 48, 49, 50)
    _thu_normal_Israel = (
        52, 53, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, -21, 23, 24, 25, 25, -26,
        -28,
        30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, -41, 43, 44, 45, 46, 47, 48, 49, 50)
    _thu_long = (
        52, 53, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 25,
        -26,
        -28, 30, -31, 33, 34, 35, 36, 37, 38, 39, 40, -41, 43, 44, 45, 46, 47, 48, 49, 50)
    _shabbos_short_leap = (
        52, 52, 53, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25,
        26,
        27, 28, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, -41, 43, 44, 45, 46, 47, 48, 49, -50)
    _shabbos_long_leap = (
        52, 52, 53, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25,
        26,
        27, 28, 28, 29, 30, 31, 32, 33, 34, 34, 35, 36, 37, -38, 40, -41, 43, 44, 45, 46, 47, 48, 49, -50)
    _mon_short_leap = (
        51, 52, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26,
        27,
        28, 28, 29, 30, 31, 32, 33, 34, 34, 35, 36, 37, -38, 40, -41, 43, 44, 45, 46, 47, 48, 49, -50)
    _mon_short_leap_Israel = (
        51, 52, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26,
        27,
        28, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, -41, 43, 44, 45, 46, 47, 48, 49, -50)
    _mon_long_leap = (
        51, 52, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26,
        27,
        28, 28, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, -41, 43, 44, 45, 46, 47, 48, 49, 50)
    _mon_long_leap_Israel = (
        51, 52, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26,
        27,
        28, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50)
    _thu_short_leap = (
        52, 53, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26,
        27,
        28, 29, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50)
    _thu_long_leap = (
        52, 53, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26,
        27,
        28, 29, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, -50)

    @classmethod
    def get_sedra(cls, jd, israel):
        # If we are between the first day of Sukkos and Simchas Torah, the sedra will always be Vezos Habracha.
        if jd.month == 7 and 15 <= jd.day < (23 if israel else 24):
            return cls._sedralist[53],

        sedra_order = cls.get_sedra_order(jd.year, israel)
        # find the first saturday on or after today's date
        abs_date = cls.get_day_on_or_before(6, jd.ordinal + 6)
        week_num = int((abs_date - sedra_order['first_sat_in_year']) / 7)
        index = 0

        if week_num >= len(sedra_order['sedra_array']):
            index_last = sedra_order['sedra_array'][len(sedra_order['sedra_array']) - 1]
            if index_last < 0:
                # advance 2 parashiyot ahead after a doubled week
                index = (-index_last) + 2
            else:
                index = index_last + 1
        else:
            index = sedra_order['sedra_array'][week_num]

        if index >= 0:
            sedra_array = cls._sedralist[index],
        else:
            i = -index  # undouble the sedra
            sedra_array = cls._sedralist[i], cls._sedralist[i + 1]

        return sedra_array

    @staticmethod
    def get_day_on_or_before(day_of_week, date):
        return date - ((date - day_of_week) % 7)

    @classmethod
    def get_sedra_order(cls, year, israel):
        # If the last call is within the same year as this one, we reuse the data.
        # If memory is an issue, remove these next few lines
        if cls._lastcalculatedyear is not None and cls._lastcalculatedyear['year'] == year and \
                        cls._lastcalculatedyear['israel'] == israel:
            return cls._lastcalculatedyear

        long_cheshvon = JDate.has_long_cheshvan(year)
        short_kislev = JDate.has_short_kislev(year)
        rosh_hashana = JDate.toordinal(year, 7, 1)
        rosh_hashana_dow = abs(rosh_hashana % 7)
        first_sat_in_year = cls.get_day_on_or_before(6, rosh_hashana + 6)
        year_type = 'regular'
        s_array = None

        if long_cheshvon and not short_kislev:
            year_type = 'complete'
        elif not long_cheshvon and short_kislev:
            year_type = 'incomplete'

        if not JDate.isleap_jyear(year):
            if rosh_hashana_dow == 6:
                if year_type == "incomplete":
                    s_array = cls._shabbos_short
                elif year_type == 'complete':
                    s_array = cls._shabbos_long
            elif rosh_hashana_dow == 1:
                if year_type == 'incomplete':
                    s_array = cls._mon_short
                elif year_type == 'complete':
                    s_array = cls._mon_short if israel else cls._mon_long
            elif rosh_hashana_dow == 2:
                if year_type == 'regular':
                    s_array = cls._mon_short if israel else cls._mon_long
            elif rosh_hashana_dow == 4:
                if year_type == 'regular':
                    s_array = cls._thu_normal_Israel if israel else cls._thu_normal
                elif year_type == 'complete':
                    s_array = cls._thu_long
            else:
                raise ValueError("improper sedra year type calculated.")
        # leap year
        else:
            if rosh_hashana_dow == 6:
                if year_type == 'incomplete':
                    s_array = cls._shabbos_short_leap
                elif year_type == 'complete':
                    s_array = cls._shabbos_short_leap if israel else cls._shabbos_long_leap
            elif rosh_hashana_dow == 1:
                if year_type == 'incomplete':
                    s_array = cls._mon_short_leap_Israel if israel else cls._mon_short_leap
                elif year_type == 'complete':
                    s_array = cls._mon_long_leap_Israel if israel else cls._mon_long_leap
            elif rosh_hashana_dow == 2:
                if year_type == 'regular':
                    s_array = cls._mon_long_leap_Israel if israel else cls._mon_long_leap
            elif rosh_hashana_dow == 4:
                if year_type == 'incomplete':
                    s_array = cls._thu_short_leap
                elif year_type == 'complete':
                    s_array = cls._thu_long_leap
            else:
                raise ValueError("improper sedra year type calculated.")

        retobj = dict(first_sat_in_year=first_sat_in_year, sedra_array=s_array, year=year, israel=israel)

        # Save the data in case the next call is for the same year
        cls._lastcalculatedyear = retobj

        return retobj


if __name__ == '__main__':
    sedras = Sedra.get_sedra(JDate.today(), True)
    text = ' - '.join([s[0] for s in sedras])
    print(text)
