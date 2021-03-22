import pandas as pd
import plotly.graph_objects as go

from util.file_finder import get_full_path


def plot():
    full_path = get_full_path('overview.csv')
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
        title='Amount of Animals',
        xaxis_title='Day'
    )

    fig.show()


if __name__ == "__main__":
    plot()
