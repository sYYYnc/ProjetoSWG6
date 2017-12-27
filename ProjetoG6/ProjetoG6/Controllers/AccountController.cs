using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProjetoG6.Models;
using ProjetoG6.Models.AccountViewModels;
using ProjetoG6.Services;
using ProjetoG6.Data;
using System.Net.Mail;
using MimeKit;

namespace ProjetoG6.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<AccountController> logger,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _context = context;
        }

        [TempData]
        public string ErrorMessage { get; set; }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWith2fa(bool rememberMe, string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            var model = new LoginWith2faViewModel { RememberMe = rememberMe };
            ViewData["ReturnUrl"] = returnUrl;

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWith2fa(LoginWith2faViewModel model, bool rememberMe, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, model.RememberMachine);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID {UserId} logged in with 2fa.", user.Id);
                return RedirectToLocal(returnUrl);
            }
            else if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                _logger.LogWarning("Invalid authenticator code entered for user with ID {UserId}.", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWithRecoveryCode(string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWithRecoveryCode(LoginWithRecoveryCodeViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            var recoveryCode = model.RecoveryCode.Replace(" ", string.Empty);

            var result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID {UserId} logged in with a recovery code.", user.Id);
                return RedirectToLocal(returnUrl);
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                _logger.LogWarning("Invalid recovery code entered for user with ID {UserId}", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid recovery code entered.");
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }


        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToAction(nameof(Login));
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
                return RedirectToLocal(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalLogin", new ExternalLoginViewModel { Email = email });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    throw new ApplicationException("Error loading external login information during confirmation.");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(nameof(ExternalLogin), model);
        }

        
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
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

        #endregion

        #region MetodosUsados

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            var queryEmail = from h in _context.Help where h.Campo == "Email" select h.Descricao;
            var queryPassword = from h in _context.Help where h.Campo == "Password" select h.Descricao;

            ViewData["HelpEmail"] = queryEmail.First();
            ViewData["HelpPassword"] = queryPassword.First();



            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {

            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var usr = _context.Candidatos.Where(us => us.Password == model.Password
                                                && us.Email == model.Email).FirstOrDefault();

                if (usr != null)
                {//é porque existe na bd
                 // Require the user to have a confirmed email before they can log on.
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user != null)//tem con
                    {
                        if (!await _userManager.IsEmailConfirmedAsync(user))
                        {
                            ViewBag.Message = "Verifique a conta antes de fazer login...";
                            ViewBag.Status = false;
                            return View(model);
                        }
                    }
                    // This doesn't count login failures towards account lockout
                    // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User logged in.");
                        return RedirectToLocal(returnUrl);
                    }
                    
                    if (result.IsLockedOut)
                    {
                        _logger.LogWarning("User account locked out.");
                        return RedirectToAction(nameof(Lockout));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return View(model);
                    }
                }
                else
                {
                    ViewBag.Message = "Utilizador ou Palavra-Chave invalido";
                    ViewBag.Status = false;
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            var queryEmail = from h in _context.Help where h.Campo == "Email" select h.Descricao;
            var queryPassword = from h in _context.Help where h.Campo == "Password" select h.Descricao;
            var queryPasswordConf = from h in _context.Help where h.Campo == "PasswordConf" select h.Descricao;
            var queryNome = from h in _context.Help where h.Campo == "Nome" select h.Descricao;
            var queryNumeroAluno = from h in _context.Help where h.Campo == "NumeroAluno" select h.Descricao;
            var queryDataNascimento = from h in _context.Help where h.Campo == "DataNascimento" select h.Descricao;

            ViewData["HelpEmail"] = queryEmail.First();
            ViewData["HelpPassword"] = queryPassword.First();
            ViewData["HelpPasswordConf"] = queryPasswordConf.First();
            ViewData["HelpNome"] = queryNome.First();
            ViewData["HelpNumeroAluno"] = queryNumeroAluno.First();
            ViewData["HelpDataNascimento"] = queryDataNascimento.First();
            

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Candidatos model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                if (isEmailExist(model.Email))
                {// ja existe
                    ViewBag.Message = "Já existe registo com este email";
                    ViewBag.Status = false;
                    return View(model);
                }
                //se ano escolhido for maior q ano atual
                if (model.DataNascimento.Year>=DateTime.Now.Year) {
                    ModelState.AddModelError("", string.Format(
                    "Ano Invalido..."));
                    return View();
                }
                //se nao tem 18 anos
                if ((DateTime.Now.Year-model.DataNascimento.Year)<=17) {
                    ModelState.AddModelError("", string.Format(
                   "Tem que ter pelo menos 18 anos..."));
                    return View();
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //criar novo candidato
                    Candidatos candidato = new Candidatos
                    {
                        Nome = model.Nome,
                        Email = model.Email,
                        DataNascimento = model.DataNascimento,
                        NumeroAluno = model.NumeroAluno,
                        Password = model.Password,
                        PasswordConfirmacao = model.PasswordConfirmacao
                    };
                    _context.Add(candidato);
                    await _context.SaveChangesAsync();
                    //enviar email
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                    await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);
                    ViewBag.Message = " Registo efetuado com sucesso. Foi enviado para " + model.Email +
                                      " um email de confirmação.";
                    ViewBag.Status = true;
                    return View(model);
                }
                AddErrors(result);
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            var queryEmail = from h in _context.Help where h.Campo == "Email" select h.Descricao;
            
            ViewData["HelpEmail"] = queryEmail.First();
            
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!isEmailExist(model.Email))
                {// ja existe
                    ViewBag.Message = "Este email não esta registado";
                    ViewBag.Status = false;
                    return View(model);
                }
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    ViewBag.Message = "Confirme o email antes de solicitar a mudança de Palavra-Chave";
                    return View();
                    // Don't reveal that the user does not exist or is not confirmed
                    //return RedirectToAction(nameof(ForgotPasswordConfirmation));
                }
                //eviar email 

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);
                await _emailSender.SendEmailAsync(model.Email, "Mudar Palavra-Chave",
                  $"Olá, " +user.UserName +
                  $"<br/>Houve uma tentativa de mudar a Palavra - Chave, caso não solicitou esta atividade" +
                  $" ignore este email.Caso contrario carregue no link <a href='{callbackUrl}'> Mudar Palavra-Chave </a>");       
                //return RedirectToAction(nameof(ForgotPasswordConfirmation));
                ViewBag.Message = "Por favor verifique o seu email " + model.Email + " para mudar a passowrd.";
                ViewBag.Status = true;
                return View();
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {

            var queryEmail = from h in _context.Help where h.Campo == "Email" select h.Descricao;
            var queryPassword = from h in _context.Help where h.Campo == "Password" select h.Descricao;
            var queryPasswordConf = from h in _context.Help where h.Campo == "PasswordConf" select h.Descricao;
           

            ViewData["HelpEmail"] = queryEmail.First();
            ViewData["HelpPassword"] = queryPassword.First();
            ViewData["HelpPasswordConf"] = queryPasswordConf.First();
            

            if (code == null)
            {
                throw new ApplicationException("A code must be supplied for password reset.");
            }
            var model = new ResetPasswordViewModel { Code = code };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                var usr = _context.Candidatos.Where(us => user.Email == model.Email).FirstOrDefault();
                usr.Password = model.Password;
                usr.PasswordConfirmacao = model.ConfirmPassword;
                _context.Update(usr);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            AddErrors(result);
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded) {
                ViewBag.Message = "A sua conta foi ativada com succeso. Carregue aqui ";
                ViewBag.Status = true;
                return View();
            }

            return View(result.Succeeded ? "" : "Error");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            ViewBag.Message = "Alterou a Palavra-Chave com sucesso. ";
            ViewBag.Status = true;
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Help()
        {
            var help = from h in _context.Help where h.Campo.Contains("High") select h;
            help.First();

            List<Help> Helpers = help.ToList();
            

            return View(help);
        }

        #region Metodos NonAction
        [NonAction]
        public bool isEmailExist(string email)
        {
            if (_context.Candidatos == null)
            {
                return false;
            }
            var usr = _context.Candidatos.Where(us =>
                        us.Email == email).FirstOrDefault();
            return usr != null;
        }

        #endregion

        #endregion

    }
}
