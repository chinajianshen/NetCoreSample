using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBook.Bee.Core
{

    /// <summary>
    /// MEF测试用的
    /// </summary>
    public interface IService
    {
        string Do();
    }

    public interface ISecondService : IService
    {
        string SecondDo();
    }

    public class FirstModel : IService
    {
        private string first = "first";
        public string Do()
        {
            return first;
        }

    }

    public class SecondModel : ISecondService
    {
        private string second = "second";
        public string Do()
        {
            return second;
        }

        public string SecondDo()
        {
            return $"{second}-{second}";
        }
    }

    public class FirstModel2 : IService
    {
        private string first = "first2";
        public string Do()
        {
            return first;
        }
    }
    
    public class ThirdModel
    {
        private string third = "third";
        public string Do()
        {
            return third;
        }
    }

    public interface IAutoService
    {
        string ShowAuto();
    }

    public class AuotoModel :IAutoService
    {
        public string AutoTitle { get; set; }

        //构造函数有参数 现在没有找到解决方法
        //public AuotoModel(string autoTitle)
        //{
        //    this.AutoTitle = autoTitle;
        //}

        public string ShowAuto()
        {
            return "AuotoModel";
        }
    }

    public abstract class ATest
    {
        public abstract string title();
    }

    public class ATestModel : ATest
    {
        public override string title()
        {
            return "ATestModel";
        }
    }

    public interface IValidationRule
    {
        string Validate();
    }

    public abstract class CAbstract
    {
        public IValidationRule Rule { get; set; }
        public string Test()
        {
            return Rule.Validate();
        }
        public abstract string Do();
    }
    public class CInstence : IValidationRule
    {
        public string Validate()
        {
            return "CInstence";
        }
    }

    public class CInstence2: IValidationRule
    {
        public string Validate()
        {
            return "CInstence2";
        }
    }

    public class CClient : CAbstract
    {
        public override string Do()
        {
            return "CClient";
        }
    }

}
