using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserScreen.Models
{
    public class ModelArea : ModelCity
    {
        public int areaId { get; set; }
        public int cityId { get; set; }
        public string areaName { get; set; }
    }
}