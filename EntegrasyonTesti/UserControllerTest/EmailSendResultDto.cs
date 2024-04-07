using ProgrammersBlog.Shared.Utilities.Results.Abstract;
using ProgrammersBlog.Shared.Utilities.Results.ComplexTypes;
using System;

namespace ProgrammersBlog.Tests
{
    internal class EmailSendResultDto : IResult
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }

        public ResultStatus ResultStatus => throw new NotImplementedException();

        public Exception Exception => throw new NotImplementedException();

        public bool? Success { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}