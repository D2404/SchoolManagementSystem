
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
    $('#Filter').hide();
    var currentTime = new Date();

    // Format the time as HH:mm
    var hours = currentTime.getHours().toString().padStart(2, '0');
    var minutes = currentTime.getMinutes().toString().padStart(2, '0');
    var formattedTime = hours + ':' + minutes;

    // Set the value of the input element
    document.getElementById('StartTime').value = formattedTime;
    GetClassRoom();
    GetTeacher();
    GetTimetableList(1);
    
});
function myFunction() {
    document.getElementById("myDropdown").classList.toggle("show");
}

function InsertData() {

    var val = true;
    var Id = $('#hdnintId').val();
    var TeacherId = $('#TeacherId').val();
    if (TeacherId === 0 || TeacherId === "0") {
        $("#errTeacherId").html("Please select teacher.");
        val = false;
    }
    var ClassId = $('#ClassId').val();
    if (ClassId === 0 || ClassId === "0") {
        $("#errClassId").html("Please select class.");
        val = false;
    }
    var SubjectId = $('#SubjectId').val();
    if (SubjectId === 0 || SubjectId === "0") {
        $("#errSubjectId").html("Please select subject.");
        val = false;
    }
    var Days = $('#Days').val();
    if (Days === 0 || Days === "0") {
        $("#errDays").html("Please select days.");
        val = false;
    }
    var StartTime = $('#StartTime').val();
    if (StartTime === '') {
        $("#errStartTime").html("Please select starttime.");
        val = false;
    }
    var EndTime = $('#EndTime').val();
    if (EndTime ==='') {
        $("#errEndTime").html("Please select endtime.");
        val = false;
    }
    var formData = new FormData();
    if (val === false) {
        return;
    }

    formData.append('Id', Id);
    formData.append('TeacherId', TeacherId);
    formData.append('ClassId', ClassId);
    formData.append('SubjectId', SubjectId);
    formData.append('Days', Days);
    formData.append('StartTime', StartTime);
    formData.append('EndTime', EndTime);
    ShowWait();
    $.ajax({
        type: "POST",
        url: '/Timetable/InsertTimetable',
        contentType: "application/json; charset=utf-8",
        contentType: false,
        processData: false,
        data: formData,
        success: function (data) {
            if (data !== null) {
                if (data === 'Success') {
                    toastr.success('Timetable inserted successfully');
                    GetTimetableList(1);
                    $('#Timetable').click();
                    ClearData(1);
                }
                else if (data === 'Updated') {
                    toastr.success('Timetable updated successfully');
                    GetTimetableList(1);
                    $('#Timetable').click();
                    ClearData(1);
                }
                else if (data === 'Exists') {
                    toastr.error('Timetable already exists!');
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
function GetClassRoom() {
    var cls = {
    }
    $.ajax({
        url: '/Common/GetClassRoom',
        contentType: "application/json; charset=utf-8",
        type: "GET",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {

            var html = "";
            html = html + ' <option value="0" selected>Select ClassRoom</option>';
            for (var i = 0; i < data.LSTClassRoomList.length; i++) {
                html = html + '<option value="' + data.LSTClassRoomList[i].Id + '">' + data.LSTClassRoomList[i].ClassNo + '</option>';
                $("#ClassId").empty();
                $("#ClassId").append(html);
            }
        }
    });
}
function onClass() {
    var ClassId = $('#ClassId').val();
    $.ajax({
        type: "GET",
        url: '/Common/LoadSubject?ClassId=' + ClassId,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            //$('#TeacherId').empty();
            //$("#TeacherId").append($("<option     />").val("0").text("Select Teacher name.."));
            //$.each(data, function (i, v) {
            //    $("#TeacherId").append($("<option     />").val(v.TeacherId).text(v.TeacherName));
            //});
            $('#SubjectId').empty();
            $("#SubjectId").append($("<option     />").val("0").text("Select subject name.."));
            $.each(data, function (i, v) {
                $("#SubjectId").append($("<option     />").val(v.SubjectId).text(v.SubjectName));
            });
            HideWait();
        },
        failure: function () {
            HideWait();
            alert("Failed!");
        }
    });
}
function GetTimetableList(page) {

    var Id = 0;
    var Days = document.getElementById('ddlDays').value;
    var TeacherId = document.getElementById('ddlTeacherId').value;
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
        Days: Days,
        ClassId: ClassId,
        TeacherId: TeacherId,
        PageSize: PageSize,
        PageIndex: PageIndex,
    }
    ShowWait();
    $.ajax({
        url: '/Timetable/GetTimetable',
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
function ExportTimetable() {
    var Id = 0;
    var Days = document.getElementById('ddlDays').value;
    var TeacherId = document.getElementById('ddlTeacherId').value;

    var cls = {
        Id: Id,
        Days: Days,
        TeacherId: TeacherId,
    };

    ShowWait();
    $.ajax({
        url: '/Timetable/ExpotToExcelTimetableReport',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {
            if (data === "success") {
                window.location.href = "/Timetable/ExportToExcel";
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
function GetSingleTimetableData(id) {
    $('#errSubjectName').html("");
    document.getElementById('btnAdd').innerHTML = "Update";
    $("#btnAdd").attr('title', 'Update');
    document.getElementById('PopupTitle').innerHTML = "Update Timetable";
    var cls = {
        Id: id
    }

    $.ajax({
        url: '/Timetable/GetSingleTimetableData',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {

            if (data !== null) {
                document.getElementById('hdnintId').value = data.LSTTimetableList[0].Id;
                $('#ClassId').val(data.LSTTimetableList[0].ClassId).trigger("change");
                $('#TeacherId').val(data.LSTTimetableList[0].TeacherId).trigger("change");
                $('#SubjectId').val(data.LSTTimetableList[0].SubjectId).trigger("change");
                $('#StartTime').val(data.LSTTimetableList[0].StartTime).trigger("change");
                $('#EndTime').val(data.LSTTimetableList[0].EndTime).trigger("change");
                //$('#StartTime').on('change', function () {
                //    // Your code to handle the time change goes here
                //    var newTime = $(this).val();
                //    alert('Time changed to: ' + newTime);

                //    // You can perform additional actions based on the new time value
                //    // For example, update other elements or make an AJAX request, etc.
                //});
                //$('#EndTime').val(data.LSTTimetableList[0].EndTime).trigger("change");
                $('#Days').val(data.LSTTimetableList[0].Days).trigger("change");
                /*document.getElementById('StartTime').value = data.LSTClassRoomList[0].StartTime;*/
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

function deleteTimetable() {
    var Id = document.getElementById('hdnintId').value;
    var cls = {
        Id: Id
    }
    ShowWait();
    $.ajax({
        url: '/Timetable/deleteTimetable',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {

            if (data.Response === 'Success') {

                toastr.success('Timetable deleted successfully');
                document.getElementById('hdnintId').value = "0";
                GetTimetableList();
                $('#delete_Timetable').click();
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

    document.getElementById('hdnintId').value = "0";
    document.getElementById('ClassId').value = "0";
    document.getElementById('TeacherId').value = "0";
    document.getElementById('SubjectId').value = "0";
    document.getElementById('Days').value = "0";
    document.getElementById('StartTime').value = "";
    document.getElementById('EndTime').value = "";
    $('#errStartTime').html("");
    $('#errEndTime').html("");
    $('#errClassId').html("");
    $('#errTeacherId').html("");
    $('#errSubjectId').html("");
    $('#errDays').html("");
    document.getElementById('btnAdd').innerHTML = "Add";
    $("#btnAdd").attr('title', 'Upload');
    document.getElementById('PopupTitle').innerHTML = "Add Timetable";
}
function UpdateStatus() {
    var Id = document.getElementById('hdnintId').value;
    var cls = {
        Id: Id
    }
    ShowWait();
    $.ajax({
        url: '/Timetable/UpdateStatus',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {
            if (data === 'success') {
                toastr.success('status updated successfully.');
                document.getElementById('hdnintId').value = "0";
                GetTimetableList(1);
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
function ClearData(type) {

    if (type === 1) {
        var Id = document.getElementById('hdnintId').value;

        document.getElementById('ClassId').value = "0";
        document.getElementById('TeacherId').value = "0";
        document.getElementById('SubjectId').value = "0";
        document.getElementById('Days').value = "0";
        document.getElementById('StartTime').value = "";
        document.getElementById('EndTime').value = "";
        $('#errStartTime').html("");
        $('#errEndTime').html("");
        $('#errClassId').html("");
        $('#errTeacherId').html("");
        $('#errSubjectId').html("");
        $('#errDays').html("");

        if (Id === "0") {

            document.getElementById('hdnintId').value = "0";
            document.getElementById('btnAdd').innerHTML = "Add";
            $("#btnAdd").attr('title', 'Add');
            document.getElementById('PopupTitle').innerHTML = "Add Timetable";
        }
        else {
            document.getElementById('btnAdd').innerHTML = "Update";
            $("#btnAdd").attr('title', 'Update');
            document.getElementById('PopupTitle').innerHTML = "Update Timetable";
        }
    }
    else {
        document.getElementById('ddlDays').value = '0';
        $("#ddlTeacherId").val('0').trigger('change');
        GetTimetableList();
    }
}
function RemoveFile() {
    document.getElementById('divUploadFile').style.display = "block";
    document.getElementById('divFile').style.display = "none";
    //document.getElementById('hdnstrFile').value = "";
    bISFile = 0;
}


function opendeleteModel(id) {

    document.getElementById('hdnintId').value = id;
}
function openstatusModel(id) {

    document.getElementById('hdnintId').value = id;
}
