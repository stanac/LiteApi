﻿#pragma warning disable RECS0154 // Parameter is never used

namespace LiteApi.Tests.Controllers
{
    public class Two : LiteController
    {
        public int GetTheValue(FakeTwoModelOne model)
        {
            return model.I + model.J;
        }

        [HttpDelete]
        public int DeleteTheValue(FakeTwoModelOne model)
        {
            return model.I + model.J;
        }

        [ActionRoute("")]
        public int NoSegments()
        {
            return 1;
        }
        
        [RequiresAuthorizationPolicy("NonExistingPolicy")]
        [ActionRoute("{param1}/{param2}/{param3}")]
        public int NoConstantSegments(int param1, int param2, int param3)
        {
            return 1;
        }
    }

    [RequiresAuthorizationPolicy("NonExistingPolicy2")]
    public class TwoController : LiteController
    {
        [HttpPost]
        public int PostInt (FakeTwoModelOne m1, FakeTwoModelOne m2)
        {
            return m1.J + m2.I;
        }

        [ActionRoute("/a/{a}")]
        public int RouteParamWithDefaultValue(int a = 0)
        {
            return a + 1;
        }

        [ActionRoute("/ab/{b}")]
        public int RouteSegmentWithoutParam()
        {
            return 0;
        }

        public int RouteParamWithoutSegment([FromRoute]int b)
        {
            return 1 + b;
        }

        
    }

    public class FakeTwoModelOne
    {
        public int I { get; set; }
        public int J { get; set; }
    }
}
#pragma warning restore RECS0154 // Parameter is never used
