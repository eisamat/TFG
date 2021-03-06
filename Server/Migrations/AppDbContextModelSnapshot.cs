// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Server.Database;

namespace Server.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PatientVideo", b =>
                {
                    b.Property<Guid>("AssignersId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("VideosId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("AssignersId", "VideosId");

                    b.HasIndex("VideosId");

                    b.ToTable("PatientVideo");
                });

            modelBuilder.Entity("Server.Database.Models.AssignmentRecord", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AssignmentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("PatientId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("VideoId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("PatientId");

                    b.HasIndex("VideoId");

                    b.ToTable("AssignmentRecords");
                });

            modelBuilder.Entity("Server.Database.Models.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Server.Database.Models.Message", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsFromPatient")
                        .HasColumnType("bit");

                    b.Property<Guid?>("PatientId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("TherapistId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CreatedAt");

                    b.HasIndex("PatientId");

                    b.HasIndex("TherapistId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Server.Database.Models.Patient", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nhc")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid?>("TherapistId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Zip")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Nhc")
                        .IsUnique()
                        .HasFilter("[Nhc] IS NOT NULL");

                    b.HasIndex("TherapistId");

                    b.HasIndex("Token")
                        .IsUnique()
                        .HasFilter("[Token] IS NOT NULL");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("Server.Database.Models.Therapist", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Therapists");
                });

            modelBuilder.Entity("Server.Database.Models.Video", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("YoutubeId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("Name");

                    b.HasIndex("YoutubeId")
                        .IsUnique()
                        .HasFilter("[YoutubeId] IS NOT NULL");

                    b.ToTable("Videos");
                });

            modelBuilder.Entity("PatientVideo", b =>
                {
                    b.HasOne("Server.Database.Models.Patient", null)
                        .WithMany()
                        .HasForeignKey("AssignersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Server.Database.Models.Video", null)
                        .WithMany()
                        .HasForeignKey("VideosId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Server.Database.Models.AssignmentRecord", b =>
                {
                    b.HasOne("Server.Database.Models.Patient", "Patient")
                        .WithMany("Records")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Server.Database.Models.Video", "Video")
                        .WithMany("Records")
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Patient");

                    b.Navigation("Video");
                });

            modelBuilder.Entity("Server.Database.Models.Message", b =>
                {
                    b.HasOne("Server.Database.Models.Patient", "Patient")
                        .WithMany()
                        .HasForeignKey("PatientId");

                    b.HasOne("Server.Database.Models.Therapist", "Therapist")
                        .WithMany()
                        .HasForeignKey("TherapistId");

                    b.Navigation("Patient");

                    b.Navigation("Therapist");
                });

            modelBuilder.Entity("Server.Database.Models.Patient", b =>
                {
                    b.HasOne("Server.Database.Models.Therapist", "Therapist")
                        .WithMany("Patients")
                        .HasForeignKey("TherapistId");

                    b.Navigation("Therapist");
                });

            modelBuilder.Entity("Server.Database.Models.Video", b =>
                {
                    b.HasOne("Server.Database.Models.Category", "Category")
                        .WithMany("Videos")
                        .HasForeignKey("CategoryId");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Server.Database.Models.Category", b =>
                {
                    b.Navigation("Videos");
                });

            modelBuilder.Entity("Server.Database.Models.Patient", b =>
                {
                    b.Navigation("Records");
                });

            modelBuilder.Entity("Server.Database.Models.Therapist", b =>
                {
                    b.Navigation("Patients");
                });

            modelBuilder.Entity("Server.Database.Models.Video", b =>
                {
                    b.Navigation("Records");
                });
#pragma warning restore 612, 618
        }
    }
}
