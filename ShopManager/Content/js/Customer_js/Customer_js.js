
var createSuc_Mes = "Tạo mới khách hàng thành công!";
var createFail_Mes = "Tạo mới khách hàng thất bại";
var createFaildup_Mes = "Khách hàng này đã tồn tại";
var editFail_Mes = "Chỉnh sửa thông tin thất bại";
var editSuc_Mes = "Chỉnh sửa thông tin thành công";
var fail_Mes = "Lỗi hệ thống";
var oTable;
var oTable_Order;
//var oTable_Order;

function validate(mode, value) {

    //var hidden_warning = document.getElementById("mail_errorTxt");
    var valid_name = 0;
    var valid_email = 1;
    var valid_phone = 1;

    //1 = email mode
    if (mode == 1) {
        var atpos = value.indexOf("@");
        var dotpos = value.lastIndexOf(".");
        //alert(atpos + " " + (dotpos ));
        //alert(value.length);
        //alert(value.length - dotpos);
        if ((atpos < 1 || dotpos < atpos + 2 || dotpos + 4 < value.length || value.length - dotpos <= 2 || atpos == -1 || dotpos == -1) && value.length != 0) {

            document.getElementById("mail_errorTxt").style.display = "block";
            valid_email = 0;
            //alert("false");
        }
        else {

            document.getElementById("mail_errorTxt").style.display = "none";
            valid_email = 1;
            //alert("true");
        }
        //2 = phone mode
    }
    else if (mode == 2) {
        var phoneno = /^[0-9]+$/;
        if (value.match(phoneno) && value.length < 12 && value.length > 9 || value.length == 0) {
            document.getElementById("phone_errorTxt").style.display = "none";
            valid_phone = 1;
        }
        else {
            document.getElementById("phone_errorTxt").style.display = "block";
            valid_phone = 0;
        }
    }
        // 3 = name mode
    else if (mode == 3) {
        if (!value.trim()) {
            document.getElementById("name_errorTxt").style.display = "block";
            valid_name = 0;
        }
        else valid_name = 1;

    }
    if (valid_phone == 1 && valid_email == 1 && valid_name == 1) document.getElementById("btnSave").disabled = false;
    else document.getElementById("btnSave").disabled = true;
}



function Addingvalidate(mode, value) {

    //var hidden_warning = document.getElementById("mail_errorTxt");


    //1 = email mode
    if (mode == 1) {
        var atpos = value.indexOf("@");
        var dotpos = value.lastIndexOf(".");
        //alert(atpos + " " + (dotpos ));
        //alert(value.length);
        //alert(value.length - dotpos);
        if ((atpos < 1 || dotpos < atpos + 2 || dotpos + 4 < value.length || value.length - dotpos <= 2 || atpos == -1 || dotpos == -1) && value.length != 0) {

            document.getElementById("mail_errorTxt1").style.display = "block";
            //alert("false");
            document.getElementById("btnSave1").disabled = true;
        }
        else {

            document.getElementById("mail_errorTxt1").style.display = "none";
            document.getElementById("btnSave1").disabled = false;
            //alert("true");
        }
        //2 = phone mode
    }
    else if (mode == 2) {
        var phoneno = /^[0-9]+$/;
        if (value.match(phoneno) && value.length < 12 && value.length > 9 || value.length == 0) {

            document.getElementById("phone_errorTxt1").style.display = "none";
            document.getElementById("btnSave1").disabled = false;
        }
        else {

            document.getElementById("phone_errorTxt1").style.display = "block";
            document.getElementById("btnSave1").disabled = true;
        }
    }

}


function clear_table() {

    document.getElementById("phone_errorTxt").style.display = "none";
    document.getElementById("mail_errorTxt").style.display = "none";
    $('#btnSave').prop('disabled', false);
}


function clear_detail_table() {
    document.getElementById("detailName").value = "";
    document.getElementById("detailDes").value = "";
    document.getElementById("detailAddr").value = "";
    document.getElementById("detailPhone").value = "";
    document.getElementById("detailMail").value = "";
}




