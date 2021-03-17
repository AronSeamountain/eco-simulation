import glob

import pandas as pd
import plotly.express as px

file_name = 'log.csv'
text_files = glob.glob("./**/" + file_name, recursive = True)

if not text_files:
  raise Exception('Found no ' + file_name + ' file.')

full_path = text_files[0]

# Plot
df = pd.read_csv(full_path)
#fig = px.line(df, x = 'day', y = 'value', title='My Cool Graph')
fig = px.scatter_3d(df, x='day', y='speed', z='size', color='amount')

fig.show()
