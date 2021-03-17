import glob
import json

import plotly.express as px
import plotly.graph_objects as go


def extract(key, data):
    return [i[key] for i in data]


file_name = 'animal_log.json'
text_files = glob.glob("./**/" + file_name, recursive=True)

if not text_files:
    raise Exception('Found no ' + file_name + ' file.')

full_path = text_files[0]

f = open(full_path)
data = json.load(f)

day = extract('day', data)
size = extract('size', data)
speed = extract('speed', data)
species = extract('species', data)

df = px.data.iris()

fig = px.scatter(
    df,
    x=day,
    y=speed,
    size=size,
    color=species,
    labels={'x':'day', 'y':'speed'}
)

fig.show()
