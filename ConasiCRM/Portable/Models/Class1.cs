//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace ConasiCRM.Portable.Models
//{
//    class Class1
//    {              
//            /*
//            hướng:                   TB: Tây Bắc       B: Bắc      DB: Đông Bắc

//                                 T: Tây                                   D: Đông
//                                      TN: Tây Nam      N: Nam      DN: Đông Nam

//            mệnh:
//                + caan: Cấn             + ly: Ly
//                + can: Càn              + kham: Khảm
//                + doai: Đoài            + ton: Tốn
//                + khon: Khôn            + chan: Chấn

//            khí: sắp sếp độ mạnh từ trên xuống
//                        Tốt                                     xấu
//                    + sinhkhi: Sinh khí                 + tuyetmenh: Tuyệt Mệnh
//                    + thieny: Thiên Y                   + nguquy: Ngũ Quỷ
//                    + diennien: Diên Niên               + lucsat: Lục Sát
//                    + phucvi: Phục Vị                   + hoahai: Họa Hại

//            Tứ trạch:
//                + Đông tứ trạch: 4 hướng tốt: bắc, nam, đông, đông nam + 4 mệnh tốt: khảm, ly, chấn, tốn
//                + Tây tứ trạch: 4 hướng tốt: tây, tây bắc, tây nam, đông bắc + 4 mệnh tốt: càn, khôn, cấn, đài
//                => 2 tứ trạch trên ngịch nhau

//            Can-Chi
//                        (lấy chữ số cuối cùng của năm sinh)                 (lấy 2 chữ số cuối của năm sinh chia lấy dư cho 12)
//                        Số quy ước      Can                                 Số quy ước           Chi
//                        0               Canh                                0                   Tý
//                        1               Tân                                 1                   Sửu
//                        2               Nhâm                                2                   Dần
//                        3               Quý                                 3                   Mẹo
//                        4               Giáp                                4                   Thìn
//                        5               Ất                                  5                   Tỵ
//                        6               Bính                                6                   Ngọ
//                        7               Đinh                                7                   Mùi
//                        8               Mậu                                 8                   Thân
//                        9               Kỷ                                  9                   Dậu
//                                                                            10                  Tuất
//                                                                            11                  Hợi

//            Thiên Can:                                              Địa chi:
//                        Số quy ước      Can                                 Số quy ước      Chi
//                        1               Giáp, Ất                            0               Tý, Sửu, Ngọ, Mùi
//                        2               Bính, Đinh                          1               Dần, Mão, Thân, Dậu
//                        3               Mậu, Kỷ                             2               Thìn, Tỵ, Tuất, Hợi
//                        4               Canh, Tân
//                        5               Nhâm, Quý

//            Mệnh:   Thiên Can + Can Chi = Mệnh (nếu lớn hơn 5 thì -5)
//                        Số quy ước      Mệnh
//                        1               Kim
//                        2               Thủy
//                        3               Hỏa
//                        4               Thổ
//                        5               Mộc
//            */


//            var menh = "";
//        var menh_full = "<label>Quẻ mệnh: </label>";
//        var dong_tutrach = true;    //đông tứ trạch : true => nếu đông tứ trạch :false -> tây tứ trạch
//        var huong_tot = [];
//        var huong_xau = [];
//        var kq_huong_tot = "<label>Hướng tốt: </label><br>";
//        var kq_huong_xau = "<label>Hướng Xấu: </label><br>";
//        var ten_namsinh_amlich = "<label>Năm sinh âm lịch: </label>";
//        var menh_nguhanh_value = 0;
//        var menh_nguhanh = "<label>Ngũ hành: </label>";

