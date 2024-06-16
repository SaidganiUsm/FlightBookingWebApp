using FlightBookingApp.Core.Common;

namespace FlightBookingApp.Core.Entities
{
    public class Rank : BaseAuditableEntity
    {
        public string? RankName { get; set; }

        public int RankPriceRatio { get; set; }
    }
}
