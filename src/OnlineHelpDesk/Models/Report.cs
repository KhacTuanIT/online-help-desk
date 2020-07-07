namespace OnlineHelpDesk.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Report")]
    public partial class Report
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Url)]
        public string Resource { get; set; }

        [ForeignKey("Creator")]
        public string CreatorId { get; set; }

        public DateTime? CreatedAt { get; set; }

        public virtual ApplicationUser Creator { get; set; }
    }
}
