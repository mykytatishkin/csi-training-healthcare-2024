﻿using CSI.IBTA.Consumer.Models;
using CSI.IBTA.Consumer.Interfaces;
using CSI.IBTA.Consumer.Extensions;
using CSI.IBTA.Consumer.Constants;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CSI.IBTA.Consumer.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthClient _client;

        public AuthController(IAuthClient client)
        {
            _client = client;
        }

        [HttpGet("/Login")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("/Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            var response = await _client.Authenticate(model.ToDto());
            if (!response.Result)
            {
                ModelState.AddModelError("", response.Error.Title);
                return View("Index");
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete(TokenConstants.JwtTokenCookieName);
            return RedirectToAction("Index", "Auth");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
