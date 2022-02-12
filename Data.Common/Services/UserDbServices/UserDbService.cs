using StepEbay.Data.Common.Services.Default;
using StepEbay.Data.Models.Users;

namespace StepEbay.Data.Common.Services.UserDbServices
{
    public class UserDbService : DefaultDbService<int, User>, IUserDbService
    {
        private readonly ApplicationDbContext _context;
        public UserDbService(ApplicationDbContext context) 
            : base(context)
        {
            _context = context;
        }
    }
}
