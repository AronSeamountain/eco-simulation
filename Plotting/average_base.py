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


def create_scatter(data, species, days_to_average, legend_name, color, highlighted=True):
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
        opacity=1 if highlighted else 0.5,
        name=legend_name,
        marker=dict(
            color=color
        ),
        line=dict(color=color, width=3),
        showlegend=highlighted
    )


@app.callback(Output('live-update-graph', 'figure'),
              Input('slider', 'value'))
def update_graph_live(days_to_average):
    full_paths = get_full_paths('detailed.json')
    fig = go.Figure()
    n = len(full_paths)

    max_day = 0
    max_day_path = ':^)'

    for full_path in full_paths:
        data = json.load(open(full_path))
        day = data[-1]['day']

        if day > max_day:
            max_day = day
            max_day_path = full_path

    i = 1
    for full_path in full_paths:
        if full_path == max_day_path:
            print('Skipping max day file')
            i = i + 1
            continue

        print('Plotting ' + str(i) + '/' + str(n) + ' (' + full_path + ')')
        tic = time.perf_counter()

        f = open(full_path)
        data = json.load(f)

        inject_plots(fig, data, days_to_average, False)

        print('Finished #' + str(i) + ', took: ' + str(time.perf_counter() - tic) + ' seconds')

        i = i + 1

    print('Started plotting max day')
    inject_plots(fig, json.load(open(max_day_path)), days_to_average, True)
    print('Finished plotting max day')

    fig.update_layout(
        title_text=title,
        xaxis_title='days',
        yaxis_title=y_column,
    )

    return fig


def inject_plots(fig, data, days_to_average, highlighted):
    fig.add_trace(
        create_scatter(
            data=data,
            species='Rabbit',
            days_to_average=days_to_average,
            legend_name='Rabbits',
            color='blue',
            highlighted=highlighted
        )
    )

    fig.add_trace(
        create_scatter(
            data=data,
            species='Wolf',
            days_to_average=days_to_average,
            legend_name='Wolves',
            color='red',
            highlighted=highlighted
        )
    )


def run():
    app.run_server(debug=True, use_reloader=True)


def plot_average(t, y):
    global y_column
    global title
    title = t
    y_column = y
    run()
