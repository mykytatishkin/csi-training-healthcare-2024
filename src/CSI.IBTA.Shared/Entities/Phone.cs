using System.ComponentModel.DataAnnotations.Schema;

namespace CSI.IBTA.Shared.Entities
{
    public class Phone
    {
        public int Id { get; set; }
        [Column("Phone")]
        public string PhoneNumber { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
