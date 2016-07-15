class HourMinute:
    def __init__(self, hour, minute):
        while (minute < 0):
            minute += 60
            hour -= 1
        while minute >= 60:
            minute -= 60
            hour += 1
        if (hour < 0):
            hour = 24 + (hour % 24)
        if (hour > 23):
            hour = hour % 24

        self.hour = hour
        self.minute = minute

    def __str__(self, army=True):
        return self.tostring(army)

    def __repr__(self):
        return 'HourMinute(hour=%r, minute=%r)' % (self.hour, self.minute)

    def __int__(self):
        return self.totalMinutes()

    def __add__(self, other):
        return self.add(other)

    def __sub__(self, other):
        return self.add(-other)

    def __eq__(self, other):
        return self.hour == other.hour and self.minute == other.minute

    def add(self, other):
        if isinstance(other, HourMinute):
            return HourMinute(self.hour + other.hour, self.minute + other.minute)
        elif isinstance(other, int):
            return HourMinute.fromMinutes(self.totalMinutes() + other)

    # Add the given number of minutes and hours to the given time
    def addtime(self, hours=0, minutes=0):
        return HourMinute(self.hour + hours, self.minute + minutes)

    # Gets the total number of minutes in the given time
    def totalMinutes(self) -> int:
        return self.hour * 60 + self.minute

    # Returns the given time in a formatted string.
    # if army is falsey, the returned string will be: 11:42 PM
    # otherwise it will be 23:42
    def tostring(self, army=True):
        if army:
            return '{}:{:02d}'.format(self.hour, self.minute)
        else:
            return '{}:{:02d} {}'.format(
                12 if self.hour % 12 == 0 else self.hour % 12,
                self.minute,
                'AM' if self.hour <= 12 else 'PM')

    @staticmethod
    def fromMinutes(minutes):
        return HourMinute(int(minutes / 60), minutes % 60)
