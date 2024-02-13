using System.ComponentModel.DataAnnotations.Schema;

namespace CSI.IBTA.Shared.Entities
{
    public class Email
    {
        public int Id { get; set; }
        [Column("Email")]
        public string EmailAddress { get; set; } = null!;
        public User Account { get; set; } = null!;
    }
}
