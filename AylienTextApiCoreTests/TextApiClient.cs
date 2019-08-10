//#define PAID_PLAN

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Configuration;

namespace Aylien.TextApi.Tests
{
    [TestClass]
    public class UnitTestClient
    {
        private string url, text, phrase, title, imageUrl, taxonomy, domain;
        private Client client;

        private void setRequireVariables()
        {
            var appId = ConfigurationManager.AppSettings["appId"];
            var appKey = ConfigurationManager.AppSettings["appKey"];

            client = new Client(appId, appKey);
            url = "http://www.bbc.co.uk/news/world-australia-30544493#sa-ns_mchannel=rss&ns_source=PublicRSS20-sa";
            imageUrl = "https://www.fujifilmusa.com/products/digital_cameras/x/fujifilm_x_m1/sample_images/img/index/ff_x_m1_018.JPG";
            text = "John is a very good football player!";
            phrase = "Dublin";
            title = "Test title";
            taxonomy = "iab-qag";
            domain = "airlines";
        }

        [TestMethod]
        [ExpectedException(typeof(Error))]
        public void ShouldThrowErrorWithUnauthenticatedClient()
        {
            Client invalidClient = new Client("WrongAppId", "WrongAppKey");
            Sentiment sentiment = invalidClient.Sentiment(text: "John is a bad football player");
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfClassify()
        {
            setRequireVariables();
            Classify classify = client.Classify(text: text);

            Assert.IsInstanceOfType(classify, typeof(Classify));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfConcepts()
        {
            setRequireVariables();
            Concepts concepts = client.Concepts(text: text);

            Assert.IsInstanceOfType(concepts, typeof(Concepts));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfEntities()
        {
            setRequireVariables();
            Entities entities = client.Entities(text: text);

            Assert.IsInstanceOfType(entities, typeof(Entities));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfExtract()
        {
            setRequireVariables();
            Extract extract = client.Extract(url: url);

            Assert.IsInstanceOfType(extract, typeof(Extract));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfHashtags()
        {
            setRequireVariables();
            Hashtags hashtags = client.Hashtags(text: text);

            Assert.IsInstanceOfType(hashtags, typeof(Hashtags));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfLanguage()
        {
            setRequireVariables();
            Language language = client.Language(text: text);

            Assert.IsInstanceOfType(language, typeof(Language));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfSentiment()
        {
            setRequireVariables();
            Sentiment sentiment = client.Sentiment(text: text);

            Assert.IsInstanceOfType(sentiment, typeof(Sentiment));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfSummarize()
        {
            setRequireVariables();
            Summarize summarize = client.Summarize(text: text, title: title);

            Assert.IsInstanceOfType(summarize, typeof(Summarize));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfCombined()
        {
            setRequireVariables();
            string[] endpoints = new string[] { "classify", "concepts", "entities", "extract", "hashtags", "language", "sentiment", "summarize" };
            Combined combined = client.Combined(url: url, endpoints: endpoints);

            Assert.IsInstanceOfType(combined, typeof(Combined));
        }

#if PAID_PLAN

        [TestMethod]
        public void ShouldReturnAnInstanceOfImageTags()
        {
            setRequireVariables();
            ImageTags imageTags = client.ImageTags(url: imageUrl);

            Assert.IsInstanceOfType(imageTags, typeof(ImageTags));
        }

#endif

        [TestMethod]
        public void ShouldReturnAnInstanceOfClassifyByTaxonomy()
        {
            setRequireVariables();
            ClassifyByTaxonomy classifyByTaxonomy = client.ClassifyByTaxonomy(taxonomy, url: url);

            Assert.IsInstanceOfType(classifyByTaxonomy, typeof(ClassifyByTaxonomy));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfAspectBasedSentiment()
        {
            setRequireVariables();
            AspectBasedSentiment aspectBasedSentiment = client.AspectBasedSentiment(domain, text: text);

            Assert.IsInstanceOfType(aspectBasedSentiment, typeof(AspectBasedSentiment));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfElsa()
        {
            setRequireVariables();
            EntityLevelSentiment elsa = client.EntityLevelSentiment(text: text);

            Assert.IsInstanceOfType(elsa, typeof(EntityLevelSentiment));
        }

        [TestMethod]
        public void ShouldNotReturnEmptyRateLimit()
        {
            setRequireVariables();
            Dictionary<string, int> rateLimit = client.RateLimit;

            Assert.AreNotEqual(rateLimit["Limit"], -1);
            Assert.AreNotEqual(rateLimit["Remaining"], -1);
            Assert.AreNotEqual(rateLimit["Reset"], -1);
        }
    }
}
