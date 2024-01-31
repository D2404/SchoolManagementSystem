﻿var type = 1
function ShowFilter() {
    if (type === 1) {
        $('#FilterDiv').show();
        type = 2;
    }
    else {
        $('#FilterDiv').hide();
        type = 1;
    }
}
$(document).ready(function () {
    $('#FilterDiv').hide();
    GetLeaveList(1);
});
function myFunction() {
    document.getElementById("myDropdown").classList.toggle("show");
}


function InsertData() {

    var val = true;
    var Id = $('#hdnintId').val();
    var FromDate = $('#FromDate').val();
    if (FromDate === "" || /\S/.test(FromDate) === false) {
        $("#errFromDate").html("Please enter Fromdate.");
        val = false;
    }
    var ToDate = $('#ToDate').val();
    if (ToDate === "" || /\S/.test(ToDate) === false) {
        $("#errToDate").html("Please enter Todate.");
        val = false;
    }

    var NoOfDays = $('#NoOfDays').val();
    if (NoOfDays === "" || /\S/.test(NoOfDays) === false) {
        $("#errDays").html("Please enter No of days.");
        val = false;
    }
    var LeaveType = $('#LeaveType').val();
    if (LeaveType === 0 || /\S/.test(LeaveType) === false) {
        $("#errLeaveType").html("Please enter leave type.");
        val = false;
    }
    var Reason = $('#Reason').val();
    if (Reason === "" || /\S/.test(Reason) === false) {
        $("#errReason").html("Please enter reason.");
        val = false;
    }

    var formData = new FormData();
    if (val === false) {
        return;
    }

    formData.append('Id', Id);
    formData.append('FromDate', FromDate);
    formData.append('ToDate', ToDate);
    formData.append('NoOfDays', NoOfDays);
    formData.append('LeaveType', LeaveType);
    formData.append('Reason', Reason);
    ShowWait();
    $.ajax({
        type: "POST",
        url: '/Leave/InsertLeave',
        contentType: "application/json; charset=utf-8",
        contentType: false,
        processData: false,
        data: formData,
        success: function (data) {
            if (data === null) {
                if (data === 'Success') {
                    toastr.success('Leave inserted successfully');
                    GetLeaveList(1);
                    $('#Leave').click();
                    ClearData();
                }
                else if (data === 'Updated') {
                    toastr.success('Leave updated successfully');
                    GetLeaveList(1);
                    $('#Leave').click();
                    ClearData();
                }
                else if (data === 'Exists') {
                    toastr.error('Leave already exists!');
                    document.getElementById('Email').value = "";
                }
            }
            HideWait();
        },
        error: function (xyz) {
            HideWait();
            alert('errors');
        }
    });
}

function GetLeaveList(page) {


    var Id = 0;
    if (document.getElementById('PageSize') !== null) {
        PageSize = document.getElementById('PageSize').value;
    }
    else {
        PageSize = 10;
    }
    if (page === undefined) {
        page = 1;
    }
    var PageIndex = page;

    PageIndex = page;
    var cls = {
        Id: Id,
        PageSize: PageSize,
        PageIndex: PageIndex,
    }
    ShowWait();
    $.ajax({
        url: '/Leave/GetLeave',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {

            $('#tblBody').empty();
            $('#tblBody').append(data);
            HideWait();
        },
        error: function (xhr) {
            HideWait();
            alert('errors');
        }
    });
}

function deleteLeave() {
    var Id = document.getElementById('hdnintId').value;
    var cls = {
        Id: Id
    }
    ShowWait();
    $.ajax({
        url: '/Leave/deleteLeave',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {

            if (data.Response === 'Success') {

                toastr.success('Leave deleted successfully');
                document.getElementById('hdnintId').value = "0";
                GetLeaveList();
                $('#delete_Leave').click();
            }

            else {
                alert('error');
            }
            HideWait();
        },
        error: function (xhr) {
            HideWait();
            alert('errors');

        }
    });
}

function Clear() {

    document.getElementById('FromDate').value = "";
    document.getElementById('ToDate').value = "";
    document.getElementById('NoOfDays').value = "";
    document.getElementById('Reason').value = "";
    $('#errFromDate').html("");
    $('#errToDate').html("");
    $('#errDays').html("");
    $('#errReason').html("");
    $('#errLeaveType').html("");
    $("#LeaveType").val('0').trigger('change');
    document.getElementById('btnAdd').innerHTML = "Add";
    $("#btnAdd").attr('title', 'Upload');
    document.getElementById('PopupTitle').innerHTML = "Add Leave";
}

function ClearData() {

    var Id = document.getElementById('hdnintId').value;
    document.getElementById('FromDate').value = "";
    document.getElementById('ToDate').value = "";
    document.getElementById('NoOfDays').value = "";
    document.getElementById('Reason').value = "";
    $('#errFromDate').html("");
    $('#errToDate').html("");
    $('#errDays').html("");
    $('#errReason').html("");
    $('#errLeaveType').html("");
    $("#LeaveType").val('0').trigger('change');


    document.getElementById('hdnintId').value = "0";
    document.getElementById('btnAdd').innerHTML = "Add";
    $("#btnAdd").attr('title', 'Add');
    document.getElementById('PopupTitle').innerHTML = "Add Leave";
}

function opendeleteModel(id) {

    document.getElementById('hdnintId').value = id;
}

function openstatusModel(id) {

    document.getElementById('hdnintId').value = id;
}
function ApproveStatus() {

    var Id = document.getElementById('hdnintId').value;
    var cls = {
        Id: Id
    }
    ShowWait();
    $.ajax({
        url: '/Leave/ApproveStatus',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {
            if (data === 'success') {
                toastr.success('Leave approved successfully.');
                document.getElementById('hdnintId').value = "0";
                updateNotificationBadge();
                GetLeaveList(1);
                $('#status').click();
            }
            else if (data === 'Exist') {
                toastr.error('Exists');
            }
            else {
                alert('error');
            }
            HideWait();
        },
        error: function (xhr) {
            HideWait();
            alert('errors');
        }
    });
}

function RejectStatus() {
    var Id = document.getElementById('hdnintId').value;
    var cls = {
        Id: Id
    }
    ShowWait();
    $.ajax({
        url: '/Leave/RejectStatus',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {
            if (data === 'success') {
                toastr.success('Leave rejected successfully.');
                document.getElementById('hdnintId').value = "0";
                updateNotificationBadge();
                GetLeaveList(1);
                $('#status').click();
            }
            else if (data === 'Exist') {
                toastr.error('Exists');
            }
            else {
                alert('error');
            }
            HideWait();
        },
        error: function (xhr) {
            HideWait();
            alert('errors');
        }
    });
}