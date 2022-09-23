using Borigran.OneData.Domain;

namespace Borigran.OneData.Authorization.Domain.Entities
{
    public class User : EntityBase
    {
        public virtual string PhoneNumber { get; set; }
        public virtual string? RefreshToken { get; set; }
        public virtual DateTime? RefreshTokenExpired { get; set; }
        public virtual bool IsPhisical { get; set; }
    }
}
