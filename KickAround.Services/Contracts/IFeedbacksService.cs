using System.Collections.Generic;
using KickAround.Data;
using KickAround.Models.EntityModels;

namespace KickAround.Services.Contracts
{
    public interface IFeedbacksService
    {
        IEnumerable<Feedback> GetAllFeedbacks();
        Feedback GetFeedback(string id);
        void DeleteFeedback(string id);
        KickAroundContext Context { get; }
    }
}