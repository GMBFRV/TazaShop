using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TazaShop.Models
{
    public class Kind
    {
        public Kind()
        {
            Units = new HashSet<Unit>();
        }
        [Key]
        public int Id { get; set; }
        public string Product { get; set; }
        public string Sex { get; set; }
        public string Elem { get; set; }

        [InverseProperty(nameof(Unit.Kind))]
        public virtual ICollection<Unit> Units { get; set; }
    }
}
