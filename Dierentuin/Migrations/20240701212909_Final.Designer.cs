﻿// <auto-generated />
using System;
using Dierentuin.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Dierentuin.Migrations
{
    [DbContext(typeof(DierentuinContext))]
    [Migration("20240701212909_Final")]
    partial class Final
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.4");

            modelBuilder.Entity("Dierentuin.Models.Animal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Activity")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Diet")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("EnclosureId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Prey")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Security")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Size")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Space")
                        .HasColumnType("REAL");

                    b.Property<string>("Species")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("EnclosureId");

                    b.ToTable("Animals");
                });

            modelBuilder.Entity("Dierentuin.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Dierentuin.Models.Enclosure", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Climate")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Habitat")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Security")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Size")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("Enclosures");
                });

            modelBuilder.Entity("Dierentuin.Models.Animal", b =>
                {
                    b.HasOne("Dierentuin.Models.Category", "Category")
                        .WithMany("Animals")
                        .HasForeignKey("CategoryId");

                    b.HasOne("Dierentuin.Models.Enclosure", "Enclosure")
                        .WithMany("Animals")
                        .HasForeignKey("EnclosureId");

                    b.Navigation("Category");

                    b.Navigation("Enclosure");
                });

            modelBuilder.Entity("Dierentuin.Models.Category", b =>
                {
                    b.Navigation("Animals");
                });

            modelBuilder.Entity("Dierentuin.Models.Enclosure", b =>
                {
                    b.Navigation("Animals");
                });
#pragma warning restore 612, 618
        }
    }
}