function addCustomer() {
    var _id = "";
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
            success: function (data) {
                if (data == 1) {
                    swal({
                        title: "Tạo mới thành công!",
                        type: "success",
                        confirmButtonText: "OK",
                        closeOnConfirm: true
                    },
                    function (isConfirm) {
                        if (isConfirm) { oTable.fnDraw() }
                    });
                }
                if (data == 2) {
                    swal({
                        title: "Khách hàng này đã có!",
                        type: "warning",
                        confirmButtonText: "OK",
                        closeOnConfirm: true
                    },
                       function (isConfirm) {
                           if (isConfirm) { oTable.fnDraw() }
                       });
                }
                if (data == 3) {
                    swal({
                        title: "Tạo mới thất bại!",
                        type: "error",
                        confirmButtonText: "Đóng",
                        closeOnConfirm: true
                    },
                        function (isConfirm) {
                            if (isConfirm) { oTable.fnDraw() }
                        });
                }
            }


        });
    }
    else swal("Vui lòng nhập tên", "", "warning");
}

function editCustomerDetail() {
    var _inputCusID = document.getElementById("cusEditID").value;
    if (_inputCusID != null && _inputCusID != "") {
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
            function (isConfirm) {
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

function clear_add() {

    document.getElementById("phone_errorTxt1").style.display = "none";
    document.getElementById("mail_errorTxt1").style.display = "none";
    document.getElementById("addNameTxt").value = "";
    document.getElementById("addAddrTxt").value = "";
    document.getElementById("addDescTxt").value = "";
    document.getElementById("addPhoneTxt").value = "";
    document.getElementById("addMailTxt").value = "";
    $('#btnSave1').prop('disabled', false);
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

function getCustomerOrder(e, cusId) {
    var id = null;
    var languageConfig = {
        "oPaginate": {
            "sNext": "Tiếp"
            , "sLast": "Cuối"
            , "sPrevious": "Trước"
            , "sFirst": "Đầu"
        }
                , "infoFiltered": "(được lọc từ _MAX_ đơn hàng)"
                , "sInfo": "Kết quả từ _START_ đến _END_ trong số _TOTAL_"
                , "sInfoThousands": "."
                , "sLoadingRecords": "Đang tải ..."
                , "sProcessing": "Đang xử lý ..."
                , "sSearch": "Tìm kiếm đơn hàng: "
                , "sZeroRecords": "Không tìm thấy kết quả"
                , "sLengthMenu": "Hiện _MENU_ đơn hàng"
                , "searchPlaceholder": "Search records"

    }
    var currentRow = $(e).closest('tr');
    var tds = currentRow.find("td");
    $('#orderOfCus').text(tds.eq(0).text());
    $('#cusOrderList').modal('show');

    //(cusId);
    //oTable_Order.dataTable().fnDestroy();
    oTable_Order = $('#cusOrderTable').dataTable({
        "bProcessing": true,
        "bServerSide": true,
        "destroy": true,
        "sServerMethod": "POST",
        "oLanguage": languageConfig,
        //'sDom': 'l<"toolbar"f>rtip',
        //'sDom': '<"right"Bflrtip>rt<"bottom"pi><"clear">',
        //'sDom':'ilftpr',
        //'sDom': 'B<"top"l>frt<"bottom"i>p<"clear">',
        "sjquery": true,
        "sAjaxSource": "/Customer/GetAllOrderByCustomerId",
        "fnServerParams": function (data) {
            //alert(cusId);
            data.push({ "name": "cusId", "value": cusId });
        },
        "aoColumns": [
        {
            "mData": function (source) {
                id = source.Id;
                return source.Id;
            }, "bSortable": true
        },
        {
            "mData": function (source) {
                return FormatDateVN(source.DateModified);
            }, "bSortable": true
        },
        { "mData": "Total", "bSortable": true },
        {
            "mData": function (source) {
                return "<a href='/Order?Id=" + id + "' target='_blank' class='btn btn-success search-dropdown'>chi tiết </a>";
            }, "bSortable": false
        }
        ],



    });
    $('.dataTables_filter input').attr("placeholder", "Nhập mã đơn hàng");
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