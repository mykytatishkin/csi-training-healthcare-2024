using System.ComponentModel.DataAnnotations.Schema;

namespace CSI.IBTA.Shared.Entities
{
    public class Email
    {
        public int Id { get; set; }
        [Column("Email")]
        public int EmailAddress { get; set; }
        public Account Account { get; set; } = null!;
    }
}
