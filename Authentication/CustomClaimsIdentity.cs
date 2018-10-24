using System.Security.Claims;

namespace Authentication
{
    public class CustomClaimsIdentity : ClaimsIdentity
    {
        public CustomClaimsIdentity()
        {
            AddClaim(new Claim(ClaimTypes.Name, "Adam"));
            AddClaim(new Claim(ClaimTypes.HomePhone, "555-6666"));

        }

        //AuthenticationType is necessary because AuthenicationType of CliamsIdentity is read only
        public override string AuthenticationType
        {
            get
            {
                return "Custom Type";
            }
        }

        //easy access not used above but good to know
        public string Phone
        {
            get
            {
                return FindFirst(ClaimTypes.HomePhone).Value;
            }
        }
    }
}
