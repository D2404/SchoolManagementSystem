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
    var currentDate = new Date().toISOString().slice(0, 10);
    document.getElementById('Date').value = currentDate;
    GetTeacherAttandenceList(1);
    GetTeacher();
});


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

function InsertData() {

    var val = true;
    var Id = $('#hdnintId').val();
    var SearchText = $('#SearchText').val();
    var TeacherId = $('#TeacherId').val();
    if (TeacherId === 0) {
        $("#errTeacher").html("Please select teacher.");
        val = false;
    }
    var Date = $('#Date').val();
    var LeaveType = $('#LeaveType').val();
    var Reason = $('#Reason').val();
    var Status = $("input[type='radio']:checked").val();
    if (Status === "Active") {
        Status = true
    }
    else {
        Status = false
    }

    if (Status === 0) {
        if (LeaveType === 0) {
            $("#errLeaveType").html("Please enter leave type.");
            val = false;
        }
        if (Reason === "" || /\S/.test(Reason) === false) {
            $("#errReason").html("Please enter reason.");
            val = false;
        }
    }

    var formData = new FormData();
    if (val === false) {
        return;
    }

    formData.append('Id', Id);
    formData.append('TeacherId', TeacherId);
    formData.append('Status', Status);
    formData.append('SearchText', SearchText);
    formData.append('Date', Date);
    formData.append('LeaveType', LeaveType);
    formData.append('Reason', Reason);
    ShowWait();
    $.ajax({
        type: "POST",
        url: '/TeacherAttandence/InsertTeacherAttandence',
        contentType: "application/json; charset=utf-8",
        contentType: false,
        processData: false,
        data: formData,
        success: function (data) {
            if (data !== null) {
                if (data === 'Success') {
                    toastr.success('TeacherAttandence inserted successfully');
                    GetTeacherAttandenceList(1);
                    $('#TeacherAttandence').click();
                    ClearData(1);
                }
                else if (data === 'Updated') {
                    toastr.success('TeacherAttandence updated successfully');
                    GetTeacherAttandenceList(1);
                    $('#TeacherAttandence').click();
                    ClearData(1);
                }
                else if (data === 'Exists') {
                    toastr.error('TeacherAttandence already exists!');
                    document.getElementById('Date').value = "";
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

function ClearData1(type) {

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


    var Id = 0;
    var intActive = document.getElementById('intActive').value;
    var TeacherId = document.getElementById('ddlTeacherId').value
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
        intActive: intActive,
        PageSize: PageSize,
        PageIndex: PageIndex,
    }
    ShowWait();
    $.ajax({
        url: '/TeacherAttandence/GetTeacherAttandence',
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
        document.getElementById('intActive').value = '3';
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
    else {
        document.getElementById('intActive').value = '3';
        $("#ddlTeacherId").val('0').trigger('change');
        GetTeacherAttandenceList();
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
