
$(document).ready(function () {
    GetClassRoom();
    GetStudentList(1);
    GetStudentGrid();
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

    if (Id > 4) {
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

function GetStudentList(page) {

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
    });
}

function GetStudentGrid(page) {

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
        url: '/Student/GetStudentGrid',
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
    });
}

function ExportStudent() {
    var Id = 0;
    var SearchText = document.getElementById('SearchText').value;
    var intActive = document.getElementById('intActive').value;

    var cls = {
        Id: Id,
        intActive: intActive,
        SearchText: SearchText,
    };

    ShowWait();
    $.ajax({
        url: '/Student/ExpotToExcelStudentReport',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {
            if (data === "success") {
                window.location.href = "/Student/ExportToExcel";
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

function ValidateBasicDetails(id) {

    var Id = $('#hdnintId').val();
    var val = true;
    var Title = $('#Title').val();
    var StudentName = $('#StudentName').val();
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
    var RollNo = $('#RollNo').val();
    var RoleId = 2
    if (Title === "0") {
        $("#errTitle").html("Please select title");
        val = false;
    }

    if (StudentName === "" || /\S/.test(StudentName) === false) {
        $("#errStudentName").html("Please enter Student name.");
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
    if (Gender === "0") {
        $("#errGender").html("Please select gender.");
        val = false;
    }
    if (BloodGroup === "0") {
        $("#errBloodGroup").html("Please select bloodroup.");
        val = false;
    }
    if (ClassId === "0") {
        $("#errClassId").html("Please select class.");
        val = false;
    }
    if (RollNo === "" || RollNo.trim() === '') {
        $("#errRollNo").html("Please enter rollNo.");
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

function ValidateParentDetails(id) {

    var val = true;
    var ParentType = $('#ParentType').val();
    var ParentName = $('#ParentName').val();
    var ParentFatherName = $('#ParentFatherName').val();
    var ParentGender = $('#ParentGender').val();
    var ParentEmail = $('#ParentEmail').val();
    var ParentMobileNo = $('#ParentMobileNo').val();
    var AnniversaryDate = $('#AnniversaryDate').val();
    var Qualification = $('#Qualification').val();
    var Profession = $('#Profession').val();
  

    if (ParentType === "0") {
        $("#errParentType").html("Please enter parenttype.");
        val = false;
    }
    if (ParentName === "" || /\S/.test(ParentName) === false) {
        $("#errParentName").html("Please enter parentname.");
        val = false;
    }
    if (ParentFatherName === "" || /\S/.test(ParentFatherName) === false) {
        $("#errParentFatherName").html("Please enter parentfathername.");
        val = false;
    }
    if (ParentGender === "0") {
        $("#errParentGender").html("Please enter parentgender.");
        val = false;
    }
    if (ParentEmail === "" || /\S/.test(ParentEmail) === false) {
        $("#errParentEmail").html("Please enter parentemail.");
        val = false;
    }
    if (ParentMobileNo === "" || /\S/.test(ParentMobileNo) === false) {
        $("#errParentMobileNo").html("Please enter parentgender.");
        val = false;
    }
    if (AnniversaryDate === "" || /\S/.test(AnniversaryDate) === false) {
        $("#errAnniversaryDate").html("Please enter anniversarydate.");
        val = false;
    }
    if (Qualification === "" || /\S/.test(Qualification) === false) {
        $("#errQualification").html("Please enter qualification.");
        val = false;
    }
    if (Profession === "" || /\S/.test(Profession) === false) {
        $("#errProfession").html("Please enter profession.");
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
    var StudentName = $('#StudentName').val();
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
    var ParentType = $('#ParentType').val();
    var ParentName = $('#ParentName').val();
    var ParentFatherName = $('#ParentFatherName').val();
    var ParentGender = $('#ParentGender').val();
    var ParentEmail = $('#ParentEmail').val();
    var ParentMobileNo = $('#ParentMobileNo').val();
    var AnniversaryDate = $('#AnniversaryDate').val();
    var Qualification = $('#Qualification').val();
    var Profession = $('#Profession').val();
    var CurrentAddress = $('#CurrentAddress').val();
    var CurrentPincode = $('#CurrentPincode').val();
    var CurrentCity = $('#CurrentCity').val();
    var CurrentState = $('#CurrentState').val();
    var PermenantAddress = $('#PermenantAddress').val();
    var PermenantPincode = $('#PermenantPincode').val();
    var PermenantCity = $('#PermenantCity').val();
    var PermenantState = $('#PermenantState').val();
  
    var RollNo = $('#RollNo').val();

    var RoleId = 2
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
   
    if (id > 4) {

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
        if (id === 0) {
            var Profile = document.getElementById('Profile').value;
            if (Profile === null || Profile === "") {
                Profile = "~/Data/Profile/dummy.jpg";
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
    formData.append('StudentName', StudentName);
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
    formData.append('RollNo', RollNo);
    formData.append('Qualification', Qualification);
    formData.append('AnniversaryDate', AnniversaryDate);
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
    formData.append('ParentType', ParentType);
    formData.append('ParentName', ParentName);
    formData.append('ParentFatherName', ParentFatherName);
    formData.append('ParentEmail', ParentEmail);
    formData.append('ParentMobileNo', ParentMobileNo);
    formData.append('ParentGender', ParentGender);
    formData.append('AnniversaryDate', AnniversaryDate);
    formData.append('Qualification', Qualification);
    formData.append('Profession', Profession);
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
                    WelcomeMail(Email);
                    window.location.replace("/Student/StudentList");
                }
                else if (data === 'Updated') {
                    toastr.success('Student updated successfully');
                    window.location.replace("/Student/StudentList");
                }
                else if (data === 'Exists') {
                    toastr.error('Student already exists!');
                    document.getElementById('Email').value = "";
                }
            }
            HideWait();
        },
    });
}

function WelcomeMail(Email) {
    debugger
    var cls = {
        Email: Email
    }
    $.ajax({
        url: '/Student/WelcomeMail',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {

            if (data !== null) {
                document.getElementById('hdnintId').value = data.LSTStudentList[0].Id;
                document.getElementById('TeacherName').value = data.LSTStudentList[0].StudentName;
                document.getElementById('Email').value = data.LSTStudentList[0].Email;
                document.getElementById('Password').value = data.LSTStudentList[0].Password;
                document.getElementById('RoleName').value = data.LSTStudentList[0].RoleName;
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

function AddBulkStudentData() {
    var val = true;
    $('#tblMsg').empty();
    var formData = new FormData();
    var totalFiles = document.getElementById("File").files.length;
    if (totalFiles === 0) {
        $("#errFile").html("Please select file.");
        val = false;
    }

    for (var i = 0; i < totalFiles; i++) {
        var file = document.getElementById("File").files[i];
        formData.append("file", file);
    }
    if (val === false) {
        return;
    }
    ShowWait();
    $.ajax({

        url: '/Student/StudentBulkUpload',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        contentType: false,
        processData: false,
        data: formData,
        success: function (data) {
            debugger

            try {
                var strHTML = '';
                var j = 0;
                if (data !== null && data.length > 0 && data[0].ErrorMessage !== 'Student Uploaded Successfully') {

                    strHTML = strHTML + '<table class="table table-striped table-nowrap custom-table datatable">'
                    strHTML = strHTML + '<tr class="thead-dark">'
                    strHTML = strHTML + '<th class="thead-dark">Row No.</th>'
                    strHTML = strHTML + '<th class="thead-dark">Error Message.</th>'
                    strHTML = strHTML + '</tr>'
                    strHTML = strHTML + '<tbody>'
                    for (var i = 0; i < data.length; i++) {
                        j = j + 1;

                        strHTML = strHTML + '<tr>'
                        strHTML = strHTML + '<td>' + j + '</td>';
                        strHTML = strHTML + '<td style="color:red">' + data[i].ErrorMessage + '</td>';
                        strHTML = strHTML + '</tr>'

                    }
                    strHTML = strHTML + '</tbody>'
                    strHTML = strHTML + '</table>'
                    $('#tblMsg').empty();
                    $('#tblMsg').append(strHTML);

                }
                else {
                    if (data[0].ErrorMessage === 'Teacher Uploaded Successfully') {
                        toastr.success(data[0].ErrorMessage);
                        $('#BulkTeacher').click();
                        GetTeacherList();
                        WelcomeMail(data[0].TempEmail);
                        toastr.success('Mail Sent Successfully');

                        ClearData1(1);
                    }
                }
                HideWait();
            }

            catch (ex) {
                debugger
            }
            finally {
                debugger
            }

        },
        error: function (xhr) {

            alert('errors');
            GetStudentList(1);
            $('#BulkStudent').click();
            ClearData(1);
        }
    });
}

function PrintErrmsg(data) {
    $('#Error_msg').modal('show');
    $('#errormsg').text(data);
}

function ClearData1() {
    document.getElementById('File').value = "";
    $('#tblMsg').html("");
}

function Clear1() {
    document.getElementById('File').value = "";
    $('#tblMsg').html("");
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
                GetStudentGrid();
                $('#delete_Student').click();
            }
            //else if (data.Response === 'dependency') {
            //    alert('Student already used in system.');
            //    document.getElementById('hdnintId').value = "0";
            //    GetStudentList();
            //    $('#delete_Student').click();
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

function ClearBasicDetails() {
    
    $("#Title").val('0').trigger('change');
    $('#errTitle').html("");
    document.getElementById('StudentName').value = "";
    $('#errStudentName').html("");
    document.getElementById('FatherName').value = "";
    $('#errFatherName').html("");
    document.getElementById('Surname').value = "";
    $('#errSurname').html("");
    $("#Gender").val('0').trigger('change');
    $('#errGender').html("");
    $("#BloodGroup").val('0').trigger('change');
    $('#errBloodGroup').html("");
    document.getElementById('Dob').value = "";
    $('#errDob').html("");
    document.getElementById('Email').value = "";
    $('#errEmail').html("");
    document.getElementById('Password').value = "";
    $('#errPassword').html("");
    document.getElementById('MobileNo').value = "";
    $('#errMobile').html("");
    $('#AlternateMobileNo').val('');
    $("#ClassId").val('0').trigger('change');
    $('#errClassId').html("");
    document.getElementById('RollNo').value = "";
    $('#errRollNo').html("");
}

function ClearParentDetails() {
    $("#ParentType").val('0').trigger('change');
    $('#errParentType').html("");
    document.getElementById('ParentName').value = "";
    $('#errParentName').html("");
    document.getElementById('ParentFatherName').value = "";
    $('#errParentFatherName').html("");
    $("#ParentGender").val('0').trigger('change');
    $('#errParentGender').html("");
    document.getElementById('ParentEmail').value = "";
    $('#errParentEmail').html("");
    document.getElementById('ParentMobileNo').value = "";
    $('#errParentMobile').html("");
    document.getElementById('Qualification').value = "";
    $('#errQualification').html("");
    document.getElementById('AnniversaryDate').value = "";
    $('#errAnniversaryDate').html("");
    document.getElementById('Profession').value = "";
    $('#errProfession').html("");

}

function ClearAddressDetails() {
    document.getElementById('CurrentAddress').value = "";
    $('#errCurrentAddress').html("");
    document.getElementById('CurrentPincode').value = "";
    $('#errCurrentPincode').html("");
    document.getElementById('CurrentCity').value = "";
    $('#errCurrentCity').html("");
    document.getElementById('CurrentState').value = "";
    $('#errCurrentState').html("");
    document.getElementById('PermenantAddress').value = "";
    $('#errPermenantAddress').html("");
    document.getElementById('PermenantPincode').value = "";
    $('#errPermenantPincode').html("");
    document.getElementById('PermenantCity').value = "";
    $('#errPermenantCity').html("");
    document.getElementById('PermenantState').value = "";
    $('#errPermenantState').html("");
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
        url: '/Student/UpdateStatus',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {
            if (data === 'success') {
                toastr.success('status updated successfully.');
                document.getElementById('hdnintId').value = "0";
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