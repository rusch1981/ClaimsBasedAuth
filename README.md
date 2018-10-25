# ClaimsBasedAuth
The below examples are from the turorials listed below.  For detailed info refer to Microsoft 
Documentation 
[here](https://docs.microsoft.com/en-us/dotnet/framework/security/)
## Authentication Project
Claims Based Security .Net4.5.  The majority of the code here is from 
[this](https://dotnetcodr.com/2013/02/14/introduction-to-claims-based-security-in-net4-5-with-c-part-2-the-new-inheritance-model/) 
tutorial.  The tutorial covers Claims, ClaimsIdentities, inhertance, Custom ClaimsAuthorizationManager and ClaimsPrincipalPermission Attribute, Custom ClaimsAuthenticationManager and much more.


## WebApplicationAuthentication Project
Claims Based Security .Net4.5.  The majority of the code here is from 
[this](https://dotnetcodr.com/2013/02/25/claims-based-authentication-in-mvc4-with-net4-5-c-part-1-claims-transformation/) 
tutorial.  The tutorial covers Custom ClaimsAuthenticationManager, User Retrieval in Razor, and Caching,

See the below classes for specific details: 
 * CustomClaimsTransformer.cs 
 * _LoginPartial.cshtml (User Retrieval in Razor)
 * Web.config and CustomClaimsTransformer (page post automagic)
 * AccountController and CustomClaimsTransformer (Caching user session)  stopped 
[here](https://dotnetcodr.com/2013/02/28/claims-based-authentication-in-mvc4-with-net4-5-c-part-2-storing-authentication-data-in-an-authentication-session/)
at AccountController



