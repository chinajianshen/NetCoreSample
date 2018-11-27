using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBook.Bee.Entity
{
    /// <summary>
    /// 数据表字段类型
    /// </summary>
    public class DataFieldType
    {
        private string _filedname;

        public string FiledName
        {
            get { return _filedname; }
            set { _filedname = value; }
        }
        private string _type;

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private int _length;

        public int Length
        {
            get { return _length; }
            set { _length = value; }
        }
    }

    public class DataFieldTypeCollection : IList<DataFieldType>
    {
        IList<DataFieldType> lists = new List<DataFieldType>();
        #region IList<DataFieldType> 成员

        public int IndexOf(DataFieldType item)
        {
            return lists.IndexOf(item);
        }

        public void Insert(int index, DataFieldType item)
        {
            lists.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            lists.RemoveAt(index);
        }

        public DataFieldType this[int index]
        {
            get
            {
                return lists[index];
            }
            set
            {
                lists[index] = value;
            }
        }

        #endregion

        #region ICollection<DataFieldType> 成员

        public void Add(DataFieldType item)
        {
            lists.Add(item);
        }

        public void Clear()
        {
            lists.Clear();
        }

        public bool Contains(DataFieldType item)
        {
            return lists.Contains(item);
        }

        public void CopyTo(DataFieldType[] array, int arrayIndex)
        {
            lists.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return lists.Count; }
        }

        public bool IsReadOnly
        {
            get { return lists.IsReadOnly; }
        }

        public bool Remove(DataFieldType item)
        {
            return lists.Remove(item);
        }

        #endregion

        #region IEnumerable<DataFieldType> 成员

        public IEnumerator<DataFieldType> GetEnumerator()
        {
            return lists.GetEnumerator();
        }

        #endregion

        #region IEnumerable 成员

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return lists.GetEnumerator();
        }

        #endregion
    }
}
