using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumahUmat.Models
{
    public class AuthLogin : BaseEntity
    {
        public string Account { get; set; }
        public string Psswd { get; set; }
        public string SignWith { get; set; }
        public string Device { get; set; }
        public string IdMachine { get; set; }
    }
    public class SignUpModel : BaseEntity
    {
        public string _a { get; set; } //Account
        public string Psswd { get; set; } //password
        public string _Suw { get; set; } // SignUpWith
        public string _D { get; set; } // Device
        public string _N { get; set; } //Name
        public string _uc { get; set; } //UniqueCd
        public string _p { get; set; } //Password
        public string _cp { get; set; } // Confirm Password
    }
    public class VerifyCode : BaseEntity
    {
        public string _a { get; set; } // Account
        public string _uc { get; set; } //Unik Code
        public string _p { get; set; } //Password
        public string _cp { get; set; } // Confirm Password
    }
    public class Donatur : BaseEntity
    {
        public int DonaturId { get; set; }
        public string Nama { get; set; }
        public string Email { get; set; }
        public string NoHp { get; set; }
        public string Passwd { get; set; }
        public string Alamat { get; set; }
        public DateTime BirthOfDate { get; set; }
        public string SignUpWith { get; set; }
        public string SosmedId { get; set; }
        public string UserInput { get; set; }
        public string UserEdit { get; set; }
        public DateTime TimeInput { get; set; }
        public DateTime TimeEdit { get; set; }
    }
    public class MailModel : BaseEntity
    {
        public string From
        {
            get;
            set;
        }
        public string To
        {
            get;
            set;
        }
        public string Subject
        {
            get;
            set;
        }
        public string Body
        {
            get;
            set;
        }
        public string token { get; set; }
        public Boolean error { get; set; }
        public string message { get; set; }
    }
}
