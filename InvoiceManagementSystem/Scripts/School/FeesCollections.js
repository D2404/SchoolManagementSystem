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
    $('#FilterDiv').hide();
    GetFeesCollectionList(1);
    GetClassRoom();
    GetMonthList(count, 0);
    CalTotal();
    var currentDate = new Date().toISOString().slice(0, 10);
    document.getElementById('Date').value = currentDate;
    var monthDropDown = $('.MonthId');
    if (monthDropDown.length > 0) {

        ;
        for (var i = 0; i < monthDropDown.length; i++) {
            GetMonthList(i, $('#hdnMonthId_' + i).val());
        }
    }
 });
function CalTotal() {
    
        var SubTotal = 0;
        $("input[id*='FeesAmount']").each(function () {
            SubTotal = SubTotal + ((this.value == undefined || this.value == "") ? 0 : parseFloat(this.value));
        });
        $("#SubTotal").val(parseFloat(SubTotal).toFixed(2));
    }

function GetFeesCollectionList(page) {
    
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
        url: '/FeesCollection/GetFeesCollectionList',
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
    var RollNo = $('#RollNo').text();
    if (RollNo === '' || RollNo === 0 || RollNo === '0') {
        $("#errRollNo").html("Please enter roll no.");
        val = false;
    }
    var Date = $('#Date').val();
    if (Date === '' || /\S/.test(Date) === false) {
        $("#errDate").html("Please select date.");
        val = false;
    }
    if (val === false) {
        return;
    }
}

function ValidateData(IsSubmit) {
    
    //ShowWait();
    $("#divSucess").hide();
    $("#msgIdSucess").text("");
    $("#divError").hide();
    $("#msgIdError").text("");

    var Flag = 0;

    var Id = $('#hdnintId').val();
    var ClassId = $('#ClassId').val();
    if (ClassId === 0 || ClassId === '0') {
        $("#errClass").html("Please select classroom.");
        Flag = 1;
    }
    var SectionId = $('#SectionId').val();
    if (SectionId === 0 || SectionId === '0') {
        $("#errSection").html("Please select section.");
        Flag = 1;
    }
    var StudentId = $('#StudentId').val();
    if (StudentId === 0 || StudentId === '0') {
        $("#errStudent").html("Please select student.");
        Flag = 1;
    }
    var RollNo = $('#RollNo').text();
    if (RollNo === '' || RollNo === 0 || RollNo === '0') {
        $("#errRollNo").html("Please enter roll no.");
        Flag = 1;
    }
    var Date = $('#Date').val();
    if (Date === '' || /\S/.test(Date) === false) {
        $("#errDate").html("Please select date.");
        Flag = 1;
    }
    if (IsSubmit == 1) {
        
        var i = 0;
        $('.wrapper').find('.element').each(function () {
            var MonthId = $(this).find("select[id*='MonthId']").val();
            var FeesAmount = $(this).find("input[id*='FeesAmount']").val();
            var DataError = 1;
            
            if (MonthId == undefined || MonthId == "" || MonthId == "0") {
                $(this).find("span[name='errorMonth']").html("Please select Month.");
                DataError = 0;
            }
            else {
                $(this).find("span[name='errorMonth']").html("");
                //DataError = 1;
            }


            if (FeesAmount == undefined || FeesAmount == "" || FeesAmount == "0") {
                $(this).find("span[name='errorFeesAmount']").html("Please enter FeesAmount.");
                DataError = 0;
            }
            else {
                $(this).find("span[name='errorFeesAmount']").html("");
                //DataError = 1;
            }

            if (DataError == 0) {
                ;
                Flag = 1;
            }
            i++;
        });
    }
    if (Flag == 1) {
        //HideWait();
        return false;
    }
    else {
        //HideWait();
        return true;
    }
}



function GetClassRoom() {
    var cls = {};

    $.ajax({
        url: '/Common/GetClassRoom',
        contentType: "application/json; charset=utf-8",
        type: "GET",
        data: JSON.stringify({ cls: cls }),
        success: function (data) {
            var html = '<option value="0" selected>Select ClassRoom</option>';
            
            for (var i = 0; i < data.LSTClassRoomList.length; i++) {
                html += '<option value="' + data.LSTClassRoomList[i].Id + '">' + data.LSTClassRoomList[i].ClassNo + '</option>';
            }

            $("#ClassId").empty().append(html);
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
        url: '/FeesCollection/LoadRollNo?StudentId=' + StudentId,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            
            // Assuming data contains an object with a RollNo property
            if (data) {
                $('#RollNo').val(data.RollNo);
            } else {
                $('#RollNo').val('');
            }
            HideWait();
        },
        error: function () {
            HideWait();
            alert("Failed to load Roll Number!");
        }
    });
}

function addDiv(id, obj) {
    
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


    //if ($("#AcademicYearId_0").val() === "0") {
    //    $("#errorAcademicYear").html("Please enter academic year.");
    //    NewDiv = 0;
    //}
    if (NewDiv === 1) {
        var fistChildID = $(".wrapper .element:first-child").attr("id");
        $(obj).closest('.element').find("span[name='errFeesAmount']").html("");
        $(obj).closest('.element').find("span[name='errorMonth']").html("");

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
    maincount++;
    /* $(".fa-plus")[0].remove();*/
}
function insertAfter(referenceNode, newNode) {
    referenceNode.parentNode.insertBefore(newNode, referenceNode.nextSibling);
}
function repeatHTML(j) {
    
    var strHTML = '';
    varIndex = $(".wrapper .element").length + 1;
    //strHTML = strHTML + '<input type="hidden" id="inthdnPreviousQty_'+j+'" name="PreviousQty">';
    strHTML = strHTML + '<div class="col-md-12">';
    strHTML = strHTML + '<span name="errDetail" class="valid-er" style="text-align: center;font-size: 14px;"></span>';
    strHTML = strHTML + '</div>';
    strHTML = strHTML + '<div class="col-md-1" style=" float: left; text-align: center; width: 16%;">';
    strHTML = strHTML + '<span class="crcel" style="color:black">' + varIndex + '</span>';
    strHTML = strHTML + '</div>';

    strHTML = strHTML + '<div class="col-md-4" style="float:left;margin-left: 69px">';
    strHTML = strHTML + ' <select class="form-control form-control-alternative" style="width: 65%" id="MonthId_' + j + '" name ="MonthId" ><option value="0" >Select Month</option></select><span name = "errorItem" class="valid-er"></span >';
    strHTML = strHTML + '</div>';

    //strHTML = strHTML + '<div class="col-md-4" style="float:left;margin-left: -62px">';
    ///*strHTML = strHTML + '  <input id="AmountDate" name="AmountDate" class="form-control form-control-alternative" type="date"style="width:65%"><span name = "errorItem" class="valid-er" ></span >';*/
    //strHTML = strHTML + '</div>';

    strHTML = strHTML + '<div class="col-md-2" style="float: left; width: 22%; margin-left: -62px;">';
    strHTML = strHTML + '<input id="FeesAmount" name="FeesAmount" placeholder="Enter Amount" class="form-control form-control-alternative count" type="text" maxlength="5" onkeypress = "return onlyNumbers(event)" onblur="CalTotal(this);"><span name = "errFeesAmount" class="valid-er"></span >';
    strHTML = strHTML + '</div>';
    
    strHTML = strHTML + '<div class="col-md-1" style="float: right;margin-right:69px">';
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

function GetMonthArrayList(k, SelectedValue) {

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


function GetMonthList(k, SelectedValue) {
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
