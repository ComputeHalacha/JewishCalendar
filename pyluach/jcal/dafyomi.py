from collections import namedtuple
from datetime import date

import jcal.utils as Utils
from jcal.jdate import JDate

'''
  Computes the Day Yomi for the given day.
  Sample of use - to get today's daf:
      dafEng = Dafyomi.tostring(JDate.today())
      dafHeb = Dafyomi.tostring_heb(JDate.today())
  The code was converted to python and tweaked by CBS.
  It is directly based on the C code in Danny Sadinoff's HebCal - Copyright (C) 1994.
  The HebCal code for dafyomi was adapted by Aaron Peromsik from Bob Newell's public domain daf.el.'''


class Dafyomi:
    # Represents a single Masechta
    Masechta = namedtuple('Masechta', 'eng heb dappim')

    _masechtaList = [
        Masechta('Berachos', 'ברכות', 64),
        Masechta('Shabbos', 'שבת', 157),
        Masechta('Eruvin', 'ערובין', 105),
        Masechta('Pesachim', 'פסחים', 121),
        Masechta('Shekalim', 'שקלים', 22),
        Masechta('Yoma', 'יומא', 88),
        Masechta('Sukkah', 'סוכה', 56),
        Masechta('Beitzah', 'ביצה', 40),
        Masechta('Rosh Hashana', 'ראש השנה', 35),
        Masechta('Taanis', 'תענית', 31),
        Masechta('Megillah', 'מגילה', 32),
        Masechta('Moed Katan', 'מועד קטן', 29),
        Masechta('Chagigah', 'חגיגה', 27),
        Masechta('Yevamos', 'יבמות', 122),
        Masechta('Kesubos', 'כתובות', 112),
        Masechta('Nedarim', 'נדרים', 91),
        Masechta('Nazir', 'נזיר', 66),
        Masechta('Sotah', 'סוטה', 49),
        Masechta('Gitin', 'גיטין', 90),
        Masechta('Kiddushin', 'קדושין', 82),
        Masechta('Baba Kamma', 'בבא קמא', 119),
        Masechta('Baba Metzia', 'בבא מציעא', 119),
        Masechta('Baba Batra', 'בבא בתרא', 176),
        Masechta('Sanhedrin', 'סנהדרין', 113),
        Masechta('Makkot', 'מכות', 24),
        Masechta('Shevuot', 'שבועות', 49),
        Masechta('Avodah Zarah', 'עבודה זרה', 76),
        Masechta('Horayot', 'הוריות', 14),
        Masechta('Zevachim', 'זבחים', 120),
        Masechta('Menachos', 'מנחות', 110),
        Masechta('Chullin', 'חולין', 142),
        Masechta('Bechoros', 'בכורות', 61),
        Masechta('Arachin', 'ערכין', 34),
        Masechta('Temurah', 'תמורה', 34),
        Masechta('Kerisos', 'כריתות', 28),
        Masechta('Meilah', 'מעילה', 22),
        Masechta('Kinnim', 'קנים', 4),
        Masechta('Tamid', 'תמיד', 10),
        Masechta('Midos', 'מדות', 4),
        Masechta('Niddah', 'נדה', 73)]

    @classmethod
    def getdaf(cls, jd):
        ordinal = jd.ordinal
        dafcnt = 40
        osday = date(1923, 9, 11).toordinal()
        nsday = date(1975, 6, 24).toordinal()

        #  No cycle, new cycle, old cycle
        if ordinal < osday:
            return None  # [2] yomi hadn't started yet
        if ordinal >= nsday:
            cno = 8 + int(((ordinal - nsday) / 2711))
            dno = (ordinal - nsday) % 2711
        else:
            cno = 1 + int((ordinal - osday) / 2702)
            dno = int((ordinal - osday) / 2702)

        # Find the[2] taking note that the cycle changed slightly after cycle 7.
        total = blatt = 0
        count = -1

        #  Fix Shekalim for old cycles
        if cno <= 7:
            cls._masechtaList[4] = cls.Masechta('Shekalim', 'שקלים', 13)

        # Find the daf
        j = 0
        while (j < dafcnt):
            count += 1
            total = total + cls._masechtaList[j].dappim - 1
            if (dno < total):
                blatt = (cls._masechtaList[j].dappim + 1) - (total - dno)
                #  fiddle with the weird ones near the end
                if count == 36:
                    blatt += 21
                elif count == 37:
                    blatt += 24
                elif count == 38:
                    blatt += 33
                # Bailout
                j = 1 + dafcnt
            j += 1

        return cls._masechtaList[count], blatt

    # Returns the name of the Masechta and daf number in English, For example: Sukkah, Daf 3
    @classmethod
    def tostring(cls, jd):
        d = cls.getdaf(jd)
        return d[0].eng + ", Daf " + str(d[1])

    # Returns the name of the Masechta and daf number in Hebrew. For example: 'סוכה דף כ.
    @classmethod
    def tostring_heb(cls, jd):
        d = cls.getdaf(jd)
        return d[0].heb + " דף " + Utils.to_jnum(d[1])


if __name__ == '__main__':
    jd = JDate.today()
    dafEng = Dafyomi.tostring(jd)
    dafHeb = Dafyomi.tostring_heb(jd)
    print(dafEng)
    print(dafHeb)
