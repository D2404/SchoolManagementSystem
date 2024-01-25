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
    GetStudentAttandenceList(1);
    GetStudent();
});
function myFunction() {
    document.getElementById("myDropdown").classList.toggle("show");
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


function InsertData() {

    var val = true;
    var Id = $('#hdnintId').val();
    var SearchText = $('#SearchText').val();
    var TeacherId = $('#TeacherId').val();
    if (TeacherId === 0) {
        $("#errStudent").html("Please select teacher.");
        val = false;
    }
    var date = $('#date').val();
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
    var MonthId = 5
    var YearId = 1
    formData.append('Id', Id);
    formData.append('TeacherId', TeacherId);
    formData.append('Status', Status);
    formData.append('SearchText', SearchText);
    formData.append('date', date);
    formData.append('LeaveType', LeaveType);
    formData.append('Reason', Reason);
    formData.append('MonthId', MonthId);
    formData.append('YearId', YearId);
    ShowWait();
    $.ajax({
        type: "POST",
        url: '/StudentAttandence/InsertStudentAttandence',
        contentType: "application/json; charset=utf-8",
        contentType: false,
        processData: false,
        data: formData,
        success: function (data) {
            if (data !== null) {
                if (data === 'Success') {
                    toastr.success('StudentAttandence inserted successfully');
                    GetStudentAttandenceList(1);
                    $('#StudentAttandence').click();
                    ClearData(1);
                }
                else if (data === 'Updated') {
                    toastr.success('StudentAttandence updated successfully');
                    GetStudentAttandenceList(1);
                    $('#StudentAttandence').click();
                    ClearData(1);
                }
                else if (data === 'Exists') {
                    toastr.error('StudentAttandence already exists!');
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

function GetStudent() {

    var cls = {
    }
    $.ajax({
        url: '/StudentAttandence/GetStudent',
        contentType: "application/json; charset=utf-8",
        type: "GET",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {

            var html = "";
            html = html + ' <option value="0" selected>Select Student</option>';
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

function GetStudentAttandenceList(page) {


    var Id = 0;
    var intActive = document.getElementById('intActive').value;
    var TeacherId = document.getElementById('ddlTeacherId').value
    var date = document.getElementById('date').value
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
        date: date,
        intActive: intActive,
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

function GetSingleTeacherAttandenceData(id) {
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
                document.getElementById('hdnintId').value = data.LSTTeacherAttandenceList[0].Id;
                $('#TeacherId').val(data.LSTTeacherAttandenceList[0].TeacherId).trigger("change");
                if (data.LSTTeacherAttandenceList[0].Status === true) {
                    $("#Active").prop('checked', true);
                    Hide();
                }
                else {
                    $("#InActive").prop('checked', true);
                    Show();
                }
                document.getElementById('LeaveType').value = data.LSTTeacherAttandenceList[0].LeaveType;
                $('#Reason').val(data.LSTTeacherAttandenceList[0].Reason).trigger("change");
                document.getElementById('date').value = data.LSTTeacherAttandenceList[0].Date;

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
    document.getElementById('date').value = "";
    document.getElementById('Reason').value = "";
    $('#errLeaveType').html("");
    $('#errReason').html("");
    $('#errStudent').html("");
    document.getElementById('errStudent').value = "";
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
        document.getElementById('date').value = "";
        document.getElementById('Reason').value = "";
        $('#errLeaveType').html("");
        $('#errReason').html("");
        $('#errStudent').html("");
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
        GetStudentAttandenceList();
    }
}

function opendeleteModel(id) {

    document.getElementById('hdnintId').value = id;
}
