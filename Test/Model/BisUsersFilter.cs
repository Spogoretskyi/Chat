using DBLayer.BizLayer;
using LinqKit;
using System;
using System.Linq.Expressions;

namespace AuthenticationService.Model
{
    class BisUsersFilter
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int Registration { get; set; }

        public Expression<Func<BisUsers, bool>> predicate
        {
            get
            {
                var predicate = PredicateBuilder.New<BisUsers>(true);
                if (Name != null)
                    predicate = predicate.And(e => e.name.Contains(Name));
                if (Password != null)
                    predicate = predicate.And(e => e.password.Contains(Password));
                if (Email != null)
                    predicate = predicate.And(e => e.email.Contains(Email));
                if (Phone != null)
                    predicate = predicate.And(e => e.phone.Contains(Phone));
                if (Registration != 0)
                    predicate = predicate.And(e => e.registration.ToString() == Convert.ToString(Registration));
                return predicate;
            }
        }

    }
}
