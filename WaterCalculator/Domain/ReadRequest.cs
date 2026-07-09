using WaterCalculator.Domain.Reads;

namespace WaterCalculator.Domain
{
    //TODO: To allow apartments to add reads by their own - admin must open read request to allow data entering for given period.
    public class ReadRequest : BaseEntity
    {
        public Guid GroupId { get; set; }
        public Group Group { get; set; }
        public DateTime ReadStart { get; set; } = DateTime.Now;
        public ICollection<Read> Reads { get; set; } = [];

        //when entering new invoice -> Close ReadRequest from invoice period
        public ReadRequestStatus Status { get; set; } = ReadRequestStatus.Open;
    }

    public enum ReadRequestStatus
    {
        Open = 0,
        Closed = 1,
    }
}
