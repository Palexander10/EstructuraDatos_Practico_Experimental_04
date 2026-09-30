using System.Diagnostics;

namespace VuelosBaratos;

public static class Benchmark
{
    private static (GrafoVuelos Grafo, string[] Codigos) GenerarGrafo(int aeropuertos, int salidas, int semilla = 7)
    {
        var azar = new Random(semilla);
        var grafo = new GrafoVuelos();
        var codigos = Enumerable.Range(0, aeropuertos).Select(i => $"A{i:D5}").ToArray();
        foreach (string c in codigos) grafo.AgregarAeropuerto(c);

        foreach (string origen in codigos)
        {
            for (int k = 0; k < salidas; k++)
            {
                string destino = codigos[azar.Next(codigos.Length)];
                if (destino == origen) continue;
                double precio = Math.Round(40 + azar.NextDouble() * 760, 2);
                grafo.AgregarVuelo(new Vuelo(origen, destino, "Sim", precio, azar.Next(40, 701)));
            }
        }
        return (grafo, codigos);
    }

    private static double Medir(Action accion, int repeticiones = 5)
    {
        accion(); // llamada de calentamiento para que el compilador JIT no distorsione la medida
        double mejor = double.MaxValue;
        for (int i = 0; i < repeticiones; i++)
        {
            var reloj = Stopwatch.StartNew();
            accion();
            reloj.Stop();
            mejor = Math.Min(mejor, reloj.Elapsed.TotalMilliseconds);
        }
        return mejor;
    }

    public static void Ejecutar()
    {
        Console.WriteLine("\nANALISIS DE TIEMPO DE EJECUCION");
        Console.WriteLine($"{"V",7} {"E",8} {"Dijkstra (ms)",15} {"BFS (ms)",10}");
        Console.WriteLine(new string('-', 44));
        foreach (int n in new[] { 100, 1000, 5000, 20000 })
        {
            var (grafo, codigos) = GenerarGrafo(n, 5);
            string origen = codigos[0], destino = codigos[^1];
            double tDijkstra = Medir(() => Algoritmos.RutaMasBarata(grafo, origen, destino));
            double tBfs = Medir(() => Algoritmos.RutaMenosEscalas(grafo, origen, destino));
            Console.WriteLine($"{n,7} {grafo.TotalVuelos(),8} {tDijkstra,15:F3} {tBfs,10:F3}");
        }
    }
}
