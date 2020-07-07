namespace OnlineHelpDesk.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Notification")]
    public partial class Notification
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Message { get; set; }

        public bool? Seen { get; set; }

        public DateTime? CreatedAt { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
