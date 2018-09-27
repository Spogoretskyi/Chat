using DBLayer.BisLayer;
using LinqKit;
using System;
using System.Linq.Expressions;

namespace AuthenticationService.Model
{
    class BisUserFilter
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public Expression<Func<BisUser, bool>> predicate
        {
            get
            {
                var predicate = PredicateBuilder.New<BisUser>(true);
                if (Name != null) predicate = predicate.And(e => e.Name.Contains(Name));
                if (Phone != null) predicate = predicate.And(e => e.Phone.Contains(Phone));
                if (Email != null) predicate = predicate.And(e => e.Email.Contains(Phone));
                return predicate;
            }
        }
    }
}
