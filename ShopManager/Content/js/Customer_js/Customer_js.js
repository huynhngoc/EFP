var create_validate_flag = 1;
var createSuc_Mes = "Tạo mới khách hàng thành công!";
var createFail_Mes = "Tạo mới khách hàng thất bại";
var createFaildup_Mes = "Khách hàng này đã tồn tại";
var editFail_Mes = "Chỉnh sửa thông tin thất bại";
var editSuc_Mes = "Chỉnh sửa thông tin thành công";
var fail_Mes = "Lỗi hệ thống";
var oTable;

function validate(mode, value) {

    var hidden_warning = document.getElementById("mail_errorTxt");
    create_validate_flag = 1;

    //1 = email mode
    if (mode == 1) {
        var atpos = value.indexOf("@");
        var dotpos = value.lastIndexOf(".");
        //alert(atpos + " " + (dotpos ));
        //alert(value.length);
        //alert(value.length - dotpos);
        if (atpos < 1 || dotpos < atpos + 2 || dotpos + 4 < value.length || value.length- dotpos<=2 || atpos == -1 || dotpos == -1) {
            create_validate_flag = 0;
            document.getElementById("mail_errorTxt").style.display = "block";
            //alert("false");
            document.getElementById("btnSave").disabled=true;
        }
        else {
            create_validate_flag = 1;
            document.getElementById("mail_errorTxt").style.display = "none";
            document.getElementById("btnSave").disabled=false;
            //alert("true");
        }
        //2 = phone mode
    }
    else if (mode == 2) {
        var phoneno = /^[0-9]+$/;
        if (value.match(phoneno) && value.length < 12 && value.length > 9 || value.length == 0) {
            create_validate_flag = 1;
            document.getElementById("phone_errorTxt").style.display = "none";
            document.getElementById("btnSave").disabled = false;
        }
        else {
            create_validate_flag = 0;
            document.getElementById("phone_errorTxt").style.display = "block";
            document.getElementById("btnSave").disabled = true;
        }
    }

}


function clear_table() {
    create_validate_flag = 1;
    document.getElementById("phone_errorTxt").style.display = "none";
    document.getElementById("mail_errorTxt").style.display = "none";
}

//function setSucMes(sucmes) {
//    document.getElementById("sucTxt").innerHTML = sucmes;
//}
//function setfailMes(failmes) {
//    document.getElementById("failTxt").innerHTML = failmes;
//}

//$('.demo3').click(function () {
//    swal({
//        title: "Are you sure?",
//        text: "You will not be able to recover this imaginary file!",
//        type: "warning",
//        showCancelButton: true,
//        confirmButtonColor: "#DD6B55",
//        confirmButtonText: "Yes, delete it!",
//        closeOnConfirm: false
//    }, function () {
//        swal("Deleted!", "Your imaginary file has been deleted.", "success");
//    });
//});

$(document).ready(function () {
    $("#saveID").hide();
    $("#cancelID").hide();


    var languageConfig = {
        "oPaginate": {
            "sNext": "Tiếp"
            , "sLast": "Cuối"
            , "sPrevious": "Trước"
            , "sFirst": "Đầu"
        }
                , "infoFiltered": "(được lọc từ _MAX_ khách hàng)"
                , "sInfo": "Kết quả từ _START_ đến _END_ trong số _TOTAL_"
                , "sInfoThousands": "."
                , "sLoadingRecords": "Đang tải ..."
                , "sProcessing": "Đang xử lý ..."
                , "sSearch": "Tìm kiếm khách hàng: "
                , "sZeroRecords": "Không tìm thấy kết quả"
                , "sLengthMenu": "Hiện _MENU_ khách hàng"
                , "searchPlaceholder": "Search records"
                
    }




    oTable = $('#cusDataTable').dataTable({
        "bProcessing": true,
        "bServerSide": true,
        "sServerMethod": "POST",
        "oLanguage": languageConfig,
        //'sDom': 'l<"toolbar"f>rtip',
        //'sDom': '<"right"Bflrtip>rt<"bottom"pi><"clear">',
        //'sDom':'ilftpr',
        //'sDom': 'B<"top"l>frt<"bottom"i>p<"clear">',
        "sjquery": true,
        "sAjaxSource": "/Customer/GetAllCustomer",
        "aoColumns": [
        { "mData": "Name", "bSortable": true },
        { "mData": "Description", "bSortable": true },
        { "mData": "Address", "bSortable": true },
        { "mData": "Phone", "bSortable": true },
        { "mData": "Email", "bSortable": true },
        {
            "mData": function (source) {
                return "<a id='" + source.CustomerId + "' onclick='getEditCustomer(this," + source.CustomerId + ")' class='btn btn-success search-dropdown'><span class='fa fa-edit' aria-hidden='true'></span> </a>"
            }, "bSortable": false
        }
        ],


    });

    $('#sucModal').on('hidden.bs.modal', function (e) {
        if ($('.modal').hasClass('in')) {
            $('body').addClass('modal-open');
        }
    });
});




