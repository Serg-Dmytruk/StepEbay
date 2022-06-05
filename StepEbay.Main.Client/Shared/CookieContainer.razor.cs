using Microsoft.AspNetCore.Components;
using StepEbay.Common.Lockers;
using StepEbay.Common.Storages;
using System.Threading.Tasks;

namespace StepEbay.Main.Client.Shared
{
    public partial class CookieContainer
    {
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Inject] private StorageQueue Queue { get; set; }
        [Inject] private CookieLocker CookieLocker { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await CookieLocker.LockAsync(async () =>
                {
                    while (Queue.Tasks.Count > 0)
                        await Queue.Tasks.Dequeue().Invoke();

                    Queue.Ready = true;
                });
            }
        }
    }
}