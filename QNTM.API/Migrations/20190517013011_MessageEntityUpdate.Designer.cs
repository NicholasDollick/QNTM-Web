﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QNTM.API.Data;

namespace QNTM.API.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20190517013011_MessageEntityUpdate")]
    partial class MessageEntityUpdate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024");

            modelBuilder.Entity("QNTM.API.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content");

                    b.Property<DateTime?>("DateRead");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsRead");

                    b.Property<DateTime>("MessageSent");

                    b.Property<int>("RecipientId");

                    b.Property<int>("SenderId");

                    b.HasKey("Id");

                    b.HasIndex("RecipientId");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("QNTM.API.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PasswordSalt");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("QNTM.API.Models.Value", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Values");
                });

            modelBuilder.Entity("QNTM.API.Models.Message", b =>
                {
                    b.HasOne("QNTM.API.Models.User", "Recipient")
                        .WithMany("MessagesRecieved")
                        .HasForeignKey("RecipientId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("QNTM.API.Models.User", "Sender")
                        .WithMany("MessagesSent")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}