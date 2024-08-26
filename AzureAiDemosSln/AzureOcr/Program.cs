using Azure;
using Azure.AI.Vision.ImageAnalysis;
using System.Runtime;

namespace AzureOcr
{
    internal class Program
    {
        public async static Task<ImageAnalysisResult> ReadImage(Stream stream)
        {
            AppSettings settings = new AppSettings();

            ImageAnalysisClient client;
            BinaryData imageData;

            client = new ImageAnalysisClient(
                new Uri(settings.AzureAiVisionEndpoint),
                new AzureKeyCredential(settings.AzureAiVisionKey));

            imageData = BinaryData.FromStream(stream);

            ImageAnalysisResult result = await client.AnalyzeAsync(imageData,
                        VisualFeatures.Caption | VisualFeatures.Read,
                        new ImageAnalysisOptions { GenderNeutralCaption = true });


            return result;
        }

        static async Task Main(string[] args)
        {
            MemoryStream ocrDocumentStream = new MemoryStream(File.ReadAllBytes(@".\Images\spectrum.jpg"));
            var result = await ReadImage(ocrDocumentStream);

            Console.WriteLine("Caption: " + result.Caption.Text);

            Console.WriteLine();

            if (result.Read != null)
            {
                foreach (var block in result.Read.Blocks)
                {
                    foreach (var line in block.Lines)
                    {
                        Console.WriteLine(line.Text);

                        var polygon = line.BoundingPolygon;

                        foreach (var point in polygon)
                        {
                            string position = point.ToString();
                            Console.Write(position + " ");
                        }

                        Console.WriteLine();
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