//        var bsd_can = { menh: "bsd_can", sinhkhi: "T", thieny: "DB", diennien: "TN", phucvi: "TB", tuyetmenh: "N", nguquy: "D", lucsat: "B", hoahai: "DN" };
//        var bsd_doai = { menh: "bsd_doai", sinhkhi: "TB", thieny: "TN", diennien: "DB", phucvi: "T", tuyetmenh: "D", nguquy: "N", lucsat: "DN", hoahai: "B" };
//        var bsd_caan = { menh: "bsd_caan", sinhkhi: "TN", thieny: "TB", diennien: "T", phucvi: "DB", tuyetmenh: "DN", nguquy: "B", lucsat: "D", hoahai: "N" };
//        var bsd_khon = { menh: "bsd_khon", sinhkhi: "DB", thieny: "T", diennien: "TB", phucvi: "TN", tuyetmenh: "B", nguquy: "DN", lucsat: "N", hoahai: "D" };
//        var bsd_ly = { menh: "bsd_ly", sinhkhi: "D", thieny: "DN", diennien: "B", phucvi: "N", tuyetmenh: "TB", nguquy: "T", lucsat: "TN", hoahai: "DB" };
//        var bsd_kham = { menh: "bsd_kham", sinhkhi: "DN", thieny: "D", diennien: "N", phucvi: "B", tuyetmenh: "TN", nguquy: "DB", lucsat: "TB", hoahai: "T" };
//        var bsd_ton = { menh: "bsd_ton", sinhkhi: "B", thieny: "N", diennien: "D", phucvi: "DN", tuyetmenh: "DB", nguquy: "TN", lucsat: "T", hoahai: "TB" };
//        var bsd_chan = { menh: "bsd_chan", sinhkhi: "N", thieny: "B", diennien: "DN", phucvi: "D", tuyetmenh: "T", nguquy: "TB", lucsat: "DB", hoahai: "TN" };
//        var list_khi = [bsd_can, bsd_doai, bsd_caan, bsd_khon, bsd_ly, bsd_kham, bsd_ton, bsd_chan];

//        var lotrunghoa_hoa_napam = { name: "Lô Trung Hỏa - Lửa trong lò", can_id: 2, chi_id: 1 };
//        var sondauhoa_hoa_napam = { name: "Sơn Đầu Hỏa - Lửa đỉnh núi", can_id: 1, chi_id: 2 };
//        var tichlichhoa_hoa_napam = { name: "Tích Lịch Hỏa - Lửa sấm sét", can_id: 3, chi_id: 0 };
//        var sonhoahoa_hoa_napam = { name: "Sơn Họa Hỏa - Lửa chân núi", can_id: 2, chi_id: 1 };
//        var phucdanghoa_hoa_napam = { name: "Phúc Đăng Hỏa - Lửa đèn lớn", can_id: 1, chi_id: 2 };
//        var thienthuonghoa_hoa_napam = { name: "Thiên Thượng Hỏa - Lửa trên trời", can_id: 3, chi_id: 0 };
//        var hoa_napam = [lotrunghoa_hoa_napam, sondauhoa_hoa_napam, tichlichhoa_hoa_napam, sonhoahoa_hoa_napam, phucdanghoa_hoa_napam, thienthuonghoa_hoa_napam];
//        var menh_hoa = { nguhanh_id: 3, napam: hoa_napam };

//        var gianhathuy_thuy_napam = { name: "Giản Hạ Thủy - Nước dưới suối", can_id: 2, chi_id: 0 };
//        var truongluu_thuy_napam = { name: "Trường Lưu Thủy - Nước suối lớn", can_id: 5, chi_id: 2 };
//        var tinhtuyenthuy_thuy_napam = { name: "Tinh Tuyền Thủy - Nước trong giếng", can_id: 1, chi_id: 1 };
//        var thienhathuy_thuy_napam = { name: "Thiên Hà Thủy - Nước sông trời", can_id: 2, chi_id: 0 };
//        var daikhuethuy_thuy_napam = { name: "Đại Khuê Thủy - Nước khe lớn", can_id: 1, chi_id: 1 };
//        var daihaithuy_thuy_napam = { name: "Đại Hải Thủy - Nước biển lớn", can_id: 5, chi_id: 2 };
//        var thuy_napam = [gianhathuy_thuy_napam, truongluu_thuy_napam, tinhtuyenthuy_thuy_napam, thienhathuy_thuy_napam, daikhuethuy_thuy_napam, daihaithuy_thuy_napam];
//        var menh_thuy = { nguhanh_id: 2, napam: thuy_napam };

