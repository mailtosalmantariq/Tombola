using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tombola.Api.Data.DTO.Import
{
    public class BeanDto
    {
        public string _id { get; set; } = default!; 
        public int index { get; set; }
        public bool isBOTD { get; set; }
        public string Cost { get; set; } = default!; 
        public string Image { get; set; } = default!; 
        public string colour { get; set; } = default!; 
        public string Name { get; set; } = default!; 
        public string Description { get; set; } = default!; 
        public string Country { get; set; } = default!;
    }
}
