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
    GetFeesList(1);
});
function myFunction() {
    document.getElementById("myDropdown").classList.toggle("show");
}


$(function () {
    $("#Dob").datetimepicker({
        format: 'DD/MM/YYYY',
        maxDate: new Date,
        useCurrent: true,
        ignoreReadonly: true
    })
});
function InsertData() {

    var val = true;
    var Id = $('#hdnintId').val();
    var ClassId = $('#ClassId').val();
    if (ClassId == "0") {
        $("#errClassId").html("Please select class.");
        val = false;
    }
    var Monthly = $('#Monthly').val();
    if (Monthly === "" || /\S/.test(Monthly) === false) {
        $("#errMonthly").html("Please enter Monthly fees.");
        val = false;
    }
    var Yearly = $('#Yearly').val();
    if (Yearly === "" || /\S/.test(Yearly) === false) {
        $("#errYearly").html("Please enter Yearly fees.");
        val = false;
    }

    var formData = new FormData();
    if (val === false) {
        return;
    }

    formData.append('Id', Id);
    formData.append('ClassId', ClassId);
    formData.append('Monthly', Monthly);
    formData.append('Yearly', Yearly);
    ShowWait();
    $.ajax({
        type: "POST",
        url: '/Fees/InsertClassWiseFees',
        contentType: "application/json; charset=utf-8",
        contentType: false,
        processData: false,
        data: formData,
        success: function (data) {
            if (data !== null) {
                if (data === 'Success') {
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
                    $("#ClassId").val('0').trigger('change');
                    document.getElementById('Monthly').value = "";
                    document.getElementById('Yearly').value = "";
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
function GetFeesList(page) {

    var Id = 0;

    var intActive = document.getElementById('intActive').value;
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
        intActive: intActive,
        ClassId: ClassId,
        PageSize: PageSize,
        PageIndex: PageIndex,
    }
    ShowWait();
    $.ajax({
        url: '/Fees/GetClassWiseFees',
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
    $('#errSubjectName').html("");
    document.getElementById('btnAdd').innerHTML = "Update";
    $("#btnAdd").attr('title', 'Update');
    document.getElementById('PopupTitle').innerHTML = "Update Fees";
    var cls = {
        Id: id
    }

    $.ajax({
        url: '/Fees/GetSingleClassWiseFeesData',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {

            if (data !== null) {
                document.getElementById('hdnintId').value = data.LSTFeesList[0].Id;
                document.getElementById('Monthly').value = data.LSTFeesList[0].Monthly;
                document.getElementById('Yearly').value = data.LSTFeesList[0].Yearly;
                $('#ClassId').val(data.LSTFeesList[0].ClassId).trigger("change");
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

function deleteFees() {
    var Id = document.getElementById('hdnintId').value;
    var cls = {
        Id: Id
    }
    ShowWait();
    $.ajax({
        url: '/Fees/deleteClassWiseFees',
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
    document.getElementById('Monthly').value = "";
    $('#errMonthly').html("");
    document.getElementById('Yearly').value = "";
    $('#errYearly').html("");
    $("#ddlClassId").val('0').trigger('change');
    $('#errClassId').html("");
    document.getElementById('btnAdd').innerHTML = "Add";
    $("#btnAdd").attr('title', 'Upload');
    document.getElementById('PopupTitle').innerHTML = "Add Fees";
}
function UpdateStatus() {
    var Id = document.getElementById('hdnintId').value;
    var cls = {
        Id: Id
    }
    ShowWait();
    $.ajax({
        url: '/Fees/UpdateStatus',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {
            if (data === 'success') {
                toastr.success('status updated successfully.');
                document.getElementById('hdnintId').value = "0";
                GetFeesList(1);
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
function ClearData(type) {

    if (type === 1) {
        var Id = document.getElementById('hdnintId').value;
        document.getElementById('Monthly').value = "";
        $('#errMonthly').html("");
        document.getElementById('Yearly').value = "";
        $('#errYearly').html("");
        $("#ClassId").val('0').trigger('change');
        $('#errClassId').html("");
        if (Id === "0") {

            document.getElementById('hdnintId').value = "0";
            document.getElementById('btnAdd').innerHTML = "Add";
            $("#btnAdd").attr('title', 'Add');
            document.getElementById('PopupTitle').innerHTML = "Add Fees";
        }
        else {
            document.getElementById('btnAdd').innerHTML = "Update";
            $("#btnAdd").attr('title', 'Update');
            document.getElementById('PopupTitle').innerHTML = "Update Fees";
        }
    }
    else {
        document.getElementById('intActive').value = '3';
        $("#ddlClassId").val('0').trigger('change');
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
function openstatusModel(id) {

    document.getElementById('hdnintId').value = id;
}
