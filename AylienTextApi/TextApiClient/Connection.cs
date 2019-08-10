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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Aylien.TextApi
{
    class Connection
    {
        private List<Dictionary<string, string>> Parameters { get; set; }
        private HttpWebRequest Request { set; get; }
        private string RequestUri { set; get; }

        public Connection(string endpoint, List<Dictionary<string, string>> parameters, Configuration configuration)
        {
            RequestUri = configuration.BaseUri + endpoint;
            Parameters = parameters;
            compileRequestParams(configuration);
        }

        internal Response request()
        {
            try
            {
                var response = (HttpWebResponse)Request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                Response returnResponse = new Response(responseString, response.Headers);
                response.Close();

                return returnResponse;
            }
            catch (WebException we)
            {
                throw new Error(we, true);
            }
        }

        private void compileRequestParams(Configuration configuration)
        {
            HttpWebRequest _request;

            if (configuration.Method == "POST")
            {
                _request = (HttpWebRequest)WebRequest.Create(RequestUri);
                _request.Method = configuration.Method;

                _request.Headers.Add(Configuration.Headers["AppKey"], configuration.AppKey);
                _request.Headers.Add(Configuration.Headers["AppId"], configuration.AppId);
                _request.UserAgent = configuration.UserAgent;

                var postData = Parameters.Aggregate("",
                  (memo, pair) =>
                     "&" + pair.First().Key + "=" + EscapeDataString(pair.First().Value) + memo
                  ).Substring(1);

                var data = Encoding.UTF8.GetBytes(postData);

                _request.ContentType = "application/x-www-form-urlencoded";
                _request.ContentLength = data.Length;

                using (var stream = _request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                    stream.Close();
                }
            }
            else if (configuration.Method == "GET")
            {
                var query = Parameters.Aggregate("",
                  (memo, pair) =>
                     "&" + pair.First().Key + "=" + EscapeDataString(pair.First().Value) + memo
                     );

                if (query != null && query.Length > 2)
                    query = query.Substring(1);

                if (Parameters.Count > 0)
                    _request = (HttpWebRequest)WebRequest.Create(RequestUri + "?" + query);
                else
                    _request = (HttpWebRequest)WebRequest.Create(RequestUri);
                _request.Method = configuration.Method;

                _request.Headers.Add(Configuration.Headers["AppKey"], configuration.AppKey);
                _request.Headers.Add(Configuration.Headers["AppId"], configuration.AppId);
                _request.UserAgent = configuration.UserAgent;
            }
            else
            {
                throw new ArgumentException("Method should be GET or POST.");
            }

            Request = _request;
        }

        /// <summary>
        /// Improved EscapeDataString to replace Url.EscapeDataString, which has a 64K string limitation.
        /// </summary>
        /// <param name="stringToEscape"></param>
        /// <returns></returns>
        private string EscapeDataString(string stringToEscape) {
            const int chunkSize = 32766;                // Ensure that this will work on any .Net platform, incl Store / portable
            var originalSize = stringToEscape.Length;   // Save the original size to make it easier to allocate the StringBuilder at 10% larger to account for encoding.
            var loops = originalSize / chunkSize;       // Number of chunkSize sized loops.

            // Allocate the StringBuilder with some 'air' for encoded characters.
            StringBuilder sb = new StringBuilder(originalSize + originalSize / 10);

            // Iterate the string by chunkSize, and call the native .Net API for each safe-sized chunk.
            for (int loop = 0; loop <= loops; loop++) {
                if (loop < loops) {
                    sb.Append(Uri.EscapeDataString(stringToEscape.Substring(chunkSize * loop, chunkSize)));
                }
                else {
                    sb.Append(Uri.EscapeDataString(stringToEscape.Substring(chunkSize * loop)));
                }
            }

            // Return the chunked and escaped string.
            return sb.ToString();
        }
    }

    class Response
    {
        public Response(string responseString, WebHeaderCollection headers)
        {
            ResponseResult = responseString;
            ResponseHeader = headers;
        }

        internal string ResponseResult { get; set; }
        internal WebHeaderCollection ResponseHeader { get; set; }
    }
}
