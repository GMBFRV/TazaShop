using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using System.Security.Cryptography;
using System.Text;
using TazaShop.Models;

namespace TazaShop.Controllers
{
    public class UnitsController : Controller
    {
        private readonly ShopDbContext _context;

        public UnitsController(ShopDbContext context)
        {
            _context = context;
        }

        // GET: UnitsController
        public async Task<ActionResult> Index()
        {
            var shopDbContext = _context.Units.Include(c => c.Kind);
            return View(await shopDbContext.ToListAsync());
        }

        
        public async Task<ActionResult> Display(string id)
        {
            int SelectedKind = _context.Kinds.Where(c => c.Product == id).FirstOrDefault().Id;
            

            var units = _context.Units.Include(c => c.Kind).Where(c => c.KindId == SelectedKind);
            return View(await units.ToListAsync());
        }

        // GET: UnitsController/Details/5
        //public async Task<ActionResult> Details(int id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var unit1 = await _context.Units
        //        .Include(c => c.Kind)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (unit1 == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewBag.Images = _context.Images.Where(i => i.UnitId == id).ToList<Image>();
        //    return View(unit1);
        //}
        public async Task<ActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            dynamic mymodel = new ExpandoObject();
            
            mymodel.Sizes= _context.Sizes.Where(i => i.UnitId == id).ToList<Size>();
            mymodel.Units = await _context.Units
                .Include(c => c.Kind)
                .FirstOrDefaultAsync(m => m.Id == id);

            ViewBag.Images = _context.Images.Where(i => i.UnitId == id).ToList<Image>();
            return View(mymodel);
        }

        

        public IActionResult AddToCart(int? id)
        {
            int? userId = HttpContext.Session.GetInt32("LoggedUser");

            if (userId == null)
            {
                return RedirectToAction("Login");
            }
            
            if (id != null)
            {
                int Sid = _context.Sizes.Where(c => c.UnitId == (int)id).FirstOrDefault().Id;
                Cart cart = new Cart { SizeId = Sid, CustomerId = (int)userId, Qty = 1 };
                _context.Carts.Add(cart);
                _context.SaveChanges();
            }

            //var selected = _context.Carts.Where(c => c.CustomerId == (int)userId).ToList();
            //List<Unit> units = new List<Unit>();
            //foreach (Cart item in selected)
            //{
            //    var cfid = _context.Sizes.Where(c => c.Id == item.SizeId).FirstOrDefault();
            //    units.Add(_context.Units.Include(p => p.Kind).Where(c => c.Id == cfid.UnitId).FirstOrDefault());
            //}

            dynamic mymodel = new ExpandoObject();

            var selected = _context.Carts.Where(c => c.CustomerId == (int)userId).ToList();
            List<Unit> units = new List<Unit>();
            List<Size> sizes = new List<Size>();
            foreach (Cart item in selected)
            {
                var cfid = _context.Sizes.Where(c => c.Id == item.SizeId).FirstOrDefault();
                sizes.Add(cfid);
                units.Add(_context.Units.Include(p => p.Kind).Where(c => c.Id == cfid.UnitId).FirstOrDefault());
            }
            mymodel.Sizes=sizes;
            mymodel.Units=units;
            return View(mymodel);
        }

        public IActionResult ChangeCart(string unitId, string productSize, string qty)
        {
            int newUnitId = Int32.Parse(unitId);
            int newSize = Int32.Parse(productSize);
            int newQty = Int32.Parse(qty);
            int? id = HttpContext.Session.GetInt32("LoggedUser");

            var cart = _context.Carts.Where(c => c.CustomerId == (int)id &&
                c.SizeId == c.Size.Id).FirstOrDefault();
            cart.Qty = newQty;
            cart.SizeId = _context.Sizes.Where(c => c.ProductSize == newSize && c.UnitId == newUnitId).FirstOrDefault().Id;
            _context.SaveChanges();

            return RedirectToAction("Payment");
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(Customer customer)
        {
            Customer cust = new Customer { FirstName = customer.FirstName, SecondName=customer.SecondName, Email=customer.Email, Password=GetHash(customer.Password) };
            _context.Customers.Add(cust);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Customer customer)
        {
            if (customer == null)
            {
                return RedirectToAction("Login");
            }
            Customer fromDb = _context.Customers.Where(c => c.Email == customer.Email).FirstOrDefault();
            //string str = GetHash(fromDb.Password);
            if (fromDb.Password == GetHash(customer.Password))
            {
                //Add user Id into Session
                HttpContext.Session.SetInt32("LoggedUser", fromDb.Id);
            }
            else
            {
                return RedirectToAction("Login");
            }
            return RedirectToAction("Index");
        }



        [HttpGet]
        public IActionResult Payment()
        {
            int? id = HttpContext.Session.GetInt32("LoggedUser");
            //change Qty in units table
            //delete records from table carts
            //change field total for user with id
            var selected = _context.Carts.Where(c => c.CustomerId == (int)id).ToList();
            foreach (Cart item in selected)
            {
                Size size = _context.Sizes.Where(c => c.Id == item.SizeId).FirstOrDefault();
                size.Qty = size.Qty - item.Qty;

                Customer customer = _context.Customers.Where(c => c.Id == item.CustomerId).FirstOrDefault();
                customer.TotalIncome += _context.Units.Where(c => c.Id == size.UnitId).FirstOrDefault().Price;

                _context.SaveChanges();
            }
            var toDelete = _context.Carts.Where(c => c.CustomerId == id).ToList();
            _context.Carts.RemoveRange(toDelete);
            _context.SaveChanges();
            return View();
        }

        public string GetHash(string input)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

            return Convert.ToBase64String(hash);
        }
    }
}
