using Microsoft.JSInterop;

namespace StepEbay.Common.Storages
{
    public class LocalStorage : IAsyncDisposable
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly StorageQueue _queue;

        private Task<IJSObjectReference> _module;

        private Task<IJSObjectReference> Module => _module ??=
            _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/local-storage.js",
                Timeout.InfiniteTimeSpan).AsTask();

        public LocalStorage(IJSRuntime jsRuntime, StorageQueue queue)
        {
            _jsRuntime = jsRuntime;
            _queue = queue;
        }

        public async Task InvokeSet(Func<Task> func)
        {
            if (_queue.Ready)
                await func.Invoke();
            else
                _queue.Tasks.Enqueue(func);
        }

        public async Task SetLocal(string name, string value)
        {
            await InvokeSet(async () =>
                await (await Module).InvokeVoidAsync("setLocal", Timeout.InfiniteTimeSpan, name, value));
        }

        public async Task<string> GetLocal(string name)
        {
            return await (await Module).InvokeAsync<string>("getLocal", Timeout.InfiniteTimeSpan, name);
        }

        public async ValueTask DisposeAsync()
        {
            if (_module != null)
                await (await _module).DisposeAsync();
        }
    }
}
