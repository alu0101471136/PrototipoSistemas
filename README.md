# LifeLike 
Proyecto de unity en el que se simula el comportamiento de dos agentes autónomos que interactúan entre sí y con el entorno. Este proyecto se ha realizado con la herramienta ml-agents de unity, y se ha utilizado el algoritmo de aprendizaje por refuerzo PPO.

## Composición del repositorio
Este repositorio contiene diferentes ramas en las que se encuentra cada parte del proyecto. Dentro de la rama cazador se encuentra el proyecto de unity en el que se simula el comportamiento de un agente autónomo que se mueve en un entorno y persigue a una presa. Dentro de la rama presa se encuentra el proyecto de unity en el que se simula el comportamiento de un agente autónomo que se mueve en un entorno y huye de un cazador. Dentro de la rama unidos se encuentra el proyecto de unity en el que se simula el comportamiento de dos agentes autónomos que se mueven en un entorno y se persiguen entre sí.

### Modelos
Los modelos generados en los entrenamientos se encuentran en la carpeta `results` de cada rama, estan diferenciados por entrenemientos, por lo que se puede encontrar el mejor modelo de cada entrenamiento.

## Cazador

### Objetivo
El objetivo de este agente es simular el comportamiento de un agente autónomo que se mueve en un entorno y persigue a una presa.

### Aprenizaje por refuerzo
Para entrenar al agente autónomo se ha utilizado el algoritmo de aprendizaje por refuerzo PPO. El entrenamiento de este agente fue el más costoso ya que se ha implementado una mecánica que limita la visión del agente, con el fin de nivelar la persecución. Al tener una visión limitada, el agente no puede ver a la presa lo que dificulta la fase de entrenamiento ya que para que la red neuronal aprenda, el agente debe ver a la presa. Para solucionar este problema se ha hecho un aprendizaje previo con conocimiento constante de la presa, y una vez que la red neuronal ha aprendido a perseguir a la presa, se ha limitado la visión del agente.

## Presa
### Objetivo
El objetivo de este agente es simular el comportamiento de un agente autónomo que se mueve en un entorno y huye de un cazador.

### Aprenizaje por refuerzo
Para entrenar al agente autónomo se ha utilizado el algoritmo de aprendizaje por refuerzo PPO. El entrenamiento de este agente fue el más sencillo ya que no se ha implementado ninguna mecánica que lo limite, no como en el caso anterior, por eso el entrenamiento de este fue simplemente la presa siendo perseguida por un cazador que siempre va hacia él. El agente puede ver al cazador en todo momento, lo que facilita la fase de entrenamiento.

## Unidos
### Objetivo
El objetivo de esta fase es simular el comportamiento de dos agentes autónomos que se mueven en un entorno y compiten entre sí.

### Aprenizaje por refuerzo
Este entrenamiento simplemente consiste en entrenar simultaneamente a los dos agentes autónomos, para que aprendan a competir entre sí. Fruto de esta fase se ha obtenido un comportamiento mediocre de la presa, mientras que el cazador ha aprendido a perseguir a la presa. Por lo que aparentemente los agentes han aprendido mejor en las fases individuales.