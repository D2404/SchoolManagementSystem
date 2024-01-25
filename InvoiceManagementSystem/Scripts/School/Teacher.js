﻿
$(document).ready(function () {
    GetClassRoom();
    GetTeacherList(1);
    GetTeacherGrid();
    DisabledAnniversaryDate();
    $('#removeButton').hide();
    $('#FilterDiv').hide();
    $('#list-view').show();
    $('#grid-view').hide();
    $(".gambar").attr("src", "/Data/Profile/dummy.jpg");

    // Event handler for list-view tab
    $('li[data-tab-id="list-view"]').on('click', function () {
        $('#list-view').show();
        $('#grid-view').hide();
        $('#bank-view').hide();
    });

    // Event handler for grid-view tab
    $('li[data-tab-id="grid-view"]').on('click', function () {
        $('#list-view').hide();
        $('#bank-view').hide();
        $('#grid-view').show();
    });
    $('li[data-tab-id="bank-view"]').on('click', function () {
        $('#list-view').hide();
        $('#grid-view').hide();
        $('#bank-view').show();

    });
    var currentDate = new Date().toISOString().slice(0, 10);
    document.getElementById('DateOfJoining').value = currentDate;

    var Id = $('#hdnintId').val();

    if (Id > 1) {
        $('#Passworddiv').hide();
        $('#ClassRoomdiv').hide();
    }
    $("#sameAsAbove").on("change", function () {
        if ($(this).prop("checked")) {
            var CurrentAddress = $('#CurrentAddress').val();
            var CurrentPincode = $('#CurrentPincode').val();
            var CurrentCity = $('#CurrentCity').val();
            var CurrentState = $('#CurrentState').val();
            $("#PermenantAddress").val(CurrentAddress);
            $("#PermenantPincode").val(CurrentPincode);
            $("#PermenantCity").val(CurrentCity);
            $("#PermenantState").val(CurrentState);
        } else {
            $("#PermenantAddress").$('#PermenantAddress').val();
            $("#PermenantPincode").$('#PermenantPincode').val();
            $("#PermenantCity").$('#PermenantCity').val();
            $("#PermenantState").$('#PermenantState').val();
        }
    });
    $("#MaritalStatus").change(function () {
        DisabledAnniversaryDate();
    });

});
function openFileInput() {
    document.getElementById('Profile').click();
}



