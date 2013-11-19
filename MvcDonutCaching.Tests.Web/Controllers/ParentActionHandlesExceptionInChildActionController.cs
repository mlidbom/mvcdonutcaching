using System;
using System.Threading;
using System.Web.Mvc;
using DevTrends.MvcDonutCaching.Mlidbom;

namespace MvcDonutCaching.Tests.Web.Controllers
{
    [AutoOutputCache(Duration = .1)]
    public class ParentActionHandlesExceptionInChildActionController : Controller
    {
        private ActionResult DelayAndDisplay(ActionResult result)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(1));
            return result;
        }

        public ActionResult Index() { return DelayAndDisplay(View()); }

        public ActionResult TopLayoutHeader() { return DelayAndDisplay(View()); }

        public ActionResult TopLayoutFooter() { return DelayAndDisplay(View()); }

        public ActionResult NestedLayoutHeader() { return DelayAndDisplay(View()); }

        public ActionResult NestedLayoutFooter() { return DelayAndDisplay(View()); }

        public ActionResult PageBody() { return DelayAndDisplay(View()); }

        public ActionResult SwallowExceptionInNestedAction() { return DelayAndDisplay(View()); }

        public ActionResult SwallowExceptionInNestedView() { return DelayAndDisplay(View()); }

        public ActionResult SwallowExceptions() { return DelayAndDisplay(View()); }

        public ActionResult SwallowChildActionException() { return DelayAndDisplay(View()); }

        public ActionResult SwallowChildViewException() { return DelayAndDisplay(View()); }


        [AutoOutputCache(Duration = 0)]
        public ActionResult ThrowExceptionInView() { return DelayAndDisplay(View()); }

        public ActionResult ThrowInNestedChildView() { return DelayAndDisplay(View()); }
        
        public ActionResult ThrowInNestedChildAction() { return DelayAndDisplay(View()); }

        [AutoOutputCache(Duration = 0)]
        public ActionResult ThrowExceptionInAction()
        {
            throw new Exception("ExceptionTextThatShouldNeverBeShown");
        }
    }
}
