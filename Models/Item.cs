using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CosmoAPI.Models
{
    public class Item
    {
        
        [Newtonsoft.Json.JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [Required]
        [Newtonsoft.Json.JsonProperty(PropertyName = "nome")]
        public string Nome { get; set; }

        [Newtonsoft.Json.JsonProperty(PropertyName = "descricao")]
        public string Descricao { get; set; }

    }

}
