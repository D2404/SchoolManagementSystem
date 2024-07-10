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
    GetSchoolList(1);
});

function GetSchoolList(page) {
    
    var Id = 0;
    var SearchText = document.getElementById('SearchText').value;
   /* var intActive = document.getElementById('intActive').value;*/
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
      /*  intActive: intActive,*/
        PageSize: PageSize,
        PageIndex: PageIndex,
    }
    ShowWait();
    $.ajax({
        url: '/School/GetSchool',
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

function InsertData() {

    var val = true;
    var Id = $('#hdnintId').val();
    var SearchText = $('#SearchText').val();
    var SchoolName = $('#SchoolName').val();
    if (SchoolName === "" || /\S/.test(SchoolName) === false) {
        $("#errSchoolName").html("Please enter school name.");
        val = false;
    }
    var Email = $('#Email').val();
    if (Email === "" || /\S/.test(Email) === false) {
        $("#errEmail").html("Please enter Email.");
        val = false;
    }
    var MobileNo = $('#MobileNo').val();
    if (MobileNo === "" || /\S/.test(MobileNo) === false) {
        $("#errMobileNo").html("Please enter MobileNo.");
        val = false;
    }
    var Since = $('#Since').val();
    if (Since === "" || /\S/.test(Since) === false) {
        $("#errSince").html("Please enter Since.");
        val = false;
    }
    var Address = $('#Address').val();
    if (Address === "" || /\S/.test(Address) === false) {
        $("#errAddress").html("Please enter Address.");
        val = false;
    }

    var formData = new FormData();
    var fileCount = document.getElementById("Photo").files.length;
    var hdnfile = $('#HiddenfileForImage').val();

    if (hdnfile === null || hdnfile === "" || hdnfile === undefined) {
        var Photo = document.getElementById('Photo').value;
        if (Photo === null || Photo === "") {
            $("#errPhoto").html('Please select image.');
            return;
        }
        if (fileCount > 0) {
            for (var i = 0; i < fileCount; i++) {
                var Photo = document.getElementById("Photo").files[i];
                var ext = Photo.name.split('.').pop();
                if (ext.toLowerCase() === "jpg" || ext.toLowerCase() === "jpeg" || ext.toLowerCase() === "png") {
                    formData.append("Photo", Photo);
                }
                else {
                    alert('Please upload valid file.');
                    return;
                }
            }
        }
    }
    else {

        var PhotoImg = hdnfile;

    }
    if (val === false) {
        return;
    }

    formData.append('Id', Id);
    formData.append('SearchText', SearchText);
    formData.append('SchoolName', SchoolName);
    formData.append('Email', Email);
    formData.append('MobileNo', MobileNo);
    formData.append('Since', Since);
    formData.append('Address', Address);
    formData.append('Photo', Photo);
    formData.append('PhotoImg', PhotoImg);
    ShowWait();
    $.ajax({
        type: "POST",
        url: '/School/InsertSchool',
        contentType: "application/json; charset=utf-8",
        contentType: false,
        processData: false,
        data: formData,
        success: function (data) {
            if (data !== null) {
                if (data === 'Success') {
                    toastr.success('School inserted successfully');
                    GetSchoolList(1);
                    $('#School').click();
                    ClearData(1);
                }
                else if (data === 'Updated') {
                    toastr.success('School updated successfully');
                    GetSchoolList(1);
                    $('#School').click();
                    ClearData(1);
                }
                else if (data === 'Exists') {
                    toastr.error('School already exists!');
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

function GetSingleSchoolData(id) {
    $('#errSchoolName').html("");
    $('#errEmail').html("");
    $('#errMobileNo').html("");
    $('#errSince').html("");
    $('#errAddress').html("");
    document.getElementById('btnAdd').innerHTML = "Update";
    $("#btnAdd").attr('title', 'Update');
    document.getElementById('PopupTitle').innerHTML = "Update School";
    var cls = {
        Id: id
    }

    $.ajax({
        url: '/School/GetSingleSchoolData',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {

            if (data !== null) {
                document.getElementById('hdnintId').value = data.LSTSchoolList[0].Id;
                document.getElementById('SchoolName').value = data.LSTSchoolList[0].SchoolName;
                document.getElementById('Email').value = data.LSTSchoolList[0].Email;
                document.getElementById('MobileNo').value = data.LSTSchoolList[0].MobileNo;
                document.getElementById('Address').value = data.LSTSchoolList[0].Address;
                document.getElementById('Since').value = data.LSTSchoolList[0].Since;
                if (data.PhotoImg !== null && data.PhotoImg !== "") {
                    document.getElementById('hdnFile').value = data.PhotoImg;

                    document.getElementById('divUploadFile').style.display = "none";
                    document.getElementById('divFile').style.display = "block";

                    $('#divFile').empty();
                    var strHTML = "";
                    strHTML += '<label>File</label><br>';
                    strHTML += '<span><img src = "/Data/SchoolPhoto/' + data.PhotoImg + '" alt = "attachment" title = "Download attachment" style = "width:80px;" ></span >';
                    strHTML += '&nbsp;<span title="remove"  style="cursor: pointer;font-size: 15px;color: red;" onclick="RemoveFile()">×</span>';
                    $('#divFile').append(strHTML);
                }
                else {
                    document.getElementById('divUploadFile').style.display = "block";
                    document.getElementById('divFile').style.display = "none";
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

function deleteSchool() {
    var Id = document.getElementById('hdnintId').value;
    var cls = {
        Id: Id
    }
    ShowWait();
    $.ajax({
        url: '/School/deleteSchool',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {

            if (data.Response === 'Success') {

                toastr.success('School deleted successfully');
                document.getElementById('hdnintId').value = "0";
                GetSchoolList();
                $('#delete_School').click();
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
    document.getElementById('SchoolName').value = "";
    document.getElementById('Email').value = "";
    document.getElementById('MobileNo').value = "";
    document.getElementById('Since').value = "";
    document.getElementById('Address').value = "";
    $('#errSchoolName').html("");
    $('#errEmail').html("");
    $('#errMobileNo').html("");
    $('#errSince').html("");
    $('#errAddress').html("");
    document.getElementById('btnAdd').innerHTML = "Add";
    $("#btnAdd").attr('title', 'Upload');
    document.getElementById('PopupTitle').innerHTML = "Add School";
}

function UpdateStatus() {
    var Id = document.getElementById('hdnintId').value;
    var cls = {
        Id: Id
    }
    ShowWait();
    $.ajax({
        url: '/School/UpdateStatus',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {
            if (data === 'success') {
                toastr.success('status updated successfully.');
                document.getElementById('hdnintId').value = "0";
                GetSchoolList(1);
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
        document.getElementById('SchoolName').value = "";
        document.getElementById('Email').value = "";
        document.getElementById('MobileNo').value = "";
        document.getElementById('Since').value = "";
        document.getElementById('Address').value = "";
        $('#errSchoolName').html("");
        $('#errEmail').html("");
        $('#errMobileNo').html("");
        $('#errSince').html("");
        $('#errAddress').html("");

        if (Id === "0") {

            document.getElementById('hdnintId').value = "0";
            document.getElementById('btnAdd').innerHTML = "Add";
            $("#btnAdd").attr('title', 'Add');
            document.getElementById('PopupTitle').innerHTML = "Add School";
        }
        else {
            document.getElementById('btnAdd').innerHTML = "Update";
            $("#btnAdd").attr('title', 'Update');
            document.getElementById('PopupTitle').innerHTML = "Update School";
        }
    }
    else {
        document.getElementById('SearchText').value = "";
        /*document.getElementById('intActive').value = '3';*/
        GetSchoolList();
    }
}

function opendeleteModel(id) {

    document.getElementById('hdnintId').value = id;
}

function openstatusModel(id) {

    document.getElementById('hdnintId').value = id;
}