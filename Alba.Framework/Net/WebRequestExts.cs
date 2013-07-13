using System.Net;
using Alba.Framework.Sys;

namespace Alba.Framework.Net
{
    public static class WebRequestExts
    {
        public static WebRequest CloneRequest (this WebRequest source)
        {
            if (source == null)
                return null;
            WebRequest copy = WebRequest.Create(source.RequestUri);
            copy.AuthenticationLevel = source.AuthenticationLevel;
            copy.CachePolicy = source.CachePolicy;
            copy.ConnectionGroupName = source.ConnectionGroupName;
            copy.ContentType = source.ContentType;
            copy.Credentials = source.Credentials;
            // TODO Copy special headers
            foreach (string headerName in source.Headers.AllKeys)
                copy.Headers[headerName] = source.Headers[headerName];
            copy.ImpersonationLevel = source.ImpersonationLevel;
            copy.Method = source.Method;
            copy.PreAuthenticate = source.PreAuthenticate;
            copy.Proxy = source.Proxy;
            copy.Timeout = source.Timeout;
            copy.UseDefaultCredentials = source.UseDefaultCredentials;

            CopyHttpRequestProps(source, copy);
            CopyFtpRequestProps(source, copy);
            CopyFileRequestProps(source, copy);

            return copy;
        }

        private static void CopyHttpRequestProps (WebRequest source, WebRequest copy)
        {
            var httpSource = source as HttpWebRequest;
            if (httpSource == null)
                return;

            var httpCopy = (HttpWebRequest)copy;
            httpCopy.Accept = httpSource.Accept;
            httpCopy.AllowAutoRedirect = httpSource.AllowAutoRedirect;
            httpCopy.AllowReadStreamBuffering = httpSource.AllowReadStreamBuffering;
            httpCopy.AllowWriteStreamBuffering = httpSource.AllowWriteStreamBuffering;
            httpCopy.AutomaticDecompression = httpSource.AutomaticDecompression;
            httpCopy.ClientCertificates = httpSource.ClientCertificates;
            httpCopy.Connection = httpSource.Connection;
            httpCopy.ContinueTimeout = httpSource.ContinueTimeout;
            httpCopy.CookieContainer = httpSource.CookieContainer;
            httpCopy.Date = httpSource.Date;
            httpCopy.Expect = httpSource.Expect;
            httpCopy.Host = httpSource.Host;
            httpCopy.IfModifiedSince = httpSource.IfModifiedSince;
            httpCopy.KeepAlive = httpSource.KeepAlive;
            httpCopy.MaximumAutomaticRedirections = httpSource.MaximumAutomaticRedirections;
            httpCopy.MaximumResponseHeadersLength = httpSource.MaximumResponseHeadersLength;
            httpCopy.MediaType = httpSource.MediaType;
            httpCopy.Pipelined = httpSource.Pipelined;
            httpCopy.ProtocolVersion = httpSource.ProtocolVersion;
            httpCopy.ReadWriteTimeout = httpSource.ReadWriteTimeout;
            httpCopy.Referer = httpSource.Referer;
            httpCopy.SendChunked = httpSource.SendChunked;
            httpCopy.ServerCertificateValidationCallback = httpSource.ServerCertificateValidationCallback;
            httpCopy.TransferEncoding = httpSource.TransferEncoding;
            httpCopy.UnsafeAuthenticatedConnectionSharing = httpSource.UnsafeAuthenticatedConnectionSharing;
            httpCopy.UserAgent = httpSource.UserAgent;
        }

        private static void CopyFtpRequestProps (WebRequest source, WebRequest copy)
        {
            var ftpSource = source as FtpWebRequest;
            if (ftpSource == null)
                return;

            var ftpCopy = (FtpWebRequest)copy;
            ftpCopy.EnableSsl = ftpSource.EnableSsl;
            ftpCopy.KeepAlive = ftpSource.KeepAlive;
            ftpCopy.ReadWriteTimeout = ftpSource.ReadWriteTimeout;
            ftpCopy.RenameTo = ftpSource.RenameTo;
            ftpCopy.UseBinary = ftpSource.UseBinary;
            ftpCopy.UsePassive = ftpSource.UsePassive;
        }

        private static void CopyFileRequestProps (WebRequest source, WebRequest copy)
        {
            var fileSource = source as FileWebRequest;
            if (fileSource == null)
                return;

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            copy.To<FileWebRequest>();
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }
    }
}