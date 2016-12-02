
var oTable;
var oTable_Order;
//var oTable_Order;
var valid_edit_name = 1;
var valid_edit_email = 1;
var valid_edit_phone = 1;

var valid_name = 0;
var valid_email = 1;
var valid_phone = 1;

var email_pattern = /^[\w._-]+[+]?[\w._-]+@[\w.-]+\.[a-zA-Z]{2,6}$/;
function validate(mode, value) {

    //var hidden_warning = document.getElementById("mail_errorTxt");
   

    //1 = email mode
    if (mode == 1) {
        //alert(atpos + " " + (dotpos ));
        //alert(value.length);
        //alert(value.length - dotpos);
        if (value.length != 0) {
            if (!value.match(email_pattern) || value.indexOf("..") != -1) {
                document.getElementById("mail_errorTxt").style.visibility = "visible";
                //    //alert("false");
                valid_edit_email = 0;
            }
            else {

                document.getElementById("mail_errorTxt").style.visibility = "hidden";
                //alert("true");
                valid_edit_email = 1;
            }
        }
        else {

            document.getElementById("mail_errorTxt").style.visibility = "hidden";
            //alert("true");
            valid_edit_email = 1;
        }
        //2 = phone mode
    }
    else if (mode == 2) {
        var phoneno = /^[0-9]+$/;
        if (value.match(phoneno) && value.length < 12 && value.length > 9 || value.length == 0) {
            document.getElementById("phone_errorTxt").style.visibility = "hidden";
            valid_edit_phone = 1;
        }
        else {
            document.getElementById("phone_errorTxt").style.visibility = "visible";
            valid_edit_phone = 0;
        }
    }
        // 3 = name mode
    else if (mode == 3) {
        if (value) {
            valid_edit_name = 1;
            document.getElementById("name_errorTxt").style.visibility = "hidden";
        }
        else {
            valid_edit_name = 0;
            document.getElementById("name_errorTxt").style.visibility = "visible";
        }
    }

    if (valid_edit_phone == 1 && valid_edit_email == 1 && valid_edit_name == 1) document.getElementById("btnSave_edit").disabled = false;
    else document.getElementById("btnSave_edit").disabled = true;
}



function Addingvalidate(mode, value) {

    //var hidden_warning = document.getElementById("mail_errorTxt");
    value = value.trim();

    //1 = email mode
    if (mode == 1) {
        //var pattern = /^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$/;
        //var pattern = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;

        var atpos = value.indexOf("@");
        var dotpos = value.lastIndexOf(".");

        if (value.length != 0) {
            if (!value.match(email_pattern) || value.indexOf("..") != -1) {
                document.getElementById("mail_errorTxt1").style.visibility = "visible";
                //    //alert("false");
                valid_email = 0;
            }
            else {

                document.getElementById("mail_errorTxt1").style.visibility = "hidden";
                //alert("true");
                valid_email = 1;
            }
        }
        else {

            document.getElementById("mail_errorTxt1").style.visibility = "hidden";
            //alert("true");
            valid_email = 1;
        }
        //2 = phone mode
    }
    else if (mode == 2) {
        var phoneno = /^[0-9]+$/;
        if (value.match(phoneno) && value.length < 12 && value.length > 9 || value.length == 0) {

            document.getElementById("phone_errorTxt1").style.visibility = "hidden";
            valid_phone = 1;
        }
        else {

            document.getElementById("phone_errorTxt1").style.visibility = "visible";
            valid_phone = 0;
        }
    }
    else if (mode == 3) {
        if (value) {
            document.getElementById("name_errorTxt1").style.visibility = "hidden";
            valid_name = 1;
        }
        else {
            document.getElementById("name_errorTxt1").style.visibility = "visible";
            valid_name = 0;
    }
}

    //alert(valid_name + "_" + valid_phone + "_" + valid_email);
    if (valid_phone == 1 && valid_email == 1 && valid_name == 1) document.getElementById("btnSave_add").disabled = false;
    else document.getElementById("btnSave_add").disabled = true;
}


function clear_table() {

    document.getElementById("phone_errorTxt").style.visibility = "hidden";
    document.getElementById("mail_errorTxt").style.visibility = "hidden";
    document.getElementById("name_errorTxt").style.visibility = "hidden";
    document.getElementById("editName").value = "";
    document.getElementById("editDes").value = "";
    document.getElementById("editAddr").value = "";
    document.getElementById("editPhone").value = "";
    document.getElementById("editMail").value = "";
    document.getElementById("cusEditID").value == "";
    $('#btnSave_edit').prop('disabled', false);
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

    if (_name.trim() != "") {
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
                        if (isConfirm) { oTable.fnDraw(); clear_add();}
                    });
                }
                if (data == 2) {
                    swal({
                        title: "Khách hàng này đã có!",
                        type: "warning",
                        confirmButtonText: "Đóng",
                        closeOnConfirm: true
                    },
                       function (isConfirm) {
                           if (isConfirm) { clear_add(); }
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
                            if (isConfirm) { clear_add();
                        }
                        });
                }
            }


        });
    }
    else swal("Vui lòng nhập tên", "", "warning");
    
}

