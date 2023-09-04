﻿// <auto-generated />
using System;
using Catfish.API.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Catfish.API.Repository.Migrations
{
    [DbContext(typeof(RepoDbContext))]
    [Migration("20230904182825_RenamedDownloadLink")]
    partial class RenamedDownloadLink
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Catfish.API.Repository.Models.BackgroundJobs.JobRecord", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DataFile")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("DataFileSize")
                        .HasColumnType("bigint");

                    b.Property<string>("DownloadDataFileLink")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ExpectedDataRows")
                        .HasColumnType("int");

                    b.Property<string>("JobLabel")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProcessedDataRows")
                        .HasColumnType("int");

                    b.Property<DateTime>("Started")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CF_Repo_JobRecords");
                });

            modelBuilder.Entity("Catfish.API.Repository.Models.Entities.EntityData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EntityType")
                        .HasColumnType("int");

                    b.Property<string>("SerializedData")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<Guid>("TemplateId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("TemplateId");

                    b.ToTable("CF_Repo_Entities", (string)null);
                });

            modelBuilder.Entity("Catfish.API.Repository.Models.Entities.EntityTemplate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SerializedEntityTemplateSettings")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("CF_Repo_EntityTemplates", (string)null);
                });

            modelBuilder.Entity("Catfish.API.Repository.Models.Entities.Relationship", b =>
                {
                    b.Property<Guid>("SubjectEntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ObjectEntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.HasKey("SubjectEntityId");

                    b.HasIndex("ObjectEntityId");

                    b.ToTable("CF_Repo_Relationships");
                });

            modelBuilder.Entity("Catfish.API.Repository.Models.Forms.FormData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("FormId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SerializedFieldData")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("CF_Repo_FormData");
                });

            modelBuilder.Entity("Catfish.API.Repository.Models.Forms.FormTemplate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SerializedFields")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("CF_Repo_Forms");
                });

            modelBuilder.Entity("Catfish.API.Repository.Models.Workflow.WorkflowDbRecord", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("EntityTemplateId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SerializedWorkflow")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EntityTemplateId");

                    b.ToTable("CF_Repo_Workflows");
                });

            modelBuilder.Entity("EntityTemplateFormTemplate", b =>
                {
                    b.Property<Guid>("EntityTemplatesId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FormsId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("EntityTemplatesId", "FormsId");

                    b.HasIndex("FormsId");

                    b.ToTable("CF_Repo_EntityTemplateFormTemplate");
                });

            modelBuilder.Entity("Catfish.API.Repository.Models.Entities.EntityData", b =>
                {
                    b.HasOne("Catfish.API.Repository.Models.Entities.EntityTemplate", "Template")
                        .WithMany()
                        .HasForeignKey("TemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Template");
                });

            modelBuilder.Entity("Catfish.API.Repository.Models.Entities.Relationship", b =>
                {
                    b.HasOne("Catfish.API.Repository.Models.Entities.EntityData", "ObjectEntity")
                        .WithMany("ObjectRelationships")
                        .HasForeignKey("ObjectEntityId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Catfish.API.Repository.Models.Entities.EntityData", "SubjectEntity")
                        .WithMany("SubjectRelationships")
                        .HasForeignKey("SubjectEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ObjectEntity");

                    b.Navigation("SubjectEntity");
                });

            modelBuilder.Entity("Catfish.API.Repository.Models.Workflow.WorkflowDbRecord", b =>
                {
                    b.HasOne("Catfish.API.Repository.Models.Entities.EntityTemplate", "EntityTemplate")
                        .WithMany("Workflows")
                        .HasForeignKey("EntityTemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EntityTemplate");
                });

            modelBuilder.Entity("EntityTemplateFormTemplate", b =>
                {
                    b.HasOne("Catfish.API.Repository.Models.Entities.EntityTemplate", null)
                        .WithMany()
                        .HasForeignKey("EntityTemplatesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Catfish.API.Repository.Models.Forms.FormTemplate", null)
                        .WithMany()
                        .HasForeignKey("FormsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Catfish.API.Repository.Models.Entities.EntityData", b =>
                {
                    b.Navigation("ObjectRelationships");

                    b.Navigation("SubjectRelationships");
                });

            modelBuilder.Entity("Catfish.API.Repository.Models.Entities.EntityTemplate", b =>
                {
                    b.Navigation("Workflows");
                });
#pragma warning restore 612, 618
        }
    }
}
