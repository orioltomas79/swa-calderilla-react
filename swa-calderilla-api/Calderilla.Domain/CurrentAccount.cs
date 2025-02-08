using System.ComponentModel.DataAnnotations;

namespace Calderilla.Domain
{
    public class CurrentAccount
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Type { get; set; }
    }
}
