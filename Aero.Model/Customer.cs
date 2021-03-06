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
    
    public partial class Customer
    {
        public Customer()
        {
            this.RFQs = new HashSet<RFQ>();
            this.POes = new HashSet<PO>();
        }
    
        public int Id { get; set; }
        public Nullable<int> ContactId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
    
        public virtual Contact Contact { get; set; }
        public virtual ICollection<RFQ> RFQs { get; set; }
        public virtual ICollection<PO> POes { get; set; }
    }
}
