using Microsoft.AspNetCore.Http;

namespace DebtsAPI.Test.Fakes
{
    public class FakeHttpContextAccessor : IHttpContextAccessor
    {
        public FakeHttpContextAccessor()
        {

        }

        public virtual HttpContext HttpContext { get; set; }

    }
}
