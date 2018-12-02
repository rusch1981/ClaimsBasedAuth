using System.Security.Claims;

namespace FormsAuthFFClient.Security
{
    public class DemoPrincipal : ClaimsPrincipal
    {

        public DemoPrincipal(DemoIdentity identity)
            : base(identity)
        {

        }

        public DemoPrincipal(ClaimsPrincipal claimsPrincipal)
            : base(claimsPrincipal)
        {

        }
    }
}