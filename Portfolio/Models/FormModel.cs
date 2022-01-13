using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.Models
{
    public class FormModel
    {
        [Key]
        [Required(ErrorMessage = "Your Email is required.")]
        [MaxLength(255)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [MaxLength(255)]
        [Required(ErrorMessage = "Your Name is required.")]
        public string Name { get; set; }

        [MaxLength(4000)]
        [Required(ErrorMessage = "A Message is required.")]
        public string Message { get; set; }

        public string Password { get; set; }
    }
}
