import pandas as pd
import plotly.graph_objects as go
from plotly.subplots import make_subplots

from util.file_finder import get_full_path


def plot():
    overview_full_path = get_full_path('overview.csv')
    df_overview = pd.read_csv(overview_full_path)

    fps_full_path = get_full_path('fps.csv')
    df_fps = pd.read_csv(fps_full_path)
    fig = make_subplots(specs=[[{"secondary_y": True}]])

    fig.add_trace(
        go.Scatter(
            x=df_fps['day'], y=df_fps['average'], name='average fps', line=dict(color='royalblue', width=4)
        ),
        secondary_y=False
    )

    fig.add_trace(
        go.Scatter(
            x=df_overview['day'], y=df_overview['amount'], name='# animals', line=dict(color='darkgrey', width=4)
        ),
        secondary_y=True
    )

    fig.update_layout(
        title='Average FPS',
        xaxis_title='day',
    )

    fig.update_yaxes(title_text='average fps', secondary_y=False)
    fig.update_yaxes(title_text='# animals', secondary_y=True)

    fig.show()


if __name__ == "__main__":
    plot()
