using System;
#pragma warning disable RECS0154 // Parameter is never used

namespace LiteApi.Tests.Controllers
{
    public class TestStringController : LiteController
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
    }
}
#pragma warning restore RECS0154 // Parameter is never used
