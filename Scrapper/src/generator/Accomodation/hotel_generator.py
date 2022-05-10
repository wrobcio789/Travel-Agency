import random

import numpy as np

import utils
from generator.Accomodation.hotel import Hotel
from generator.Accomodation.room import Room


def amenities_subset(all_amenities, discard_count=0):
    amenities_count_max = len(all_amenities)
    amenities_count_min = amenities_count_max - discard_count
    if amenities_count_min < 0:
        amenities_count_min = 0
    selected_amenities_count = random.randint(amenities_count_min, amenities_count_max)
    return list(np.random.choice(all_amenities, selected_amenities_count, replace=False))


def generate_hotels(arrivals: dict, names: dict, output_dir_path: str, compact=True, pretty=False):
    hotel_names = names['hotel']
    all_amenities = [
        "Toiletries",
        "Personal care",
        "Coffee Kit",
        "Bathrobes and slippers",
        "Free breakfast",
        "Free WiFi",
        "Free parking",
        "Snack baskets",
        "Free bike",
        "VIP",
    ]

    hotel_counts_prob = {
        1: 0.7,
        2: 0.25,
        3: 0.04,
        4: 0.01
    }

    hotels = []
    hotels_ctr = 0
    for country, cities in arrivals.items():
        for city, regions in cities.items():
            for region in regions:
                hotel_count = np.random.choice(list(hotel_counts_prob.keys()), p=list(hotel_counts_prob.values()))
                for _ in range(hotel_count):
                    name = hotel_names[hotels_ctr % len(hotel_names)]
                    hotels.append(Hotel(name=name, country=country, city=city, region=region))
                    hotels_ctr += 1

    for hotel in hotels:
        amenities = amenities_subset(all_amenities, discard_count=3)
        hotel.amenities = amenities

    for hotel in hotels:
        rooms = []
        for capacity in range(1, 7):
            room_count = random.randint(5, 20)
            newborns_friendly = bool(random.getrandbits(1))
            amenities = amenities_subset(hotel.amenities)
            rooms.append(Room(capacity, room_count, newborns_friendly, amenities))
        hotel.rooms = rooms
    if compact:
        utils.save_json(hotels, f"{output_dir_path}/hotels.json", pretty=False)
    if pretty:
        utils.save_json(hotels, f"{output_dir_path}/hotels_pretty.json", pretty=True)


def main():
    arrivals = utils.load_json("../../data/arrivals.json")
    names = utils.load_json("../../consts/names.json")
    generate_hotels(arrivals, names, "../../data")


if __name__ == '__main__':
    main()
