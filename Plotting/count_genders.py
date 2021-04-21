import json

from util.file_finder import get_full_path
from util.json_extracter import extract_unique

full_path = get_full_path('detailed.json')
f = open(full_path)
data = json.load(f)

animal_uuids = extract_unique('uuid', data)

counted = {}
for uuid in animal_uuids:
    counted[uuid] = False

males = 0
females = 0

for row in data:
    uuid = row['uuid']

    if not counted[uuid]:
        counted['uuid'] = True

        if row['gender'] == 'Male':
            males = males + 1
        else:
            females = females + 1

total = males + females

print('Males: ' + str(males) + '. Males %: ' + str(males / total))
print('Females: ' + str(females) + '. Females %: ' + str(females / total))
