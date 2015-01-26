﻿#region License
/*
Copyright 2014 Aylien, Inc. All Rights Reserved.

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
        private Configuration configuration;
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
        /// images & videos from a URL and removes all the surrounding clutter.
        /// </summary>
        /// <param name="url">A valid URL</param>
        /// <param name="html">HTML as string</param>
        /// <param name="bestImage">Whether extract the best image of the article</param>
        /// <returns>A <see cref="Extract"/></returns>
        public Extract Extract(string url = null, string html = null, bool bestImage = false)
        {
            Extract extract = new Aylien.TextApi.Extract(configuration);
            Response r = extract.call(url, html, bestImage.ToString());
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
        /// Detects sentiment of a document in terms of
        /// polarity ("positive" or "negative") and
        /// subjectivity ("subjective" or "objective").
        /// </summary>
        /// <param name="url">A valid URL</param>
        /// <param name="text">Text</param>
        /// <param name="mode">Analyze mode. Valid options are
        ///  tweet, and document. Default is tweet.</param>
        /// <returns>A <see cref="Sentiment"/></returns>
        public Sentiment Sentiment(string url = null, string text = null, string mode = null)
        {
            Sentiment sentiment = new Aylien.TextApi.Sentiment(configuration);
            Response r = sentiment.call(url, text, mode);
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
        /// Returns phrases related to the provided unigram, or bigram.
        /// </summary>
        /// <param name="phrase">Phrase</param>
        /// <param name="count">Number of phrases to return.
        ///  Default is 20. Max is 100.</param>
        /// <returns>A <see cref="Related"/></returns>
        public Related Related(string phrase = null, int count = 0)
        {
            Related related = new Aylien.TextApi.Related(configuration);
            Response r = related.call(phrase, count.ToString());
            extractRateLimitParameters(r);
            return related;
        }

        /// <summary>
        /// Returns Rate Limit of API calls.
        /// </summary>
        /// <returns> Return a Dictionary<string, int> of rate limit</returns>
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
                {"Limit", int.Parse(r.ResponseHeader["X-RateLimit-Limit"])},
                {"Remaining", int.Parse(r.ResponseHeader["X-RateLimit-Remaining"])},
                {"Reset", int.Parse(r.ResponseHeader["X-RateLimit-Reset"])}
            };
        }
    }
}
