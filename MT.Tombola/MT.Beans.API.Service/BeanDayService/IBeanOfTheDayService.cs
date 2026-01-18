using MT.Tombola.Api.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Beans.API.Service.BeanDayService
{
    public interface IBeanOfTheDayService
    {
        Task<Bean> GetTodayAsync();
    }

}
