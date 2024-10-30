namespace HeadCode.Core.KafkaContracts;

using Enums;
using HelpingModels;

public class RunTestsMessage
{
    public List<TestInfo> Tests { get; set; } = [];
    
    public Languages Language { get; set; }
    public string Code { get; set; }
    
    
    public static RunTestsMessage Create(Languages language, string code, List<TestInfo> tests)
    {
        return new RunTestsMessage
        {
            Tests = tests,
            Language = language,
            Code = code
        };
    }
}