using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TazaShop.Models
{
    public class Size
    {
        public Size()
        {
            Carts = new HashSet<Cart>();
        }
        [Key]
        public int Id { get; set; }
        public int UnitId { get; set; }

        [ForeignKey(nameof(UnitId))]
        [InverseProperty("Sizes")]
        public virtual Unit Unit { get; set; }

        public int ProductSize { get; set; }
        public int Qty { get; set; }

        [InverseProperty(nameof(Cart.Size))]
        public virtual ICollection<Cart> Carts { get; set; }
    }
}
