using Catfish.Core.Models.Contents;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Linq;

namespace Catfish.Core.Models.Contents
{
    public class XmlModelList<T> : IList<T> where T : XmlModel
    {
        private XElement mData;
        private List<T> mList = new List<T>();
        //public XmlModelList(string tagName)
        //{ 
        //    mData = new XElement(tagName);
        //}

        /// <summary>
        /// Instantiate a new XmlModelList object which encapsulates a given XElement
        /// </summary>
        /// <param name="data">Encapsulating XElement (child container)</param>
        /// <param name="populateChildren">Set to true if the constructor should go through the
        /// the children of the container and build the list of children into the array.
        /// </param>
        /// <param name="childTags">Only applicable if populateChildren is set to true.
        /// If childTags is assigned a single tag name, selects child elements with that tag name for 
        /// building the list. If it is assigned with a comman-separted list of tag names, then 
        /// the returned list will contain elements with any of those tags.
        /// </param>
        public XmlModelList(XElement data, bool populateChildren = true, string childTags = null)
        {
            mData = data;

            if (populateChildren)
            {

                IEnumerable<XElement> elements = data.Elements();
                string[] tags = string.IsNullOrEmpty(childTags) ? null : childTags.Split(new char[] { ',' });
                foreach (var ele in elements)
                {
                    if (tags != null && !tags.Contains(ele.Name.LocalName))
                        continue;

                    T child = XmlModel.InstantiateContentModel(ele) as T;
                    if (child != null)
                    {
                        //Initializing the child. We are not changing its GUID, even if it doesn't exit at all.
                        child.Initialize(XmlModel.eGuidOption.Ignore); 
                        mList.Add(child);
                    }
                }
            }
        }

        public T this[int index] 
        { 
            get => ((IList<T>)mList)[index];
            set => throw new NotImplementedException(); //((IList<T>)mList)[index] = value; 
        }

        public int Count => ((IList<T>)mList).Count;

        public bool IsReadOnly => ((IList<T>)mList).IsReadOnly;

        public void Add(T item)
        {
            mData.Add(item.Data);
            ((IList<T>)mList).Add(item);
        }

        public void Clear()
        {
            foreach (var item in mList)
                item.Data.Remove();

            ((IList<T>)mList).Clear();
        }

        public bool Contains(T item)
        {
            return ((IList<T>)mList).Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            ((IList<T>)mList).CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IList<T>)mList).GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return ((IList<T>)mList).IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            //We cannot implement this method because apparenrly we cannot
            //insert an element to a specific index in the child element list in an
            //XElement object.
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            if (mList.Contains(item))
                item.Data.Remove();
            return ((IList<T>)mList).Remove(item);
        }

        public void RemoveAt(int index)
        {
            mList[index].Data.Remove();
            ((IList<T>)mList).RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<T>)mList).GetEnumerator();
        }

        public T Find(Guid id)
        {
            return ((IList<T>)mList)
                .Where(item => item.Data.Attribute("id").Value == id.ToString())
                .FirstOrDefault();
        }

        public T FindByAttribute(string attName, string attValue)
        {
            return ((IList<T>)mList)
                .Where(item => item.Data.Attribute(attName).Value == attValue)
                .FirstOrDefault();
        }

    }
}
