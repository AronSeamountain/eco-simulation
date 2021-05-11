import json
import sys

import pandas as pd
import plotly.graph_objects as go

from util.file_finder import get_full_paths


def plot():
    full_paths = get_full_paths('overview.csv')
    plot_all = False
    avg = False
    all_longest = False

    if len(sys.argv) >= 2 and str(sys.argv[1]) == 'all':
        plot_all = True

    if len(sys.argv) >= 2 and str(sys.argv[1]) == 'avg':
        avg = True

    if len(sys.argv) >= 2 and str(sys.argv[1]) == 'all-longest':
        all_longest = True

    if plot_all:
        print('Plotting all overview.csv files')

        for full_path in full_paths:
            plot_this(full_path)

    elif all_longest:
        print('Plot All Highlight Longest')
        plot_all_longest(full_paths)

    elif avg:
        print('Averaging')
        plot_avg(full_paths)

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
    fig = go.Figure()

    inject_traces(fig, full_path)

    fig.show()


def inject_traces(fig, full_path, highlighted=True):
    df = pd.read_csv(full_path)

    print('Plotting: ' + full_path)

    plant_color = 'forestgreen' if highlighted else 'palegreen'
    wolf_color = 'red' if highlighted else 'lightpink'
    rabbit_color = 'royalblue' if highlighted else 'lightblue'

    width = 4 if highlighted else 4
    opacity = 1 if highlighted else 0.5

    fig.add_trace(
        go.Scatter(
            x=df['day'], y=df['amount_wolfs'], name='# wolfs', line=dict(color=wolf_color, width=width),
            showlegend=highlighted,
            opacity=opacity
        )
    )

    fig.add_trace(
        go.Scatter(
            x=df['day'], y=df['amount_rabbits'], name='# rabbits', line=dict(color=rabbit_color, width=width),
            showlegend=highlighted,
            opacity=opacity
        )
    )

    fig.add_trace(
        go.Scatter(
            x=df['day'], y=df['plants'], name='# mature plants', line=dict(color=plant_color, width=width),
            showlegend=highlighted,
            opacity=opacity
        )
    )

    fig.update_layout(
        title='Amount of Animals and Plants (' + full_path + ')',
        xaxis_title='day',
        yaxis_title='amount'
    )

    if highlighted:
        print('R correlation wolves and rabbits: ' + str(
            pd.Series(df['amount_rabbits']).corr(pd.Series(df['amount_wolfs']))))
        print('R correlation plants and rabbits: ' + str(pd.Series(df['amount_rabbits']).corr(pd.Series(df['plants']))))
        print('R correlation plants and wolves: ' + str(pd.Series(df['amount_wolfs']).corr(pd.Series(df['plants']))))


def plot_all_longest(full_paths):
    fig = go.Figure()

    max_day = 0
    max_day_path = ': - )'

    for full_path in full_paths:
        df = pd.read_csv(full_path)
        day = df['day'].tolist()[-1]

        if day > max_day:
            max_day = day
            max_day_path = full_path

    for full_path in full_paths:
        if full_path == max_day_path: continue
        inject_traces(fig, full_path, highlighted=False)

    inject_traces(fig, max_day_path, highlighted=True)

    fig.show()


def plot_avg(full_paths):
    fig = go.Figure()

    day_rabbits = {}
    day_wolves = {}
    day_plants = {}
    day_amount_reached = {}

    for full_path in full_paths:
        inject_traces(fig, full_path)
        df = pd.read_csv(full_path)

        for day in df['day']:
            if day not in day_rabbits:
                day_rabbits[day] = 0
            if day not in day_wolves:
                day_wolves[day] = 0
            if day not in day_amount_reached:
                day_amount_reached[day] = 0
            if day not in day_plants:
                day_plants[day] = 0

            day_amount_reached[day] = day_amount_reached[day] + 1
            day_rabbits[day] = day_rabbits[day] + df['amount_rabbits'][day - 1]
            day_wolves[day] = day_wolves[day] + df['amount_wolfs'][day - 1]
            day_plants[day] = day_plants[day] + df['plants'][day - 1]

    days = []
    avg_plants = []
    avg_wolves = []
    avg_rabbits = []

    for day in day_amount_reached:
        n_reached = day_amount_reached[day]
        days.append(day)

        avg_rabbits.append(day_rabbits[day] / n_reached)
        avg_plants.append(day_plants[day] / n_reached)
        avg_wolves.append(day_wolves[day] / n_reached)

    # rabbits
    fig.add_trace(
        go.Scatter(
            x=days, y=avg_rabbits, name='temp', line=dict(color='royalblue', width=4)
        )
    )

    # wolves
    fig.add_trace(
        go.Scatter(
            x=days, y=avg_wolves, name='temp', line=dict(color='red', width=4)
        )
    )

    # plants
    fig.add_trace(
        go.Scatter(
            x=days, y=avg_plants, name='temp', line=dict(color='limegreen', width=4)
        )
    )

    fig.show()


if __name__ == "__main__":
    plot()
