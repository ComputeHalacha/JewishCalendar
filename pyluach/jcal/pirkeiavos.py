from jcal.jdate import JDate

'''****************************************************************************************************************
 * Computes the Perek/Prakim of the week for the given Shabbos.
 * Returns a list of prakim (integers) (either one or two) for the given Jewish Date
 * Sample of use to get the perek for Shabbos Chukas 5776 in Israel:
 *     from JDate import JDate
 *     from Utils import Utils
 *     import PirkeiAvos
 *     chukas5776 = JDate.create(5776, 4, 3)
 *     prakim = PirkeiAvos.getpirkeiavos(chukas5776, True)
 *     text = 'Pirkei Avos: ' + ' and '.join([Utils.toSuffixed(p) +
 *     ' Perek' for p in prakim])
 *     print(text)
 ***************************************************************************************************************'''


def getpirkeiavos(jd, israel):
    if jd.getdow() != 6:
        return []

    jmonth = jd.month
    jday = jd.day

    # Pirkei Avos is from after Pesach until Rosh Hashana
    # All Shabbosim through Iyar, Sivan, Tamuz, Av - besides for the day/s of Shavuos and Tisha B'Av
    if (jmonth == 1 and jday > (21 if israel else 22)) or ((1 < jmonth < 6 and (
            not ((jmonth == 3 and jday == 6) or (not israel and jmonth == 3 and jday == 7))) and (
            not (jmonth == 5 and jday == 9)))):
        return [get1stPerek(jd, israel)]

    # Ellul can have multiple prakim
    elif jmonth == 6:
        return ellul(jd, israel)
        # No Pirkei Avos
    else:
        return []


def get1stPerek(jd, israel):
    jyear = jd.year
    jmonth = jd.month
    jday = jd.day
    pes1 = JDate.create(jyear, 1, 15)
    # How many days after the first day of pesach was the first shabbos after pesach
    shb1 = (7 if israel else 8) + (6 - pes1.getdow())
    # What number shabbos after pesach is the current date
    cshb = (1 if (jmonth == 1 and jday == (shb1 + 15)) else
            int((jd.ordinal - (pes1.ordinal + shb1)) / 7) + 1)
    prk = cshb % 6
    if prk == 0:
        prk = 6
    # If the second day of Shavuos was on Shabbos, we missed a week.
    # The second day of Pesach is always the same day as the first day of Shavuos.
    # So if Pesach was on Thursday, Shavuos will be on Friday and Shabbos in Chu"l.
    # Pesach can never come out on Friday, so in E. Yisroel Shavuos is never on Shabbos.
    if (not israel) and pes1.getdow() == 4 and (jmonth > 3 or (jmonth == 3 and jday > 6)):
        prk = 6 if prk == 1 else prk - 1
    # If Tisha B'Av was on Shabbos, we missed a week. 
    # The first day of Pesach is always the same day of the week as Tisha b'av.
    if pes1.getdow() == 6 and (jmonth > 5 or (jmonth == 5 and jday > 9)):
        prk = 6 if prk == 1 else prk - 1
    return prk


def ellul(jd, israel):
    praklist = ()
    jyear = jd.year
    jday = jd.day
    # The first day of Ellul.
    # The year/month/day/ordinal constructor is used for efficiency.
    day1 = JDate(jyear, 6, 1, jd.ordinal - jd.day + 1)
    day1dow = day1.getdow()
    shabbos1day = 1 if day1dow == 6 else ((6 - (day1dow + 6) % 6) + 1)
    shabbos1date = JDate(jyear, 6, shabbos1day, day1.ordinal + shabbos1day - 1)
    # Which shabbos in Ellul are we working out now?
    cshb = 1 if jday == shabbos1day else int((jday - shabbos1day) / 7) + 1

    perek1 = get1stPerek(shabbos1date, israel)
    if perek1 == 1:
        if cshb == 1:
            praklist = 1,
        elif cshb == 2:
            praklist = 2,
        elif cshb == 3:
            praklist = 3, 4
        elif cshb == 4:
            praklist = 5, 6
    elif perek1 == 2:
        if cshb == 1:
            praklist = 2,
        elif cshb == 2:
            praklist = 3,
        elif cshb == 3:
            praklist = 4,
        elif cshb == 4:
            praklist = 5, 6
    elif perek1 == 3:
        if cshb == 1:
            praklist = 3,
        elif cshb == 2:
            praklist = 4,
        elif cshb == 3:
            praklist = 5,
        elif cshb == 4:
            praklist = 6,
    elif perek1 == 4:
        # This can only happen in Chutz La'aretz
        if cshb == 1:
            praklist = 4, 5
        elif cshb == 2:
            praklist = 6, 1
        elif cshb == 3:
            praklist = 2, 3
        elif cshb == 4:
            praklist = 4, 5, 6
    elif perek1 == 5:
        if cshb == 1:
            praklist = 5, 6
        elif cshb == 2:
            praklist = 1, 2
        elif cshb == 3:
            praklist = 3, 4
        elif cshb == 4:
            praklist = 5, 6
    elif perek1 == 6:
        if cshb == 1:
            praklist = 6,
        elif cshb == 2:
            praklist = 1, 2
        elif cshb == 3:
            praklist = 3, 4
        elif cshb == 4:
            praklist = 5, 6
    return praklist


if __name__ == '__main__':
    import jcal.utils as Utils

    chukas5776 = JDate.create(5776, 4, 10)
    prakim = getpirkeiavos(chukas5776, True)
    text = 'Pirkei Avos: ' + ' and '.join([Utils.toSuffixed(p) + ' Perek' for p in prakim])
    print(text)
