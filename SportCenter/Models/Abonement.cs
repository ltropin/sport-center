using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportCenter.Models
{
    public partial class Abonement
    {
        [Key]
        public long Number { get; set; }
        public bool IsActive { get; set; }
        [Column(TypeName = "date")]
        public DateTime? DateBlock { get; set; }
        public int? IntervalBlock { get; set; }
        public int Capacity { get; set; }
        [Column("ID_Client")]
        public int IdClient { get; set; }
        public TimeSpan Time { get; set; }
        public int Term { get; set; }

        [ForeignKey(nameof(IdClient))]
        [InverseProperty(nameof(Client.Abonement))]
        public virtual Client IdClientNavigation { get; set; }
    }
}
