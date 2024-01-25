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
    GetStudentList(1);
    GetClassRoom();
});


function InsertData() {

    var val = true;
    var Id = $('#hdnintId').val();
    var SearchText = $('#SearchText').val();
    var FullName = $('#FullName').val();
    var UserName = $('#UserName').val();
    var RollNo = $('#RollNo').val();
    var ClassId = $('#ClassId').val();
    var Email = $('#Email').val();
    var Password = $('#Password').val();
    var MobileNo = $('#MobileNo').val();
    var Dob = $('#Dob').val();
    var Address = $('#Address').val();
    var RoleId = 3

    if (FullName === "" || /\S/.test(FullName) === false) {
        $("#errFullName").html("Please enter first name.");
        val = false;
    }
    else {
        var isValid = FullName.match(".*[a-zA-Z]+.*");
        if (isValid === null) {
            $("#errFullName").html("Please enter valid name.");
            val = false;
        }
        else {
            $("#errFullName").html("");
        }
    }
    if (UserName === "" || /\S/.test(UserName) === false) {
        $("#errUserName").html("Please enter user name.");
        val = false;
    }
    else {
        var isValid = FullName.match(".*[a-zA-Z]+.*");
        if (isValid === null) {
            $("#errUserName").html("Please enter valid name.");
            val = false;
        }
        else {
            $("#errFullName").html("");
        }
    }
    if (ClassId === 0) {
        $("#errClassroom").html("Please select classroom.");
        val = false;
    }
    if (Email === "" || /\S/.test(Email) === false) {
        $("#errEmail").html("Please enter email.");
        val = false;
    }
    if (Password === "" || /\S/.test(Password) === false) {
        $("#errPassword").html("Please enter password.");
        val = false;
    }
    if (MobileNo === "" || /\S/.test(MobileNo) === false) {
        $("#errMobile").html("Please enter mobile.");
        val = false;
    }
    if (RollNo === "" || /\S/.test(RollNo) === false) {
        $("#errRollNo").html("Please enter rollno.");
        val = false;
    }
    if (Dob === "" || /\S/.test(Dob) === false) {
        $("#errDob").html("Please select dob.");
        val = false;
    }
    var Gender = $("input[type='radio']:checked").val();
    if (Gender === "Active") {
        Gender = 1
    }
    else {
        Gender = 0
    }

    var formData = new FormData();
    var fileCount = document.getElementById("Profile").files.length;
    var hdnfile = document.getElementById("hdnfile").value;

    if (hdnfile === null || hdnfile === "") {
        var Profile = document.getElementById('Profile').value;
        if (Profile === null || Profile === "") {
            $("#errProfile").html('Please select image.');
            return;
        }
        if (fileCount > 0) {
            for (var i = 0; i < fileCount; i++) {
                var Profile = document.getElementById("Profile").files[i];
                var ext = Profile.name.split('.').pop();
                if (ext.toLowerCase() === "jpg" || ext.toLowerCase() === "jpeg" || ext.toLowerCase() === "png") {
                    formData.append("Profile", Profile);
                }
                else {
                    alert('Please upload valid file.');
                    return;
                }
            }
        }
    }
    else {

        var ProfileImg = hdnfile;

    }
    if (val === false) {
        return;
    }

    formData.append('Id', Id);
    formData.append('RoleId', RoleId);
    formData.append('FullName', FullName);
    formData.append('UserName', UserName);
    formData.append('Email', Email);
    formData.append('ClassId', ClassId);
    formData.append('RollNo', RollNo);
    formData.append('Password', Password);
    formData.append('MobileNo', MobileNo);
    formData.append('Dob', Dob);
    formData.append('Gender', Gender);
    formData.append('Address', Address);

    formData.append('SearchText', SearchText);
    formData.append('Profile', Profile);
    formData.append('ProfileImg', ProfileImg);
    ShowWait();
    $.ajax({
        type: "POST",
        url: '/Student/InsertStudent',
        contentType: "application/json; charset=utf-8",
        contentType: false,
        processData: false,
        data: formData,
        success: function (data) {
            if (data !== null) {
                if (data === 'Success') {
                    toastr.success('Student inserted successfully');
                    GetStudentList(1);
                    $('#Student').click();
                    ClearData(1);
                }
                else if (data === 'Updated') {
                    toastr.success('Student updated successfully');
                    GetStudentList(1);
                    $('#Student').click();
                    ClearData(1);
                }
                else if (data === 'Exists') {
                    toastr.error('Student already exists!');
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
function GetClassRoom() {

    var cls = {
    }
    $.ajax({
        url: '/Student/GetClassRoom',
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
                //$("#ddlClassId").empty();
                //$("#ddlClassId").append(html);
            }
        }
    });
}
function GetStudentList(page) {

    var Id = 0;
    var SearchText = document.getElementById('SearchText').value;

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
        PageSize: PageSize,
        PageIndex: PageIndex,
    }
    ShowWait();
    $.ajax({
        url: '/Student/GetStudent',
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

function GetSingleStudentData(id) {
    $('#errName').html("");
    document.getElementById('btnAdd').innerHTML = "Update";
    $("#btnAdd").attr('title', 'Update');
    document.getElementById('PopupTitle').innerHTML = "Update Student";
    var cls = {
        Id: id
    }

    $.ajax({
        url: '/Student/GetSingleStudentData',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {

            if (data !== null) {
                document.getElementById('hdnintId').value = data.LSTStudentList[0].Id;
                document.getElementById('FullName').value = data.LSTStudentList[0].FullName;
                document.getElementById('UserName').value = data.LSTStudentList[0].UserName;
                document.getElementById('Email').value = data.LSTStudentList[0].Email;
                document.getElementById('Password').value = data.LSTStudentList[0].Password;
                document.getElementById('MobileNo').value = data.LSTStudentList[0].MobileNo;
                document.getElementById('Address').value = data.LSTStudentList[0].Address;
                document.getElementById('RollNo').value = data.LSTStudentList[0].RollNo;
                $('#ClassId').val(data.LSTStudentList[0].ClassId).trigger("change");
                document.getElementById('Dob').value = data.LSTStudentList[0].Dob;
                //document.getElementById('Dob').value = ConvertRazorToDate(data.Dob);
                if (data.LSTStudentList[0].Gender === true) {
                    $("#Active").prop('checked', true);
                }
                else {
                    $("#InActive").prop('checked', true);
                }
                if (data.ProfileImg !== null && data.ProfileImg !== "") {
                    document.getElementById('hdnFile').value = data.ProfileImg;

                    document.getElementById('divUploadFile').style.display = "none";
                    document.getElementById('divFile').style.display = "block";

                    $('#divFile').empty();
                    var strHTML = "";
                    strHTML += '<label>File</label><br>';
                    strHTML += '<span><img src = "/Data/Profile/' + data.ProfileImg + '" alt = "attachment" title = "Download attachment" style = "width:80px;" ></span >';
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

function deleteStudent() {
    var Id = document.getElementById('hdnintId').value;
    var cls = {
        Id: Id
    }
    ShowWait();
    $.ajax({
        url: '/Student/deleteStudent',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {

            if (data.Response === 'Success') {
                //alert('Student deleted successfully.');
                toastr.success('Student deleted successfully');
                //swal(
                //    'Good job!',
                //    'Student deleted successfully',
                //    'success'
                //)
                //  toastr.success(data.Response, 'Student deleted successfully.', new {timeOut: 300 });
                document.getElementById('hdnintId').value = "0";
                GetStudentList();
                $('#delete_Student').click();
            }
            //else if (data.Response === 'dependency') {
            //    alert('Student already used in system.');
            //    document.getElementById('hdnintId').value = "0";
            //    GetStudentList();
            //    $('#delete_User').click();
            //}

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
function UpdateStatus() {

    var Id = document.getElementById('hdnintId').value;
    var cls = {
        Id: Id
    }
    ShowWait();
    $.ajax({
        url: '/Student/UpdateStatus',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {
            if (data === 'success') {
                toastr.success('status updated successfully.');
                GetStudentList(1);
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
function Clear() {
    document.getElementById('FullName').value = "";
    $('#errFullName').html("");
    document.getElementById('UserName').value = "";
    $('#errUserName').html("");
    document.getElementById('Email').value = "";
    $('#errEmail').html("");
    document.getElementById('MobileNo').value = "";
    $('#errMobile').html("");
    document.getElementById('RollNo').value = "";
    $('#errRollNo').html("");
    $("#ClassId").val('0').trigger('change');
    $('#errClassroom').html("");
    document.getElementById('Dob').value = "";
    $('#errDob').html("");
    document.getElementById('Password').value = "";
    $('#errPassword').html("");
    $('#Address').val('');
    $('#errProfile').html("");

    document.getElementById('btnAdd').innerHTML = "Add";
    $("#btnAdd").attr('title', 'Upload');
    document.getElementById('PopupTitle').innerHTML = "Add Student";
}

function ClearData(type) {

    if (type === 1) {

        var Id = document.getElementById('hdnintId').value;
        document.getElementById('FullName').value = "";
        $('#errFullName').html("");
        document.getElementById('UserName').value = "";
        $('#errUserName').html("");
        document.getElementById('Email').value = "";
        $('#errEmail').html("");
        document.getElementById('MobileNo').value = "";
        $('#errMobile').html("");
        document.getElementById('RollNo').value = "";
        $('#errRollNo').html("");
        $("#ClassId").val('0').trigger('change');
        $('#errClassroom').html("");
        document.getElementById('Dob').value = "";
        $('#errDob').html("");
        document.getElementById('Password').value = "";
        $('#errPassword').html("");
        $('#Address').val('');
        $('#errProfile').html("");

        if (Id === "0") {

            document.getElementById('hdnintId').value = "0";
            document.getElementById('btnAdd').innerHTML = "Add";
            $("#btnAdd").attr('title', 'Add');
            document.getElementById('PopupTitle').innerHTML = "Add Student";
        }
        else {
            document.getElementById('btnAdd').innerHTML = "Update";
            $("#btnAdd").attr('title', 'Update');
            document.getElementById('PopupTitle').innerHTML = "Update Student";
        }
    }
    else {
        document.getElementById('SearchText').value = "";
        GetStudentList();
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
