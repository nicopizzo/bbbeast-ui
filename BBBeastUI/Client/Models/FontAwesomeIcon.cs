using Havit.Blazor.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BBBeastUI.Models
{
    public class FontAwesomeIcon : IconBase
    {
        public override Type RendererComponentType => typeof(FAIcon);

		public string Name { get; set; }

        public static FontAwesomeIcon Check = new FontAwesomeIcon("fas fa-check");
        public static FontAwesomeIcon CircleExclamation = new FontAwesomeIcon("fas fa-exclamation-circle");
        public static FontAwesomeIcon TriangleExclamation = new FontAwesomeIcon("fas fa-exclamation-triangle");
        public static FontAwesomeIcon CircleQuestion = new FontAwesomeIcon("fas fa-question-circle");
        public static FontAwesomeIcon Bars = new FontAwesomeIcon("fas fa-bars");
        public static FontAwesomeIcon Discord = new FontAwesomeIcon("fab fa-discord");
        public static FontAwesomeIcon Twitter = new FontAwesomeIcon("fab fa-twitter");
        public static FontAwesomeIcon PlusCircle = new FontAwesomeIcon("fas fa-plus-circle");
        public static FontAwesomeIcon MinusCircle = new FontAwesomeIcon("fas fa-minus-circle");

        private FontAwesomeIcon(string name)
		{
			Name = name;
		}
	}

    internal class FAIcon : ComponentBase
    {
        [Parameter] public FontAwesomeIcon Icon { get; set; }
        [Parameter] public string CssClass { get; set; }
        [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "i");
            builder.AddAttribute(1, "class", CssClassHelper.Combine(Icon.Name, CssClass));
            builder.AddMultipleAttributes(2, AdditionalAttributes);
            builder.CloseElement();
        }
    }
}
