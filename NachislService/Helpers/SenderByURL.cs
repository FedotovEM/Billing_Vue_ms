using NachislService.Models;
using Newtonsoft.Json;
using System.Text;

namespace NachislService.Helpers
{
    public class SenderByURL
    {
        /// <summary>
        /// Отправляет HTTP-запрос
        /// </summary>
        /// <param name="dataToHTTPRequest">Данные для HTTP-запроса</param>
        /// <param name="urlPath">URL, по которому необходимо отправить запрос</param>
        /// <returns>Объект ответа от запрашиваемого метода</returns>
        public static async Task<PayResponseHist> SendHTTPRequest<T>(T dataToHTTPRequest, string urlPath)
        {
            string json = JsonConvert.SerializeObject(dataToHTTPRequest);
            PayResponseHist payMonthResult = new PayResponseHist();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(urlPath, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    payMonthResult = JsonConvert.DeserializeObject<PayResponseHist>(responseContent);
                }
            }
            return payMonthResult;
        }

    }
}
