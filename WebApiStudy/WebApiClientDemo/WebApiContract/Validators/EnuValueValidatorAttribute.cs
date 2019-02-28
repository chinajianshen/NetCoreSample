using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebApiContract.Validators
{
    /// <summary>
    /// 用法: [EnuValueValidator("Value1", "Value2", "Value3")]
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class EnuValueValidatorAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "{0} 不合法.";

        private readonly List<string> m_values = new List<string>();

        public EnuValueValidatorAttribute(params string[] param)
            : base(DefaultErrorMessage)
        {
            foreach (string s in param)
            {
                m_values.Add(s);
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!(value is string) || m_values.All(v => v != (string)value))
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName), new[] { validationContext.MemberName });
            }
            return ValidationResult.Success;
        }
    }
}
