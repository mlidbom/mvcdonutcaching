using System;
using System.Threading;
using System.Web.Mvc;
using NCrunch.Framework;
using NUnit.Framework;

namespace MvcDonutCaching.Tests.Mlidbom
{
    public abstract class PageThatSwallowsExceptionInChildActionsAndViews : ControllerTestBase
    {
        private const string Controller = "ParentActionHandlesExceptionInChildAction";
        override protected string ControllerName { get { return Controller; } }

        protected abstract string ActionName { get; }

        public override void RunAfterEachTest()
        {
            ClearActionCache(Controller, ActionName);
        }

        [Test]
        public void DoesNotCrashOnInitialRender()
        {
            ExecuteAction(ActionName);
        }

        [Test]
        public void SubsequentCallsRenderIdenticalContent()
        {
            var initialContent = ExecuteAction(ActionName);

            long requestsMade = 0;

            var runUntil = DateTime.Now + TimeSpan.FromSeconds(1);

            while(DateTime.Now < runUntil)
            {
                ++requestsMade;
                var newContent = ExecuteAction(ActionName);
                Assert.That(newContent, Is.EqualTo(initialContent));
            }

            Console.WriteLine("Made {0} requests with no failure");
        }
    }

    [ExclusivelyUses("ParentActionHandlesExceptionInChildAction_Index")]
    public class PageThatSwallowsExceptionInChildActionsAndViews_Index : PageThatSwallowsExceptionInChildActionsAndViews
    {
        override protected string ActionName { get { return "Index"; } }
    }

    [ExclusivelyUses("ParentActionHandlesExceptionInChildAction_SwallowChildActionException")]
    public class PageThatSwallowsExceptionInChildActionsAndViews_SwallowChildActionException : PageThatSwallowsExceptionInChildActionsAndViews
    {
        override protected string ActionName { get { return "SwallowChildActionException"; } }
    }

    [ExclusivelyUses("ParentActionHandlesExceptionInChildAction_SwallowChildViewException")]
    public class PageThatSwallowsExceptionInChildActionsAndViews_SwallowChildViewException : PageThatSwallowsExceptionInChildActionsAndViews
    {
        override protected string ActionName { get { return "SwallowChildViewException"; } }
    }

    [ExclusivelyUses("ParentActionHandlesExceptionInChildAction_SwallowExceptionInNestedAction")]
    public class PageThatSwallowsExceptionInChildActionsAndViews_SwallowExceptionInNestedAction : PageThatSwallowsExceptionInChildActionsAndViews
    {
        override protected string ActionName { get { return "SwallowExceptionInNestedAction"; } }
    }

    [ExclusivelyUses("ParentActionHandlesExceptionInChildAction_SwallowExceptionInNestedView")]
    public class PageThatSwallowsExceptionInChildActionsAndViews_SwallowExceptionInNestedView : PageThatSwallowsExceptionInChildActionsAndViews
    {
        override protected string ActionName { get { return "SwallowExceptionInNestedView"; } }
    }

    [ExclusivelyUses("ParentActionHandlesExceptionInChildAction_SwallowExceptions")]
    public class PageThatSwallowsExceptionInChildActionsAndViews_SwallowExceptions : PageThatSwallowsExceptionInChildActionsAndViews
    {
        override protected string ActionName { get { return "SwallowExceptions"; } }
    }
}