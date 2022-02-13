using BBBeast.UI.Shared.Interfaces;
using BBBeast.UI.Shared.Models;
using Havit.Blazor.Components.Web.Bootstrap;
using Microsoft.AspNetCore.Components;

namespace BBBeastUI.Pages
{
    public partial class Provenance : ComponentBase
    {
        [Inject]
        protected IProvenanceHashService _HashService { get; set; }

        private string _Concated
        {
            get
            {
                if(_HashValues == null) return string.Empty;
                return string.Concat(_HashValues.Select(f => f.Value));
            }
        }

        private Dictionary<int, string> _HashValues
        {
            get
            {
                if(_HashDto == null) return null;
                return _HashDto.Hashes.Select(f => f.Split("\t")).ToDictionary(x => int.Parse(x[0]), y => y[1]);
            }
        }

        private bool _Loaded = false;
        private HashDto _HashDto = null;

        protected override async Task OnInitializedAsync()
        {           
            try
            {
                _HashDto = await _HashService.GetProvenanceHash();
                if(_HashDto != null) _Loaded = true;
            }
            catch
            {
                _Loaded = true;
            }
            await base.OnInitializedAsync();
        }

        private Task<GridDataProviderResult<KeyValuePair<int, string>>> GridDataProvider(GridDataProviderRequest<KeyValuePair<int, string>> request)
        {
            return Task.FromResult(request.ApplyTo(_HashValues));
        }
    }
}