function editCustomerDetail() {
    var cusName = document.getElementById("editName").value.trim();
    var _inputCusID = document.getElementById("cusEditID").value;
    if (cusName!="") {
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
                //alert(data);
                //showSucModal(editSuc_Mes);
                if (data=="True")
                    {
                swal({
                    title: "Chỉnh sửa thành công!",
                    type: "success",
                    confirmButtonText: "Đóng",
                    closeOnConfirm: true
                },
            function (isConfirm) {
                if (isConfirm) { oTable.fnDraw(); $('#cusEditModal').modal('hide'); }
            });
                }
                else {
                swal({
                        title: "Chỉnh sửa thất bại!",
                            type: "error",
                            confirmButtonText: "Đóng",
                            closeOnConfirm: true
                        },
                function (isConfirm) {
                        $('#cusEditModal').modal('hide');
                        });
            }

            },
            error: function () {
                swal({
                    title: "Chỉnh sửa thất bại!",
                    type: "error",
                    confirmButtonText: "Đóng",
                    closeOnConfirm: true
                },
                function (isConfirm) {
                    $('#cusEditModal').modal('hide');
                });
            }
        });
    }
    else swal("Vui lòng nhập tên", "", "warning");
}

function clear_add() {
    document.getElementById("phone_errorTxt1").style.visibility = "hidden";
    document.getElementById("mail_errorTxt1").style.visibility = "hidden";
    document.getElementById("name_errorTxt1").style.visibility = "hidden";
    document.getElementById("addNameTxt").value = "";
    document.getElementById("addAddrTxt").value = "";
    document.getElementById("addDescTxt").value = "";
    document.getElementById("addPhoneTxt").value = "";
    document.getElementById("addMailTxt").value = "";
    $('#btnSave_add').prop('disabled', true);
}

function getEditCustomer(e, cusEditID) {
    var currentRow = $(e).closest('tr');
    var tds = currentRow.find("td");
    document.getElementById("editName").value = tds.eq(0).text();
    document.getElementById("editDes").value = tds.eq(1).text();
    document.getElementById("editAddr").value = tds.eq(2).text();
    document.getElementById("editPhone").value = tds.eq(3).text();
    document.getElementById("editMail").value = tds.eq(4).text();
    getCustomerOrder(cusEditID);
    $('#clickableH').find('th')[1].click();
    $('#clickableH').find('th')[1].click();
    showCusEditModal(cusEditID);
}

function getCustomerOrder(cusId) {
    var id = null;
    var languageConfig = {
        "oPaginate": {
            "sNext": "Sau"
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
    //var currentRow = $(e).closest('tr');
    //var tds = currentRow.find("td");
    //$('#orderOfCus').text(tds.eq(0).text());
    //$('#cusOrderList').modal('show');

    //(cusId);
    //oTable_Order.dataTable().fnDestroy();
    //console.log(('#cusOrderTable').length);
    oTable_Order = $('#cusOrderTable').dataTable({
        "bProcessing": true,
        "bServerSide": true,
        "destroy": true,
        "sServerMethod": "POST",
        "oLanguage": languageConfig,
        "scrollCollapse": true,
        "scrollY": "200px",
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
        "fnInitComplete": function () {
            //var table_header = $('.dataTables_scrollHeadInner')
            //table_header.css("width", "100%");
            //var new_table=table_header.find(".table");
            //new_table.css("width", "100%");
            $('#clickableH').find('th')[1].click();
            $('#clickableH').find('th')[1].click();
        }


    });
    $('#cusOrderTable_filter input').attr("placeholder", "Nhập mã đơn hàng");
    
}

function refreshPage() {
    location.reload();
}



/////// modal section

function showCusEditModal(cuseditid) {
    valid_edit_name = 1;
    valid_edit_email = 1;
    valid_edit_phone = 1;
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
    valid_name = 0;
    valid_email = 1;
    valid_phone = 1;
    $('#cusAddModal').modal('show');
}

function showCreateFailModal() {
    $('#createFailModal').modal('show');
}

function showcusDeleteModal(id) {
    $('#cusDeleteModal').modal('show');
    document.getElementById("cusDeleteID").value = id;

}