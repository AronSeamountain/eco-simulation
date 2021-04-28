import sys

import pandas as pd
import plotly.graph_objects as go

from util.file_finder import get_full_paths


def plot():
    full_paths = get_full_paths('overview.csv')
    plot_all = False

    if len(sys.argv) >= 2 and str(sys.argv[1]) == 'all':
        plot_all = True

    if plot_all:
        print('Plotting all overview.csv files')

        for full_path in full_paths:
            plot_this(full_path)
    else:
        print('Plotting first overview.csv found')
        plot_this(full_paths[0])


def plot_this(full_path):
    df = pd.read_csv(full_path)
    fig = go.Figure()

    fig.add_trace(
        go.Scatter(
            x=df['day'], y=df['amount_wolfs'], name='# wolfs', line=dict(color='firebrick', width=4)
        )
    )

    fig.add_trace(
        go.Scatter(
            x=df['day'], y=df['amount_rabbits'], name='# rabbits', line=dict(color='royalblue', width=4)
        )
    )

    fig.update_layout(
        title='Amount of Animals (' + full_path + ')',
        xaxis_title='day',
        yaxis_title='amount'
    )

    fig.show()


if __name__ == "__main__":
    plot()
