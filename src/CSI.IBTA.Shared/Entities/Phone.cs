using System.ComponentModel.DataAnnotations.Schema;

namespace CSI.IBTA.Shared.Entities
{
    public class Phone
    {
        public int Id { get; set; }
        [Column("Phone")]
        public string PhoneNumber { get; set; }
        public Account Account { get; set; } = null!;
    }
}