//        var haitrungkim_kim_napam = { name: "Hải Trung Kim - Vàng trong biển", can_id: 1, chi_id: 0 };
//        var kiemphongkim_kim_napam = { name: "Kiếm Phong Kim - Sắt đầu kiếm", can_id: 5, chi_id: 1 };
//        var bachlapkim_kim_napam = { name: "Bạch Lạp Kim - Vàng trong nến", can_id: 4, chi_id: 2 };
//        var satrungkim_kim_napam = { name: "Sa Trung Kim - Vàng trong cát", can_id: 1, chi_id: 0 };
//        var kimbachkim_kim_napam = { name: "Kim Bạch Kim - Vàng pha bạc", can_id: 5, chi_id: 1 };
//        var thoaxuyenkim_kim_napam = { name: "Thoa Xuyến Kim - Vàng trang sức", can_id: 4, chi_id: 2 };
//        var kim_napam = [haitrungkim_kim_napam, kiemphongkim_kim_napam, bachlapkim_kim_napam, satrungkim_kim_napam, kimbachkim_kim_napam, thoaxuyenkim_kim_napam];
//        var menh_kim = { nguhanh_id: 1, napam: kim_napam };

//        var dailammoc_moc_napam = { name: "Đại Lâm Mộc - Gỗ trong rừng", can_id: 3, chi_id: 2 };
//        var duonglieumoc_moc_napam = { name: "Dương Liễu Mộc - Gỗ dương liễu", can_id: 5, chi_id: 0 };
//        var tungbachmoc_moc_napam = { name: "Tùng Bách Mộc - Gỗ tùng bách", can_id: 4, chi_id: 1 };
//        var binhdiamoc_moc_napam = { name: "Bình Địa Mộc - Cây đất bằng", can_id: 3, chi_id: 2 };
//        var tangdomoc_moc_napam = { name: "Tang Đố Mộc - Gỗ cây dâu", can_id: 5, chi_id: 0 };
//        var thachluumoc_moc_napam = { name: "Thạch Lựu Mộc - Gỗ thạc lựu", can_id: 4, chi_id: 1 };
//        var moc_napam = [dailammoc_moc_napam, duonglieumoc_moc_napam, tungbachmoc_moc_napam, binhdiamoc_moc_napam, tangdomoc_moc_napam, thachluumoc_moc_napam];
//        var menh_moc = { nguhanh_id: 5, napam: moc_napam };

//        var lobantho_tho_napam = { name: "Lộ Bàng Thổ - Đất ven đường", can_id: 4, chi_id: 0 };
//        var thanhdautho_tho_napam = { name: "Thành Đầu Thổ - Đất đầu thành", can_id: 3, chi_id: 1 };
//        var octhuongtho_tho_napam = { name: "Ốc Thượng Thổ - Đất mái nhà", can_id: 2, chi_id: 2 };
//        var bichthuongtho_tho_napam = { name: "Bích Thượng Thổ - Đất trên vách", can_id: 4, chi_id: 0 };
//        var daidichtho_tho_napam = { name: "Đại Dịch Thổ - Đất rộng lớn", can_id: 3, chi_id: 1 };
//        var satrungtho_tho_napam = { name: "Sa Trung Thổ - Đất trong cát", can_id: 2, chi_id: 2 };
//        var tho_napam = [lobantho_tho_napam, thanhdautho_tho_napam, octhuongtho_tho_napam, bichthuongtho_tho_napam, daidichtho_tho_napam, satrungtho_tho_napam];
//        var menh_tho = { nguhanh_id: 4, napam: tho_napam };

//        var list_nguhanhnapam = [menh_kim, menh_thuy, menh_hoa, menh_tho, menh_moc];
//        //get data on form
//        var objInfor = new Object();
//        console.log("Get data form");
//            if (window.parent.Xrm.Page.getAttribute("bsd_direction_tmpvalue") != null) {
//                objInfor.bsd_direction_tmpvalue = window.parent.Xrm.Page.getAttribute("bsd_direction_tmpvalue").getValue();
//    } else {
//                objInfor.bsd_direction_tmpvalue = null;
//            }
//console.log("bsd_direction_tmpvalue: " + objInfor.bsd_direction_tmpvalue);

