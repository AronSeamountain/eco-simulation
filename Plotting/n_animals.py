import sys
import json

import pandas as pd
import plotly.graph_objects as go

from util.file_finder import get_full_paths, get_full_path


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

def get_death_cause_data(path):
    f = open(path)
    data = json.load(f)

    rabbits = [i for i in data if i['species'] == "Rabbit" and i['cause'] == "eaten"]

    maxDay = data[len(data) - 1]['day']
    count = [0] * maxDay

    for day in range(1, maxDay + 1):
        count[day - 1] = len([i for i in rabbits if i['day'] == day])

    return count

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

    death_cause_path = get_full_path('deathCause.json')
    death_array = get_death_cause_data(death_cause_path)

    fig.add_trace(
        go.Scatter(
            x=df['day'], y=death_array, name='# eaten rabbits', line=dict(color='black', width=4)
        )
    )

    fig.update_layout(
        title='Amount of Animals (' + full_path + ')' + ' (' + death_cause_path + ')',
        xaxis_title='day',
        yaxis_title='amount'
    )

    print('R correlation wolves and eaten rabbits: ' + str(pd.Series(death_array).corr(pd.Series(df['amount_wolfs']))))
    print('R correlation wolves and rabbits: ' + str(pd.Series(df['amount_rabbits']).corr(pd.Series(df['amount_wolfs']))))
    print('R correlation rabbits and eaten rabbits: ' + str(pd.Series(death_array).corr(pd.Series(df['amount_rabbits']))))

    fig.show()


if __name__ == "__main__":
    plot()
