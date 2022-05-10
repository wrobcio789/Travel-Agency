import random as rand


class Hotel:
    def __init__(self, name=None, country=None, city=None, region=None, rooms=None, rating=None, amenities=None):
        self.name = name
        self.country = country
        self.city = city
        self.region = region
        if rating is None:
            rating = rand.randint(1, 5)
        self.rating = rating
        if amenities is None:
            amenities = []
        self.amenities = amenities
        if rooms is None:
            rooms = []
        self.rooms = rooms
