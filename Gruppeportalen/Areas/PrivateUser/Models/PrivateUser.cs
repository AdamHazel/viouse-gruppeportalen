﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gruppeportalen.Models;

namespace Gruppeportalen.Areas.PrivateUser.Models;

public class PrivateUser
{
    [Key]
    [ForeignKey("ApplicationUser")]
    public string Id {get; set;}
    [Required]
    [StringLength(30)]
    public string Firstname { get; set; } = string.Empty;

    [Required] 
    [StringLength(30)] 
    public string Lastname { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Address { get; set; } = String.Empty;

    [Required]
    [StringLength(30)]
    public string City { get; set; } = String.Empty;

    [Required]
    [StringLength(4)]
    public string Postcode { get; set; } = String.Empty;

    [Required]
    [StringLength(30)]
    public string Telephone { get; set; } = String.Empty;

    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }


    public ApplicationUser? ApplicationUser{ get; set; }
    public ICollection<Person> Persons { get; set; } = new List<Person>();
}