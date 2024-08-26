using Microsoft.CognitiveServices.Speech;

namespace TextToSpeech
{
    internal class Program
    {
        private static AppSettings _settings;
        private static string _speechKey;
        private static string _speechRegion;
        private static SpeechConfig? _speechConfig;
        public static void Initialize()
        {
            _settings = new AppSettings();

            _speechKey = _settings.AzureAiSpeechKey;
            _speechRegion = _settings.AzureSpeechRegion;

            _speechConfig = SpeechConfig.FromSubscription(_speechKey, _speechRegion);
            _speechConfig.SpeechSynthesisVoiceName = _settings.AzureSpeechSynthesisVoiceName1;
        }

        static async Task Main(string[] args)
        {

            Console.WriteLine("Hello, World!");

            await SpeakAsync("Hello World!");
            await SpeakAsync("I hope you are enjoying Microsoft Season of AI Cape Town");

            string? text = string.Empty;

            do
            {
                text = Console.ReadLine();
                await SpeakAsync(text);
            }
            while (!string.IsNullOrEmpty(text));
            

        }

        public static async Task SpeakAsync(string text)
        {

            if (_speechConfig == null)
            {
                Initialize();
            }

            using (var speechSynthesizer = new SpeechSynthesizer(_speechConfig))
            {
                var speechSynthesisResult = await speechSynthesizer.SpeakTextAsync(text);
                OutputSpeechSynthesisResult(speechSynthesisResult, text);
            }
        }

        private static void OutputSpeechSynthesisResult(SpeechSynthesisResult speechSynthesisResult, string text)
        {
            switch (speechSynthesisResult.Reason)
            {
                case ResultReason.SynthesizingAudioCompleted:
                    Console.WriteLine($"Speech synthesized for text: [{text}]");
                    break;
                case ResultReason.Canceled:
                    var cancellation = SpeechSynthesisCancellationDetails.FromResult(speechSynthesisResult);
                    Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                    if (cancellation.Reason == CancellationReason.Error)
                    {
                        Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                        Console.WriteLine($"CANCELED: ErrorDetails=[{cancellation.ErrorDetails}]");
                        Console.WriteLine($"CANCELED: Did you set the speech resource key and region values?");
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
