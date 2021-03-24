import json

import plotly.graph_objects as go

from util.file_finder import get_full_path
from util.json_extracter import filter_where, extract_unique


def create_scatter(data, species, color):
    days = extract_unique('day', data)
    sizes = []
    speeds = []

    for day in days:
        relevant_data = filter_where(
            filter_where(data, 'species', species),
            'day',
            day
        )

        if len(relevant_data) == 0:
            continue

        size = sum(i['size'] for i in relevant_data) / len(relevant_data)
        sizes.append(size)

        speed = sum(i['speed'] for i in relevant_data) / len(relevant_data)
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
