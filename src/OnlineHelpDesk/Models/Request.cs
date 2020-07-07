namespace OnlineHelpDesk.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Request")]
    public partial class Request
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Request()
        {
            RequestStatus = new HashSet<RequestStatus>();
        }

        [Key]
        public int Id { get; set; }

        [ForeignKey("Petitioner")]
        public string PetitionerId { get; set; }

        [ForeignKey("AssignedHead")]
        public int? AssignedHeadId { get; set; }

        [ForeignKey("Equipment")]
        public int? EquipmentId { get; set; }

        [ForeignKey("RequestType")]
        public int? RequestTypeId { get; set; }

        [ForeignKey("RequestStatus")]
        public int RequestStatusId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Message { get; set; }

        public virtual Equipment Equipment { get; set; }

        public virtual ApplicationUser Petitioner { get; set; }

        //public virtual ApplicationUser AssignedHead { get; set; }
        public virtual FacilityHead AssignedHead { get; set; }

        public virtual RequestType RequestType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RequestStatus> RequestStatus { get; set; }
    }
}
