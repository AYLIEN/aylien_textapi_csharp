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
    public class Configuration
    {
        internal static readonly Dictionary<string, string> Endpoints = new Dictionary<string, string>
        {
	        {"Extract", "extract"},
	        {"Classify", "classify"},
            {"ClassifyByTaxonomy", "classify/:taxonomy"},
	        {"Summarize", "summarize"},
	        {"Concepts", "concepts"},
	        {"Sentiment", "sentiment"},
	        {"Language", "language"},
	        {"Hashtags", "hashtags"},
	        {"Entities", "entities"},
            {"Elsa", "elsa"},
            {"Combined", "combined"},
            {"ImageTags", "image-tags"},
            {"AspectBasedSentiment", "absa/:domain"}
        };

        internal static readonly Dictionary<string, string> Headers = new Dictionary<string, string>
        {
            {"AppKey", "X-AYLIEN-TextAPI-Application-Key"},
            {"AppId", "X-AYLIEN-TextAPI-Application-ID"}
        };

        private readonly string defaultBaseUri = "https://api.aylien.com/api/v1/";
        private readonly string defaultMethod = "POST";
        private readonly string defaultUserAgent = "Aylien Text API C# Lib 1.12.0.0";

        public Configuration(string appId, string appKey)
        {
            this.AppId = appId;
            this.AppKey = appKey;
        }

        internal string AppId { get; set;}
        internal string AppKey { get; set;}

        internal string BaseUri
        {
            get { return defaultBaseUri; }
        }

        internal string Method
        {
            get { return defaultMethod; }
        }

        internal string UserAgent
        {
            get { return defaultUserAgent; }
        }
    }
}
