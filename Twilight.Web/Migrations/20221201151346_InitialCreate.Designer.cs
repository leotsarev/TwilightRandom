﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Twilight.Web;

#nullable disable

namespace Twilight.Web.Migrations
{
    [DbContext(typeof(DbContext))]
    [Migration("20221201151346_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "player_color", new[] { "black", "yellow", "violet", "green", "blue", "orange", "red", "pink" });
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("FactionPlayerSlot", b =>
                {
                    b.Property<int>("PlayerSlotId")
                        .HasColumnType("integer");

                    b.Property<int>("PossibleFactionsId")
                        .HasColumnType("integer");

                    b.HasKey("PlayerSlotId", "PossibleFactionsId");

                    b.HasIndex("PossibleFactionsId");

                    b.ToTable("FactionPlayerSlot");
                });

            modelBuilder.Entity("Twilight.Domain.Faction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RussianName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Faction");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "The Arborec"
                        },
                        new
                        {
                            Id = 2,
                            Name = "The Barony of Letnev"
                        },
                        new
                        {
                            Id = 3,
                            Name = "The Clan of Saar"
                        },
                        new
                        {
                            Id = 4,
                            Name = "The Embers of Muaat",
                            RussianName = "Тлеющие с Муаата"
                        },
                        new
                        {
                            Id = 5,
                            Name = "The Emirates of Hacan"
                        },
                        new
                        {
                            Id = 6,
                            Name = "The Federation of Sol"
                        },
                        new
                        {
                            Id = 7,
                            Name = "The Ghosts of Creuss",
                            RussianName = "Призраки Креусса"
                        },
                        new
                        {
                            Id = 8,
                            Name = "The L1Z1X Mindnet"
                        },
                        new
                        {
                            Id = 9,
                            Name = "The Mentak Coalition"
                        },
                        new
                        {
                            Id = 10,
                            Name = "The Naalu Collective"
                        },
                        new
                        {
                            Id = 11,
                            Name = "The Nekro Virus",
                            RussianName = "Некровирус"
                        },
                        new
                        {
                            Id = 12,
                            Name = "Sardakk N’orr"
                        },
                        new
                        {
                            Id = 13,
                            Name = "The Universities of Jol-Nar",
                            RussianName = "Университеты Жол-Нара"
                        },
                        new
                        {
                            Id = 14,
                            Name = "The Winnu"
                        },
                        new
                        {
                            Id = 15,
                            Name = "The Xxcha Kingdom"
                        },
                        new
                        {
                            Id = 16,
                            Name = "The Yin Brotherhood"
                        },
                        new
                        {
                            Id = 17,
                            Name = "The Yssaril Tribes"
                        },
                        new
                        {
                            Id = 18,
                            Name = "The Argent Flight"
                        },
                        new
                        {
                            Id = 19,
                            Name = "The Empyrean"
                        },
                        new
                        {
                            Id = 20,
                            Name = "The Mahact Gene-Sorcerers"
                        },
                        new
                        {
                            Id = 21,
                            Name = "The Naaz-Rokha Alliance"
                        },
                        new
                        {
                            Id = 22,
                            Name = "The Nomad",
                            RussianName = "Кочевник"
                        },
                        new
                        {
                            Id = 23,
                            Name = "The Titans of Ul",
                            RussianName = "Титаны Ула"
                        },
                        new
                        {
                            Id = 24,
                            Name = "The Vuil'Raith Cabal",
                            RussianName = "Кабал вуил’райт)"
                        });
                });

            modelBuilder.Entity("Twilight.Domain.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.HasKey("Id");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Twilight.Domain.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsVisualImpaired")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Twilight.Domain.PlayerSlot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Color")
                        .HasColumnType("integer");

                    b.Property<int>("GameId")
                        .HasColumnType("integer");

                    b.Property<int>("PlayerId")
                        .HasColumnType("integer");

                    b.Property<int?>("SelectedFactionId")
                        .HasColumnType("integer");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("PlayerId");

                    b.HasIndex("SelectedFactionId");

                    b.ToTable("PlayerSlot");
                });

            modelBuilder.Entity("FactionPlayerSlot", b =>
                {
                    b.HasOne("Twilight.Domain.PlayerSlot", null)
                        .WithMany()
                        .HasForeignKey("PlayerSlotId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Twilight.Domain.Faction", null)
                        .WithMany()
                        .HasForeignKey("PossibleFactionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Twilight.Domain.PlayerSlot", b =>
                {
                    b.HasOne("Twilight.Domain.Game", "Game")
                        .WithMany("PlayerSlots")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Twilight.Domain.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Twilight.Domain.Faction", "SelectedFaction")
                        .WithMany()
                        .HasForeignKey("SelectedFactionId");

                    b.Navigation("Game");

                    b.Navigation("Player");

                    b.Navigation("SelectedFaction");
                });

            modelBuilder.Entity("Twilight.Domain.Game", b =>
                {
                    b.Navigation("PlayerSlots");
                });
#pragma warning restore 612, 618
        }
    }
}
