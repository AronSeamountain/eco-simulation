<div align="center">
  <a href="https://github.com/AronSeamountain/eco-simulation">
    <img alt="logo" src="Meta/logo.png" width=85%>
  </a>
</div>

<h2 align="center">
   Simulation an Ecosystem 
</h2>
<h3 align="center"> Implementing natural selecetion in a prey-predator environment</h3>

<h4>Aron Sjöberg</h4>
<h4>Emil Wingårdh</h4>
<h4>Erik Söderpalm</h4>
<h4>Theo Wiik</h4>
<h4>Viktor Fredholm</h4>
<h4>Yvonne Hansson</h4>


This is a simulation contains rabbits and wolves in a natural environment. 

This was created as a part of our Bachelor's thesis project. More information about this can be found in our report found here: [CSE_Bachelor_s_Thesis.pdf](https://github.com/AronSeamountain/eco-simulation/files/6575265/CSE_Bachelor_s_Thesis.pdf)

This readme covers the following sections:

- [Dev Tools](#dev-tools)
  - [Plot Data](#plot-data)
    - [Average Values](#average-values)
    - [Amount of Animals](#amount-of-animals)
    - [FPS](#fps)
  - [Render State Diagrams](#render-state-diagrams)
- [Simulation Details](#simulation-details)
  - [Time relations](#time-relations)
  - [Wolves](#wolves)
  - [Rabbits](#rabbits)

# Dev Tools

Tools used for development and analysis.

## Plot Data

Install python packages with `pip install -r requirements.txt` inside the Plotting folder. Run a script in `Plotting`, it searches **recursively** for the log files from the directory the script is being ran from.

### Average Values

Run `average.py`. opens an interactive graph. Searches for `detailed.json`.

<div align="center">
  <img alt="average plot" src="Meta/Plots/average.png" width="600px">
</div>

### Amount of Animals

Run `n_animals.py`. Searches for `detailed.json`.

<div align="center">
  <img alt="average plot" src="Meta/Plots/n_animals.png" width="600px">
</div>

### FPS

Run `avg_fps.py`. plots fps. Searches for `fps.csv`, `overview.csv`, `fps_baseline.csv` (the baseline fps to compare to).

<div align="center">
  <img alt="average plot" src="Meta/Plots/avg_fps.png" width="600px">
</div>

## Render State Diagrams

`java -jar plantuml.jar states.txt` in `Meta/StateDiagrams`

# Simulation Details

This section covers information about the simulation.

## Time relations

The time in the simulation is not a perfect model of reality. We do however want the relationships between the animals fertilties/pregnancy times to be correct.
Therefore we have said that 12 hour in the game represents 1 month in real life.
1 day is therefore 2 months

## Wolves

https://en.wikipedia.org/wiki/Wolf
https://www.nationalgeographic.org/media/growing-up-wolf/
Pregnancy time wolf 62-75 days
Offspring amount 5-6
Fertility time after 2 years
Generally can have 1 litter per year.
Live for 5-12 years


## Rabbits

https://en.wikipedia.org/wiki/Rabbit
Pregnancy time rabbit 28-35 days
Offspring average amount 5-6 but ranges from 2 to 10.
Fertility time 5-8 months
Can get pregnant the day after being pregnant...
