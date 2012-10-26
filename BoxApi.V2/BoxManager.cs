﻿using System;
using System.IO;
using System.Net;
using System.Threading;

namespace BoxApi.V2
{
    /// <summary>
    ///   Provides methods for using Box.NET SOAP web service
    /// </summary>
    public partial class BoxManager
    {
        private readonly BoxRestClient _restClient;
        private readonly RequestHelper _requestHelper;

        /// <summary>
        ///   Instantiates BoxManager
        /// </summary>
        /// <param name="applicationApiKey"> The unique API key which is assigned to application </param>
        /// <param name="authorizationToken"> Valid authorization ticket </param>
        /// <param name="proxy"> Proxy information </param>
        public BoxManager(string applicationApiKey, string authorizationToken, IWebProxy proxy = null)
        {
            _requestHelper = new RequestHelper();
            _restClient = new BoxRestClient(new RequestAuthenticator(applicationApiKey, authorizationToken), proxy);
        }

        private static void GuardFromNull(object arg, string argName)
        {
            if (arg == null || (arg is string && string.IsNullOrEmpty((string) arg)))
            {
                throw new ArgumentException("Argument cannot be null or empty", argName);
            }
        }

        private static void Backoff(int attempt)
        {
            Thread.Sleep((int) Math.Pow(2, attempt)*100);
        }

        private static byte[] ReadFully(Stream input)
        {
            var buffer = new byte[16*1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}