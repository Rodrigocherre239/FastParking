using System;

public interface ICalculadoraTarifa
{
    decimal CalcularTarifa(Vehiculo vehiculo, TimeSpan duracion);
}
