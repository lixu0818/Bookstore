//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IdentitySample.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string CoverImagePath { get; set; }
        public string Author { get; set; }
        public string Discription { get; set; }
        public string Condition { get; set; }
        public string PublishYear { get; set; }
        public Nullable<bool> status { get; set; }
        public string ISBN { get; set; }
    }
}
