using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XJSC.DTOs.CustomerDTOs
{
    public class SearchQueryCustomerDTO
    {

        [Display(Name = "Nombre")]
        public string? Name_Like { get; set; }

        [Display(Name = "Apellido")]
        public string? LastName_Like { get; set; }

        [Display(Name = "Pagina")]
        public int Skip { get; set; }

        [Display(Name = "CantReg X Pagina")]
        public int Take { get; set; }

        public byte SendRowCount { get; set; }
    }
}
