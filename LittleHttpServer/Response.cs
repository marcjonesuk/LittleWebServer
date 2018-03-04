using System;
using System.Net;

namespace LittleHttpServer
{
    public class Response
    {
        public int StatusCode { get; set; }
        public string Body { get; set; }
        public virtual string ContentType { get; set; }

        internal WebHeaderCollection Headers;

        public string Allow
        {
            get
            {
                return Headers["Allow"];
            }
            set
            {
                Headers["Allow"] = value;
            }
        }

        public string CacheControl
        {
            get
            {
                return Headers["Cache-Control"];
            }
            set
            {
                Headers["Cache-Control"] = value;
            }
        }

        public string ContentEncoding
        {
            get
            {
                return Headers["Content-Encoding"];
            }
            set
            {
                Headers["Content-Encoding"] = value;
            }
        }

        public string ContentLanguage
        {
            get
            {
                return Headers["Content-Language"];
            }
            set
            {
                Headers["Content-Language"] = value;
            }
        }

        public string ContentLocation
        {
            get
            {
                return Headers["Content-Location"];
            }
            set
            {
                Headers["Content-Location"] = value;
            }
        }

        public string ETag
        {
            get
            {
                return Headers["ETag"];
            }
            set
            {
                Headers["ETag"] = value;
            }
        }

        public string LastModified
        {
            get
            {
                return Headers["Last-Modified"];
            }
            set
            {
                Headers["Last-Modified"] = value;
            }
        }

        public Response()
        {
            Headers = new WebHeaderCollection();
            StatusCode = 200;
        }
    }
}
