namespace Calderilla.Domain
{
    public class Operation
    {
        public Guid Id { get; set; }

        public int MonthOperationNumber { get; set; }

        public DateTime OperationDate { get; set; }

        public required string Description { get; set; }

        public DateTime ValueDate { get; set; }

        public decimal Amount { get; set; }

        public decimal Balance { get; set; }

        public required string Payer { get; set; }

        public bool Ignore { get; set; }

        public string? Type { get; set; }

        public string? Notes { get; set; }

        public bool Reviewed { get; set; }

    }
}
