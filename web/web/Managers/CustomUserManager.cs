using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using web.Data;

namespace web.Managers
{
    public class CustomUserManager : UserManager<ApplicationUser>
    {
        public CustomUserManager(IUserStore<ApplicationUser> store, IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<ApplicationUser> passwordHasher, IEnumerable<IUserValidator<ApplicationUser>> userValidators,
            IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<ApplicationUser>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        public override async Task<IdentityResult> CreateAsync(ApplicationUser user)
        {
            var existingUser = await Users.FirstOrDefaultAsync(u => u.SiteName == user.SiteName);
            if (existingUser != null)
            {
                var error = new IdentityError
                {
                    Code = "DuplicateSiteName",
                    Description = "The site name is already taken. Please choose a different one."
                };
                return IdentityResult.Failed(error);
            }

            return await base.CreateAsync(user);
        }
    }

}
