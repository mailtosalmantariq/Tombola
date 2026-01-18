using MT.Tombola.Api.Data.Data;
using MT.Tombola.Api.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tombola.Api.Data.Repos.BeanDay
{
    public interface IBeanOfTheDayRepository
    {
        Task<Bean> GetTodayAsync();
    }

}
