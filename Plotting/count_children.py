import json

from util.file_finder import get_full_path
from util.json_extracter import extract_unique

full_path = get_full_path('detailed.json')
f = open(full_path)
data = json.load(f)

animal_uuids = extract_unique('uuid', data)

animal_map = {}
counted = {}
for uuid in animal_uuids:
    animal_map[uuid] = 0
    counted[uuid] = False

for row in data:
    uuid = row['uuid']
    if not counted[uuid]:
        counted['uuid'] = True

        if row['momUuid'] != '':
            animal_map[uuid] = animal_map[uuid] + 1

srtd = {key: value for key, value in sorted(animal_map.items(), key=lambda item: item[1])}

f = open("children.txt", "a")

for ani in srtd:
    f.write(ani + '       ' + str(animal_map[ani]) + '\n')

f.close()
