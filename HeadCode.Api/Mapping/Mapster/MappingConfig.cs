namespace HeadCode.Api.Mapping.Mapster;

using Core.Models;
using Endpoints.Problems.Get;
using Endpoints.ProblemTests.Get;
using global::Mapster;

public class MappingConfig
{
    public MappingConfig()
    {
        TypeAdapterConfig<GetProblemResponse, Problem>.NewConfig()
                                                      .Map(dest => dest.Id, src => src.Id)
                                                      .Map(dest => dest.Title, src => src.Title)
                                                      .Map(dest => dest.Description, src => src.Description)
                                                      .Map(dest => dest.DateCreated, src => src.DateCreated)
                                                      .Map(dest => dest.DatesUpdated, src => src.DatesUpdated);
        TypeAdapterConfig<GetTestResponse, ProblemTest>.NewConfig()
                                                       .Map(dest => dest.Id, src => src.Id)
                                                       .Map(dest => dest.ProblemId, src => src.ProblemId)
                                                       .Map(dest => dest.InputData, src => src.InputData)
                                                       .Map(dest => dest.CorrectOutputData,
                                                            src => src.CorrectOutputData)
                                                       .Map(dest => dest.DateCreated, src => src.DateCreated)
                                                       .Map(dest => dest.DatesUpdated, src => src.DatesUpdated);
    }
}