using System;
using System.Collections.Generic;
using System.Linq;

public class Estacionamiento
{
    private readonly Dictionary<string, Vehiculo> _vehiculosEnParqueo = new();
    private readonly ICalculadoraTarifa _calculadoraTarifa;
    private readonly HistorialSalida _historial;

    public Estacionamiento(ICalculadoraTarifa calculadoraTarifa, HistorialSalida historial)
    {
        _calculadoraTarifa = calculadoraTarifa;
        _historial = historial;
    }

    public void IngresarVehiculo(Vehiculo vehiculo)
    {
        if (!_vehiculosEnParqueo.ContainsKey(vehiculo.Placa))
            _vehiculosEnParqueo.Add(vehiculo.Placa, vehiculo);
    }

    public decimal RetirarVehiculo(string placa, out TimeSpan duracion)
    {
        if (_vehiculosEnParqueo.TryGetValue(placa, out var vehiculo))
        {
            duracion = DateTime.Now - vehiculo.HoraIngreso;
            decimal monto = _calculadoraTarifa.CalcularTarifa(vehiculo, duracion);
            _vehiculosEnParqueo.Remove(placa);
            _historial.RegistrarSalida(vehiculo, duracion, monto);
            return monto;
        }

        duracion = TimeSpan.Zero;
        return 0;
    }

    public List<string> ObtenerPlacasActuales()
    {
        return _vehiculosEnParqueo.Keys.ToList();
    }

    public IEnumerable<HistorialSalida.RegistroSalida> ObtenerHistorial()
    {
        return _historial.ObtenerHistorial();
    }

    public bool EspacioOcupado(string espacio)
    {
        return _vehiculosEnParqueo.Values.Any(v => v.EspacioAsignado == espacio);
    }

    public List<string> ObtenerEspaciosOcupados()
    {
        return _vehiculosEnParqueo.Values.Select(v => v.EspacioAsignado).ToList();
    }
}