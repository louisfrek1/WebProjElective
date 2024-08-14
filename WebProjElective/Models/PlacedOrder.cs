using System.ComponentModel.DataAnnotations;

namespace WebProjElective.Models
{
    public class PlacedOrder
    {
        public int PlacedId { get; set; }
        [Required]
        public string UN { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total price must be greater than zero.")]
        public decimal TotalPrice { get; set; }

        [Required]
        public DateTime DatePlaced { get; set; }

        [Required]
        public string Shipping { get; set; }

        [Required]
        public string PayMethod { get; set; }
    }
}
