# This file was adatped for pyluach from convertdate.
# http://github.com/fitnr/convertdate

# Licensed under the GPL-v3.0 license:
# http://opensource.org/licenses/MIT
# Copyright (c) 2016, fitnr <fitnr@fakeisthenewreal>

import datetime
from calendar import isleap as is_greg_leap
from math import floor as mfloor, trunc as trunc
from jcal.jdate import JDate

import jcal.jdate
import jcal.utils as utils

EPOCH_GREG = 1721425.5
INTERCALATION_CYCLE_YEARS = 400
INTERCALATION_CYCLE_DAYS = 146097
LEAP_SUPPRESSION_YEARS = 100
LEAP_SUPPRESSION_DAYS = 36524
LEAP_CYCLE_YEARS = 4
LEAP_CYCLE_DAYS = 1461
HAVE_30_DAYS = (4, 6, 9, 11)
HAVE_31_DAYS = (1, 3, 5, 7, 8, 10, 12)
EPOCH_JDATE = 347995.5
HEBREW_YEAR_OFFSET = 3760


def floor(x):
    return int(mfloor(x))


def months_in_jdate_year(year):
    '''How many months are there in a Hebrew year (12 = normal, 13 = leap)'''
    if JDate.isleap_jyear(year):
        return 13
    else:
        return 12


def days_in_jdate_year(year):
    '''How many days are in a Hebrew year ?'''
    return jdate_to_julian(year + 1, 7, 1) - jdate_to_julian(year, 7, 1)


def days_in_jdate_month(year, month):
    '''How many days are in a given month of a given year'''
    if month > 13:
        raise ValueError("Incorrect month index")

    # First of all, dispose of fixed-length 29 day months
    if month in (2, 4, 6, 10, 13):
        return 29

    # If it's not a leap year, Adar has 29 days
    if month == 12 and not JDate.isleap_jyear(year):
        return 29

    # If it's Heshvan, days depend on length of year
    if month == 8 and (days_in_jdate_year(year) % 10) != 5:
        return 29

    # Similarly, Kislev varies with the length of year
    if month == 9 and (days_in_jdate_year(year) % 10) == 3:
        return 29

    # Nope, it's a 30 day month
    return 30


def delay_1(year):
    '''Test for delay of start of new year and to avoid'''
    # //  Sunday, Wednesday, and Friday as start of the new year.
    months = trunc(((235 * year) - 234) / 19)
    parts = 12084 + (13753 * months)
    day = trunc((months * 29) + parts / 25920)

    if ((3 * (day + 1)) % 7) < 3:
        day += 1

    return day


def delay_2(year):
    '''Check for delay in start of new year due to length of adjacent years'''

    last = delay_1(year - 1)
    present = delay_1(year)
    next_ = delay_1(year + 1)

    if next_ - present == 356:
        return 2
    elif present - last == 382:
        return 1
    else:
        return 0


def jdate_to_julian(jdate):
    year, month, day = jdate.year, jdate.month, jdate.day
    months = months_in_jdate_year(year)
    jd = EPOCH_JDATE + delay_1(year) + delay_2(year) + day + 1

    if month < 7:
        for mon in range(7, months + 1):
            jd += days_in_jdate_month(year, mon)

        for mon in range(1, month):
            jd += days_in_jdate_month(year, mon)
    else:
        for mon in range(7, month):
            jd += days_in_jdate_month(year, mon)

    return int(jd) + 0.5


def julian_to_jdate(jd):
    jd = trunc(jd) + 0.5
    count = trunc(((jd - EPOCH_JDATE) * 98496.0) / 35975351.0)
    year = count - 1
    i = count
    while jd >= jdate_to_julian(i, 7, 1):
        i += 1
        year += 1

    if jd < jdate_to_julian(year, 1, 1):
        first = 7
    else:
        first = 1

    month = i = first
    while jd > jdate_to_julian(year, i, days_in_jdate_month(year, i)):
        i += 1
        month += 1

    day = int(jd - jdate_to_julian(year, month, 1)) + 1
    return JDate(year, month, day)


