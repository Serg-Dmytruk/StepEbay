using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace StepEbay.Main.Client.Base.Pages.PersonalAccount
{
    [Route("/person")]
    //[Authorize]
    public partial class PersonalAccount
    {
        protected bool IsDisabled { get; set; }
        protected string niknameVal { get; set; }
        public PersonalAccount()
        {
            IsDisabled = true;
            niknameVal = "";
        }

    }
}
