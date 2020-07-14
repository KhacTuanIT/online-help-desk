namespace OnlineHelpDesk.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RequestStatus
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("StatusType")]
        public int? StatusTypeId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Message { get; set; }

        public DateTime? TimeCreated { get; set; }

        public virtual Request Request { get; set; }

        public virtual StatusType StatusType { get; set; }
    }
}
