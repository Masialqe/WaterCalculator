using WaterCalculator.Domain.Invoices;

namespace WaterCalculator.Domain
{
    public class Group : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public ICollection<Apartment> Apartments { get; set; } = [];
        public ICollection<Payoff> Payoffs { get; set; } = [];

        public Group() { }
        public Group(string groupName, string groupDetails)
        {
            Name = groupName;
            Details = groupDetails;
        }
        public static Group Create(string groupName, string groupDetails)
            => new(groupName, groupDetails);
    }
}
