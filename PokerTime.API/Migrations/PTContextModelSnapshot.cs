﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PokerTime.API.Data;

namespace PokerTime.API.Migrations
{
    [DbContext(typeof(PTContext))]
    partial class PTContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.8")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EventInvitee", b =>
                {
                    b.Property<int>("EventsId")
                        .HasColumnType("int");

                    b.Property<int>("InviteesId")
                        .HasColumnType("int");

                    b.HasKey("EventsId", "InviteesId");

                    b.HasIndex("InviteesId");

                    b.ToTable("EventInvitee");
                });

            modelBuilder.Entity("PokerTime.Shared.Entities.BlindLevel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Ante")
                        .HasColumnType("int");

                    b.Property<int>("BigBlind")
                        .HasColumnType("int");

                    b.Property<int>("Minutes")
                        .HasColumnType("int");

                    b.Property<int>("SequenceNum")
                        .HasColumnType("int");

                    b.Property<int>("SmallBlind")
                        .HasColumnType("int");

                    b.Property<int>("TournamentStructureId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TournamentStructureId");

                    b.ToTable("BlindLevels");
                });

            modelBuilder.Entity("PokerTime.Shared.Entities.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("EventLinkId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("HostId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasMaxLength(75)
                        .HasColumnType("nvarchar(75)");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.Property<int>("TournamentStructureId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("HostId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("PokerTime.Shared.Entities.Host", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsPaidUser")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Hosts");

                    b.HasData(
                        new
                        {
                            Id = new Guid("e0eab422-54c4-4a3f-bdbd-0808cbd6a4ec"),
                            Email = "JimboSpain@gmail.com",
                            IsPaidUser = true,
                            Name = "Jim Spain",
                            Phone = "0987654321"
                        });
                });

            modelBuilder.Entity("PokerTime.Shared.Entities.Invitee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("HostId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("HostId");

                    b.ToTable("Invitees");
                });

            modelBuilder.Entity("PokerTime.Shared.Entities.TournamentStructure", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("HostId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("NumberOfEvents")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("HostId");

                    b.ToTable("TournamentStructures");
                });

            modelBuilder.Entity("PokerTime.Shared.Entities.TournamentTracking", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("CurrentBlindLevelId")
                        .HasColumnType("int");

                    b.Property<bool>("IsTimerRunning")
                        .HasColumnType("bit");

                    b.Property<bool>("IsTournamentRunning")
                        .HasColumnType("bit");

                    b.Property<int?>("NextBlindLevelId")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeRemaining")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CurrentBlindLevelId");

                    b.HasIndex("NextBlindLevelId");

                    b.ToTable("TournamentTrackings");
                });

            modelBuilder.Entity("EventInvitee", b =>
                {
                    b.HasOne("PokerTime.Shared.Entities.Event", null)
                        .WithMany()
                        .HasForeignKey("EventsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PokerTime.Shared.Entities.Invitee", null)
                        .WithMany()
                        .HasForeignKey("InviteesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PokerTime.Shared.Entities.BlindLevel", b =>
                {
                    b.HasOne("PokerTime.Shared.Entities.TournamentStructure", null)
                        .WithMany("BlindLevels")
                        .HasForeignKey("TournamentStructureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PokerTime.Shared.Entities.Event", b =>
                {
                    b.HasOne("PokerTime.Shared.Entities.Host", null)
                        .WithMany("Events")
                        .HasForeignKey("HostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PokerTime.Shared.Entities.Invitee", b =>
                {
                    b.HasOne("PokerTime.Shared.Entities.Host", null)
                        .WithMany("Invitees")
                        .HasForeignKey("HostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PokerTime.Shared.Entities.TournamentStructure", b =>
                {
                    b.HasOne("PokerTime.Shared.Entities.Host", null)
                        .WithMany("TournamentStructures")
                        .HasForeignKey("HostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PokerTime.Shared.Entities.TournamentTracking", b =>
                {
                    b.HasOne("PokerTime.Shared.Entities.BlindLevel", "CurrentBlindLevel")
                        .WithMany()
                        .HasForeignKey("CurrentBlindLevelId");

                    b.HasOne("PokerTime.Shared.Entities.BlindLevel", "NextBlindLevel")
                        .WithMany()
                        .HasForeignKey("NextBlindLevelId");

                    b.Navigation("CurrentBlindLevel");

                    b.Navigation("NextBlindLevel");
                });

            modelBuilder.Entity("PokerTime.Shared.Entities.Host", b =>
                {
                    b.Navigation("Events");

                    b.Navigation("Invitees");

                    b.Navigation("TournamentStructures");
                });

            modelBuilder.Entity("PokerTime.Shared.Entities.TournamentStructure", b =>
                {
                    b.Navigation("BlindLevels");
                });
#pragma warning restore 612, 618
        }
    }
}
