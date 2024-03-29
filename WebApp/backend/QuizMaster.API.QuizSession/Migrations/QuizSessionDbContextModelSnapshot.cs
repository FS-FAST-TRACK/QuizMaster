﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QuizMaster.API.QuizSession.DbContexts;

#nullable disable

namespace QuizMaster.API.QuizSession.Migrations
{
    [DbContext(typeof(QuizSessionDbContext))]
    partial class QuizSessionDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("QuizMaster.Library.Common.Entities.Questionnaire.DetailType", b =>
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

                    b.Property<DateTime?>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<int?>("QuestionDetailId")
                        .HasColumnType("int");

                    b.Property<int?>("UpdatedByUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("QuestionDetailId");

                    b.ToTable("DetailTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ActiveData = false,
                            CreatedByUserId = 0,
                            DTypeDesc = "Answer",
                            DateCreated = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 2,
                            ActiveData = false,
                            CreatedByUserId = 0,
                            DTypeDesc = "Option",
                            DateCreated = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 3,
                            ActiveData = false,
                            CreatedByUserId = 0,
                            DTypeDesc = "Minimum",
                            DateCreated = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 4,
                            ActiveData = false,
                            CreatedByUserId = 0,
                            DTypeDesc = "Maximum",
                            DateCreated = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 5,
                            ActiveData = false,
                            CreatedByUserId = 0,
                            DTypeDesc = "Interval",
                            DateCreated = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 6,
                            ActiveData = false,
                            CreatedByUserId = 0,
                            DTypeDesc = "Margin",
                            DateCreated = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 7,
                            ActiveData = false,
                            CreatedByUserId = 0,
                            DTypeDesc = "TextToAudio",
                            DateCreated = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 8,
                            ActiveData = false,
                            CreatedByUserId = 0,
                            DTypeDesc = "Language",
                            DateCreated = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
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

                    b.Property<DateTime?>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("QAudio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("QCategoryId")
                        .HasColumnType("int");

                    b.Property<int>("QDifficultyId")
                        .HasColumnType("int");

                    b.Property<string>("QImage")
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

                    b.Property<DateTime?>("DateUpdated")
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
                            DateCreated = new DateTime(2024, 3, 5, 17, 51, 8, 170, DateTimeKind.Local).AddTicks(9500),
                            QCategoryDesc = "Science"
                        },
                        new
                        {
                            Id = 2,
                            ActiveData = true,
                            CreatedByUserId = 1,
                            DateCreated = new DateTime(2024, 3, 5, 17, 51, 8, 171, DateTimeKind.Local).AddTicks(366),
                            QCategoryDesc = "Movies"
                        },
                        new
                        {
                            Id = 3,
                            ActiveData = true,
                            CreatedByUserId = 1,
                            DateCreated = new DateTime(2024, 3, 5, 17, 51, 8, 171, DateTimeKind.Local).AddTicks(371),
                            QCategoryDesc = "Animals"
                        },
                        new
                        {
                            Id = 4,
                            ActiveData = true,
                            CreatedByUserId = 1,
                            DateCreated = new DateTime(2024, 3, 5, 17, 51, 8, 171, DateTimeKind.Local).AddTicks(372),
                            QCategoryDesc = "Places"
                        },
                        new
                        {
                            Id = 5,
                            ActiveData = true,
                            CreatedByUserId = 1,
                            DateCreated = new DateTime(2024, 3, 5, 17, 51, 8, 171, DateTimeKind.Local).AddTicks(374),
                            QCategoryDesc = "People"
                        },
                        new
                        {
                            Id = 6,
                            ActiveData = true,
                            CreatedByUserId = 1,
                            DateCreated = new DateTime(2024, 3, 5, 17, 51, 8, 171, DateTimeKind.Local).AddTicks(378),
                            QCategoryDesc = "System Operations and Maintenance"
                        },
                        new
                        {
                            Id = 7,
                            ActiveData = true,
                            CreatedByUserId = 1,
                            DateCreated = new DateTime(2024, 3, 5, 17, 51, 8, 171, DateTimeKind.Local).AddTicks(380),
                            QCategoryDesc = "Data Structures"
                        },
                        new
                        {
                            Id = 8,
                            ActiveData = true,
                            CreatedByUserId = 1,
                            DateCreated = new DateTime(2024, 3, 5, 17, 51, 8, 171, DateTimeKind.Local).AddTicks(382),
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

                    b.Property<DateTime?>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("QDetailDesc")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.Property<int?>("UpdatedByUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("QuestionDetails");
                });

            modelBuilder.Entity("QuizMaster.Library.Common.Entities.Questionnaire.QuestionDetailType", b =>
                {
                    b.Property<int>("QuestionDetailId")
                        .HasColumnType("int");

                    b.Property<int>("DetailTypeId")
                        .HasColumnType("int");

                    b.Property<bool>("ActiveData")
                        .HasColumnType("bit");

                    b.Property<int>("CreatedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UpdatedByUserId")
                        .HasColumnType("int");

                    b.HasKey("QuestionDetailId", "DetailTypeId");

                    b.HasIndex("DetailTypeId");

                    b.ToTable("QuestionDetailTypes");
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

                    b.Property<DateTime?>("DateUpdated")
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
                            DateCreated = new DateTime(2024, 3, 5, 17, 51, 8, 170, DateTimeKind.Local).AddTicks(4236),
                            QDifficultyDesc = "Easy"
                        },
                        new
                        {
                            Id = 2,
                            ActiveData = true,
                            CreatedByUserId = 1,
                            DateCreated = new DateTime(2024, 3, 5, 17, 51, 8, 170, DateTimeKind.Local).AddTicks(5120),
                            QDifficultyDesc = "Average"
                        },
                        new
                        {
                            Id = 3,
                            ActiveData = true,
                            CreatedByUserId = 1,
                            DateCreated = new DateTime(2024, 3, 5, 17, 51, 8, 170, DateTimeKind.Local).AddTicks(5182),
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

                    b.Property<DateTime?>("DateUpdated")
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
                            DateCreated = new DateTime(2024, 3, 5, 17, 51, 8, 167, DateTimeKind.Local).AddTicks(2555),
                            QDetailRequired = true,
                            QTypeDesc = "Multiple Choice"
                        },
                        new
                        {
                            Id = 2,
                            ActiveData = true,
                            CreatedByUserId = 1,
                            DateCreated = new DateTime(2024, 3, 5, 17, 51, 8, 169, DateTimeKind.Local).AddTicks(388),
                            QDetailRequired = true,
                            QTypeDesc = "Multiple Choice + Audio"
                        },
                        new
                        {
                            Id = 3,
                            ActiveData = true,
                            CreatedByUserId = 1,
                            DateCreated = new DateTime(2024, 3, 5, 17, 51, 8, 169, DateTimeKind.Local).AddTicks(399),
                            QDetailRequired = false,
                            QTypeDesc = "True or False"
                        },
                        new
                        {
                            Id = 4,
                            ActiveData = true,
                            CreatedByUserId = 1,
                            DateCreated = new DateTime(2024, 3, 5, 17, 51, 8, 169, DateTimeKind.Local).AddTicks(400),
                            QDetailRequired = true,
                            QTypeDesc = "Type Answer"
                        },
                        new
                        {
                            Id = 5,
                            ActiveData = true,
                            CreatedByUserId = 1,
                            DateCreated = new DateTime(2024, 3, 5, 17, 51, 8, 169, DateTimeKind.Local).AddTicks(402),
                            QDetailRequired = true,
                            QTypeDesc = "Slider"
                        },
                        new
                        {
                            Id = 6,
                            ActiveData = true,
                            CreatedByUserId = 1,
                            DateCreated = new DateTime(2024, 3, 5, 17, 51, 8, 169, DateTimeKind.Local).AddTicks(403),
                            QDetailRequired = true,
                            QTypeDesc = "Puzzle"
                        });
                });

            modelBuilder.Entity("QuizMaster.Library.Common.Entities.Rooms.QuestionSet", b =>
                {
                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.Property<int>("SetId")
                        .HasColumnType("int");

                    b.Property<bool>("ActiveData")
                        .HasColumnType("bit");

                    b.Property<int>("CreatedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UpdatedByUserId")
                        .HasColumnType("int");

                    b.HasKey("QuestionId", "SetId");

                    b.HasIndex("SetId");

                    b.ToTable("QuestionSets");
                });

            modelBuilder.Entity("QuizMaster.Library.Common.Entities.Rooms.QuizParticipant", b =>
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

                    b.Property<DateTime?>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("QEndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("QParticipantDesc")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("QRoomId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("QStartDate")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<bool>("QStatus")
                        .HasColumnType("bit");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.Property<int?>("UpdatedByUserId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("QuizParticipants");
                });

            modelBuilder.Entity("QuizMaster.Library.Common.Entities.Rooms.QuizRoom", b =>
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

                    b.Property<DateTime?>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("QRoomDesc")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("QRoomPin")
                        .HasColumnType("int");

                    b.Property<string>("RoomOptions")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UpdatedByUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("QuizRooms");
                });

            modelBuilder.Entity("QuizMaster.Library.Common.Entities.Rooms.QuizRoomData", b =>
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

                    b.Property<DateTime?>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EndedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("HostId")
                        .HasColumnType("int");

                    b.Property<string>("ParticipantsJSON")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("QRoomId")
                        .HasColumnType("int");

                    b.Property<string>("SessionId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SetQuizRoomJSON")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UpdatedByUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("QuizRoomDatas");
                });

