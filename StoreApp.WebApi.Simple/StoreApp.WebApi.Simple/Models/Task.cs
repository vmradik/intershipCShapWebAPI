using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StoreApp.WebApi.Simple.Models
{
    public class Task : IIdEntity
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string About { get; set; }

        //[Required]
        //// foreign key
        public long ProjectId { get; set; }


        // link
        public virtual Project Project { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }
    }
}