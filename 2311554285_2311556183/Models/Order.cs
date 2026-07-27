using System;
using System.Collections.Generic;

namespace _2311554285_2311556183.Models;

public partial class Order
{
    public int Id { get; set; }

    public string CustomerName { get; set; } = null!;

    public DateTime? OrderDate { get; set; }

    public decimal? TotalAmount { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
