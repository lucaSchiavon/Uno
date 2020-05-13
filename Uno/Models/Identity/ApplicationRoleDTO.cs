using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Uno.Models.Identity
{
    public class ApplicationRoleDTO
    {
        public string Id { get; set; } = string.Empty;
        //public int AddressID { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; } = string.Empty;

    }
}
