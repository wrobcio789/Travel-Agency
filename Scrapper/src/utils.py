import json
from datetime import timedelta
from io import StringIO

from lxml import etree


def dates_between(start_date, end_date):
    return [start_date + timedelta(days=x) for x in range((end_date - start_date).days)]


def _dump(data, pretty, default):
    return json.dumps(data, indent=4, default=default, sort_keys=False, ensure_ascii=False) \
        if pretty \
        else json.dumps(data, default=default, ensure_ascii=False)


def save_json(data, filepath, pretty=False):
    default = None
    try:
        result = _dump(data, pretty, default)
    except TypeError:
        def default(o):
            return o.__dict__

        result = _dump(data, pretty, default)
    with open(filepath, "w", encoding="utf-8") as f:
        f.write(result)


def load_json(filepath):
    with open(filepath, "r", encoding="utf-8") as f:
        return json.load(f)


def parse_html_file(filename):
    f = open(filename, encoding="utf8").read()
    parser = etree.HTMLParser()
    tree = etree.parse(StringIO(f), parser)
    return tree
