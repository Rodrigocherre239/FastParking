using System;

public class Vehiculo
{
    public string Placa { get; }
    public TipoVehiculo Tipo { get; }
    public DateTime HoraIngreso { get; }

    public Vehiculo(string placa, TipoVehiculo tipo)
    {
        Placa = placa;
        Tipo = tipo;
        HoraIngreso = DateTime.Now;
    }
}
