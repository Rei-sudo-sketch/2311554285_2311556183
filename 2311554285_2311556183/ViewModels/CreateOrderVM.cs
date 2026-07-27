using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace _2311554285_2311556183.ViewModels
{
    public class CreateOrderVM
    {
        [Required]
        public string CustomerName { get; set; } = "";

        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public List<SelectListItem>? Products { get; set; }
    }
}