//if (window.parent.Xrm.Page.getAttribute("bsd_loingysinh") != null)
//{
//    objInfor.bsd_loingysinh = window.parent.Xrm.Page.getAttribute("bsd_loingysinh").getValue();
//}
//else
//{
//    objInfor.bsd_loingysinh = false;
//}
//console.log("bsd_loingysinh: " + objInfor.bsd_loingysinh);

//if (window.parent.Xrm.Page.getAttribute("bsd_nmsinh") != null)
//{
//    objInfor.bsd_nmsinh = window.parent.Xrm.Page.getAttribute("bsd_nmsinh").getValue();
//}
//else
//{
//    objInfor.bsd_nmsinh = null
//            }
//console.log("bsd_nmsinh: " + objInfor.bsd_nmsinh);

//if (window.parent.Xrm.Page.getAttribute("birthdate") != null)
//{
//    objInfor.birthdate = window.parent.Xrm.Page.getAttribute("birthdate").getValue();
//}
//else
//{
//    objInfor.birthdate = window.parent.Xrm.Page.getAttribute("new_birthday").getValue();
//}
//console.log("birthdate: " + objInfor.birthdate);
//debugger;
//if (window.parent.Xrm.Page.getAttribute("gendercode") != null)
//{
//    objInfor.gendercode = window.parent.Xrm.Page.getAttribute("gendercode").getValue();
//}
//else
//{
//    objInfor.gendercode = window.parent.Xrm.Page.getAttribute("new_gender").getValue();
//}
//console.log("gendercode: " + objInfor.gendercode);
////
//function huong_batquai()
//{
//    //var context = GetGlobalContext();
//    //var year = window.parent.Xrm.Page.getAttribute("bsd_yearofbirth").getValue();
//    //East: 861,450,000     South:861,450,002       South East:861,450,005      North West:861,450,006
//    //West: 861,450,001     North:861,450,003       North East:861,450,004      South West:861,450,007
//    var direction = objInfor.bsd_direction_tmpvalue;

//    var type = objInfor.bsd_loingysinh;

//    var birthdate = "";
//    if (!type)
//    { // năm sinh

//        var yearS = objInfor.bsd_nmsinh;
//        birthdate = new Date(yearS, 1, 1);
//    }
//    else
//    {

//        birthdate = objInfor.birthdate;
//    }



//    var year = (new Date(birthdate)).getFullYear();
//    var sex = objInfor.gendercode;
//    var menh_value = year % 9;

//    //alert("test!");

