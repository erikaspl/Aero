//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Aero.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Priority
    {
        public Priority()
        {
            this.RFQs = new HashSet<RFQ>();
        }
    
        public int Id { get; set; }
        public string Code { get; set; }
        public string Display { get; set; }
    
        public virtual ICollection<RFQ> RFQs { get; set; }
    }
}