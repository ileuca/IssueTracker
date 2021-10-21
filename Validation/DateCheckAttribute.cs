
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.Web.Mvc;

namespace IssueTracker.Validation
{
    public enum Compare
    {
        GreaterThan,
        LessThan
    }
    public class DateCheckAttribute : ValidationAttribute
    {
        public string StartDate { get; private set; }
        public string EndDate { get; private set; }
        public Compare Compare { get; set; }

        public DateCheckAttribute(string startDate, string endDate, Compare type)
            : base()
        {
            StartDate = startDate;
            EndDate = endDate;
            Compare = type;
        }
        public override string FormatErrorMessage(string message)
        {
            return message;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var startDate = validationContext.ObjectInstance.GetType().GetProperty(StartDate);
            var endDate = validationContext.ObjectInstance.GetType().GetProperty(EndDate);
            var startDateValue = startDate.GetValue(validationContext.ObjectInstance, null);
            var endDateValue = endDate.GetValue(validationContext.ObjectInstance, null);

            if (value is DateTime && (startDateValue is DateTime) && (endDateValue is DateTime))
            {
                if (Compare == Compare.GreaterThan)
                {
                    bool equals = ((DateTime)startDateValue) > ((DateTime)endDateValue);
                    if (!equals)
                        return new ValidationResult(FormatErrorMessage(StartDate + " must be greater than " + EndDate));

                }
                else if (Compare == Compare.LessThan)
                {
                    bool equals = ((DateTime)startDateValue) < ((DateTime)endDateValue);
                    if (!equals)
                        return new ValidationResult(FormatErrorMessage(StartDate + " must be less than " + EndDate));

                }
            }
            return ValidationResult.Success;
        }
    }
}