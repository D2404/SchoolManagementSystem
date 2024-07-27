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
    GetClassRoom();
    GetExamList(1);
});

function InsertData() {

    var val = true;
    var Id = $('#hdnintId').val();
    var ClassId = $('#ClassId').val();
    var StudentId = $('#StudentId').val();
    var SubjectId = $('#SubjectId').val();
    var SearchText = $('#SearchText').val();
    var RollNo = $('#RollNo').text();

    var Outofmarks = $('#Outofmarks').val();
    if (Outofmarks === "" || /\S/.test(Outofmarks) === false) {
        $("#errOutofmarks").html("Please enter marks.");
        val = false;
    }
    var Totalmarks = $('#Totalmarks').val();
    if (Totalmarks === "" || /\S/.test(Totalmarks) === false) {
        $("#errTotalmarks").html("Please enter total marks.");
        val = false;
    }
    var formData = new FormData();
    if (val === false) {
        return;
    }

    formData.append('Id', Id);
    formData.append('ClassId', ClassId);
    formData.append('StudentId', StudentId);
    formData.append('SubjectId', SubjectId);
    formData.append('RollNo', RollNo);
    formData.append('Outofmarks', Outofmarks);
    formData.append('Totalmarks', Totalmarks);
    formData.append('SearchText', SearchText);

    ShowWait();
    $.ajax({
        type: "POST",
        url: '/Exam/InsertExamMarks',
        contentType: "application/json; charset=utf-8",
        contentType: false,
        processData: false,
        data: formData,
        success: function (data) {
            if (data !== null) {
                if (data === 'Success') {
                    toastr.success('Exam inserted successfully');
                    GetExamList(1);
                    $('#Exam').click();
                    ClearData(1);
                }
                else if (data === 'Updated') {
                    toastr.success('Exam updated successfully');
                    GetExamList(1);
                    $('#Exam').click();
                    ClearData(1);
                }
                else if (data === 'Exists') {
                    toastr.error('Exam already exists!');
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
        url: '/Common/GetClassRoom',
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

function onClass() {
    var ClassId = $('#ClassId').val();
    $.ajax({
        type: "GET",
        url: '/Common/LoadSubject?ClassId=' + ClassId,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            $('#SubjectId').empty();
            $("#SubjectId").append($("<option     />").val("0").text("Select subject name.."));
            $.each(data, function (i, v) {
                $("#SubjectId").append($("<option     />").val(v.SubjectId).text(v.SubjectName));
            });
            onClasswiseStudent();
            HideWait();
        },
        failure: function () {
            HideWait();
            alert("Failed!");
        }
    });

}

function onClasswiseStudent() {
    var ClassId = $('#ClassId').val();
    $.ajax({
        type: "GET",
        url: '/Common/LoadClassWiseStudent?ClassId=' + ClassId,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {


            $('#StudentId').empty();
            $("#StudentId").append($("<option     />").val("0").text("Select student name.."));
            $.each(data, function (i, v) {
                $("#StudentId").append($("<option     />").val(v.StudentId).text(v.StudentName));
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
    var StudentId = $('#StudentId').val();
    $.ajax({
        type: "GET",
        url: '/Fees/LoadRollNo?StudentId=' + StudentId,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            $('#RollNo').empty();
            $("#RollNo").append();
            $.each(data, function (i, v) {
                $("#RollNo").append($("<option     />").val(v.RollNo).text(v.RollNo));
            });
            HideWait();
        },
        failure: function () {
            HideWait();
            alert("Failed!");
        }
    });
}

function GetExamList(page) {

    var Id = 0;
    var SearchText = document.getElementById('SearchText').value;
    var ClassId = document.getElementById('ddlClassId').value;
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
        ClassId: ClassId,
        PageSize: PageSize,
        PageIndex: PageIndex,
    }
    ShowWait();
    $.ajax({
        url: '/Exam/GetExamMarks',
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

function GetSingleExamData(id) {

    document.getElementById('btnAdd').innerHTML = "Update";
    $("#btnAdd").attr('title', 'Update');
    document.getElementById('PopupTitle').innerHTML = "Update Exam Marks";
    var cls = {
        Id: id
    }

    $.ajax({
        url: '/Exam/GetSingleExamMarksData',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {

            if (data !== null) {
                document.getElementById('hdnintId').value = data.LSTExamList[0].Id;
                $('#ClassId').val(data.LSTExamList[0].ClassId).trigger("change");
                $('#SubjectId').val(data.LSTExamList[0].SubjectId).trigger("change");
                $('#StudentId').val(data.LSTExamList[0].StudentId).trigger("change");
                document.getElementById('RollNo').text = data.LSTExamList[0].RollNo;
                document.getElementById('Outofmarks').value = data.LSTExamList[0].OutOfMarks;
                document.getElementById('Totalmarks').value = data.LSTExamList[0].TotalMarks;
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

function deleteExam() {
    var Id = document.getElementById('hdnintId').value;
    var cls = {
        Id: Id
    }
    ShowWait();
    $.ajax({
        url: '/Exam/deleteExamMarks',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {

            if (data.Response === 'Success') {

                toastr.success('Exam deleted successfully');
                document.getElementById('hdnintId').value = "0";
                GetExamList();
                $('#delete_Exam').click();
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
    document.getElementById('RollNo').value = "";
    $('#errRollNo').html("");
    document.getElementById('Outofmarks').value = "";
    $('#errOutofmarks').html("");
    document.getElementById('Totalmarks').value = "";
    $('#errTotalmarks').html("");
    $("#ClassId").val('0').trigger('change');
    $("#SubjectId").val('0').trigger('change');
    document.getElementById('btnAdd').innerHTML = "Add";
    $("#btnAdd").attr('title', 'Upload');
    document.getElementById('PopupTitle').innerHTML = "Add Exam Marks";
}

function ClearData(type) {

    if (type === 1) {
        var Id = document.getElementById('hdnintId').value;
        document.getElementById('RollNo').value = "";
        $('#errRollNo').html("");
        document.getElementById('Outofmarks').value = "";
        $('#errOutofmarks').html("");
        document.getElementById('Totalmarks').value = "";
        $('#errTotalmarks').html("");
        $("#ClassId").val('0').trigger('change');
        $("#SubjectId").val('0').trigger('change');

        if (Id === "0") {

            document.getElementById('hdnintId').value = "0";
            document.getElementById('btnAdd').innerHTML = "Add";
            $("#btnAdd").attr('title', 'Add');
            document.getElementById('PopupTitle').innerHTML = "Add Exam Marks";
        }
        else {
            document.getElementById('btnAdd').innerHTML = "Update";
            $("#btnAdd").attr('title', 'Update');
            document.getElementById('PopupTitle').innerHTML = "Update Exam Marks";
        }
    }
    else {
        document.getElementById('SearchText').value = "";
        GetExamList();
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
