import dash
import dash_core_components as dcc
import dash_html_components as html
import pandas as pd
import plotly.express as px
from dash.dependencies import Input, Output

from util.file_finder import get_full_path

app = dash.Dash()

app.layout = html.Div(
    html.Div([
        html.H1('Live Updated Average Stats'),
        html.Div(id='live-update-text'),
        dcc.Graph(id='live-update-graph'),
        dcc.Interval(
            id='interval-component',
            interval=5 * 1000,  # in milliseconds
            n_intervals=0
        )
    ])
)


@app.callback(Output('live-update-graph', 'figure'),
              Input('interval-component', 'n_intervals'))
def update_graph_live(n):
    full_path = get_full_path('overview.csv')

    df = pd.read_csv(full_path)
    fig = px.scatter_3d(
        df,
        x='day',
        y='speed',
        z='size',
        color='amount',
        title='Average Values'
    )

    return fig


if __name__ == "__main__":
    app.run_server(debug=True, use_reloader=True)
