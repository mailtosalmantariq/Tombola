using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MT.Tombola.Api.Data.Models
{
    public class Bean
    {
        public int Id { get; set; } 

        [Required]
        public string ExternalId { get; set; } = default!; 

        public int Index { get; set; }

        public bool InitialIsBotd { get; set; } 

        [Column(TypeName = "decimal(10,2)")]
        public decimal Cost { get; set; } 

        [Required]
        [MaxLength(500)]
        public string Image { get; set; } = default!;

        [Required]
        [MaxLength(50)]
        public string Colour { get; set; } = default!;

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = default!;

        [Required]
        public string Description { get; set; } = default!;

        [Required]
        [MaxLength(100)]
        public string Country { get; set; } = default!;
    }


}
