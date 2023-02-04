using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MQEC_Api.Models.ViewModels
{
    public class AllowancSellerBuyerModel
    {
        public string Identifier { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string PersonInCharge { get; set; }

        public string TelephoneNumber { get; set; }

        public string FacsimileNumber { get; set; }

        public string EmailAddress { get; set; }

        public string CustomerNumber { get; set; }
    }
}
