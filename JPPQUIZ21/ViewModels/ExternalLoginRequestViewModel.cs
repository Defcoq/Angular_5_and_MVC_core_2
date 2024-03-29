﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JPPQUIZ21.ViewModels
{
    [JsonObject(MemberSerialization.OptOut)]
    public class ExternalLoginRequestViewModel
    {
        #region Constructor
        public ExternalLoginRequestViewModel()
        {

        }
        #endregion

        #region Properties
        public string access_token { get; set; }
        public string client_id { get; set; }
        #endregion
    }
}
