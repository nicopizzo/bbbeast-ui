using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace nft.ui.Components
{
    public partial class BeastCarousel : ComponentBase
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            await JSRuntime.InvokeVoidAsync("startCarousel", "#sampleImages");
        }
    }
}
