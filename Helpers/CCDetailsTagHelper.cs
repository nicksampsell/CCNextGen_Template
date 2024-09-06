using Microsoft.AspNetCore.Razor.TagHelpers;

namespace CCVehicleMaintenance.TagHelpers
{
    public class CcDetailsTagHelper : TagHelper
    {

        // Define the properties that will accept the strings
        public string Label { get; set; } = String.Empty;
        public string Value { get; set; } = String.Empty;

        public string Empty { get; set; } = String.Empty;

        // Override the Process method to define the output
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // Set the tag to div (or any other tag if needed)
            output.TagName = "div";

            // Set the class attribute for the div
            output.Attributes.SetAttribute("class", "mb-2");

            // Build the content to be rendered inside the tag
            var content = $@"<div class=""mb-2"">{Label}</div>";

            if (!String.IsNullOrEmpty(Value))
            {

                content += $@"<strong>{Value}</strong>";
                
            }
            else
            {
                if (!String.IsNullOrEmpty(Empty))
                {
                    content += $@"<em class=""text-body-secondary"" style=""font-size: 0.8rem"">{Empty}</em>";
                }
                else
                {
                    content += $@"<span></span>";
                }
            }

            // Set the content of the output
            output.Content.SetHtmlContent(content);
        }
    }
}