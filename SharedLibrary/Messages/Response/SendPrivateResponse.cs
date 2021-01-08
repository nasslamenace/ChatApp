using System;
namespace SharedLibrary.Messages.Response
{
    [Serializable]
    public class SendPrivateResponse : Message
    {
        bool hasError;
        string error;

        public bool HasError { get => hasError; set => hasError = value; }
        public string Error { get => error; set => error = value; }

        public SendPrivateResponse(string error)
        {
            this.error = error;
            hasError = true;
        }

        public SendPrivateResponse()
        {

            hasError = false ;
        }


    }
}
