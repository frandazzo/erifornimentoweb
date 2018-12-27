using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace erifornimento.Utils
{
    public class ApiClient<T>  where T : class
    {

        readonly string url;
        public ApiClient(string url)
        {
            this.url = url;
        }

        public async Task<T> BasicCallAsync()
        {
            using (var client = new HttpClient())
            {
                var content = await client.GetStringAsync(this.url);
                return JsonConvert.DeserializeObject<T>(content);
            }
        }
   

        public async Task<T> CallAsync()
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, this.url))
            using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                var stream = await response.Content.ReadAsStreamAsync();

                if (response.IsSuccessStatusCode)
                    return DeserializeJsonFromStream(stream);

                var content = await StreamToStringAsync(stream);
                throw new ApiException
                {
                    StatusCode = (int)response.StatusCode,
                    Content = content
                };
            }

            //using (var client = new HttpClient())
            //using (var request = new HttpRequestMessage(HttpMethod.Get, this.url))
            //using (var response = await client.SendAsync(request, cancellationToken))
            //{
            //    var stream = await response.Content.ReadAsStreamAsync();

            //    if (response.IsSuccessStatusCode)
            //        return DeserializeJsonFromStream(stream);

            //    var content = await StreamToStringAsync(stream);
            //    throw new ApiException
            //    {
            //        StatusCode = (int)response.StatusCode,
            //        Content = content
            //    };
            //}



            //using (var client = new HttpClient())
            //using (var request = new HttpRequestMessage(HttpMethod.Get, this.url))
            //using (var response = await client.SendAsync(request, cancellationToken))
            //{
            //    var content = await response.Content.ReadAsStringAsync();

            //    if (response.IsSuccessStatusCode == false)
            //    {
            //        throw new ApiException
            //        {
            //            StatusCode = (int)response.StatusCode,
            //            Content = content
            //        };
            //    }

            //    return JsonConvert.DeserializeObject<List<T>>(content);
            //}
        }

        public  async Task<T> PostStreamAsync(object content)
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Post, this.url))
            using (var httpContent = CreateHttpContent(content))
            {
                request.Content = httpContent;

                using (var response = await client
                    .SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                    .ConfigureAwait(false))
                {
                    var stream = await response.Content.ReadAsStreamAsync();

                    if (response.IsSuccessStatusCode)
                        return DeserializeJsonFromStream(stream);

                    var content1 = await StreamToStringAsync(stream);
                    throw new ApiException
                    {
                        StatusCode = (int)response.StatusCode,
                        Content = content1
                    };
                }
            }
        }

        private HttpContent CreateHttpContent(object content)
        {
            HttpContent httpContent = null;

            if (content != null)
            {
                var ms = new MemoryStream();
                SerializeJsonIntoStream(content, ms);
                ms.Seek(0, SeekOrigin.Begin);
                httpContent = new StreamContent(ms);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            return httpContent;
        }

        public void SerializeJsonIntoStream(object value, Stream stream)
        {
            using (var sw = new StreamWriter(stream, new UTF8Encoding(false), 1024, true))
            using (var jtw = new JsonTextWriter(sw) { Formatting = Formatting.None })
            {
                var js = new JsonSerializer();
                js.Serialize(jtw, value);
                jtw.Flush();
            }
        }


        private  T DeserializeJsonFromStream(Stream stream)
        {
            if (stream == null || stream.CanRead == false)
                return default(T);

            using (var sr = new StreamReader(stream))
            using (var jtr = new JsonTextReader(sr))
            {
                var js = new JsonSerializer();
                var searchResult = js.Deserialize<T>(jtr);
                return searchResult;
            }
        }
        private async Task<string> StreamToStringAsync(Stream stream)
        {
            string content = null;

            if (stream != null)
                using (var sr = new StreamReader(stream))
                    content = await sr.ReadToEndAsync();

            return content;
        }


    }
}
