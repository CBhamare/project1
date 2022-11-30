using Microsoft.AspNetCore.Mvc;
using MoviesList.Models.DTO;
using MoviesList.Repositories.Abstract;

namespace MoviesList.Controllers
{
    public class UserAuthenticationController : Controller
    {
        private IUserAuthenticationService authService;
        public UserAuthenticationController(IUserAuthenticationService authService)
        {
            this.authService = authService;
        }
        /*public async Task<IActionResult> Register()
        {
            var model = new RegistrationModel
            {
                Email = "Admin@gmail.com",
                UserName = "Admin",
                Name = "Chetan",
                Password = "Admin@123",
                PasswordConfirm = "Admin@123",
                Role = "Admin"
            };
            var result = await authService.RegisterAsync(model);
            return Ok(result.Massage);
        }*/

        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await authService.LoginAsync(model);
            if(result.StatusCode==1)
            {
                return RedirectToAction("Index","Home");
            }
            else
            {
                TempData["msg"] = "Could Not Logged in...";
                return RedirectToAction(nameof(Login));
            }
            
        }

        public async Task<IActionResult> LogOut()
        {
            await authService.LogOutAsysnc();
            return RedirectToAction(nameof(Login));
        }

    }
}
