using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyBook1.Models
{
    public class CoverType
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Cover Type")]
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
