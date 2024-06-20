var type = 1
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

    var StudentId = $("#studentIdContainer").data("student-id");
    if (StudentId === null || StudentId === 0) {
        var currentDate = new Date();
        var formattedCurrentDate = currentDate.toISOString().split('T')[0];
        document.getElementById('Date').value = formattedCurrentDate;
        document.getElementById('Date').max = formattedCurrentDate;
        GetStudentAttandenceList(1);
    }
    else {
        GetStudentAttandenceListByStudentId(1);
    }
    GetStudent();
});

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
    var StudentId = "";
    for (var i = 0; i < ids.length; i++) {
        StudentId += ids[i] + ",";
    }
    StudentId = StudentId.slice(0, -1); // Remove the trailing comma

    // Set the value of the hidden field
    $("#chkVersIDs").val(StudentId);

    var VersData = {
        Status: $("#Status").val(),
        Date: $("#Date").val(),
        StudentId: StudentId
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
        if (UpgradeAttendanceData.StudentId === undefined || UpgradeAttendanceData.StudentId === null || UpgradeAttendanceData.StudentId.length <= 0) {
            toastr.error("Please select at least one Store.");
            isValid = false;
        }
        if (!isValid) {
            return false;
        }
        $('#Status_StudentAttandence').modal('show');
    }
    catch (e) {
        toastr.error("An error occurred. Please check the console for details.");
        console.error(e);
    }
}
function InsertData() {
    
    var UpgradeAttendanceData = LoadAttendanceData();
    var StudentId = UpgradeAttendanceData.StudentId;
    var Status = UpgradeAttendanceData.Status;
    var Date = UpgradeAttendanceData.Date;

    // Create an object to represent the data
    var model = {
        StudentId: StudentId,
        Status: Status,
        Date: Date
    };
    $('#StudentId').val(StudentId);
    ShowWait();
    $.ajax({
        type: "POST",
        url: '/StudentAttandence/InsertStudentAttandence',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(model), // Send data as JSON string
        success: function (data) {
            if (data !== null) {
                if (data === 'Success') {
                    toastr.success("Attedance inserted successfully.");
                    document.getElementById('Status').value = "2";
                    $('#errStatus').html("");
                    $('#Status_StudentAttandence').modal('hide');
                    GetStudentAttandenceList(1);
                }
                else {
                    toastr.success("Attedance updated successfully.");
                    document.getElementById('Status').value = "2";
                    $('#errStatus').html("");
                    $('#Status_StudentAttandence').modal('hide');
                    GetStudentAttandenceList(1);
                }
            }
        },
        error: function (response) {
            toastr.error("Error");
            console.error(response);
        }
    });
}


function ExportStudentAttendance() {
    
    var Id = 0;
    var StudentId = document.getElementById('ddlStudentId').value
    var Date = document.getElementById('Date').value

    var cls = {
        Id: Id,
        StudentId: StudentId,
        Date: Date,
    };

    ShowWait();
    $.ajax({
        url: '/StudentAttandence/ExpotToExcelStudentAttendanceReport',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {
            if (data === "success") {
                window.location.href = "/StudentAttandence/ExportToExcel";
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



function ClearData1(type) {

    var intId = document.getElementById('hdnintId').value;
    if (type === 1) {

        document.getElementById('strFile').value = "";
        $('#tblMsg').html("");
    }
}
function GetStudent() {
    var cls = {
    }
    $.ajax({
        url: '/Common/GetStudent',
        contentType: "application/json; charset=utf-8",
        type: "GET",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {

            var html = "";
            html = html + ' <option value="0" selected>Select Student</option>';
            for (var i = 0; i < data.LSTStudentList.length; i++) {
                html = html + '<option value="' + data.LSTStudentList[i].Id + '">' + data.LSTStudentList[i].FullName + '</option>';
                $("#StudentId").empty();
                $("#StudentId").append(html);
                $("#ddlStudentId").empty();
                $("#ddlStudentId").append(html);
            }
        }
    });
}


function GetStudentAttandenceList(page) {


    var Id = 0;
    /* var StudentId = document.getElementById('ddlStudentId').value*/
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
        StudentId: StudentId,
        Date: Date,
        PageSize: PageSize,
        PageIndex: PageIndex,
    }
    ShowWait();
    $.ajax({
        url: '/StudentAttandence/GetStudentAttandence',
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

function ClearSearchData(page) {


    var Id = 0;
    /* var StudentId = document.getElementById('ddlStudentId').value*/
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
        StudentId: StudentId,
        Date: Date,
        PageSize: PageSize,
        PageIndex: PageIndex,
    }
    ShowWait();
    $.ajax({
        url: '/StudentAttandence/ClearSearchData',
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

function GetStudentAttandenceListByStudentId(page) {
    
    var StudentId = $("#studentIdContainer").data("student-id");
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

        StudentId: StudentId,
        FromDate: FromDate,
        ToDate: ToDate,
        PageSize: PageSize,
        PageIndex: PageIndex,
    }
    ShowWait();
    $.ajax({
        url: '/StudentAttandence/GetStudentAttandenceByStudentId',
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


function GetSingleStudentAttandenceData(id) {
    $('#errName').html("");
    document.getElementById('btnAdd').innerHTML = "Update";
    $("#btnAdd").attr('title', 'Update');
    document.getElementById('PopupTitle').innerHTML = "Update StudentAttandence";
    var cls = {
        Id: id
    }

    $.ajax({
        url: '/StudentAttandence/GetSingleStudentAttandenceData',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {

            if (data !== null) {
                document.getElementById('hdnintId').value = data.LSTStudentAttandenceList[0].Id;
                $('#StudentId').val(data.LSTStudentAttandenceList[0].StudentId).trigger("change");
                if (data.LSTStudentAttandenceList[0].Status === true || data.LSTStudentAttandenceList[0].Status === 'True' || data.LSTStudentAttandenceList[0].Status === "true") {
                    $("#Active").prop('checked', true);
                    Hide();
                }
                else {
                    $("#InActive").prop('checked', true);
                    Show();
                }
                if (data.LSTStudentAttandenceList[0].Status === false || data.LSTStudentAttandenceList[0].Status === 'False' || data.LSTStudentAttandenceList[0].Status === "false") {
                    document.getElementById('LeaveType').value = data.LSTStudentAttandenceList[0].LeaveType;
                    $('#Reason').val(data.LSTStudentAttandenceList[0].Reason).trigger("change");
                    document.getElementById('Date').value = data.LSTStudentAttandenceList[0].Date;
                } else {
                    $("#LeaveType").val('0').trigger('change');
                    $('#Reason').val('').trigger("change");
                    document.getElementById('Date').value = data.LSTStudentAttandenceList[0].Date;
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

function deleteStudentAttandence() {
    var Id = document.getElementById('hdnintId').value;
    var cls = {
        Id: Id
    }
    ShowWait();
    $.ajax({
        url: '/StudentAttandence/deleteStudentAttandence',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {

            if (data.Response === 'Success') {

                toastr.success('StudentAttandence deleted successfully');
                document.getElementById('hdnintId').value = "0";
                GetStudentAttandenceList();
                $('#delete_StudentAttandence').click();
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
    document.getElementById('errStudent').value = "";
    $("#ddlStudentId").val('0').trigger('change');
    $("#StudentId").val('0').trigger('change');
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
        document.getElementById('errStudent').value = "";
        $("#LeaveType").val('0').trigger('change');
        $("#StudentId").val('0').trigger('change');
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
    else if (type === 2) {
        document.getElementById('FromDate').value = "";
        document.getElementById('ToDate').value = "";
        GetStudentAttandenceList();
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
