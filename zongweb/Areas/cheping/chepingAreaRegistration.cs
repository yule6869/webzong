using System.Web.Mvc;

namespace webzong2.Areas.cheping
{
    public class chepingAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "cheping";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "cheping_default",
                "cheping/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}