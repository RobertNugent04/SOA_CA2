﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SOA_CA2.Infrastructure;


#nullable disable

namespace SOA_CA2.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241202181802_UpdateUserModel")]
    partial class UpdateUserModel
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SOA_CA2.Models.Comment", b =>
                {
                    b.Property<int>("Comment_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Comment_ID"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<DateTime>("Created_At")
                        .HasColumnType("timestamp");

                    b.Property<int>("Post_ID")
                        .HasColumnType("integer");

                    b.Property<int>("User_ID")
                        .HasColumnType("integer");

                    b.HasKey("Comment_ID");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("SOA_CA2.Models.Friend", b =>
                {
                    b.Property<int>("Friend_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Friend_ID"));

                    b.Property<DateTime>("Created_At")
                        .HasColumnType("timestamp");

                    b.Property<int>("Friend_User_ID")
                        .HasColumnType("integer");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("User_ID")
                        .HasColumnType("integer");

                    b.HasKey("Friend_ID");

                    b.ToTable("Friend");
                });

            modelBuilder.Entity("SOA_CA2.Models.Like", b =>
                {
                    b.Property<int>("Like_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Like_ID"));

                    b.Property<DateTime>("Created_At")
                        .HasColumnType("timestamp");

                    b.Property<int>("Post_ID")
                        .HasColumnType("integer");

                    b.Property<int>("User_ID")
                        .HasColumnType("integer");

                    b.HasKey("Like_ID");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("SOA_CA2.Models.Message", b =>
                {
                    b.Property<int>("Message_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Message_ID"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Receiver_User_ID")
                        .HasColumnType("integer");

                    b.Property<int>("Sender_User_ID")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Sent_At")
                        .HasColumnType("timestamp");

                    b.HasKey("Message_ID");

                    b.ToTable("Message");
                });

            modelBuilder.Entity("SOA_CA2.Models.Notification", b =>
                {
                    b.Property<int>("Notification_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Notification_ID"));

                    b.Property<DateTime>("Created_At")
                        .HasColumnType("timestamp");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int>("User_ID")
                        .HasColumnType("integer");

                    b.HasKey("Notification_ID");

                    b.ToTable("Notification");
                });

            modelBuilder.Entity("SOA_CA2.Models.Post", b =>
                {
                    b.Property<int>("Post_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Post_ID"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Created_At")
                        .HasColumnType("timestamp");

                    b.Property<string>("Image_URL")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime?>("Updated_At")
                        .HasColumnType("timestamp");

                    b.Property<int>("User_ID")
                        .HasColumnType("integer");

                    b.HasKey("Post_ID");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("SOA_CA2.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UserId"));

                    b.Property<string>("Bio")
                        .HasMaxLength(500)
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamptz");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProfilePictureUrl")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamptz");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
