"""
This module contains the date conversion functions for pyluach.

It contains functions to convert Jewish Dates, Gregorian Dates and Julian Dates to and from one another.

For the conversions of Jewish Dates to and from Gregorian dates, one of two distinct algorithms are used.

If the date is after the start of the common era, than the calculations used are based on the respected algorithms
in "Calendrical Calculations" by Nachum Dershowitz and Edward M. Reingold.

For dates before the common era, a round-about calculation is used by converting the Jewish Date to a Julian date and
then converting the Julian date to a Gregorian Date, and vice versa.

The conversions to and from the Julian dates were adapted from the convertdate package at http://github.com/fitnr/convertdate
- Licensed under the GPL-v3.0 license: http://opensource.org/licenses/MIT Copyright (c) 2016, fitnr <fitnr@fakeisthenewreal>
"""

import datetime
import sys
from calendar import isleap as is_greg_leap
from math import floor as mfloor, trunc as trunc

import jcal.utils as utils

if "jcal.jdate" not in sys.modules.keys():
    # prevent circular referencing
    from jcal.jdate import JDate

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


def jdate_to_julian(jdate):
    year, month, day = jdate.year, jdate.month, jdate.day
    months = JDate.months_in_jyear(year)
    jd = EPOCH_JDATE + _jdate_delay_1(year) + _jdate_delay_2(year) + day + 1

    if month < 7:
        for mon in range(7, months + 1):
            jd += JDate.days_in_jmonth(year, mon)

        for mon in range(1, month):
            jd += JDate.days_in_jmonth(year, mon)
    else:
        for mon in range(7, month):
            jd += JDate.days_in_jmonth(year, mon)

    return int(jd) + 0.5


def julian_to_jdate(julian_date):
    julian_date = trunc(julian_date) + 0.5
    count = trunc(((julian_date - EPOCH_JDATE) * 98496.0) / 35975351.0)
    year = count - 1
    i = count
    while julian_date >= jdate_to_julian(i, 7, 1):
        i += 1
        year += 1

    if julian_date < jdate_to_julian(year, 1, 1):
        first = 7
    else:
        first = 1

    month = i = first
    while julian_date > jdate_to_julian(year, i, JDate.days_in_jmonth(year, i)):
        i += 1
        month += 1

    day = int(julian_date - jdate_to_julian(year, month, 1)) + 1
    return JDate(year, month, day)


def greg_to_jdate(date_or_year, month=None, day=None):
    """
    date_or_year can be:
      - an instance of the built-in datetime.date/datetime.datetime class
      - a utils.GregorianDate namedtuple
      - an int representing the Gregorian year
     """
    if _is_legal_greg_date(date_or_year, month, day):
        if isinstance(date_or_year, datetime.date):
            return JDate.fromordinal(date_or_year.toordinal())

        if isinstance(date_or_year, int):
            y, m, d = date_or_year, month or 1, day or 1
        else:
            y, m, d = date_or_year  # unpack the utils.GregorianDate namedtuple

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


def greg_to_julian(date_or_year, month=None, day=None):
    """
    date_or_year can be:
      - a datetime.date object
      - a utils.GregorianDate namedtuple
      - an int representing the Gregorian year
     """
    if _is_legal_greg_date(date_or_year, month, day):
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
            _floor((year - 1) / 4) + (-_floor((year - 1) / 100)) +
            _floor((year - 1) / 400) + _floor((((367 * month) - 362) / 12) + leap_adj + day)
        )


def julian_to_greg(julian_date):
    '''Return Gregorian date in a utils.GregorianDate(Y, M, D) namedtuple'''
    wjd = _floor(julian_date - 0.5) + 0.5
    depoch = wjd - EPOCH_GREG

    quadricent = _floor(depoch / INTERCALATION_CYCLE_DAYS)
    dqc = depoch % INTERCALATION_CYCLE_DAYS

    cent = _floor(dqc / LEAP_SUPPRESSION_DAYS)
    dcent = dqc % LEAP_SUPPRESSION_DAYS

    quad = _floor(dcent / LEAP_CYCLE_DAYS)
    dquad = dcent % LEAP_CYCLE_DAYS

    yindex = _floor(dquad / 365)
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

    month = _floor((((yearday + leap_adj) * 12) + 373) / 367)
    day = int(wjd - greg_to_julian(year, month, 1)) + 1

    return utils.GregorianDate(year, month, day)


def _floor(x):
    return int(mfloor(x))


def _jdate_delay_1(jyear):
    '''Test for delay of start of new jyear and to avoid'''
    # //  Sunday, Wednesday, and Friday as start of the new jyear.
    months = trunc(((235 * jyear) - 234) / 19)
    parts = 12084 + (13753 * months)
    day = trunc((months * 29) + parts / 25920)

    if ((3 * (day + 1)) % 7) < 3:
        day += 1

    return day


def _jdate_delay_2(jyear):
    '''Check for delay in start of new jyear due to length of adjacent years'''

    last = _jdate_delay_1(jyear - 1)
    present = _jdate_delay_1(jyear)
    next_ = _jdate_delay_1(jyear + 1)

    if next_ - present == 356:
        return 2
    elif present - last == 382:
        return 1
    else:
        return 0


def _is_legal_greg_date(date_or_year, month=None, day=None):
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
