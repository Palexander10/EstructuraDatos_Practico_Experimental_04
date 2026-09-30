using System.Globalization;

namespace VuelosBaratos;

/// <summary>
/// Grafo dirigido y ponderado con lista de adyacencia.
/// Cada aeropuerto es un vertice y cada vuelo una arista cuyo peso principal es el precio.
/// </summary>
public class GrafoVuelos
{
    private readonly Dictionary<string, List<Vuelo>> _adyacencia = new();

    public void AgregarAeropuerto(string codigo)
    {
        if (!_adyacencia.ContainsKey(codigo))
            _adyacencia[codigo] = new List<Vuelo>();
    }

    public void AgregarVuelo(Vuelo vuelo)
    {
        AgregarAeropuerto(vuelo.Origen);
        AgregarAeropuerto(vuelo.Destino);
        _adyacencia[vuelo.Origen].Add(vuelo);
    }

    /// <summary>Lee un archivo con formato origen,destino,aerolinea,precio,duracion.</summary>
    public int CargarDesdeArchivo(string ruta)
    {
        int cargados = 0;
        int numero = 0;
        foreach (string lineaCruda in File.ReadLines(ruta))
        {
            numero++;
            string linea = lineaCruda.Trim();
            if (linea.Length == 0 || linea.StartsWith('#'))
                continue;

            string[] partes = linea.Split(',').Select(p => p.Trim()).ToArray();
            if (partes.Length != 5)
            {
                Console.WriteLine($"Linea {numero} ignorada: formato incorrecto");
                continue;
            }

            bool precioValido = double.TryParse(partes[3], NumberStyles.Float,
                CultureInfo.InvariantCulture, out double precio);
            bool duracionValida = int.TryParse(partes[4], out int duracion);
            if (!precioValido || !duracionValida)
            {
                Console.WriteLine($"Linea {numero} ignorada: precio o duracion invalidos");
                continue;
            }

            AgregarVuelo(new Vuelo(partes[0].ToUpper(), partes[1].ToUpper(), partes[2], precio, duracion));
            cargados++;
        }
        return cargados;
    }

    public List<string> Aeropuertos()
    {
        var lista = _adyacencia.Keys.ToList();
        lista.Sort(StringComparer.Ordinal);
        return lista;
    }

    public IReadOnlyList<Vuelo> VuelosDesde(string codigo) =>
        _adyacencia.TryGetValue(codigo, out var lista) ? lista : Array.Empty<Vuelo>();

    public int TotalVuelos() => _adyacencia.Values.Sum(v => v.Count);

    public bool Existe(string codigo) => _adyacencia.ContainsKey(codigo);
}
