
$(document).ready(function () {
    $('#FilterDiv').hide();
    GetClassRoom();
    GetTeacherList(1);
});
function myFunction() {
    document.getElementById("myDropdown").classList.toggle("show");
}
function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}


$(function () {
    $("#Dob").datetimepicker({
        format: 'DD/MM/YYYY',
        maxDate: new Date,
        useCurrent: true,
        ignoreReadonly: true
    })
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
function InsertData() {
    debugger
    var val = true;
    var Id = $('#hdnintId').val();
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
    var Salary = $('#Salary').val();
    var RoleId = 2
    if (FullName == "" || /\S/.test(FullName) == false) {
        $("#errFullName").html("Please enter first name.");
        val = false;
    }
    else {
        var isValid = FullName.match(".*[a-zA-Z]+.*");
        if (isValid == null) {
            $("#errFullName").html("Please enter valid name.");
            val = false;
        }
        else {
            $("#errFullName").html("");
        }
    }
    if (UserName == "" || /\S/.test(UserName) == false) {
        $("#errUserName").html("Please enter last name.");
        val = false;
    }
    else {
        var isValid = FullName.match(".*[a-zA-Z]+.*");
        if (isValid == null) {
            $("#errUserName").html("Please enter valid name.");
            val = false;
        }
        else {
            $("#errFullName").html("");
        }
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
    if (Salary == "" || Salary.trim() == '') {
        $("#errSalary").html("Please enter salary.");
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
    //var atpos = Email.indexOf("@@");
    //var dotpos = Email.lastIndexOf(".");
    //if (atpos < 1 || dotpos < atpos + 2 || dotpos + 2 >= Email.length) {
    //    $("#errEmail").html("Please enter valid email id.");
    //    return false;
    //}
    var formData = new FormData();
    var fileCount = document.getElementById("Profile").files.length;
    var hdnfile = document.getElementById("hdnfile").value;

    if (hdnfile == null || hdnfile == "") {
        var Profile = document.getElementById('Profile').value;
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

        var ProfileImg = Profile;

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
    formData.append('Salary', Salary);
    formData.append('SearchText', SearchText);
    formData.append('Profile', Profile);
    formData.append('ProfileImg', ProfileImg);
    ShowWait();
    $.ajax({
        type: "POST",
        url: '/Teacher/InsertTeacher',
        contentType: "application/json; charset=utf-8",
        contentType: false,
        processData: false,
        data: formData,
        success: function (data) {
            if (data != null) {
                if (data == 'Success' && Id == 0) {
                    toastr.success('Teacher inserted successfully');
                    GetTeacherList(1);
                    
                }
                else if (data == 'Updated' && Id > 0) {
                    toastr.success('Teacher updated successfully');
                    GetTeacherList(1);
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
function GetSingleTeacherData(id) {
    $('#Passworddiv').hide();

    $('#Profilediv').hide();
    $('#errName').html("");
    document.getElementById('btnAdd').innerHTML = "Update";
    $("#btnAdd").attr('title', 'Update');
    document.getElementById('PopupTitle').innerHTML = "Update Teacher";
    var cls = {
        Id: id
    }

    $.ajax({
        url: '/Teacher/GetSingleTeacherData',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {
            debugger
            if (data != null) {
                document.getElementById('hdnintId').value = data.LSTTeacherList[0].Id;
                document.getElementById('FullName').value = data.LSTTeacherList[0].FullName;
                document.getElementById('UserName').value = data.LSTTeacherList[0].UserName;
                document.getElementById('Email').value = data.LSTTeacherList[0].Email;
                if (data.LSTTeacherList[0].Password != 0) {
                    document.getElementById('Password').value = data.LSTTeacherList[0].Password;
                }
                else {
                    document.getElementById('Password').style.display = "none";
                }
                document.getElementById('MobileNo').value = data.LSTTeacherList[0].MobileNo;
                document.getElementById('Address').value = data.LSTTeacherList[0].Address;
                document.getElementById('Education').value = data.LSTTeacherList[0].Education;
                document.getElementById('Salary').value = data.LSTTeacherList[0].Salary;
                document.getElementById('Dob').value = data.LSTTeacherList[0].Dob;
                $('#ClassId').val(data.LSTTeacherList[0].ClassId).trigger("change");
                //document.getElementById('Dob').value = ConvertRazorToDate(data.Dob);
                if (data.LSTTeacherList[0].Gender == false) {
                    $("#Active").prop('checked', true);
                }
                else {
                    $("#InActive").prop('checked', true);
                }
                if (data.ProfileImg != null && data.ProfileImg != "") {
                    document.getElementById('hdnfile').value = data.ProfileImg;

                    document.getElementById('divUploadFile').style.display = "none";
                    document.getElementById('divFile').style.display = "block";

                    $('#divFile').empty();
                    var strHTML = "";
                    strHTML += '<label>File</label><br>';
                    strHTML += '<span><img src="/Data/Profile/' + data.ProfileImg + '" alt="attachment" title="Download attachment" style="width:80px;" ></span >';
                    strHTML += '&nbsp;<span title="remove" style="cursor: pointer;font-size: 15px;color: red;" onclick="RemoveFile()">×</span>';
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
    document.getElementById('Salary').value = "";
    $('#errSalary').html("");
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
        document.getElementById('Salary').value = "";
        $('#errSalary').html("");
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
