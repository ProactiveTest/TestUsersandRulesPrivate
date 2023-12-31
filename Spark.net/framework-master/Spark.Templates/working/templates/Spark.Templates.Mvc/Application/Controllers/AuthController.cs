﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spark.Templates.Mvc.Application.Services.Auth;
using Spark.Templates.Mvc.Application.Models;
using System.Globalization;
using System.Security.Claims;
using Spark.Templates.Mvc.Application.ViewModels;
using Spark.Templates.Mvc.Application.Events;
using Coravel.Events.Interfaces;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Spark.Templates.Mvc.Application.Controllers
{
    public class AuthController : Controller
    {
        private readonly RolesService _rolesService;
        private readonly UsersService _usersService;
        private readonly AuthService _authService;
        private readonly IConfiguration _configuration;
        private IDispatcher _dispatcher;

        public AuthController(UsersService usersService, RolesService rolesService, IConfiguration configuration, IDispatcher dispatcher, AuthService authService)
        {
            _usersService = usersService;
            _rolesService = rolesService;
            _configuration = configuration;
            _dispatcher = dispatcher;
            _authService = authService;
        }

        [HttpGet, AllowAnonymous]
        [Route("login")]
        public IActionResult Login()
        {
            Login model = new Login();
            return View(model);
        }

        [HttpPost, AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login(Login request)
        {
            if (!ModelState.IsValid)
                return View();

            if (request == null)
            {
                return BadRequest("user is not set.");
            }

            var user = await _usersService.FindUserAsync(request.Email, _usersService.GetSha256Hash(request.Password));

            if (user == null)
            {
                ModelState.AddModelError("FailedLogin", "Login Failed: Your email or password was incorrect");
                return View();
            }

            var loginCookieExpirationDays = _configuration.GetValue("Spark:Auth:CookieExpirationDays", 5);
            var cookieClaims = await _authService.CreateCookieClaims(user);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                cookieClaims,
                new AuthenticationProperties
                {
                    IsPersistent = request.RememberMe,
                    IssuedUtc = DateTimeOffset.UtcNow,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(loginCookieExpirationDays)
                });

            return RedirectToAction("Dashboard", "Home");
        }

        [HttpGet, AllowAnonymous]
        [Route("register")]
        public IActionResult Register()
        {
            Register model = new Register();
            return View(model);
        }

        [HttpPost, AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register(Register request)
        {
            if (!ModelState.IsValid)
                return View();

            if (request == null)
            {
                return BadRequest("user is not set.");
            }

            var existingUser = await _usersService.FindUserByEmailAsync(request.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError("EmailExists", "Email already in use by another account.");
                return View();
            }

            var userForm = new User()
            {
                Name = request.Name,
                Email = request.Email,
                Password = _usersService.GetSha256Hash(request.Password),
                CreatedAt = DateTime.UtcNow
            };

            var newUser = await _usersService.CreateUserAsync(userForm);

            // broadcast user created event
            var userCreated = new UserCreated(newUser);
            await _dispatcher.Broadcast(userCreated);

            var user = await _usersService.FindUserAsync(newUser.Email, newUser.Password);

            var loginCookieExpirationDays = _configuration.GetValue("Spark:Auth:CookieExpirationDays", 5);
            var cookieClaims = await _authService.CreateCookieClaims(user);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                cookieClaims,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    IssuedUtc = DateTimeOffset.UtcNow,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(loginCookieExpirationDays)
                });

			return RedirectToAction("Dashboard", "Home");
		}

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            if (HttpContext.User != null)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
