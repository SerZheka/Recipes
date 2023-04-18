using Microsoft.AspNetCore.Mvc;

namespace MyRecipes.Controllers
{
    [Route("Account")]
    public class AccountController : ControllerBase
    {
        [Route("Login")]
        public IActionResult Login(string returnUrl = null) => 
            Redirect($"/Identity/Account/Login?returnUrl={returnUrl}");
    }
}