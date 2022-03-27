using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Entity
{
    public class Bookmark
    {
        [Key]
        public int ID { get; set; }

        [StringLength(maximumLength: 500)]
        public string URL { get; set; }

        public string ShortDescription { get; set; }

        public int? CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public DateTime CreateDate { get; set; }

        public String UserEmail{ get; set; }

        public int counter { get; set; }
        
    }
}
