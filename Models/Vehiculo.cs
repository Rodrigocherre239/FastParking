using System;

public class Vehiculo
{
    public string Placa { get; set; }
    public TipoVehiculo Tipo { get; set; }
    public DateTime HoraIngreso { get; set; }
    public string EspacioAsignado { get; set; } // Nuevo

    public Vehiculo(string placa, TipoVehiculo tipo, string espacio)
    {
        Placa = placa;
        Tipo = tipo;
        HoraIngreso = DateTime.Now;
        EspacioAsignado = espacio;
    }
}