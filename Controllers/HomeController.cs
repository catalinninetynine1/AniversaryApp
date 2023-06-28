using AniversaryApp.Data;
using AniversaryApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AniversaryApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
     
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IConfiguration configuration)
        {
            _logger = logger;
           
            _configuration = configuration;
            _context = context;
        }
        public IActionResult LandPage()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult CreateUser()
        {
            return View();
        }
        //public IActionResult AdminTable()
        //{
        //    return View();
        //}
        
        [HttpPost]
        public IActionResult Login(User model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.User.FirstOrDefault(u => u.NameA == model.NameA && u.PasswordA == model.PasswordA);
                if(user !=null)
                {
                    // Authentication successful
                    if (user.UserTypeId == 1)
                    {
                        // Admin user logged in
                        return RedirectToAction("CreateUser");
                    }
                    else if (user.UserTypeId == 2)
                    {
                        // Normal user logged in
                        return RedirectToAction("NormalTable");
                    }
                    else
                    {
                        return View("Login");
                    }
                }
            }
            // Authentication failed or model is invalid
            ModelState.AddModelError(string.Empty, "Invalid username or password");
            return View(model);
        }

        public  IActionResult AdminTable()
        {
            //if (!User.Identity.IsAuthenticated)
            //{

            //}
            List<User> usersList = GetUserList();
            return View(usersList);
        }
        public List<User> GetUserList()
        {
            using(var dbContext = _context)
            {
                List<User> userList = dbContext.User.ToList();
                return userList;
            }
        }
        public IActionResult NormalTable()
        {
           
            var usersListNormalTable = GetUserList();
            return View(usersListNormalTable);
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public List<User> CreateUserMethod([FromBody] User model)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var user = new User
                        {
                            NameA = model.NameA,
                            UserNameA = model.UserNameA,
                            PasswordA = model.PasswordA,
                            Birthday = model.Birthday,
                            UserTypeId = model.UserTypeId,
                            UserEmailA = model.UserEmailA
                        };

                        _context.User.Add(user);
                        _context.SaveChanges();

                        transaction.Commit(); // Commit the transaction if no exceptions occurred
                        return new List<User>();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback(); // Rollback the transaction if an exception occurred
                        TempData["Message"] = "Failed to create user.";
                        throw; // Re-throw the exception to handle it at a higher level if needed
                    }
                }
            }
            return null;
        }
        [HttpPost]
        public IActionResult SendBirthdayNotifications()
        {
            // Get the list of all users from the database
            var users = _context.User.ToList();

            foreach (var user in users)
            {
                // Calculate the number of days until the user's birthday
                var daysUntilBirthday = (user.Birthday.Date - DateTime.Today).Days;

                if (daysUntilBirthday == 2)
                {
                    // Send notification or store it in a database to display on the UI
                    var notificationMessage = $"The user {user.NameA} birthday is in 2 days.";
                 
                }
            }

            return RedirectToAction("Home", "LandPage"); 
        }
        [HttpGet]
        public IActionResult ConfigureNotification()
        {
            // Get the list of all users from the database
            var users = _context.User.ToList();
            if(users != null)
            {
                // Pass the list of users to the view
                return View(users);
            }
            
           return RedirectToAction("Home", "LandPage");
        }
        [HttpPost]
        public IActionResult DeleteUser(string userId)
        {
           
            if (userId != null)
            {
               
                if (1==1)
                {
                    // User deletion successful
                    // Redirect or perform other actions
                    return RedirectToAction("AdminTable");
                }
                //else
                //{
                //    // User deletion failed
                //    // Handle errors
                //    ModelState.AddModelError(string.Empty, "User deletion failed.");
                //}
            }
            return RedirectToAction("AdminTable");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}