using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TazaShop.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public int CustomerId { get; set; }

        [ForeignKey(nameof(CustomerId))]
        [InverseProperty("Carts")]
        public virtual Customer Customer { get; set; }

        public int SizeId { get; set; }

        [ForeignKey(nameof(SizeId))]
        [InverseProperty("Carts")]
        public virtual Size Size { get; set; }

        public int Qty { get; set; }
    }
}