//    if (sex == 1)
//    {//male
//        if (menh_value == 0 || menh_value == 6)
//        {
//            menh = "bsd_khon";
//            menh_full += "Khôn (Tây Tứ mệnh)";
//            dong_tutrach = false;
//        }
//        else if (menh_value == 1)
//        {
//            menh = "bsd_kham";
//            menh_full += "Khảm (Đông Tứ mệnh)";
//            dong_tutrach = true;
//        }
//        else if (menh_value == 2)
//        {
//            menh = "bsd_ly";
//            menh_full += "Ly (Đông Tứ mệnh)";
//            dong_tutrach = true;
//        }
//        else if (menh_value == 3)
//        {
//            menh = "bsd_caan";
//            menh_full += "Cần (Tây Tứ mệnh)";
//            dong_tutrach = false;
//        }
//        else if (menh_value == 4)
//        {
//            menh = "bsd_doai";
//            menh_full += "Đoài (Tây Tứ mệnh)";
//            dong_tutrach = false;
//        }
//        else if (menh_value == 5)
//        {
//            menh = "bsd_can";
//            menh_full += "Càn (Tây Tứ mệnh)";
//            dong_tutrach = false;
//        }
//        else if (menh_value == 7)
//        {
//            menh = "bsd_ton";
//            menh_full += "Tốn (Đông Tứ mệnh)";
//            dong_tutrach = true;
//        }
//        else if (menh_value == 9)
//        {
//            menh = "bsd_chan";
//            menh_full += "Chấn (Đông Tứ mệnh)";
//            dong_tutrach = true;
//        }
//    }
//    else if (sex == 2)
//    {//female
//        if (menh_value == 0)
//        {
//            menh = "bsd_ton";
//            menh_full += "Tốn (Đông Tứ mệnh)";
//            dong_tutrach = true;
//        }
//        else if (menh_value == 1 || menh_value == 4)
//        {
//            menh = "bsd_caan";
//            menh_full += "Cấn (Tây Tứ mệnh)";
//            dong_tutrach = false;
//        }
//        else if (menh_value == 2)
//        {
//            menh = "bsd_can";
//            dong_tutrach = false;
//            menh_full += "Càn (Tây Tứ mệnh)";
//        }
//        else if (menh_value == 3)
//        {
//            menh = "bsd_doai";
//            menh_full += "Đoài (Tây Tứ mệnh)";
//            dong_tutrach = false;
//        }
//        else if (menh_value == 5)
//        {
//            menh = "bsd_ly";
//            menh_full += "Ly (Đông Tứ mệnh)";
//            dong_tutrach = true;
//        }
//        else if (menh_value == 6)
//        {
//            menh = "bsd_kham";
//            menh_full += "Khảm (Đông Tứ mệnh)";
//            dong_tutrach = true;
//        }
//        else if (menh_value == 7)
//        {
//            menh = "bsd_khon";
//            menh_full += "Khôn (Tây Tứ mệnh)";
//            dong_tutrach = false;
//        }
//        else if (menh_value == 9)
//        {
//            menh = "bsd_chan";
//            menh_full += "Chấn (Đông Tứ mệnh)";
//            dong_tutrach = true;
//        }
//    }
//    if (direction != null)
//    {
//        //debugger;
//        var direction_arr = direction.split(",");
//        for (var i = 0; i < direction_arr.length; i++)
//        {
//            if (direction_arr[i] == 861450000)
//            {//dong
//                if (dong_tutrach)
//                {
//                    huong_tot.push("D");
//                }
//                else
//                {
//                    huong_xau.push("D");
//                }
//            }
//            else if (direction_arr[i] == 861450001)
//            {//tay
//                if (dong_tutrach)
//                {
//                    huong_xau.push("T");
//                }
//                else
//                {
//                    huong_tot.push("T");
//                }
//            }
//            else if (direction_arr[i] == 861450002)
//            {//nam
//                if (dong_tutrach)
//                {
//                    huong_tot.push("N");
//                }
//                else
//                {
//                    huong_xau.push("N");
//                }
//            }
//            else if (direction_arr[i] == 861450003)
//            {//bac
//                if (dong_tutrach)
//                {
//                    huong_tot.push("B");
//                }
//                else
//                {
//                    huong_xau.push("B");
//                }
//            }
//            else if (direction_arr[i] == 861450004)
//            {//dong bac
//                if (dong_tutrach)
//                {
//                    huong_xau.push("DB");
//                }
//                else
//                {
//                    huong_tot.push("DB");
//                }
//            }
//            else if (direction_arr[i] == 861450005)
//            {//dong nam
//                if (dong_tutrach)
//                {
//                    huong_tot.push("DN");
//                }
//                else
//                {
//                    huong_xau.push("DN");
//                }
//            }
//            else if (direction_arr[i] == 861450006)
//            {//tay bac
//                if (dong_tutrach)
//                {
//                    huong_xau.push("TB");
//                }
//                else
//                {
//                    huong_tot.push("TB");
//                }
//            }
//            else if (direction_arr[i] == 861450007)
//            {//tay nam
//                if (dong_tutrach)
//                {
//                    huong_xau.push("TN");
//                }
//                else
//                {
//                    huong_tot.push("TN");
//                }
//            }
//        }
//    }
//    else
//    {
//        if (dong_tutrach)
//        {
//            huong_tot = ["B", "N", "D", "DN"];
//            huong_xau = ["T", "TB", "TN", "DB"];
//        }
//        else
//        {
//            huong_tot = ["T", "TB", "TN", "DB"];
//            huong_xau = ["B", "N", "D", "DN"];
//        }
//    }
//    for (var i = 0; i < list_khi.length; i++)
//    {
//        if (list_khi[i].menh == menh)
//        {
//            for (var j = 0; j < huong_tot.length; j++)
//            {
//                var name_huong = "";
//                if (huong_tot[j] == "D")
//                    name_huong = "Đông";
//                else if (huong_tot[j] == "T")
//                    name_huong = "Tây";
//                else if (huong_tot[j] == "N")
//                    name_huong = "Nam";
//                else if (huong_tot[j] == "B")
//                    name_huong = "Bắc";
//                else if (huong_tot[j] == "DB")
//                    name_huong = "Đông Bắc";
//                else if (huong_tot[j] == "DN")
//                    name_huong = "Đông Nam";
//                else if (huong_tot[j] == "TB")
//                    name_huong = "Tây Bắc";
//                else if (huong_tot[j] == "TN")
//                    name_huong = "Tây Nam";

