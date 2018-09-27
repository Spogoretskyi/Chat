using DBLayer.BisLayer;
using LinqKit;
using System;
using System.Linq.Expressions;

namespace AuthenticationService.Model
{
    class LogConnectFilter
    {
        public int User_id { get; set; }
        public int Action { get; set; }

        public Expression<Func<BisLogConnect, bool>> predicate
        {
            get
            {
                var predicate = PredicateBuilder.New<BisLogConnect>(true);
                if (User_id != 0) predicate = predicate.And(e => e.Equals(User_id));
                if (Action != 0) predicate = predicate.And(e => e.Equals(Action));
                return predicate;
            }
        }
    }
}
