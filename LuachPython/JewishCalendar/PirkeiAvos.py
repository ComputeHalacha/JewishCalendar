from JewishCalendar.JewishDate import JewishDate
'''****************************************************************************************************************
 * Computes the Perek/Prakim of the week for the given Shabbos.
 * Returns a list of prakim (integers) (either one or two) for the given Jewish Date
 * Sample of use to get the perek for Shabbos Chukas 5776 in Israel:
 *     from JewishDate import JewishDate
 *     from Utils import Utils
 *     import PirkeiAvos
 *     chukas5776 = JewishDate.create(5776, 4, 3)
 *     prakim = PirkeiAvos.getpirkeiavos(chukas5776, True)
 *     text = 'Pirkei Avos: ' + ' and '.join([Utils.toSuffixed(p) +
 *     ' Perek' for p in prakim])
 *     print(text)
 ***************************************************************************************************************'''


def getpirkeiavos(jd, israel):
    if (jd.getDayOfWeek() != 6):
        return []

    jYear = jd.year
    jMonth = jd.month
    jDay = jd.day

    # Pirkei Avos is from after Pesach until Rosh Hashana
    # All Shabbosim through Iyar, Sivan, Tamuz, Av - besides for the day/s of Shavuos and Tisha B'Av
    if (jMonth == 1 and jDay > (21 if israel else 22)) or ((jMonth > 1 and jMonth < 6 and (not ((jMonth == 3 and jDay == 6) or (not israel and jMonth == 3 and jDay == 7))) and (not (jMonth == 5 and jDay == 9)))):
        return [get1stPerek(jd, israel)]

    # Ellul can have multiple prakim
    elif (jMonth == 6):
        return ellul(jd, israel)
        # No Pirkei Avos
    else:
        return []

def get1stPerek(jd, israel):
    jYear = jd.year
    jMonth = jd.month
    jDay = jd.day
    pes1 = JewishDate.create(jYear, 1, 15)
    # How many days after the first day of pesach was the first shabbos after pesach
    shb1 = (7 if israel else 8) + (6 - pes1.getDayOfWeek())
    # What number shabbos after pesach is the current date
    cShb = (1 if (jMonth == 1 and jDay == (shb1 + 15)) else
        int((jd.ordinal - (pes1.ordinal + shb1)) / 7) + 1)
    prk = cShb % 6
    if (prk == 0):
        prk = 6
    # If the second day of Shavuos was on Shabbos, we missed a week.
    # The second day of Pesach is always the same day as the first day of Shavuos.
    # So if Pesach was on Thursday, Shavuos will be on Friday and Shabbos in Chu"l.
    # Pesach can never come out on Friday, so in E. Yisroel Shavuos is never on Shabbos.
    if ((not israel) and pes1.getDayOfWeek() == 4 and (jMonth > 3 or (jMonth == 3 and jDay > 6))):
        prk = 6 if prk == 1 else prk - 1
    # If Tisha B'Av was on Shabbos, we missed a week. The first day of Pesach is always the same day of the week as Tisha b'av.
    if (pes1.getDayOfWeek() == 6 and (jMonth > 5 or (jMonth == 5 and jDay > 9))):
        prk = 6 if prk == 1 else prk - 1
    return prk


def ellul (jd, israel):
    prakim = None
    jYear = jd.year
    jMonth = jd.month
    jDay = jd.day
    # The first day of Ellul.
    # The year/month/day/ordinal constructor is used for efficiency.
    day1 = JewishDate(jYear, 6, 1, jd.ordinal - jd.day + 1)
    day1DOW = day1.getDayOfWeek()
    shabbos1Day = 1 if day1DOW == 6 else ((6 - (day1DOW + 6) % 6) + 1)
    shabbos1Date = JewishDate(jYear, 6, shabbos1Day, day1.ordinal + shabbos1Day - 1)
    # Which shabbos in Ellul are we working out now?
    cShb = 1 if jDay == shabbos1Day else int((jDay - shabbos1Day) / 7) + 1

    perek1 = get1stPerek(shabbos1Date, israel)
    if perek1 == 1:
        if cShb == 1:
            prakim = [1]
        elif cShb == 2:
            prakim = [2]
        elif cShb == 3:
            prakim = [3, 4]
        elif cShb == 4:
            prakim = [5, 6]
    elif perek1 == 2:
            if cShb == 1:
                prakim = [2]
            elif cShb == 2:
                prakim = [3]
            elif cShb == 3:
                prakim = [4]
            elif cShb == 4:
                prakim = [5, 6]
    elif perek1 == 3:
            if cShb == 1:
                prakim = [3]
            elif cShb == 2:
                prakim = [4]
            elif cShb == 3:
                prakim = [5]
            elif cShb == 4:
                prakim = [6]
    elif perek1 == 4:
        # This can only happen in Chutz La'aretz
            if cShb == 1:
                prakim = [4, 5]
            elif cShb == 2:
                prakim = [6, 1]
            elif cShb == 3:
                prakim = [2, 3]
            elif cShb == 4:
                prakim = [4, 5, 6]
    elif perek1 == 5:
            if cShb == 1:
                prakim = [5, 6]
            elif cShb == 2:
                prakim = [1, 2]
            elif cShb == 3:
                prakim = [3, 4]
            elif cShb == 4:
                prakim = [5, 6]
    elif perek1 == 6:
            if cShb == 1:
                prakim = [6]
            elif cShb == 2:
                prakim = [1, 2]
            elif cShb == 3:
                prakim = [3, 4]
            elif cShb == 4:
                prakim = [5, 6]
    return prakim or []
        
if __name__ == '__main__':
    from Utils import Utils
    chukas5776 = JewishDate.create(5776, 4, 3)
    prakim = getpirkeiavos(chukas5776, True)
    text = 'Pirkei Avos: ' + ' and '.join([Utils.toSuffixed(p) + ' Perek' for p in prakim])
    print(text)
