import json

import plotly.express as px

from util.file_finder import get_full_path
from util.json_extracter import extract


def plot():
    full_path = get_full_path('detailed.json')
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
        labels={'x': 'day', 'y': 'speed'},
        title='Individual Statistics'
    )

    fig.show()


if __name__ == "__main__":
    plot()