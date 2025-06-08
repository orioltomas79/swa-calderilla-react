using System.ComponentModel.DataAnnotations;

namespace Calderilla.Domain;

public class OperationType
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = default!;
}
