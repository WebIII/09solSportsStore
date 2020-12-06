using SportsStore.Models.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace SportsStore.Models.ProductViewModels {
    public class EditViewModel {
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(100, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 5)]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [Range(1, 3000, ErrorMessage = "{0} must be in the range {1} - {2}")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [Display(Name = "In stock")]
        public bool InStock { get; set; }
        [Display(Name = "Category")]
        [Required(ErrorMessage = "{0} is required")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public Availability Availability { get; set; }
        [Display(Name = "Available till")]
        [DataType(DataType.Date)]
        public DateTime? AvailableTill { get; set; }

        public EditViewModel() {
            InStock = true;
        }

        public EditViewModel(Product product) {
            Name = product.Name;
            Description = product.Description;
            Price = product.Price;
            InStock = product.InStock;
            CategoryId = product.Category?.CategoryId ?? 0;
            Availability = product.Availability;
            AvailableTill = product.AvailableTill;
        }
    }
}
