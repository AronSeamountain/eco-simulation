import pandas as pd
import plotly.graph_objects as go

from util.file_finder import get_full_path


def plot():
    full_path = get_full_path('fps.csv')
    df = pd.read_csv(full_path)
    fig = go.Figure()

    fig.add_trace(
        go.Scatter(
            x=df['day'], y=df['average'], name='average fps', line=dict(color='rebeccapurple', width=4)
        )
    )

    fig.update_layout(
        title='Average FPS',
        xaxis_title='day',
        yaxis_title='average fps'
    )

    fig.show()


if __name__ == "__main__":
    plot()
