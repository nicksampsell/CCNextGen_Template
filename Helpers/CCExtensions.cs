using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text;
using System.Web;

namespace CCNextGen_Template.Helpers
{
	public static class CCExtensions
	{
		public static string? ActiveClass(this IHtmlHelper htmlHelper, string? area = null, string? controller = null, string? action = null, string? page = null, string? cssClass = "active", string? hiddenClass = "")
		{
			var currentArea = htmlHelper?.ViewContext.RouteData.Values["area"] as string;
			var currentController = htmlHelper?.ViewContext.RouteData.Values["controller"] as string;
			var currentAction = htmlHelper?.ViewContext.RouteData.Values["action"] as string;
			var currentPage = htmlHelper?.ViewContext.RouteData.Values["page"] as string;

			var acceptedAreas = (!String.IsNullOrEmpty(area)) ? area.Split(',') : null;
			var acceptedControllers = (controller ?? currentController ?? "").Split(',');
			var acceptedActions = (action ?? currentAction ?? "").Split(',');
			var acceptedPages = (page ?? currentPage ?? "").Split(',');

			if(currentPage != null && string.IsNullOrEmpty(controller))
			{
                bool isPageMatch = false;

                foreach (var acceptedPage in acceptedPages)
                {
                    if (acceptedPage.EndsWith("/*"))
                    {
                        var folderPath = acceptedPage.Substring(0, acceptedPage.Length - 2);
                        if (currentPage.StartsWith(folderPath, StringComparison.OrdinalIgnoreCase))
                        {
                            isPageMatch = true;
                            break;
                        }
                    }
                    else if (string.Equals(acceptedPage, currentPage, StringComparison.OrdinalIgnoreCase))
                    {
                        isPageMatch = true;
                        break;
                    }
                }

                return (acceptedAreas != null ? acceptedAreas.Contains(currentArea) : true) &&
					acceptedPages.Contains(currentPage) || isPageMatch ? $"{cssClass} as-page" : hiddenClass;
            }

            return (acceptedAreas != null ? acceptedAreas.Contains(currentArea) : true) && 
					acceptedControllers.Contains(currentController) && 
					acceptedActions.Contains(currentAction) ? $"{cssClass} as-controller" : hiddenClass;
		}

        [Obsolete("ActiveExpandContract is deprecated.  Use ActiveClass() with hiddenClass parameter instead.", true)]
        public static string? ActiveExpandContract(this IHtmlHelper htmlHelper, string? area = null, string? controller = null, string? action = null, string? page = null, string? hiddenClass = "hidden", string? visibleClass = "")
        {
            var currentArea = htmlHelper?.ViewContext.RouteData.Values["area"] as string;
            var currentController = htmlHelper?.ViewContext.RouteData.Values["controller"] as string;
            var currentAction = htmlHelper?.ViewContext.RouteData.Values["action"] as string;
            var currentPage = htmlHelper?.ViewContext.RouteData.Values["page"] as string;

            var acceptedAreas = (!String.IsNullOrEmpty(area)) ? area.Split(',') : null;
            var acceptedControllers = (controller ?? currentController ?? "").Split(',');
            var acceptedActions = (action ?? currentAction ?? "").Split(',');
            var acceptedPages = (page ?? currentPage ?? "").Split(',');

            if (currentPage != null)
            {
                bool isPageMatch = false;

                foreach (var acceptedPage in acceptedPages)
                {
                    if (acceptedPage.EndsWith("/*"))
                    {
                        var folderPath = acceptedPage.Substring(0, acceptedPage.Length - 2);
                        if (currentPage.StartsWith(folderPath, StringComparison.OrdinalIgnoreCase))
                        {
                            isPageMatch = true;
                            break;
                        }
                    }
                    else if (string.Equals(acceptedPage, currentPage, StringComparison.OrdinalIgnoreCase))
                    {
                        isPageMatch = true;
                        break;
                    }
                }

                return (acceptedAreas != null ? acceptedAreas.Contains(currentArea) : true) &&
                    acceptedPages.Contains(currentPage) || isPageMatch ? visibleClass : hiddenClass;
            }

            return (acceptedAreas != null ? acceptedAreas.Contains(currentArea) : true) &&
                    acceptedControllers.Contains(currentController) &&
                    acceptedActions.Contains(currentAction) ? visibleClass : hiddenClass;
        }


		public static Type GetModelType<T>(this IHtmlHelper<T> html) => typeof(T);

	}
}