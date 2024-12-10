using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Gruppeportalen.Models;

public class MembershipType
{
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
    
        [Required]
        [Display(Name = "Navn av medlemsskapet")]
        [MaxLength(50)]
        public string MembershipName { get; set; } = string.Empty;
    
        [Required]
        [Display(Name = "Pris")]
        public float Price { get; set; }
        
        [Required]
        [Display(Name = "Dato for fornyelse av medlemsskapet")]
        public int DayReset { get; set; }
        
        [Required]
        [Display(Name = "Måned for fornyelse av medlemsskapet")]
        public int MonthReset { get; set; }
        

        [ForeignKey(nameof(LocalGroup))]
        public Guid LocalGroupId { get; set; } 
        
        public LocalGroup? LocalGroup { get; set; }
        
}