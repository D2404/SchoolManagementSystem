var type = 1
var count = 0
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
    var currentDate = new Date().toISOString().slice(0, 10);
    document.getElementById('Date').value = currentDate;
    GetClassRoom();
    GetMonthList(count, 0);
    GetAcademicYearList(count, 0);
    
});


function InsertData() {
        
    var val = true;
    var Id = $('#hdnintId').val();
    var ClassId = $('#ClassId').val();
    if (ClassId === 0 || ClassId === '0') {
        $("#errClass").html("Please select classroom.");
        val = false;
    }
    var SectionId = $('#SectionId').val();
    if (SectionId === 0 || SectionId === '0') {
        $("#errSection").html("Please select section.");
        val = false;
    }
    var StudentId = $('#StudentId').val();
    if (StudentId === 0 || StudentId === '0') {
        $("#errStudent").html("Please select student.");
        val = false;
    }
    var RollNo = $('#RollNo').val();
    if (RollNo === '' || RollNo === 0 || RollNo === '0') {
        $("#errRollNo").html("Please enter roll no.");
        val = false;
    }
    var Date = $('#Date').val();
    if (Date === '' || /\S/.test(Date) === false) {
        $("#errDate").html("Please select date.");
        val = false;
    }
  
    var formData = new FormData();
    if (val === false) {
        return;
    }
    formData.append('Id', Id);
    formData.append('ClassId', ClassId);
    formData.append('SectionId', SectionId);
    formData.append('StudentId', StudentId);
    formData.append('RollNo', RollNo);
    formData.append('Date', Date);
    formData.append('RollNo', RollNo);
    ShowWait();
    $.ajax({
        type: "POST",
        url: '/FeesCollection/InsertFeesCollection',
        contentType: "application/json; charset=utf-8",
        contentType: false,
        processData: false,
        data: formData,
        success: function (data) {
            if (data !== null) {
                if (data === 'Success' && Id === 0) {
                    toastr.success('Fees inserted successfully');
                    //GetFeesList(1);
                    //$('#Fees').click();
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
        url: '/Common/LoadSection?ClassId=' + ClassId,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            $('#SectionId').empty();
            $("#SectionId").append($("<option     />").val("0").text("Select Section.."));
            $.each(data, function (i, v) {
                $("#SectionId").append($("<option     />").val(v.SectionId).text(v.SectionNo));
            });
            HideWait();
        },
        failure: function () {
            HideWait();
            alert("Failed!");
        }
    });
}

function onClassSection() {
    debugger
    var ClassId = $('#ClassId').val();
    var SectionId = $('#SectionId').val();
    $.ajax({
        type: "GET",
        url: '/Common/LoadStudentByClassSection?ClassId=' + ClassId + '&SectionId=' + SectionId,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            $('#StudentId').empty();
            $("#StudentId").append($("<option     />").val("0").text("Select Student.."));
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
        url: '/Common/LoadRollNo?StudentId=' + StudentId,
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
function addDiv(id, obj) {
    debugger
    var maincount = 0;
    var NewDiv = 1;

    $(obj).closest('.element').find('input', 'text').each(function () {
        if (($(this).val() === "" || $(this).val() === "undefined" || $(this).val() === null) && $(this).attr('id') !== 'FeesAmount') {
            $("#errFeesAmount").html("Please enter amount.");
            NewDiv = 0;
        }
    });
    if ($("#MonthId_0").val() === "0") {
        $("#errorMonth").html("Please select month.");
        NewDiv = 0;
    }
    if ($("#AcademicYearId_0").val() === "0") {
        $("#errorAcademicYear").html("Please enter academic year.");
        NewDiv = 0;
    }
    if (NewDiv === 1) {
        var fistChildID = $(".wrapper .element:first-child").attr("id");
        $(obj).closest('.element').find("span[name='errFeesAmount']").html("");
        $(obj).closest('.element').find("span[name='errorMonth']").html("");
        $(obj).closest('.element').find("span[name='errorAcademicYear']").html("");


        maincount = document.getElementsByName('MonthId').length;// $(".wrapper .element").length + 1;
        var el = document.createElement("div");
        el.id = "element_" + (maincount + 1);
        el.className = "element";
        el.setAttribute("style", "float: left; width: 100%; margin-bottom: 5px; padding: 2px;");
        $('.i-plus').hide();
        el.innerHTML = repeatHTML(maincount);

        if (maincount > 1) {
            id = "element_" + (maincount);
        }

        insertAfter(document.getElementById(id), el);

        if (id === "element_1") {
            obj.setAttribute("style", "color: red;font-size: 25px;");
            obj.className = "";
            obj.className = "fa fa-plus text-success i-plus";
            //obj.setAttribute("onclick", "removeDiv(this)");
        }

        $('.wrapper').find('.element input').on('change', function () {

            $(this).closest('.element').find('[name=errFeesAmount]').text('');
            $(this).closest('.element').find('[name=errorMonth]').text('');
            $(this).closest('.element').find('[name=errorAcademicYear]').text('');
        })
    } else {
        $(obj).closest('.element').find("span[name='errFeesAmount']").html("Please enter amount.");
        $(obj).closest('.element').find("span[name='errorMonth']").html("Please select month.");
        $(obj).closest('.element').find("span[name='errorAcademicYear']").html("Please select academic year.");
    }
    GetMonthArrayList(maincount);
    GetAcademicYearArrayList(maincount);
    maincount++;
    /* $(".fa-plus")[0].remove();*/
}
function insertAfter(referenceNode, newNode) {
    referenceNode.parentNode.insertBefore(newNode, referenceNode.nextSibling);
}
function repeatHTML(j) {
    debugger;
    var strHTML = '';
    varIndex = $(".wrapper .element").length + 1;
    //strHTML = strHTML + '<input type="hidden" id="inthdnPreviousQty_'+j+'" name="PreviousQty">';
    strHTML = strHTML + '<div class="col-md-12">';
    strHTML = strHTML + '<span name="errDetail" class="valid-er" style="text-align: center;font-size: 14px;"></span>';
    strHTML = strHTML + '</div>';
    strHTML = strHTML + '<div class="col-md-1" style=" float: left; text-align: center; width: 11%;">';
    strHTML = strHTML + '<span class="crcel" style="color:black">' + varIndex + '</span>';
    strHTML = strHTML + '</div>';

    strHTML = strHTML + '<div class="col-md-4" style="float:left;padding-left: 20px">';
    strHTML = strHTML + ' <select class="form-control form-control-alternative" style="width: 65%" id="MonthId_' + j + '" name ="MonthId" ><option value="0" >Select Month</option></select><span name = "errorItem" class="valid-er"></span >';
    strHTML = strHTML + '</div>';

    strHTML = strHTML + '<div class="col-md-4" style="float:left;margin-left: -70px">';
    strHTML = strHTML + ' <select class="form-control form-control-alternative" style="width: 65%" id="AcademicYearId_' + j + '" name ="AcademicYearId"><option value="0" >Select Academic Year</option></select><span name = "errorItem" class="valid-er" ></span >';
    strHTML = strHTML + '</div>';

    strHTML = strHTML + '<div class="col-md-2" style="float: left; width: 22%; margin-left: -73px;">';
    strHTML = strHTML + '<input id="FeesAmount" name="FeesAmount" placeholder="Enter Amount" class="form-control form-control-alternative count" type="text" maxlength="10" onkeypress = "return onlyNumbers(event)"><span name = "errFeesAmount" class="valid-er"></span >';
    strHTML = strHTML + '</div>';
    
    strHTML = strHTML + '<div class="col-md-1" style="float: right;margin-right:27px">';
    strHTML = strHTML + '<i class="fa fa-plus text-success i-plus" style="font-size: 25px" onclick="addDiv(\'AddMore\', this)"></i> &nbsp;';
    strHTML = strHTML + '<i class="fa fa-trash-o" style="color: red;font-size: 25px;" onclick="removeDiv(this)">';
    strHTML = strHTML + '</div>';

    return strHTML;
}

function removeDiv(obj) {
    var rowcount = $('.count').length;
    if (rowcount > 1) {

        var fistChildID = $(".wrapper .element:first-child").attr("id");
        if (confirm("Are you sure you want to remove fees detail?")) {
            var Parentobj = obj.parentElement.parentElement;
            var rootobj = Parentobj.parentElement;

            var PreviosEle = $(Parentobj).prev();
            var NextEle = $(Parentobj).next();
            if ($(PreviosEle).attr("id") === fistChildID) {
                if ($(".wrapper .element").length === 2) {
                    var button = $(PreviosEle).find('button');
                    $(button).empty();
                    $(button).append('<i class="fa fa-plus i-plus" style=""></i>');
                    $(button).attr("onclick", "addDiv(\'AddMore\', this)");
                    $(button).attr("style", "");
                }
            }
            else if ($(Parentobj).attr("id") === fistChildID) {
                if ($(".wrapper .element").length === 2) {
                    var button = $(NextEle).find('.glyphicon-minus').parent();
                    button.remove();
                }
            }
            else {
                var button = $(PreviosEle).find('button');
                $(button).before('<button type="button" class="clone" onclick="addDiv(\'AddMore\', this)"><i class="fa fa-plus i-plus" style=""></i></button>');
            }
            rootobj.removeChild(Parentobj);
            $(".wrapper .element").each(function (index) {
                $(this).attr("id", "element_" + (index + 1))
                $(this).find('.crcel').text(index + 1);
            })
            /*CalAmount(obj);*/
        }
        if ($('.count').length === 1) {
            $('.i-plus').show();
        }
    }
    else {
        alert("Can't delete, At least one Item required");
    }
}

var arrayMonthList = [];
var arrayAcademicYearList = [];

function GetMonthArrayList(k, SelectedValue) {
    debugger;

    var strHTML = '<option value="0">Select Month</option>';
    if (arrayMonthList !== null && arrayMonthList.length > 0) {
        for (var i = 0; i < arrayMonthList.length; i++) {
            var item = arrayMonthList[i];
            if (SelectedValue === item.Id) {
                strHTML = strHTML + '<option value="' + item.Id + '" selected>' + item.MonthName + '</option>';
            }
            else {
                strHTML = strHTML + '<option value="' + item.Id + '">' + item.MonthName + '</option>';
            }
        }
    }
    else {
        strHTML = strHTML + '<style="text-align: center;" >' + 'No Records found' + '</td>';
    }
    if (k === 0) {
        $("#MonthId_0").empty();
        $('#MonthId_0').append(strHTML);
    }
    else {
        $("#MonthId_" + k + "").empty();
        $('#MonthId_' + k + '').append(strHTML);
    }
}

function GetAcademicYearArrayList(k, SelectedValue) {
    debugger;

    var strHTML = '<option value="0">Select Academic Year</option>';
    if (arrayAcademicYearList !== null && arrayAcademicYearList.length > 0) {
        for (var i = 0; i < arrayAcademicYearList.length; i++) {
            var item = arrayAcademicYearList[i];
            if (SelectedValue === item.AcademicYearId) {
                strHTML = strHTML + '<option value="' + item.AcademicYearId + '" selected>' + item.AcademicYear + '</option>';
            }
            else {
                strHTML = strHTML + '<option value="' + item.AcademicYearId + '">' + item.AcademicYear + '</option>';
            }
        }
    }
    else {
        strHTML = strHTML + '<style="text-align: center;" >' + 'No Records found' + '</td>';
    }
    if (k === 0) {
        $("#AcademicYearId_0").empty();
        $('#AcademicYearId_0').append(strHTML);
    }
    else {
        $("#AcademicYearId_" + k + "").empty();
        $('#AcademicYearId_' + k + '').append(strHTML);
    }
}

function GetMonthList(k, SelectedValue) {
    debugger;
    var cls = {
    }
    $.ajax({
        url: '/Common/GetMonth',
        contentType: "application/json; charset=utf-8",
        type: "GET",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {
            debugger
            if (data !== null) {
                var strHTML = '<option value="0">Select Month</option>';
                if (data !== null && data.LSTMonthList.length > 0) {
                    arrayMonthList = data.LSTMonthList;
                    for (var i = 0; i < data.LSTMonthList.length; i++) {
                        var item = data.LSTMonthList[i];
                        if (SelectedValue === item.Id) {
                            strHTML = strHTML + '<option value="' + item.Id + '" selected>' + item.MonthName + '</option>';
                        }
                        else {
                            strHTML = strHTML + '<option value="' + item.Id + '">' + item.MonthName + '</option>';
                        }
                    }
                }
                else {
                    strHTML = strHTML + '<style="text-align: center;" >' + 'No Records found' + '</td>';
                }
                if (k === 0) {
                    $("#MonthId_0").empty();
                    $('#MonthId_0').append(strHTML);
                }
                else {
                    $("#MonthId_" + k + "").empty();
                    $('#MonthId_' + k + '').append(strHTML);
                }
            }
        }
    })
}

function GetAcademicYearList(k, SelectedValue) {
    debugger;
    var cls = {
    }
    $.ajax({
        url: '/Common/GetAcademicYear',
        contentType: "application/json; charset=utf-8",
        type: "GET",
        data: JSON.stringify({
            cls: cls
        }),
        success: function (data) {
            debugger
            if (data !== null) {
                var strHTML = '<option value="0">Select Academic Year</option>';
                if (data !== null && data.LSTAcademicYearList.length > 0) {
                    arrayAcademicYearList = data.LSTAcademicYearList;
                    for (var i = 0; i < data.LSTAcademicYearList.length; i++) {
                        var item = data.LSTAcademicYearList[i];
                        if (SelectedValue === item.AcademicYearId) {
                            strHTML = strHTML + '<option value="' + item.AcademicYearId + '" selected>' + item.AcademicYear + '</option>';
                        }
                        else {
                            strHTML = strHTML + '<option value="' + item.AcademicYearId + '">' + item.AcademicYear + '</option>';
                        }
                    }
                }
                else {
                    strHTML = strHTML + '<style="text-align: center;" >' + 'No Records found' + '</td>';
                }
                if (k === 0) {
                    $("#AcademicYearId_0").empty();
                    $('#AcademicYearId_0').append(strHTML);
                }
                else {
                    $("#AcademicYearId_" + k + "").empty();
                    $('#AcademicYearId_' + k + '').append(strHTML);
                }
            }
        }
    })
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


function ClearData() {

        $("#ClassId").val('0').trigger('change');
        $('#errClass').html("");
        $("#SectionId").val('0').trigger('change');
        $('#errSection').html("");
        $("#StudentId").val('0').trigger('change');
        $('#errStudent').html("");
        document.getElementById('RollNo').value = "";
        $('#errRollNo').html("");
        document.getElementById('Date').value = "";
        $('#errDate').html("");
        if (Id === "0") {

            document.getElementById('hdnintId').value = "0";
            document.getElementById('btnAdd').innerHTML = "Save";
            $("#btnAdd").attr('title', 'Save');
        }
        else {
            document.getElementById('btnAdd').innerHTML = "Update";
            $("#btnAdd").attr('title', 'Update');
        }
    }
