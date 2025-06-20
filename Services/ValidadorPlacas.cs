using System.Text.RegularExpressions;

public static class ValidadorPlacas
{
    public static bool EsValida(string placa)
    {
        return Regex.IsMatch(placa, @"^[A-Z]{3}\d{3}$");
    }
}
