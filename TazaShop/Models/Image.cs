
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TazaShop.Models
{
    public class Image
    {
        [Key]
        public int Id { get; set; }
        public int UnitId { get; set; }

        [ForeignKey(nameof(UnitId))]
        [InverseProperty("Images")]
        public virtual Unit Unit { get; set; }

        public string ImagePath { get; set; }
    }
}
