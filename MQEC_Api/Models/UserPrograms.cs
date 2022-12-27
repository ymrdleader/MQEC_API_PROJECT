﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MQEC_Api.Models
{
    [Table("User_Programs")]
    public partial class UserPrograms
    {
        [Key]
        [Column("User_Id")]
        [StringLength(30)]
        public string UserId { get; set; }
        [Key]
        [Column("ProgramID")]
        [StringLength(100)]
        public string ProgramId { get; set; }
        [Column("N_Run")]
        public bool? NRun { get; set; }
        [Column("N_Add")]
        public bool? NAdd { get; set; }
        [Column("N_Edit")]
        public bool? NEdit { get; set; }
        [Column("N_Del")]
        public bool? NDel { get; set; }
        [Column("N_Rept")]
        public bool? NRept { get; set; }
        [Column("N_Load")]
        public bool? NLoad { get; set; }
        [Column("N_Load2")]
        public bool? NLoad2 { get; set; }
        [Column("N_Change")]
        public bool? NChange { get; set; }
        [Column("N_Change2")]
        public bool? NChange2 { get; set; }
        [Column("N_Change3")]
        public bool? NChange3 { get; set; }
        [Column("N_Excel")]
        public bool? NExcel { get; set; }
        [Column("N_Word")]
        public bool? NWord { get; set; }
        [Column("N_Approve")]
        public bool? NApprove { get; set; }
        [Column("N_Mail")]
        public bool? NMail { get; set; }
        [Column("N_Clone")]
        public bool? NClone { get; set; }
    }
}