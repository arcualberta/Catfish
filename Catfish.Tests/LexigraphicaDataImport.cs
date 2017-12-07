using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Catfish.Core.Models;
using System.Linq;
using Catfish.Core.Models.Forms;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using Catfish.Core.Services;
using Catfish.Core.Models.Data;
using System.Xml.Linq;

namespace Catfish.Tests
{
    [TestClass]
    public class LexigraphicaDataImport
    {
        public class LexigraphiBase
        {
            public static T Instantiate<T>(string str) where T : LexigraphiBase
            {
                string json = str.Trim(new char[] { '[', ']', ',' });
                try
                {
                    return JsonConvert.DeserializeObject(json, typeof(T)) as T;
                }
                catch(Exception ex)
                {
                    return null;
                }
            }

            public static List<T>InstantiateList<T>(string data) where T : LexigraphiBase
            {
                int n = data.IndexOf('[');
                data = data.Substring(n + 1);
                n = data.IndexOf(']');
                data = data.Substring(0, n);

                data = data.Replace('\n', ' ');

                string[] strList = data.Split(new string[] { "}," }, StringSplitOptions.RemoveEmptyEntries);

                return strList.Select(s => Instantiate<T>(s + "}")).ToList();

            }
        }
        public class LexigraphiModel: LexigraphiBase
        {
            public int id;
            public string created_at;
            public string updated_at;
            public int lock_version;
        }
        public class Comment: LexigraphiModel
        {
            public int user_id;
            public int photo_id;
            public string comment;
        }
        public class Photo: LexigraphiModel
        {
            public int user_id;
            public string description;
            public string filename;
            public string format;
            public string text_type;
        }
        public class PhotoWord: LexigraphiBase
        {
            public int photo_id;
            public int word_id;
        }
        public class Phrase: LexigraphiModel
        {
            public int photo_id;
            public string phrase;
        }

        public class User: LexigraphiModel
        {
            public string username;
            public string email;
            public string role;
            public int active;
            public string url;
            public string description;
        }

        public class Word: LexigraphiModel
        {
            public string word;
        }

        public void SetTimeStamps(DateTime created, DateTime updated, XElement element)
        {
            foreach (XElement ele in element.Elements())
                SetTimeStamps(created, updated, ele);

            if (element.Attribute("model-type") != null && element.Attribute("model-type").Value.Length > 0)
            {
                element.SetAttributeValue("created", created.ToString());
                element.SetAttributeValue("updated", updated.ToString());
            }
        }

