using System;
using System.Collections.Generic;

#nullable disable

namespace SchoolDBWebAPI.Services.DBModels
{
    public partial class NotificationDatum
    {
        public int Id { get; set; }
        public int? NotMasterId { get; set; }
        public int? ReceiverId { get; set; }
        public int? SenderId { get; set; }
        public int? ReadStatus { get; set; }

        public virtual NotificationsMaster NotMaster { get; set; }
    }
}
