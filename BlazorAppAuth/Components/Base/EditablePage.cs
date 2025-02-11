using BlazorAppAuth.Web.Components.Base;

namespace BlazorAppAuth.Components.Base
{
    public class EditablePage : SecuredPage
    {
        public List<EditableSection> Sections { get; set; } = new();

        protected virtual void EditStart()
        {
            foreach (var section in Sections)
            {
                section.SetVisibility(false);
            }
        }

        protected virtual void EditEnd()
        {
            foreach (var section in Sections)
            {
                section.SetVisibility(true);
            }
        }
    }
}
