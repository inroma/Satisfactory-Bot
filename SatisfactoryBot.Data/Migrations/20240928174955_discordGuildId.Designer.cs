﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SatisfactoryBot.Data;

#nullable disable

namespace SatisfactoryBot.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240928174955_discordGuildId")]
    partial class discordGuildId
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SatisfactoryBot.Data.Models.DiscordRole", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreationTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("DiscordRole");
                });

            modelBuilder.Entity("SatisfactoryBot.Data.Models.DiscordServer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreationTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("GuildId")
                        .HasColumnType("numeric(20,0)");

                    b.HasKey("Id");

                    b.ToTable("DiscordServer");
                });

            modelBuilder.Entity("SatisfactoryBot.Data.Models.Relations.SatisfactoryToDiscord", b =>
                {
                    b.Property<int>("DiscordServerId")
                        .HasColumnType("integer");

                    b.Property<int>("SatisfactoryServerId")
                        .HasColumnType("integer");

                    b.HasKey("DiscordServerId", "SatisfactoryServerId");

                    b.HasIndex("SatisfactoryServerId");

                    b.ToTable("SatisfactoryToDiscord");
                });

            modelBuilder.Entity("SatisfactoryBot.Data.Models.SatisfactoryServer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreationTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<decimal>("Owner")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("Token")
                        .HasColumnType("text");

                    b.Property<string>("Url")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.HasKey("Id");

                    b.ToTable("SatisfactoryServer");
                });

            modelBuilder.Entity("SatisfactoryBot.Data.Models.DiscordRole", b =>
                {
                    b.HasOne("SatisfactoryBot.Data.Models.DiscordServer", "DiscordServer")
                        .WithMany("DiscordRoles")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("DiscordServer");
                });

            modelBuilder.Entity("SatisfactoryBot.Data.Models.Relations.SatisfactoryToDiscord", b =>
                {
                    b.HasOne("SatisfactoryBot.Data.Models.DiscordServer", null)
                        .WithMany()
                        .HasForeignKey("DiscordServerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SatisfactoryBot.Data.Models.SatisfactoryServer", null)
                        .WithMany()
                        .HasForeignKey("SatisfactoryServerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SatisfactoryBot.Data.Models.DiscordServer", b =>
                {
                    b.Navigation("DiscordRoles");
                });
#pragma warning restore 612, 618
        }
    }
}