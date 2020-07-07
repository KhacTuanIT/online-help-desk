namespace OnlineHelpDesk.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FacilityHead")]
    public partial class FacilityHead
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [ForeignKey("Facility")]
        public int? FacilityId { get; set; }

        [StringLength(255)]
        public string PositionName { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual Facility Facility { get; set; }
    }
}
