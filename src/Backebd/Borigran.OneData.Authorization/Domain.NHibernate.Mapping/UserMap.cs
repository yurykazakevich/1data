using Borigran.OneData.Authorization.Domain.Entities;
using Borigran.OneData.Domain.NHibernate.Mapping;

namespace Borigran.OneData.Authorization.Domain.NHibernate.Mapping
{
    public class UserMap : MapBase<User>
    {
        public UserMap()
            :base()
        {
            Map(x => x.PhoneNumber)
 //               .Index($"{nameof(User.PhoneNumber)}_IDX")
                .UniqueKey($"{nameof(User.PhoneNumber)}_UK");
            Map(x => x.RefreshToken);
            Map(x => x.RefreshTokenExpired);
        }
    }
}
