﻿// <auto-generated />
using System;
using DebtsAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DebtsAPI.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20190110223422_AddPaymentsMigration")]
    partial class AddPaymentsMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DebtsAPI.Models.Debt", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("Date");

                    b.Property<DateTimeOffset>("Deadline");

                    b.Property<string>("Description");

                    b.Property<int?>("GiverId");

                    b.Property<int>("Sum");

                    b.Property<int?>("TakerId");

                    b.Property<bool>("isRepaid");

                    b.HasKey("Id");

                    b.HasIndex("GiverId");

                    b.HasIndex("TakerId");

                    b.ToTable("Debts");
                });

            modelBuilder.Entity("DebtsAPI.Models.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Amount");

                    b.Property<int>("DebtId");

                    b.Property<string>("Description");

                    b.HasKey("Id");

                    b.HasIndex("DebtId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("DebtsAPI.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsVirtual");

                    b.Property<string>("LastName");

                    b.Property<byte[]>("PasswordHash");

                    b.Property<byte[]>("PasswordSalt");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DebtsAPI.Models.UserContacts", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("ContactId");

                    b.Property<bool>("IsRead");

                    b.HasKey("UserId", "ContactId");

                    b.HasIndex("ContactId");

                    b.ToTable("UserContacts");
                });

            modelBuilder.Entity("DebtsAPI.Models.Debt", b =>
                {
                    b.HasOne("DebtsAPI.Models.User", "Giver")
                        .WithMany("GivenDebts")
                        .HasForeignKey("GiverId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("DebtsAPI.Models.User", "Taker")
                        .WithMany("TakenDebts")
                        .HasForeignKey("TakerId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("DebtsAPI.Models.Payment", b =>
                {
                    b.HasOne("DebtsAPI.Models.Debt", "Debt")
                        .WithMany("Payments")
                        .HasForeignKey("DebtId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("DebtsAPI.Models.UserContacts", b =>
                {
                    b.HasOne("DebtsAPI.Models.User", "Contact")
                        .WithMany("ReceivedInvitations")
                        .HasForeignKey("ContactId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("DebtsAPI.Models.User", "User")
                        .WithMany("SentInvitations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
