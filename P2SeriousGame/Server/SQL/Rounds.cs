//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace P2SeriousGame.Server.SQL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rounds
    {
        public int Id { get; set; }
        public int RoundID { get; set; }
        public Nullable<double> Clicks { get; set; }
        public Nullable<double> AVG_Clicks { get; set; }
        public Nullable<int> Win { get; set; }
        public Nullable<int> Loss { get; set; }
        public Nullable<double> Time_Used { get; set; }
    }
}
