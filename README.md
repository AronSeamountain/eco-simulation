<div align="center">
  <a href="https://github.com/AronSeamountain/eco-simulation">
    <img alt="logo" src="Meta/logo.png">
  </a>
</div>

<h2 align="center">
  Ecosystem - A bachelor thesis project.
</h2>
This is a simulation contains rabbits and wolfs in a natural environment.

<h3 align="center">
  Time relations
</h3>
The time in the simulation is not a perfect model of reality. We do however want the relationships between the animals fertilties/pregnancy times to be correct.
Therefore we have said that 12 hour in the game represents 1 month in real life.
### Graph Data with Plotly
1. Install python packages with `pip install -r requirements.txt` inside the Plotting folder.
2. Run the script, it searches recursively for data_log.csv from the directory the script is ran from.

### Render State Diagrams
`java -jar plantuml.jar states.txt` in `Meta/StateDiagrams`
