using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EliteBookings.Models
{
    /// <summary>
    /// conditions table
    /// </summary>
    public partial class Condition
    {
        [Key]
        [Column("condition_id")]
        public long ConditionId { get; set; }
        [Column("section_id")]
        public long? SectionId { get; set; }
        [Column("min_bottle_count")]
        public long? MinBottleCount { get; set; }
        [Column("min_people")]
        public long? MinPeople { get; set; }
        [Column("max_people")]
        public long? MaxPeople { get; set; }
        [Column("other_conditions")]
        public string? OtherConditions { get; set; }

        [ForeignKey(nameof(SectionId))]
        [InverseProperty("Conditions")]
        public virtual Section? Section { get; set; }
    }
}
