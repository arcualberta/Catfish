using Catfish.Core.Models;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.Fields;
using ElmahCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Catfish.Core.Services
{
    public class NewSeedingService
    {
        protected AppDbContext Db;
        private readonly ErrorLog _errorLog;
        public NewSeedingService(AppDbContext db, ErrorLog errorLog)
        {
            Db = db;
            _errorLog = errorLog;
        }

        public MetadataSet NewDublinCoreMetadataSet()
        {
            MetadataSet ms = new MetadataSet();
            ms.Name.SetContent("Dublin Core");
            ms.Description.SetContent("Dublin Core metadata element set, Version 1.1")
                ;

            ms.Fields.Add(new TextField("Contributor",
                "An entity responsible for making contributions to the resource."));

            ms.Fields.Add(new TextArea("Coverage",
                "The spatial or temporal topic of the resource, the spatial applicability of the resource, or the jurisdiction under which the resource is relevant."));

            ms.Fields.Add(new TextField("Creator",
                "An entity primarily responsible for making the resource."));

            ms.Fields.Add(new DateField("Date",
                "A point or period of time associated with an event in the lifecycle of the resource."));

            ms.Fields.Add(new TextArea("Description",
                "The Supreme Court hears a case remotely for the first time. An internal Trump administration report projects about 200,000 new cases each day " +
                "by the end of the month. The White House bars coronavirus task force officials from testifying to Congress without approval. As President Trump presses for states " +
                "to reopen their economies, his administration is privately projecting a steady rise in the number of coronavirus cases and deaths over the next several weeks.</br> The daily " +
                "death toll will reach about 3,000 on June 1, according to an internal document obtained by The New York Times, nearly double the current number of about 1,750. The projections, based on government modeling pulled" +
                " together in chart form by the Federal Emergency Management Agency, forecast about 200, 000 new cases each day by the end of the month, up from about 25, 000 cases a day currently."));

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
            ms.Description.SetContent("The flight data recorders were recovered from the debris and are to be analyzed at the National Research Council in Ottawa.In an " +
                "interview with CBC News that took place before the search switched to recovery mode, Sajjan acknowledged the difficulty involved in reaching wreckage that " +
                "may be as much as 3, 000 metres below the surface of the Ionian Sea. Few nations possess that kind of deep - diving capability and Sajjan said he's been " +
                "talking to NATO's secretary general and allies about the technological options. I can assure you we will put in all of the resources necessary, " +
                "said Sajjan who expressed confidence in the investigation team. Our folks on the ground will figure what happened.The debris also is believed to be " +
                "spread over a wide area on the ocean floor.One expert said that spread suggests something about the forces involved in the crash.");

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
            DbEntityService entityService = new DbEntityService(Db, _errorLog);

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
            int numTestCollections = 1;
            int numTestItems = 2;

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
                    foreach (var ms in it.MetadataSets)
                    {
                        foreach (var field in ms.Fields)
                        {
                            var languages = field.Name.Values.Select(val => val.Language).Distinct();
                            
                            if (typeof(TextField).IsAssignableFrom(field.GetType()))
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

                            foreach (var lang in languages)
                            {
                                string desc = @"A couple of weeks after the first coronavirus case arrived in the Netherlands, we were told to stay inside. Bars and schools closed down and my hometown of Amsterdam came to a halt.
After the first feelings of confusion and uncertainty, I slowly got used to the idea. There was a calmness in the streets I hadn't experienced in years.
In the past decade, Amsterdam has become a hasty and chaotic place, its occupants increasingly short-tempered. The city's population of 863,000 was annually swollen by nine million tourists.
The shops in the city center were given over to cater to them, selling waffles, souvenirs and cannabis seeds. Stores catering to residents closed down because of extreme hikes in rent and the lack of customers.
More and more, locals have started to avoid the most beautiful part of their city, as its houses were rented out to tourists and expats.";
                                field.Description.SetContent(desc);
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
