import json
import re


class Location:
    _locations_list_raw = {}

    def __init__(self, name, israel, latitude, longitude, utcOffset, elevation):
        self.name = name or 'Unknown Location'
        self.israel = israel or (latitude > 29.45 and latitude < 33 and longitude < -34.23 and longitude > -35.9)
        self.latitude = latitude
        self.longitude = longitude
        self.utcOffset = 2 if israel else (utcOffset or -int(longitude / 15))
        self.elevation = elevation or 0
        self.hebrew = name  # defaults to the English name
        self.candles = 0

    def __repr__(self):
        return 'name={}, israel={}, latitude={}, longitude={}, utcOffset={}, elevation={}'.format(
            self.n, self.i, self.latitude, self.longitude, self.utcOffset, self.elevation)

    @classmethod
    def get_location(cls, search_pattern):
        if not cls._locations_list_raw:
            file = open('./Files/LocationsList.json', 'r', encoding='utf-8')
            cls._locations_list_raw = json.load(file)

        if cls._locations_list_raw:
            r = re.compile(search_pattern)
            return (Location.parse(m) for m in cls._locations_list_raw['locations']
                    if r.match(m['n']) or (('h' in m) and r.match(m['h'])))

    @staticmethod
    def get_jerusalem():
        j = Location("Jerusalem", True, 31.78, -35.22, 2, 775)
        j.hebrew = 'ירושלים'
        j.candles = 40
        return j

    '''Parses a location that was loaded to a dict from the jason file of Locations.
    Sample entry:
        {'tz': '2', 'cl': '40', 'lt': '31.78', 'n': 'Jerusalem', 'el': '830', 'ln': '-35.22', 'h': 'ירושלים', 'i': 'y'} 
    The 'i', 'el', 'cl' and 'h' keys are optional.'''

    @staticmethod
    def parse(d):
        l = Location(
            name=d['n'],
            israel='i' in d and d['i'] == 'y',
            latitude=float(d['lt']),
            longitude=float(d['ln']),
            utcOffset=int(d['tz']),
            elevation=('el' in d and int(d['el'])) or 0)
        if 'h' in d:
            l.hebrew = d['h']
        if 'cl' in d:
            l.candles = int(d['cl'])
        return l
