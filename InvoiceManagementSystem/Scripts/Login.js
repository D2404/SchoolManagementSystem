function ShowWait() {
    $('#divloader').show();
}
function HideWait() {
    $('#divloader').hide();
}

function SignUp() {

    var val = true;
    var UserName = $('#UserName').val();
    if (UserName === "" || /\S/.test(UserName) === false) {
        $("#errName").html("Please enter username.");
        val = false;
    }

    var Email = $('#Email1').val();
    if (Email === '' || Email.trim() === '') {
        $("#errEmail").html('Please enter email id.');
        val = false;
    }

    //var atpos = Email.indexOf("@@");
    //var dotpos = Email.lastIndexOf(".");
    //if (atpos < 1 || dotpos  < atpos + 2 || dotpos + 2 >= Email.length) {
    //    $("#errEmail").html("Please enter valid email id.");
    //    val = false;
    //}
    var Password = $('#Password1').val();
    if (Password === "" || /\S/.test(Password) === false) {
        $("#errPassword").html("Please enter password.");
        val = false;
    }
    if (val === false) {
        return;
    }
    var cls = {
        UserName: UserName,
        Email: Email,
        Password: Password,
    }
    ShowWait();
    $.ajax({
        type: "POST",
        url: '/Account/SignUp',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {
            if (data !== null) {
                if (data === 'Success') {
                    toastr.success('User registered inserted successfully');
                    $('.login').click();
                    //window.location.href = '/Home/Index';
                }
                else if (data === 'Exists') {
                    toastr.error('Email already exists!');
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

function Login() {
    
    var val = true;
    var UserName = document.getElementById('UserName').value;
    var Password = document.getElementById('Password').value;
    if (UserName === "" || /\S/.test(UserName) === false) {
        $("#errUserName").html("Please Enter Username.");
        val = false;
    }
    if (Password === "" || /\S/.test(Password) === false) {
        $("#errPassword").html("Please Enter Password.");
        val = false;
    }
    if (val === false) {
        return;
    }

    var cls = {
        UserName: UserName,
        Password: Password
    }
    $.ajax({
        url: '/Account/Login',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {

            if (data === 'Success') {
                window.location.href = '/Home/Index';
                toastr.success('Logged in successfully.');
            }
            else if (data === 'Error' || data === null) {
                toastr.error('Invalid Username or Password.');
            }
            document.getElementById('Email').value = '';
            document.getElementById('Password').value = '';
        },
        error: function (xhr) {

            alert('errors');
        }
    });
}
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
function CheckPassword() {
    var val = true;
    var Password = document.getElementById('oldPassword').value;
   
    if (val === false) {
        return;
    }

    var cls = {
        Password: oldPassword
    }
    $.ajax({
        url: '/Account/CheckPassword',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {

            if (data === 'Success') {
                window.location.href = '/Home/Index';

            }
            //else if (data.intId === 0) {
            //    alert(data.strResponse);
            //}
            else if (data === 'Error' || data === null) {
                toastr.error('Invalid Username or Password.');
            }
            document.getElementById('Email').value = '';
            document.getElementById('Password').value = '';
        },
        error: function (xhr) {

            alert('errors');
        }
    });
}



function MyProfile() {
    ShowWait();
    $.ajax({
        url: '/Account/GetMyProfile',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: {},
        success: function (data) {
            if (data !== null) {
                document.getElementById('hdnintId').value = data.LSTAccountList[0].Id;
                document.getElementById('FullName').value = data.LSTAccountList[0].FullName;
                document.getElementById('Email').value = data.LSTAccountList[0].Email;
                document.getElementById('MobileNo').value = data.LSTAccountList[0].Mobile;
                document.getElementById('Address').value = data.LSTAccountList[0].Address;
                document.getElementById('UserName').value = data.LSTAccountList[0].UserName;
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


//function UpdateProfile(id) {
//    debugger
//    var val = true;
//    var Id = id

//    var fileCount = document.getElementById("Profile").files.length;
//    var Profile = document.getElementById('Profile').value;
//    if (fileCount > 0) {
//        for (var i = 0; i < fileCount; i++) {
//            var Profile = document.getElementById("Profile").files[i];
//            var ext = Profile.name.split('.').pop();
//            if (ext.toLowerCase() === "jpg" || ext.toLowerCase() === "jpeg" || ext.toLowerCase() === "png") {
//                formData.append("Profile", Profile);
//            }
//            else {
//                alert('Please upload valid file.');
//                return;
//            }
//        }
//    }
//    var formData = new FormData();
//    var UserName = document.getElementById('UserName').value;
//    if (UserName === "" || /\S/.test(UserName) === false) {
//        $("#errusername").html("Please enter Username.");
//        val = false;
//    }
//    var FullName = document.getElementById('FullName').value;
//    if (FullName === "" || /\S/.test(FullName) === false) {
//        $("#errname").html("Please enter FullName.");
//        val = false;
//    }
//    var Email = document.getElementById('Email').value;

//    var MobileNo = document.getElementById('MobileNo').value;
//    if (MobileNo === "" || /\S/.test(MobileNo) === false) {
//        $("#errmobile").html("Please enter MobileNo.");
//        val = false;
//    }
//    var Address = document.getElementById('Address').value;
//    if (Address === "" || /\S/.test(Address) === false) {
//        $("#erraddress").html("Please enter Address.");
//        val = false;
//    }
//    if (val === false) {
//        return;
//    }

//    formData.append('Id', Id);
//    formData.append('UserName', UserName);
//    formData.append('Profile', Profile);
//    formData.append('FullName', FullName);
//    formData.append('Email', Email);
//    formData.append('Mobile', Mobile);
//    formData.append('Address', Address);

    
//    ShowWait()
//    $.ajax({
//        url: '/Account/UpdateProfile',
//        contentType: "application/json; charset=utf-8",
//        type: "POST",
//        data: formData,
//        success: function (data) {

//            if (data != null) {
//                if (data.Response === 'Success') {
//                    toastr.success('Profile updated successfully');
//                    MyProfile();
//                }
//            }
//            HideWait();
//        },
//        error: function (xhr) {
//            HideWait();
//            alert('errors');
//        }
//    });
//}


function UpdateProfile(id) {

    var val = true;
    var Id =id;
    var FullName = $('#FullName').val();
    var UserName = $('#UserName').val();
    var Email = $('#Email').val();
    var MobileNo = $('#MobileNo').val();
    var Address = $('#Address').val();
   
    if (FullName === "" || /\S/.test(FullName) === false) {
        $("#errFullName").html("Please enter first name.");
        val = false;
    }
   
    if (UserName === "" || /\S/.test(UserName) === false) {
        $("#errUserName").html("Please enter last name.");
        val = false;
    }
    
   
    if (MobileNo === "" || MobileNo.trim() === '') {
        $("#errMobile").html("Please enter mobile.");
        val = false;
    }
   
   
    
    if (Email === '' || Email.trim() === '') {
        $("#errEmail").html('Please enter email id.');
        return;
    }
   
    var formData = new FormData();
    var fileCount = document.getElementById("Profile").files.length;
    var hdnfile = document.getElementById("HiddenfileForImage").value;

    if (hdnfile === null || hdnfile === "") {
        var Profile = document.getElementById('Profile').value;
        if (Profile === null || Profile === "") {
            $("#errProfile").html("Please select Profile.");
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

        var ProfileImg = Profile;

    }
    
    if (val === false) {
        return;
    }

    formData.append('Id', Id);
    formData.append('FullName', FullName);
    formData.append('UserName', UserName);
    formData.append('Email', Email);
    formData.append('Mobile', MobileNo);
    formData.append('Address', Address);
    formData.append('Profile', Profile);
    formData.append('ProfileImg', ProfileImg);
    /*formData.append('Profile', Profile);*/
    ShowWait();
    $.ajax({
        type: "POST",
        url: '/Account/UpdateProfile',
        contentType: "application/json; charset=utf-8",
        contentType: false,
        processData: false,
        data: formData,
        success: function (data) {
            if (data !== null) {
                if (data.Response === 'Success') {
                    toastr.success('Profile updated successfully');
                    MyProfile();
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






function ChangePassword() {
    var val = true;
    var oldPassword = document.getElementById('oldPassword').value;
    var retypePassword = document.getElementById('retypePassword').value;
    var newPassword = document.getElementById('newPassword').value;

    if (oldPassword === "" || /\S/.test(oldPassword) === false) {
        $("#errOldPass").html("Please enter old Password.");
        val = false;
    }
    if (retypePassword === "" || /\S/.test(retypePassword) === false) {
        $("#errRetypePass").html("Please enter re-type Password.");
        val = false;
    }
    if (newPassword === "" || /\S/.test(newPassword) === false) {
        $("#errNewPass").html("Please enter New Password.");
        val = false;
    }
    if (oldPassword !== retypePassword ) {
        toastr.error('OldPassword and Re-TypePassword do not match.');
        val = false;
    }
    if (newPassword === oldPassword && newPassword !== "" && oldPassword !== "") {
        toastr.error('NewPassword and OldPassword are same.');
        val = false;
    }

    if (val === false) {
        return;
    }

    var cls = {
        Password: oldPassword,
        Password: retypePassword,
        Password: newPassword
    }
    ShowWait()
    $.ajax({
        url: '/Account/UpdatePassword',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {

            if (data !== null) {
                if (data.Response === 'Success') {
                    toastr.success('Password changed successfully');
                    MyProfile();
                }
            }
            HideWait();
        },
        error: function (xhr) {
            HideWait();
            alert('errors');
        }
    });
}


function showPwd() {
    var x = document.getElementById("Password");

    if (x.type === "password") {
        x.type = "text";
    }
    else {
        x.type = "password";
    }
}

function Logout() {

    $.ajax({
        url: '/Account/Logout',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({

        }),
        success: function (data) {
            window.location.href = '/Account/Login';

        },
        error: function (xhr) {
            alert('errors');
        }
    });
}