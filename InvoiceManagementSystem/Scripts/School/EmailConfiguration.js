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
    GetEmailConfigurationList(1);
    $('#FilterDiv').hide();
});

function GetEmailConfigurationList(page) {
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
        url: '/EmailConfiguration/GetEmailConfiguration',
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

function ExportEmailConfiguration() {
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
        url: '/EmailConfiguration/ExpotToExcelEmailConfigurationReport',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {
            if (data === "success") {
                window.location.href = "/EmailConfiguration/ExportToExcel";
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
    var Rolename = $('#RoleId').val();
    if (Rolename === "" || /\S/.test(Rolename) === false) {
        $("#errRole").html("Please enter Role.");
        val = false;
    }
    var Username = $('#UserName').text();
    var FromMail = $('#FromMail').text();
    var Password = $('#Password').val();
    if (Password === "" || /\S/.test(Password) === false) {
        $("#errPassword").html("Please enter password.");
        val = false;
    }
    
    var TeacherId = null;
    if (Rolename === "Teacher") {
        TeacherId = $('#TeacherId').val();
    }
    else if (Rolename === "Student"){
        TeacherId = $('#TeacherId1').val();
    }

    if (TeacherId === "" || /\S/.test(TeacherId) === false) {
        $("#errTeacher").html("Please select teacher.");
        val = false;
    }
    var TeacherId1 = $('#TeacherId1').val();
    var StudentId = $('#StudentId').val();
    if (StudentId === "" || /\S/.test(StudentId) === false) {
        $("#errStudent").html("Please select student.");
        val = false;
    }

    var formData = new FormData();
    if (val === false) {
        return;
    }

    formData.append('Id', Id);
    formData.append('SearchText', SearchText);
    formData.append('RoleName', Rolename);
    formData.append('UserName', Username);
    formData.append('FromMail', FromMail);
    formData.append('Password', Password);
    if (Rolename === "Teacher") {
        formData.append('TeacherId', TeacherId);
    }
    else if (Rolename === "Student")
    {
        formData.append('TeacherId', TeacherId1);
    }
    formData.append('StudentId', StudentId);
    ShowWait();
    $.ajax({
        type: "POST",
        url: '/EmailConfiguration/InsertEmailConfiguration',
        contentType: "application/json; charset=utf-8",
        contentType: false,
        processData: false,
        data: formData,
        success: function (data) {
            if (data !== null) {
                if (data === 'Success') {
                    toastr.success('EmailConfiguration inserted successfully');
                    window.location.replace("/EmailConfiguration/EmailConfigurationList");
                    ClearData(1);
                }
                else if (data === 'Updated') {
                    toastr.success('EmailConfiguration updated successfully');
                    window.location.replace("/EmailConfiguration/EmailConfigurationList");
                    ClearData(1);
                }
                else if (data === 'Exists') {
                    toastr.error('EmailConfiguration already exists!');
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

function GetSingleEmailConfigurationData(id) {
    $('#errClassNo').html("");
    document.getElementById('btnAdd').innerHTML = "Update";
    $("#btnAdd").attr('title', 'Update');
    document.getElementById('PopupTitle').innerHTML = "Update EmailConfiguration";
    var cls = {
        Id: id
    }

    $.ajax({
        url: '/EmailConfiguration/GetSingleEmailConfigurationData',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {

            if (data !== null) {
                document.getElementById('hdnintId').value = data.LSTEmailConfigurationList[0].Id;
                document.getElementById('ClassNo').value = data.LSTEmailConfigurationList[0].ClassNo;
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

function deleteEmailConfiguration() {
    var Id = document.getElementById('hdnintId').value;
    var cls = {
        Id: Id
    }
    ShowWait();
    $.ajax({
        url: '/EmailConfiguration/deleteEmailConfiguration',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {

            if (data.Response === 'Success') {

                toastr.success('EmailConfiguration deleted successfully');
                window.location.replace("/EmailConfiguration/EmailConfigurationList");
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
        url: '/EmailConfiguration/ExportToExcel',
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
    document.getElementById('PopupTitle').innerHTML = "Add EmailConfiguration";
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
            document.getElementById('PopupTitle').innerHTML = "Add EmailConfiguration";
        }
        else {
            document.getElementById('btnAdd').innerHTML = "Update";
            $("#btnAdd").attr('title', 'Update');
            document.getElementById('PopupTitle').innerHTML = "Update EmailConfiguration";
        }
    }
    else {
        document.getElementById('SearchText').value = "";
        GetEmailConfigurationList();
    }
}

function opendeleteModel(id) {

    document.getElementById('hdnintId').value = id;
}

function openstatusModel(id) {

    document.getElementById('hdnintId').value = id;
}


function RoleType() {
    debugger
    var Role = $('#RoleId').val();
    if (Role === "Admin") {
        onUser();
        teacherDiv.style.display = "none";
        teacher1Div.style.display = "none";
        studentDiv.style.display = "none";
    }
    else if(Role === "Teacher")
    {
        teacher1Div.style.display = "none";
        teacherDiv.style.display = "block";
        studentDiv.style.display = "none";
        GetTeacher();
        /*$('#errLeaveSubType').html("");*/
    }
    else {
        GetTeacher1();
        teacherDiv.style.display = "none";
        teacher1Div.style.display = "block";
        studentDiv.style.display = "block";
        /*$('#errLeaveSubType').html("");*/
    }
}

function GetTeacher() {
    var cls = {
    }
    $.ajax({
        url: '/Common/GetTeacher',
        contentType: "application/json; charset=utf-8",
        type: "GET",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {

            var html = "";
            html = html + ' <option value="0" selected>Select Teacher</option>';
            for (var i = 0; i < data.LSTTeacherList.length; i++) {
                html = html + '<option value="' + data.LSTTeacherList[i].Id + '">' + data.LSTTeacherList[i].FullName + '</option>';
                $("#TeacherId").empty();
                $("#TeacherId").append(html);
            }
        }
    });
}
function GetTeacher1() {
    var cls = {
    }
    $.ajax({
        url: '/Common/GetTeacher',
        contentType: "application/json; charset=utf-8",
        type: "GET",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {

            var html = "";
            html = html + ' <option value="0" selected>Select Teacher</option>';
            for (var i = 0; i < data.LSTTeacherList.length; i++) {
                html = html + '<option value="' + data.LSTTeacherList[i].Id + '">' + data.LSTTeacherList[i].FullName + '</option>';
                $("#TeacherId1").empty();
                $("#TeacherId1").append(html);
            }
        }
    });
}
function onUser() {
    var UserId = $('#UserId').val();
    $.ajax({
        type: "GET",
        url: '/Common/LoadUser?UserId=' + UserId,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $('#UserName').empty();
            $("#UserName").append();
            $.each(data, function (i, v) {
                $("#UserName").append($("<option     />").val(v.Username).text(v.Username));
            });
            $('#FromMail').empty();
            $("#FromMail").append();
            $.each(data, function (i, v) {
                $("#FromMail").append($("<option     />").val(v.FromMail).text(v.FromMail));
            });
            HideWait();
        },
        failure: function () {
            HideWait();
            alert("Failed!");
        }
    });
}

function onTeacher() {
    var TeacherId = $('#TeacherId').val();
    $.ajax({
        type: "GET",
        url: '/Common/LoadTeacher?TeacherId=' + TeacherId,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $('#UserName').empty();
            $("#UserName").append();
            $.each(data, function (i, v) {
                $("#UserName").append($("<option     />").val(v.Username).text(v.Username));
            });
            $('#FromMail').empty();
            $("#FromMail").append();
            $.each(data, function (i, v) {
                $("#FromMail").append($("<option     />").val(v.FromMail).text(v.FromMail));
            });
            HideWait();
        },
        failure: function () {
            HideWait();
            alert("Failed!");
        }
    });
}

function onStudent() {
    var TeacherId = $('#TeacherId1').val();
    $.ajax({
        type: "GET",
        url: '/Common/LoadStudent?TeacherId=' + TeacherId,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $('#StudentId').empty();
            $("#StudentId").append($("<option     />").val("0").text("Select student name.."));
            $.each(data, function (i, v) {
                $("#StudentId").append($("<option     />").val(v.StudentId).text(v.StudentName));
            });
            $('#UserName').empty();
            $("#UserName").append();
            $.each(data, function (i, v) {
                $("#UserName").append($("<option     />").val(v.Username).text(v.Username));
            });
            $('#FromMail').empty();
            $("#FromMail").append();
            $.each(data, function (i, v) {
                $("#FromMail").append($("<option     />").val(v.FromMail).text(v.FromMail));
            });
            HideWait();
        },
        failure: function () {
            HideWait();
            alert("Failed!");
        }
    });
}

function onStudentDetails() {
    var StudentId = $('#StudentId').val();
    $.ajax({
        type: "GET",
        url: '/Common/LoadStudentDetails?StudentId=' + StudentId,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $('#UserName').empty();
            $("#UserName").append();
            $.each(data, function (i, v) {
                $("#UserName").append($("<option     />").val(v.Username).text(v.Username));
            });
            $('#FromMail').empty();
            $("#FromMail").append();
            $.each(data, function (i, v) {
                $("#FromMail").append($("<option     />").val(v.FromMail).text(v.FromMail));
            });
            HideWait();
        },
        failure: function () {
            HideWait();
            alert("Failed!");
        }
    });
}