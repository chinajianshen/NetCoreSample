using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transfer8Pro.Entity
{
    public class ParamtersForDBPageEntity
    {
        public string TableName { get; set; }
        private string _Fields = "*";

        public string Fields
        {
            get { return _Fields; }
            set { _Fields = value; }
        }

        public string OrderField { get; set; }
        private string _sqlWhere = " 1=1 ";

        public string SqlWhere
        {
            get { return _sqlWhere; }
            set { _sqlWhere = value; }
        }

        private int _pageSize = 50;

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }

        private int _pageIndex = 1;

        public int PageIndex
        {
            get { return _pageIndex; }
            set { _pageIndex = value; }
        }

        private bool _UserPagination = true;

        public bool UserPagination
        {
            get { return _UserPagination; }
            set { _UserPagination = value; }
        }
    }

    public class ParamtersForDBPageEntity<T> : ParamtersForDBPageEntity where T : class
    {
        public IList<T> DataList { get; set; }     

        public int Total { get; set; }
    }
}
