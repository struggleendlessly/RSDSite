using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using web.Data;
using web.Interfaces;

namespace web.Managers
{
    public class SiteNameSetter : ISiteNameSetter
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly StateManager _stateManager;

        public SiteNameSetter(UserManager<ApplicationUser> userManager, StateManager stateManager)
        {
            _userManager = userManager;
            _stateManager = stateManager;
        }

        public async  Task SetUserSiteName(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            _stateManager.SiteName = user.SiteName;
        }
    }
}
