var type = 1
function ShowFilter() {
    if (type === 1) {
        $('#Filter').show();
        type = 2;
    }
    else {
        $('#Filter').hide();
        type = 1;
    }
}
$(document).ready(function () {
    var teacherId = $("#teacherIdContainer").data("teacher-id");
    if (teacherId === null || teacherId === 0) {
        var currentDate = new Date();
        var formattedCurrentDate = currentDate.toISOString().split('T')[0];
        document.getElementById('Date').value = formattedCurrentDate;
        document.getElementById('Date').max = formattedCurrentDate;
        GetTeacherAttandenceList(1);
    }
    else {
        GetTeacherAttandenceListByTeacherId(1);
    }
    GetTeacher();
});
function AttendanceType() {

    var Status = $('#Status').val();
    if (Status === "Absent") {
        leaveTypeDiv.style.display = "block";
    } else {
        leaveTypeDiv.style.display = "none";
    }
}

function LeaveType() {

    var LeaveType = $('#LeaveType').val();
    if (LeaveType === "Half-Day") {
        leaveSubTypeDiv.style.display = "block";
    } else {
        leaveSubTypeDiv.style.display = "none";
    }
}

function SelectAll() {
    var selectAllChecked = $('#selectAllCheckbox').prop('checked');
    $('.chkVersIDs').prop('checked', selectAllChecked);

}

$('.chkVersIDs').change(function () {
    var allChecked = $('.chkVersIDs:checked').length === $('.chkVersIDs').length;
    $('#selectAllCheckbox').prop('checked', allChecked);
});

function GetSelectedId() {
    var selectedValues = [];
    $('input[type="checkbox"][name="chkVersIDs"]:checked').each(function () {
        selectedValues.push($(this).data("id"));
    });
    return selectedValues;
}

function Show() {

    if (document.getElementById('InActive').checked) {
        document.getElementById('ifYes').style.visibility = 'visible';

    }
    else {
        document.getElementById('ifYes').style.visibility = 'hidden';
    }
}

function Hide() {

    if (document.getElementById('Active').checked) {

        document.getElementById('ifYes').style.visibility = 'hidden';

    }
    else {
        document.getElementById('ifYes').style.visibility = 'visible';
    }
}

function LoadAttendanceData() {
    var ids = GetSelectedId();
    var TeacherId = "";
    for (var i = 0; i < ids.length; i++) {
        TeacherId += ids[i] + ",";
    }
    TeacherId = TeacherId.slice(0, -1); // Remove the trailing comma

    // Set the value of the hidden field
    $("#chkVersIDs").val(TeacherId);

    var VersData = {
        Status: $("#Status").val(),
        Date: $("#Date").val(),
        LeaveType: $("#LeaveType").val(),
        LeaveSubType: $("#LeaveSubType").val(),
        TeacherId: TeacherId
    };
    return VersData;
}



function UpgradeAttendanceData() {
    try {
        var isValid = true;
        var UpgradeAttendanceData = LoadAttendanceData();
        if (UpgradeAttendanceData.Status === "2") {
            $("#errStatus").html("Please select status.");
            isValid = false;
        }
        if (UpgradeAttendanceData.TeacherId === undefined || UpgradeAttendanceData.TeacherId === null || UpgradeAttendanceData.TeacherId.length <= 0) {
            toastr.error("Please select at least one Store.");
            isValid = false;
        }
        if (!isValid) {
            return false;
        }
        $('#Status_TeacherAttandence').modal('show');
    }
    catch (e) {
        toastr.error("An error occurred. Please check the console for details.");
        console.error(e);
    }
}
function InsertData() {
    var UpgradeAttendanceData = LoadAttendanceData();
    var TeacherId = UpgradeAttendanceData.TeacherId;
    var Status = UpgradeAttendanceData.Status;
    var Date = UpgradeAttendanceData.Date;
    var LeaveType = UpgradeAttendanceData.LeaveType;
    var LeaveSubType = UpgradeAttendanceData.LeaveSubType;

    // Create an object to represent the data
    var model = {
        TeacherId: TeacherId,
        Status: Status,
        Date: Date,
        LeaveType: LeaveType,
        LeaveSubType: LeaveSubType,
    };
    $('#TeacherId').val(TeacherId);
    ShowWait();
    $.ajax({
        type: "POST",
        url: '/TeacherAttandence/InsertTeacherAttandence',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(model), // Send data as JSON string
        success: function (data) {
            if (data !== null) {
                if (data === 'Success') {
                    toastr.success("Attedance inserted successfully.");
                    document.getElementById('Status').value = "2";
                    $('#errStatus').html("");
                    $('#Status_TeacherAttandence').modal('hide');
                    GetTeacherAttandenceList(1);
                    ClearData2();
                }
                else {
                    toastr.success("Attedance updated successfully.");
                    document.getElementById('Status').value = "2";
                    $('#errStatus').html("");
                    $('#Status_TeacherAttandence').modal('hide');
                    GetTeacherAttandenceList(1);
                    ClearData2();
                }
            }
        },
        error: function (response) {
            toastr.error("Error");
            console.error(response);
        }
    });
}


