using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineHelpDesk.Models
{
    public class MediateEquipmentViewModel
    {
        public int FacilityId { get; set; }
        public int EquipmentTypeId { get; set; }
    }

    public class HomeViewModel 
    {
        public List<Notification> Notifications { get; set; }
        public List<RequestViewModel> RequestViewModels { get; set; } 
        public int Requests { get; set; }
        public int Users { get; set; }
        public int Facilities { get; set; }
        public int Equipments { get; set; }
    }

    public class RequestViewModel
    {
        public int Id { get; set; }
        public string Petitioner { get; set; }
        public string Equipment { get; set; }
        public string Facility { get; set; }
        public string RequestType { get; set; }
        public string RequestMessage { get; set; }
        public DateTime? CreatedTime { get; set; }
    }
}