﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyBook.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "The Display Order must be between 1 and 100!")]
        public int DiplayOrder { get; set; }

        public DateTime CreaDateTime { get; set; } = DateTime.Now;


    }
}