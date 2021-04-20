import json

from util.file_finder import get_full_path
from util.json_extracter import extract_unique

full_path = get_full_path('detailed.json')
f = open(full_path)
data = json.load(f)

all_animals = extract_unique('uuid', data)

print(all_animals)
