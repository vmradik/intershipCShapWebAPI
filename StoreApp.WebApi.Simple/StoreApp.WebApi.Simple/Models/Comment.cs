using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StoreApp.WebApi.Simple.Models
{
    public class Comment : IIdEntity
    {
        public long Id { get; set; }

        [Required]
        public string Value { get; set; }

        //// foreign key
        public long TaskId { get; set; }
        public string UserId { get; set; }
        // link
        public virtual Task Task { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}