def greg_to_jdate(date_or_year, month=None, day=None):
    """
    date_or_year can be:
      - a datetime.date object
      - a utils.GregorianDate namedtuple
      - an int representing the Gregorian year
     """
    if legal_greg_date(date_or_year, month, day):
        if isinstance(date_or_year, datetime.date):
            return JDate.fromordinal(date_or_year.toordinal())

        if isinstance(date_or_year, int):
            y, m, d = date_or_year, month or 1, day or 1
        else:
            y, m, d = date_or_year

        if y > 0:
            return JDate.fromordinal(datetime.date(y, m, d).toordinal())
        else:
            return julian_to_jdate(greg_to_julian(d, m, y))


def jdate_to_greg(jdate):
    if jdate.ordinal > 0:
        sd = datetime.datetime.fromordinal(jdate.ordinal)
        return utils.GregorianDate(year=sd.year, month=sd.month, day=sd.day)
    else:
        return julian_to_greg(jdate_to_julian(jdate))


def legal_greg_date(date_or_year, month=None, day=None):
    """
    date_or_year can be:
      - a datetime.date object
      - a utils.GregorianDate namedtuple
      - an int representing the Gregorian year
     """
    if isinstance(date_or_year, int):
        year, month, day = date_or_year, month or 1, day or 1
    else:
        year, month, day = date_or_year.year, date_or_year.month, date_or_year.day

    '''Check if this is a legal date in the Gregorian calendar'''
    if month == 2:
        daysinmonth = 29 if is_greg_leap(year) else 28
    else:
        daysinmonth = 30 if month in HAVE_30_DAYS else 31

    if not (0 < day <= daysinmonth):
        raise IndexError("Month {} doesn't have a day {}".format(month, day))

    return True


def greg_to_julian(date_or_year, month=None, day=None):
    """
    date_or_year can be:
      - a datetime.date object
      - a utils.GregorianDate namedtuple
      - an int representing the Gregorian year
     """
    if legal_greg_date(date_or_year, month, day):
        if isinstance(date_or_year, int):
            year, month, day = date_or_year, month or 1, day or 1
        else:
            year, month, day = date_or_year.year, date_or_year.month, date_or_year.day

        if month <= 2:
            leap_adj = 0
        elif is_greg_leap(year):
            leap_adj = -1
        else:
            leap_adj = -2

        return (
            EPOCH_GREG - 1 + (365 * (year - 1)) +
            floor((year - 1) / 4) + (-floor((year - 1) / 100)) +
            floor((year - 1) / 400) + floor((((367 * month) - 362) / 12) + leap_adj + day)
        )


def julian_to_greg(jd):
    '''Return Gregorian date in a utils.GregorianDate(Y, M, D) namedtuple'''
    wjd = floor(jd - 0.5) + 0.5
    depoch = wjd - EPOCH_GREG

    quadricent = floor(depoch / INTERCALATION_CYCLE_DAYS)
    dqc = depoch % INTERCALATION_CYCLE_DAYS

    cent = floor(dqc / LEAP_SUPPRESSION_DAYS)
    dcent = dqc % LEAP_SUPPRESSION_DAYS

    quad = floor(dcent / LEAP_CYCLE_DAYS)
    dquad = dcent % LEAP_CYCLE_DAYS

    yindex = floor(dquad / 365)
    year = (
        quadricent * INTERCALATION_CYCLE_YEARS +
        cent * LEAP_SUPPRESSION_YEARS +
        quad * LEAP_CYCLE_YEARS + yindex
    )

    if not (cent == 4 or yindex == 4):
        year += 1

    yearday = wjd - greg_to_julian(year, 1, 1)

    isleap = is_greg_leap(year)

    if yearday < 58 + isleap:
        leap_adj = 0
    elif isleap:
        leap_adj = 1
    else:
        leap_adj = 2

    month = floor((((yearday + leap_adj) * 12) + 373) / 367)
    day = int(wjd - greg_to_julian(year, month, 1)) + 1

    return utils.GregorianDate(year, month, day)
