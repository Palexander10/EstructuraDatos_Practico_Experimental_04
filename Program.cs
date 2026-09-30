using VuelosBaratos;

string rutaDatos = Path.Combine(AppContext.BaseDirectory, "datos", "vuelos.txt");
var grafo = new GrafoVuelos();
int cargados = grafo.CargarDesdeArchivo(rutaDatos);
Console.WriteLine($"Se cargaron {cargados} vuelos desde {rutaDatos}");

string PedirCodigo(string mensaje)
{
    Console.Write(mensaje);
    return (Console.ReadLine() ?? "").Trim().ToUpper();
}

while (true)
{
    Console.WriteLine("\n=== BUSCADOR DE VUELOS BARATOS ===");
    Console.WriteLine("1. Ver aeropuertos");
    Console.WriteLine("2. Ver lista de adyacencia");
    Console.WriteLine("3. Ver vuelos desde un aeropuerto");
    Console.WriteLine("4. Ruta mas barata (Dijkstra)");
    Console.WriteLine("5. Ruta con menos escalas (BFS)");
    Console.WriteLine("6. Ruta barata con limite de vuelos");
    Console.WriteLine("7. Destinos alcanzables (DFS)");
    Console.WriteLine("8. Analisis de tiempo de ejecucion");
    Console.WriteLine("0. Salir");
    Console.Write("Opcion: ");
    string opcion = (Console.ReadLine() ?? "").Trim();

    if (opcion == "0")
    {
        Console.WriteLine("Hasta pronto.");
        break;
    }

    switch (opcion)
    {
        case "1":
            Reporteria.MostrarAeropuertos(grafo);
            break;
        case "2":
            Reporteria.MostrarListaAdyacencia(grafo);
            break;
        case "3":
            Reporteria.MostrarVuelosDesde(grafo, PedirCodigo("Aeropuerto: "));
            break;
        case "4":
        {
            string o = PedirCodigo("Origen: "), d = PedirCodigo("Destino: ");
            Reporteria.MostrarRuta("RUTA MAS BARATA", Algoritmos.RutaMasBarata(grafo, o, d));
            break;
        }
        case "5":
        {
            string o = PedirCodigo("Origen: "), d = PedirCodigo("Destino: ");
            Reporteria.MostrarRuta("RUTA CON MENOS ESCALAS", Algoritmos.RutaMenosEscalas(grafo, o, d));
            break;
        }
        case "6":
        {
            string o = PedirCodigo("Origen: "), d = PedirCodigo("Destino: ");
            Console.Write("Maximo de vuelos: ");
            if (!int.TryParse(Console.ReadLine(), out int limite))
            {
                Console.WriteLine("Debe ingresar un numero entero.");
                break;
            }
            Reporteria.MostrarRuta($"RUTA BARATA CON MAXIMO {limite} VUELOS",
                Algoritmos.RutaBarataConLimite(grafo, o, d, limite));
            break;
        }
        case "7":
        {
            var lista = Algoritmos.Alcanzables(grafo, PedirCodigo("Origen: "));
            Console.WriteLine("Alcanzables: " + (lista.Count > 0 ? string.Join(", ", lista) : "ninguno"));
            break;
        }
        case "8":
            Benchmark.Ejecutar();
            break;
        default:
            Console.WriteLine("Opcion no valida.");
            break;
    }
}
