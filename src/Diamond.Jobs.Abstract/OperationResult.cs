using System.Collections.Generic;

namespace Diamond.Jobs.Abstract
{
    public sealed class OperationResult
    {
        private static readonly OperationResult success = new OperationResult(true);

        public bool Succeeded { get; private set; }

        public IEnumerable<string> Errors { get; private set; }

        public static OperationResult Success => success;

        public OperationResult(params string[] errors) : this((IEnumerable<string>)errors) { }

        public OperationResult(IEnumerable<string> errors)
        {
            if (errors == null)
            {
                errors = [OperationResultConstants.DefaultErrorMessage];
            }

            this.Succeeded = false;
            this.Errors = errors;
        }

        private OperationResult(bool success)
        {
            this.Succeeded = success;
            this.Errors = [];
        }

        public static OperationResult Failed(params string[] errors) => new OperationResult(errors);
    }
}