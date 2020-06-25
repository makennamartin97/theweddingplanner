using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using weddingplanner.Models;

namespace weddingplanner.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context { get; set; }
        private PasswordHasher<User> regHasher = new PasswordHasher<User> ();
        private PasswordHasher<LoginUser> logHasher = new PasswordHasher<LoginUser> ();
        public  User GetUser()
        {
            return _context.users.FirstOrDefault( u =>  u.userID == HttpContext.Session.GetInt32("userID"));
        }


        public HomeController(MyContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {

            return View("Index");
        }
        [HttpPost("register")]

        public IActionResult Register(User newuser)
        {
            if(ModelState.IsValid)
            {
                if(_context.users.FirstOrDefault(u => u.email == newuser.email) != null)
                {
                  // Manually add a ModelState error to the Email field, with provided
                  // error message
                  ModelState.AddModelError("Email", "Email already in use!");
                  // You may consider returning to the View at this point
                  return View("Index");
                }
              string hash = regHasher.HashPassword (newuser, newuser.password);
              newuser.password = hash;
              _context.users.Add(newuser);
              _context.SaveChanges();
              HttpContext.Session.SetInt32("userID", newuser.userID);




                return RedirectToAction("Dashboard");
            }
            return View("Index");
        }
        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            User current = GetUser();
            if(current == null)
            {
                return Redirect("/");
            }
            ViewBag.User = current;
            List<Wedding> allweddings = _context.weddings
                .Include(m => m.planner)
                .Include(m => m.attendees)
                .ThenInclude(wp => wp.attendee)
                .Where(m => m.date >= DateTime.Now)
                .OrderBy(m => m.date)
                .ToList();
            return View("dashboard", allweddings);
        }
        [HttpPost("login")]
        public IActionResult Login(LoginUser user)
        {
            if(ModelState.IsValid)
            {
                User userindb = _context.users.FirstOrDefault(u => u.email == user.loginemail );
                if(userindb == null)
                {
                    ModelState.AddModelError("loginemail", "Invalid email or password!");
                    return View("Index");
                }
                var result = logHasher.VerifyHashedPassword (user, userindb.password, user.loginpassword);
                if(result == 0)
                {
                    ModelState.AddModelError ("loginpassword", "Invalid email or password!");
                    return View("Index");
                }
                HttpContext.Session.SetInt32("userID", userindb.userID);
                return Redirect("/dashboard");
            }
            return View("Index");
        }
        [HttpGet("logout")]
        public IActionResult Logout()
        {
          HttpContext.Session.Clear();
          return RedirectToAction("Index");
        }
        [HttpGet("newwedding")]
        public IActionResult NewWeddingPage()
        {
            User current = GetUser();
            if (current == null)
            {
                return Redirect ("/");
            }

            return View("newwedding");
        }
        [HttpGet("weddings/{weddingID}")]
        public IActionResult SeeDetails(int weddingID)
        {
            User current =GetUser();
            if(current == null)
            {
                return RedirectToAction("Index");
            }
            Wedding thewedding = _context.weddings
                .Include(w => w.attendees)
                .ThenInclude(a => a.attendee)
                .Include(w =>w.planner)
                .FirstOrDefault(w => w.weddingID == weddingID);
            ViewBag.User = current;

            return View("weddingdetails", thewedding);
        }
        [HttpPost("createwedding")]
        public IActionResult CreateWedding(Wedding newwedding)
        {
            User current = GetUser();
            if (current == null)
            {
                return Redirect ("/");
            }
            if(ModelState.IsValid)
            {
                newwedding.userID = current.userID;
                _context.weddings.Add(newwedding);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");


            }
            return View("newwedding");
        }
        [HttpGet("/weddings/{weddingID}/leave")]
        public IActionResult unRSVP(int weddingID)
        {
            User current = GetUser();
            if (current == null)
            {
                return RedirectToAction("Index");
            }
            Association unrsvp = _context.associations
                .FirstOrDefault(a => a.weddingID == weddingID && a.userID == current.userID);
            _context.associations.Remove(unrsvp);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        [HttpGet("/weddings/{weddingID}/rsvp")]
        public IActionResult RSVP(int weddingID)
        {
            User current = GetUser();
            if (current == null)
            {
                return RedirectToAction("Index");
            }
            Association rsvp = new Association();
            rsvp.weddingID = weddingID;
            rsvp.userID = current.userID;
            _context.Add(rsvp);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        [HttpGet("/weddings/{weddingID}/delete")]
        public IActionResult delete(int weddingID)
        {
            User current = GetUser();
            if (current == null)
            {
                return RedirectToAction("Index");
            }
            Wedding todelete = _context.weddings
                .FirstOrDefault(w => w.weddingID == weddingID);
                _context.weddings.Remove(todelete);
                _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
    

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
