using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PetsHeaven.DTO;
using PetsHeaven.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace PetsHeaven.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManger;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration config;

        public AccountController(UserManager<ApplicationUser> userManger, IConfiguration config, RoleManager<IdentityRole> roleManager)
        {
            this.userManger = userManger;
            this.config = config;
            this.roleManager = roleManager;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDTO regUserDTO)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();

                user.FirstName = regUserDTO.FirstName;
                user.LastName = regUserDTO.LastName;
                user.UserName = regUserDTO.UserName;
                user.Email = regUserDTO.Email;

                if (await userManger.FindByEmailAsync(user.Email) is not null)
                {
                    ModelState.AddModelError("emailError", "Email already exists");
                    return BadRequest(ModelState);
                }
                if (await userManger.FindByNameAsync(user.UserName) is not null)
                {
                    ModelState.AddModelError("usernameError", "Username already exists");
                    return BadRequest(ModelState);
                }


                IdentityResult result = await userManger.CreateAsync(user, regUserDTO.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return BadRequest(ModelState);
                }

                // Add role to the user
                if (regUserDTO.isAdmin)
                {
                    //await roleManager.RoleExistsAsync(UserRoles.Admin);
                    if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                        await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                    await userManger.AddToRoleAsync(user, UserRoles.Admin);
                }
                else
                {
                    if (!await roleManager.RoleExistsAsync(UserRoles.User))
                        await roleManager.CreateAsync(new IdentityRole(UserRoles.User));
                    await userManger.AddToRoleAsync(user, UserRoles.User);
                }

                LoginUserDTO logUserDTO = new LoginUserDTO();
                logUserDTO.Email = regUserDTO.Email;
                logUserDTO.Password = regUserDTO.Password;
                return await Login(logUserDTO);
                //return Ok("Account Created Successfully");
            }
            return BadRequest(ModelState);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDTO logUserDto)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await userManger.FindByEmailAsync(logUserDto.Email);
                if (user is not null)
                {
                    bool rightPw = await userManger.CheckPasswordAsync(user, logUserDto.Password);
                    if (rightPw)
                    {
                        var userClaims = new List<Claim>();
                        userClaims.Add(new Claim("userID", user.Id));
                        userClaims.Add(new Claim("userEmail", user.Email));
                        userClaims.Add(new Claim("userName", user.UserName));
                        userClaims.Add(new Claim("userFullName", user.FirstName + ' ' + user.LastName));
                        userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                        var role = await userManger.GetRolesAsync(user);
                        foreach (var r in role)
                        {
                            userClaims.Add(new Claim(ClaimTypes.Role, r));
                        }
                        if (role.Contains(UserRoles.Admin))
                        {
                            userClaims.Add(new Claim("userRole", "Admin"));
                        }
                        else
                        {
                            userClaims.Add(new Claim("userRole", "User"));
                        }

                        SecurityKey securityKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Secret"]));


                        SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                        JwtSecurityToken petsToken = new JwtSecurityToken(
                            issuer: config["JWT:ValidIssuer"],//url web api
                            audience: config["JWT:ValidAudiance"],//url consumer angular
                            claims: userClaims,
                            expires: DateTime.Now.AddDays(15),
                            signingCredentials: credentials
                        );

                        // Save the token to the AspNetUserTokens table
                        await userManger.SetAuthenticationTokenAsync(await userManger.FindByEmailAsync(logUserDto.Email), "JWT", "AccessToken", new JwtSecurityTokenHandler().WriteToken(petsToken));
                        return Ok(
                        new
                        {
                            Message = "Logged in successfully",
                            StatusCode = StatusCodes.Status200OK,
                            token = new JwtSecurityTokenHandler().WriteToken(petsToken),
                            validTo = petsToken.ValidTo
                        });
                    }
                    return Unauthorized();
                }
                return Unauthorized();
            }
            return Unauthorized();
        }

        [HttpPost("logout/{userId}")]
        [Authorize] // Requires authentication
        public async Task<IActionResult> Logout(string userId)
        {
            var user = await userManger.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var result = await userManger.RemoveAuthenticationTokenAsync(user, "JWT", "AccessToken");
            if (result.Succeeded)
            {

                await userManger.UpdateSecurityStampAsync(user);
                return Ok("Logged out successfully");
            }

            return BadRequest("Failed to logout");
        }

    }
}
