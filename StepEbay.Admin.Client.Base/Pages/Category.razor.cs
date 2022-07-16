using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using StepEbay.Admin.Client.Common.RestServices;
using StepEbay.Admin.Common.Models.Products;
using StepEbay.Admin.Common.Models.Telegram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepEbay.Admin.Client.Base.Pages
{
    [Route("/category")]
    //[Authorize(Roles = "admin")]
    public partial class Category
    {
        private string _nameToAdd;
        private string _nameToRemove;
        private int? _idToRemove;

        private int? _idToUpdate;
        private string _nameToUpdate;
        private string _nameToUpdate1;
        private string _nameToUpdate2;

        private List<CategoryResponseDto> _list;
        [Inject] IApiService _service { get; set; }
        public Category()
        {
            _list = new List<CategoryResponseDto>();
        }

        public async Task AddCategory()
        {
            await _service.ExecuteRequest(() => _service.ApiMethods.AddCategory(_nameToAdd));
            _nameToAdd = null;
            this.StateHasChanged();
        }
        public async Task RemoveCategoryById()
        {
            await _service.ExecuteRequest(() => _service.ApiMethods.RemoveCategory(_idToRemove.Value));
            _idToRemove = null;
            this.StateHasChanged();
        }
        public async Task RemoveCategoryByName()
        {
            await _service.ExecuteRequest(() => _service.ApiMethods.RemoveCategoryName(_nameToRemove));
            _nameToRemove = null;
            this.StateHasChanged();
        }
        public async Task UpdateCategoryById()
        {
            await _service.ExecuteRequest(() => _service.ApiMethods.UpdateCategory(_idToUpdate.Value, _nameToUpdate1));
            _idToUpdate = null;
            _nameToUpdate1 = null;
            this.StateHasChanged();
        }
        public async Task UpdateCategoryByName()
        {
            await _service.ExecuteRequest(() => _service.ApiMethods.UpdateCategoryName(_nameToUpdate, _nameToUpdate2));
            _nameToUpdate = null;
            _nameToUpdate2 = null;
            this.StateHasChanged();
        }

        public async Task Update()
        {
            _list = (await _service.ExecuteRequest(() => _service.ApiMethods.GetAllCategoryDto())).Data;
            this.StateHasChanged();
        }
    }
}
