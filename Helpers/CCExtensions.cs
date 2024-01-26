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
		public static string? ActiveClass(this IHtmlHelper htmlHelper, string? area = null, string? controller = null, string? action = null, string? cssClass = "active")
		{
			var currentArea = htmlHelper?.ViewContext.RouteData.Values["area"] as string;
			var currentController = htmlHelper?.ViewContext.RouteData.Values["controller"] as string;
			var currentAction = htmlHelper?.ViewContext.RouteData.Values["action"] as string;

			var acceptedAreas = (!String.IsNullOrEmpty(area)) ? area.Split(',') : null;
			var acceptedControllers = (controller ?? currentController ?? "").Split(',');
			var acceptedActions = (action ?? currentAction ?? "").Split(',');

			return (acceptedAreas != null ? acceptedAreas.Contains(currentArea) : true) && acceptedControllers.Contains(currentController) && acceptedActions.Contains(currentAction) ? cssClass : "";
		}

		public static string? ActiveExpandContract(this IHtmlHelper htmlHelper, string? area = null, string? controller = null, string? action = null, string? hiddenClass = "hidden", string? visibleClass = "")
		{
			var currentArea = htmlHelper?.ViewContext.RouteData.Values["area"] as string;
			var currentController = htmlHelper?.ViewContext.RouteData.Values["controller"] as string;
			var currentAction = htmlHelper?.ViewContext.RouteData.Values["action"] as string;

			var acceptedAreas = (area != null) ? area.Split(',') : null;
			var acceptedControllers = (controller ?? currentController ?? "").Split(',');
			var acceptedActions = (action ?? currentAction ?? "").Split(',');

			return ((area != null) ? acceptedAreas.Contains(currentArea) : true) && acceptedControllers.Contains(currentController) && acceptedActions.Contains(currentAction) ? visibleClass : hiddenClass;
		}


		public static Type GetModelType<T>(this IHtmlHelper<T> html) => typeof(T);

	}
}