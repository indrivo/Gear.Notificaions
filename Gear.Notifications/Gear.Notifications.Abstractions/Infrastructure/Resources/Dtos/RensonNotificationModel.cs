using System;
using System.Collections.Generic;

namespace Gear.Notifications.Abstractions.Infrastructure.Resources.Dtos
{
    public class RensonNotificationModel
    {
        public List<Personalization> personalizations { get; set; }
        public RensonMailAddress from { get; set; }
        public RensonMailAddress replyTo { get; set; }
        public string subject { get; set; }
        public string templateId { get; set; }
        public List<RensonAttachment> attachments { get; set; }
        public DateTime sendAt { get; set; }
    }

    public class Personalization
    {
        public List<RensonMailAddress> to { get; set; }
        public List<RensonMailAddress> cc { get; set; }
        public List<RensonMailAddress> bcc { get; set; }
        public string subject { get; set; }
        public Object dynamicData { get; set; }
        public DateTime sendAt { get; set; }
    }


    public class RensonAttachment
    {
        public string content { get; set; }
        public string type { get; set; }
        public string fileName { get; set; }
        public string disposition { get; set; }
        public string contentId { get; set; }
    }

    public class RensonMailAddress
    {
        public string email { get; set; }
        public string name { get; set; }
    }

    public class Token
    {
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public int ext_expires_in { get; set; }
        public string access_token { get; set; }
    }
}
