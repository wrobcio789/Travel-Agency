import itertools
import re
from operator import itemgetter

from scrapper.offer import Offer
from utils import save_json, parse_html_file


def process_tours(source_filepath, output_dir_path, compact=True, pretty=False):
    offer_xpath = "//div[contains(@class,'offer-tile--listingOffer')]"
    offer_title_xpath = ".//span[@class='offer-tile-body__hotel-name']"
    offer_hotel_rating_xpath = ".//div[contains(@class,'offer-tile__hotel-rating')]"
    location_xpath = ".//nav[contains(@data-testid,'offer-tile-breadcrumbs')]//span[@property='name']"
    info_xpath = ".//span[contains(@data-testid,'offer-tile-departure-date')]"
    info_regex = r"(\d{2}\.\d{2}\.\d{4}) - (\d{2}\.\d{2}\.\d{4}) \((\d{1,}) noclegów\)"  # 13.05.2022 - 20.05.2022 (7 noclegów)
    price_xpath = ".//span[contains(@data-testid,'price-amount')]"
    is_price_per_person_xpath = ".//span[contains(@data-testid,'price-value-currency-info')]"

    tree = parse_html_file(source_filepath)

    offers = tree.xpath(offer_xpath)
    i = 1
    offers_output = []
    for offer in offers:
        title_obj = offer.xpath(offer_title_xpath)
        title = title_obj[0].text

        hotel_rating_objs = offer.xpath(offer_hotel_rating_xpath)
        if len(hotel_rating_objs) > 0:
            hotel_rating_obj = hotel_rating_objs[0].attrib['class']
            rating_text = "rating-#"
            index = hotel_rating_obj.find(rating_text)
            rating = hotel_rating_obj[index + len(rating_text)]
        else:
            rating = "-1"

        locations_obj = offer.xpath(location_xpath)
        locations = [location.text for location in locations_obj]
        if len(locations) != 3:
            continue

        info_obj = offer.xpath(info_xpath)
        info = info_obj[0].text
        result = re.search(info_regex, info)
        if len(result.groups()) != 3:
            continue
        start, end, days = result.groups()

        price_obj = offer.xpath(price_xpath)
        price = price_obj[0].text.strip().replace(" ", "")

        price_per_obj = offer.xpath(is_price_per_person_xpath)
        price_per = price_per_obj[0].text

        print(i, title, rating, locations, info, price, price_per)
        i += 1
        offers_output.append(
            Offer(title, locations[0], locations[1], locations[2], int(price), int(rating), start, end, int(days)))
    if compact:
        save_json(offers_output, f"{output_dir_path}/offers_pretty.json", pretty=True)
    if pretty:
        save_json(offers_output, f"{output_dir_path}/offers.json", pretty=False)

    offer_locations = sorted(list(set([(offer.country, offer.city, offer.region) for offer in offers_output])))

    tuple_elem_index = 0
    location_output = dict(
        (k, list(g)) for k, g in itertools.groupby(offer_locations, key=itemgetter(tuple_elem_index)))
    tuple_elem_index += 1
    for k, v in location_output.items():
        location_output[k] = dict((k, [loc[tuple_elem_index + 1] for loc in list(g)]) for k, g in
                                  itertools.groupby(v, key=itemgetter(tuple_elem_index)))

    save_json(location_output, f"{output_dir_path}/arrivals_pretty.json", pretty=True)
    save_json(location_output, f"{output_dir_path}/arrivals.json", pretty=False)


if __name__ == '__main__':
    process_tours("../data/page.html", "../data")
