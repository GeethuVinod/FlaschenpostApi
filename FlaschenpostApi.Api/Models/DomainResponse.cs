namespace FlaschenpostApi.Api.Models
{
    /// <summary>
    /// Wrapper class for response, any domain validation error or other business logic error could go here
    /// </summary>
    /// <typeparam name="TDomainModel"></typeparam>
    public class DomainResponse<TDomainModel>
      where TDomainModel : class
    {
        public TDomainModel Result { get; set; }
        public string ErrorMessage { get; set; }

        public DomainResponse(TDomainModel result, string error = "")
        {
            Result = result;
            ErrorMessage = error;
        }
    }
}
