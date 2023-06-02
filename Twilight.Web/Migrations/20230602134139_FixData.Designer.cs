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
    [Migration("20230602134139_FixData")]
    partial class FixData
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
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

                    b.Property<string>("WikiLink")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Factions");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "The Arborec",
                            RussianName = "Арбореки",
                            WikiLink = "https://twilight-imperium.fandom.com/wiki/The_Arborec"
                        },
                        new
                        {
                            Id = 2,
                            Name = "The Barony of Letnev",
                            RussianName = "Баронат Летнев",
                            WikiLink = "https://twilight-imperium.fandom.com/wiki/The_Barony_of_Letnev"
                        },
                        new
                        {
                            Id = 3,
                            Name = "The Clan of Saar",
                            RussianName = "Клан Сааров",
                            WikiLink = "https://twilight-imperium.fandom.com/wiki/The_Clan_of_Saar"
                        },
                        new
                        {
                            Id = 4,
                            Name = "The Embers of Muaat",
                            RussianName = "Тлеющие с Муаата",
                            WikiLink = "https://twilight-imperium.fandom.com/wiki/The_Embers_of_Muaat"
                        },
                        new
                        {
                            Id = 5,
                            Name = "The Emirates of Hacan",
                            RussianName = "Хаканские Эмираты",
                            WikiLink = "https://twilight-imperium.fandom.com/wiki/The_Emirates_of_Hacan"
                        },
                        new
                        {
                            Id = 6,
                            Name = "The Federation of Sol",
                            RussianName = "Федерация Сол",
                            WikiLink = "https://twilight-imperium.fandom.com/wiki/The_Federation_of_Sol"
                        },
                        new
                        {
                            Id = 7,
                            Name = "The Ghosts of Creuss",
                            RussianName = "Призраки Креусса",
                            WikiLink = "https://twilight-imperium.fandom.com/wiki/The_Ghosts_of_Creuss"
                        },
                        new
                        {
                            Id = 8,
                            Name = "The L1Z1X Mindnet",
                            RussianName = "Психосеть Л131КС",
                            WikiLink = "https://twilight-imperium.fandom.com/wiki/The_L1Z1X_Mindnet"
                        },
                        new
                        {
                            Id = 9,
                            Name = "The Mentak Coalition",
                            RussianName = "Коалиция Ментака",
                            WikiLink = "https://twilight-imperium.fandom.com/wiki/The_Mentak_Coalition"
                        },
                        new
                        {
                            Id = 10,
                            Name = "The Naalu Collective",
                            RussianName = "Клубок Наалу",
                            WikiLink = "https://twilight-imperium.fandom.com/wiki/The_Naalu_Collective"
                        },
                        new
                        {
                            Id = 11,
                            Name = "The Nekro Virus",
                            RussianName = "Некровирус",
                            WikiLink = "https://twilight-imperium.fandom.com/wiki/The_Nekro_Virus"
                        },
                        new
                        {
                            Id = 12,
                            Name = "Sardakk N’orr",
                            RussianName = "Сардак Н'орр",
                            WikiLink = "https://twilight-imperium.fandom.com/wiki/Sardakk_N%27orr"
                        },
                        new
                        {
                            Id = 13,
                            Name = "The Universities of Jol-Nar",
                            RussianName = "Жол-Нарские университеты",
                            WikiLink = "https://twilight-imperium.fandom.com/wiki/The_Universities_of_Jol-Nar"
                        },
                        new
                        {
                            Id = 14,
                            Name = "The Winnu",
                            RussianName = "Винну",
                            WikiLink = "https://twilight-imperium.fandom.com/wiki/The_Winnu"
                        },
                        new
                        {
                            Id = 15,
                            Name = "The Xxcha Kingdom",
                            RussianName = "Королевство Ззча",
                            WikiLink = "https://twilight-imperium.fandom.com/wiki/The_Xxcha_Kingdom"
                        },
                        new
                        {
                            Id = 16,
                            Name = "The Yin Brotherhood",
                            RussianName = "Братство Инь",
                            WikiLink = "https://twilight-imperium.fandom.com/wiki/The_Yin_Brotherhood"
                        },
                        new
                        {
                            Id = 17,
                            Name = "The Yssaril Tribes",
                            RussianName = "Племена Иссарилов",
                            WikiLink = "https://twilight-imperium.fandom.com/wiki/The_Yssaril_Tribes"
                        },
                        new
                        {
                            Id = 18,
                            Name = "The Argent Flight",
                            RussianName = "Серебряная стая",
                            WikiLink = "https://twilight-imperium.fandom.com/wiki/The_Argent_Flight"
                        },
                        new
                        {
                            Id = 19,
                            Name = "The Empyrean",
                            RussianName = "Возвышенные",
                            WikiLink = "https://twilight-imperium.fandom.com/wiki/The_Empyrean"
                        },
                        new
                        {
                            Id = 20,
                            Name = "The Mahact Gene-Sorcerers",
                            RussianName = "Генные чародеи Мэхакт",
                            WikiLink = "https://twilight-imperium.fandom.com/wiki/The_Mahact_Gene-Sorcerers"
                        },
                        new
                        {
                            Id = 21,
                            Name = "The Naaz-Rokha Alliance",
                            RussianName = "Альянс Нааз-Роха",
                            WikiLink = "https://twilight-imperium.fandom.com/wiki/The_Naaz-Rokha_Alliance"
                        },
                        new
                        {
                            Id = 22,
                            Name = "The Nomad",
                            RussianName = "Кочевник",
                            WikiLink = "https://twilight-imperium.fandom.com/wiki/The_Nomad"
                        },
                        new
                        {
                            Id = 23,
                            Name = "The Titans of Ul",
                            RussianName = "Титаны Ула",
                            WikiLink = "https://twilight-imperium.fandom.com/wiki/The_Titans_of_Ul"
                        },
                        new
                        {
                            Id = 24,
                            Name = "The Vuil'Raith Cabal",
                            RussianName = "Кабал вуил’райт",
                            WikiLink = "https://twilight-imperium.fandom.com/wiki/The_Vuil%27Raith_Cabal"
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

                    b.Property<string>("AlliedWith")
                        .HasColumnType("text");

                    b.Property<bool>("ChoosePlace")
                        .HasColumnType("boolean");

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

                    b.Property<bool>("Speaker")
                        .HasColumnType("boolean");

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
