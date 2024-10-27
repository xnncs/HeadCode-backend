namespace HeadCode.Api.Mapping.Mapster;

using Core.Models;
using Endpoints.Problems.GetAllProblems;
using global::Mapster;

public class MappingConfig
{
    public MappingConfig()
    {
        TypeAdapterConfig<GetProblemResponse, Problem>.NewConfig()
                                                      .Map(dest => dest.Id, src => src.Id)
                                                      .Map(dest => dest.Title, src => src.Title)
                                                      .Map(dest => dest.Description, src => src.Description)
                                                      .Map(dest => dest.DateCreated, src => src.DateCreated);
    }
}