using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportCenter.Models
{
    public partial class RequestAbonement
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("ID_Client")]
        public int IdClient { get; set; }
        public int Term { get; set; }
        public TimeSpan Time { get; set; }

        [ForeignKey(nameof(IdClient))]
        [InverseProperty(nameof(Client.RequestAbonement))]
        public virtual Client IdClientNavigation { get; set; }
    }
}
