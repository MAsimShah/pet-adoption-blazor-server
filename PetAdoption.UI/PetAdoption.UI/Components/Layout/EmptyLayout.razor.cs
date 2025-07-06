namespace PetAdoption.UI.Components.Layout
{
    public partial class EmptyLayout
    {
        private bool showLoader;

        protected override void OnInitialized()
        {
            Loader.OnShow += () => InvokeAsync(() =>
            {
                showLoader = true;
                StateHasChanged();
            });

            Loader.OnHide += () => InvokeAsync(() =>
            {
                showLoader = false;
                StateHasChanged();
            });
        }
    }
}
