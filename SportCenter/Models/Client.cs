using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace SportCenter.Models
{
    public partial class Client
    {
        public Client()
        {
            OrderGroup = new HashSet<OrderGroup>();
        }

        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Required]
        [Column("FIO")]
        public string Fio { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        [InverseProperty("IdClientNavigation")]
        public virtual ICollection<OrderGroup> OrderGroup { get; set; }

        public static string HashPass(string pass)
        {
            using (var sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(pass));
                return Encoding.UTF8.GetString(hash);
            }
        }
    }
}
