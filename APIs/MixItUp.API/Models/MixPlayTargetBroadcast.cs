﻿using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace MixItUp.API.Models
{
    [DataContract]
    public class MixPlayTargetBroadcast
    {
        [Required]
        [DataMember]
        public string[] Scopes { get; set; }

        [Required]
        [DataMember]
        public JObject Data { get; set; }
    }
}