using System.ComponentModel.DataAnnotations.Schema;

namespace CSI.IBTA.Shared.Entities
{
    public class Email
    {
        public int Id { get; set; }
        [Column("Email")]
        public string EmailAddress { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
