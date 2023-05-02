using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TazaShop.Models
{
    public class Customer
    {
        public Customer()
        {
            Carts = new HashSet<Cart>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string SecondName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public int TotalIncome { get; set; }

        [InverseProperty(nameof(Cart.Customer))]
        public virtual ICollection<Cart> Carts { get; set; }
    }
}
