using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserScreen.Models
{
    public class ModelAdvertiseImages : ModelMyAdvertise
    {
        public int advertiseImageId { get; set; }
        public int advertiseId { get; set; }

        public string imageData { get; set; }

        public DateTime createdOn { get; set; }
        public DateTime updatedOn { get; set; }
    }
}