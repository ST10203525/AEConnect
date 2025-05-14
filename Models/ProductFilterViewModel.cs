using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AEConnect.Models // Replace with your actual namespace
{
    public class ProductFilterViewModel
    {
        // List of products after filtering
        public List<Product> Products { get; set; }

        // Filter options
        [Display(Name = "Farmer")]
        public int? SelectedFarmerId { get; set; }

        [Display(Name = "Category")]
        public string Category { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        // Dropdown sources
        public List<SelectListItem> Farmers { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
    }
}
