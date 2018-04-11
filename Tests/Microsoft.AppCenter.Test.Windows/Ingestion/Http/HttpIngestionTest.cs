﻿using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AppCenter.Ingestion.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.AppCenter.Test.Windows.Ingestion.Http
{
    [TestClass]
    public class HttpIngestionTest : BaseIngestionTest
    {
        private HttpIngestion _httpIngestion;

        [TestInitialize]
        public void InitializeHttpIngestionTest()
        {
            _adapter = new Mock<IHttpNetworkAdapter>();
            _httpIngestion = new HttpIngestion(_adapter.Object);
        }

        /// <summary>
        /// Verify that ingestion call http adapter and not fails on success.
        /// </summary>
        [TestMethod]
        public async Task HttpIngestionStatusCodeOk()
        {
            SetupAdapterSendResponse(HttpStatusCode.OK);
            using (var call = _httpIngestion.Call(AppSecret, InstallId, Logs))
            {
                await call.ToTask();
                VerifyAdapterSend(Times.Once());
            }

            // No throw any exception
        }

        /// <summary>
        /// Verify that ingestion throw exception on error response.
        /// </summary>
        [TestMethod]
        public async Task HttpIngestionStatusCodeError()
        {
            SetupAdapterSendResponse(HttpStatusCode.NotFound);
            using (var call = _httpIngestion.Call(AppSecret, InstallId, Logs))
            {
                await Assert.ThrowsExceptionAsync<HttpIngestionException>(() => call.ToTask());
                VerifyAdapterSend(Times.Once());
            }
        }

        /// <summary>
        /// Verify that ingestion don't call http adapter when call is closed.
        /// </summary>
        [TestMethod]
        public async Task HttpIngestionCancel()
        {
            _adapter
                .Setup(a => a.SendAsync(
                    It.IsAny<string>(),
                    "POST",
                    It.IsAny<IDictionary<string, string>>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .Returns(async () =>
                {
                    await Task.Delay(500);
                    return "";
                });
            using (var call = _httpIngestion.Call(AppSecret, InstallId, Logs))
            {
                call.Cancel();
                await Assert.ThrowsExceptionAsync<TaskCanceledException>(() => call.ToTask());
            }
        }

        /// <summary>
        /// Verify that ingestion create headers correctly.
        /// </summary>
        [TestMethod]
        public void HttpIngestionCreateHeaders()
        {
            var headers = _httpIngestion.CreateHeaders(AppSecret, InstallId);
            
            Assert.IsTrue(headers.ContainsKey(HttpIngestion.AppSecret));
            Assert.IsTrue(headers.ContainsKey(HttpIngestion.InstallId));
        }
    }
}