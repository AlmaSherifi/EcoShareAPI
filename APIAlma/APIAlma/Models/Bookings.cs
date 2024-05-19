

using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace APIAlma.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [ForeignKey("ToolId")]
        public int ToolId { get; set; }

        public Tool? Tool { get; set; }

        [JsonIgnore]
        public CustomUser? Renter { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset EndDate { get; set; }


        [ForeignKey("UserId")]
        [ValidateNever]
        [JsonIgnore]   
        public string? UserId { get; set; }
    }
}
