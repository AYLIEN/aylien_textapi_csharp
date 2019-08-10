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

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Aylien.TextApi
{
    public class AspectBasedSentiment : Base
    {
        /// <summary>
        /// Default constructor added to provide better serialization support.
        /// </summary>
        public AspectBasedSentiment() : base() { }

        public AspectBasedSentiment(Configuration config) : base(config) { }

        internal Response call(string domain, string url, string text)
        {
            List<Dictionary<string, string>> parameters = new List<Dictionary<string, string>>();

            if (!String.IsNullOrWhiteSpace(url))
                parameters.Add(new Dictionary<string, string> { { "url", url } });

            if (!String.IsNullOrWhiteSpace(text))
                parameters.Add(new Dictionary<string, string> { { "text", text } });

            if (String.IsNullOrEmpty(domain))
                throw new Error("Invalid Domain. Domain can't be blank.");

            var endpoint = Configuration.Endpoints["AspectBasedSentiment"].Replace(":domain", domain);
            Connection connection = new Connection(endpoint, parameters, configuration);
            var response = connection.request();
            populateData(response.ResponseResult);

            return response;
        }

        public string Text { get; set; }
        public string Domain { get; set; }
        public Aspect[] Aspects { get; set; }
        public Sentence[] Sentences { get; set; }

        private void populateData(string jsonString)
        {
            AspectBasedSentiment m = JsonConvert.DeserializeObject<AspectBasedSentiment>(jsonString);

            Text = m.Text;
            Domain = m.Domain;
            Aspects = m.Aspects;
            Sentences = m.Sentences;
        }
    }

    public class Aspect
    {
        [JsonProperty("aspect")]
        public string _Aspect { get; set; }

        [JsonProperty("aspect_confidence")]
        public double AspectConfidence { get; set; }

        public string Polarity { get; set; }

        [JsonProperty("polarity_confidence")]
        public double PolarityConfidence { get; set; }
    }

    public class Sentence
    {
        public string Text { get; set; }
        public string Polarity { get; set; }

        [JsonProperty("polarity_confidence")]
        public double PolarityConfidence { get; set; }
        public Aspect[] Aspects { get; set; }
    }
}
