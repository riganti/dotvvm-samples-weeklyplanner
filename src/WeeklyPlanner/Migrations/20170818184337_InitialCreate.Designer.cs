﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using WeeklyPlanner.Data;

namespace WeeklyPlanner.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20170818184337_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WeeklyPlanner.Data.ScheduledTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CompletedDate");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime?>("DeletedDate");

                    b.Property<DateTime>("DueDate");

                    b.Property<string>("Text")
                        .IsRequired();

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("ScheduledTasks");
                });

            modelBuilder.Entity("WeeklyPlanner.Data.ScheduledTaskTag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ScheduledTaskId");

                    b.Property<int>("TagId");

                    b.HasKey("Id");

                    b.HasIndex("ScheduledTaskId");

                    b.HasIndex("TagId");

                    b.ToTable("ScheduledTaskTags");
                });

            modelBuilder.Entity("WeeklyPlanner.Data.ScheduledTaskTransfer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("NewDueDate");

                    b.Property<DateTime>("OldDueDate");

                    b.Property<int>("ScheduledTaskId");

                    b.Property<DateTime>("TransferDate");

                    b.HasKey("Id");

                    b.HasIndex("ScheduledTaskId");

                    b.ToTable("ScheduledTaskTransfers");
                });

            modelBuilder.Entity("WeeklyPlanner.Data.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("WeeklyPlanner.Data.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DisplayName")
                        .HasMaxLength(100);

                    b.Property<string>("ObjectId")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WeeklyPlanner.Data.ScheduledTask", b =>
                {
                    b.HasOne("WeeklyPlanner.Data.User", "User")
                        .WithMany("ScheduledTasks")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WeeklyPlanner.Data.ScheduledTaskTag", b =>
                {
                    b.HasOne("WeeklyPlanner.Data.ScheduledTask", "ScheduledTask")
                        .WithMany("Tags")
                        .HasForeignKey("ScheduledTaskId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WeeklyPlanner.Data.Tag", "Tag")
                        .WithMany("ScheduledTasks")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WeeklyPlanner.Data.ScheduledTaskTransfer", b =>
                {
                    b.HasOne("WeeklyPlanner.Data.ScheduledTask", "ScheduledTask")
                        .WithMany("Transfers")
                        .HasForeignKey("ScheduledTaskId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
