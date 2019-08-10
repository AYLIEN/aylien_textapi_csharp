﻿#region License
/*
Copyright 2016 Aylien, Inc. All Rights Reserved.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
#endregion

using System.Collections.Generic;

namespace Aylien.TextApi
{
    /// <summary>
    /// A Client can make calls to the Text API.
    /// </summary>
    public class Client
    {
        private readonly Configuration configuration;
        private Dictionary<string, int> rateLimit = new Dictionary<string, int>
        {
            {"Limit", -1}, {"Remaining", -1}, {"Reset", -1}
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        /// <param name="appId">A key which you can get it from Aylien developer website</param>
        /// <param name="appKey">A key which you can get it from Aylien developer website</param>
        public Client(string appId, string appKey)
        {
            configuration = new Configuration(appId, appKey);
        }

        /// <summary>
        /// Extracts the main body of article, including embedded media such as
        /// images &amp; videos from a URL and removes all the surrounding clutter.
        /// </summary>
        /// <param name="url">A valid URL</param>
        /// <param name="html">HTML as string</param>
        /// <param name="bestImage">Whether extract the best image of the article</param>
        /// <returns>A <see cref="Extract"/></returns>
        public Extract Extract(string url = null, string html = null, bool bestImage = false, bool keepHtmlFormatting = false)
        {
            Extract extract = new Aylien.TextApi.Extract(configuration);
            Response r = extract.call(url, html, bestImage.ToString(), keepHtmlFormatting.ToString());
            extractRateLimitParameters(r);
            return extract;
        }

        /// <summary>
        /// Summarizes an article into a few key sentences.
        /// </summary>
        /// <param name="text">Text</param>
        /// <param name="title">Title</param>
        /// <param name="url">A Valid URL</param>
        /// <param name="mode">Analyze mode. Valid options are default
        ///   and short. Default is default. short mode produces shorter sentences.</param>
        /// <param name="sentencesNumber">Number of sentences to be returned.
        ///   Only in default mode (not applicable to short mode).
        ///   Default value is 5.
        ///   has precedence over sentencesPercentage.</param>
        /// <param name="sentencesPercentage">Percentage of sentences to be returned.
        ///   Only in default mode (not applicable to short mode).
        ///   Possible range is 1-100.
        ///   sentencesNumber has precedence over this parameter.</param>
        /// <returns>A <see cref="Summarize"/></returns>
        public Summarize Summarize(string text = null, string title = null, string url = null, string mode = null, int sentencesNumber = 0, int sentencesPercentage = 0)
        {
            Summarize summarize = new Aylien.TextApi.Summarize(configuration);
            Response r = summarize.call(text, title, url, mode, sentencesNumber.ToString(), sentencesPercentage.ToString());
            extractRateLimitParameters(r);
            return summarize;
        }

        /// <summary>
        /// Classifies a piece of text according to IPTC NewsCode standard.
        /// </summary>
        /// <param name="url">A valid URL</param>
        /// <param name="text">Text</param>
        /// <param name="language">Language of text. Valid options are
        ///  en, de, fr, es, it, pt, and auto. If set to auto, it'll try to
        ///  detect the language. Default is en.</param>
        /// <returns>A <see cref="Classify"/></returns>
        public Classify Classify(string url = null, string text = null, string language = null)
        {
            Classify classify = new Aylien.TextApi.Classify(configuration);
            Response r = classify.call(url, text, language);
            extractRateLimitParameters(r);
            return classify;
        }

        /// <summary>
        /// Classifies a piece of text according to specified taxonomy.
        /// </summary>
        /// <param name="taxonomy">Taxonomy to classify according to</param>
        /// <param name="url">A valid URL</param>
        /// <param name="text">Text</param>
        /// <param name="language">Language of text. Valid options are
        ///  en, de, fr, es, it, pt, and auto. If set to auto, it'll try to
        ///  detect the language. Default is en.</param>
        /// <returns>A <see cref="Classify"/></returns>
        public ClassifyByTaxonomy ClassifyByTaxonomy(string taxonomy, string url = null, string text = null, string language = null)
        {
            ClassifyByTaxonomy classify = new Aylien.TextApi.ClassifyByTaxonomy(configuration);
            Response r = classify.call(taxonomy, url, text, language);
            extractRateLimitParameters(r);
            return classify;
        }

        /// <summary>
        /// Detects sentiment of a document in terms of
        /// polarity ("positive" or "negative") and
        /// subjectivity ("subjective" or "objective").
        /// </summary>
        /// <param name="url">A valid URL</param>
        /// <param name="text">Text</param>
        /// <param name="mode">Analyze mode. Valid options are
        ///  tweet, and document. Default is tweet.</param>
        ///  <param name="language">Language</param>
        /// <returns>A <see cref="Sentiment"/></returns>
        public Sentiment Sentiment(string url = null, string text = null, string mode = null, string language = null)
        {
            Sentiment sentiment = new Aylien.TextApi.Sentiment(configuration);
            Response r = sentiment.call(url, text, mode, language);
            extractRateLimitParameters(r);
            return sentiment;
        }

        /// <summary>
        /// Extracts named entities (people, organizations and locations) and
        /// values (URLs, emails, telephone numbers, currency amounts and percentages)
        /// mentioned in a body of text.
        /// </summary>
        /// <param name="url">A valid URL</param>
        /// <param name="text">Text</param>
        /// <returns>A <see cref="Entities"/></returns>
        public Entities Entities(string url = null, string text = null)
        {
            Entities entities = new Aylien.TextApi.Entities(configuration);
            Response r = entities.call(url, text);
            extractRateLimitParameters(r);
            return entities;
        }

        /// <summary>
        /// Extracts named entities mentioned in a document, disambiguates and
        /// cross-links them to DBPedia and Linked Data entities, along with
        /// their semantic types (including DBPedia and schema.org).
        /// </summary>
        /// <param name="url">A valid URL</param>
        /// <param name="text">Text</param>
        /// <param name="language">Language of text. Valid options are
        /// en, de, fr, es, it, pt, and auto. If set to auto, it'll try to
        /// detect the language. Default is en.</param>
        /// <returns>A <see cref="Concepts"/></returns>
        public Concepts Concepts(string url = null, string text = null, string language = null)
        {
            Concepts concepts = new Aylien.TextApi.Concepts(configuration);
            Response r = concepts.call(url, text, language);
            extractRateLimitParameters(r);
            return concepts;
        }

        /// <summary>
        /// Suggests hashtags describing the document.
        /// </summary>
        /// <param name="url">A valid URL</param>
        /// <param name="text">Text</param>
        /// <param name="language">Language of text. Valid options are
        ///  en, de, fr, es, it, pt, and auto. If set to auto, it'll try to
        ///  detect the language. Default is en.</param>
        /// <returns>A <see cref="Hashtags"/></returns>
        public Hashtags Hashtags(string url = null, string text = null, string language = null)
        {
            Hashtags hashtags = new Aylien.TextApi.Hashtags(configuration);
            Response r = hashtags.call(url, text, language);
            extractRateLimitParameters(r);
            return hashtags;
        }

        /// <summary>
        /// Detects the main language a document is written in and returns it
        /// in ISO 639-1 format.
        /// </summary>
        /// <param name="url">A valid URL</param>
        /// <param name="text">Text</param>
        /// <returns>A <see cref="Language"/></returns>
        public Language Language(string url = null, string text = null)
        {
            Language language = new Aylien.TextApi.Language(configuration);
            Response r = language.call(url, text);
            extractRateLimitParameters(r);
            return language;
        }

        /// <summary>
        /// Runs multiple analysis operations in one API call.
        /// </summary>
        /// <param name="url">A valid URL</param>
        /// <param name="text">Text</param>
        /// <param name="endpoints">List of operations</param>
        /// <returns>A <see cref="Combined"/></returns>
        public Combined Combined(string url = null, string text = null, string[] endpoints = null)
        {
            Combined combined = new Aylien.TextApi.Combined(configuration);
            Response r = combined.call(url, text, endpoints);
            extractRateLimitParameters(r);
            return combined;
        }

        /// <summary>
        /// Assigns relevant tags to an image.
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns>A <see cref="ImageTags"/></returns>
        public ImageTags ImageTags(string url)
        {
            ImageTags imageTags = new Aylien.TextApi.ImageTags(configuration);
            Response r = imageTags.call(url);
            extractRateLimitParameters(r);
            return imageTags;
        }

        /// <summary>
        /// Detects aspects and sentiment of a body of text.
        /// </summary>
        /// <param name="domain">Domain which document belongs to</param>
        /// <param name="url">URL</param>
        /// <param name="text">Text</param>
        /// <returns>A <see cref="AspectBasedSentiment"/></returns>
        public AspectBasedSentiment AspectBasedSentiment(string domain, string url = null, string text = null)
        {
            AspectBasedSentiment aspectBasedSentiment = new Aylien.TextApi.AspectBasedSentiment(configuration);
            Response r = aspectBasedSentiment.call(domain, url, text);
            extractRateLimitParameters(r);
            return aspectBasedSentiment;
        }

        /// <summary>
        /// Returns the sentiment towards entities on given text
        /// </summary>
        /// <param name="url">A valid URL</param>
        /// <param name="text">Text</param>
        /// <returns>A <see cref="EntityLevelSentiment"/></returns>
        public EntityLevelSentiment EntityLevelSentiment(string url = null, string text = null)
        {
            EntityLevelSentiment elsa = new Aylien.TextApi.EntityLevelSentiment(configuration);
            Response r = elsa.call(url, text);
            extractRateLimitParameters(r);
            return elsa;
        }

        /// <summary>
        /// Returns Rate Limit of API calls.
        /// </summary>
        /// <returns> Return a Dictionary&lt;string, int&gt; of rate limit</returns>
        public Dictionary<string, int> RateLimit
        {
            get
            {
                if (rateLimit["Limit"] == -1 && rateLimit["Remaining"] == -1 && rateLimit["Reset"] == -1)
                {
                    Response r = new Aylien.TextApi.Language(configuration).call(null, "Test");
                    extractRateLimitParameters(r);
                }
                return rateLimit;
            }
            private set
            {
                rateLimit = value;
            }
        }

        private void extractRateLimitParameters(Response r){
            RateLimit = new Dictionary<string, int>
            {
                {"Limit", SafeParse(r.ResponseHeader["X-RateLimit-Limit"], -1)},
                {"Remaining", SafeParse(r.ResponseHeader["X-RateLimit-Remaining"], -1)},
                {"Reset", SafeParse(r.ResponseHeader["X-RateLimit-Reset"], -1)}
            };
        }

        /// <summary>
        /// Added this method because extractRateLimitParameters throws null exceptions 
        /// when the API response does not include X-RateLimit headers.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private int SafeParse(string value, int defaultValue) {
            int.TryParse(value, out defaultValue);
            return defaultValue;
        }
    }
}
