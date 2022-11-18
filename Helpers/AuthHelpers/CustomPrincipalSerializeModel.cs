using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuidanceConsultancy.Helpers.AuthHelpers
{
    public class CustomPrincipalSerializeModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string AvatarURL { get; set; }
        public string[] Roles { get; set; }
    }
}