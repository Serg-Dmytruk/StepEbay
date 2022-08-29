using System;

namespace StepEbay.Common.Models.RefitModels
{
    public class BoolResult
    {
        public bool Value { get; set; }
        public BoolResult(bool value)
        {
            Value = value;
        }
    }
}
