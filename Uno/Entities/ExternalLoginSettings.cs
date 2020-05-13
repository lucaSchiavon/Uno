using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Uno.Entities
{
    public class ExternalLoginSettings
    {
        public string FB_AppId { get; set; }
        public string FB_AppSecret { get; set; }
        public string Google_ClientId { get; set; }
        public string Google_ClientSecret { get; set; }
    }
}
