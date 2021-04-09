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
    full_path = get_full_path('detailed.json')
    fig = make_subplots(rows = 2, cols = 1, subplot_titles=("Rabbit senses", "Wolf senses"))#go.figure()
    data = json.load(open(full_path))

    all_days = extract_unique('day', data)

    fig.append_trace(
        genScatter(data, 'Rabbit', 'hearingPercentage', 'firebrick', 'Eyesight'),
        row=1, col=1
    )

    fig.append_trace(
        genScatter(data, 'Rabbit', 'visionPercentage', 'royalblue', 'Hearing'),
        row=1, col=1
    )

    #wolf graph
    fig.append_trace(
        genScatter(data, 'Wolf', 'hearingPercentage', 'firebrick', 'Eyesight'),
        row=2, col=1
    )

    fig.append_trace(
        genScatter(data, 'Wolf', 'visionPercentage', 'royalblue', 'Hearing'),
        row=2, col=1
    )

    fig.update_layout(
        showlegend=True,
        title = 'Distribution of eyesight and hearing power',
    )
    
    fig.update_yaxes(
        title='Percentage',  
        type='linear',
        range=[1, 100],
        ticksuffix='%',
        row = 1,
        col = 1
    )
    
    fig.update_xaxes(
        title='Day',
        type = 'category',
        row = 1,
        col = 1
    )

    fig.update_yaxes(
        title='Percentage',  
        type='linear',
        range=[1, 100],
        ticksuffix='%',
        row = 2,
        col = 1 
    )

    fig.update_xaxes(
        title='Day',
        type = 'category',
        row = 2,
        col = 1
    )

    fig.show()

def genScatter(data, species, type, color, title):
    all_days = extract_unique('day', data)

    return go.Scatter(
            x = all_days, 
            y = calcAverage(data, species, type), 
            line = dict(color = color, width = 4),
            name = title,
            stackgroup='one',
            groupnorm='percent'
        )

def calcAverage(data, species, dataToConsider):

    all_days = extract_unique('day', data)

    averages = []

    for day in all_days:
        values = [i[dataToConsider] for i in data if i['day'] == day and i['species'] == species]

        average = sum(values)
        if len(values) != 0: average = sum(values)/len(values)

        averages.append(average)
    
    return averages



if __name__ == '__main__':
    plot()