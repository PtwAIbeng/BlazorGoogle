using BlazorAppAuth.Components.Base;
using BlazorGoogle.Development.Core.Account;
using Microsoft.AspNetCore.Components;
using System.Diagnostics;

namespace BlazorAppAuth.Web.Components.Base
{
    public class EditableSection : ComponentBase
    {
        public bool EditMode { get; set; }
        public bool Visible { get; set; } = true;

        [CascadingParameter]
        public EditablePage ParentPage { get; set; }

        [Parameter]
        public EventCallback OnEditStart { get; set; }

        [Parameter]
        public EventCallback OnEditEnd { get; set; }

        protected bool IsViewMode()
        {
            return !EditMode && Visible;
        }

        protected bool IsEditMode()
        {
            return EditMode && Visible;
        }

        public async Task ToggleEdit(bool isEditMode)
        {
            if (isEditMode)
            {
                await OnEditStart.InvokeAsync();
            }
            else
            {
                await OnEditEnd.InvokeAsync();
            }

            EditMode = isEditMode;
            SetVisibility(true);
        }

        public void SetVisibility(bool isVisible)
        {
            Visible = isVisible;
        }

        protected override void OnInitialized()
        {
            ParentPage.Sections.Add(this);
        }

        protected Account GetAccount()
        {
            return ParentPage.GetAccount();
        }
    }
}
