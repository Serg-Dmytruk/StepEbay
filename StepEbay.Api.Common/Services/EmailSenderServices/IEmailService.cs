using StepEbay.Common.Models.RefitModels;

namespace StepEbay.Main.Api.Common.Services.EmailSenderServices
{
    public interface IEmailService
    {
        Task SendRegistrationConfirm(string email, string nickname, int id, string emailKey);
        Task SendBetPlace(string emeil);
        Task SendNews(string title, string description);
        Task<ResponseData<BoolResult>> ConfirmRegistration(int id, string key);

        //Task SendBetWin(string email);
    }
}
