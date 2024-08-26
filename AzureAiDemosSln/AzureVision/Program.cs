using Azure;
using Azure.AI.Vision.ImageAnalysis;

namespace AzureObjectDetection
{
    internal class Program
    {
        public async static Task<ImageAnalysisResult> ObjectDetect(Stream stream)
        {
            AppSettings settings = new AppSettings();

            ImageAnalysisClient client;
            BinaryData imageData;

            client = new ImageAnalysisClient(
                new Uri(settings.AzureAiVisionEndpoint),
                new AzureKeyCredential(settings.AzureAiVisionKey));

            imageData = BinaryData.FromStream(stream);

            ImageAnalysisResult result = await client.AnalyzeAsync(imageData,
                        VisualFeatures.Caption | VisualFeatures.Objects | VisualFeatures.DenseCaptions,
                        new ImageAnalysisOptions { GenderNeutralCaption = true });


            return result;
        }


        static async Task Main(string[] args)
        {
            MemoryStream ocrDocumentStream = new MemoryStream(File.ReadAllBytes(@".\Images\input.jpg"));
            var result = await ObjectDetect(ocrDocumentStream);

            Console.WriteLine("Caption: " + result.Caption.Text);

            Console.WriteLine();

            if (result.Objects != null)
            {
                foreach (var detectedObject in result.Objects.Values)
                {
                    foreach (var tag in detectedObject.Tags)
                        Console.WriteLine(tag.Name + " " + tag.Confidence);
                    var boundingBox = detectedObject.BoundingBox;
                    Console.WriteLine(boundingBox);
                    Console.WriteLine();
                }
            }

            Console.WriteLine();

            if (result.DenseCaptions != null)
            {
                foreach (var denseCaption in result.DenseCaptions.Values)
                {
                    Console.WriteLine(denseCaption.Text);
                    var boundingBox = denseCaption.BoundingBox;
                    Console.WriteLine(boundingBox);
                    Console.WriteLine();
                }
            }

        }
    }
}
