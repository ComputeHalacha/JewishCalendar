import json
import re


class Location:
    _locations_list_raw = {}

    def __init__(self, latitude, longitude, utcoffset=0, elevation=0, name=None, israel=False):
        self.latitude = latitude
        self.longitude = longitude
        self.israel = israel if israel is bool else \
            ((29.45 < latitude < 33) and (-34.23 > longitude > -35.9))
        self.utcoffset = utcoffset if utcoffset is int else \
            (2 if israel else -(longitude // 15))
        self.elevation = elevation or 0
        self.name = name or 'Unknown Location'
        self.hebrew = name or 'לא ידוע'  # defaults to the English name
        self.candles = 18  # Defaults to 18 minutes before Shkiah

    def __repr__(self):
        return 'latitude={}, longitude={}, utcoffset={}, elevation={}, name={}, israel={}'.format(
            self.latitude, self.longitude, self.utcoffset, self.elevation, self.name, self.israel)

    @classmethod
    def get_location(cls, search_pattern, case_sensitive=False):
        if not cls._locations_list_raw:
            file = open('./Files/LocationsList.json', 'r', encoding='utf-8')
            cls._locations_list_raw = json.load(file)

        if cls._locations_list_raw:
            r = re.compile(search_pattern, re.IGNORECASE if not case_sensitive else re.UNICODE)
            return [Location.parse(m) for m in cls._locations_list_raw['locations']
                    if r.match(m['n']) or (('h' in m) and r.match(m['h']))]

    @staticmethod
    def get_jerusalem():
        j = Location(latitude=31.78,
                     longitude=-35.22,
                     utcoffset=2,
                     elevation=775,
                     name="Jerusalem",
                     israel=True, )
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
            latitude=float(d['lt']),
            longitude=float(d['ln']),
            utcoffset=int(d['tz']),
            elevation=('el' in d and int(d['el'])) or 0,
            name=d['n'],
            israel='i' in d and d['i'] == 'y')
        if 'h' in d:
            l.hebrew = d['h']
        if 'cl' in d:
            l.candles = int(d['cl'])
        return l
