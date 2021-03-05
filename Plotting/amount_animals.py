import glob

import pandas as pd
import plotly.express as px
import plotly.graph_objects as go

file_name = 'log.csv'
text_files = glob.glob("./**/" + file_name, recursive=True)

if not text_files:
    raise Exception('Found no ' + file_name + ' file.')

full_path = text_files[0]

df = pd.read_csv(full_path)

# Plot
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
    title='Ecosimulation',
    xaxis_title='Day'
)

fig.show()
