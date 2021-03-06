// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WineClub.DataAccess;

namespace WineClub.DataAccess.Migrations
{
    [DbContext(typeof(WineDbContext))]
    [Migration("20220520101528_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GrapeWine", b =>
                {
                    b.Property<int>("GrapesGrapeId")
                        .HasColumnType("int");

                    b.Property<int>("WinesWineId")
                        .HasColumnType("int");

                    b.HasKey("GrapesGrapeId", "WinesWineId");

                    b.HasIndex("WinesWineId");

                    b.ToTable("GrapeWine");
                });

            modelBuilder.Entity("RatingWine", b =>
                {
                    b.Property<int>("RatingsRatingId")
                        .HasColumnType("int");

                    b.Property<int>("WinesWineId")
                        .HasColumnType("int");

                    b.HasKey("RatingsRatingId", "WinesWineId");

                    b.HasIndex("WinesWineId");

                    b.ToTable("RatingWine");
                });

            modelBuilder.Entity("RegionWine", b =>
                {
                    b.Property<int>("RegionsRegionId")
                        .HasColumnType("int");

                    b.Property<int>("WinesWineId")
                        .HasColumnType("int");

                    b.HasKey("RegionsRegionId", "WinesWineId");

                    b.HasIndex("WinesWineId");

                    b.ToTable("RegionWine");
                });

            modelBuilder.Entity("UserWineEvent", b =>
                {
                    b.Property<int>("AttendeesUserId")
                        .HasColumnType("int");

                    b.Property<int>("WineEventsWineEventId")
                        .HasColumnType("int");

                    b.HasKey("AttendeesUserId", "WineEventsWineEventId");

                    b.HasIndex("WineEventsWineEventId");

                    b.ToTable("UserWineEvent");
                });

            modelBuilder.Entity("WineClub.DataAccess.Model.Grape", b =>
                {
                    b.Property<int>("GrapeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("GrapeName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("GrapeId");

                    b.ToTable("Grapes");
                });

            modelBuilder.Entity("WineClub.DataAccess.Model.Rating", b =>
                {
                    b.Property<int>("RatingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("DateOfRating")
                        .HasColumnType("datetimeoffset");

                    b.Property<int?>("RatedByUserId")
                        .HasColumnType("int");

                    b.Property<string>("ReviewText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.HasKey("RatingId");

                    b.HasIndex("RatedByUserId");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("WineClub.DataAccess.Model.Region", b =>
                {
                    b.Property<int>("RegionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("RegionName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RegionId");

                    b.ToTable("Regions");
                });

            modelBuilder.Entity("WineClub.DataAccess.Model.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WineClub.DataAccess.Model.Wine", b =>
                {
                    b.Property<int>("WineId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AddedByUserId")
                        .HasColumnType("int");

                    b.Property<double>("AlcoholContent")
                        .HasColumnType("float");

                    b.Property<byte[]>("Image")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WineType")
                        .HasColumnType("int");

                    b.Property<int?>("WineryId")
                        .HasColumnType("int");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("WineId");

                    b.HasIndex("AddedByUserId");

                    b.HasIndex("WineryId");

                    b.ToTable("Wines");
                });

            modelBuilder.Entity("WineClub.DataAccess.Model.WineEvent", b =>
                {
                    b.Property<int>("WineEventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("DateAndTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MaxPersons")
                        .HasColumnType("int");

                    b.Property<string>("StreetAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("WineEventId");

                    b.ToTable("WineEvents");
                });

            modelBuilder.Entity("WineClub.DataAccess.Model.Winery", b =>
                {
                    b.Property<int>("WineryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("WineryId");

                    b.ToTable("Wineries");
                });

            modelBuilder.Entity("WineWineEvent", b =>
                {
                    b.Property<int>("EventsWineEventId")
                        .HasColumnType("int");

                    b.Property<int>("WinesWineId")
                        .HasColumnType("int");

                    b.HasKey("EventsWineEventId", "WinesWineId");

                    b.HasIndex("WinesWineId");

                    b.ToTable("WineWineEvent");
                });

            modelBuilder.Entity("GrapeWine", b =>
                {
                    b.HasOne("WineClub.DataAccess.Model.Grape", null)
                        .WithMany()
                        .HasForeignKey("GrapesGrapeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WineClub.DataAccess.Model.Wine", null)
                        .WithMany()
                        .HasForeignKey("WinesWineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RatingWine", b =>
                {
                    b.HasOne("WineClub.DataAccess.Model.Rating", null)
                        .WithMany()
                        .HasForeignKey("RatingsRatingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WineClub.DataAccess.Model.Wine", null)
                        .WithMany()
                        .HasForeignKey("WinesWineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RegionWine", b =>
                {
                    b.HasOne("WineClub.DataAccess.Model.Region", null)
                        .WithMany()
                        .HasForeignKey("RegionsRegionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WineClub.DataAccess.Model.Wine", null)
                        .WithMany()
                        .HasForeignKey("WinesWineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("UserWineEvent", b =>
                {
                    b.HasOne("WineClub.DataAccess.Model.User", null)
                        .WithMany()
                        .HasForeignKey("AttendeesUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WineClub.DataAccess.Model.WineEvent", null)
                        .WithMany()
                        .HasForeignKey("WineEventsWineEventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WineClub.DataAccess.Model.Rating", b =>
                {
                    b.HasOne("WineClub.DataAccess.Model.User", "RatedBy")
                        .WithMany()
                        .HasForeignKey("RatedByUserId");

                    b.Navigation("RatedBy");
                });

            modelBuilder.Entity("WineClub.DataAccess.Model.Wine", b =>
                {
                    b.HasOne("WineClub.DataAccess.Model.User", "AddedBy")
                        .WithMany("Wines")
                        .HasForeignKey("AddedByUserId");

                    b.HasOne("WineClub.DataAccess.Model.Winery", "Winery")
                        .WithMany("Wines")
                        .HasForeignKey("WineryId");

                    b.Navigation("AddedBy");

                    b.Navigation("Winery");
                });

            modelBuilder.Entity("WineWineEvent", b =>
                {
                    b.HasOne("WineClub.DataAccess.Model.WineEvent", null)
                        .WithMany()
                        .HasForeignKey("EventsWineEventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WineClub.DataAccess.Model.Wine", null)
                        .WithMany()
                        .HasForeignKey("WinesWineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WineClub.DataAccess.Model.User", b =>
                {
                    b.Navigation("Wines");
                });

            modelBuilder.Entity("WineClub.DataAccess.Model.Winery", b =>
                {
                    b.Navigation("Wines");
                });
#pragma warning restore 612, 618
        }
    }
}
