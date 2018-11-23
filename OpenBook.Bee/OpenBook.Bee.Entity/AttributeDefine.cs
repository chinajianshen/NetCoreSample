using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBook.Bee.Entity
{
    public class WCFServiceAttribute : Attribute
    {

    }


    [AttributeUsage(AttributeTargets.Method)]
    public class WebSocketCommandAttribute : Attribute
    {
        public string CommandName { get; set; }

        public WebSocketCommandAttribute(string name)
        {
            CommandName = name;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        private string _TableName;

        public string TableName
        {
            get { return _TableName; }
            set { _TableName = value; }
        }

        public TableAttribute(string tablename)
        {
            _TableName = tablename;
        }

    }


    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKeyAttribute : Attribute { }


    [AttributeUsage(AttributeTargets.Property)]
    public class UpdateFiledAttribute : Attribute
    {

    }





    [AttributeUsage(AttributeTargets.Property)]
    public class WhereFiledAttribute : Attribute
    {
        private WhereDefaultNullValueType _NullValueType;

        public WhereDefaultNullValueType NullValueType
        {
            get { return _NullValueType; }
            set { _NullValueType = value; }
        }
        public WhereFiledAttribute(WhereDefaultNullValueType nullvaluetype)
        {
            _NullValueType = nullvaluetype;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class WhereOperationAttribute : Attribute
    {
        private WhereOperationTypes _whereopt;

        public WhereOperationTypes Whereopt
        {
            get { return _whereopt; }
            set { _whereopt = value; }
        }
        public WhereOperationAttribute(WhereOperationTypes t)
        {
            _whereopt = t;
        }
    }

    //[AttributeUsage(AttributeTargets.Property)]
    //public class DefaultOrderFieldAttribute : Attribute
    //{
    //    private int _OrderWeight;

    //    public int OrderWeight
    //    {
    //        get { return _OrderWeight; }
    //        set { _OrderWeight = value; }
    //    }

    //    private OrderFieldType _orderType;

    //    public OrderFieldType OrderType
    //    {
    //        get { return _orderType; }
    //        set { _orderType = value; }
    //    }

    //    public DefaultOrderFieldAttribute(int orderweight, OrderFieldType ordertype)
    //    {
    //        _orderType = ordertype;
    //        _OrderWeight = orderweight;
    //    }
    //    public DefaultOrderFieldAttribute()
    //    {
    //        _OrderWeight = 0;
    //        _orderType = OrderFieldType.Asc;
    //    }
    //}

    [AttributeUsage(AttributeTargets.Property)]
    public class OrderIndexAttribute : Attribute
    {
        private int _OrderIndex;

        public int OrderIndex
        {
            get { return _OrderIndex; }
            set { _OrderIndex = value; }
        }

        public OrderIndexAttribute(int orderindex)
        {
            _OrderIndex = orderindex;
        }
    }
    [AttributeUsage(AttributeTargets.Property)]
    public class MustWhereFiledForDeleteAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreThisFiledAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Class)]
    public class DBIDAttribute : Attribute
    {
        private int _DBID;

        public int DBID
        {
            get { return _DBID; }
            set { _DBID = value; }
        }
        public DBIDAttribute(int dbid)
        {
            this._DBID = dbid;
        }
    }

    public class KeyDisplayAttribute : Attribute
    {
        public KeyDisplayAttribute(string text)
        {
            this._DisplayText = text;
        }
        private string _DisplayText;
        /// <summary>
        /// 含义
        /// </summary>
        public string DisplayText
        {
            get { return _DisplayText; }
            set { _DisplayText = value; }
        }
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Property)]
    public class CommonAttribute : Attribute
    {
        public CommonAttribute()
        {

        }
        public string V1 { get; set; }
        public string V2 { get; set; }
        public string V3 { get; set; }
    }

    public enum WhereOperationTypes
    {
        Equals,
        Less_Than,
        Greater_Than,
        LessOrEquals,
        GreaterOrEquals
    }

    public enum WhereDefaultNullValueType
    {
        /// <summary>
        /// -1
        /// </summary>
        Int_Minus_One,
        /// <summary>
        /// datetime.min
        /// </summary>
        DateTime_Min,
        /// <summary>
        /// empty
        /// </summary>
        String_Empty
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class ExcelChartIDAttribute : Attribute
    {
        public ExcelChartIDAttribute(string chartid)
        {
            this.ChartID = chartid;
        }
        public string ChartID { get; set; }
    }
}

