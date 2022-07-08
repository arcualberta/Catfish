using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Catfish.Core.Helpers
{
    public class BigList<T> : IList<T>, IEnumerator<T> where T : class
    {
        private int Total { get; set; }
        private int TotalPerPage { get; set; }
        private int CurrentPageIndex { get; set; }
        private int CurrentIndex { get; set; }
        private BigListPage<T> CurrentPage { get; set; }
        private List<string> Pages { get; set; }

        public T this[int index] { get => GetAtIndex(index); set => throw new NotImplementedException(); }

        public int Count => Total;

        public bool IsReadOnly => false;

        public T Current => CurrentIndex >= 0 && CurrentIndex < Total ? GetAtIndex(CurrentIndex) : null;

        object IEnumerator.Current => Current;

        public BigList(int totalPerPage = 1000){
            TotalPerPage = totalPerPage;
            Pages = new List<string>();
        }

        public void Add(T item)
        {
            if(CurrentPage == null || CurrentPage.Entries.Count >= TotalPerPage)
            {
                if (CurrentPage != null)
                {
                    SerializeBigListPage(CurrentPage);
                    ++CurrentPageIndex;
                }

                CurrentPage = new BigListPage<T>(TotalPerPage, "Catfish_" + Guid.NewGuid().ToString().Replace('-', '_'));
                Pages.Add(CurrentPage.Name);
            }

            CurrentPage.Entries.Add(item);
            ++Total;
        }

        public void AddRange(IEnumerable<T> items)
        {
            foreach(T item in items)
            {
                Add(item);
            }
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            Reset();
            return this;
        }

        public int IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            Reset();
            return this;
        }

        private T GetAtIndex(int index)
        {
            int page = index / TotalPerPage;

            if(page < Pages.Count)
            {
                int i = index - (page * TotalPerPage);
                if (page != CurrentPageIndex)
                {
                    SerializeBigListPage(CurrentPage);
                    CurrentPage = DeserializeBigListPage(Pages[page]);
                    CurrentPageIndex = page;
                }

                return CurrentPage.Entries[i];
            }

            return null;
        }

        private T SetAtIndex(T item, int index)
        {
            throw new NotImplementedException();
        }

        private void SerializeBigListPage(BigListPage<T> page)
        {
            string file = Path.Combine(Path.GetTempPath(), page.Name + ".cf");

            if (File.Exists(file))
            {
                File.Delete(file);
            }

            using (Stream stream = File.Open(file, FileMode.CreateNew))
            {
                FileInfo info = new FileInfo(file);
                info.Attributes = FileAttributes.Temporary;

                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(stream, page);
            }
        }

        private BigListPage<T> DeserializeBigListPage(string name)
        {
            string file = Path.Combine(Path.GetTempPath(), name + ".cf");
            BigListPage<T> output = null;

            using(Stream stream = File.Open(file, FileMode.Open))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                output = (BigListPage<T>)binaryFormatter.Deserialize(stream);
            }

            return output;
        }

        public void Dispose()
        {
            //Ignore for now.
            //throw new NotImplementedException();
        }

        public bool MoveNext()
        {
            ++CurrentIndex;

            return CurrentIndex < Total;
        }

        public void Reset()
        {
            CurrentIndex = -1;
        }

        public void ForEach(Func<T, bool> foreachFunc, Func<int, int, bool> afterPageComplete)
        {
            foreach (string pageName in Pages)
            {
                BigListPage<T> page = null;

                if (CurrentPage != null && CurrentPage.Name == pageName)
                {
                    page = CurrentPage;
                }
                else
                {
                    DeserializeBigListPage(pageName);
                }

                int successCount = 0;
                int failCount = 0;

                foreach (T entry in page.Entries)
                {
                    if (foreachFunc(entry))
                    {
                        ++successCount;
                    }
                    else
                    {
                        ++failCount;
                    }
                }

                afterPageComplete(successCount, failCount);
            }
        }
    }

    [Serializable]
    public class BigListPage<T>
    {
        public int PageSize { get; private set; }
        public string Name { get; private set; }
  
        private List<T> mEntries { get; set; }
        public List<T> Entries {
            get
            {
                if (mEntries == null)
                {
                    mEntries = new List<T>(PageSize);
                }

                return mEntries;
            }
        }

        public BigListPage(int pageSize, string name){
            PageSize = pageSize;
            Name = name;
        }

        
    }
}
