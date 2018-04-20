using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace AerialObjectRecognitionFunction
{
    public static class OnNewImage
    {
        [FunctionName("OnNewImage")]
        public static async Task<object> Run([HttpTrigger(WebHookType = "genericJson")]HttpRequestMessage req, TraceWriter log)
        {
            try
            {
                log.Info($"Webhook was triggered!");

                // Get request body
                dynamic data = await req.Content.ReadAsAsync<object>();
                log.Info(data.ToString());

                // Get image url
                string imagePath = data.path;
                string imageUrl = $"https://{ConfigurationManager.AppSettings["StorageAccountName"]}.blob.core.windows.net{imagePath}";

                log.Info(imageUrl);

                // Get image prediction results
                var predictionResult = await ImagePredictor.PredictImageUrl(imageUrl);

                // Validate image
                bool objectFound = ImagePredictor.ValidateImagePrediction(predictionResult);
                Models.Output output = new Models.Output()
                {
                    ObjectFound = objectFound,
                    ImageUrl = imageUrl,
                };

                return req.CreateResponse(HttpStatusCode.OK, output);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
