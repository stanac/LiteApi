using System;
#pragma warning disable RECS0154 // Parameter is never used

namespace LiteApi.Tests.Controllers
{
    public class ActionOverloadController : LiteController
    {
        public string GetString(string a)
        {
            return "string a";
        }

        public string GetString(int a)
        {
            return "int a";
        }

        public string GetString(string a, string b)
        {
            return "string a, string b";
        }

        public string GetString(int a, int b)
        {
            return "int a, int b";
        }
        
        public string GetString(int a, int b, int c)
        {
            return "int a, int b, int c";
        }
        
        public string GetString(bool a)
        {
            return "bool a";
        }

        public string GetString(bool a, bool b)
        {
            return "bool a, bool b";
        }

        public string GetString(Guid a)
        {
            return "Guid a";
        }

        public string GetString(Guid a, Guid b)
        {
            return "Guid a, Guid b";
        }
        
        public string GetString(DateTime? a = null)
        {
            return "DateTime? a";
        }

        [DontMapToApi]
        public string NotMappedMethod()
        {
            return "";
        }
    }
}
#pragma warning restore RECS0154 // Parameter is never used
