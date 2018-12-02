namespace cg.Api.Controllers.Helper
{
    public class CustomResponse
    {
        public CustomResponse(bool hasError,string message)
        {
            HasError = hasError;
            Message = message;
        }
        public bool HasError { get; set; }
        public string Message { get; set; }

        public static CustomResponse Error(string message)=>new CustomResponse(true,message);
        public static CustomResponse Success(string message) => new CustomResponse(true, message);

    }

}
