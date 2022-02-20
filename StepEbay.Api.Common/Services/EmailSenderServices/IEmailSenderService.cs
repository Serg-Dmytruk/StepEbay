namespace StepEbay.Main.Api.Common.Services.EmailSenderServices
{
    public interface IEmailSenderService
    {
        Task SendRegistrationConfirm(string emeil);

        Task SendBetPlace(string emeil);

        Task SendNews(string title, string description);

        //Task SendBetWin(string email);
    }
}
