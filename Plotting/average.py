import pandas as pd
import plotly.express as px
from util.file_finder import get_full_path


full_path = get_full_path('overview.csv')

df = pd.read_csv(full_path)
fig = px.scatter_3d(
    df,
    x='day',
    y='speed',
    z='size',
    color='amount',
    title='Average Values'
)

fig.show()
