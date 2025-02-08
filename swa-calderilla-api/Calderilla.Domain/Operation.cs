using System.ComponentModel.DataAnnotations;

namespace Calderilla.Domain
{
    public class Operation
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public int MonthOperationNumber { get; set; }

        [Required]
        public DateTime OperationDate { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]
        public DateTime ValueDate { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public decimal Balance { get; set; }

        [Required]
        public required string Payer { get; set; }

        [Required]
        public bool Ignore { get; set; }

        public string? Type { get; set; }

        public string? Notes { get; set; }

        [Required]
        public bool Reviewed { get; set; }

    }
}
