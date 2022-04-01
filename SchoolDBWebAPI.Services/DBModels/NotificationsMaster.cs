using System;
using System.Collections.Generic;

#nullable disable

namespace SchoolDBWebAPI.Services.DBModels
{
    public partial class NotificationsMaster
    {
        public NotificationsMaster()
        {
            NotificationData = new HashSet<NotificationDatum>();
        }

        public int Id { get; set; }
        public string Message { get; set; }
        public int? SenderId { get; set; }
        public DateTime? MsgTime { get; set; }

        public virtual ICollection<NotificationDatum> NotificationData { get; set; }
    }
}