//                if (huong_tot[j] == list_khi[i].sinhkhi)
//                {
//                    kq_huong_tot += "<div>  " + name_huong + " - Sinh Khí: Phúc lộc vẹn toàn, đường con cái thuận lợi, nhà quay về hướng sinh khí của chủ nhà là ngôi nhà ấm áp, là tổ ấm của mọi thành viên trong gia đình.</div>";
//                }
//                else if (huong_tot[j] == list_khi[i].thieny)
//                {
//                    kq_huong_tot += "<div>  " + name_huong + " - Thiên Y: Được thiên thời che chở, giải bệnh dễ dàng, nhanh chóng tai qua nạn khỏi. </div>";
//                }
//                else if (huong_tot[j] == list_khi[i].diennien)
//                {
//                    kq_huong_tot += "<div>  " + name_huong + " - Diên Niên: Mọi sự ổn định, gặp nhiều may mắn,dễ gặp quý nhân phù trợ, có phúc có đức có hậu vận tốt, con cái thành đạt.</div>";
//                }
//                else if (huong_tot[j] == list_khi[i].phucvi)
//                {
//                    kq_huong_tot += "<div>  " + name_huong + " - Phục Vị: Nhàn hạ, ít lao động chân tay, công danh sự nghiệp ổn định</div>";
//                }
//            }

//            for (var j = 0; j < huong_xau.length; j++)
//            {
//                var name_huong = "";
//                if (huong_xau[j] == "D")
//                    name_huong = "Đông";
//                else if (huong_xau[j] == "T")
//                    name_huong = "Tây";
//                else if (huong_xau[j] == "N")
//                    name_huong = "Nam";
//                else if (huong_xau[j] == "B")
//                    name_huong = "Bắc";
//                else if (huong_xau[j] == "DB")
//                    name_huong = "Đông Bắc";
//                else if (huong_xau[j] == "DN")
//                    name_huong = "Đông Nam";
//                else if (huong_xau[j] == "TB")
//                    name_huong = "Tây Bắc";
//                else if (huong_xau[j] == "TN")
//                    name_huong = "Tây Nam";

