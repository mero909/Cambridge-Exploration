using System;
using System.Collections.Generic;
using Cambridge.Data.Models;

namespace Cambridge.Web.Models
{
    public class RegisterViewModel
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public String Email { get; set; }
        public String Passcode { get; set; }
        public String Phone { get; set; }
        public String Address { get; set; }
        public String Address2 { get; set; }
        public String City { get; set; }
        public Int32 StateId { get; set; }
        public String PostalCode { get; set; }
        public String Message { get; set; }
        
        public Boolean IsLocked { get; set; }
        public Boolean IsDeleted { get; set; }


        public Int32 InvestorType1 { get; set; }
        public Int32 InvestorType2 { get; set; }
        public Int32 InvestorType3 { get; set; }
        public Int32 InvestorType4 { get; set; }


        private List<State> _states;
        private List<InvestorType> _investorTypes;

        public List<State> States
        {
            get { return _states ?? new List<State>(); }
            set { _states = value; }
        }

        public State SelectedState { get; set; }

        public List<InvestorType> InvestorTypes
        {
            get { return _investorTypes ?? new List<InvestorType>(); }
            set { _investorTypes = value; }
        }

        public Int32 Solution { get; set; }
        public Int32 Term1 { get; set; }
        public Int32 Term2 { get; set; }
    }
}