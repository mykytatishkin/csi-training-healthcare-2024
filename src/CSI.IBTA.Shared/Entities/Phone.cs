using System.ComponentModel.DataAnnotations.Schema;

namespace CSI.IBTA.Shared.Entities
{
    public class Phone
    {
        public int Id { get; set; }
        [Column("Phone")]
        public string PhoneNumber { get; set; } = null!;
        public User Account { get; set; } = null!;
    }
}
