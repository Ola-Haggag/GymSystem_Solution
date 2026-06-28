using GymSystem.BLL.ViewModels.AnalyticsViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Interfaces
{
    public interface IAnalyticsServices
    {
        Task<AnalyticsViewModel> GetAnalyticsDataAsync(CancellationToken ct= default);
    }
}
