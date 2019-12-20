using System;
using System.Threading.Tasks;

namespace GuidedLearningClio
{
    public interface IBank
    {
        Task<IBankResult> GetRateAsync(string currency, DateTime date);
    }
}