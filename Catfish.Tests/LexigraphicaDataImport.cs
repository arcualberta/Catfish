using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Catfish.Core.Models;
using System.Linq;
using Catfish.Core.Models.Forms;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using Catfish.Core.Services;

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
                catch(Exception)
                {
                    return null;
                }
            }

            public static List<T>InstantiateList<T>(string data) where T : LexigraphiBase
            {
                List<T> list = new List<T>();
                int n = data.IndexOf('[');
                data = data.Substring(n + 1);
                n = data.IndexOf(']');
                data = data.Substring(0, n);



                return list;
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


            var data = File.ReadAllText(@"C:\Users\Kamal\Documents\Projects\Catfish\Lexigraphica_Source_Data\comments.json");
            List<Comment> comments = LexigraphiBase.InstantiateList<Comment>(data);

            data = File.ReadAllText(@"C:\Users\Kamal\Documents\Projects\Catfish\Lexigraphica_Source_Data\photos.json");
            List<Photo> photos = LexigraphiBase.InstantiateList<Photo>(data);

            data = File.ReadAllText(@"C:\Users\Kamal\Documents\Projects\Catfish\Lexigraphica_Source_Data\photos_words.json");
            List<PhotoWord> photo_words = LexigraphiBase.InstantiateList<PhotoWord>(data);

            data = File.ReadAllText(@"C:\Users\Kamal\Documents\Projects\Catfish\Lexigraphica_Source_Data\phrases.json");
            List<Phrase> phrases = LexigraphiBase.InstantiateList<Phrase>(data);

            data = File.ReadAllText(@"C:\Users\Kamal\Documents\Projects\Catfish\Lexigraphica_Source_Data\users.json");
            List<User> users = LexigraphiBase.InstantiateList<User>(data);

            data = File.ReadAllText(@"C:\Users\Kamal\Documents\Projects\Catfish\Lexigraphica_Source_Data\words.json");
            List<Word> words = LexigraphiBase.InstantiateList<Word>(data);

            int? photoEntityTypeId = db.EntityTypes.Where(x => x.Name == "Photo").Select(x => x.Id).FirstOrDefault();
            Assert.IsTrue(photoEntityTypeId.Value > 0);

            int? commentEntityTypeId = db.EntityTypes.Where(x => x.Name == "Comment").Select(x => x.Id).FirstOrDefault();
            Assert.IsTrue(commentEntityTypeId.Value > 0);

            int? commentFormId = db.FormTemplates.ToList().Where(x => x.Name == "Comment Form").Select(x => x.Id).FirstOrDefault();
            Assert.IsTrue(commentFormId.Value > 0);

            
            List<Item> items = new List<Item>();
            ItemService itemSrv = new ItemService(db);
            SubmissionService subSrv = new SubmissionService(db);
            foreach(var photo in photos)
            {
                Item item = itemSrv.CreateEntity<Item>(photoEntityTypeId.Value);

                User creator = users.Where(u => u.id == photo.user_id).FirstOrDefault();
                item.AddAuditEntry(AuditEntry.eAction.Create, creator != null ? creator.username : "");

                MetadataSet photoMetadata = item.MetadataSets.Where(m => m.Name == "Photo Metadata").FirstOrDefault();

                FormField wordsField = photoMetadata.Fields.Where(f => f.Name == "Words").FirstOrDefault();
                List<PhotoWord> photoWords = photo_words.Where(w => w.photo_id == photo.id).ToList();
                List<string> ww = new List<string>();
                foreach(var pw in photoWords)
                {
                    string word = words.Where(w => w.id == pw.word_id).Select(w => w.word).FirstOrDefault();
                    ww.Add(word);
                }
                wordsField.SetValues(ww);

                FormField phrasesField = photoMetadata.Fields.Where(f => f.Name == "Phrases").FirstOrDefault();
                List<Phrase> photoPhrases = phrases.Where(p => p.photo_id == photo.id).ToList();
                phrasesField.SetValues(photoPhrases.Where(p => p.photo_id == photo.id).Select(p => p.phrase).ToList());


                List<Comment> photoComments = comments.Where(c => c.photo_id == photo.id).ToList();
                foreach(var comment in photoComments)
                {
                    Form commentForm = subSrv.CreateSubmissionForm(commentFormId.Value);
                    FormField commentField = commentForm.Fields.Where(f => f.Name == "Comment").FirstOrDefault();
                    commentField.SetValues(new List<string>() { comment.comment });

                    User commentUser = users.Where(u => u.id == comment.user_id).FirstOrDefault();
                    commentForm.AddAuditEntry(AuditEntry.eAction.Create, commentUser.username);



                    Item photoComment = subSrv.CreateEntity<Item>(commentEntityTypeId.Value) as Item;
                    //photoComment
                   // subSrv.c

                }

                db.Items.Add(item);


            }
            //var photos = JsonConvert.DeserializeAnonymousType<List<Photo>>.DeserializeObject(photos_data, typeof(List<Photo>));

            var photoUsers = photos.Select(x => x.user_id).Distinct().ToList();
            var commentUsers = comments.Select(x => x.user_id).Distinct().ToList();


            int xx = 0;

        }
    }
}
