using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Net.Http.Headers;

namespace TrailsHelper.Support.HttpProgressHandler
{
    internal static class HttpHeaderExtensions
    {
        public static void CopyTo(this HttpContentHeaders fromHeaders, HttpContentHeaders toHeaders)
        {
            Contract.Assert(fromHeaders != null, "fromHeaders cannot be null.");
            Contract.Assert(toHeaders != null, "toHeaders cannot be null.");

            foreach (KeyValuePair<string, IEnumerable<string>> header in fromHeaders)
            {
                toHeaders.TryAddWithoutValidation(header.Key, header.Value);
            }
        }
    }
}
