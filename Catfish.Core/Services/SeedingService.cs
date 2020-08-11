using Catfish.Core.Models;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.Fields;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System;
using Catfish.Core.Helpers;
using Catfish.Core.Models.Contents.ViewModels;

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
            ms.Description.SetContent("Dublin Core metadata element set, Version 1.1");

            ms.Fields.Add(new TextField("Contributor", 
                "An entity responsible for making contributions to the resource."));

            ms.Fields.Add(new TextArea("Coverage",
                "The spatial or temporal topic of the resource, the spatial applicability of the resource, or the jurisdiction under which the resource is relevant."));

            ms.Fields.Add(new TextField("Creator",
                "An entity primarily responsible for making the resource."));

            ms.Fields.Add(new DateField("Date",
                "A point or period of time associated with an event in the lifecycle of the resource."));

            ms.Fields.Add(new TextArea("Description",
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

            ms.Fields.Add(new TextArea("Rights",
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

        public MetadataSet NewDefaultMetadataSet()
        {
            MetadataSet ms = new MetadataSet();
            ms.Name.SetContent("System Default");
            ms.Description.SetContent("System default, minimalistic metadata set.");

            ms.Fields.Add(new TextField("Name",
                "Name of the resourse."));

            ms.Fields.Add(new TextField("Description",
                "Description of the resourse."));

            return ms;
        }
        public ItemTemplate NewDublinCoreItem()
        {
            Item item = new Item();
            item.MetadataSets.Add(NewDublinCoreMetadataSet());

            ItemTemplate et = new ItemTemplate()
            {
                TemplateName = "Doublin Core Item",
                Data = item.Data,
                TargetType = item.GetType().FullName
            };
            return et;
        }

        public ItemTemplate NewDefaultItem()
        {
            Item item = new Item();
            item.MetadataSets.Add(NewDefaultMetadataSet());

            ItemTemplate et = new ItemTemplate()
            {
                TemplateName = "Default Item",
                Data = item.Data,
                TargetType = item.GetType().FullName
            };
            return et;
        }

        public CollectionTemplate NewDublinCoreCollection()
        {
            Collection collection = new Collection();
            collection.MetadataSets.Add(NewDublinCoreMetadataSet());

            CollectionTemplate et = new CollectionTemplate()
            {
                TemplateName = "Doublin Core Collection",
                Data = collection.Data,
                TargetType = collection.GetType().FullName
            };
            return et;
        }

        public CollectionTemplate NewDefaultCollection()
        {
            Collection collection = new Collection();
            collection.MetadataSets.Add(NewDefaultMetadataSet());

            CollectionTemplate et = new CollectionTemplate()
            {
                TemplateName = "Default Collection",
                Data = collection.Data,
                TargetType = collection.GetType().FullName
            };
            return et;
        }

        public void SeedDefaults(bool createSampleData)
        {
            DbEntityService entityService = new DbEntityService(Db);

            EntityTemplate template;

            template = NewDefaultItem();
            if (!Db.ItemTemplates.Where(et => et.TemplateName == template.TemplateName).Any())
                Db.ItemTemplates.Add(template as ItemTemplate);
            Db.SaveChanges();

            template = NewDublinCoreItem();
            if (!Db.ItemTemplates.Where(et => et.TemplateName == template.TemplateName).Any())
                Db.ItemTemplates.Add(template as ItemTemplate);

            template = NewDefaultCollection();
            if (!Db.CollectionTemplates.Where(et => et.TemplateName == template.TemplateName).Any())
                Db.CollectionTemplates.Add(template as CollectionTemplate);

            template = NewDublinCoreCollection();
            if (!Db.CollectionTemplates.Where(et => et.TemplateName == template.TemplateName).Any())
                Db.CollectionTemplates.Add(template as CollectionTemplate);

            Db.SaveChanges();

            //Using the same seed to generate the same sequence of random numbers.
            //The seeding parameters may only be effective when seeding an empty database.
            Random rand = new Random(0);
            int numTestCollections = 10;
            int numTestItems = 200;

            List<CollectionTemplate> collectionTemplates = Db.CollectionTemplates.ToList();
            if (Db.Collections.Count() == 0)
            {
                for (int i = 0; i < numTestCollections; ++i)
                {
                    int index = rand.Next(0, collectionTemplates.Count);
                    template = collectionTemplates[index];

                    Collection c = template.Instantiate<Collection>();
                    c.Name.SetContent(string.Format("Test Collection {0}", i));
                    c.Description.SetContent(string.Format("This is the test collection #{0} created by seeding.", i));
                    Db.Collections.Add(c);
                }
                Db.SaveChanges();
            }

            List<ItemTemplate> itemTemplates = Db.ItemTemplates.ToList();
            List<Collection> collections = Db.Collections.ToList();
            if (Db.Items.Count() == 0)
            {
                for (int i = 0; i < numTestItems; ++i)
                {
                    int index = rand.Next(0, itemTemplates.Count);
                    template = itemTemplates[index];

                    Item it = template.Instantiate<Item>();
                    it.Name.SetContent(string.Format("Test Item {0}", i));
                    it.Description.SetContent(string.Format("This is the test item #{0} created by seeding.", i));

                    //Filling default values for each metadata set field
                    foreach(var ms in it.MetadataSets)
                    {
                        foreach(var field in ms.Fields)
                        {
                            var languages = field.Name.Values.Select(val => val.Language).Distinct();
                            if(typeof(TextField).IsAssignableFrom(field.GetType()))
                            {
                                MultilingualValue val = new MultilingualValue();
                                (field as TextField).Values.Add(val);

                                if (typeof(TextArea).IsAssignableFrom(field.GetType()))
                                {
                                    foreach (var lang in languages)
                                        val.SetContent(LoremIpsum(5, 10, 3, 5), lang);
                                }
                                else
                                {
                                    foreach (var lang in languages)
                                        val.SetContent(LoremIpsum(), lang);
                                }
                            }
                            else if (typeof(DateField).IsAssignableFrom(field.GetType()))
                            {
                                (field as DateField).SetValue(DateTime.Today.ToShortDateString());
                            }

                        }
                    }

                    // selecting a default parent collection
                    index = rand.Next(0, collections.Count + 1);
                    if (index < collections.Count)
                        it.PrimaryCollection = collections[index];

                    Db.Items.Add(it);
                }
                Db.SaveChanges();
            }


        }

        protected string LoremIpsum(int minWords = 2, int maxWords = 5, int minSentences = 1, int maxSentences = 1, int numParagraphs = 1)
        {

            var words = new[]{"lorem", "ipsum", "dolor", "sit", "amet", "consectetuer",
            "adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod",
            "tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat"};

            var rand = new Random();
            int numSentences = rand.Next(maxSentences - minSentences)
                + minSentences + 1;
            int numWords = rand.Next(maxWords - minWords) + minWords + 1;

            StringBuilder result = new StringBuilder();

            for (int p = 0; p < numParagraphs; p++)
            {
                if (numParagraphs > 1)
                    result.Append("<p>");
                for (int s = 0; s < numSentences; s++)
                {
                    for (int w = 0; w < numWords; w++)
                    {
                        if (w > 0) { result.Append(" "); }
                        result.Append(words[rand.Next(words.Length)]);
                    }
                    result.Append(". ");
                }
                if (numParagraphs > 1)
                    result.Append("</p>");
            }

            return result.ToString();
        }
    }
}
