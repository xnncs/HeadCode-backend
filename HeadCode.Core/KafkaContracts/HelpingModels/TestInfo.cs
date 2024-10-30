namespace HeadCode.Core.KafkaContracts.HelpingModels;

public class TestInfo
{
    public string InputData { get; set; }
    public string CorrectOutputData { get; set; }

    public static TestInfo Create(string inputData, string correctOutputData)
    {
        return new TestInfo
        {
            InputData = inputData,
            CorrectOutputData = correctOutputData
        };
    }
}