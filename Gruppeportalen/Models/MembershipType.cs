using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Gruppeportalen.Models;

public class MembershipType
{
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
    
        [Required]
        public string Name { get; set; } = string.Empty;
    
        [Required]
        public float Price { get; set; }
    
        [Required]
        public DateTime MembershipStart { get; set; } 
    
        [Required]
        public DateTime MembershipEnd { get; set; } 

        [ForeignKey(nameof(LocalGroup))]
        public Guid LocalGroupId { get; set; } 
        
        public LocalGroup? LocalGroup { get; set; }
    

}