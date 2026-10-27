using System.Globalization;

namespace Calificaciones.Services;

/// <summary>
/// Holds the grading rules shared by the screen and its report.
/// </summary>
public static class GradeCalculator
{
    private static readonly CultureInfo SpanishCulture = CultureInfo.GetCultureInfo("es-EC");

    public static bool TryParseGrade(string rawValue, out decimal grade, out string error)
    {
        grade = 0m;
        error = "Ingresa un número entre 0 y 10.";

        var value = rawValue.Trim();
        if (value.Length == 0)
        {
            error = "Ingresa una calificación.";
            return false;
        }

        if (value.Any(character => !char.IsDigit(character) && character is not '.' and not ','))
        {
            error = "Usa solo números y un separador decimal.";
            return false;
        }

        if (value.Count(character => character is '.' or ',') > 1
            || (value.Contains('.') && value.Contains(',')))
        {
            error = "Usa una sola coma o un solo punto decimal.";
            return false;
        }

        if (value.EndsWith('.') || value.EndsWith(','))
        {
            error = "Completa los dígitos después del separador decimal.";
            return false;
        }

        var normalizedValue = value.Replace(',', '.');
        if (!decimal.TryParse(normalizedValue, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out grade))
        {
            error = "Ingresa una nota numérica válida.";
            return false;
        }

        if (grade is < 0m or > 10m)
        {
            error = "La nota debe estar entre 0 y 10.";
            return false;
        }

        error = string.Empty;
        return true;
    }

    public static decimal CalculatePartial(decimal followUp, decimal exam) =>
        followUp * 0.3m + exam * 0.2m;

    public static decimal CalculateFinal(decimal partialOne, decimal partialTwo) =>
        partialOne + partialTwo;

    public static string Classify(decimal finalScore) => finalScore switch
    {
        >= 7m => "APROBADO",
        >= 5m => "COMPLEMENTARIO",
        _ => "REPROBADO"
    };

    public static string Format(decimal score) => score.ToString("F2", SpanishCulture);

    public static string BuildReport(string student, DateTime evaluationDate,
        decimal partialOne, decimal partialTwo, decimal finalScore) =>
        $"Nombre: {student}\n"
        + $"Fecha: {evaluationDate.ToString("dd/MM/yyyy", SpanishCulture)}\n"
        + $"Nota Parcial 1: {Format(partialOne)}\n"
        + $"Nota Parcial 2: {Format(partialTwo)}\n"
        + $"Nota Final: {Format(finalScore)}\n"
        + $"Estado: {Classify(finalScore)}";
}
