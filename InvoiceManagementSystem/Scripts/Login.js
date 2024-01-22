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
    var ProfileImg1 = document.getElementById('ProfileImg1');
    var ProfileImg2 = document.getElementById('ProfileImg2');

    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            ProfileImg.src = e.target.result;
            ProfileImg1.src = e.target.result;
            ProfileImg2.src = e.target.result;
            $('#removeButton').show(); // Show remove button when an image is selected
        };
        reader.readAsDataURL(input.files[0]);
    } else {
        ProfileImg.src = "~/Data/Profile/dummy.jpg";
    }
}

function removeImage() {

    var ProfileImg = document.getElementById('ProfileImg');
    var ProfileImg1 = document.getElementById('ProfileImg1');
    var ProfileImg2 = document.getElementById('ProfileImg2');
    ProfileImg.src = "/Data/Profile/dummy.jpg";
    ProfileImg1.src = "/Data/Profile/dummy.jpg";
    ProfileImg2.src = "/Data/Profile/dummy.jpg";
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
    var cls = {}
    ShowWait();
    $.ajax({
        url: '/Account/GetMyProfile',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {
            HideWait();
        },
        error: function (xhr) {
            HideWait();
            alert('errors');
        }
    });
}

function UpdateProfile(id) {
    
    var val = true;
    var Id =id;
    var UserName = $('#UserName').val();
    var UserName1 = $('#UserName1').val();
    var FatherName = $('#FatherName').val();
    var SurName = $('#SurName').val();
    var SurName1 = $('#SurName1').val();
    var Email = $('#Email').val();
    var MobileNo = $('#MobileNo').val();
    var Address = $('#Address').val();
    UserName1 = UserName;
    SurName1 = SurName;
    if (UserName === "" || /\S/.test(UserName) === false) {
        $("#errUserName").html("Please enter last name.");
        val = false;
    }
    if (FatherName === "" || /\S/.test(FatherName) === false) {
        $("#errFatherName").html("Please enter father name.");
        val = false;
    }
    if (SurName === "" || /\S/.test(SurName) === false) {
        $("#errSurName").html("Please enter surname.");
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

    var hdnfile = document.getElementById('HiddenfileForImage').value;
    var fileCount = document.getElementById("Profile").files.length;

    var Profile = document.getElementById('Profile').value;

    if (Profile === null || Profile === "") {
        Profile = hdnfile;
    }

    if (fileCount > 0) {
        for (var i = 0; i < fileCount; i++) {
            Profile = document.getElementById("Profile").files[i];
            var ext = Profile.name.split('.').pop();

            if (ext.toLowerCase() === "jpg" || ext.toLowerCase() === "jpeg" || ext.toLowerCase() === "png") {
                formData.append("Profile", Profile);
            } else {
                alert('Please upload valid file.');
                return;
            }
        }
    }

    if (val === false) {
        return;
    }

    formData.append('Id', Id);
    formData.append('FatherName', FatherName);
    formData.append('SurName', SurName);
    formData.append('UserName', UserName);
    formData.append('Email', Email);
    formData.append('Mobile', MobileNo);
    formData.append('Address', Address);
    formData.append('Profile', Profile);
    formData.append('ProfileImg', ProfileImg);
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