using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SportCenter.Models;

namespace SportCenter.Data
{
    public partial class SportCenterContext : DbContext
    {
        //public SportCenterContext()
        //{
        //}

        public SportCenterContext(DbContextOptions<SportCenterContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<GroupTrain> GroupTrain { get; set; }
        public virtual DbSet<OrderGroup> OrderGroup { get; set; }
        public virtual DbSet<Trainer> Trainer { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GroupTrain>(entity =>
            {
                entity.HasOne(d => d.IdTrainerNavigation)
                    .WithMany(p => p.GroupTrain)
                    .HasForeignKey(d => d.IdTrainer)
                    .HasConstraintName("FK_GroupTrain_Trainer_ID");
            });

            modelBuilder.Entity<OrderGroup>(entity =>
            {
                entity.HasOne(d => d.IdClientNavigation)
                    .WithMany(p => p.OrderGroup)
                    .HasForeignKey(d => d.IdClient)
                    .HasConstraintName("FK_OrderGroup_Client_ID");

                entity.HasOne(d => d.IdGroupTrainNavigation)
                    .WithMany(p => p.OrderGroup)
                    .HasForeignKey(d => d.IdGroupTrain)
                    .HasConstraintName("FK_OrderGroup_GroupTrain_ID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
