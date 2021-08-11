namespace ForumSystem.Web.Areas.Administration.Controllers
{
    using ForumSystem.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static ForumSystem.Common.GlobalConstants;

    [Area("Administration")]
    [Authorize(Roles = AdministratorRoleName)]
    public class AdministrationController : BaseController
    {
    }
}
