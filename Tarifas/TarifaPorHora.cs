using System;

public class TarifaPorHora : ICalculadoraTarifa
{
    public decimal CalcularTarifa(Vehiculo vehiculo, TimeSpan duracion)
    {
        decimal tarifaPorHora = 5;
        int horas = (int)Math.Ceiling(duracion.TotalHours);
        return tarifaPorHora * horas;
    }
}