//                if (huong_xau[j] == list_khi[i].tuyetmenh)
//                {
//                    kq_huong_xau += "<div>  " + name_huong + " - Tuyệt Mệnh: Gia đình dễ chia ly về tình cảm, đổ vỡ, người trong nhà phải tự bươn chải, ít có sự giúp đõ từ mọi phía.</div>";
//                }
//                else if (huong_xau[j] == list_khi[i].nguquy)
//                {
//                    kq_huong_xau += "<div>  " + name_huong + " - Ngũ Quỷ: Dễ gặp tai họa, kẻ xấu quấy phá, bị cản trở đường sự nghiệp, nhà có hướng về hướng này dễ bực bội khó chịu, mệt mõi về sức khỏe, bất hòa trong gia đình, không may mắn trong sự nghiệp.</div>";
//                }
//                else if (huong_xau[j] == list_khi[i].lucsat)
//                {
//                    kq_huong_xau += "<div>  " + name_huong + " - Lục Sát: Nhà có sát khí, có sự thiệt hại về người và của, sinh nở có thể xảy thai hoặc trẻ sơ sinh khó nuôi, nhà có thể có người chết trẻ.</div>";
//                }
//                else if (huong_xau[j] == list_khi[i].hoahai)
//                {
//                    kq_huong_xau += "<div>  " + name_huong + " - Họa Hại:Hay gặp tai nạn bất ngờ, trong nhà thường có người có bệnh tật bẩm sinh, mãn tính, hoặc nan y. </div>";
//                }
//            }
//            break;
//        }
//    }
//}

//function NguHanhNapAm()
//{//Can Chi
// //var year = window.parent.Xrm.Page.getAttribute("bsd_yearofbirth").getValue();

//    var type = objInfor.bsd_loingysinh

//                var birthdate = "";
//    if (type == true)
//    { // năm sinh

//        var yearS = objInfor.bsd_nmsinh;
//        birthdate = new Date(yearS, 1, 1);
//    }
//    else
//    {

//        birthdate = objInfor.birthdate;
//    }



//    var year = (new Date(birthdate)).getFullYear();
//    var can_value = year % 10;
//    //var chi_value = (year % 100) % 12;
//    //Hồ Fix 25-05-2019
//    var chi_value = (year - 4) % 12;
//    var can_id = 0;
//    var chi_id = 0;
//    if (can_value == 0)
//    {
//        ten_namsinh_amlich += "Canh ";
//        can_id = 4;
//        menh_nguhanh_value += 4;
//    }
//    else if (can_value == 1)
//    {
//        ten_namsinh_amlich += "Tân ";
//        can_id = 4;
//        menh_nguhanh_value += 4;
//    }
//    else if (can_value == 2)
//    {
//        ten_namsinh_amlich += "Nhâm ";
//        can_id = 5;
//        menh_nguhanh_value += 5;
//    }
//    else if (can_value == 3)
//    {
//        ten_namsinh_amlich += "Quý ";
//        can_id = 5;
//        menh_nguhanh_value += 5;
//    }
//    else if (can_value == 4)
//    {
//        ten_namsinh_amlich += "Giáp ";
//        can_id = 1;
//        menh_nguhanh_value += 1;
//    }
//    else if (can_value == 5)
//    {
//        ten_namsinh_amlich += "Ất ";
//        can_id = 1;
//        menh_nguhanh_value += 1;
//    }
//    else if (can_value == 6)
//    {
//        ten_namsinh_amlich += "Bính ";
//        can_id = 2;
//        menh_nguhanh_value += 2;
//    }
//    else if (can_value == 7)
//    {
//        ten_namsinh_amlich += "Đinh ";
//        can_id = 2;
//        menh_nguhanh_value += 2;
//    }
//    else if (can_value == 8)
//    {
//        ten_namsinh_amlich += "Mậu ";
//        can_id = 3;
//        menh_nguhanh_value += 3;
//    }
//    else if (can_value == 9)
//    {
//        ten_namsinh_amlich += "Kỷ ";
//        can_id = 3;
//        menh_nguhanh_value += 3;
//    }

