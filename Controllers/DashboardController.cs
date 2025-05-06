using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Project1.Controllers

{
    [Authorize(Roles = "user , admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _DbContext;
        public DashboardController(ApplicationDbContext DbContext)
        {
            _DbContext = DbContext;
        }
        // GET: /<controller>/
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            User user = await _DbContext.Users.FirstOrDefaultAsync(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View(user);
        }

        public IActionResult AddPost()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPost(PostViewModel PViewModel)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("model state wasn't valid");
                Console.WriteLine(" \n");
                if (!ModelState.IsValid)
                {
                    foreach (var entry in ModelState)
                    {
                        foreach (var error in entry.Value.Errors)
                        {
                            Console.WriteLine($"Key: {entry.Key}, Error: {error.ErrorMessage}");
                        }
                    }
                }
                return View(PViewModel);
            }

            using (var memoryStream = new MemoryStream())
            {
                await PViewModel.Image.CopyToAsync(memoryStream);

                Post post = new Post
                {
                    Text = PViewModel.Description,
                    Title = PViewModel.Title,
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    Image = new Image
                    {
                        Content = memoryStream.ToArray(),
                        ContentType = PViewModel.Image.ContentType,
                        ImageName = PViewModel.Image.FileName
                    }

                };

                _DbContext.Posts.Add(post);
                await _DbContext.SaveChangesAsync();
                Console.WriteLine("post saving was successful!");


                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetImage(int id)
        {

            Image image = await _DbContext.Images.FirstOrDefaultAsync(image => image.PostId == id);
            if (image == null)
            {
                return NotFound("ass hole there was no image to get");
            }

            return File(image.Content, image.ContentType);
        }


        [HttpGet]
        public async Task<IActionResult> MyPosts()
        {
            IEnumerable<Post> posts = await _DbContext.Posts.Where(p => p.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)).ToListAsync();
            return View(posts);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var ids = await _DbContext.Posts.Where(p => p.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)).Select(i => i.Id).ToListAsync();
            if (ids.Contains(id))
            {
                Post post = await _DbContext.Posts.FirstOrDefaultAsync(p => p.Id == id);

                return View(new PostViewModel
                {
                    Id = post.Id,
                    Title = post.Title,
                    Description = post.Text
                });
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PostViewModel postvm)
        {
            using (var memoryStream = new MemoryStream())
            {
                await postvm.Image.CopyToAsync(memoryStream);

                Post Updated = new Post
                {
                    Id = postvm.Id,
                    Text = postvm.Description,
                    Title = postvm.Title,
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                };
                Image Updatedimage = new Image
                {
                    Id = _DbContext.Images.AsNoTracking().FirstOrDefault(i => i.PostId == postvm.Id).Id,
                    Content = memoryStream.ToArray(),
                    ContentType = postvm.Image.ContentType,
                    ImageName = postvm.Image.FileName,
                    Post = Updated,

                };

                _DbContext.Update(Updated);
                _DbContext.Update(Updatedimage);
                await _DbContext.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Home");

        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {

            Post post = await _DbContext.Posts.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            Image image = await _DbContext.Images.AsNoTracking().FirstOrDefaultAsync(im => im.PostId == id);
            _DbContext.Images.Remove(image);
            _DbContext.Posts.Remove(post);
            await _DbContext.SaveChangesAsync();
            return RedirectToAction("MyPosts","Dashboard");

        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            User user = await _DbContext.Users.FirstOrDefaultAsync(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
            UserViewModel UserVM = new UserViewModel
            {
                Id= user.Id,
                Email = user.Email,
                UserName = user.UserName
            };
            return View(UserVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(UserViewModel UserVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            User user = await _DbContext.Users.FirstOrDefaultAsync(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
            user.Email = UserVM.Email;
            user.UserName = UserVM.UserName;
            _DbContext.Update<User>(user);
            _DbContext.SaveChanges();
            return RedirectToAction("Dashboard", "Dashboard");
        }
    }
}

