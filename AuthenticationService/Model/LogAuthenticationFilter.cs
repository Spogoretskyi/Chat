using DBLayer.BisLayer;
using LinqKit;
using System;
using System.Linq.Expressions;

namespace AuthenticationService.Model
{
    class LogAuthenticationFilter
    {
        public int User_id { get; set; }
        public int Action { get; set; }
        public DateTime Time { get; set; }

        public Expression<Func<BisLogAuthentication, bool>> predicate
        {
            get
            {
                var predicate = PredicateBuilder.New<BisLogAuthentication>(true);
                if (User_id != 0) predicate = predicate.And(e => e.User_id.Equals(User_id));
                if (Action != 0) predicate = predicate.And(e => e.Action.Equals(Action));
                return predicate;
            }
        }
    }
}
