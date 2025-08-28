using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace CCNextGen_Template.Helpers
{
    /// <summary>
    /// A TagHelper that renders CRUD action buttons (Edit, Details, Delete)
    /// for a specified entity in an ASP.NET Core application.
    /// </summary>
    [HtmlTargetElement("crud-actions")]
    public class CrudActionsTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IAntiforgery _antiforgery;
        private readonly HtmlEncoder _htmlEncoder;

        /// <summary>
        /// Initializes a new instance of the <see cref="CrudActionsTagHelper"/> class.
        /// </summary>
        /// <param name="urlHelperFactory">The URL helper factory used to generate URLs.</param>
        /// <param name="antiforgery">The antiforgery service used to generate request verification tokens.</param>
        /// <param name="htmlEncoder">The HTML encoder used to encode HTML content.</param>
        public CrudActionsTagHelper(
            IUrlHelperFactory urlHelperFactory,
            IAntiforgery antiforgery,
            HtmlEncoder htmlEncoder)
        {
            _urlHelperFactory = urlHelperFactory;
            _antiforgery = antiforgery;
            _htmlEncoder = htmlEncoder;
            System.Diagnostics.Debug.WriteLine("CrudActionsTagHelper created!");
        }
        /// <summary>
        /// The ID of the entity for which the CRUD actions are generated.
        /// </summary>
        [HtmlAttributeName("id")]
        public object? Id { get; set; }

        /// <summary>
        /// The name of the controller to use for generating action URLs.
        /// If not set, the current controller will be used.
        /// </summary>
        [HtmlAttributeName("controller")]
        public string? Controller { get; set; }

        /// <summary>
        /// The base path for Razor Pages when <see cref="UsePages"/> is true.
        /// </summary>
        [HtmlAttributeName("page-base")]
        public string PageBase { get; set; } = String.Empty; // For Razor Pages

        /// <summary>
        /// Indicates whether to use Razor Pages instead of MVC actions for links.
        /// </summary>
        [HtmlAttributeName("use-pages")]
        public bool UsePages { get; set; } = false;

        /// <summary>
        /// Determines whether to show the Edit button.
        /// </summary>
        [HtmlAttributeName("show-edit")]
        public bool ShowEdit { get; set; } = true;

        /// <summary>
        /// Determines whether to show the Details button.
        /// </summary>
        [HtmlAttributeName("show-details")]
        public bool ShowDetails { get; set; } = true;

        /// <summary>
        /// Determines whether to show the Delete button.
        /// </summary>
        [HtmlAttributeName("show-delete")]
        public bool ShowDelete { get; set; } = true;

        /// <summary>
        /// The current view context. This is automatically populated by the framework.
        /// </summary>
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; } = new ViewContext();

        /// <summary>
        /// Asynchronously processes the tag helper to render the CRUD action buttons.
        /// </summary>
        /// <param name="context">The current tag helper context.</param>
        /// <param name="output">The output to write to.</param>
        /// <returns>A completed task.</returns>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            Process(context, output);
            await Task.CompletedTask;
        }

        /// <summary>
        /// Synchronously processes the tag helper to render the CRUD action buttons.
        /// </summary>
        /// <param name="context">The current tag helper context.</param>
        /// <param name="output">The output to write to.</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);

            string currentController = !string.IsNullOrEmpty(Controller)
                ? Controller
                : ViewContext.RouteData.Values["controller"]?.ToString() ?? "";

            string pageBase = !string.IsNullOrEmpty(PageBase)
                ? PageBase
                : $"/{currentController}";

            var buttonHtml = new List<string>();

            if (ShowEdit)
                buttonHtml.Add(RenderLinkOrForm("Edit", urlHelper, pageBase, currentController));
            if (ShowDetails)
                buttonHtml.Add(RenderLinkOrForm("Details", urlHelper, pageBase, currentController));
            if (ShowDelete)
                buttonHtml.Add(RenderLinkOrForm("Delete", urlHelper, pageBase, currentController));

            string wrapperDiv = $@"
<div class='d-flex justify-content-end align-items-center gap-1 align-self-end'>
    {string.Join(Environment.NewLine, buttonHtml)}
</div>";

            output.TagName = null; // Prevents rendering <crud-actions> tag
            output.Content.SetHtmlContent(wrapperDiv);
        }

        private string RenderLinkOrForm(string action, IUrlHelper urlHelper, string pageBase, string? controller = "")
        {
            var cssClass = $"btn btn-sm {GetButtonClass(action)}";

            string url = UsePages
                ? urlHelper.Page($"{pageBase}/{action}", new { id = Id })
                : urlHelper.Action(action, controller, new { id = Id });

            if (string.Equals(action, "Delete", StringComparison.OrdinalIgnoreCase))
            {
                var antiForgeryToken = _antiforgery.GetTokens(ViewContext.HttpContext).RequestToken;
                var antiForgeryInput = $@"<input name=""__RequestVerificationToken"" type=""hidden"" value=""{_htmlEncoder.Encode(antiForgeryToken)}"">";

                return $@"
<form method='post' action='{url}' style='display:inline;' onsubmit='return confirm(""Are you sure you want to delete this item?"")'>
    {antiForgeryInput}
    <button type='submit' class='{cssClass}'>{action}</button>
</form>";
            }
            else
            {
                return $"<a href='{url}' class='{cssClass}'>{action}</a>";
            }
        }

        private string GetButtonClass(string action) => action.ToLower() switch
        {
            "edit" => "btn-primary",
            "details" or "view" => "btn-secondary",
            "delete" => "btn-danger",
            _ => "btn-light"
        };
    }
}
