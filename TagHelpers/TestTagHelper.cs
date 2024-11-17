using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ShoesForFeet.TagHelpers
{
    public class TestTagHelper : TagHelper
    {
        public string Link { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a"; // Replace <test> with <a>
            output.Attributes.SetAttribute("href", Link);
            output.Content.SetContent("Visit ShoesForFeet Partners");
        }
    }
}