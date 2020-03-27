using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class CandidateModel
    {
        [Key]
        public int ID { get; set; }
        [Column(TypeName ="nvarchar(30)")]
        public string Name { get; set; }
        [Column(TypeName ="nvarchar(30)")]
        public string LastName { get; set; }
        [Column(TypeName = "nvarchar(13)")]
        public string MobileNo { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Address { get; set; }
    }
}
