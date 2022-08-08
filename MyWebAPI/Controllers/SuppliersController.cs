using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebAPI.Models;

namespace MyWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuppliersController : Controller
    {
        private DataContext _context;

        public SuppliersController(DataContext context)        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<Supplier> GetSupplier(long id)
        {
            Supplier supplier = await _context.Suppliers.Include(s => s.Products).FirstAsync(s => s.SupplierId == id);
            if(supplier.Products != null)
            {
                foreach(Product p in supplier.Products)
                {
                    p.Supplier = null;
                }
            }
            return supplier;
        }

        [HttpPatch("{id}")]
        public async Task<Supplier> PatchSupplier(long id, JsonPatchDocument<Supplier> patchDoc)
        {
            Supplier? supplier = await _context.Suppliers.FindAsync(id);
            if(supplier != null)
            {
                patchDoc.ApplyTo(supplier);
                await _context.SaveChangesAsync();
            }
            return supplier;
        }
 
    }
}
