import json

import dash
import dash_core_components as dcc
import dash_html_components as html
import numpy as np
import plotly.graph_objects as go
from dash.dependencies import Input, Output

from util.file_finder import get_full_path
from util.json_extracter import extract_unique

app = dash.Dash()

app.layout = html.Div(
    html.Div([
        dcc.Graph(id='live-update-graph', style={"height": "80vh"}),
        html.P('Days to Average'),
        dcc.Slider(
            id='slider',
            min=1,
            max=20,
            step=1,
            value=10,
            dots=True,
            tooltip={'always_visible': True}
        ),
    ])
)


def create_scatter(data, species, days_to_average, color):
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
        day_sequence_speed = day_sequence_speed + [i['fullyGrownSpeed'] for i in day_species_data]

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
            size=7
        ),
        name=species
    )


@app.callback(Output('live-update-graph', 'figure'),
              Input('slider', 'value'))
def update_graph_live(days_to_average):
    full_path = get_full_path('detailed.json')
    f = open(full_path)
    data = json.load(f)

    fig = go.Figure()

    fig.add_trace(
        create_scatter(data, 'Rabbit', days_to_average, 'royalblue')
    )

    fig.add_trace(
        create_scatter(data, 'Wolf', days_to_average, 'red')
    )

    fig.update_layout(
        title_text='Average Values by Species (' + full_path + ')',
        scene=dict(
            xaxis_title='day',
            yaxis_title='fully grown size',
            zaxis_title='fully grown speed'
        )
    )

    return fig


def run():
    app.run_server(debug=True, use_reloader=True)


if __name__ == "__main__":
    run()
