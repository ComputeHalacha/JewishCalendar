from jcal.jdate import JDate

'''************************************************************************************
 * Computes the Sedra/Sedras of the week for the given day.
 * Returns an array of sedras (either one or two) for the given Jewish Date
 * Sample of use to get todays sedra in Israel:
 *     sedras = Sedra.getsedra(JDate.today(), True)
 *     text = (' - ').join([s[0] for s in sedras])
 * The code was converted to python and tweaked by CBS.
 * It is directly based on the C code in Danny Sadinoff's HebCal - Copyright (C) 1994.
 * Portions of that code are Copyright (c) 2002 Michael J. Radwin. All Rights Reserved.
 * Many of the algorithms were taken from hebrew calendar routines
 * implemented by Nachum Dershowitz
 * ***********************************************************************************'''


class Sedra:
    __lastCalculatedYear = None
    __sedraList = [("Bereshis", "בראשית"), ("Noach", "נח"), ("Lech-Lecha", "לך לך"), ("Vayera", "וירא"),
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
    __shabbos_short = (
        52, 52, 53, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, -21, 23, 24, 25, 25,
        -26,
        -28, 30, -31, 33, 34, 35, 36, 37, 38, 39, 40, -41, 43, 44, 45, 46, 47, 48, 49, 50)
    __shabbos_long = (
        52, 52, 53, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, -21, 23, 24, 25, 25,
        -26,
        -28, 30, -31, 33, 34, 35, 36, 37, 38, 39, 40, -41, 43, 44, 45, 46, 47, 48, 49, -50)
    __mon_short = (
        51, 52, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, -21, 23, 24, 25, 25, -26,
        -28,
        30, -31, 33, 34, 35, 36, 37, 38, 39, 40, -41, 43, 44, 45, 46, 47, 48, 49, -50)
    __mon_long = (
        51, 52, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, -21, 23, 24, 25, 25, -26,
        -28,
        30, -31, 33, 34, 34, 35, 36, 37, -38, 40, -41, 43, 44, 45, 46, 47, 48, 49, -50)
    __thu_normal = (
        52, 53, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, -21, 23, 24, 25, 25, 25,
        -26,
        -28, 30, -31, 33, 34, 35, 36, 37, 38, 39, 40, -41, 43, 44, 45, 46, 47, 48, 49, 50)
    __thu_normal_Israel = (
        52, 53, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, -21, 23, 24, 25, 25, -26,
        -28,
        30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, -41, 43, 44, 45, 46, 47, 48, 49, 50)
    __thu_long = (
        52, 53, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 25,
        -26,
        -28, 30, -31, 33, 34, 35, 36, 37, 38, 39, 40, -41, 43, 44, 45, 46, 47, 48, 49, 50)
    __shabbos_short_leap = (
        52, 52, 53, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25,
        26,
        27, 28, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, -41, 43, 44, 45, 46, 47, 48, 49, -50)
    __shabbos_long_leap = (
        52, 52, 53, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25,
        26,
        27, 28, 28, 29, 30, 31, 32, 33, 34, 34, 35, 36, 37, -38, 40, -41, 43, 44, 45, 46, 47, 48, 49, -50)
    __mon_short_leap = (
        51, 52, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26,
        27,
        28, 28, 29, 30, 31, 32, 33, 34, 34, 35, 36, 37, -38, 40, -41, 43, 44, 45, 46, 47, 48, 49, -50)
    __mon_short_leap_Israel = (
        51, 52, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26,
        27,
        28, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, -41, 43, 44, 45, 46, 47, 48, 49, -50)
    __mon_long_leap = (
        51, 52, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26,
        27,
        28, 28, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, -41, 43, 44, 45, 46, 47, 48, 49, 50)
    __mon_long_leap_Israel = (
        51, 52, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26,
        27,
        28, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50)
    __thu_short_leap = (
        52, 53, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26,
        27,
        28, 29, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50)
    __thu_long_leap = (
        52, 53, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26,
        27,
        28, 29, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, -50)

    @staticmethod
    def getsedra(jd, israel):
        # If we are between the first day of Sukkos and Simchas Torah, the sedra will always be Vezos Habracha.
        if (jd.month == 7 and jd.day >= 15 and jd.day < (23 if israel else 24)):
            return Sedra.__sedraList[53],

        sedraOrder = Sedra.getSedraOrder(jd.year, israel)
        # find the first saturday on or after today's date
        absDate = Sedra.getDayOnOrBefore(6, jd.ordinal + 6)
        weekNum = int((absDate - sedraOrder['firstSatInYear']) / 7)
        index = 0

        if weekNum >= len(sedraOrder['sedraArray']):
            indexLast = sedraOrder['sedraArray'][len(sedraOrder['sedraArray'].length) - 1]
            if (indexLast < 0):
                # advance 2 parashiyot ahead after a doubled week
                index = (-indexLast) + 2
            else:
                index = indexLast + 1
        else:
            index = sedraOrder['sedraArray'][weekNum]

        if index >= 0:
            sedraArray = Sedra.__sedraList[index],
        else:
            i = -index  # undouble the sedra
            sedraArray = Sedra.__sedraList[i], Sedra.__sedraList[i + 1]

        return sedraArray

    @staticmethod
    def getDayOnOrBefore(day_of_week, date):
        return date - ((date - day_of_week) % 7)

    @staticmethod
    def getSedraOrder(year, israel):
        # If the last call is within the same year as this one, we reuse the data.
        # If memory is an issue, remove these next few lines
        if Sedra.__lastCalculatedYear is not None and Sedra.__lastCalculatedYear['year'] == year and \
                        Sedra.__lastCalculatedYear['israel'] == israel:
            return Sedra.__lastCalculatedYear

        longCheshvon = JDate.has_long_cheshvan(year)
        shortKislev = JDate.isShortKislev(year)
        roshHashana = JDate.toordinal(year, 7, 1)
        roshHashanaDOW = abs(roshHashana % 7)
        firstSatInYear = Sedra.getDayOnOrBefore(6, roshHashana + 6)
        yearType = 'regular'

        if (longCheshvon and not shortKislev):
            yearType = 'complete'
        elif (not longCheshvon and shortKislev):
            yearType = 'incomplete'

        if (not JDate.isJdLeapY(year)):
            if roshHashanaDOW == 6:
                if (yearType == "incomplete"):
                    sArray = Sedra.__shabbos_short
                elif (yearType == 'complete'):
                    sArray = Sedra.__shabbos_long
            elif roshHashanaDOW == 1:
                if (yearType == 'incomplete'):
                    sArray = Sedra.__mon_short
                elif (yearType == 'complete'):
                    sArray = Sedra.__mon_short if israel else Sedra.__mon_long
            elif roshHashanaDOW == 2:
                if (yearType == 'regular'):
                    sArray = Sedra.__mon_short if israel else Sedra.__mon_long
            elif roshHashanaDOW == 4:
                if (yearType == 'regular'):
                    sArray = Sedra.__thu_normal_Israel if israel else  Sedra.__thu_normal
                elif (yearType == 'complete'):
                    sArray = Sedra.__thu_long
            else:
                raise ValueError("improper sedra year type calculated.")
        # leap year
        else:
            if roshHashanaDOW == 6:
                if (yearType == 'incomplete'):
                    sArray = Sedra.__shabbos_short_leap
                elif (yearType == 'complete'):
                    sArray = Sedra.__shabbos_short_leap if israel else Sedra.__shabbos_long_leap
            elif roshHashanaDOW == 1:
                if (yearType == 'incomplete'):
                    sArray = Sedra.__mon_short_leap_Israel if israel else Sedra.__mon_short_leap
                elif (yearType == 'complete'):
                    sArray = Sedra.__mon_long_leap_Israel if israel else Sedra.__mon_long_leap
            elif roshHashanaDOW == 2:
                if (yearType == 'regular'):
                    sArray = Sedra.__mon_long_leap_Israel if israel else Sedra.__mon_long_leap
            elif roshHashanaDOW == 4:
                if (yearType == 'incomplete'):
                    sArray = Sedra.__thu_short_leap
                elif (yearType == 'complete'):
                    sArray = Sedra.__thu_long_leap
            else:
                raise ValueError("improper sedra year type calculated.")

        retobj = dict(firstSatInYear=firstSatInYear, sedraArray=sArray, year=year, israel=israel)

        # Save the data in case the next call is for the same year
        Sedra.__lastCalculatedYear = retobj

        return retobj


if __name__ == '__main__':
    sedras = Sedra.getsedra(JDate.today(), True)
    text = ' - '.join([s[0] for s in sedras])
    print(text)
