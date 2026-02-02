using IdentityService.Infrastructure.Entity;
using IdentityService.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Duende.IdentityServer.Services;
using EventBus.Abstractions;
using Contracts.IntegrationEvents;

namespace IdentityService.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    /* NEW v2: Add IIdentityServerInteractionService for Duende logout */
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IEventPublisher _eventPublisher;


    /* OLD v1: Constructor without IIdentityServerInteractionService
    public AccountController(
        UserManager<User> userManager,
        SignInManager<User> signInManager)
        //IEventPublisher eventPublisher)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        //_eventPublisher = eventPublisher;
    }
    */

    /* NEW v2: Constructor with IIdentityServerInteractionService */
    public AccountController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IIdentityServerInteractionService interaction,
        IEventPublisher eventPublisher)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _interaction = interaction;
        _eventPublisher = eventPublisher;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string returnUrl = null)
    {
        if(User.Identity.IsAuthenticated)
        {
            return RedirectToLocal(returnUrl);
        }

        Console.WriteLine($"Login GET called {returnUrl}");

        ViewData["ReturnUrl"] = returnUrl;

        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginModel model)
    {
        ViewData["ReturnUrl"] = model.ReturnUrl;

        Console.WriteLine($"Login POST called {model.ReturnUrl}");

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.FindByNameAsync(model.UserName);

        var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: false, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            return RedirectToLocal(model.ReturnUrl);
        }

        return View(model);
    }

    /* OLD v1: Simple logout without Duende IdentityServer interaction
    [HttpGet]
    public async Task<IActionResult> Logout(LogoutModel model)
    {
        //await _signInManager.SignOutAsync();
        //return RedirectToAction(nameof(HomeController.Index), "Home");

        await _signInManager.SignOutAsync();
        return Redirect("~/");
    }
    */

    /* OLD v2: Logout GET - Show logout confirmation page
    [HttpGet]
    public async Task<IActionResult> Logout(string? logoutId)
    {
        // Build a model so the logout page knows what to display
        var model = new LogoutModel { LogoutId = logoutId };

        if (User?.Identity?.IsAuthenticated != true)
        {
            // if the user is not authenticated, then just show logged out page
            return await Logout(model);
        }

        // Show the logout prompt. This prevents attacks where the user
        // is automatically signed out by another malicious web page.
        return View(model);
    }
    */

    /* NEW v3: Logout GET - Silent logout without confirmation UI */
    [HttpGet]
    public async Task<IActionResult> Logout(string? logoutId)
    {
        // Get the logout context
        var logout = await _interaction.GetLogoutContextAsync(logoutId);

        // Sign out from ASP.NET Identity immediately without showing UI
        if (User?.Identity?.IsAuthenticated == true)
        {
            await _signInManager.SignOutAsync();
        }

        // Determine the post logout redirect URI
        var postLogoutRedirectUri = logout?.PostLogoutRedirectUri;

        // If no post logout redirect URI, default to SPA
        if (string.IsNullOrEmpty(postLogoutRedirectUri))
        {
            postLogoutRedirectUri = "http://localhost:3000/";
        }

        // Redirect immediately to the post logout redirect URI
        return Redirect(postLogoutRedirectUri);
    }

    /* OLD v2: Logout POST - Perform actual logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout(LogoutModel model)
    {
        // Get the logout context
        var logout = await _interaction.GetLogoutContextAsync(model.LogoutId);

        // Sign out from ASP.NET Identity
        await _signInManager.SignOutAsync();

        // Determine the post logout redirect URI
        var postLogoutRedirectUri = logout?.PostLogoutRedirectUri;

        // If no post logout redirect URI, default to SPA
        if (string.IsNullOrEmpty(postLogoutRedirectUri))
        {
            postLogoutRedirectUri = "http://localhost:3000/";
        }

        // Return to the post logout redirect URI
        return Redirect(postLogoutRedirectUri);
    }
    */

    /* NEW v3: Logout POST - Keep for compatibility but redirect to GET */
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout(LogoutModel model)
    {
        // Redirect to GET method for silent logout
        return await Logout(model.LogoutId);
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        var user = await _userManager.FindByNameAsync(model.UserName);

        if (user != null)
        {
            return View("Success");
        }

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View();
        }

        await _eventPublisher.PublishAsync(new UserCreatedIntegrationEvent
        {
            UserId = user.Id,
            Email = user.Email
        });


        return View("Success");
    }

    public IActionResult Error()
    {
        return View();
    }
    public IActionResult AccessDenied()
    {
        return View();
    }

    private IActionResult RedirectToLocal(string returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }
        else
        {
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
