import pandas as pd
import plotly.graph_objects as go
from plotly.subplots import make_subplots

from util.file_finder import get_full_path


def plot():
    fps_full_path = get_full_path('fps.csv')
    df_fps = pd.read_csv(fps_full_path)

    has_baseline = False
    try:
        fps_baseline_full_path = get_full_path('fps_baseline.csv')
        has_baseline = True
    except:
        print('Did not find fps_baseline.csv, create it manually if desired')

    if has_baseline:
        df_fps_baseline = pd.read_csv(fps_baseline_full_path)

    overview_full_path = get_full_path('overview.csv')
    df_overview = pd.read_csv(overview_full_path)

    overview_baseline_full_path = get_full_path('overview_baseline.csv')
    df_overview_baseline = pd.read_csv(overview_baseline_full_path)

    fig = make_subplots(specs=[[{"secondary_y": True}]])

    fig.add_trace(
        go.Scatter(
            x=df_fps['day'],
            y=df_fps['average'],
            name='average fps',
            line=dict(color='royalblue', width=4)
        ),
        secondary_y=False
    )

    fig.add_trace(
        go.Scatter(
            x=df_overview['day'],
            y=df_overview['amount'],
            name='# animals',
            line=dict(color='navy', width=4),
            opacity=0.5
        ),
        secondary_y=True
    )

    if has_baseline:
        fig.add_trace(
            go.Scatter(
                x=df_fps_baseline['day'],
                y=df_fps_baseline['average'],
                name='average fps baseline',
                line=dict(color='firebrick', width=4)
            ),
            secondary_y=False
        )

        fig.add_trace(
            go.Scatter(
                x=df_overview_baseline['day'],
                y=df_overview_baseline['amount'],
                name='# animals baseline',
                line=dict(color='lightcoral', width=4),
                opacity=0.5
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
