using System.ComponentModel.DataAnnotations;

namespace WaterCalculator.Features.Groups.Create
{
    public sealed record UpsertGroupCommand(string GroupName, string GroupDetails, Guid? GroupId);
}

