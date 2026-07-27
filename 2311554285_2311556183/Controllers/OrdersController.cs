
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _2311554285_2311556183.Models;
using _2311554285_2311556183.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

public class OrdersController : Controller
{
    private readonly Employee23Bitv01Context _context;

    public OrdersController(Employee23Bitv01Context context)
    {
        _context = context;
    }

    // GET: ORDERS
    public async Task<IActionResult> Index(
    string customerName,
    decimal? minAmount,
    decimal? maxAmount)
    {
        var orders = _context.Orders
            .Include(o => o.OrderDetails)
            .AsQueryable();

        if (!string.IsNullOrEmpty(customerName))
        {
            orders = orders.Where(o =>
                o.CustomerName.Contains(customerName));
        }

        if (minAmount.HasValue)
        {
            orders = orders.Where(o =>
                o.TotalAmount >= minAmount);
        }

        if (maxAmount.HasValue)
        {
            orders = orders.Where(o =>
                o.TotalAmount <= maxAmount);
        }

        return View(await orders.ToListAsync());
    }

    // GET: ORDERS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var order = await _context.Orders
            .FirstOrDefaultAsync(m => m.Id == id);
        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }

    // GET: ORDERS/Create
    public IActionResult Create()
    {
        var vm = new CreateOrderVM();

        vm.Products = _context.Products
            .Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name + " (Stock: " + p.StockQuantity + ", Price: " + p.Price + ")"
            })
            .ToList();

        return View(vm);
    }

    // POST: ORDERS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateOrderVM vm)
    {
        if (!ModelState.IsValid)
        {
            vm.Products = _context.Products.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name + " (Stock: " + p.StockQuantity + ", Price: " + p.Price + ")"
            }).ToList();

            return View(vm);
        }

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var product = await _context.Products.FindAsync(vm.ProductId);

            if (product == null)
            {
                ModelState.AddModelError("", "Product not found.");
            }
            else if (product.StockQuantity < vm.Quantity)
            {
                ModelState.AddModelError("", "Not enough stock.");
            }
            else
            {
                Order order = new Order
                {
                    CustomerName = vm.CustomerName,
                    OrderDate = DateTime.Now,
                    TotalAmount = product.Price * vm.Quantity
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                OrderDetail detail = new OrderDetail
                {
                    OrderId = order.Id,
                    ProductId = product.Id,
                    Quantity = vm.Quantity,
                    UnitPrice = product.Price
                };

                _context.OrderDetails.Add(detail);

                product.StockQuantity -= vm.Quantity;

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return RedirectToAction(nameof(Index));
            }
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

        await transaction.RollbackAsync();

        vm.Products = _context.Products.Select(p => new SelectListItem
        {
            Value = p.Id.ToString(),
            Text = p.Name + " (Stock: " + p.StockQuantity + ", Price: " + p.Price + ")"
        }).ToList();

        return View(vm);
    }

    // GET: ORDERS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var order = await _context.Orders.FindAsync(id);
        if (order == null)
        {
            return NotFound();
        }
        return View(order);
    }

    // POST: ORDERS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,CustomerName,OrderDate,TotalAmount,OrderDetails")] Order order)
    {
        if (id != order.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(order);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(order.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(order);
    }

    // GET: ORDERS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var order = await _context.Orders
            .FirstOrDefaultAsync(m => m.Id == id);
        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }

    // POST: ORDERS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order != null)
        {
            _context.Orders.Remove(order);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Statistics()
    {
        var vm = new StatisticsVM();

        vm.TotalRevenue = await _context.Orders
            .SumAsync(o => o.TotalAmount ?? 0);

        vm.TopProducts = await _context.OrderDetails
            .GroupBy(od => od.Product.Name)
            .Select(g => new ProductStatisticVM
            {
                ProductName = g.Key,
                TotalSold = g.Sum(x => x.Quantity)
            })
            .OrderByDescending(x => x.TotalSold)
            .Take(3)
            .ToListAsync();

        return View(vm);
    }
    private bool OrderExists(int? id)
    {
        return _context.Orders.Any(e => e.Id == id);
    }
}
