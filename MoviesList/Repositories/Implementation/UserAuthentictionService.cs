using Microsoft.AspNetCore.Identity;
using MoviesList.Models.Domain;
using MoviesList.Models.DTO;
using MoviesList.Repositories.Abstract;
using System.Security.Claims;

namespace MoviesList.Repositories.Implementation
{
    public class UserAuthentictionService : IUserAuthenticationService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public UserAuthentictionService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;

        }

        public async Task<Status> RegisterAsync(RegistrationModel model)
        {
            var status = new Status();
            var userExists = await userManager.FindByNameAsync(model.UserName);
            if (userExists != null)
            {
                status.StatusCode = 0;
                status.Massage = "User already exist";
                return status;
            }
            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                Name = model.Name,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                status.StatusCode = 0;
                status.Massage = "User creation failed";
                return status;
            }

            if (!await roleManager.RoleExistsAsync(model.Role))
                await roleManager.CreateAsync(new IdentityRole(model.Role));


            if (await roleManager.RoleExistsAsync(model.Role))
            {
                await userManager.AddToRoleAsync(user, model.Role);
            }

            status.StatusCode = 1;
            status.Massage = "You have registered successfully";
            return status;
        }


        public async Task<Status> LoginAsync(LoginModel model)
        {
            var status = new Status();
            var user = await userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                status.StatusCode = 0;
                status.Massage = "Invalid username";
                return status;
            }

            if (!await userManager.CheckPasswordAsync(user, model.Password))
            {
                status.StatusCode = 0;
                status.Massage = "Invalid Password";
                return status;
            }

            var signInResult = await signInManager.PasswordSignInAsync(user, model.Password, true, true);
            if (signInResult.Succeeded)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                status.StatusCode = 1;
                status.Massage = "Logged in successfully";
            }
            else if (signInResult.IsLockedOut)
            {
                status.StatusCode = 0;
                status.Massage = "User is locked out";
            }
            else
            {
                status.StatusCode = 0;
                status.Massage = "Error on logging in";
            }

            return status;
        }

        public async Task LogOutAsysnc()
        {
            await signInManager.SignOutAsync();

        }

        //public async Task<Status> ChangePasswordAsync(ChangePasswordModel model, string username)
        //{
        //    var status = new Status();

        //    var user = await userManager.FindByNameAsync(username);
        //    if (user == null)
        //    {
        //        status.Message = "User does not exist";
        //        status.StatusCode = 0;
        //        return status;
        //    }
        //    var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
        //    if (result.Succeeded)
        //    {
        //        status.Message = "Password has updated successfully";
        //        status.StatusCode = 1;
        //    }
        //    else
        //    {
        //        status.Message = "Some error occcured";
        //        status.StatusCode = 0;
        //    }
        //    return status;

        //}
    }
}
