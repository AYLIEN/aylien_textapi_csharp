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
using System.IO;
using System.Net;

namespace Aylien.TextApi
{
    /// <summary>
    /// Custom exception containing information returned from Aylien TextAPI (extracted from WebException).
    /// </summary>
    /// <remarks>
    /// The Aylien Text API will return an HTTP 4XX in the case of an exception, and will put the error into the body of the response. 
    /// This exception extracts the relevant information and presents in a more logical format.
    /// </remarks>
    public class Error: ApplicationException
    {
        /// <summary>
        /// The HTTP status code returned by Aylien Text API - usually a 4xx
        /// </summary>
        public HttpStatusCode Status { get; private set; }

        ///// <summary>
        ///// The HTTP response Message returned by Aylien Text API
        ///// </summary>
        public new string Message { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message">Message</param>
        public Error(string message)
        {
            this.Message = message;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="closeResponse">If True, the WebException.Response object is closed.</param>
        public Error(WebException ex, bool closeResponse)
        {
            this.Status = ((HttpWebResponse)ex.Response).StatusCode;
            this.Message = new StreamReader(((HttpWebResponse)ex.Response).GetResponseStream()).ReadToEnd();

            if (closeResponse)
            {
                ex.Response.Close();
            }
        }
    }
}