namespace VuelosBaratos;

public static class Reporteria
{
    public static void MostrarAeropuertos(GrafoVuelos grafo)
    {
        Console.WriteLine("\nAEROPUERTOS REGISTRADOS");
        Console.WriteLine(new string('-', 40));
        foreach (string codigo in grafo.Aeropuertos())
            Console.WriteLine($"{codigo}  salidas: {grafo.VuelosDesde(codigo).Count}");
        Console.WriteLine($"Total: {grafo.Aeropuertos().Count} aeropuertos, {grafo.TotalVuelos()} vuelos");
    }

    public static void MostrarListaAdyacencia(GrafoVuelos grafo)
    {
        Console.WriteLine("\nLISTA DE ADYACENCIA");
        Console.WriteLine(new string('-', 60));
        foreach (string codigo in grafo.Aeropuertos())
        {
            var destinos = grafo.VuelosDesde(codigo).Select(v => $"{v.Destino}(${v.Precio:F0})");
            string texto = string.Join(", ", destinos);
            Console.WriteLine($"{codigo}: {(texto.Length > 0 ? texto : "sin salidas")}");
        }
    }

    public static void MostrarVuelosDesde(GrafoVuelos grafo, string codigo)
    {
        if (!grafo.Existe(codigo))
        {
            Console.WriteLine($"El aeropuerto {codigo} no existe en la base de datos.");
            return;
        }
        Console.WriteLine($"\nVUELOS DESDE {codigo}");
        Console.WriteLine(new string('-', 60));
        foreach (Vuelo vuelo in grafo.VuelosDesde(codigo).OrderBy(v => v.Precio))
            Console.WriteLine(vuelo);
    }

    public static void MostrarRuta(string titulo, Ruta? resultado)
    {
        Console.WriteLine($"\n{titulo}");
        Console.WriteLine(new string('-', 60));
        if (resultado is null)
        {
            Console.WriteLine("No existe una ruta disponible con esas condiciones.");
            return;
        }
        if (resultado.Vuelos.Count == 0)
        {
            Console.WriteLine("El origen y el destino son el mismo aeropuerto.");
            return;
        }
        for (int i = 0; i < resultado.Vuelos.Count; i++)
            Console.WriteLine($"{i + 1}. {resultado.Vuelos[i]}");

        int minutos = resultado.Vuelos.Sum(v => v.Duracion);
        Console.WriteLine($"Vuelos: {resultado.Vuelos.Count} | Escalas: {resultado.Vuelos.Count - 1} | " +
                          $"Costo total: ${resultado.Costo:F2} | Tiempo en aire: {minutos / 60} h {minutos % 60} min");
    }
}
