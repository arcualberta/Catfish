using Catfish.Core.Models;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.Fields;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Services
{
    public class SeedingService
    {
        protected AppDbContext Db;
        public SeedingService(AppDbContext db)
        {
            Db = db;
        }

        public MetadataSet NewDublinCoreMetadataSet()
        {
            MetadataSet ms = new MetadataSet();
            ms.Name.SetContent("Dublin Core");
            ms.Description.SetContent("Dublin Core Metadata Element Set, Version 1.1");

            ms.Fields.Add(new TextField("Contributor", 
                "An entity responsible for making contributions to the resource."));

            ms.Fields.Add(new TextField("Coverage",
                "The spatial or temporal topic of the resource, the spatial applicability of the resource, or the jurisdiction under which the resource is relevant."));

            ms.Fields.Add(new TextField("Creator",
                "An entity primarily responsible for making the resource."));

            ms.Fields.Add(new TextField("Date",
                "A point or period of time associated with an event in the lifecycle of the resource."));

            ms.Fields.Add(new TextField("Description",
                "An account of the resource."));

            ms.Fields.Add(new TextField("Format",
                "The file format, physical medium, or dimensions of the resource."));

            ms.Fields.Add(new TextField("Identifier",
                "An unambiguous reference to the resource within a given context."));

            ms.Fields.Add(new TextField("Language",
                "A language of the resource."));

            ms.Fields.Add(new TextField("Publisher",
                "An entity responsible for making the resource available."));

            ms.Fields.Add(new TextField("Relation",
                "A related resource."));

            ms.Fields.Add(new TextField("Rights",
                "Information about rights held in and over the resource."));

            ms.Fields.Add(new TextField("Source",
                "A related resource from which the described resource is derived."));

            ms.Fields.Add(new TextField("Subject",
                "The topic of the resource."));

            ms.Fields.Add(new TextField("Title",
                "A name given to the resource."));

            ms.Fields.Add(new TextField("Type",
                "The nature or genre of the resource."));




            return ms;
        }
    }
}
