using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewCurator
{
    public class ReviewDownloadException : Exception
    {
        public ReviewDownloadException(string message, System.Exception inner) : base(message, inner) { }

        public ReviewDownloadException(string message) : base(message) { }
        public ReviewDownloadException() : base() { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client. 
        protected ReviewDownloadException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
        { }
    }
}
