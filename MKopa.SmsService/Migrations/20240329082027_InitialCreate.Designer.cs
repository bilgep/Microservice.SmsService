﻿// <auto-generated />
using System;
using MKopa.DataAccess.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MKopa.Core.Migrations
{
    [DbContext(typeof(SmsDbContext))]
    [Migration("20240329082027_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.0");

            modelBuilder.Entity("MKopa.Core.Entities.Providers.BaseSmsProvider", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("CountryCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsPrimary")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("SmsServiceProviders");

                    b.HasData(
                        new
                        {
                            Id = "bf3b76bf-dd12-4e31-889f-6dddbb35b23e",
                            CountryCode = "90",
                            IsPrimary = true,
                            Name = "Provider A"
                        },
                        new
                        {
                            Id = "3e1a033d-e821-4b6a-acef-9c51f77ec6a4",
                            CountryCode = "90",
                            IsPrimary = false,
                            Name = "Provider B"
                        });
                });

            modelBuilder.Entity("MKopa.Core.Entities.Sms.BaseSmsMessage", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("CountryCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("ModifiedDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SmsText")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("SmsMessages");

                    b.HasData(
                        new
                        {
                            Id = "be2b2b50-f839-4d11-ab98-ee5ea6562314",
                            CountryCode = "90",
                            CreatedDate = new DateTimeOffset(new DateTime(2024, 3, 28, 8, 20, 27, 237, DateTimeKind.Unspecified).AddTicks(6400), new TimeSpan(0, 0, 0, 0, 0)),
                            ModifiedDate = new DateTimeOffset(new DateTime(2024, 3, 29, 8, 15, 27, 237, DateTimeKind.Unspecified).AddTicks(6409), new TimeSpan(0, 0, 0, 0, 0)),
                            PhoneNumber = "555555555",
                            SmsText = "Hello world",
                            Status = 1
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
