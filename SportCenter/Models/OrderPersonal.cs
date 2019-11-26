using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportCenter.Models
{
    public partial class OrderPersonal
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("ID_Client")]
        public int IdClient { get; set; }
        [Column("ID_PersonalTrain")]
        public int IdPersonalTrain { get; set; }
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [ForeignKey(nameof(IdClient))]
        [InverseProperty(nameof(OrderPersonal))]
        public virtual Client IdClientNavigation { get; set; }
        [ForeignKey(nameof(IdPersonalTrain))]
        [InverseProperty(nameof(OrderPersonal))]
        public virtual PersonalTrain IdPersonalTrainNavigation { get; set; }
    }
}
