﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MQEC_Api.Models
{
    [Table("comVatType")]
    public partial class ComVatType
    {
        [Key]
        [Column("VatType_No")]
        [StringLength(20)]
        public string VatTypeNo { get; set; }
        [Required]
        [Column("VatType_Name")]
        [StringLength(30)]
        public string VatTypeName { get; set; }
    }
}