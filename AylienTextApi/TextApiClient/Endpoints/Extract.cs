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

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Aylien.TextApi
{
    public class Extract : Base
    {
        public Extract(Configuration config) : base(config) { }

        internal Response call(string url, string html, string bestImage, Dictionary<string, string> extraParameters)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            if (!String.IsNullOrWhiteSpace(url))
                parameters["url"] = url;

            if (!String.IsNullOrWhiteSpace(html))
                parameters["html"] = html;

            if (!String.IsNullOrWhiteSpace(bestImage))
                parameters["best_image"] = bestImage;

            if (extraParameters != null && extraParameters.Count > 0)
                foreach (var keyValue in extraParameters)
                {
                    if (!parameters.ContainsKey(keyValue.Key) && !String.IsNullOrWhiteSpace(keyValue.Key))
                        parameters.Add(keyValue.Key, keyValue.Value);
                }

            Connection connection = new Connection(Configuration.Endpoints["Extract"], parameters, configuration);
            var response = connection.request();
            populateData(response.ResponseResult);

            return response;
        }

        public string Title { get; set; }
        public string Article { get; set; }
        public string Image { get; set; }
        public string Author { get; set; }
        public string[] Videos { get; set; }
        public string[] Feeds { get; set; }

        private void populateData(string jsonString)
        {
            Extract m = JsonConvert.DeserializeObject<Extract>(jsonString);

            Title = m.Title;
            Article = m.Article;
            Image = m.Image;
            Author = m.Author;
            Feeds = m.Feeds;
            Videos = m.Videos;
        }
    }
}
