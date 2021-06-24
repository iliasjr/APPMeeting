using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.entities
{
    public class Utlisateurs
    {      [Key]
        public int Idutil { get; set; }
        public String Username { get; set; }
    }
}
