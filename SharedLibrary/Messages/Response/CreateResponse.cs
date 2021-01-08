using System;
namespace SharedLibrary.Messages.Response
{
    [Serializable]
    public class CreateResponse : Message
    {
        string error;
        bool hasError;

        public string Error { get => error; set => error = value; }
        public bool HasError { get => hasError; set => hasError = value; }

        public CreateResponse(string error)
        {
            hasError = true;
        }
        public CreateResponse()
        {
            hasError = false;
        }
    }
}
