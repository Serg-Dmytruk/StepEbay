using System;

namespace StepEbay.Common.Models.RefitModels
{
    public class BoolResult
    {
        public bool Value { get; set; }
        public string ErrorMessage { get; set; }
        public BoolResult(bool value)
        {
            Value = value;
        }
        public BoolResult(bool value, string errorMessage)
        {
            Value = value;
            ErrorMessage = errorMessage;
        }
    }
}
