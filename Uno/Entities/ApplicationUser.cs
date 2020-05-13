using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Uno.Entities
{
    public class ApplicationUser : IdentityUser
    {
        
        public int AddressID { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        //public string Password { get; set; }
        public string ImmagineProfilo { get; set; }
        public  ICollection<Address> Addresses { get; set; }
    }
}
