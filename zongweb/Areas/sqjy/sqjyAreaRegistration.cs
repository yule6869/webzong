using System.Web.Mvc;

namespace zongweb.Areas.sqjy
{
    public class sqjyAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "sqjy";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "sqjy_default",
                "sqjy/{controller}/{action}/{id}",
                new { action = "nnjh", id = UrlParameter.Optional }
            );
        }
    }
}