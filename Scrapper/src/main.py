import utils
from scrapper import fetcher, processor
from generator.Transportation.transport_generator import generate_transportation
from generator.Accomodation.hotel_generator import generate_hotels


def main():
    generate_pretty_json = True
    generate_compact_json = True
    output_dir_path = "data"
    html_file_path = "data/page.html"
    # fetcher.scrap_tours()
    # processor.process_tours(html_file_path, output_dir_path, generate_compact_json, generate_pretty_json)

    arrivals = utils.load_json("data/arrivals.json")
    departures = utils.load_json("consts/departures.json")
    names = utils.load_json("consts/names.json")
    generate_transportation(arrivals, departures, names, output_dir_path, generate_compact_json, generate_pretty_json)
    generate_hotels(arrivals, names, output_dir_path, generate_compact_json, generate_pretty_json)


if __name__ == '__main__':
    main()
