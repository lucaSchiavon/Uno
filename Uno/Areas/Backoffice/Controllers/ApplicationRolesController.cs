using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Localization;
using Uno.Areas.Backoffice.Models;
using Uno.Data;
using Uno.Entities;
using Uno.Models;
using Uno.Models.Identity;

namespace Uno.Backoffice.Controllers
{
    [Area("Backoffice")]
    public class ApplicationRolesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ApplicationRolesController> _localizer;
        private const int DefaultPageSize = 1;
        private List<ApplicationRoleDTO> allRolesDTO = new List<ApplicationRoleDTO>();

        //private const int DefaultPageSize = 10;
        //private List<Product> allProducts = new List<Product>();
        //private readonly string[] allCategories = new string[3] { "Shoes", "Electronics", "Food" };


        //private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationRolesController(ApplicationDbContext context, IMapper mapper, IStringLocalizer<ApplicationRolesController> localizer)
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;

            InitializeProducts();
        }
        //private void InitializeProducts()
        //{
        //    // Create a list of products. 50% of them are in the Shoes category, 25% in the Electronics 
        //    // category and 25% in the Food category.
        //    for (var i = 0; i < 527; i++)
        //    {
        //        var product = new Product();
        //        product.Name = "Product " + (i + 1);
        //        var categoryIndex = i % 4;
        //        if (categoryIndex > 2)
        //        {
        //            categoryIndex = categoryIndex - 3;
        //        }
        //        product.Category = allCategories[categoryIndex];
        //        allProducts.Add(product);
        //    }
        //}
        private void InitializeProducts()
        {

            List<IdentityRole> IdentityRolesLst = _context.Roles.ToList();
            List<ApplicationRoleDTO> ApplicationRoleDTOLst = new List<ApplicationRoleDTO>();
            ApplicationRoleDTOLst = _mapper.Map<List<ApplicationRoleDTO>>(IdentityRolesLst);
            allRolesDTO = ApplicationRoleDTOLst;
        }

        // GET: ApplicationUsers
        //public async Task<IActionResult> Index()
        //{
        //    List<IdentityRole> IdentityRolesLst = await _context.Roles.ToListAsync();
        //    List<ApplicationRoleDTO> ApplicationRoleDTOLst = new List<ApplicationRoleDTO>();
        //    ApplicationRoleDTOLst = _mapper.Map<List<ApplicationRoleDTO>>(IdentityRolesLst);

        //    return View(ApplicationRoleDTOLst);
        //}


        public IActionResult Index(int p = 1)
        {

            var currentPageNum = p;
            var offset = (DefaultPageSize * currentPageNum) - DefaultPageSize;
            var model = new RoleListViewModel();
            model.Roles.Data = this.allRolesDTO
                .Skip(offset)
                .Take(DefaultPageSize)
                .ToList();

            model.Roles.PageNumber = currentPageNum;
            model.Roles.PageSize = DefaultPageSize;
            model.Roles.TotalItems = allRolesDTO.Count;


            return View(model);


        }
        //public IActionResult Index(int p = 1)
        //{

        //    var currentPageNum = p;
        //    var offset = (DefaultPageSize * currentPageNum) - DefaultPageSize;
        //    var model = new ProductListViewModel();
        //    model.Products.Data = this.allProducts
        //        .Skip(offset)
        //        .Take(DefaultPageSize)
        //        .ToList();

        //    model.Products.PageNumber = currentPageNum;
        //    model.Products.PageSize = DefaultPageSize;
        //    model.Products.TotalItems = allProducts.Count;


        //    return View(model);


        //}
        // GET: ApplicationUsers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roles = await _context.Roles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (roles == null)
            {
                return NotFound();
            }

            return View(roles);
        }

        // GET: ApplicationUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ApplicationUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationRoleDTO applicationRoleDTO)
        {
            
            if (ModelState.IsValid)
            {

                var role = new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = applicationRoleDTO.Name
                };

                EntityEntry<IdentityRole> ApplicationRole = _context.Roles.Add(role);
                try
                {
                    _context.SaveChanges();
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, _localizer["Entity insert error, Retry."]);
                    return View(applicationRoleDTO);
                }

                //if (ApplicationRole == null)
                //{
                       
                   
                //}


                return RedirectToAction(nameof(Index));
            }

            return View(applicationRoleDTO);

        }

        // GET: ApplicationUsers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var applicationRole = await _context.Roles.FindAsync(id);
            if (applicationRole == null)
            {
                return NotFound();
            }

            ApplicationRoleDTO ApplicationRoleDTO = null;
            ApplicationRoleDTO = _mapper.Map<ApplicationRoleDTO>(applicationRole);

            return View(ApplicationRoleDTO);
        }

        // POST: Addresses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ApplicationRoleDTO applicationRoleDTO)
        {
            //if (id != applicationUser.Id)
            //{
            //    return NotFound();
            //}
            IdentityRole Role=null;
            if (ModelState.IsValid)
            {
                try
                {
                    //ApplicationUserDTO ApplicationUserDTO = null;

                    Role = _context.Roles.First(x => x.Id == id);
                    //ApplicationUserDTO = _mapper.Map<ApplicationUserDTO>(ApplicationUser);
                    if (Role == null)
                    {
                        return NotFound();
                    }
                    Role.Name = applicationRoleDTO.Name;
                   
                    _context.Update(Role);
                 
                    return View(applicationRoleDTO);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationRoleExists(Role.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, _localizer["Entity edit error, Retry."]);
                        return View(applicationRoleDTO);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(applicationRoleDTO);
        }

        //// GET: ApplicationUser/Delete/5
        //public async Task<IActionResult> Delete(string id)
        //{
        //    if (string.IsNullOrEmpty(id))
        //    {
        //        return NotFound();
        //    }

        //    var applicationUser = await _context.Users
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (applicationUser == null)
        //    {
        //        return NotFound();
        //    }

            
        //    ApplicationUserDTO ApplicationUserDTO = null;
        //    ApplicationUserDTO = _mapper.Map<ApplicationUserDTO>(applicationUser);

        //    return View(ApplicationUserDTO);
        //}

        // POST: ApplicationUser/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var Role = await _context.Roles.FindAsync(id);
            _context.Roles.Remove(Role);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationRoleExists(string id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }
    }
}
