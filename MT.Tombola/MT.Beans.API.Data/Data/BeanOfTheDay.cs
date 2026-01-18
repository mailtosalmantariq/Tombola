using MT.Tombola.Api.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tombola.Api.Data.Data
{
    public class BeanOfTheDay
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public int BeanId { get; set; }
        public Bean Bean { get; set; } = default!;
    }

}
