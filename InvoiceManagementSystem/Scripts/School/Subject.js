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
    GetClassRoom();
    GetSubjectList(1);
    $('#Filter').hide();

});
function InsertData() {

    var val = true;
    var Id = $('#hdnintId').val();
    var ClassId = $('#ClassId').val();
    if (ClassId === "0" || ClassId === 0) {
        $("#errClassId").html("Please select class room.");
        val = false;
    }
    var SearchText = $('#SearchText').val();
    var SubjectName = $('#SubjectName').val();
    if (SubjectName === "" || /\S/.test(SubjectName) === false) {
        $("#errSubjectName").html("Please enter SubjectName.");
        val = false;
    }
    else {
        var isValid = SubjectName.match(".*[a-zA-Z]+.*");
        if (isValid === null) {
            $("#errSubjectName").html("Please enter valid SubjectName.");
            val = false;
        }
        else {
            $("#errSubjectName").html("");
        }
    }
    var formData = new FormData();
    if (val === false) {
        return;
    }

    formData.append('Id', Id);
    formData.append('ClassId', ClassId);
    formData.append('SearchText', SearchText);
    formData.append('SubjectName', SubjectName);
    ShowWait();
    $.ajax({
        type: "POST",
        url: '/Subject/InsertSubject',
        contentType: "application/json; charset=utf-8",
        contentType: false,
        processData: false,
        data: formData,
        success: function (data) {
            if (data !== null) {
                if (data === 'Success') {
                    toastr.success('Subject inserted successfully');
                    GetSubjectList(1);
                    $('#Subject').click();
                    ClearData(1);
                }
                else if (data === 'Updated') {
                    toastr.success('Subject updated successfully');
                    GetSubjectList(1);
                    $('#Subject').click();
                    ClearData(1);
                }
                else if (data === 'Exists') {
                    toastr.error('Subject already exists!');
                    document.getElementById('SubjectName').value = "";
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

function GetSubjectList(page) {

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
    var model = {
        Id: Id,
        SearchText: SearchText,
        intActive: intActive,
        ClassId: ClassId,
        PageSize: PageSize,
        PageIndex: PageIndex,
    }
    ShowWait();
    $.ajax({
        url: '/Subject/GetSubject',
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

function GetSingleSubjectData(id) {
    $('#errSubjectName').html("");
    document.getElementById('btnAdd').innerHTML = "Update";
    $("#btnAdd").attr('title', 'Update');
    document.getElementById('PopupTitle').innerHTML = "Update Subject";
    var cls = {
        Id: id
    }

    $.ajax({
        url: '/Subject/GetSingleSubjectData',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {

            if (data !== null) {
                document.getElementById('hdnintId').value = data.LSTSubjectList[0].Id;
                document.getElementById('SubjectName').value = data.LSTSubjectList[0].SubjectName;
                $('#ClassId').val(data.LSTSubjectList[0].ClassId).trigger("change");
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

function deleteSubject() {
    var Id = document.getElementById('hdnintId').value;
    var cls = {
        Id: Id
    }
    ShowWait();
    $.ajax({
        url: '/Subject/deleteSubject',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {

            if (data.Response === 'Success' ) {

                toastr.success('Subject deleted successfully');
                document.getElementById('hdnintId').value = "0";
                GetSubjectList();
                $('#delete_Subject').click();
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
    document.getElementById('SubjectName').value = "";
    $("#ClassId").val('0').trigger('change');
    $('#errSubjectName').html("");
    $('#errClassId').html("");
    $("#ddlClassId").val('0').trigger('change');
    document.getElementById('btnAdd').innerHTML = "Add";
    $("#btnAdd").attr('title', 'Upload');
    document.getElementById('PopupTitle').innerHTML = "Add Subject";
}

function UpdateStatus() {
    var Id = document.getElementById('hdnintId').value;
    var cls = {
        Id: Id
    }
    ShowWait();
    $.ajax({
        url: '/Subject/UpdateStatus',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {
            if (data === 'success') {
                toastr.success('status updated successfully.');
                document.getElementById('hdnintId').value = "0";
                GetSubjectList(1);
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
        document.getElementById('SubjectName').value = "";
        $('#errSubjectName').html("");
        $('#errClassId').html("");
        $("#ClassId").val('0').trigger('change');

        if (Id === "0") {

            document.getElementById('hdnintId').value = "0";
            document.getElementById('btnAdd').innerHTML = "Add";
            $("#btnAdd").attr('title', 'Add');
            document.getElementById('PopupTitle').innerHTML = "Add Subject";
        }
        else {
            document.getElementById('btnAdd').innerHTML = "Update";
            $("#btnAdd").attr('title', 'Update');
            document.getElementById('PopupTitle').innerHTML = "Update Subject";
        }
    }
    else {
        document.getElementById('SearchText').value = "";
        document.getElementById('intActive').value = '3';
        $("#ddlClassId").val('0').trigger('change');
        GetSubjectList();
    }
}

function opendeleteModel(id) {

    document.getElementById('hdnintId').value = id;
}

function openstatusModel(id) {

    document.getElementById('hdnintId').value = id;
}

function ExportSubject() {
    var Id = 0;
    var SearchText = document.getElementById('SearchText').value;
    var intActive = document.getElementById('intActive').value;
    var ClassId = document.getElementById('ddlClassId').value;

    var cls = {
        Id: Id,
        SearchText: SearchText,
        intActive: intActive,
        ClassId: ClassId,
    };

    ShowWait();
    $.ajax({
        url: '/Subject/ExpotToExcelSubjectReport',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {
            if (data === "success") {
                window.location.href = "/Subject/ExportToExcel";
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