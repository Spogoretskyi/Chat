using DBLayer.BisLayer;
using LinqKit;
using System;
using System.Linq.Expressions;

namespace AuthenticationService.Model
{
    class LogMessagingFilter
    {
        public int Sender { get; set; }
        public string message { get; set; }
        public string file_name { get; set; }
        public string reciever { get; set; }

        public Expression<Func<BisLogMessaging, bool>> predicate
        {
            get
            {
                var predicate = PredicateBuilder.New<BisLogMessaging>(true);
                if (Sender != 0) predicate = predicate.And(e => e.Equals(Sender));
                if (message != null) predicate = predicate.And(e => e.message.Contains(message));
                if (file_name != null) predicate = predicate.And(e => e.file_name.Contains(file_name));
                if (reciever != null) predicate = predicate.And(e => e.reciever.Contains(reciever));
                return predicate;
            }
        }
    }
}
