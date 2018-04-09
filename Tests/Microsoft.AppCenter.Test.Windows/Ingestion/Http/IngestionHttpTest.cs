﻿using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AppCenter.Ingestion;
using Microsoft.AppCenter.Ingestion.Http;
using Microsoft.AppCenter.Ingestion.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.AppCenter.Test.Ingestion.Http
{
    [TestClass]
    public class IngestionHttpTest : HttpIngestionTest
    {
        private HttpIngestion _httpIngestion;

        [TestInitialize]
        public void InitializeIngestionHttpTest()
        {
            _adapter = new Mock<IHttpNetworkAdapter>();
            _httpIngestion = new HttpIngestion(_adapter.Object);
        }

        /// <summary>
        /// Verify that ingestion call http adapter and not fails on success.
        /// </summary>
        [TestMethod]
        public void IngestionHttpStatusCodeOk()
        {
            var call = PrepareServiceCall();
            SetupAdapterSendResponse(HttpStatusCode.OK);
            _httpIngestion.ExecuteCallAsync(call).RunNotAsync();
            VerifyAdapterSend(Times.Once());

            // No throw any exception
        }

        /// <summary>
        /// Verify that ingestion throw exception on error response.
        /// </summary>
        [TestMethod]
        public void IngestionHttpStatusCodeError()
        {
            var call = PrepareServiceCall();
            SetupAdapterSendResponse(HttpStatusCode.NotFound);
            Assert.ThrowsException<HttpIngestionException>(() => _httpIngestion.ExecuteCallAsync(call).RunNotAsync());
            VerifyAdapterSend(Times.Once());
        }

        /// <summary>
        /// Verify that ingestion don't call http adapter when call is closed.
        /// </summary>
        [TestMethod]
        public void IngestionHttpCancel()
        {
            var call = PrepareServiceCall();
            call.Cancel();
            SetupAdapterSendResponse(HttpStatusCode.OK);
            _httpIngestion.ExecuteCallAsync(call).RunNotAsync();
            VerifyAdapterSend(Times.Never());
        }

        /// <summary>
        /// Verify that ingestion prepare ServiceCall correctly.
        /// </summary>
        [TestMethod]
        public void IngestionHttpPrepareServiceCall()
        {
            var appSecret = Guid.NewGuid().ToString();
            var installId = Guid.NewGuid();
            var logs = new List<Log>();
            var call = _httpIngestion.PrepareServiceCall(appSecret, installId, logs);
            Assert.IsInstanceOfType(call, typeof(HttpServiceCall));
            Assert.AreEqual(call.Ingestion, _httpIngestion);
            Assert.AreEqual(call.AppSecret, appSecret);
            Assert.AreEqual(call.InstallId, installId);
            Assert.AreEqual(call.Logs, logs);
        }

        /// <summary>
        /// Verify that ingestion create headers correctly.
        /// </summary>
        [TestMethod]
        public void IngestionHttpCreateHeaders()
        {
            var appSecret = Guid.NewGuid().ToString();
            var installId = Guid.NewGuid();
            var headers = _httpIngestion.CreateHeaders(appSecret, installId);
            
            Assert.IsTrue(headers.ContainsKey(HttpIngestion.AppSecret));
            Assert.IsTrue(headers.ContainsKey(HttpIngestion.InstallId));
        }

        /// <summary>
        /// Helper for prepare ServiceCall.
        /// </summary>
        private IServiceCall PrepareServiceCall()
        {
            var appSecret = Guid.NewGuid().ToString();
            var installId = Guid.NewGuid();
            var logs = new List<Log>();
            return _httpIngestion.PrepareServiceCall(appSecret, installId, logs);
        }
    }
}