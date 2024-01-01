
$(document).ready(function () {
    GetClassRoom();
    GetTeacherList(1);
    GetTeacherGrid();
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
    var Id = $('#hdnId').val();

    if (Id > 0) {
        $('#Passworddiv').hide();
    }
    
});



function GetClassRoom() {
    var cls = {
    }
    $.ajax({
        url: '/Subject/GetClassRoom',
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

function GetTeacherList(page) {

    var Id = 0;
    var SearchText = document.getElementById('SearchText').value;
    var FromDate = document.getElementById('FromDate').value;
    var ToDate = document.getElementById('ToDate').value;
    var intActive = document.getElementById('intActive').value;
    if (document.getElementById('PageSize') != null) {
        PageSize = document.getElementById('PageSize').value;
    }
    else {
        PageSize = 10;
    }
    if (page == undefined) {
        page = 1;
    }
    var PageIndex = page;

    PageIndex = page;
    var cls = {
        Id: Id,
        SearchText: SearchText,
        Date: FromDate,
        Date: ToDate,
        intActive: intActive,
        PageSize: PageSize,
        PageIndex: PageIndex,
    }
    ShowWait();
    $.ajax({
        url: '/Teacher/GetTeacher',
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
function GetTeacherGrid() {

    var Id = 0;
    var SearchText = document.getElementById('SearchText').value;
    var FromDate = document.getElementById('FromDate').value;
    var ToDate = document.getElementById('ToDate').value;
    var intActive = document.getElementById('intActive').value;
    var cls = {
        Id: Id,
        SearchText: SearchText,
        Date: FromDate,
        Date: ToDate,
        intActive: intActive,
    }
    ShowWait();
    $.ajax({
        url: '/Teacher/GetTeacherGrid',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {
            $('#gridView').empty();
            $('#gridView').append(data);
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
    var Id = $('#hdnId').val();
    var SearchText = $('#SearchText').val();
    var FullName = $('#FullName').val();
    var UserName = $('#UserName').val();
    var Email = $('#Email').val();
    var Password = $('#Password').val();
    var MobileNo = $('#MobileNo').val();
    var ClassId = $('#ClassId').val();
    var Dob = $('#Dob').val();
    var Address = $('#Address').val();
    var Education = $('#Education').val();
    /*var Salary = $('#Salary').val();*/
    var RoleId = 2
    if (FullName == "" || /\S/.test(FullName) == false) {
        $("#errFullName").html("Please enter first name.");
        val = false;
    }
    
    if (UserName == "" || /\S/.test(UserName) == false) {
        $("#errUserName").html("Please enter last name.");
        val = false;
    }
    
    if (ClassId == 0) {
        $("#errClassId").html("Please select classroom");
        val = false;
    }

    if (MobileNo == "" || MobileNo.trim() == '') {
        $("#errMobile").html("Please enter mobile.");
        val = false;
    }
    if (Password == "" || Password.trim() == '') {
        $("#errPassword").html("Please enter password.");
        val = false;
    }
    if (Education == "" || Education.trim() == '') {
        $("#errEducation").html("Please enter education.");
        val = false;
    }
   
    var Gender = $("input[type='radio']:checked").val();
    if (Gender == "Active") {
        Gender = 1
    }
    else {
        Gender = 0
    }
    if (Email == '' || Email.trim() == '') {
        $("#errEmail").html('Please enter email id.');
        return;
    }
   
    var formData = new FormData();
    var fileCount = document.getElementById("Profile").files.length;
    var hdnfile = document.getElementById("hdnfile").value;

    var formData = new FormData();
    var fileCount = document.getElementById("Profile").files.length;
    var hdnfile = document.getElementById("hdnfile").value;

    var formData = new FormData();
    var fileCount = document.getElementById("Profile").files.length;
    var hdnfile = document.getElementById("hdnfile").value;

    if (hdnfile == null || hdnfile == "") {
        var Profile = document.getElementById('Profile').value;
        if (Profile == null || Profile == "") {
            $("#errProfile").html('Please select image.');
            return;
        }
        if (fileCount > 0) {
            for (var i = 0; i < fileCount; i++) {
                var Profile = document.getElementById("Profile").files[i];
                var ext = Profile.name.split('.').pop();
                if (ext.toLowerCase() == "jpg" || ext.toLowerCase() == "jpeg" || ext.toLowerCase() == "png") {
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
    if (val == false) {
        return;
    }

    formData.append('Id', Id);
    formData.append('RoleId', RoleId);
    formData.append('FullName', FullName);
    formData.append('UserName', UserName);
    formData.append('ClassId', ClassId);
    formData.append('Email', Email);
    formData.append('Password', Password);
    formData.append('MobileNo', MobileNo);
    formData.append('Dob', Dob);
    formData.append('Gender', Gender);
    formData.append('Address', Address);
    formData.append('Education', Education);
    /*formData.append('Salary', Salary);*/
    formData.append('SearchText', SearchText);
    formData.append('Profile', Profile);
    formData.append('ProfileImg', ProfileImg);
    ShowWait();
    $.ajax({
        type: "POST",
        url: '/Teacher/Teacher',
        contentType: "application/json; charset=utf-8",
        contentType: false,
        processData: false,
        data: formData,
        success: function (data) {
            if (data != null) {
                if (data == 'Success' && Id == 0) {
                    toastr.success('Teacher inserted successfully');
                    window.location.replace("/Teacher/TeacherList");
                    
                }
                else if (data == 'Updated' && Id > 0) {
                    toastr.success('Teacher updated successfully');
                    window.location.replace("/Teacher/TeacherList");
                }
                else if (data == 'Exists') {
                    toastr.error('Teacher already exists!');
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

function deleteTeacher() {
    var Id = document.getElementById('hdnintId').value;
    var cls = {
        Id: Id
    }
    ShowWait();
    $.ajax({
        url: '/Teacher/deleteTeacher',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {

            if (data.Response == 'Success' && Id > 0) {
                //alert('Teacher deleted successfully.');
                toastr.success('Teacher deleted successfully');
                //swal(
                //    'Good job!',
                //    'Teacher deleted successfully',
                //    'success'
                //)
                //  toastr.success(data.Response, 'Teacher deleted successfully.', new {timeOut: 300 });
                document.getElementById('hdnintId').value = "0";
                GetTeacherList();
                $('#delete_Teacher').click();
            }
            //else if (data.Response == 'dependency') {
            //    alert('Teacher already used in system.');
            //    document.getElementById('hdnintId').value = "0";
            //    GetTeacherList();
            //    $('#delete_Teacher').click();
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

function Clear() {
    document.getElementById('hdnintId').value = 0;
    document.getElementById('FullName').value = "";
    $('#errFullName').html("");
    document.getElementById('UserName').value = "";
    $('#errUserName').html("");
    document.getElementById('Email').value = "";
    $('#errEmail').html("");
    document.getElementById('MobileNo').value = "";
    $('#errMobile').html("");
    document.getElementById('Password').value = "";
    $('#errPassword').html("");
    document.getElementById('Password').value = "";
    $('#errPassword').html("");
    $('#Address').val('');
    document.getElementById('Education').value = "";
    $('#errEducation').html("");
    //document.getElementById('Salary').value = "";
    //$('#errSalary').html("");
    $("#ClassId").val('0').trigger('change');
    $('#errClassId').html("");
    document.getElementById('btnAdd').innerHTML = "Add";
    $("#btnAdd").attr('title', 'Upload');
    document.getElementById('PopupTitle').innerHTML = "Add Teacher";
}

function ClearData(type) {

    if (type == 1) {
        var Id = document.getElementById('hdnintId').value;
        document.getElementById('FullName').value = "";
        $('#errFullName').html("");
        document.getElementById('UserName').value = "";
        $('#errUserName').html("");
        document.getElementById('Email').value = "";
        $('#errEmail').html("");
        document.getElementById('MobileNo').value = "";
        $('#errMobile').html("");
        document.getElementById('Password').value = "";
        $('#errPassword').html("");
        $('#Address').val('');
        document.getElementById('Education').value = "";
        $('#errEducation').html("");
        document.getElementById('Dob').value = "";
        $('#errDob').html("");

        $("#ClassId").val('0').trigger('change');
        $('#errClassId').html("");
        if (Id == "0") {

            document.getElementById('hdnintId').value = "0";
            document.getElementById('btnAdd').innerHTML = "Add";
            $("#btnAdd").attr('title', 'Add');
            document.getElementById('PopupTitle').innerHTML = "Add Teacher";
        }
        else {
            document.getElementById('btnAdd').innerHTML = "Update";
            $("#btnAdd").attr('title', 'Update');
            document.getElementById('PopupTitle').innerHTML = "Update Teacher";
        }
    }
    else {
        document.getElementById('SearchText').value = "";
        document.getElementById('intActive').value = '3';
        GetTeacherList();
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
function UpdateStatus() {
    var Id = document.getElementById('hdnintId').value;
    var cls = {
        Id: Id
    }
    ShowWait();
    $.ajax({
        url: '/Teacher/UpdateStatus',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {
            if (data == 'success') {
                toastr.success('status updated successfully.');
                document.getElementById('hdnintId').value = "0";
                GetTeacherList(1);
                $('#status').click();
            }
            else if (data == 'Exist') {
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
