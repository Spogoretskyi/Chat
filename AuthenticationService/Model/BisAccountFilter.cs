using DBLayer.BisLayer;
using LinqKit;
using System;
using System.Linq.Expressions;

namespace AuthenticationService.Model
{
    class BisAccountFilter
    {
        public int User_Id { get; set; }
        public string Password { get; set; }
        public int Registration { get; set; }

        public Expression<Func<BisAccount, bool>> predicate
        {
            get
            {
                var predicate = PredicateBuilder.New<BisAccount>(true);
                if (User_Id != 0) predicate = predicate.And(e => e.User_Id.Equals(User_Id));
                if (Password != null) predicate = predicate.And(e => e.Password.Contains(Password));
                if (Registration != 0) predicate = predicate.And(e => e.registration.Equals(Registration));
                return predicate;
            }
        }
    }
}
