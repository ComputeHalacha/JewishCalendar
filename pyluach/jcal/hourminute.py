class HourMinute:
    """
    Represents a very simple time structure of hour and minute.
    This class is the time representation object used for the Zmanim calculations.
    It is better suited for this purpose than the built-in datetime.time class.

    As zmanim are never exact enough to warrant using seconds, it only has hour and minute properties.

    Includes comparison, addition and subtraction operations.
    """

    def __init__(self, hour, minute):
        """
        Create a new HourMinute with the given hour and minute
        :param hour: Hours since midnight. If the number supplied is less than 0 or more than 23,
            it is assumed that we have "slipped" into the previous or next day for and the hour is set accordingly.
        :type hour: int
        :param minute: Minutes after the hour. If the number supplied is less than 0 or more than 59,
            it is assumed that we have "slipped" into the previous or next hour
            and the hour is adjusted accordingly (if the newly adjusted hour slips into another day,
            it is corrected).
        :type minute: int
        """
        while minute < 0:
            minute += 60
            hour -= 1
        while minute >= 60:
            minute -= 60
            hour += 1
        if hour < 0:
            hour = 24 + (hour % 24)
        if hour > 23:
            hour %= 24

        self._hour = hour
        self._minute = minute

    def __str__(self, army=True):
        return self.tostring(army)

    def __repr__(self):
        return 'HourMinute(hour=%r, minute=%r)' % (self._hour, self._minute)

    def __int__(self):
        return self.total_minutes

    def __add__(self, other):
        return self.add(other)

    def __sub__(self, other):
        return self.add(-other)

    def __eq__(self, other):
        return self._hour == other.hour and self._minute == other.minute

    def __lt__(self, other):
        return self.total_minutes < other.total_minutes

    def __gt__(self, other):
        return self.total_minutes > other.total_minutes

    def add(self, other):
        """
        Add either another HourMinute or a set number of minutes to the current one.
        :param other: Either an HourMinute to add to the current one or the number of minutes to add.
        :type other: HourMinute or int
        :return: The new HourMinute created by adding the two HourMinutes.
        :rtype: HourMinute
        """
        if isinstance(other, HourMinute):
            return HourMinute(self._hour + other.hour, self._minute + other.minute)
        elif isinstance(other, int):
            return HourMinute.from_minutes(self.total_minutes + other)

    def add_time(self, hours=0, minutes=0):
        """
        Add the given number of minutes and hours to the current time
        :param hours: The number of hours to add. If we "slip" into another day, the hour is adjusted accordingly.
        :type hours: int
        :param minutes: The number of minutes to add. If we "slip" into another hour, the hour is adjusted accordingly.
            If the newly adjusted hour "slips" into another day, it is adjusted again.
        :type minutes: int
        :return: The new HourMinute obtained from adding the given time to the current HourMinute
        :rtype: HourMinute
        """
        return HourMinute(self._hour + hours, self._minute + minutes)

    @property
    def hour(self):
        """
        The number of hours since midnight
        :return: The hour component of the current instance
        :rtype: int
        """
        return self._hour

    @property
    def minute(self):
        """
        The number of minutes after the hour
        :return: The minute component of the current instance
        :rtype: int
        """
        return self._minute

    @property
    def total_minutes(self):
        """
        The total number of minutes since zero hour/ midnight for the current HourMinute
        :return: The total number of minutes since zero hour/ midnight for the current HourMinute
        :rtype: int
        """
        return self._hour * 60 + self._minute

    def tostring(self, army=True):
        """
        Returns the current HourMinute as a formatted string.
        If army is falsey, the returned string will be: 11:42 PM
        otherwise it will be 23:42

        :param army: Should the 24 hour clock be used?
        :type army: bool
        :return: The current HourMinute as a formatted string.
        :rtype: str
        """
        if army:
            return '{}:{:02d}'.format(self._hour, self._minute)
        else:
            return '{}:{:02d} {}'.format(
                12 if self._hour % 12 == 0 else self._hour % 12,
                self._minute,
                'AM' if self._hour <= 12 else 'PM')

    @staticmethod
    def from_minutes(minutes):
        """
        Creates a new HourMinute from the given number of minutes since zero hour/midnight
        :param minutes: The number of minutes since midnight
        :type minutes: int
        :return: A new HourMinute
        :rtype: HourMinute
        """
        return HourMinute(int(minutes / 60), minutes % 60)
