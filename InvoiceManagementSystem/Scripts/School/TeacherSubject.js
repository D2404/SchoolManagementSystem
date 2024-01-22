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
    GetClassRoom();
    GetTeacher();
    GetTeacherSubjectList(1);
});

function InsertData() {

    var val = true;
    var Id = $('#hdnintId').val();
    var TeacherId = $('#TeacherId').val();
    var SearchText = $('#SearchText').val();
    var ClassId = $('#ClassId').val();
    var SubjectId = $('#SubjectId').val();
    var formData = new FormData();
    if (val === false) {
        return;
    }

    formData.append('Id', Id);
    formData.append('TeacherId', TeacherId);
    formData.append('ClassId', ClassId);
    formData.append('SubjectId', SubjectId);
    formData.append('SearchText', SearchText);
    ShowWait();
    $.ajax({
        type: "POST",
        url: '/TeacherSubject/InsertTeacherSubject',
        contentType: "application/json; charset=utf-8",
        contentType: false,
        processData: false,
        data: formData,
        success: function (data) {
            if (data !== null) {
                if (data === 'Success' ) {
                    toastr.success('TeacherSubject inserted successfully');
                    GetTeacherSubjectList(1);
                    $('#TeacherSubject').click();
                    ClearData(1);
                }
                else if (data === 'Updated' ) {
                    toastr.success('TeacherSubject updated successfully');
                    GetTeacherSubjectList(1);
                    $('#TeacherSubject').click();
                    ClearData(1);
                }
                else if (data === 'Exists') {
                    toastr.error('TeacherSubject already exists!');
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
                $("#ddlClassId").empty();
                $("#ddlClassId").append(html);
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

            $('#SubjectId').empty();
            $("#SubjectId").append($("<option />").val("0").text("Select subject name.."));
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
function GetTeacherSubjectList(page) {
    var Id = 0;
    var SearchText = document.getElementById('SearchText').value;
    var intActive = document.getElementById('intActive').value;
    var ClassId = document.getElementById('ddlClassId').value;
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
        SearchText: SearchText,
        intActive: intActive,
        ClassId: ClassId,
        PageSize: PageSize,
        PageIndex: PageIndex,
    }
    ShowWait();
    $.ajax({
        url: '/TeacherSubject/GetTeacherSubject',
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

function GetSingleTeacherSubjectData(id) {
    $('#errSubjectName').html("");
    document.getElementById('btnAdd').innerHTML = "Update";
    $("#btnAdd").attr('title', 'Update');
    document.getElementById('PopupTitle').innerHTML = "Update TeacherSubject";
    var cls = {
        Id: id
    }

    $.ajax({
        url: '/TeacherSubject/GetSingleTeacherSubjectData',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {

            if (data !== null) {
                document.getElementById('hdnintId').value = data.LSTTeacherSubjectList[0].Id;
                $('#ClassId').val(data.LSTTeacherSubjectList[0].ClassId).trigger("change");
                $('#TeacherId').val(data.LSTTeacherSubjectList[0].TeacherId).trigger("change");
                $('#SubjectId').val(data.LSTTeacherSubjectList[0].SubjectId).trigger("change");
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

function deleteTeacherSubject() {
    var Id = document.getElementById('hdnintId').value;
    var cls = {
        Id: Id
    }
    ShowWait();
    $.ajax({
        url: '/TeacherSubject/deleteTeacherSubject',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {

            if (data.Response === 'Success' ) {

                toastr.success('TeacherSubject deleted successfully');
                document.getElementById('hdnintId').value = "0";
                GetTeacherSubjectList();
                $('#delete_TeacherSubject').click();
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

    document.getElementById('ClassId').value = "0";
    document.getElementById('TeacherId').value = "0";
    document.getElementById('SubjectId').value = "0";
    document.getElementById('btnAdd').innerHTML = "Add";
    $("#btnAdd").attr('title', 'Upload');
    document.getElementById('PopupTitle').innerHTML = "Add TeacherSubject";
}
function UpdateStatus() {
    var Id = document.getElementById('hdnintId').value;
    var cls = {
        Id: Id
    }
    ShowWait();
    $.ajax({
        url: '/TeacherSubject/UpdateStatus',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {
            if (data === 'success') {
                toastr.success('status updated successfully.');
                document.getElementById('hdnintId').value = "0";
                GetTeacherSubjectList(1);
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

        if (Id === "0") {

            document.getElementById('hdnintId').value = "0";
            document.getElementById('btnAdd').innerHTML = "Add";
            $("#btnAdd").attr('title', 'Add');
            document.getElementById('PopupTitle').innerHTML = "Add TeacherSubject";
        }
        else {
            document.getElementById('btnAdd').innerHTML = "Update";
            $("#btnAdd").attr('title', 'Update');
            document.getElementById('PopupTitle').innerHTML = "Update TeacherSubject";
        }
    }
    else {
        document.getElementById('SearchText').value = "";
        document.getElementById('intActive').value = '3';
        $("#ddlClassId").val('0').trigger('change');
        GetTeacherSubjectList();
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
