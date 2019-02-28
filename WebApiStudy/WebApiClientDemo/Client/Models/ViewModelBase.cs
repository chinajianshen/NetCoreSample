using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Client.Models
{
    /// <summary>
    /// 所有的ViewModel都从这个类派生
    /// ModelError属性表示是这个Model对象的错误，而不只是针对Model中的某个属性
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged, IDataErrorInfo
    {
        private Dictionary<string, object> _values = new Dictionary<string, object>();
        private Dictionary<string, object> _snapshot;

        public void TakeSnapshot()
        {
            _snapshot = _values.ToDictionary(entry => entry.Key, entry => entry.Value);
        }

        public void RecoverFromSnapshot()
        {
            if(_snapshot!=null)
                _values = _snapshot.ToDictionary(entry => entry.Key, entry => entry.Value);
            ClearExtraError();
            NotifyPropertyChanged("");
        }

        public bool IsDataBeModifiedFromSnapshot()
        {
            if (_snapshot == null)
            {
                return false;
            }
            return _snapshot.Keys.Any(key => key != "ModelType" && _values[key] != _snapshot[key]);
        }

        private string GetPropertyName(LambdaExpression expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new InvalidOperationException();
            }

            return memberExpression.Member.Name;
        }

        protected void SetValue<T>(Expression<Func<T>> propertySelector, T value)
        {
            SetValue(GetPropertyName(propertySelector), value);
        }

        protected void SetValue<T>(string propertyName, T value)
        {
            _values[propertyName] = value;
            NotifyPropertyChanged(propertyName);
        }

        protected T GetValue<T>(Expression<Func<T>> propertySelector)
        {
            return GetValue<T>(GetPropertyName(propertySelector));
        }

        protected T GetValue<T>(string propertyName)
        {
            object value;
            if (!_values.TryGetValue(propertyName, out value))
            {
                value = default(T);
                _values.Add(propertyName, value);
            }
            return (T)value;
        }

        protected virtual string OnValidate(string propertyName)
        {
            string error = null;
            object value;
            if (_values.TryGetValue(propertyName, out value))
            {
                var results = new List<ValidationResult>(1);
                var result = Validator.TryValidateProperty(
                    value,
                    new ValidationContext(this, null, null)
                    {
                        MemberName = propertyName
                    },
                    results);

                if (!result)
                {
                    var validationResult = results.First();
                    error = validationResult.ErrorMessage;
                }
            }
            return error;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        protected void NotifyPropertyChanged<T>(Expression<Func<T>> propertySelector)
        {
            var propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                string propertyName = GetPropertyName(propertySelector);
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        //经过大量研究和实验，我发觉这个属性在WPF中没法用，故自行实现了ModelError
        string IDataErrorInfo.Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string ModelError
        {
            get
            {
                if (!string.IsNullOrEmpty(m_strExtraError))
                    return m_strExtraError;
                return ModelValidate();
            }
        }

        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                if (propertyName == "ModelError")
                {
                    return ModelError;
                }
                return OnValidate(propertyName);
            }
        }

        /// <summary>
        /// 派生类通过重载这个方法来实现Model对象级别的验证
        /// </summary>
        /// <returns>ModelError的出错内容</returns>
        public virtual string ModelValidate()
        {
            return null;
        }

        /// <summary>
        /// 整个Model是否完全正确？
        /// </summary>
        /// <returns></returns>
        public bool IsModelValid()
        {
            ClearExtraError();
            NotifyPropertyChanged("");
            PropertyInfo[] properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (properties.Where(p => p.CanWrite && p.CanRead).Any(p => OnValidate(p.Name) != null)) return false;
            return ModelValidate() == null;
        }

        /// <summary>
        /// ExtraError通常用于向用户显示执行错误信息
        /// </summary>
        /// <param name="strExtraError"></param>
        public void SetExtraError(string strExtraError)
        {
            m_strExtraError = strExtraError;
            NotifyPropertyChanged("");
        }

        public void ClearExtraError()
        {
            m_strExtraError = null;
        }

        protected string m_strExtraError;
    }

    public enum EnuModelType
    {
        View,
        Add,
        Edit
    }
}
