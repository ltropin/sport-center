using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportCenter.Models
{
    public partial class OrderGroup
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("ID_GroupTrain")]
        public int IdGroupTrain { get; set; }
        [Column("ID_Client")]
        public int IdClient { get; set; }
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [ForeignKey(nameof(IdClient))]
        [InverseProperty(nameof(Client.OrderGroup))]
        public virtual Client IdClientNavigation { get; set; }
        [ForeignKey(nameof(IdGroupTrain))]
        [InverseProperty(nameof(GroupTrain.OrderGroup))]
        public virtual GroupTrain IdGroupTrainNavigation { get; set; }
    }
}
