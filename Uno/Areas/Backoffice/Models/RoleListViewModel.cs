using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uno.Models.Identity;

namespace Uno.Areas.Backoffice.Models
{
    public class RoleListViewModel
    {
        public RoleListViewModel()
        {
            Roles = new PagedResult<ApplicationRoleDTO>();
        }

        public string Query { get; set; } = string.Empty;

        public PagedResult<ApplicationRoleDTO> Roles { get; set; } = null;
    }
}
