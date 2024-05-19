using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;


namespace APIAlma.Models
{
    public class Tool

    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required] 
        public string? Description { get; set; }

        [Required]
        public int RentalPrice { get; set; }

        [ForeignKey("UserId")]
        [ValidateNever]
        [JsonIgnore]
        public string? UserId { get; set; }

        [JsonIgnore]
        public CustomUser? User { get ; set; }

    }
}

