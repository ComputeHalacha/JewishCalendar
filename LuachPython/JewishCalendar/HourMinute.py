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

    def __repr__(self):
        return self.tostring()
    
    def __add__(self, other):
        return self.add(other)
        
    def __eq__(self, other):
        return self.hour == other.hour and self.minute == other.minute
    
    def add(self, other):
        if isinstance(other, HourMinute):
            return HourMinute(self.hour + hm.hour, self.minute + hm.minute)
        elif isinstance(other, int):
            return HourMinute.fromMinutes(self.totalMinutes() + other)
        
    # Add the given number of minutes and hours to the given time
    def addtime(self, hours=0, minutes=0):
        return HourMinute(self.hour + hours, self.minute + minutes)

    # Gets the total number of minutes in the given time
    def totalMinutes(self):
        return self.hour * 60 + self.minute

    # Returns the given time in a formatted string.
    # if army is falsey, the returned string will be: 11:42 PM
    # otherwise it will be 23:42
    def tostring(self, army=True):
        if army:
            return '{0:02d}:{1:02d}'.format(self.hour, self.minute)
        else:
            return '{0}:{1:02d} {2}'.format(self.hour % 12,
                    self.minute,
                    'AM' if self.hour <= 12 else 'PM')
    
    @staticmethod                
    def fromMinutes(minutes):
        return HourMinute(int(minutes / 60), minutes % 60)
