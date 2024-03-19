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
    GetClassRoom();
    GetMonth();
    GetFeesList(1);
    if (document.getElementById('MonthId')) {
        var element = document.getElementById('MonthId');
        const example = new Choices(element, {
            searchEnabled: false
        });
    };
});

$("#FeesAmount").keyup(function () {

    var amt = $('#FeesAmount').val();

    var Yearly = $('#Yearly').text();
    if (amt > Number(Yearly)) {
        $('#errFeesAmount').html('Fees must be less than YearlyFees.');
        $('#FeesAmount').val(0);
        return false;
    }
});

function InsertData() {

    var val = true;
    var Id = $('#hdnintId').val();
    var ClassId = $('#ClassId').val();
    if (ClassId === 0) {
        $("#errClassNo").html("Please select classroom.");
        val = false;
    }
    var StudentId = $('#StudentId').val();
    if (StudentId === 0) {
        $("#errStudent").html("Please select student.");
        val = false;
    }
    var MonthId = $('#MonthId').val();
    if (MonthId === 0) {
        $("#errMonth").html("Please select month.");
        val = false;
    }
    var YearId = $('#YearId').val();
    if (YearId === 0) {
        $("#errYear").html("Please select year.");
        val = false;
    }
    var SearchText = $('#SearchText').val();
    var Date = $('#Date').val();
    if (Date === "" || /\S/.test(Date) === false) {
        $("#errDate").html("Please select date.");
        val = false;
    }
    var RollNo = $('#RollNo').text();
    var FeesAmount = $('#FeesAmount').val();
    if (FeesAmount === "" || /\S/.test(FeesAmount) === false) {
        $("#errFeesAmount").html("Please enter fees amount.");
        val = false;
    }
    var formData = new FormData();
    if (val === false) {
        return;
    }
    formData.append('Id', Id);
    formData.append('ClassId', ClassId);
    formData.append('StudentId', StudentId);
    formData.append('MonthId', MonthId);
    formData.append('YearId', YearId);
    formData.append('RollNo', RollNo);
    formData.append('Date', Date);
    formData.append('FeesAmount', FeesAmount);
    formData.append('SearchText', SearchText);
    ShowWait();
    $.ajax({
        type: "POST",
        url: '/Fees/InsertFees',
        contentType: "application/json; charset=utf-8",
        contentType: false,
        processData: false,
        data: formData,
        success: function (data) {
            if (data !== null) {
                if (data === 'Success' && Id === 0) {
                    toastr.success('Fees inserted successfully');
                    GetFeesList(1);
                    $('#Fees').click();
                    ClearData(1);
                }
                else if (data === 'Updated') {
                    toastr.success('Fees updated successfully');
                    GetFeesList(1);
                    $('#Fees').click();
                    ClearData(1);
                }
                else if (data === 'Exists') {
                    toastr.error('Fees already exists!');
                    document.getElementById('Email').value = "";
                }
                else if (data === 'LastMonth') {
                    toastr.success('LastMonth Fees inserted successfully!');
                    GetFeesList(1);
                    $('#Fees').click();
                    ClearData(1);
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


function InsertandPayData() {

    var val = true;
    var Id = $('#hdnintId').val();
    var ClassId = $('#ClassId').val();
    if (ClassId === 0) {
        $("#errClassNo").html("Please select classroom.");
        val = false;
    }
    var StudentId = $('#StudentId').val();
    if (StudentId === 0) {
        $("#errStudent").html("Please select student.");
        val = false;
    }
    var MonthId = $('#MonthId').val();
    if (MonthId === 0) {
        $("#errMonth").html("Please select month.");
        val = false;
    }
    var YearId = $('#YearId').val();
    if (YearId === 0) {
        $("#errYear").html("Please select year.");
        val = false;
    }
    var SearchText = $('#SearchText').val();
    var Date = $('#Date').val();
    if (Date === "" || /\S/.test(Date) === false) {
        $("#errDate").html("Please select date.");
        val = false;
    }
    var RollNo = $('#RollNo').text();
    var FeesAmount = $('#FeesAmount').val();
    if (FeesAmount === "" || /\S/.test(FeesAmount) === false) {
        $("#errFeesAmount").html("Please enter fees amount.");
        val = false;
    }
    var formData = new FormData();
    if (val === false) {
        return;
    }
    formData.append('Id', Id);
    formData.append('ClassId', ClassId);
    formData.append('StudentId', StudentId);
    formData.append('MonthId', MonthId);
    formData.append('YearId', YearId);
    formData.append('RollNo', RollNo);
    formData.append('Date', Date);
    formData.append('FeesAmount', FeesAmount);
    formData.append('SearchText', SearchText);
    ShowWait();
    $.ajax({
        type: "POST",
        url: '/Fees/InsertandPayFees',
        contentType: "application/json; charset=utf-8",
        contentType: false,
        processData: false,
        data: formData,
        success: function (data) {
            if (data !== null) {
                if (data === 'Success' && Id === 0) {
                    toastr.success('Fees inserted successfully');
                    GetFeesList(1);
                    $('#Fees').click();
                    ClearData(1);
                }
                else if (data === 'Updated') {
                    toastr.success('Fees updated successfully');
                    GetFeesList(1);
                    $('#Fees').click();
                    ClearData(1);
                }
                else if (data === 'Exists') {
                    toastr.error('Fees already exists!');
                    $("#MonthId").val('0').trigger('change');
                }
                else if (data === 'LastMonth') {
                    toastr.success('LastMonth Fees inserted successfully!');
                    GetFeesList(1);
                    $('#Fees').click();
                    ClearData(1);
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
        url: '/Fees/GetClassRoom',
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
function GetMonth() {
    var cls = {
    }
    $.ajax({
        url: '/Fees/GetMonth',
        contentType: "application/json; charset=utf-8",
        type: "GET",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {
            var html = "";
            html = html + ' <option value="0" selected>Select Month</option>';
            for (var i = 0; i < data.LSTMonthList.length; i++) {
                html = html + '<option value="' + data.LSTMonthList[i].Id + '">' + data.LSTMonthList[i].MonthName + '</option>';
                $("#MonthId").empty();
                $("#MonthId").append(html);
                //$("#ddlClassId").empty();
                //$("#ddlClassId").append(html);
            }
        }
    });
}

function onClass() {
    var ClassId = $('#ClassId').val();
    $.ajax({
        type: "GET",
        url: '/Fees/LoadStudent?ClassId=' + ClassId,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            $('#StudentId').empty();
            $("#StudentId").append($("<option     />").val("0").text("Select student name.."));
            $.each(data, function (i, v) {
                $("#StudentId").append($("<option     />").val(v.StudentId).text(v.StudentName));
            });
            $('#Monthly').empty();
            $("#Monthly").append();
            $.each(data, function (i, v) {
                $("#Monthly").append($("<option     />").val(v.Monthly).text(v.Monthly));
            });
            $('#Yearly').empty();
            $("#Yearly").append();
            $.each(data, function (i, v) {
                $("#Yearly").append($("<option     />").val(v.Yearly).text(v.Yearly));
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

function onddlClass() {

    var ClassId = $('#ddlClassId').val();
    $.ajax({
        type: "GET",
        url: '/Fees/LoadDDlStudent?ClassId=' + ClassId,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            $('#ddlStudentId').empty();
            $("#ddlStudentId").append($("<option />").val("0").text("Select student name.."));
            $.each(data, function (i, v) {
                $("#ddlStudentId").append($("<option     />").val(v.StudentId).text(v.StudentName));
            });
            HideWait();
        },
        failure: function () {
            HideWait();
            alert("Failed!");
        }
    });
}
function GetFeesList(page) {

    var Id = 0;
    //var SearchText = document.getElementById('SearchText').value;
    var ClassId = document.getElementById('ddlClassId').value;
    var StudentId = document.getElementById('ddlStudentId').value;
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
        //SearchText: SearchText,
        ClassId: ClassId,
        StudentId: StudentId,
        PageSize: PageSize,
        PageIndex: PageIndex,
    }
    ShowWait();
    $.ajax({
        url: '/Fees/GetFees',
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
function GetSingleFeesData(id) {

    document.getElementById('btnAdd').innerHTML = "Update";
    $("#btnAdd").attr('title', 'Update');
    document.getElementById('btnAdd1').innerHTML = "Update & Pay";
    $("#btnAdd1").attr('title', 'Update');
    document.getElementById('PopupTitle').innerHTML = "Update Fees";
    var cls = {
        Id: id
    }
    $.ajax({
        url: '/Fees/GetSingleFeesData',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {

            if (data !== null) {
                document.getElementById('hdnintId').value = data.LSTFeesList[0].Id;
                $('#ClassId').val(data.LSTFeesList[0].ClassId).trigger("change");
                $('#StudentId').val(data.LSTFeesList[0].StudentId).trigger("change");
                $('#MonthId').val(data.LSTFeesList[0].MonthId).trigger("change");
                $('#YearId').val(data.LSTFeesList[0].YearId).trigger("change");
                document.getElementById('RollNo').value = data.LSTFeesList[0].RollNo;
                $('#Date').val(data.LSTFeesList[0].Date); // Set the value using jQuery

                document.getElementById('FeesAmount').value = data.LSTFeesList[0].FeesAmount;
            } else {
                alert('error');
            }
        },
        error: function (xhr) {
            alert('errors');
        }
    });
}
function deleteFees() {
    var Id = document.getElementById('hdnintId').value;
    var cls = {
        Id: Id
    }
    ShowWait();
    $.ajax({
        url: '/Fees/deleteFees',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {
            if (data.Response === 'Success') {
                toastr.success('Fees deleted successfully');
                document.getElementById('hdnintId').value = "0";
                GetFeesList();
                $('#delete_Fees').click();
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
    $("#ClassId").val('0').trigger('change');
    $('#errClassNo').html("");
    $("#StudentId").val('0').trigger('change');
    $('#errStudent').html("");
    $("#MonthId").val('0').trigger('change');
    $('#errMonth').html("");
    $("#YearId").val('0').trigger('change');
    $('#errYear').html("");
    document.getElementById('Date').value = "";
    $('#errDate').html("");
    document.getElementById('FeesAmount').value = "";
    $('#errFeesAmount').html("");
    document.getElementById('btnAdd').innerHTML = "Save";
    document.getElementById('btnAdd1').innerHTML = "Save & Pay";
    $("#btnAdd").attr('title', 'Upload');
    document.getElementById('PopupTitle').innerHTML = "Add Fees";
}
function ClearData(type) {
    if (type === 1) {
        var Id = document.getElementById('hdnintId').value;
        $("#ClassId").val('0').trigger('change');
        $('#errClassNo').html("");
        $("#StudentId").val('0').trigger('change');
        $('#errStudent').html("");
        $("#MonthId").val('0').trigger('change');
        $('#errMonth').html("");
        $("#YearId").val('0').trigger('change');
        $('#errYear').html("");
        document.getElementById('Date').value = "";
        $('#errDate').html("");
        document.getElementById('FeesAmount').value = "";
        $('#errFeesAmount').html("");
        if (Id === "0") {
            document.getElementById('hdnintId').value = "0";
            document.getElementById('btnAdd').innerHTML = "Save";
            $("#btnAdd").attr('title', 'Add');
            document.getElementById('btnAdd1').innerHTML = "Save & Pay";
            $("#btnAdd1").attr('title', 'Add');
            document.getElementById('PopupTitle').innerHTML = "Add Fees";
        }
        else {
            document.getElementById('btnAdd').innerHTML = "Update";
            $("#btnAdd").attr('title', 'Update');
            document.getElementById('btnAdd1').innerHTML = "Update & Pay";
            $("#btnAdd1").attr('title', 'Update');
            document.getElementById('PopupTitle').innerHTML = "Update Fees";
        }
    }
    else {
        $("#ddlClassId").val('0').trigger('change');
        $("#ddlStudentId").val('0').trigger('change');
        GetFeesList();
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
