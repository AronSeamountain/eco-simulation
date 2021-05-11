import json
import time

import dash
import dash_core_components as dcc
import dash_html_components as html
import numpy as np
import plotly.graph_objects as go
from dash.dependencies import Input, Output

from util.file_finder import get_full_paths
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

y_column = ''
title = 'Title'
start = -1


def create_scatter(data, species, days_to_average, legend_name, color):
    day_counter = 0

    all_days = extract_unique('day', data)
    days = []
    column_values = []

    column_sequence_size = []

    prefiltered_data = [i for i in data if i['species'] == species]

    i = 0
    for day in all_days:
        day_counter = day_counter + 1

        day_species_data = []

        while i < len(prefiltered_data):
            if prefiltered_data[i]['day'] == day:
                day_species_data.append(prefiltered_data[i])
                i = i + 1
                continue

            break

        column_sequence_size = column_sequence_size + [i[y_column] for i in day_species_data]

        if day_counter == days_to_average:
            day_counter = 0

            if len(column_sequence_size) != 0:
                days.append(day)
                column_values.append(np.mean(column_sequence_size))

            column_sequence_size = []

    return go.Scatter(
        x=days,
        y=column_values,
        name=legend_name,
        marker=dict(
            color=color
        )
    )


@app.callback(Output('live-update-graph', 'figure'),
              Input('slider', 'value'))
def update_graph_live(days_to_average):
    full_paths = get_full_paths('detailed.json')
    fig = go.Figure()
    n = len(full_paths)

    i = 1
    for full_path in full_paths:
        print('Plotting ' + str(i) + ' (' + full_path + ')')
        tic = time.perf_counter()

        f = open(full_path)
        data = json.load(f)

        fig.add_trace(
            create_scatter(
                data=data,
                species='Rabbit',
                days_to_average=days_to_average,
                legend_name='Rabbits',
                color='blue'
            )
        )

        fig.add_trace(
            create_scatter(
                data=data,
                species='Wolf',
                days_to_average=days_to_average,
                legend_name='Wolves',
                color='red'
            )
        )

        fig.update_layout(
            title_text=title + ' (' + full_path + ')',
            xaxis_title='days',
            yaxis_title=y_column,
        )

        print('Finished #' + str(i) + ', took: ' + str(time.perf_counter() - tic) + ' seconds')

        i = i + 1

    return fig


def run():
    app.run_server(debug=True, use_reloader=True)


def plot_average(t, y):
    global y_column
    global title
    title = t
    y_column = y
    run()
