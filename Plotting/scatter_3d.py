import json

import plotly.express as px

from util.file_finder import get_full_path
from util.json_extracter import extract

full_path = get_full_path('detailed.json')
f = open(full_path)
data = json.load(f)

day = extract('day', data)
size = extract('size', data)
speed = extract('speed', data)
species = extract('species', data)

df = px.data.iris()

fig = px.scatter_3d(
    df,
    x=day,
    y=speed,
    z=size,
    color=species,
    size=size,
    labels={'x': 'day', 'y': 'speed', 'z': 'size', 'color': 'species'},
    title='Individual Statistics'
)

fig.show()
