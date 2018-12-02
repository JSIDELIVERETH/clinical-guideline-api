using System.Collections.Generic;

namespace cg.Api.Controllers.Helper
{

    public class BaseResult<T> where T: new()
    {
        public IList<T> Data { get; set; }
    }

    public static class BaseResultWrapper
    {
        public static BaseResult<T> ToBaseResult<T>(this List<T> t) where T: new ()
        {
            var result = new BaseResult<T>()
            {
                Data = t
            };
            return result;
        }
    }
}
