using StepEbay.Common.Models.RefitModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepEbay.Main.Api.Common.Services.BetServices
{
    public interface IBetService
    {
        public Task<ResponseData> PlaceBet(int userId, int productId);
    }
}
