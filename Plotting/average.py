import json

import numpy as np
import plotly.graph_objects as go

from util.file_finder import get_full_path
from util.json_extracter import extract_unique


def create_scatter(data, species, color):
    days_to_average = 10
    day_counter = 0

    all_days = extract_unique('day', data)
    days = []
    sizes = []
    speeds = []

    day_sequence_size = []
    day_sequence_speed = []

    for day in all_days:
        day_counter = day_counter + 1

        day_species_data = [i for i in data if i['species'] == species and i['day'] == day]
        day_sequence_size = day_sequence_size + [i['fullyGrownSize'] for i in day_species_data]
        day_sequence_speed = day_sequence_speed + [i['speed'] for i in day_species_data]

        if day_counter == days_to_average:
            day_counter = 0

            if len(day_sequence_size) != 0:
                days.append(day)
                sizes.append(np.mean(day_sequence_size))
                speeds.append(np.mean(day_sequence_speed))

            day_sequence_size = []
            day_sequence_speed = []

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
