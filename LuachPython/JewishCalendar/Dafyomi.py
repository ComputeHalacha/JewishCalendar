from JewishCalendar.Utils import Utils
from JewishCalendar.JewishDate import JewishDate
from datetime import date

'''
  Computes the Day Yomi for the given day.
  Sample of use - to get todays daf:
      dafEng = Dafyomi.toString(JewishDate.today())
      dafHeb = Dafyomi.toStringHeb(JewishDate.today())
  The code was converted to python and tweaked by CBS.
  It is directly based on the C code in Danny Sadinoff's HebCal - Copyright (C) 1994.
  The HebCal code for dafyomi was adapted by Aaron Peromsik from Bob Newell's public domain daf.el.'''


class Dafyomi:

    __masechtaList = (
        ['Berachos', 'ברכות', 64],
        ['Shabbos', 'שבת', 157],
        ['Eruvin', 'ערובין', 105],
        ['Pesachim', 'פסחים', 121],
        ['Shekalim', 'שקלים', 22],
        ['Yoma', 'יומא', 88],
        ['Sukkah', 'סוכה', 56],
        ['Beitzah', 'ביצה', 40],
        ['Rosh Hashana', 'ראש השנה', 35],
        ['Taanis', 'תענית', 31],
        ['Megillah', 'מגילה', 32],
        ['Moed Katan', 'מועד קטן', 29],
        ['Chagigah', 'חגיגה', 27],
        ['Yevamos', 'יבמות', 122],
        ['Kesubos', 'כתובות', 112],
        ['Nedarim', 'נדרים', 91],
        ['Nazir', 'נזיר', 66],
        ['Sotah', 'סוטה', 49],
        ['Gitin', 'גיטין', 90],
        ['Kiddushin', 'קדושין', 82],
        ['Baba Kamma', 'בבא קמא', 119],
        ['Baba Metzia', 'בבא מציעא', 119],
        ['Baba Batra', 'בבא בתרא', 176],
        ['Sanhedrin', 'סנהדרין', 113],
        ['Makkot', 'מכות', 24],
        ['Shevuot', 'שבועות', 49],
        ['Avodah Zarah', 'עבודה זרה', 76],
        ['Horayot', 'הוריות', 14],
        ['Zevachim', 'זבחים', 120],
        ['Menachos', 'מנחות', 110],
        ['Chullin', 'חולין', 142],
        ['Bechoros', 'בכורות', 61],
        ['Arachin', 'ערכין', 34],
        ['Temurah', 'תמורה', 34],
        ['Kerisos', 'כריתות', 28],
        ['Meilah', 'מעילה', 22],
        ['Kinnim', 'קנים', 4],
        ['Tamid', 'תמיד', 10],
        ['Midos', 'מדות', 4],
        ('Niddah', 'נדה', 73))

    @staticmethod
    def getDaf(jd):
        ordinal = jd.ordinal
        dafcnt = 40
        osday = date(1923, 8, 11).toordinal()
        nsday = date(1975, 5, 24).toordinal()

        #  No cycle, new cycle, old cycle
        if (ordinal < osday):
            return None # [2] yomi hadn't started yet
        if (ordinal >= nsday):
            cno = 8 + int(((ordinal - nsday) / 2711))
            dno = (ordinal - nsday) % 2711
        else:
            cno = 1 + int((ordinal - osday) / 2702)
            dno = int((ordinal - osday) / 2702)

        # Find the[2] taking note that the cycle changed slightly after cycle 7.
        total = blatt = 0
        count = -1

        #  Fix Shekalim for old cycles
        if (cno <= 7):
            Dafyomi.__masechtaList[4][2] = 13
        else:
            Dafyomi.__masechtaList[4][2] = 22

        #  Find the daf
        j = 0
        while (j < dafcnt):
            count += 1
            total = total + Dafyomi.__masechtaList[j][2] - 1
            if (dno < total):
                blatt = (Dafyomi.__masechtaList[j][2] + 1) - (total - dno)
                #  fiddle with the weird ones near the end
                if count == 36:
                    blatt += 21
                elif count == 37:
                    blatt += 24
                elif count == 38:
                    blatt += 33
                #  Bailout
                j = 1 + dafcnt
            j += 1

        return Dafyomi.__masechtaList[count], blatt


    # Returns the name of the Masechta and[2] number in English, For example: Sukkah, Daf 3
    @staticmethod
    def toString(jd):
        d = Dafyomi.getDaf(jd)
        return d[0][0] + ", Daf " + str(d[1])

    # Returns the name of the Masechta and[2] number in Hebrew. For example: 'סוכה דף כ.
    @staticmethod
    def toStringHeb(jd):
        d = Dafyomi.getDaf(jd)
        return d[0][1] + " דף " + Utils.toJNum(d[1])
        
if __name__ == '__main__':
    jd = JewishDate.today()
    dafEng = Dafyomi.toString(jd)
    dafHeb = Dafyomi.toStringHeb(jd)
    print(dafEng)
    print(dafHeb)

