# Buscador de vuelos baratos con grafos (C#)

Practica 04 de Estructura de Datos, Universidad Estatal Amazonica.

El programa modela una red de vuelos como un grafo dirigido y ponderado. Cada aeropuerto es un
vertice y cada vuelo es una arista cuyo peso es el precio del boleto. Los datos se leen desde
`datos/vuelos.txt`.

## Requisitos
.NET SDK 8.0 o superior. No usa paquetes externos.

## Ejecucion
```
dotnet run
```

## Archivos
| Archivo | Funcion |
|---|---|
| Vuelo.cs | Registro Vuelo, que representa una arista |
| GrafoVuelos.cs | Lista de adyacencia y carga desde archivo |
| Algoritmos.cs | Dijkstra, BFS, Dijkstra con limite de vuelos y DFS |
| Reporteria.cs | Metodos para mostrar aeropuertos, vuelos y rutas |
| Benchmark.cs | Medicion del tiempo de ejecucion con grafos sinteticos |
| Program.cs | Menu de consola |
| datos/vuelos.txt | Base de datos ficticia de vuelos |
