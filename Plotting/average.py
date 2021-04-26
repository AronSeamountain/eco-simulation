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


def create_scatter(data, species, days_to_average, y_column, legend_name, color):
    day_counter = 0

    all_days = extract_unique('day', data)
    days = []
    column_values = []

    column_sequence_size = []

    for day in all_days:
        day_counter = day_counter + 1

        day_species_data = [i for i in data if i['species'] == species and i['day'] == day]
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
        name='something',
        marker=dict(
            color=color
        )
    )


@app.callback(Output('live-update-graph', 'figure'),
              Input('slider', 'value'))
def update_graph_live(days_to_average):
    y_column = 'fullyGrownSize'

    full_path = get_full_path('detailed.json')
    f = open(full_path)
    data = json.load(f)

    fig = go.Figure()

    fig.add_trace(
        create_scatter(
            data=data,
            species='Rabbit',
            days_to_average=days_to_average,
            y_column='fullyGrownSize',
            legend_name='first thing',
            color='blue'
        )
    )

    fig.update_layout(
        title_text='Average Values by Species',
        xaxis_title="days",
        yaxis_title=y_column,
    )

    return fig


def run():
    app.run_server(debug=True, use_reloader=True)


if __name__ == "__main__":
    run()