function displaySelectedImage(input) {
    var ProfileImg = document.getElementById('ProfileImg');

    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            ProfileImg.src = e.target.result;
            $('#removeButton').show(); // Show remove button when an image is selected
        };
        reader.readAsDataURL(input.files[0]);
    } else {
        ProfileImg.src = "~/Data/Profile/dummy.jpg";
    }
}
function removeImage() {
    $(".gambar").attr("src", "/Data/Profile/dummy.jpg");
    $('#removeButton').hide();
}
function DisabledAnniversaryDate() {
    if ($("#MaritalStatus").val() === "Single") {
        $("#AnniversaryDate").prop("disabled", true);
    } else {
        $("#AnniversaryDate").prop("disabled", false);
    }
}
function onlyNumbers(event) {
    var keyCode = event.which || event.keyCode;

    if ((keyCode >= 48 && keyCode <= 57) || keyCode === 8) {
        return true;
    } else {
        event.preventDefault();
        return false;
    }
}
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
function GetTeacherGrid(page) {

    var Id = 0;
    var SearchText = document.getElementById('SearchText').value;
    var FromDate = document.getElementById('FromDate').value;
    var ToDate = document.getElementById('ToDate').value;
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
        Date: FromDate,
        Date: ToDate,
        intActive: intActive,
        PageSize: PageSize,
        PageIndex: PageIndex,
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

function ValidateBasicDetails(id) {

    var Id = $('#hdnintId').val();
    var val = true;
    var Title = $('#Title').val();
    var TeacherName = $('#TeacherName').val();
    var FatherName = $('#FatherName').val();
    var Surname = $('#Surname').val();
    var Gender = $('#Gender').val();
    var BloodGroup = $('#BloodGroup').val();
    var Dob = $('#Dob').val();
    var Email = $('#Email').val();
    var Password = $('#Password').val();
    var MobileNo = $('#MobileNo').val();
    var AlternateMobileNo = $('#AlternateMobileNo').val();
    var DateOfJoining = $('#DateOfJoining').val();
    var ClassId = $('#ClassId').val();
    var Education = $('#Education').val();
    var MaritalStatus = $('#MaritalStatus').val();
    var AnniversaryDate = $('#AnniversaryDate').val();
    var Experience = $('#Experience').val();
    var RoleId = 2
    if (Title === 0) {
        $("#errTitle").html("Please select title");
        val = false;
    }

    if (TeacherName === "" || /\S/.test(TeacherName) === false) {
        $("#errTeacherName").html("Please enter teacher name.");
        val = false;
    }
    if (FatherName === "" || /\S/.test(FatherName) === false) {
        $("#errFatherName").html("Please enter father name.");
        val = false;
    }
    if (Surname === "" || /\S/.test(Surname) === false) {
        $("#errSurname").html("Please enter surname.");
        val = false;
    }
    if (Gender === 0) {
        $("#errGender").html("Please select gender.");
        val = false;
    }
    if (BloodGroup === 0) {
        $("#errBloodGroup").html("Please select bloodroup.");
        val = false;
    }
    if (Dob === "" || /\S/.test(Dob) === false) {
        $("#errDob").html("Please select dob.");
        val = false;
    }
    if (Email === "" || Email.trim() === '') {
        $("#errEmail").html("Please enter email.");
        val = false;
    }
    if (Password === "" || Password.trim() === '') {
        $("#errPassword").html("Please enter password.");
        val = false;
    }
    if (MobileNo === "" || MobileNo.trim() === '') {
        $("#errMobile").html("Please enter mobile.");
        val = false;
    }
    if (DateOfJoining === "" || /\S/.test(DateOfJoining) === false) {
        $("#errDateOfJoining").html("Please select DateOfJoining.");
        val = false;
    }
    if (MaritalStatus === 0) {
        $("#errMaritalStatus").html("Please select maritalstatus.");
        val = false;
    }
    if (Education === "" || Education.trim() === '') {
        $("#errEducation").html("Please enter education.");
        val = false;
    }

    if (Experience === "" || /\S/.test(Experience) === false) {
        $("#errExperience").html("Please select Experience.");
        val = false;
    }

    if (Id === 0) {
        if (ClassId === 0) {
            $("#errClassId").html("Please select classroom");
            val = false;
        }
        var formData = new FormData();
        var fileCount = document.getElementById("Profile").files.length;
        var hdnfile = document.getElementById("Profile").value;

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
    }
    if (val === false) {
        return;
    }
    if (id > 0) {
        $('#list-view').show();
        $('#grid-view').hide();
    }
    else {
        $('#list-view').hide();
        $('#grid-view').show();
    }
}
function ValidateAddressDetails(id) {

    var val = true;
    var CurrentAddress = $('#CurrentAddress').val();
    var CurrentPincode = $('#CurrentPincode').val();
    var CurrentCity = $('#CurrentCity').val();
    var CurrentState = $('#CurrentState').val();
    var PermenantAddress = $('#PermenantAddress').val();
    var PermenantPincode = $('#PermenantPincode').val();
    var PermenantCity = $('#PermenantCity').val();
    var PermenantState = $('#PermenantState').val();

    if (CurrentAddress === "" || /\S/.test(CurrentAddress) === false) {
        $("#errCurrentAddress").html("Please enter current address.");
        val = false;
    }
    if (CurrentPincode === "" || /\S/.test(CurrentPincode) === false) {
        $("#errCurrentPincode").html("Please enter current pincode.");
        val = false;
    }
    if (CurrentCity === "" || /\S/.test(CurrentCity) === false) {
        $("#errCurrentCity").html("Please enter current city.");
        val = false;
    }
    if (CurrentState === "" || /\S/.test(CurrentState) === false) {
        $("#errCurrentState").html("Please enter current state.");
        val = false;
    }
    if (PermenantAddress === "" || /\S/.test(PermenantAddress) === false) {
        $("#errPermenantAddress").html("Please enter permanent address.");
        val = false;
    }
    if (PermenantPincode === "" || /\S/.test(PermenantPincode) === false) {
        $("#errPermenantPincode").html("Please enter permanent pincode.");
        val = false;
    }
    if (PermenantCity === "" || /\S/.test(PermenantCity) === false) {
        $("#errPermenantCity").html("Please enter permanent city.");
        val = false;
    }
    if (PermenantState === "" || /\S/.test(PermenantState) === false) {
        $("#errPermenantState").html("Please enter permanent city.");
        val = false;
    }


    if (val === false) {
        return;
    }

    if (id > 0) {
        $('#grid-view').show();
        $('#bank-view').hide();
    }
    else {
        $('#grid-view').hide();
        $('#bank-view').show();
    }
}

function InsertData(id) {
    var val = true;
    var Id = id;
    var Title = $('#Title').val();
    var TeacherName = $('#TeacherName').val();
    var FatherName = $('#FatherName').val();
    var Surname = $('#Surname').val();
    var Gender = $('#Gender').val();
    var BloodGroup = $('#BloodGroup').val();
    var Dob = $('#Dob').val();
    var Email = $('#Email').val();
    var Password = $('#Password').val();
    var MobileNo = $('#MobileNo').val();
    var AlternateMobileNo = $('#AlternateMobileNo').val();
    var DateOfJoining = $('#DateOfJoining').val();
    var Education = $('#Education').val();
    var MaritalStatus = $('#MaritalStatus').val();
    var AnniversaryDate = $('#AnniversaryDate').val();
    var Experience = $('#Experience').val();
    
    var CurrentAddress = $('#CurrentAddress').val();
    var CurrentPincode = $('#CurrentPincode').val();
    var CurrentCity = $('#CurrentCity').val();
    var CurrentState = $('#CurrentState').val();
    var PermenantAddress = $('#CurrentAddress').val();
    var PermenantPincode = $('#CurrentPincode').val();
    var PermenantCity = $('#CurrentCity').val();
    var PermenantState = $('#CurrentState').val();
    var BankName = $('#BankName').val();
    var BankBranch = $('#BankBranch').val();
    var AccountNo = $('#AccountNo').val();
    var IFSCCode = $('#IFSCCode').val();
    
    var RoleId = 2

    if (BankName === "" || /\S/.test(BankName) === false) {
        $("#errBankName").html("Please enter bankname.");
        val = false;
    }
    if (BankBranch === "" || /\S/.test(BankBranch) === false) {
        $("#errBankBranch").html("Please enter bankbranch.");
        val = false;
    }
    if (AccountNo === "" || /\S/.test(AccountNo) === false) {
        $("#errAccountNo").html("Please enter accountno.");
        val = false;
    }
    if (IFSCCode === "" || /\S/.test(IFSCCode) === false) {
        $("#errIFSCCode").html("Please enter ifsccode.");
        val = false;
    }
    if (id > 0) {

        var hdnClassId = $('#hdnClassId').val();
        var ClassId = hdnClassId;
    }
    else {
        var ClassId = $('#ClassId').val();
    }
    var formData = new FormData();

    var fileCount = document.getElementById("Profile").files.length;
    var hdnfile = $('#HiddenfileForImage').val();

    if (hdnfile === null || hdnfile === "" || hdnfile === undefined) {
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
    formData.append('Title', Title);
    formData.append('TeacherName', TeacherName);
    formData.append('FatherName', FatherName);
    formData.append('Surname', Surname);
    formData.append('Gender', Gender);
    formData.append('BloodGroup', BloodGroup);
    formData.append('Dob', Dob);
    formData.append('Email', Email);
    formData.append('Password', Password);
    formData.append('MobileNo', MobileNo);
    formData.append('AlternateMobileNo', AlternateMobileNo);
    formData.append('DateOfJoining', DateOfJoining);
    formData.append('ClassId', ClassId);
    formData.append('Education', Education);
    formData.append('MaritalStatus', MaritalStatus);
    formData.append('AnniversaryDate', AnniversaryDate);
    formData.append('Experience', Experience);
    formData.append('Profile', Profile);
    formData.append('ProfileImg', ProfileImg);
    formData.append('CurrentAddress', CurrentAddress);
    formData.append('CurrentPincode', CurrentPincode);
    formData.append('CurrentCity', CurrentCity);
    formData.append('CurrentState', CurrentState);
    formData.append('PermenantAddress', PermenantAddress);
    formData.append('PermenantPincode', PermenantPincode);
    formData.append('PermenantCity', PermenantCity);
    formData.append('PermenantState', PermenantState);
    formData.append('BankName', BankName);
    formData.append('BankBranch', BankBranch);
    formData.append('AccountNo', AccountNo);
    formData.append('IFSCCode', IFSCCode);
    ShowWait();
    $.ajax({
        type: "POST",
        url: '/Teacher/InsertTeacher',
        contentType: "application/json; charset=utf-8",
        contentType: false,
        processData: false,
        data: formData,
        success: function (data) {
            if (data !== null) {
                if (data === 'Success' ) {
                    toastr.success('Teacher inserted successfully');
                    window.location.replace("/Teacher/TeacherList");

                }
                else if (data === 'Updated' ) {
                    toastr.success('Teacher updated successfully');
                    window.location.replace("/Teacher/TeacherList");
                }
                else if (data === 'Exists') {
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

            if (data.Response === 'Success' ) {
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
                GetTeacherGrid();
                $('#delete_Teacher').click();
            }
            //else if (data.Response === 'dependency') {
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
        document.getElementById('Password').value = "";
        $('#errPassword').html("");
        $('#Address').val('');
        document.getElementById('Education').value = "";
        $('#errEducation').html("");
        document.getElementById('Dob').value = "";
        $('#errDob').html("");

        $("#ClassId").val('0').trigger('change');
        $('#errClassId').html("");
        if (Id === "0") {

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
            if (data === 'success') {
                toastr.success('status updated successfully.');
                document.getElementById('hdnintId').value = "0";
                GetTeacherList(1);
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
