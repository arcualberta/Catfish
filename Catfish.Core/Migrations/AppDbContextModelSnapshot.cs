﻿// <auto-generated />
using System;
using Catfish.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Catfish.Core.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Catfish.Core.Models.Contents.Form", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .HasColumnType("xml");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("FormName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Catfish_Forms");
                });

            modelBuilder.Entity("Catfish.Core.Models.Entity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .HasColumnType("xml");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("GroupId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("PrimaryCollectionId")
                        .HasColumnName("PrimaryCollectionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("StatusId")
                        .HasColumnName("StatusId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("TemplateId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UserEmail")
                        .HasColumnName("UserEmail")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PrimaryCollectionId");

                    b.HasIndex("StatusId");

                    b.ToTable("Catfish_Entities");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Entity");
                });

            modelBuilder.Entity("Catfish.Core.Models.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("GroupStatus")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Catfish_Groups");
                });

            modelBuilder.Entity("Catfish.Core.Models.GroupRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("Catfish_GroupRoles");
                });

            modelBuilder.Entity("Catfish.Core.Models.GroupTemplate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("EntityTemplateId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("EntityTemplateId");

                    b.HasIndex("GroupId");

                    b.ToTable("Catfish_GroupTemplates");
                });

            modelBuilder.Entity("Catfish.Core.Models.Relationship", b =>
                {
                    b.Property<Guid>("SubjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ObjctId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Predicate")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SubjectId", "ObjctId");

                    b.HasIndex("ObjctId");

                    b.ToTable("Catfish_Relationships");
                });

            modelBuilder.Entity("Catfish.Core.Models.SystemPage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<Guid>("PageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PageKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("SiteId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Catfish_SystemPages");
                });

            modelBuilder.Entity("Catfish.Core.Models.SystemStatus", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("EntityTemplateId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsEditable")
                        .HasColumnType("bit");

                    b.Property<string>("NormalizedStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Catfish_SystemStatuses");
                });

            modelBuilder.Entity("Catfish.Core.Models.UserGroupRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GroupRoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("GroupRoleId");

                    b.ToTable("Catfish_UserGroupRoles");
                });

            modelBuilder.Entity("Catfish.Core.Models.Collection", b =>
                {
                    b.HasBaseType("Catfish.Core.Models.Entity");

                    b.ToTable("Catfish_Entities");

                    b.HasDiscriminator().HasValue("Collection");
                });

            modelBuilder.Entity("Catfish.Core.Models.EntityTemplate", b =>
                {
                    b.HasBaseType("Catfish.Core.Models.Entity");

                    b.Property<string>("Domain")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TargetType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TemplateName")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("Catfish_Entities");

                    b.HasDiscriminator().HasValue("EntityTemplate");
                });

            modelBuilder.Entity("Catfish.Core.Models.Item", b =>
                {
                    b.HasBaseType("Catfish.Core.Models.Entity");

                    b.ToTable("Catfish_Entities");

                    b.HasDiscriminator().HasValue("Item");
                });

            modelBuilder.Entity("Catfish.Core.Models.CollectionTemplate", b =>
                {
                    b.HasBaseType("Catfish.Core.Models.EntityTemplate");

                    b.ToTable("Catfish_Entities");

                    b.HasDiscriminator().HasValue("CollectionTemplate");
                });

            modelBuilder.Entity("Catfish.Core.Models.ItemTemplate", b =>
                {
                    b.HasBaseType("Catfish.Core.Models.EntityTemplate");

                    b.ToTable("Catfish_Entities");

                    b.HasDiscriminator().HasValue("ItemTemplate");
                });

            modelBuilder.Entity("Catfish.Core.Models.Entity", b =>
                {
                    b.HasOne("Catfish.Core.Models.Collection", "PrimaryCollection")
                        .WithMany()
                        .HasForeignKey("PrimaryCollectionId");

                    b.HasOne("Catfish.Core.Models.SystemStatus", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId");
                });

            modelBuilder.Entity("Catfish.Core.Models.GroupRole", b =>
                {
                    b.HasOne("Catfish.Core.Models.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Catfish.Core.Models.GroupTemplate", b =>
                {
                    b.HasOne("Catfish.Core.Models.EntityTemplate", "EntityTemplate")
                        .WithMany()
                        .HasForeignKey("EntityTemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Catfish.Core.Models.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Catfish.Core.Models.Relationship", b =>
                {
                    b.HasOne("Catfish.Core.Models.Entity", "Objct")
                        .WithMany("ObjectRelationships")
                        .HasForeignKey("ObjctId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Catfish.Core.Models.Entity", "Subject")
                        .WithMany("SubjectRelationships")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("Catfish.Core.Models.UserGroupRole", b =>
                {
                    b.HasOne("Catfish.Core.Models.GroupRole", "GroupRole")
                        .WithMany()
                        .HasForeignKey("GroupRoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
