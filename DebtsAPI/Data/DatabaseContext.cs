using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DebtsAPI.Models;

namespace DebtsAPI.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {

        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public virtual DbSet<Debt> Debts { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public DbSet<Payment> Payments { get; set; }

        public DbSet<UserContacts> UserContacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Debt>().ToTable("Debts");
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Payment>().ToTable("Payments");

            modelBuilder.Entity<UserContacts>().HasKey(r => new { r.UserId, r.ContactId });

            modelBuilder.Entity<UserContacts>()
            .HasOne(pt => pt.User)
            .WithMany(p => p.SentInvitations)
            .HasForeignKey(pt => pt.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserContacts>()
            .HasOne(pt => pt.Contact)
            .WithMany(p => p.ReceivedInvitations)
            .HasForeignKey(pt => pt.ContactId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Debt>()
            .HasOne(s => s.Giver)
            .WithMany(g => g.GivenDebts)
            .HasForeignKey(s => s.GiverId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Debt>()
            .HasOne(s => s.Taker)
            .WithMany(g => g.TakenDebts)
            .HasForeignKey(s => s.TakerId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
            .HasOne(s => s.Debt)
            .WithMany(g => g.Payments)
            .HasForeignKey(s => s.DebtId)
            .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