            modelBuilder.Entity("QuizMaster.Library.Common.Entities.Rooms.Set", b =>
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

                    b.Property<DateTime?>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("QSetDesc")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QSetName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("UpdatedByUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Sets");
                });

            modelBuilder.Entity("QuizMaster.Library.Common.Entities.Rooms.SetQuizRoom", b =>
                {
                    b.Property<int>("QSetId")
                        .HasColumnType("int");

                    b.Property<int>("QRoomId")
                        .HasColumnType("int");

                    b.Property<bool>("ActiveData")
                        .HasColumnType("bit");

                    b.Property<int>("CreatedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UpdatedByUserId")
                        .HasColumnType("int");

                    b.HasKey("QSetId", "QRoomId");

                    b.HasIndex("QRoomId");

                    b.ToTable("SetQuizRooms");
                });

            modelBuilder.Entity("QuizMaster.Library.Common.Entities.Questionnaire.DetailType", b =>
                {
                    b.HasOne("QuizMaster.Library.Common.Entities.Questionnaire.QuestionDetail", null)
                        .WithMany("DetailTypes")
                        .HasForeignKey("QuestionDetailId");
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
                    b.HasOne("QuizMaster.Library.Common.Entities.Questionnaire.Question", "Question")
                        .WithMany("Details")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");
                });

            modelBuilder.Entity("QuizMaster.Library.Common.Entities.Questionnaire.QuestionDetailType", b =>
                {
                    b.HasOne("QuizMaster.Library.Common.Entities.Questionnaire.DetailType", "DetailType")
                        .WithMany()
                        .HasForeignKey("DetailTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QuizMaster.Library.Common.Entities.Questionnaire.QuestionDetail", "QuestionDetail")
                        .WithMany()
                        .HasForeignKey("QuestionDetailId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DetailType");

                    b.Navigation("QuestionDetail");
                });

            modelBuilder.Entity("QuizMaster.Library.Common.Entities.Rooms.QuestionSet", b =>
                {
                    b.HasOne("QuizMaster.Library.Common.Entities.Questionnaire.Question", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QuizMaster.Library.Common.Entities.Rooms.Set", "Set")
                        .WithMany()
                        .HasForeignKey("SetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");

                    b.Navigation("Set");
                });

            modelBuilder.Entity("QuizMaster.Library.Common.Entities.Rooms.SetQuizRoom", b =>
                {
                    b.HasOne("QuizMaster.Library.Common.Entities.Rooms.QuizRoom", "QRoom")
                        .WithMany()
                        .HasForeignKey("QRoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QuizMaster.Library.Common.Entities.Rooms.Set", "QSet")
                        .WithMany()
                        .HasForeignKey("QSetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("QRoom");

                    b.Navigation("QSet");
                });

            modelBuilder.Entity("QuizMaster.Library.Common.Entities.Questionnaire.Question", b =>
                {
                    b.Navigation("Details");
                });

            modelBuilder.Entity("QuizMaster.Library.Common.Entities.Questionnaire.QuestionDetail", b =>
                {
                    b.Navigation("DetailTypes");
                });
#pragma warning restore 612, 618
        }
    }
}
