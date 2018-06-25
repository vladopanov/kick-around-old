using System.Collections.Generic;
using System.Linq;
using KickAround.Models.EntityModels;
using KickAround.Services.Contracts;

namespace KickAround.Services
{
    public class FeedbacksService : Service, IFeedbacksService
    {
        public IEnumerable<Feedback> GetAllFeedbacks()
        {
            IEnumerable<Feedback> feedbacks = this.Context.Feedbacks.ToList();

            return feedbacks;
        }

        public Feedback GetFeedback(string id)
        {
            Feedback feedback = this.Context.Feedbacks.Find(id);

            return feedback;
        }

        public void DeleteFeedback(string id)
        {
            Feedback feedback = this.GetFeedback(id);
            this.Context.Feedbacks.Remove(feedback);

            this.Context.SaveChanges();
        }
    }
}
