class Location:
    def __init__(self, name, israel, latitude, longitude, utcOffset, elevation):
        self.name = name or 'Unknown Location'
        self.israel = israel or (latitude > 29.45 and latitude < 33 and longitude < -34.23 and longitude > -35.9)
        self.latitude = latitude
        self.longitude = longitude
        self.utcOffset  = 2 if israel else (utcOffset or -int(longitude / 15))
        self.elevation = elevation or 0

    @staticmethod
    def getJerusalem():
        return Location("Jerusalem", True, 31.78, -35.22, 2, 775)
