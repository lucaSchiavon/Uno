using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Uno.Data;
using Uno.Entities;
using Uno.Models.Identity;

namespace Uno.Backoffice.Controllers
{
    [Area("Backoffice")]
    public class ApplicationUsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        //private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationUsersController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            //_userManager = userManager;
        }

        //// GET: ApplicationUsers
        public async Task<IActionResult> Index()
        {
            List<ApplicationUser> ApplicationUsersLst = await _context.Users.ToListAsync();  
            List<ApplicationUserDTO> ApplicationUserDTO = new List<ApplicationUserDTO>();
            ApplicationUserDTO = _mapper.Map<List<ApplicationUserDTO>>(ApplicationUsersLst);

             return View(ApplicationUserDTO);
        }

        //public IActionResult Index()
        //{
        //    //List<ApplicationUser> ApplicationUsersLst = await _context.Users.ToListAsync();
        //    List<ApplicationUser> ApplicationUsersLst =  _context.Users.ToList();
        //    List<ApplicationUserDTO> ApplicationUserDTO = new List<ApplicationUserDTO>();
        //    ApplicationUserDTO = _mapper.Map<List<ApplicationUserDTO>>(ApplicationUsersLst);

        //    // return View(ApplicationUserDTO);

        //    return View(ApplicationUserDTO);
        //}

        // GET: ApplicationUsers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
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
        public async Task<IActionResult> Create([FromServices]  UserManager<ApplicationUser> _UserManager, ApplicationUserInsDTO applicationUserInsDTO)
        {
            //-------------------------------
           
            if (ModelState.IsValid)
            {
                
                //crea l'utente
                var user = new ApplicationUser {
                    UserName = applicationUserInsDTO.Email,
                    Email = applicationUserInsDTO.Email,
                    EmailConfirmed =true,
                    Nome = applicationUserInsDTO.Nome,
                    Cognome = applicationUserInsDTO.Cognome };

                var result = await _UserManager.CreateAsync(user, applicationUserInsDTO.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                   
                    return View(applicationUserInsDTO);
                }


                return RedirectToAction(nameof(Index));
            }

            return View(applicationUserInsDTO);



            //-------------------------------
            //if (ModelState.IsValid)
            //{
            //    _context.Add(applicationUser);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(applicationUser);
        }

        // GET: ApplicationUsers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var applicationUser = await _context.Users.FindAsync(id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            ApplicationUserModDTO applicationUserModDTO = null;
            applicationUserModDTO = _mapper.Map<ApplicationUserModDTO>(applicationUser);

            return View(applicationUserModDTO);
        }

        // POST: Addresses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Nome,Cognome")] ApplicationUserModDTO applicationUserModDTO)
        {
            //if (id != applicationUser.Id)
            //{
            //    return NotFound();
            //}
            ApplicationUser ApplicationUser=null;
            if (ModelState.IsValid)
            {
                try
                {
                    //ApplicationUserDTO ApplicationUserDTO = null;

                   ApplicationUser = _context.Users.First(x => x.Id == id);
                    //ApplicationUserDTO = _mapper.Map<ApplicationUserDTO>(ApplicationUser);
                    if (ApplicationUser == null)
                    {
                        return NotFound();
                    }
                    ApplicationUser.Nome = applicationUserModDTO.Nome;
                    ApplicationUser.Cognome = applicationUserModDTO.Cognome;
                    _context.Update(ApplicationUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserExists(ApplicationUser.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(applicationUserModDTO);
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
            var applicationUser = await _context.Users.FindAsync(id);
            _context.Users.Remove(applicationUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationUserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
