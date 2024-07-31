namespace Jarogor.Extensions.CodeAnalysis.CSharp;

/// <summary>
/// Информация об атрибуте.
/// </summary>
/// <param name="Position">Позиция параметра</param>
/// <param name="Name">Название параметра</param>
/// <param name="Value">Значение параметра</param>
public record struct AttributeInfo(int Position, string Name, string Value);