function ExportTeacherAttendance() {
    
    var Id = 0;
    var TeacherId = document.getElementById('ddlTeacherId').value
    var Date = document.getElementById('Date').value

    var cls = {
        Id: Id,
        TeacherId: TeacherId,
        Date: Date,
    };

    ShowWait();
    $.ajax({
        url: '/TeacherAttandence/ExpotToExcelTeacherAttendanceReport',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {
            if (data === "success") {
                window.location.href = "/TeacherAttandence/ExportToExcel";
            }
            else {
                alert("No Record Found.");
                HideWait();
            }
            HideWait();
        },
        error: function (xhr) {
            HideWait();
            alert('Error');
        }
    });
}



function ClearData1() {

    $("#Status").val('0').trigger('change');
    $("#LeaveType").val('None').trigger('change');
    $("#LeaveSubType").val('None').trigger('change');
    leaveTypeDiv.style.display = "none";
    leaveSubTypeDiv.style.display = "none";
}


function ClearData2() {

    var intId = document.getElementById('hdnintId').value;
    if (type === 1) {

        document.getElementById('strFile').value = "";
        $('#tblMsg').html("");
    }
}
function GetTeacher() {
    var cls = {
    }
    $.ajax({
        url: '/Common/GetTeacher',
        contentType: "application/json; charset=utf-8",
        type: "GET",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {

            var html = "";
            html = html + ' <option value="0" selected>Select Teacher</option>';
            for (var i = 0; i < data.LSTTeacherList.length; i++) {
                html = html + '<option value="' + data.LSTTeacherList[i].Id + '">' + data.LSTTeacherList[i].FullName + '</option>';
                $("#TeacherId").empty();
                $("#TeacherId").append(html);
                $("#ddlTeacherId").empty();
                $("#ddlTeacherId").append(html);
            }
        }
    });
}


