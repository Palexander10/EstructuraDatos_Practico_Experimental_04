namespace VuelosBaratos;

/// <summary>Resultado de una busqueda: costo total y lista ordenada de vuelos.</summary>
public record Ruta(double Costo, List<Vuelo> Vuelos);

public static class Algoritmos
{
    /// <summary>Dijkstra con cola de prioridad. Devuelve null si no hay ruta.</summary>
    public static Ruta? RutaMasBarata(GrafoVuelos grafo, string origen, string destino)
    {
        if (!grafo.Existe(origen) || !grafo.Existe(destino)) return null;

        var costo = new Dictionary<string, double> { [origen] = 0.0 };
        var previo = new Dictionary<string, Vuelo>();
        var visitados = new HashSet<string>();
        var cola = new PriorityQueue<string, double>();
        cola.Enqueue(origen, 0.0);

        while (cola.TryDequeue(out string? actual, out double acumulado))
        {
            if (!visitados.Add(actual)) continue;
            if (actual == destino) break;

            foreach (Vuelo vuelo in grafo.VuelosDesde(actual))
            {
                double nuevo = acumulado + vuelo.Precio;
                if (!costo.TryGetValue(vuelo.Destino, out double conocido) || nuevo < conocido)
                {
                    costo[vuelo.Destino] = nuevo;
                    previo[vuelo.Destino] = vuelo;
                    cola.Enqueue(vuelo.Destino, nuevo);
                }
            }
        }

        if (!costo.ContainsKey(destino)) return null;
        return new Ruta(costo[destino], Reconstruir(previo, origen, destino));
    }

    /// <summary>BFS: minimiza la cantidad de vuelos sin considerar el precio.</summary>
    public static Ruta? RutaMenosEscalas(GrafoVuelos grafo, string origen, string destino)
    {
        if (!grafo.Existe(origen) || !grafo.Existe(destino)) return null;

        var previo = new Dictionary<string, Vuelo>();
        var visitados = new HashSet<string> { origen };
        var cola = new Queue<string>();
        cola.Enqueue(origen);

        while (cola.Count > 0)
        {
            string actual = cola.Dequeue();
            if (actual == destino) break;

            foreach (Vuelo vuelo in grafo.VuelosDesde(actual))
            {
                if (visitados.Add(vuelo.Destino))
                {
                    previo[vuelo.Destino] = vuelo;
                    cola.Enqueue(vuelo.Destino);
                }
            }
        }

        if (destino != origen && !previo.ContainsKey(destino)) return null;
        var ruta = Reconstruir(previo, origen, destino);
        return new Ruta(ruta.Sum(v => v.Precio), ruta);
    }

    /// <summary>Dijkstra sobre estados (aeropuerto, vuelos usados) para limitar las escalas.</summary>
    public static Ruta? RutaBarataConLimite(GrafoVuelos grafo, string origen, string destino, int maxVuelos)
    {
        if (!grafo.Existe(origen) || !grafo.Existe(destino)) return null;

        var mejor = new Dictionary<(string, int), double> { [(origen, 0)] = 0.0 };
        var previo = new Dictionary<(string, int), (Vuelo Vuelo, (string, int) Anterior)>();
        var cola = new PriorityQueue<(string Aeropuerto, int Usados), double>();
        cola.Enqueue((origen, 0), 0.0);

        while (cola.TryDequeue(out var estadoActual, out double acumulado))
        {
            var (actual, usados) = estadoActual;

            if (actual == destino)
            {
                var ruta = new List<Vuelo>();
                var estado = estadoActual;
                while (previo.TryGetValue(estado, out var paso))
                {
                    ruta.Add(paso.Vuelo);
                    estado = paso.Anterior;
                }
                ruta.Reverse();
                return new Ruta(acumulado, ruta);
            }

            if (acumulado > mejor.GetValueOrDefault(estadoActual, double.PositiveInfinity)) continue;
            if (usados == maxVuelos) continue;

            foreach (Vuelo vuelo in grafo.VuelosDesde(actual))
            {
                var siguiente = (vuelo.Destino, usados + 1);
                double nuevo = acumulado + vuelo.Precio;
                if (nuevo < mejor.GetValueOrDefault(siguiente, double.PositiveInfinity))
                {
                    mejor[siguiente] = nuevo;
                    previo[siguiente] = (vuelo, estadoActual);
                    cola.Enqueue(siguiente, nuevo);
                }
            }
        }
        return null;
    }

    /// <summary>DFS iterativo con pila: aeropuertos a los que se puede llegar desde el origen.</summary>
    public static List<string> Alcanzables(GrafoVuelos grafo, string origen)
    {
        if (!grafo.Existe(origen)) return new List<string>();

        var visitados = new HashSet<string> { origen };
        var pila = new Stack<string>();
        pila.Push(origen);

        while (pila.Count > 0)
        {
            string actual = pila.Pop();
            foreach (Vuelo vuelo in grafo.VuelosDesde(actual))
            {
                if (visitados.Add(vuelo.Destino))
                    pila.Push(vuelo.Destino);
            }
        }

        visitados.Remove(origen);
        var resultado = visitados.ToList();
        resultado.Sort(StringComparer.Ordinal);
        return resultado;
    }

    private static List<Vuelo> Reconstruir(Dictionary<string, Vuelo> previo, string origen, string destino)
    {
        var ruta = new List<Vuelo>();
        string actual = destino;
        while (actual != origen)
        {
            Vuelo vuelo = previo[actual];
            ruta.Add(vuelo);
            actual = vuelo.Origen;
        }
        ruta.Reverse();
        return ruta;
    }
}
