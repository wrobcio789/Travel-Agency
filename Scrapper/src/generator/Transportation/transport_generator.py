import random

import utils
from generator.Transportation.transportation import Transportation


def generate_transportation(arrivals: dict, departures: list, names: dict, output_dir_path: str, compact=True,
                            pretty=False):
    transportation_people_ranges = {
        "plane": {"from": 100, "to": 200, "step": 10},
        "ship": {"from": 100, "to": 300, "step": 10},
        "train": {"from": 100, "to": 500, "step": 10}
    }
    transportation_types_prob = {
        "plane": 1,
        "ship": 0.2,
        "train": 0.1
    }
    transportation_names_ctr = {
        "plane": 0,
        "ship": 0,
        "train": 0
    }

    transports = []
    for departure in departures:
        for country, cities in arrivals.items():
            for city, regions in cities.items():
                chance = random.random()
                for transport, prob in transportation_types_prob.items():
                    transport_names = names[transport]
                    if chance <= prob:
                        t = transportation_people_ranges[transport]
                        capacity = random.randint(t["from"], t["to"] + 1)
                        for _arrival, _departure in [(city, departure), (departure, city)]:
                            name = transport_names[transportation_names_ctr[transport] % len(transport_names)]
                            transports.append(Transportation(name, transport, capacity, _arrival, _departure))
                            transportation_names_ctr[transport] += 1
    transports.append({
        "type": "own",
        "capacity": 0,
        "arrival": "",
        "departure": ""
    })
    if compact:
        utils.save_json(transports, f"{output_dir_path}/transports.json", pretty=False)
    if pretty:
        utils.save_json(transports, f"{output_dir_path}/transports_pretty.json", pretty=True)


def main():
    arrivals = utils.load_json("../../data/arrivals.json")
    departures = utils.load_json("../../consts/departures.json")
    generate_transportation(arrivals, departures, "../../data")


if __name__ == '__main__':
    main()
