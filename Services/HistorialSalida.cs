using System;
using System.Collections.Generic;

public class HistorialSalida
{
    public record RegistroSalida(
    string Placa,
    TipoVehiculo Tipo,
    TimeSpan Tiempo,
    decimal Monto,
    DateTime HoraIngreso,
    DateTime HoraSalida
);


    private readonly List<RegistroSalida> _registros = new();

    public void RegistrarSalida(Vehiculo v, TimeSpan tiempo, decimal monto)
{
    var salida = DateTime.Now;
    _registros.Add(new RegistroSalida(v.Placa, v.Tipo, tiempo, monto, v.HoraIngreso, salida));
}


    public IEnumerable<RegistroSalida> ObtenerHistorial()
    {
        return _registros;
    }
}
