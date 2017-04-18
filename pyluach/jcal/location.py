import json
import re


class Location:
    """
    Represents a single Location for zmanim.


    The file pyluach/files/LocationsList.json.gz contains a list of 1,291 locations worldwide.
    If the list is accessed, they are all loaded into a dictionary
    and stored in cls._locations_list_raw.


    Use the get_location function to search the list.

    """
    _locations_list_raw = {}

    def __init__(self, latitude, longitude, utcoffset=0, elevation=0, name=None, israel=False):
        self.latitude = latitude
        self.longitude = longitude
        self.israel = israel if isinstance(israel, bool) else \
            ((29.45 < latitude < 33) and (-34.23 > longitude > -35.9))
        self.utcoffset = utcoffset if isinstance(utcoffset, int) else \
            (2 if israel else -(longitude // 15))
        self.elevation = elevation or 0
        self._name = name
        self._hebrew = name  # defaults to the English name
        self.candles = 18  # Defaults to 18 minutes before Shkiah

    def __repr__(self):
        return 'latitude={}, longitude={}, utcoffset={}, elevation={}, name={}, israel={}'.format(
            self.latitude, self.longitude, self.utcoffset, self.elevation, self._name or '<NOT SET>', self.israel)

    def __str__(self):
        return 'Name: {1:<30}Latitude: {2:<8}Longitude: {3:<8}Elevation (Meters): {4:<5}Hebrew Name: {5:<20}'.format(
            ' ', self._name or '<NOT SET>', self.latitude, self.longitude, self.elevation,
            self._hebrew if self._hebrew != self._name else '<NOT SET>', self.utcoffset)

    @property
    def name(self):
        return self._name or 'Latitude: {} Longitude: {} Elevation: {} ft.'.format(
            self.latitude, self.longitude, int(self.elevation * 3.28084))

    @name.setter
    def name(self, value):
        self._name = value

    @property
    def hebrew(self):
        return self._hebrew or 'קו אורך: {} קו רוחב: {} גובה: {} מטר'.format(
            self.latitude, self.longitude, self.elevation)

    @hebrew.setter
    def hebrew(self, value):
        self._hebrew = value

    @classmethod
    def get_location(cls, search_pattern, case_sensitive=False):
        if not cls._locations_list_raw:
            import gzip
            with gzip.open('./files/LocationsList.json.gz', 'rt', encoding="utf-8") as file:
                cls._locations_list_raw = json.load(file)

        if cls._locations_list_raw:
            r = re.compile(search_pattern, re.IGNORECASE if not case_sensitive else re.UNICODE)
            return [Location.parse(m) for m in cls._locations_list_raw['locations']
                    if r.match(m['n']) or (('h' in m) and r.match(m['h']))]
   
                    
    @staticmethod
    def get_jerusalem():
        """
        Get the Location object for Jerusalem.
        :return: the Location object for Jerusalem
        :rtype: Location
        """
        j = Location(latitude=31.78,
                     longitude=-35.22,
                     utcoffset=2,
                     elevation=775,
                     name="Jerusalem",
                     israel=True, )
        j.hebrew = 'ירושלים'
        j.candles = 40
        return j

    @staticmethod
    def parse(d):
        """
        Parses a location that was loaded to a dict from the json file of Locations.
        Sample entry:
            {'tz': '2', 'cl': '40', 'lt': '31.78', 'n': 'Jerusalem', 'el': '830', 'ln': '-35.22', 'h': 'ירושלים', 'i': 'y'}
        The 'i', 'el', 'cl' and 'h' keys are optional.'''

        :param d: Dictionary of a single location in the "raw" location format.
        :type d: dict
        :return: The Location object instance.
        :rtype: Location
        """
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
