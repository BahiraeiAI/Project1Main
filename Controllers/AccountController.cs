       
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace Project1.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _UserManager;
        private readonly SignInManager<User> _SigninManager;
        private readonly RoleManager<IdentityRole> _RoleManager;
        private readonly ApplicationDbContext _DbContext;
        public AccountController(UserManager<User> usermanager,SignInManager<User> signInmanager,RoleManager<IdentityRole> roleManager,ApplicationDbContext Context)
        {
            _UserManager = usermanager;
            _SigninManager = signInmanager;
            _RoleManager = roleManager;
            _DbContext = Context;
        }
        
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel LViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(LViewModel);
            }

            User? User = await _UserManager.FindByEmailAsync(LViewModel.Email);
            if (User is null)
            {
                ViewData["Error"] = "User doesn't exist";
                return View(LViewModel);
            }

            if(!await _UserManager.CheckPasswordAsync(User, LViewModel.Password))
            {
                ViewData["Error"] = "Wrong credentials";
                return View(LViewModel);
            }

            using (_DbContext.Database.BeginTransaction())
            {
                try
                {
                    var result = await _SigninManager.PasswordSignInAsync(User, LViewModel.Password, true, true);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "home");
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                catch
                {
                    _DbContext.Database.RollbackTransaction();
                    ViewData["Error"] = "Login Failed Try again";
                    return View(LViewModel);
                }
                
            }
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel RViewModel)
        {
            ViewBag.PasswordError = null;
            ViewData["Error"] = null;
            if (!ModelState.IsValid)
            {
                return View(RViewModel);
            }

            User? User = await _UserManager.FindByEmailAsync(RViewModel.Email);
            if (User is not null)
            {
                ViewData["Error"] = "The email is already in use, try using another email";
                return View(RViewModel);
            }

            using (_DbContext.Database.BeginTransaction())
            {
                try
                {
                    
                    User user = new User
                    {
                        Email = RViewModel.Email,
                        UserName = RViewModel.Email
                    };
                    IdentityResult Result = await _UserManager.CreateAsync(user, RViewModel.Password);
                    if (Result.Succeeded)
                    {
                        IdentityResult result = await _UserManager.AddToRoleAsync(user, UserRoles.User);
                        if (result.Succeeded)
                        {
                            Console.WriteLine($" {result.Succeeded}");

                        }
                        else 
                        {

                            Console.WriteLine("role asigning was not successful");
                            throw new Exception();
                        }
                    }
                    else
                    {
                        ViewBag.PasswordError = Result.Errors.Select(e => e.Description).ToList();
                        foreach (var item in Result.Errors.Select(e => e.Description).ToList())
                        {
                            Console.WriteLine(item);
                        }
                        Console.WriteLine("user Creation was not successful");
                        throw new Exception();
                    }

                    await _DbContext.SaveChangesAsync();
                    _DbContext.Database.CommitTransaction();
                    Console.WriteLine("transaction was successfull");
                    return RedirectToAction("RegisterationSuccess", "Account");
                }
                catch
                {
                    await _DbContext.Database.RollbackTransactionAsync();
                    Console.WriteLine("transaction was rolled back");
                    return View(RViewModel);
                }
            }

        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RegisterationSuccess()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles= "admin,user")]
        public IActionResult Logout()
        {
            _SigninManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
    }
}

