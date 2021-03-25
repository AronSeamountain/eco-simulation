import json

import numpy as np
import plotly.graph_objects as go

from util.file_finder import get_full_path
from util.json_extracter import extract_unique


def create_scatter(data, species, color):
    days = extract_unique('day', data)
    sizes = []
    speeds = []

    for day in days:
        day_species_data = [i for i in data if i['species'] == species and i['day'] == day]

        if len(day_species_data) == 0:
            continue

        size = np.mean([i['fullyGrownSize'] for i in day_species_data])
        sizes.append(size)

        speed = np.mean([i['speed'] for i in day_species_data])
        speeds.append(speed)

    return go.Scatter3d(
        x=days,
        y=sizes,
        z=speeds,
        marker=dict(
            color=color,
        ),
        name=species
    )


def plot():
    full_path = get_full_path('detailed.json')
    f = open(full_path)
    data = json.load(f)

    fig = go.Figure()

    fig.add_trace(
        create_scatter(data, 'Rabbit', 'royalblue')
    )

    fig.add_trace(
        create_scatter(data, 'Wolf', 'red')
    )

    fig.update_layout(
        title_text='Average Values by Species',
        scene=dict(
            xaxis_title='day',
            yaxis_title='size',
            zaxis_title='speed'
        )
    )

    fig.show()


if __name__ == "__main__":
    plot()