function addCustomer() {
    var _id = Math.round(Math.random() * 1000);
    var _name = document.getElementById("addNameTxt").value;
    var _addr = document.getElementById("addAddrTxt").value;
    var _desc = document.getElementById("addDescTxt").value;
    var _phone = document.getElementById("addPhoneTxt").value;
    var _mail = document.getElementById("addMailTxt").value;

    if (_name != "") {
        $.ajax({
            url: "/Customer/AddCustomer",
            cache: false,
            traditional: true,
            type: "POST",
            data: ({
                Id: _id,
                Name: _name,
                Addr: _addr,
                Desc: _desc,
                Phone: _phone,
                Email: _mail
            }),

            
        });
    }
    else showCreateFailModal();
}

function editCustomerDetail() {
    var _inputCusID = document.getElementById("cusEditID").value;
    if (_inputCusID != null && _inputCusID != "" && create_validate_flag == 1) {
        $.ajax({
            url: "/Customer/EditCustomer",
            cache: true,
            traditional: true,
            data: ({
                Id: document.getElementById("cusEditID").value.trim(),
                Name: document.getElementById("editName").value.trim(),
                Desc: document.getElementById("editDes").value.trim(),
                Addr: document.getElementById("editAddr").value.trim(),
                Phone: document.getElementById("editPhone").value.trim(),
                Email: document.getElementById("editMail").value.trim()
            }),

            success: function (data) {
                //showSucModal(editSuc_Mes);
                    swal({
                        title: "Chỉnh sửa thành công!",
                        type: "success",
                        confirmButtonText: "Đóng",
                        closeOnConfirm: true
                    },
                function (isConfirm){ 
                    if (isConfirm) { oTable.fnDraw() }
                });
                
            },
            error: function () {
                swal({
                    title: "Chỉnh sửa thất bại!",
                    type: "error",
                    confirmButtonText: "Đóng",
                    closeOnConfirm: true
                },
                function (isConfirm) {
                    
                });
            }
        });
    }
}

function getEditCustomer(e, cusEditID) {
    var currentRow = $(e).closest('tr');
    var tds = currentRow.find("td");
    document.getElementById("editName").value = tds.eq(0).text();
    document.getElementById("editDes").value = tds.eq(1).text();
    document.getElementById("editAddr").value = tds.eq(2).text();
    document.getElementById("editPhone").value = tds.eq(3).text();
    document.getElementById("editMail").value = tds.eq(4).text();
    showCusEditModal(cusEditID);
}

function refreshPage() {
    location.reload();
}



/////// modal section

function showCusEditModal(cuseditid) {
    $('#cusEditModal').modal('show');
    document.getElementById("cusEditID").value = cuseditid;
}
function showSucModal(suc_mes) {
    setSucMes(suc_mes);
    $('#sucModal').modal('show');
}

function showFailModal(fail_mes) {
    setFailMes(fail_mes);
    $('#failModal').modal('show');
}

function showCusAddModal() {
    $('#cusAddModal').modal('show');
}

function showCreateFailModal() {
    $('#createFailModal').modal('show');
}

function showcusDeleteModal(id) {
    $('#cusDeleteModal').modal('show');
    document.getElementById("cusDeleteID").value = id;

}