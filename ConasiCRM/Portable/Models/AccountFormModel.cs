using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Models
{
    public class AccountFormModel : BaseViewModel
    {
        private Guid _accountid;
        public Guid accountid { get { return _accountid; } set { _accountid = value; OnPropertyChanged(nameof(accountid)); } }

        private string _bsd_name;
        public string bsd_name { get { return _bsd_name; } set { _bsd_name = value; OnPropertyChanged(nameof(bsd_name)); } }

        private string _bsd_accountnameother;
        public string bsd_accountnameother { get { return _bsd_accountnameother; } set { _bsd_accountnameother = value; OnPropertyChanged(nameof(bsd_accountnameother)); } }

        private string _bsd_companycode;
        public string bsd_companycode { get { return _bsd_companycode; } set { _bsd_companycode = value; OnPropertyChanged(nameof(bsd_companycode)); } }

        private string __primarycontactid_value;
        public string _primarycontactid_value { get { return __primarycontactid_value; } set { __primarycontactid_value = value; OnPropertyChanged(nameof(_primarycontactid_value)); } }

        private string _primarycontactname;
        public string primarycontactname { get { return _primarycontactname; } set { _primarycontactname = value; OnPropertyChanged(nameof(primarycontactname)); } }

        private string _bsd_businesstype;
        public string bsd_businesstype { get { return _bsd_businesstype; } set { _bsd_businesstype = value; OnPropertyChanged(nameof(bsd_businesstype)); } }

        private string _bsd_businesstypevalue;
        public string bsd_businesstypevalue { get { return _bsd_businesstypevalue; } 
        set {
            _bsd_businesstypevalue = value;
            if(value != null)
                {
                    var tmp = value.Split(',');
                    foreach(var item in tmp)
                    {
                        if (item == "100000000")
                        {
                            bsd_businesstype_customer = true;
                        }
                        else if (item == "100000001")
                        {
                            bsd_businesstype_partner = true;
                        }
                        else if (item == "100000002")
                        {
                            bsd_businesstype_saleagents = true;
                        }
                        else if (item == "100000003")
                        {
                            bsd_businesstype_deverloper = true;
                        }
                    }
                }
                OnPropertyChanged(nameof(bsd_businesstypevalue)); 
         } }

        private bool _bsd_businesstype_customer;
        public bool bsd_businesstype_customer { get { return _bsd_businesstype_customer; } set { _bsd_businesstype_customer = value; OnPropertyChanged(nameof(bsd_businesstype_customer)); } }

        private bool _bsd_businesstype_partner;
        public bool bsd_businesstype_partner { get { return _bsd_businesstype_partner; } set { _bsd_businesstype_partner = value; OnPropertyChanged(nameof(bsd_businesstype_partner)); } }

        private bool _bsd_businesstype_saleagents;
        public bool bsd_businesstype_saleagents { get { return _bsd_businesstype_saleagents; } set { _bsd_businesstype_saleagents = value; OnPropertyChanged(nameof(bsd_businesstype_saleagents)); } }

        private bool _bsd_businesstype_deverloper;
        public bool bsd_businesstype_deverloper { get { return _bsd_businesstype_deverloper; } set { _bsd_businesstype_deverloper = value; OnPropertyChanged(nameof(bsd_businesstype_deverloper)); } }

        private string _bsd_localization;
        public string bsd_localization { get { return _bsd_localization; } set { _bsd_localization = value; OnPropertyChanged(nameof(bsd_localization)); } }

        private string _bsd_customergroup;
        public string bsd_customergroup { get { return _bsd_customergroup; } set { _bsd_customergroup = value; OnPropertyChanged(nameof(bsd_customergroup)); } }

        private decimal? _bsd_diemdanhgia;
        public decimal? bsd_diemdanhgia { get => _bsd_diemdanhgia; set { _bsd_diemdanhgia = value; OnPropertyChanged(nameof(bsd_diemdanhgia)); } }
        public string bsd_diemdanhgia_format { get { return bsd_diemdanhgia.HasValue ? string.Format("{0:#,0.#}", bsd_diemdanhgia.Value) : " "; } }

        private string _telephone1;
        public string telephone1 { get { return _telephone1; } set { _telephone1 = value; OnPropertyChanged(nameof(telephone1)); } }

        private string _emailaddress1;
        public string emailaddress1 { get { return _emailaddress1; } set { _emailaddress1 = value; OnPropertyChanged(nameof(emailaddress1)); } }

        private string _bsd_email2;
        public string bsd_email2 { get { return _bsd_email2; } set { _bsd_email2 = value; OnPropertyChanged(nameof(bsd_email2)); } }

        private string _fax;
        public string fax { get { return _fax; } set { _fax = value; OnPropertyChanged(nameof(fax)); } }

        private string _websiteurl;
        public string websiteurl { get { return _websiteurl; } set { _websiteurl = value; OnPropertyChanged(nameof(websiteurl)); } }

        private string _bsd_registrationcode;
        public string bsd_registrationcode { get { return _bsd_registrationcode; } set { _bsd_registrationcode = value; OnPropertyChanged(nameof(bsd_registrationcode)); } }

        private DateTime? _bsd_issuedon;
        public DateTime? bsd_issuedon { get { return _bsd_issuedon; } set { _bsd_issuedon = value; OnPropertyChanged(nameof(bsd_issuedon)); } }

        private string _bsd_placeofissue;
        public string bsd_placeofissue { get { return _bsd_placeofissue; } set { _bsd_placeofissue = value; OnPropertyChanged(nameof(bsd_placeofissue)); } }

        private bool _bsd_khachhangdagiaodich;
        public bool bsd_khachhangdagiaodich { get { return _bsd_khachhangdagiaodich; } set { _bsd_khachhangdagiaodich = value; OnPropertyChanged(nameof(bsd_khachhangdagiaodich)); } }

        private string _bsd_vatregistrationnumber;
        public string bsd_vatregistrationnumber { get { return _bsd_vatregistrationnumber; } set { _bsd_vatregistrationnumber = value; OnPropertyChanged(nameof(bsd_vatregistrationnumber)); } }

        private string _bsd_address;
        public string bsd_address { get { return _bsd_address; } set { _bsd_address = value; OnPropertyChanged(nameof(bsd_address)); } }

        private string _bsd_diachi;
        public string bsd_diachi { get { return _bsd_diachi; } set { _bsd_diachi = value; OnPropertyChanged(nameof(bsd_diachi)); } }

        private string _bsd_permanentaddress1;
        public string bsd_permanentaddress1 { get { return _bsd_permanentaddress1; } set { _bsd_permanentaddress1 = value; OnPropertyChanged(nameof(bsd_permanentaddress1)); } }

        private string _bsd_diachithuongtru;
        public string bsd_diachithuongtru { get { return _bsd_diachithuongtru; } set { _bsd_diachithuongtru = value; OnPropertyChanged(nameof(bsd_diachithuongtru)); } }

        private string _bsd_housenumberstreet;
        public string bsd_housenumberstreet { get { return _bsd_housenumberstreet; } set { _bsd_housenumberstreet = value; OnPropertyChanged(nameof(bsd_housenumberstreet)); } }

        private string _bsd_street;
        public string bsd_street { get { return _bsd_street; } set { _bsd_street = value; OnPropertyChanged(nameof(bsd_street)); } }

        private string _bsd_permanenthousenumberstreetwardvn;
        public string bsd_permanenthousenumberstreetwardvn { get { return _bsd_permanenthousenumberstreetwardvn; } set { _bsd_permanenthousenumberstreetwardvn = value; OnPropertyChanged(nameof(bsd_permanenthousenumberstreetwardvn)); } }

        private string _bsd_permanenthousenumberstreetward;
        public string bsd_permanenthousenumberstreetward { get { return _bsd_permanenthousenumberstreetward; } set { _bsd_permanenthousenumberstreetward = value; OnPropertyChanged(nameof(bsd_permanenthousenumberstreetward)); } }

        //private string _bsd_nation;
        //public string bsd_nation { get { return _bsd_nation; } set { _bsd_nation = value; OnPropertyChanged(nameof(bsd_nation)); } }

        //private string _bsd_province;
        //public string bsd_province { get { return _bsd_province; } set { _bsd_province = value; OnPropertyChanged(nameof(bsd_province)); } }

        //private string _bsd_district;
        //public string bsd_district { get { return _bsd_district; } set { _bsd_district = value; OnPropertyChanged(nameof(bsd_district)); } }

        private string _nation_name;
        public string nation_name { get { return _nation_name; } set { _nation_name = value; OnPropertyChanged(nameof(nation_name)); } }

        private string _nation_nameen;
        public string nation_nameen { get { return _nation_nameen; } set { _nation_nameen = value; OnPropertyChanged(nameof(nation_nameen)); } }

        private string __bsd_nation_value;
        public string _bsd_nation_value { get { return __bsd_nation_value; } set { __bsd_nation_value = value; OnPropertyChanged(nameof(_bsd_nation_value)); } }

        private string _province_name;
        public string province_name { get { return _province_name; } set { _province_name = value; OnPropertyChanged(nameof(province_name)); } }

        private string _province_nameen;
        public string province_nameen { get { return _province_nameen; } set { _province_nameen = value; OnPropertyChanged(nameof(province_nameen)); } }

        private string __bsd_province_value;
        public string _bsd_province_value { get { return __bsd_province_value; } set { __bsd_province_value = value; OnPropertyChanged(nameof(_bsd_province_value)); } }

        private string _district_name;
        public string district_name { get { return _district_name; } set { _district_name = value; OnPropertyChanged(nameof(district_name)); } }

        private string _district_nameen;
        public string district_nameen { get { return _district_nameen; } set { _district_nameen = value; OnPropertyChanged(nameof(district_nameen)); } }

        private string __bsd_district_value;
        public string _bsd_district_value { get { return __bsd_district_value; } set { __bsd_district_value = value; OnPropertyChanged(nameof(_bsd_district_value)); } }


        //private string _bsd_permanentnation;
        //public string bsd_permanentnation { get { return _bsd_permanentnation; } set { _bsd_permanentnation = value; OnPropertyChanged(nameof(bsd_permanentnation)); } }

        //private string _bsd_permanentprovince;
        //public string bsd_permanentprovince { get { return _bsd_permanentprovince; } set { _bsd_permanentprovince = value; OnPropertyChanged(nameof(bsd_permanentprovince)); } }

        //private string _bsd_permanentdistrict;
        //public string bsd_permanentdistrict { get { return _bsd_permanentdistrict; } set { _bsd_permanentdistrict = value; OnPropertyChanged(nameof(bsd_permanentdistrict)); } }

        private string _permanentnation_name;
        public string permanentnation_name { get { return _permanentnation_name; } set { _permanentnation_name = value; OnPropertyChanged(nameof(permanentnation_name)); } }

        private string _permanentnation_nameen;
        public string permanentnation_nameen { get { return _permanentnation_nameen; } set { _permanentnation_nameen = value; OnPropertyChanged(nameof(permanentnation_nameen)); } }

        private string __bsd_permanentnation_value;
        public string _bsd_permanentnation_value { get { return __bsd_permanentnation_value; } set { __bsd_permanentnation_value = value; OnPropertyChanged(nameof(_bsd_permanentnation_value)); } }

        private string _permanentprovince_name;
        public string permanentprovince_name { get { return _permanentprovince_name; } set { _permanentprovince_name = value; OnPropertyChanged(nameof(permanentprovince_name)); } }

        private string _permanentprovince_nameen;
        public string permanentprovince_nameen { get { return _permanentprovince_nameen; } set { _permanentprovince_nameen = value; OnPropertyChanged(nameof(permanentprovince_nameen)); } }

        private string __bsd_permanentprovince_value;
        public string _bsd_permanentprovince_value { get { return __bsd_permanentprovince_value; } set { __bsd_permanentprovince_value = value; OnPropertyChanged(nameof(_bsd_permanentprovince_value)); } }

        private string _permanentdistrict_name;
        public string permanentdistrict_name { get { return _permanentdistrict_name; } set { _permanentdistrict_name = value; OnPropertyChanged(nameof(permanentdistrict_name)); } }

        private string _permanentdistrict_nameen;
        public string permanentdistrict_nameen { get { return _permanentdistrict_nameen; } set { _permanentdistrict_nameen = value; OnPropertyChanged(nameof(permanentdistrict_nameen)); } }

        private string __bsd_permanentdistrict_value;
        public string _bsd_permanentdistrict_value { get { return __bsd_permanentdistrict_value; } set { __bsd_permanentdistrict_value = value; OnPropertyChanged(nameof(_bsd_permanentdistrict_value)); } }

        private bool _bsd_mandatorysecondary;
        public bool bsd_mandatorysecondary { get { return _bsd_mandatorysecondary; } set { _bsd_mandatorysecondary = value; OnPropertyChanged(nameof(bsd_mandatorysecondary)); } }

    }
}

