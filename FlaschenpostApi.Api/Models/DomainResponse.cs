using System;
namespace FlaschenpostApi.Api.Models
{
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
