using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Uno.Models.Identity
{
    public class ApplicationUserModDTO
    {
        public string Id { get; set; }
      
        public string Nome { get; set; }
        public string Cognome { get; set; }
       
    }
}