        [TestMethod]
        public void ImportData()
        {
            CatfishDbContext db = new CatfishDbContext();

            //Making sure that the Lexigraphica metadata set exists
            var mss = db.MetadataSets.ToList();
            MetadataSet ms = null;
            foreach (var m in mss)
                if ((ms = m).Name == "Lexigraphica")
                    break;
            Assert.AreNotEqual(null, ms);
            Assert.AreEqual(1, ms.Fields.Where(f => f.Name == "Words").Count());
            Assert.AreEqual(1, ms.Fields.Where(f => f.Name == "Phrases").Count());
            Assert.AreEqual(1, ms.Fields.Where(f => f.Name == "Description").Count());

            List<Comment> comments = new List<Comment>();
            var str = File.ReadAllText(@"C:\Users\Kamal\Documents\Projects\Catfish\Lexigraphica_Source_Data\comments.txt");
            var data = str.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
            int n = 7;
            for (var i = n; i < data.Count - n; i = i + n)
            {
                Comment obj = new Comment()
                {
                    id = Int32.Parse(data[i]),
                    user_id = Int32.Parse(data[i + 1]),
                    photo_id = Int32.Parse(data[i + 2]),
                    created_at = data[i + 3],
                    updated_at = data[i + 4],
                    lock_version = Int32.Parse(data[i + 5]),
                    comment = data[i + 6]
                };
                comments.Add(obj);
            }

            List<Photo> photos = new List<Photo>();
            str = File.ReadAllText(@"C:\Users\Kamal\Documents\Projects\Catfish\Lexigraphica_Source_Data\photos.txt");
            data = str.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
            n = 9;
            for (var i = n; i < data.Count - n; i = i + n)
            {
                //id	|||	user_id	|||	description	|||	filename	|||	format	|||	created_at	|||	updated_at	|||	lock_version	|||	text_type	|||
                Photo obj = new Photo()
                {
                    id = Int32.Parse(data[i]),
                    user_id = Int32.Parse(data[i + 1]),
                    description = data[i + 2],
                    filename = data[i + 3],
                    format = data[i + 4],
                    created_at = data[i + 5],
                    updated_at = data[i + 6],
                    lock_version = Int32.Parse(data[i + 7]),
                    text_type = data[i + 8]
                };
                photos.Add(obj);
            }

            List<PhotoWord> photo_words = new List<PhotoWord>();
            str = File.ReadAllText(@"C:\Users\Kamal\Documents\Projects\Catfish\Lexigraphica_Source_Data\photo_words.txt");
            data = str.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
            n = 2;
            for (var i = n; i < data.Count - n; i = i + n)
            {
                PhotoWord obj = new PhotoWord()
                {
                    photo_id = Int32.Parse(data[i]),
                    word_id = Int32.Parse(data[i + 1])
                };
                photo_words.Add(obj);
            }

            List<Phrase> phrases = new List<Phrase>();
            str = File.ReadAllText(@"C:\Users\Kamal\Documents\Projects\Catfish\Lexigraphica_Source_Data\phrases.txt");
            data = str.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
            n = 6;
            for (var i = n; i < data.Count - n; i = i + n)
            {
                //id	|||	photo_id	|||	phrase	|||	created_at	|||	updated_at	|||	lock_version	|||
                Phrase obj = new Phrase()
                {
                    id = Int32.Parse(data[i]),
                    photo_id = Int32.Parse(data[i + 1]),
                    phrase = data[i + 2],
                    created_at = data[i + 3],
                    updated_at = data[i + 4],
                    lock_version = Int32.Parse(data[i + 5])
                };
                phrases.Add(obj);
            }

            List<User> users = new List<User>();
            str = File.ReadAllText(@"C:\Users\Kamal\Documents\Projects\Catfish\Lexigraphica_Source_Data\users.txt");
            data = str.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
            n = 10;
            for (var i = n; i < data.Count - n; i = i + n)
            {
                //id	|||	username	|||	email	|||	role	|||	created_at	|||	updated_at	|||	lock_version	|||	active	|||	url	|||	description
                User obj = new User()
                {
                    id = Int32.Parse(data[i]),
                    username = data[i + 1],
                    email = data[i + 2],
                    role = data[i + 3],
                    created_at = data[i + 4],
                    updated_at = data[i + 5],
                    lock_version = Int32.Parse(data[i + 6]),
                    active = Int32.Parse(data[i + 7]),
                    url = data[i + 8],
                    description = data[i + 9]

                };
                users.Add(obj);
            }

            List<Word> words = new List<Word>();
            str = File.ReadAllText(@"C:\Users\Kamal\Documents\Projects\Catfish\Lexigraphica_Source_Data\words.txt");
            data = str.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
            n = 5;
            for (var i = n; i < data.Count - n; i = i + n)
            {
                Word obj = new Word()
                {
                    id = Int32.Parse(data[i]),
                    word = data[i + 1],
                    created_at = data[i + 2],
                    updated_at = data[i + 3],
                    lock_version = Int32.Parse(data[i + 4])
                };
                words.Add(obj);
            }


            int? photoEntityTypeId = db.EntityTypes.Where(x => x.Name == "Photo").Select(x => x.Id).FirstOrDefault();
            Assert.IsTrue(photoEntityTypeId.Value > 0);

            int? commentEntityTypeId = db.EntityTypes.Where(x => x.Name == "Comment").Select(x => x.Id).FirstOrDefault();
            Assert.IsTrue(commentEntityTypeId.Value > 0);

            int? commentFormId = db.FormTemplates.ToList().Where(x => x.Name == "Comment Form").Select(x => x.Id).FirstOrDefault();
            Assert.IsTrue(commentFormId.Value > 0);


            List<Item> items = new List<Item>();
            ItemService itemSrv = new ItemService(db);
            SubmissionService subSrv = new SubmissionService(db);
            foreach (var photo in photos)
            {
                Item item = itemSrv.CreateEntity<Item>(photoEntityTypeId.Value);
                List<AuditEntry> audits = new List<AuditEntry>();

                User creator = users.Where(u => u.id == photo.user_id).FirstOrDefault();
                AuditEntry audit = item.AddAuditEntry(AuditEntry.eAction.Create, creator != null ? creator.username : "");
                audits.Add(audit);

                MetadataSet photoMetadata = item.MetadataSets.Where(m => m.Name == "Photo Metadata").FirstOrDefault();

                FormField wordsField = photoMetadata.Fields.Where(f => f.Name == "Words").FirstOrDefault();
                List<PhotoWord> photoWords = photo_words.Where(w => w.photo_id == photo.id).ToList();
                List<string> wList = new List<string>();
                foreach (var pw in photoWords)
                {
                    string word = words.Where(w => w.id == pw.word_id).Select(w => w.word).FirstOrDefault();
                    wList.Add(word);
                }
                wordsField.SetValues(wList);

                FormField phrasesField = photoMetadata.Fields.Where(f => f.Name == "Phrases").FirstOrDefault();
                List<Phrase> photoPhrases = phrases.Where(p => p.photo_id == photo.id).ToList();
                if (photoPhrases.Count > 0)
                {
                    phrasesField.SetValues(photoPhrases.Where(p => p.photo_id == photo.id).Select(p => p.phrase).ToList());
                    phrasesField.Created = photoPhrases.Select(s => DateTime.Parse(s.created_at)).Min();
                    phrasesField.SetAttribute("updated", photoPhrases.Select(s => DateTime.Parse(s.created_at)).Max().ToString());

                }

                List<Comment> photoComments = comments.Where(c => c.photo_id == photo.id).ToList();
                List<DateTime> commentCreated = new List<DateTime>();
                List<DateTime> commentUpdated = new List<DateTime>();
                List<FormSubmission> commentSubmissions = new List<FormSubmission>();
                foreach (var comment in photoComments)
                {
                    commentCreated.Add(DateTime.Parse(comment.created_at));
                    commentUpdated.Add(DateTime.Parse(comment.updated_at));

                    Form commentForm = subSrv.CreateSubmissionForm(commentFormId.Value);
                    FormField commentField = commentForm.Fields.Where(f => f.Name == "Comment").FirstOrDefault();
                    commentField.SetValues(new List<string>() { comment.comment });

                    FormSubmission commentSubmission = new FormSubmission();
                    commentSubmission.ReplaceFormData(commentForm.Data);
                    item.AddData(commentSubmission);
                    commentSubmissions.Add(commentSubmission);

                    User commentUser = users.Where(u => u.id == comment.user_id).FirstOrDefault();
                    var comment_create_audit = item.AddAuditEntry(AuditEntry.eAction.Update, commentUser.username);
                    audits.Add(comment_create_audit);
                }

                DateTime photCreated = DateTime.Parse(photo.created_at);
                DateTime photoUpdated = DateTime.Parse(photo.updated_at);
                DateTime lastUpdated = photoComments.Count > 0 ? commentUpdated.Max() : photoUpdated;

                SetTimeStamps(photCreated, photoUpdated, item.Data);
                for (int i = 0; i < commentSubmissions.Count; ++i)
                {
                    SetTimeStamps(commentCreated[i], commentUpdated[i], commentSubmissions[i].Data);
                    SetTimeStamps(commentCreated[i], commentUpdated[i], audits[i+1].Data);
                }
                item.Data.SetAttributeValue("updated", lastUpdated.ToString());
                

                db.Items.Add(item);
            }
            //var photos = JsonConvert.DeserializeAnonymousType<List<Photo>>.DeserializeObject(photos_data, typeof(List<Photo>));

            var photoUsers = photos.Select(x => x.user_id).Distinct().ToList();
            var commentUsers = comments.Select(x => x.user_id).Distinct().ToList();

            int xx = 0;

        }
    }
}
