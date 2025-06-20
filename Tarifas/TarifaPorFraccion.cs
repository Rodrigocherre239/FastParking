using System;

public class TarifaPorFraccion : ICalculadoraTarifa
{
    public decimal CalcularTarifa(Vehiculo vehiculo, TimeSpan duracion)
    {
        decimal tarifaPorBloque = 2;
        int bloques = (int)Math.Ceiling(duracion.TotalMinutes / 15.0);
        return tarifaPorBloque * bloques;
    }
}
