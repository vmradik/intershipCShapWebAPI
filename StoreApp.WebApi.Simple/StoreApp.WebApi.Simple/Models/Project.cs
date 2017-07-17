using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StoreApp.WebApi.Simple.Models
{
    public class Project : IIdEntity
    {
        public long Id { get; set; }
        public string nameololo { get; set; }
        public string nameololo2 { get; set; }
        [Required]
        public string Name { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}