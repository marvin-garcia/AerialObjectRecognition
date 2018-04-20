using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Cognitive.CustomVision.Prediction;
using Microsoft.Cognitive.CustomVision.Prediction.Models;


namespace AerialObjectRecognitionFunction
{
    /// <summary>
    /// 
    /// </summary>
    public class ImagePredictor
    {
        private static string projectId = ConfigurationManager.AppSettings["PredictionProjectId"];
        private static string predictionKey = ConfigurationManager.AppSettings["PredictionKey"];

        private static double objectMinimumScore = 0.5;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        public static async Task<ImagePredictionResultModel> PredictImageUrl(string imageUrl)
        {
            PredictionEndpoint endpoint = new PredictionEndpoint()
            {
                ApiKey = predictionKey,
            };
            
            var result = await endpoint.PredictImageUrlAsync(new Guid(projectId), new ImageUrl(imageUrl), iterationId: new Guid("3a06600a-2917-41de-9a9a-95a70157b007"));

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predictionResult"></param>
        /// <returns></returns>
        public static bool ValidateImagePrediction(ImagePredictionResultModel predictionResult)
        {
            var objectTag = predictionResult.Predictions.Where(x => x.Tag == "object").FirstOrDefault();
            var oceanTag = predictionResult.Predictions.Where(x => x.Tag == "ocean").FirstOrDefault();

            bool objectFound = false;

            if (objectTag != null && objectTag.Probability >= objectMinimumScore)
                objectFound = true;

            return objectFound;
        }
    }
}
