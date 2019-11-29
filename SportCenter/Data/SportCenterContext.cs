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

        public virtual DbSet<Abonement> Abonement { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<GroupTrain> GroupTrain { get; set; }
        public virtual DbSet<OrderGroup> OrderGroup { get; set; }
        public virtual DbSet<PersonalTrain> PersonalTrain { get; set; }
        public virtual DbSet<RequestAbonement> RequestAbonement { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Trainer> Trainer { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Abonement>(entity =>
            {
                entity.HasKey(e => e.Number)
                    .HasName("PK_Abonement_ID");

                entity.HasOne(d => d.IdClientNavigation)
                    .WithMany(p => p.Abonement)
                    .HasForeignKey(d => d.IdClient)
                    .HasConstraintName("FK_Abonement_Client_ID");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.Property(e => e.IdRole).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.IdRoleNavigation)
                    .WithMany(p => p.Client)
                    .HasForeignKey(d => d.IdRole)
                    .HasConstraintName("FK_Client_Role_ID");
            });

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

            modelBuilder.Entity<PersonalTrain>(entity =>
            {
                entity.HasOne(d => d.IdClientNavigation)
                    .WithMany(p => p.PersonalTrain)
                    .HasForeignKey(d => d.IdClient)
                    .HasConstraintName("FK_PersonalTrain_Client_ID");

                entity.HasOne(d => d.IdTrainerNavigation)
                    .WithMany(p => p.PersonalTrain)
                    .HasForeignKey(d => d.IdTrainer)
                    .HasConstraintName("FK_PersonalTrain_Trainer_ID");
            });

            modelBuilder.Entity<RequestAbonement>(entity =>
            {
                entity.HasOne(d => d.IdClientNavigation)
                    .WithMany(p => p.RequestAbonement)
                    .HasForeignKey(d => d.IdClient)
                    .HasConstraintName("FK_RequestAbonement_Client_ID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
