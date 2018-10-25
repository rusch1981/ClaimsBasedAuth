using System.Linq;
using System.Security.Claims;

namespace Authentication
{
    //In order for this work you need to register your custom claims Manager (Authorizer) in app.config. 
    //The basic idea is that actions and resources can be passed in via context
    // and used to specify the flow of authorization to complete those actions.  
    //Reference System.Security.Claims.dll
    //THIS CLASS IS NEVER REFERENCED DIRECTLY
    public class CustomAuthorizationManager : ClaimsAuthorizationManager
    {
        //we must overrid the CheckAccess method
        public override bool CheckAccess(AuthorizationContext context)
        {
            //In this example the following attribute was used 
            //  [ClaimsPrincipalPermission(SecurityAction.Demand, Operation = "Show", Resource = "Code")]
            //  If you break point into the context you will see Action and Resource collections.  They
            //  are claims and have a value of Show and Code consecutively.
            //This context will include information about the user who’s trying to access the method. 
            //  The ‘Principal’ property will describe the current User.
            string resource = context.Resource.First().Value;
            string action = context.Action.First().Value;

            if (action == "Show" && resource == "Code")
            {
                bool registered = context.Principal.HasClaim("Registered", "True");
                //  if a false is returned then a Security Exception will be thrown
                return registered;
            }

            if (action == "Show2" && resource == "Code")
            {
                bool registered = context.Principal.HasClaim("Registered", "True");
                //  if a false is returned then a Security Exception will be thrown
                return registered;
            }

            // if you hit this line a Security Exception will be thrown.  
            return false;
        }
    }
}
