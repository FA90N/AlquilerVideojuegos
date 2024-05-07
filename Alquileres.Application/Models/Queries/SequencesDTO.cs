namespace Alquileres.Application.Models.Queries;

public sealed record class SequencesDTO
{
    public int Id { get; set; }

    public string EntityName { get; set; } = null!;

    public int LastNumber { get; set; }

    public string LastNumberFormat { get; set; } = null!;

    public bool ResetYear { get; set; }

    public int? Year { get; set; }
}
