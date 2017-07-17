using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StoreApp.WebApi.Simple.Models
{
    public class Activity : IIdEntity
    {
        public long Id { get; set; }

        [Required]
        public DateTime Begin { get; set; }
        [Required]
        public DateTime End { get; set; }
        [Required]
        public long TaskId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public double Level { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Task Task { get; set; }
    }
}