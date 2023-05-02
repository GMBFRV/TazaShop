using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TazaShop.Models
{
    public class Unit
    {
        public Unit()
        {
            Sizes = new HashSet<Size>();
            Images = new HashSet<Image>();
            
        }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string Collection { get; set; }
        public int Price { get; set; }
        public string Material { get; set; }
        public string Sex { get; set; }
        public string Description { get; set; }
        public int KindId { get; set; }

        [ForeignKey(nameof(KindId))]
        [InverseProperty("Units")]
        public virtual Kind Kind { get; set; }
        public string Img { get; set; }


        [InverseProperty(nameof(Size.Unit))]
        public virtual ICollection<Size> Sizes { get; set; }

        [InverseProperty(nameof(Image.Unit))]
        public virtual ICollection<Image> Images { get; set; }

        

    }
}
