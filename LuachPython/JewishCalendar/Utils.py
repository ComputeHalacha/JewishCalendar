class Utils:
    jMonthsEng = ["", "Nissan", "Iyar", "Sivan", "Tamuz", "Av", "Ellul", "Tishrei", "Cheshvan", "Kislev", "Teves", "Shvat", "Adar", "Adar Sheini"]
    jMonthsHeb = ["", "ניסן", "אייר", "סיון", "תמוז", "אב", "אלול", "תשרי", "חשון", "כסלו", "טבת", "שבט", "אדר", "אדר שני"]
    sMonthsEng = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"]
    dowEng = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Erev Shabbos", "Shabbos Kodesh"]
    dowHeb = ["יום ראשון", "יום שני", "יום שלישי", "יום רביעי", "יום חמישי", "ערב שבת קודש", "שבת קודש"]
    jsd = ['א', 'ב', 'ג', 'ד', 'ה', 'ו', 'ז', 'ח', 'ט']
    jtd = ['י', 'כ', 'ל', 'מ', 'נ', 'ס', 'ע', 'פ', 'צ']
    jhd = ['ק', 'ר', 'ש', 'ת']
    jsnum = ["", "אחד", "שנים", "שלשה", "ארבעה", "חמשה", "ששה", "שבעה", "שמונה", "תשעה"]
    jtnum = ["", "עשר", "עשרים", "שלושים", "ארבעים"]
    
        
    # Gets the Jewish representation of a number (365 = שס"ה)
    # Minimum number is 1 and maximum is 9999.
    def toJNum(number):
        if (number < 1):
            raise RuntimeError("Min value is 1")
        if (number > 9999):
            raise RuntimeError("Max value is 9999")

        n = number
        retval = ''
    
        if n >= 1000:
            retval += Utils.jsd[int((n - (n % 1000)) / 1000) - 1] + "'"
            n = n % 1000
        
        while n >= 400:
            retval += 'ת'
            n -= 400
        
        if n >= 100:
            retval += Utils.jhd[int((n - (n % 100)) / 100) - 1]
            n = n % 100
    
        if n == 15:
            retval += "טו"
        elif n == 16:
            retval += "טז"
        else:
            if n > 9:
                retval += Utils.jtd[int((n - (n % 10)) / 10) - 1]
            if (n % 10) > 0:
                retval += Utils.jsd[(n % 10) - 1]
        if number > 999 and (number % 1000 < 10):
            retval = "'" + retval
        elif len(retval) > 1:
            retval = retval[:-1] + '"' + retval[-1]
        
        return retval
        
    # Add two character suffix to number. e.g. 21st, 102nd, 93rd, 500th
    def toSuffixed(num):
        t = str(num)
        suffix = "th"
        if len(t) == 1 or t[:-2] != '1':
            last = t[:-1]
            if last == '1':
                suffix = "st"
            elif last == '2':
                suffix = "nd"
            elif last == '3':
                suffix = "rd"
        return t + suffix;
