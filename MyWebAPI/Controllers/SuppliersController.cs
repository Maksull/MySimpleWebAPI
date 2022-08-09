using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebAPI.Models;

namespace MyWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class SuppliersController : Controller
    {
        private DataContext _context;

        public SuppliersController(DataContext context)        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSuppliers()
        {
            IEnumerable<Supplier>? suppliers =  _context.Suppliers.Include(s => s.Products);

            if(suppliers != null)
            {
                foreach(var s in suppliers)
                {
                    foreach(var p in s.Products)
                    {
                        p.Supplier = null;
                    }
                }
                return Ok(suppliers);
            }
            return NotFound();
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSupplier(long id)
        {
            Supplier? supplier = await _context.Suppliers.Include(s => s.Products).FirstOrDefaultAsync(s => s.SupplierId == id);
            if(supplier.Products != null)
            {
                foreach(Product p in supplier.Products)
                {
                    p.Supplier = null;
                }
                return Ok(supplier);
            }
            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SaveSupplier(SupplierBindingTarget supplierBinding)
        {
            if (ModelState.IsValid)
            {
                Supplier s = supplierBinding.ToSupplier();
                await _context.Suppliers.AddAsync(s);
                await _context.SaveChangesAsync();
                return Ok(s);
            }
            return BadRequest(ModelState);
        }


        [HttpPut]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateSupplier(Supplier supplier)
        {
            if (await _context.Suppliers.ContainsAsync(supplier))
            {
                _context.Suppliers.Update(supplier);
                await _context.SaveChangesAsync();
                return Ok(supplier);
            }
            return NotFound();
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSupplier(long id)
        {
            Supplier? s = await _context.Suppliers.FindAsync(id);

            if(s != null)
            {
                _context.Suppliers.Remove(s);
                await _context.SaveChangesAsync();
                return Ok(s);
            }
            return NotFound();
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
