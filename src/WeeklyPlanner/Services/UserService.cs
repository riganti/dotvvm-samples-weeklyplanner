using System.Linq;
using System.Security.Claims;
using WeeklyPlanner.Data;
using WeeklyPlanner.DTO;

namespace WeeklyPlanner.Services
{
    public class UserService : ServiceBase
    {

        public UserService(AppDbContext context) : base(context)
        {
        }


        public UserDTO GetOrCreateUser(ClaimsIdentity identity)
        {
            var objectId = identity.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            var user = Context.Users.SingleOrDefault(u => u.ObjectId == objectId);
            if (user == null)
            {
                user = new User()
                {
                    ObjectId = objectId,
                    DisplayName = identity.FindFirst(ClaimTypes.Name).Value
                };
                Context.Users.Add(user);
                Context.SaveChanges();
            }

            return new UserDTO()
            {
                Id = user.Id,
                DisplayName = user.DisplayName
            };
        }

    }

}