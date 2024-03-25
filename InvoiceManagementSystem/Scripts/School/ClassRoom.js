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
    GetClassRoomList(1);
    $('#FilterDiv').hide();
    $('#list-view').show();
    $('#grid-view').hide();

    // Event handler for list-view tab
    $('li[data-tab-id="list-view"]').on('click', function () {
        $('#list-view').show();
        $('#grid-view').hide();
    });

    // Event handler for grid-view tab
    $('li[data-tab-id="grid-view"]').on('click', function () {
        $('#list-view').hide();
        $('#grid-view').show();
    });

});

function GetClassRoomList(page) {
    var Id = 0;
    var SearchText = document.getElementById('SearchText').value;
    var intActive = document.getElementById('intActive').value;
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
        PageSize: PageSize,
        PageIndex: PageIndex,
    }
    ShowWait();
    $.ajax({
        url: '/ClassRoom/GetClassRoom',
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

function ExportClassRoom() {
    var Id = 0;
    var SearchText = document.getElementById('SearchText').value;
    var intActive = document.getElementById('intActive').value;
    
    var cls = {
        Id: Id,
        SearchText: SearchText,
        intActive: intActive,
    };

    ShowWait();
    $.ajax({
        url: '/ClassRoom/ExpotToExcelClassRoomReport',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {
            if (data === "success") {
                window.location.href = "/ClassRoom/ExportToExcel";
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


function InsertData() {

    var val = true;
    var Id = $('#hdnintId').val();
    var SearchText = $('#SearchText').val();
    var ClassNo = $('#ClassNo').val();
    if (ClassNo === "" || /\S/.test(ClassNo) === false) {
        $("#errClassNo").html("Please enter ClassNo.");
        val = false;
    }

    var formData = new FormData();
    if (val === false) {
        return;
    }

    formData.append('Id', Id);
    formData.append('SearchText', SearchText);
    formData.append('ClassNo', ClassNo);
    ShowWait();
    $.ajax({
        type: "POST",
        url: '/ClassRoom/InsertClassRoom',
        contentType: "application/json; charset=utf-8",
        contentType: false,
        processData: false,
        data: formData,
        success: function (data) {
            if (data !== null) {
                if (data === 'Success') {
                    toastr.success('ClassRoom inserted successfully');
                    GetClassRoomList(1);
                    $('#ClassRoom').click();
                    ClearData(1);
                }
                else if (data === 'Updated') {
                    toastr.success('ClassRoom updated successfully');
                    GetClassRoomList(1);
                    $('#ClassRoom').click();
                    ClearData(1);
                }
                else if (data === 'Exists') {
                    toastr.error('ClassRoom already exists!');
                    document.getElementById('ClassNo').value = "";
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

function GetSingleClassRoomData(id) {
    $('#errClassNo').html("");
    document.getElementById('btnAdd').innerHTML = "Update";
    $("#btnAdd").attr('title', 'Update');
    document.getElementById('PopupTitle').innerHTML = "Update ClassRoom";
    var cls = {
        Id: id
    }

    $.ajax({
        url: '/ClassRoom/GetSingleClassRoomData',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {

            if (data !== null) {
                document.getElementById('hdnintId').value = data.LSTClassRoomList[0].Id;
                document.getElementById('ClassNo').value = data.LSTClassRoomList[0].ClassNo;
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

function deleteClassRoom() {
    var Id = document.getElementById('hdnintId').value;
    var cls = {
        Id: Id
    }
    ShowWait();
    $.ajax({
        url: '/ClassRoom/deleteClassRoom',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {

            if (data.Response === 'Success' && Id > 0) {
                toastr.success('ClassRoom deleted successfully');
                document.getElementById('hdnintId').value = "0";
                GetClassRoomList(1);
                $('#delete_ClassRoom').click();
            }
            else if (data.Response == 'dependency') {
                $('#delete_ClassRoom').click();
                toastr.error('ClassRoom already used in subject.');
                document.getElementById('hdnintId').value = "0";
                GetClassRoomList(1);
                //$('#delete_ClassRoom').click();
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


function ExportToExcel() {
    var Id = 0;
    var SearchText = document.getElementById('SearchText').value;
    var intActive = document.getElementById('intActive').value;
    var cls = {
        Id: Id,
        SearchText: SearchText,
        intActive: intActive,
    };

    ShowWait();

    $.ajax({
        url: '/ClassRoom/ExportToExcel',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {
            if (data && data.downloadUrl) {
                // Create a hidden iframe to trigger the file download
                var iframe = document.createElement('iframe');
                iframe.style.display = 'none';
                iframe.src = data.downloadUrl;
                document.body.appendChild(iframe);
            } else {
                console.error('File download URL not found in the response.');
            }

            HideWait();
        },
        error: function (xhr) {
            HideWait();
            alert('Error exporting file: ' + xhr.responseText);
        }
    });
}


function Clear() {
    document.getElementById('ClassNo').value = "";
    $('#errClassNo').html("");
    document.getElementById('btnAdd').innerHTML = "Add";
    $("#btnAdd").attr('title', 'Upload');
    document.getElementById('PopupTitle').innerHTML = "Add ClassRoom";
}

function UpdateStatus() {
    var Id = document.getElementById('hdnintId').value;
    var cls = {
        Id: Id
    }
    ShowWait();
    $.ajax({
        url: '/ClassRoom/UpdateStatus',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {
            if (data === 'success') {
                toastr.success('status updated successfully.');
                document.getElementById('hdnintId').value = "0";
                GetClassRoomList(1);
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
        document.getElementById('ClassNo').value = "";
        $('#errClassNo').html("");

        if (Id === "0") {

            document.getElementById('hdnintId').value = "0";
            document.getElementById('btnAdd').innerHTML = "Add";
            $("#btnAdd").attr('title', 'Add');
            document.getElementById('PopupTitle').innerHTML = "Add ClassRoom";
        }
        else {
            document.getElementById('btnAdd').innerHTML = "Update";
            $("#btnAdd").attr('title', 'Update');
            document.getElementById('PopupTitle').innerHTML = "Update ClassRoom";
        }
    }
    else {
        document.getElementById('SearchText').value = "";
        document.getElementById('intActive').value = '3';
        GetClassRoomList();
    }
}

function opendeleteModel(id) {

    document.getElementById('hdnintId').value = id;
}

function openstatusModel(id) {

    document.getElementById('hdnintId').value = id;
}