using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.DAL
{
    public abstract class Entity 
    {
        [Key]
        public int Id { get; set; }
    }
}
