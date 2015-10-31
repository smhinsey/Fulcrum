using BrockAllen.MembershipReboot.Ef;
using SeedComponents.Membership.Entities;

namespace SeedComponents.Membership
{

    public class MembershipDbContext : MembershipRebootDbContext<ApplicationUser, ApplicationGroup>
    {
        public MembershipDbContext(string name)
            :base(name)
        {
        }
    }
}