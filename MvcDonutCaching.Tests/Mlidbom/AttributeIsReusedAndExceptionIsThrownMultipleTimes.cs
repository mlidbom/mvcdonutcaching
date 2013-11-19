using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using DevTrends.MvcDonutCaching;
using DevTrends.MvcDonutCaching.Mlidbom;
using Moq;
using NUnit.Framework;

namespace MvcDonutCaching.Tests.Mlidbom
{
    public class AttributeIsReusedAndExceptionIsThrownMultipleTimes
    {
        [Test]
        public void OutputIsStillCorrectlyRestored()
        {
            var keyBuilderMock = new Mock<IKeyBuilder>(MockBehavior.Strict);

            var context = TestUtil.CreateMockActionExecutingControllerContext();

            var attribute = AutoOutputCacheAttribute.CreateForTestPurposes(
                keyBuilder: keyBuilderMock.Object,
                keyGenerator: new DummyKeyGenerator(keyBuilderMock.Object, "akey"));
            attribute.Duration = 3600;            

            var correctOutput = context.HttpContext.Response.Output;

            context.ActionDescriptor = new StaticActionDescriptor("controller", "action");

            //context.RouteData.Values["action"] = "action";
            //context.RouteData.Values["controller"] = "controller";

            attribute.OnActionExecuting(context);

        }
    }

    public class DummyKeyGenerator : KeyGenerator
    {
        public DummyKeyGenerator(IKeyBuilder keyBuilder, string key) : base(keyBuilder)
        {            
        }

        public override string GenerateKey(ControllerContext context, CacheSettings cacheSettings)
        {
            throw new Exception("Please please crash here. Not doing so is insane!");
        }

        override protected string AbsolutelyInsaneThatThisCouldEverBeCalledBecauseTheSubClassVersionOfGenerateKeyShouldHaveBeenCalled()
        {
            return Key;
        }

        public string Key { get; set; }
    }

}