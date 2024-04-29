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
    GetClassRoom();
    GetSection();
    GetClassSectionList(1);
    $('#FilterDiv').hide();

});
function InsertData() {

    var val = true;
    var Id = $('#hdnintId').val();
    var ClassId = $('#ClassId').val();
    if (ClassId === "0") {
        $("#errClassId").html("Please select class");
        val = false;
    }
    var SectionId = $('#SectionId').val();
    if (SectionId === "0") {
        $("#errSectionId").html("Please select section");
        val = false;
    }
    var SearchText = $('#SearchText').val();
    var formData = new FormData();
    if (val === false) {
        return;
    }

    formData.append('Id', Id);
    formData.append('ClassId', ClassId);
    formData.append('SectionId', SectionId);
    formData.append('SearchText', SearchText);
    ShowWait();
    $.ajax({
        type: "POST",
        url: '/ClassSection/InsertClassSection',
        contentType: "application/json; charset=utf-8",
        contentType: false,
        processData: false,
        data: formData,
        success: function (data) {
            if (data !== null) {
                if (data === 'Success') {
                    toastr.success('ClassSection inserted successfully');
                    GetClassSectionList(1);
                    $('#ClassSection').click();
                    ClearData(1);
                }
                else if (data === 'Updated') {
                    toastr.success('ClassSection updated successfully');
                    GetClassSectionList(1);
                    $('#ClassSection').click();
                    ClearData(1);
                }
                else if (data === 'Exists') {
                    toastr.error('ClassSection already exists!');
                    $("#ClassId").val('0').trigger('change');
                    $("#SectionId").val('0').trigger('change');
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
function GetSection() {
    var cls = {
    }
    $.ajax({
        url: '/Common/GetSection',
        contentType: "application/json; charset=utf-8",
        type: "GET",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {

            var html = "";
            html = html + ' <option value="0" selected>Select Section</option>';
            for (var i = 0; i < data.LSTSectionList.length; i++) {
                html = html + '<option value="' + data.LSTSectionList[i].Id + '">' + data.LSTSectionList[i].SectionNo + '</option>';
                $("#SectionId").empty();
                $("#SectionId").append(html);
            }
        }
    });
}

function onClass() {
    debugger
    var ClassId = $('#ClassId').val();
    ShowWait();
    $.ajax({
        type: "POST",
        url: '/Common/LoadSection?ClassId=' + ClassId,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var html = "";
            html = html + ' <option value="0" selected>Select Section</option>';
            for (var i = 0; i < data.LSTSectionList.length; i++) {
                html = html + '<option value="' + data.LSTSectionList[i].Id + '">' + data.LSTSectionList[i].SectionNo + '</option>';
                $("#SectionId").empty();
                $("#SectionId").append(html);
            }
        },
        failure: function () {
            alert("Failed!");
        }
    });
    HideWait();
}

function GetClassSectionList(page) {

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
        url: '/ClassSection/GetClassSection',
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

function GetSingleClassSectionData(id) {
    $('#errClassId').html("");
    $('#errSectionId').html("");
    document.getElementById('btnAdd').innerHTML = "Update";
    $("#btnAdd").attr('title', 'Update');
    document.getElementById('PopupTitle').innerHTML = "Update ClassSection";
    var cls = {
        Id: id
    }

    $.ajax({
        url: '/ClassSection/GetSingleClassSectionData',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {

            if (data !== null) {
                document.getElementById('hdnintId').value = data.LSTClassSectionList[0].Id;
                $('#ClassId').val(data.LSTClassSectionList[0].ClassId).trigger("change");
                $('#SectionId').val(data.LSTClassSectionList[0].SectionId).trigger("change");
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

function deleteClassSection() {
    var Id = document.getElementById('hdnintId').value;
    var cls = {
        Id: Id
    }
    ShowWait();
    $.ajax({
        url: '/ClassSection/deleteClassSection',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {

            if (data.Response === 'Success' ) {

                toastr.success('ClassSection deleted successfully');
                document.getElementById('hdnintId').value = "0";
                GetClassSectionList();
                $('#delete_ClassSection').click();
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
    $('#errClassId').html("");
    $('#errSectionId').html("");
    $("#ddlClassId").val('0').trigger('change');
    $("#ClassId").val('0').trigger('change');
    $("#SectionId").val('0').trigger('change');
    document.getElementById('btnAdd').innerHTML = "Add";
    $("#btnAdd").attr('title', 'Upload');
    document.getElementById('PopupTitle').innerHTML = "Add ClassSection";
}

function UpdateStatus() {
    var Id = document.getElementById('hdnintId').value;
    var cls = {
        Id: Id
    }
    ShowWait();
    $.ajax({
        url: '/ClassSection/UpdateStatus',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {
            if (data === 'success') {
                toastr.success('status updated successfully.');
                document.getElementById('hdnintId').value = "0";
                GetClassSectionList(1);
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
        $("#ClassId").val('0').trigger('change');
        $("#SectionId").val('0').trigger('change');
        $('#errClassId').html("");
        $('#errSectionId').html("");
        if (Id === "0") {

            document.getElementById('hdnintId').value = "0";
            document.getElementById('btnAdd').innerHTML = "Add";
            $("#btnAdd").attr('title', 'Add');
            document.getElementById('PopupTitle').innerHTML = "Add ClassSection";
        }
        else {
            document.getElementById('btnAdd').innerHTML = "Update";
            $("#btnAdd").attr('title', 'Update');
            document.getElementById('PopupTitle').innerHTML = "Update ClassSection";
        }
    }
    else {
        document.getElementById('SearchText').value = "";
        document.getElementById('intActive').value = '3';
        $("#ddlClassId").val('0').trigger('change');
        GetClassSectionList();
    }
}

function opendeleteModel(id) {

    document.getElementById('hdnintId').value = id;
}

function openstatusModel(id) {

    document.getElementById('hdnintId').value = id;
}

function ExportClassSection() {
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
        url: '/ClassSection/ExpotToExcelClassSectionReport',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {
            if (data === "success") {
                window.location.href = "/ClassSection/ExportToExcel";
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