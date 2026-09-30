namespace VuelosBaratos;

/// <summary>Arista dirigida y ponderada del grafo.</summary>
public record Vuelo(string Origen, string Destino, string Aerolinea, double Precio, int Duracion)
{
    public override string ToString() =>
        $"{Origen} -> {Destino} | {Aerolinea,-14} | ${Precio,7:F2} | {Duracion} min";
}
