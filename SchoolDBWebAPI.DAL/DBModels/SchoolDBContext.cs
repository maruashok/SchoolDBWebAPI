﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace SchoolDBWebAPI.DAL.DBModels
{
    public partial class SchoolDBContext : DbContext
    {
        public SchoolDBContext()
        {
        }

        public SchoolDBContext(DbContextOptions<SchoolDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<ClassBatch> ClassBatches { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<EducationCouse> EducationCouses { get; set; }
        public virtual DbSet<EducationDetail> EducationDetails { get; set; }
        public virtual DbSet<NotificationDatum> NotificationData { get; set; }
        public virtual DbSet<NotificationsMaster> NotificationsMasters { get; set; }
        public virtual DbSet<PaymentInfo> PaymentInfos { get; set; }
        public virtual DbSet<QueOption> QueOptions { get; set; }
        public virtual DbSet<QueResponse> QueResponses { get; set; }
        public virtual DbSet<QuizDetail> QuizDetails { get; set; }
        public virtual DbSet<QuizQuestion> QuizQuestions { get; set; }
        public virtual DbSet<QuizResponse> QuizResponses { get; set; }
        public virtual DbSet<RoleMaster> RoleMasters { get; set; }
        public virtual DbSet<Semester> Semesters { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<StudentMaster> StudentMasters { get; set; }
        public virtual DbSet<UserMaster> UserMasters { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<staff> staff { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=ConnectionStrings:Default");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Address");

                entity.Property(e => e.Details)
                    .HasMaxLength(100)
                    .IsFixedLength(true);

                entity.Property(e => e.Pincode)
                    .HasMaxLength(10)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.StateId).HasColumnName("state_id");
            });

            modelBuilder.Entity<ClassBatch>(entity =>
            {
                entity.ToTable("ClassBatch");

                entity.Property(e => e.BatchName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Phonecode).HasColumnName("phonecode");

                entity.Property(e => e.Sortname)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("sortname");
            });

            modelBuilder.Entity<EducationCouse>(entity =>
            {
                entity.ToTable("EducationCouse");

                entity.Property(e => e.CourseName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<EducationDetail>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.EducationDetails)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EduDetails_EducationCouse");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.EducationDetails)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("FK_EduDetails_StudentMaster");
            });

            modelBuilder.Entity<NotificationDatum>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.HasOne(d => d.NotMaster)
                    .WithMany(p => p.NotificationData)
                    .HasForeignKey(d => d.NotMasterId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_NotificationData_NotificationsMaster");
            });

            modelBuilder.Entity<NotificationsMaster>(entity =>
            {
                entity.ToTable("NotificationsMaster");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Message).HasColumnType("text");

                entity.Property(e => e.MsgTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<PaymentInfo>(entity =>
            {
                entity.ToTable("PaymentInfo");

                entity.Property(e => e.Currency)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.Guid)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("GUID");

                entity.Property(e => e.HandlingFee).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.InvoiceNo).HasMaxLength(50);

                entity.Property(e => e.ItemName).HasMaxLength(50);

                entity.Property(e => e.PayerId).HasMaxLength(50);

                entity.Property(e => e.PaymentDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentEndDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentStartDate).HasColumnType("datetime");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Subtotal).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TaxAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.PaymentInfos)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("FK_PaymentInfo_StudentMaster");
            });

            modelBuilder.Entity<QueOption>(entity =>
            {
                entity.Property(e => e.OptionValue)
                    .HasMaxLength(100)
                    .IsFixedLength(true);

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.QueOptions)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK_QuestionOptions_QuizQuestions");
            });

            modelBuilder.Entity<QueResponse>(entity =>
            {
                entity.ToTable("QueResponse");

                entity.Property(e => e.QueScore).HasColumnType("decimal(18, 4)");

                entity.HasOne(d => d.Response)
                    .WithMany(p => p.QueResponses)
                    .HasForeignKey(d => d.ResponseId)
                    .HasConstraintName("FK_QueResponse_QuizResponse");
            });

            modelBuilder.Entity<QuizDetail>(entity =>
            {
                entity.ToTable("QuizDetail");

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .IsFixedLength(true);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.PaidQuiz)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<QuizQuestion>(entity =>
            {
                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsFixedLength(true);

                entity.HasOne(d => d.Quiz)
                    .WithMany(p => p.QuizQuestions)
                    .HasForeignKey(d => d.QuizId)
                    .HasConstraintName("FK_QuizQuestions_QuizDetail");
            });

            modelBuilder.Entity<QuizResponse>(entity =>
            {
                entity.ToTable("QuizResponse");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.QuizScore).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.SubmitDate).HasColumnType("datetime");

                entity.HasOne(d => d.Quiz)
                    .WithMany(p => p.QuizResponses)
                    .HasForeignKey(d => d.QuizId)
                    .HasConstraintName("FK_QuizResponse_QuizDetail");
            });

            modelBuilder.Entity<RoleMaster>(entity =>
            {
                entity.ToTable("RoleMaster");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Semester>(entity =>
            {
                entity.Property(e => e.SemesterName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<State>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CountryId)
                    .HasColumnName("country_id")
                    .HasDefaultValueSql("('1')");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<StudentMaster>(entity =>
            {
                entity.ToTable("StudentMaster");

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.Emaild)
                    .HasMaxLength(50)
                    .HasColumnName("EMaild")
                    .IsFixedLength(true);

                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.Gender)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.IsActive)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.Property(e => e.MobileNo).HasMaxLength(15);

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.Property(e => e.StudentCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.StudentPic).HasMaxLength(100);

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.StudentMasters)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("FK_StudentMaster_Address");
            });

            modelBuilder.Entity<UserMaster>(entity =>
            {
                entity.ToTable("UserMaster");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Emaild).HasMaxLength(50);

                entity.Property(e => e.IsVerified)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ProfilePicPath).HasMaxLength(100);
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("UserRole");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRole_RoleMaster");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserRole_UserMaster");
            });

            modelBuilder.Entity<staff>(entity =>
            {
                entity.ToTable("Staff");

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.Emaild)
                    .HasMaxLength(50)
                    .HasColumnName("EMaild")
                    .IsFixedLength(true);

                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.Gender)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.IsActive)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.Property(e => e.MobileNo).HasMaxLength(15);

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.Property(e => e.Salary).HasColumnType("numeric(18, 4)");

                entity.Property(e => e.StaffCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.staff)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("FK_EmpMaster_Address");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
