using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;
using Talabat.Dtos;
using Talabat.Errors;

namespace Talabat.Controllers
{
    public class AccountsController : APIBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AccountsController(UserManager<AppUser> userManager 
            , SignInManager<AppUser> signInManager
            , ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        //Register

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Regester(RegisterDto model)
        {
            var User = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
                PhoneNumber = model.PhoneNumber
            };
            var Result = await _userManager.CreateAsync(User, model.Password);
            if (!Result.Succeeded) return BadRequest(new ApiResponse(400));

            var ReturnedUser = new UserDto()
            {
                DisplayName = User.DisplayName,
                Email = User.Email,
                Token = await _tokenService.CreateTokenAsync(User, _userManager)
            };

            return Ok(ReturnedUser);
        }

        //Login

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login (LoginDto model)
        {
            var User = await _userManager.FindByEmailAsync(model.Email);
            if (User == null) return Unauthorized(new ApiResponse(401));

            var Result = await _signInManager.CheckPasswordSignInAsync(User, model.Password, false);

            if (!Result.Succeeded) return Unauthorized(new ApiResponse(401));

            return Ok(new UserDto()
            {
                DisplayName = User.DisplayName,
                Email = User.Email,
                Token = await _tokenService.CreateTokenAsync(User, _userManager)
            });
        }
    }
}
