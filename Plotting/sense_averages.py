import json

import dash
import dash_core_components as dcc
import dash_html_components as html
import numpy as np
import pandas as pd
import plotly.graph_objects as go
from dash.dependencies import Input, Output
from plotly.subplots import make_subplots

from util.file_finder import get_full_path
from util.json_extracter import extract_unique

def plot():
    full_path = get_full_path('overview.csv')
    df = pd.read_csv(full_path)
    fig = go.Figure()
    data = json.load(open(full_path))

    all_days = extract_unique('day', data)

    fig.add_trace(
        go.scatter(//TODO make all of this into a metho dewhen something works
            x = df['day'], 
            y = calcAverage(data, 'Rabbit', 'hearingPercentage'), 
            line = dict(color = 'firebrick', width = 4),
            stackgroup='one',
            groupnorm='percent'
        )
    )

    fig.add_trace(
        go.scatter(
            x = df['day'], 
            y = calcAverage(data, Rabbit, 'visionPercentage'), 
            line = dict(color = 'royalblue', width = 4),
            stackgroup='one',
            groupnorm='percent'
        )
    )

    fig.add_trace(go.Scatter(
    x=df['day'], 
    y=[100 for i in all_days],
    mode='lines',
    line=dict(width=0.5, color='rgb(131, 90, 241)'),
    stackgroup='one'
    ))

    fig.update_layout(
        showlegend=True,
        title = 'Distribution of vision and hearing power',
        xaxis_title='day',
        yaxis_title='percentage',
        xaxis_type='category',
        yaxis=dict(
            type='linear',
            range=[1, 100],
            ticksuffix='%'))
    
    
    fig.show()

def calcAverage(data, species, dataToConsider):

    all_days = extract_unique('day', data)

    averages = []

    for day in all_days:
        values = [i[dataToConsider] for i in data if i['day'] == day and i['species'] == species]

        average = values.sum()/values.len()

        averages.append(average)
    
    return averages



if __name__ == '__main__':
    plot()