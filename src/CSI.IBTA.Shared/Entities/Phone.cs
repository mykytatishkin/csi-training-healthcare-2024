using System.ComponentModel.DataAnnotations.Schema;

namespace CSI.IBTA.Shared.Entities
{
    internal class Phone
    {
        public int Id { get; set; }
        [Column("Phone")]
        public int PhoneNumber { get; set; }
        public Account Account { get; set; } = null!;
    }
}