function GetTeacherAttandenceList(page) {

    debugger
    var Id = 0;
   /* var TeacherId = document.getElementById('ddlTeacherId').value*/
    var Date = document.getElementById('Date').value
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
    var model= {
        Id: Id,
        TeacherId: TeacherId,
        Date: Date,
        PageSize: PageSize,
        PageIndex: PageIndex,
    }
    ShowWait();
    $.ajax({
        url: '/TeacherAttandence/GetTeacherAttandence',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            model: model
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

function ClearSearchData(page) {


    var Id = 0;
    /* var TeacherId = document.getElementById('ddlTeacherId').value*/
    var Date = document.getElementById('Date').value
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
        TeacherId: TeacherId,
        Date: Date,
        PageSize: PageSize,
        PageIndex: PageIndex,
    }
    ShowWait();
    $.ajax({
        url: '/TeacherAttandence/ClearSearchData',
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

function GetTeacherAttandenceListByTeacherId(page) {
    debugger
    var TeacherId = $("#teacherIdContainer").data("teacher-id");
    var FromDate = document.getElementById('FromDate').value
    var ToDate = document.getElementById('ToDate').value
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
       
        TeacherId: TeacherId,
        FromDate: FromDate,
        ToDate: ToDate,
        PageSize: PageSize,
        PageIndex: PageIndex,
    }
    ShowWait();
    $.ajax({
        url: '/TeacherAttandence/GetTeacherAttandenceByTeacherId',
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


function GetSingleTeacherAttandenceData(id) {
    $('#errName').html("");
    document.getElementById('btnAdd').innerHTML = "Update";
    $("#btnAdd").attr('title', 'Update');
    document.getElementById('PopupTitle').innerHTML = "Update TeacherAttandence";
    var cls = {
        Id: id
    }

    $.ajax({
        url: '/TeacherAttandence/GetSingleTeacherAttandenceData',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {

            if (data !== null) {
                document.getElementById('hdnintId').value = data.LSTTeacherAttandenceList[0].Id;
                $('#TeacherId').val(data.LSTTeacherAttandenceList[0].TeacherId).trigger("change");
                if (data.LSTTeacherAttandenceList[0].Status === true || data.LSTTeacherAttandenceList[0].Status === 'True' || data.LSTTeacherAttandenceList[0].Status === "true") {
                    $("#Active").prop('checked', true);
                    Hide();
                }
                else {
                    $("#InActive").prop('checked', true);
                    Show();
                }
                if (data.LSTTeacherAttandenceList[0].Status === false || data.LSTTeacherAttandenceList[0].Status === 'False' || data.LSTTeacherAttandenceList[0].Status === "false") {
                    document.getElementById('LeaveType').value = data.LSTTeacherAttandenceList[0].LeaveType;
                    $('#Reason').val(data.LSTTeacherAttandenceList[0].Reason).trigger("change");
                    document.getElementById('Date').value = data.LSTTeacherAttandenceList[0].Date;
                } else {
                    $("#LeaveType").val('0').trigger('change');
                    $('#Reason').val('').trigger("change");
                    document.getElementById('Date').value = data.LSTTeacherAttandenceList[0].Date;
                }
            }
            else {
                alert('error');
            }

        },
        error: function (xhr) {

            alert('errors');
        }
    });
}

function deleteTeacherAttandence() {
    var Id = document.getElementById('hdnintId').value;
    var cls = {
        Id: Id
    }
    ShowWait();
    $.ajax({
        url: '/TeacherAttandence/deleteTeacherAttandence',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {

            if (data.Response === 'Success') {

                toastr.success('TeacherAttandence deleted successfully');
                document.getElementById('hdnintId').value = "0";
                GetTeacherAttandenceList();
                $('#delete_TeacherAttandence').click();
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
    document.getElementById('datepicker').value = "";

    document.getElementById('Reason').value = "";
    document.getElementById('errLeaveType').value = "";
    document.getElementById('errReason').value = "";
    document.getElementById('errTeacher').value = "";
    $("#ddlTeacherId").val('0').trigger('change');
    $("#TeacherId").val('0').trigger('change');
    $("#LeaveType").val('0').trigger('change');
    document.getElementById('btnAdd').innerHTML = "Add";
    $("#btnAdd").attr('title', 'Upload');
    document.getElementById('PopupTitle').innerHTML = "Add Attandence";
}

function ClearData(type) {

    if (type === 1) {
        var Id = document.getElementById('hdnintId').value;
        document.getElementById('datepicker').value = "";
        document.getElementById('Reason').value = "";
        document.getElementById('errLeaveType').value = "";
        document.getElementById('errReason').value = "";
        document.getElementById('errTeacher').value = "";
        $("#LeaveType").val('0').trigger('change');
        $("#TeacherId").val('0').trigger('change');
        if (Id === "0") {

            document.getElementById('hdnintId').value = "0";
            document.getElementById('btnAdd').innerHTML = "Add";
            $("#btnAdd").attr('title', 'Add');
            document.getElementById('PopupTitle').innerHTML = "Add Attandence";
        }
        else {
            document.getElementById('btnAdd').innerHTML = "Update";
            $("#btnAdd").attr('title', 'Update');
            document.getElementById('PopupTitle').innerHTML = "Update Attandence";
        }
    }
    else if (type === 2){
        document.getElementById('FromDate').value = "";
        document.getElementById('ToDate').value = "";
        GetTeacherAttandenceList();
    }
    else {
        document.getElementById('FromDate').value = "";
        document.getElementById('ToDate').value = "";
        ClearSearchData();
    }
}


function Clear1() {

    document.getElementById('strFile').value = "";
    $('#tblMsg').html("");
    document.getElementById('btnAdd1').innerHTML = "Import";
    $("#btnAdd1").attr('title', 'Import');
    document.getElementById('PopupTitle').innerHTML = "Import BulkAttendance";
}

function opendeleteModel(id) {

    document.getElementById('hdnintId').value = id;
}