//    if (chi_value == 0)
//    {
//        ten_namsinh_amlich += "Tý";
//    }
//    else if (chi_value == 1)
//    {
//        ten_namsinh_amlich += "Sửu";
//    }
//    else if (chi_value == 2)
//    {
//        ten_namsinh_amlich += "Dần";
//        chi_id = 1;
//        menh_nguhanh_value += 1;
//    }
//    else if (chi_value == 3)
//    {
//        ten_namsinh_amlich += "Mão";
//        chi_id = 1;
//        menh_nguhanh_value += 1;
//    }
//    else if (chi_value == 4)
//    {
//        ten_namsinh_amlich += "Thìn";
//        chi_id = 2;
//        menh_nguhanh_value += 2;
//    }
//    else if (chi_value == 5)
//    {
//        ten_namsinh_amlich += "Tỵ";
//        chi_id = 2;
//        menh_nguhanh_value += 2;
//    }
//    else if (chi_value == 6)
//    {
//        ten_namsinh_amlich += "Ngọ";
//    }
//    else if (chi_value == 7)
//    {
//        ten_namsinh_amlich += "Mùi";
//    }
//    else if (chi_value == 8)
//    {
//        ten_namsinh_amlich += "Thân";
//        chi_id = 1;
//        menh_nguhanh_value += 1;
//    }
//    else if (chi_value == 9)
//    {
//        ten_namsinh_amlich += "Dậu";
//        chi_id = 1;
//        menh_nguhanh_value += 1;
//    }
//    else if (chi_value == 10)
//    {
//        ten_namsinh_amlich += "Tuất";
//        chi_id = 2;
//        menh_nguhanh_value += 2;
//    }
//    else if (chi_value == 11)
//    {
//        ten_namsinh_amlich += "Hợi";
//        chi_id = 2;
//        menh_nguhanh_value += 2;
//    }

//    if (menh_nguhanh_value > 5) menh_nguhanh_value -= 5;

//    if (menh_nguhanh_value == 1)
//    {
//        for (var i = 0; i < 6; i++)
//        {
//            if (kim_napam[i].can_id == can_id && kim_napam[i].chi_id == chi_id)
//            {
//                menh_nguhanh += "Kim, " + kim_napam[i].name;
//                break;
//            }
//        }
//    }
//    else if (menh_nguhanh_value == 2)
//    {
//        for (var i = 0; i < 6; i++)
//        {
//            if (thuy_napam[i].can_id == can_id && thuy_napam[i].chi_id == chi_id)
//            {
//                menh_nguhanh += "Thủy, " + thuy_napam[i].name;
//                break;
//            }
//        }
//    }
//    else if (menh_nguhanh_value == 3)
//    {
//        for (var i = 0; i < 6; i++)
//        {
//            if (hoa_napam[i].can_id == can_id && hoa_napam[i].chi_id == chi_id)
//            {
//                menh_nguhanh += "Hỏa, " + hoa_napam[i].name;
//                break;
//            }
//        }
//    }
//    else if (menh_nguhanh_value == 4)
//    {
//        for (var i = 0; i < 6; i++)
//        {
//            if (tho_napam[i].can_id == can_id && tho_napam[i].chi_id == chi_id)
//            {
//                menh_nguhanh += "Thổ, " + tho_napam[i].name;
//                break;
//            }
//        }
//    }
//    else if (menh_nguhanh_value == 5)
//    {
//        for (var i = 0; i < 6; i++)
//        {
//            if (moc_napam[i].can_id == can_id && moc_napam[i].chi_id == chi_id)
//            {
//                menh_nguhanh += "Mộc, " + moc_napam[i].name;
//                break;
//            }
//        }
//    }


//}
           
                           
//            $(document).ready(function() {
//    huong_batquai();
//    NguHanhNapAm();

//    setTimeout(function() {
//        var type = objInfor.bsd_loingysinh;

//        var birthdate = "";
//        if (type == true)
//        { // năm sinh

//            var yearS = objInfor.bsd_nmsinh;
//            birthdate = new Date(yearS, 1, 1);
//        }
//        else
//        {

//            birthdate = objInfor.birthdate;
//        }

//        if (birthdate != "" && objInfor.gendercode != "")
//        {
//                        $('.namsinhamlich').html(ten_namsinh_amlich);
//                        $('.quemenh').html(menh_full);
//                        $('.nguhanh').html(menh_nguhanh);
//                        $('#batquai').append("<img src='../WebResources/" + menh + "' width='335' height='335' /> ");
//                        $('.huongtot').html(kq_huong_tot);
//                        $('.huongxau').html(kq_huong_xau);
//        }
//        console.log(birthdate + " space " + objInfor.gendercode);
//    }, 500);
//});      
//    }
//}
