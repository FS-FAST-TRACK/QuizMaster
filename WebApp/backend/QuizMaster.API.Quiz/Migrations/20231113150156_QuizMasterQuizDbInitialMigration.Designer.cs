﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QuizMaster.API.Quiz.DbContexts;

#nullable disable

namespace QuizMaster.API.Quiz.Migrations
{
    [DbContext(typeof(QuestionDbContext))]
    [Migration("20231113150156_QuizMasterQuizDbInitialMigration")]
    partial class QuizMasterQuizDbInitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("QuizMaster.Library.Common.Entities.Questionnaire.Detail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("ActiveData")
                        .HasColumnType("bit");

                    b.Property<int>("CreatedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("DetailDesc")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UpdatedByUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Details");
                });

            modelBuilder.Entity("QuizMaster.Library.Common.Entities.Questionnaire.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("ActiveData")
                        .HasColumnType("bit");

                    b.Property<int>("CreatedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("QAudio")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("QCategoryId")
                        .HasColumnType("int");

                    b.Property<int>("QDifficultyId")
                        .HasColumnType("int");

                    b.Property<string>("QImage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QStatement")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("QTime")
                        .HasColumnType("int");

                    b.Property<int>("QTypeId")
                        .HasColumnType("int");

                    b.Property<int?>("UpdatedByUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("QCategoryId");

                    b.HasIndex("QDifficultyId");

                    b.HasIndex("QTypeId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("QuizMaster.Library.Common.Entities.Questionnaire.QuestionCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("ActiveData")
                        .HasColumnType("bit");

                    b.Property<int>("CreatedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("QCategoryDesc")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("UpdatedByUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ActiveData = true,
                            CreatedByUserId = 1,
                            DateCreated = new DateTime(2023, 11, 13, 23, 1, 56, 334, DateTimeKind.Local).AddTicks(289),
                            DateUpdated = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            QCategoryDesc = "Science"
                        },
                        new
                        {
                            Id = 2,
                            ActiveData = true,
                            CreatedByUserId = 1,
                            DateCreated = new DateTime(2023, 11, 13, 23, 1, 56, 334, DateTimeKind.Local).AddTicks(1659),
                            DateUpdated = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            QCategoryDesc = "Movies"
                        },
                        new
                        {
                            Id = 3,
                            ActiveData = true,
                            CreatedByUserId = 1,
                            DateCreated = new DateTime(2023, 11, 13, 23, 1, 56, 334, DateTimeKind.Local).AddTicks(1664),
                            DateUpdated = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            QCategoryDesc = "Animals"
                        },
                        new
                        {
                            Id = 4,
                            ActiveData = true,
                            CreatedByUserId = 1,
                            DateCreated = new DateTime(2023, 11, 13, 23, 1, 56, 334, DateTimeKind.Local).AddTicks(1666),
                            DateUpdated = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            QCategoryDesc = "Places"
                        },
                        new
                        {
                            Id = 5,
                            ActiveData = true,
                            CreatedByUserId = 1,
                            DateCreated = new DateTime(2023, 11, 13, 23, 1, 56, 334, DateTimeKind.Local).AddTicks(1667),
                            DateUpdated = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            QCategoryDesc = "People"
                        },
                        new
                        {
                            Id = 6,
                            ActiveData = true,
                            CreatedByUserId = 1,
                            DateCreated = new DateTime(2023, 11, 13, 23, 1, 56, 334, DateTimeKind.Local).AddTicks(1675),
                            DateUpdated = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            QCategoryDesc = "System Operations and Maintenance"
                        },
                        new
                        {
                            Id = 7,
                            ActiveData = true,
                            CreatedByUserId = 1,
                            DateCreated = new DateTime(2023, 11, 13, 23, 1, 56, 334, DateTimeKind.Local).AddTicks(1676),
                            DateUpdated = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            QCategoryDesc = "Data Structures"
                        },
                        new
                        {
                            Id = 8,
                            ActiveData = true,
                            CreatedByUserId = 1,
                            DateCreated = new DateTime(2023, 11, 13, 23, 1, 56, 334, DateTimeKind.Local).AddTicks(1677),
                            DateUpdated = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            QCategoryDesc = "Algorithms"
                        });
                });

            modelBuilder.Entity("QuizMaster.Library.Common.Entities.Questionnaire.QuestionDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("ActiveData")
                        .HasColumnType("bit");

                    b.Property<int>("CreatedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<int>("DetailId")
                        .HasColumnType("int");

                    b.Property<int>("QuestionDetailTypeId")
                        .HasColumnType("int");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.Property<int?>("UpdatedByUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DetailId");

                    b.HasIndex("QuestionDetailTypeId");

                    b.HasIndex("QuestionId");

                    b.ToTable("QuestionDetails");
                });

            modelBuilder.Entity("QuizMaster.Library.Common.Entities.Questionnaire.QuestionDetailType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("ActiveData")
                        .HasColumnType("bit");

                    b.Property<int>("CreatedByUserId")
                        .HasColumnType("int");

                    b.Property<string>("DTypeDesc")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UpdatedByUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("DetailTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ActiveData = true,
                            CreatedByUserId = 1,
                            DTypeDesc = "Answer",
                            DateCreated = new DateTime(2023, 11, 13, 23, 1, 56, 334, DateTimeKind.Local).AddTicks(5944),
                            DateUpdated = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 2,
                            ActiveData = true,
                            CreatedByUserId = 1,
                            DTypeDesc = "Option",
                            DateCreated = new DateTime(2023, 11, 13, 23, 1, 56, 334, DateTimeKind.Local).AddTicks(6715),
                            DateUpdated = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 3,
                            ActiveData = true,
                            CreatedByUserId = 1,
                            DTypeDesc = "Minimum",
                            DateCreated = new DateTime(2023, 11, 13, 23, 1, 56, 334, DateTimeKind.Local).AddTicks(6719),
                            DateUpdated = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 4,
                            ActiveData = true,
                            CreatedByUserId = 1,
                            DTypeDesc = "Maximum",
                            DateCreated = new DateTime(2023, 11, 13, 23, 1, 56, 334, DateTimeKind.Local).AddTicks(6720),
                            DateUpdated = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 5,
                            ActiveData = true,
                            CreatedByUserId = 1,
                            DTypeDesc = "Interval",
                            DateCreated = new DateTime(2023, 11, 13, 23, 1, 56, 334, DateTimeKind.Local).AddTicks(6720),
                            DateUpdated = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 6,
                            ActiveData = true,
                            CreatedByUserId = 1,
                            DTypeDesc = "Margin",
                            DateCreated = new DateTime(2023, 11, 13, 23, 1, 56, 334, DateTimeKind.Local).AddTicks(6721),
                            DateUpdated = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 7,
                            ActiveData = true,
                            CreatedByUserId = 1,
                            DTypeDesc = "TextToAudio",
                            DateCreated = new DateTime(2023, 11, 13, 23, 1, 56, 334, DateTimeKind.Local).AddTicks(6722),
                            DateUpdated = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 8,
                            ActiveData = true,
                            CreatedByUserId = 1,
                            DTypeDesc = "Language",
                            DateCreated = new DateTime(2023, 11, 13, 23, 1, 56, 334, DateTimeKind.Local).AddTicks(6723),
                            DateUpdated = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("QuizMaster.Library.Common.Entities.Questionnaire.QuestionDifficulty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("ActiveData")
                        .HasColumnType("bit");

                    b.Property<int>("CreatedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("QDifficultyDesc")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<int?>("UpdatedByUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Difficulties");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ActiveData = true,
                            CreatedByUserId = 1,
                            DateCreated = new DateTime(2023, 11, 13, 23, 1, 56, 333, DateTimeKind.Local).AddTicks(2307),
                            DateUpdated = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            QDifficultyDesc = "Easy"
                        },
                        new
                        {
                            Id = 2,
                            ActiveData = true,
                            CreatedByUserId = 1,
                            DateCreated = new DateTime(2023, 11, 13, 23, 1, 56, 333, DateTimeKind.Local).AddTicks(3675),
                            DateUpdated = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            QDifficultyDesc = "Average"
                        },
                        new
                        {
                            Id = 3,
                            ActiveData = true,
                            CreatedByUserId = 1,
                            DateCreated = new DateTime(2023, 11, 13, 23, 1, 56, 333, DateTimeKind.Local).AddTicks(3751),
                            DateUpdated = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            QDifficultyDesc = "Difficult"
                        });
                });

            modelBuilder.Entity("QuizMaster.Library.Common.Entities.Questionnaire.QuestionType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("ActiveData")
                        .HasColumnType("bit");

                    b.Property<int>("CreatedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<bool>("QDetailRequired")
                        .HasColumnType("bit");

                    b.Property<string>("QTypeDesc")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("UpdatedByUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Types");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ActiveData = true,
                            CreatedByUserId = 1,
                            DateCreated = new DateTime(2023, 11, 13, 23, 1, 56, 329, DateTimeKind.Local).AddTicks(6012),
                            DateUpdated = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            QDetailRequired = true,
                            QTypeDesc = "Multiple Choice"
                        },
                        new
                        {
                            Id = 2,
                            ActiveData = true,
                            CreatedByUserId = 1,
                            DateCreated = new DateTime(2023, 11, 13, 23, 1, 56, 330, DateTimeKind.Local).AddTicks(8490),
                            DateUpdated = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            QDetailRequired = true,
                            QTypeDesc = "Multiple Choice + Audio"
                        },
                        new
                        {
                            Id = 3,
                            ActiveData = true,
                            CreatedByUserId = 1,
                            DateCreated = new DateTime(2023, 11, 13, 23, 1, 56, 330, DateTimeKind.Local).AddTicks(8512),
                            DateUpdated = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            QDetailRequired = false,
                            QTypeDesc = "True or False"
                        },
                        new
                        {
                            Id = 4,
                            ActiveData = true,
                            CreatedByUserId = 1,
                            DateCreated = new DateTime(2023, 11, 13, 23, 1, 56, 330, DateTimeKind.Local).AddTicks(8514),
                            DateUpdated = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            QDetailRequired = true,
                            QTypeDesc = "Type Answer"
                        },
                        new
                        {
                            Id = 5,
                            ActiveData = true,
                            CreatedByUserId = 1,
                            DateCreated = new DateTime(2023, 11, 13, 23, 1, 56, 330, DateTimeKind.Local).AddTicks(8515),
                            DateUpdated = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            QDetailRequired = true,
                            QTypeDesc = "Slider"
                        },
                        new
                        {
                            Id = 6,
                            ActiveData = true,
                            CreatedByUserId = 1,
                            DateCreated = new DateTime(2023, 11, 13, 23, 1, 56, 330, DateTimeKind.Local).AddTicks(8516),
                            DateUpdated = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            QDetailRequired = true,
                            QTypeDesc = "Puzzle"
                        });
                });

            modelBuilder.Entity("QuizMaster.Library.Common.Entities.Questionnaire.Question", b =>
                {
                    b.HasOne("QuizMaster.Library.Common.Entities.Questionnaire.QuestionCategory", "QCategory")
                        .WithMany()
                        .HasForeignKey("QCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QuizMaster.Library.Common.Entities.Questionnaire.QuestionDifficulty", "QDifficulty")
                        .WithMany()
                        .HasForeignKey("QDifficultyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QuizMaster.Library.Common.Entities.Questionnaire.QuestionType", "QType")
                        .WithMany()
                        .HasForeignKey("QTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("QCategory");

                    b.Navigation("QDifficulty");

                    b.Navigation("QType");
                });

            modelBuilder.Entity("QuizMaster.Library.Common.Entities.Questionnaire.QuestionDetail", b =>
                {
                    b.HasOne("QuizMaster.Library.Common.Entities.Questionnaire.Detail", "Detail")
                        .WithMany()
                        .HasForeignKey("DetailId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QuizMaster.Library.Common.Entities.Questionnaire.QuestionDetailType", "QuestionDetailType")
                        .WithMany()
                        .HasForeignKey("QuestionDetailTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QuizMaster.Library.Common.Entities.Questionnaire.Question", "Question")
                        .WithMany("Details")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Detail");

                    b.Navigation("Question");

                    b.Navigation("QuestionDetailType");
                });

            modelBuilder.Entity("QuizMaster.Library.Common.Entities.Questionnaire.Question", b =>
                {
                    b.Navigation("Details");
                });
#pragma warning restore 612, 618
        }
    }
}