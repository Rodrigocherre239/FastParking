using System;

public class TarifaPorTipoVehiculo : ICalculadoraTarifa
{
    public decimal CalcularTarifa(Vehiculo vehiculo, TimeSpan duracion)
    {
        int minutos = (int)duracion.TotalMinutes;

        if (vehiculo.Tipo == TipoVehiculo.Delivery)
        {
            if (minutos <= 30)
                return 0;
            else
                return 2 * (decimal)Math.Ceiling(duracion.TotalHours);
        }

        decimal tarifa = vehiculo.Tipo switch
        {
            TipoVehiculo.Moto => 2,
            TipoVehiculo.Auto => 3,
            TipoVehiculo.Camion => 4,
            _ => 4
        };

        return tarifa * (decimal)Math.Ceiling(duracion.TotalHours);
    }
}
