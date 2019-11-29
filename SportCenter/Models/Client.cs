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
            Abonement = new HashSet<Abonement>();
            OrderGroup = new HashSet<OrderGroup>();
            PersonalTrain = new HashSet<PersonalTrain>();
            RequestAbonement = new HashSet<RequestAbonement>();
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
        [Column("ID_Role")]
        public int IdRole { get; set; }

        [ForeignKey(nameof(IdRole))]
        [InverseProperty(nameof(Role.Client))]
        public virtual Role IdRoleNavigation { get; set; }
        [InverseProperty("IdClientNavigation")]
        public virtual ICollection<Abonement> Abonement { get; set; }
        [InverseProperty("IdClientNavigation")]
        public virtual ICollection<OrderGroup> OrderGroup { get; set; }
        [InverseProperty("IdClientNavigation")]
        public virtual ICollection<PersonalTrain> PersonalTrain { get; set; }
        [InverseProperty("IdClientNavigation")]
        public virtual ICollection<RequestAbonement> RequestAbonement { get; set; }

        public static string HashPass(string pass)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(pass));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    // lowercase
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }
}
