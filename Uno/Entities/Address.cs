using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Uno.Entities
{
    public class Address
    {
        public int AddressID { get; set; }
        public string RagioneSociale { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string Indirizzo { get; set; }
        public string CAP { get; set; }
        public string Comune { get; set; }
        public string SDI { get; set; }
        public string Pec { get; set; }
        public string Email { get; set; }
        public string PIVA { get; set; }
        public int ProvinciaID { get; set; }
        public int NazioneID { get; set; }
        public bool IsDeleted { get; set; }
        public int EntityID { get; set; }
        public int EntityTypeID { get; set; }
        public ApplicationUser User { get; set; }
    }
}
