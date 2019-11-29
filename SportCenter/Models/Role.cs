using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportCenter.Models
{
    public partial class Role
    {
        public Role()
        {
            Client = new HashSet<Client>();
        }

        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [InverseProperty("IdRoleNavigation")]
        public virtual ICollection<Client> Client { get; set; }
    }
}
