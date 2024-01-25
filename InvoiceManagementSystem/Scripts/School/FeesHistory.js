$(document).ready(function () {
    GetFeesHistoryList(1);
});

function GetFeesHistoryList(page) {

    var Id = 0;
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
        PageSize: PageSize,
        PageIndex: PageIndex,
    }
    ShowWait();
    $.ajax({
        url: '/Fees/GetFeesHistory',
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
                document.getElementById('FeesAmount').value = data.LSTFeesList[0].FeesAmount;
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
function deleteFeesHistory() {

    var Id = document.getElementById('hdnintId').value;
    var StudentId = document.getElementById('hdnstudentid').value;
    var MonthId = document.getElementById('hdnmonthid').value;
    var cls = {
        Id: Id,
        StudentId: StudentId,
        MonthId: MonthId,
    }
    ShowWait();
    $.ajax({
        url: '/Fees/deleteFeesHistory',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {
            if (data.Response === 'Success') {
                toastr.success('Fees History deleted successfully');
                document.getElementById('hdnintId').value = "0";
                GetFeesHistoryList(1);
                $('#delete_FeesHistory').click();
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

function opendeleteModel(id, studentid, monthid) {
    document.getElementById('hdnintId').value = id;
    document.getElementById('hdnstudentid').value = studentid;
    document.getElementById('hdnmonthid').value = monthid;
}
