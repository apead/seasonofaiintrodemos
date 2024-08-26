using Microsoft.CognitiveServices.Speech;

namespace AzureOpenAi.Speech
{
    public class AzureSpeech
    {
        private  AppSettings _settings;
        private  string _speechKey;
        private  string _speechRegion;
        private  SpeechConfig? _speechConfig;
        public void Initialize()
        {
            _settings = new AppSettings();

            _speechKey = _settings.AzureAiSpeechKey;
            _speechRegion = _settings.AzureSpeechRegion;

            _speechConfig = SpeechConfig.FromSubscription(_speechKey, _speechRegion);
            _speechConfig.SpeechSynthesisVoiceName = _settings.AzureSpeechSynthesisVoiceName2;
        }
        public async Task SpeakAsync(string text)
        {

            if (_speechConfig == null)
            {
                Initialize();
            }

            using (var speechSynthesizer = new SpeechSynthesizer(_speechConfig))
            {
                var speechSynthesisResult = await speechSynthesizer.SpeakTextAsync(text);
             //   OutputSpeechSynthesisResult(speechSynthesisResult, text);
            }
        }

        private void OutputSpeechSynthesisResult(SpeechSynthesisResult speechSynthesisResult, string text)
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
