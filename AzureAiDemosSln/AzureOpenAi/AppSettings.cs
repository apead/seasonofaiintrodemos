namespace AzureOpenAi
{
    public class AppSettings
    {
        public string EndpointUrl = "<OPENAIENDPOINTURL>";
        public string OpenAiKey = "<OPENAIKEY>";
        public string DeploymentName = "<OPENAIMODELDEPLOYMENT>";


        public string AzureAiSpeechKey = "<AZURESPEECHKEY>";
        public string AzureSpeechRegion = "eastus";
        public string AzureSpeechSynthesisVoiceName1 = "af-ZA-AdriNeural";
        public string AzureSpeechSynthesisVoiceName2 = "en-ZA-LukeNeural";

        public string SystemBehaviourMessage1 = "You are a helpful assistant that loves the Microsoft Kaapstad Seisoen van Kunsmatige intelligensie event.  You love to gush about how cool it is with every response.  You only speak afrikaans.";
        public string SystemBehaviourMessage2 = "You are a helpful assistant that loves the Microsoft Season of AI event.  You love to gush about how cool it is with every response.";

    }
}
