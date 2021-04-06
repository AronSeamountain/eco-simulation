import pandas as pd
import plotly.graph_objects as go

from util.file_finder import get_full_path

def plot():
    full_path = get_full_path('overview.csv')
    df = pd.read_csv(full_path)
    fig = go.figure()

    rabbitVisionAverages  = []
    rabbitHearingAverages = []
    wolfVisionAverages  = []
    wolfHearingAverages = []

    def calcAverage(){
        day_counter
    }

    fig.add_trace(
        go.scatter(
            x = df['day'], 
            y = df['hearingPercentage'], 
            name = 'hearing percentage', 
            line = dict(color = 'firebrick', width = 4)
        )
    )

    
    fig.add_trace(
        go.scatter(
            x = df['day'], 
            y = df['visionPercentage'], 
            name = 'vision percentage', 
            line = dict(color = 'royalblue', width = 4)
        )
    )

    fig.update_layout(
        title = 'Distribution of vision and hearing power',
        xaxis_title='day',
        yaxis_title='percentage'
    )

    fig.show()

if __name__ == '__main__':
    plot()