﻿using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using NUnit.Framework;

namespace MvcDonutCaching.Tests
{

    public class TestsBase {
        [SetUp]
        public void SetupTask()
        {
            ClearCache();
        }

        [TearDown]
        public void TearDownTask()
        {
            UseDefaultSettingsGlobally();
        }

        protected void AssertRenderedDuringSameRequest(
            params DateTime[] times)
        {
            var baseTime = times.First();
            foreach(var dateTime in times)
            {
                Assert.That(dateTime, Is.EqualTo(baseTime).Within(TimeSpan.FromMilliseconds(100)));
            }
        }


        protected void AssertRenderedDuringLastRequest(
            params DateTime[] times)
        {
            var baseTime = DateTime.Now;
            foreach (var dateTime in times)
            {
                Assert.That(dateTime, Is.EqualTo(baseTime).Within(TimeSpan.FromMilliseconds(100)));
            }
        }

        private static void ClearCache()
        {
            var result = GetUrlContent("/TestControl/ClearCache");
            Assert.That((object)result, Is.EqualTo("Done"), "Failed to clear cache");
        }

        private static void EnableReplaceDonutsInChildActionsGlobally()
        {
            var result = GetUrlContent("/TestControl/EnableReplaceDonutsInChildActionsGlobally");
            Assert.That((object)result, Is.EqualTo("Done"), "Failed:EnableReplaceDonutsInChildActionsGlobally");
        }

        private static void UseDefaultSettingsGlobally()
        {
            var result = GetUrlContent("/TestControl/UseDefaultSettingsGlobally");
            Assert.That((object)result, Is.EqualTo("Done"), "Failed:UseDefaultSettingsGlobally");
        }

        public static string GetUrlContent(string relativeUrl)
        {
            var uri = string.Format("http://localhost:{0}{1}",49421, relativeUrl);
            var webRequest = WebRequest.Create(uri);
            webRequest.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);

            try
            {
                using(var response = webRequest.GetResponse())
                    return ReadResponse(response);
            }
            catch(WebException e)
            {
                string responseText = null;
                try
                {
                    responseText = ReadResponse(e.Response);
                }
                catch(Exception)
                {
                    
                }
                if(responseText != null)
                {
                    throw new Exception(responseText, e);
                }
                throw;
            }  
        }

        private static string ReadResponse(WebResponse response)
        {
            using(var stream = response.GetResponseStream())
            {
                using(var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}