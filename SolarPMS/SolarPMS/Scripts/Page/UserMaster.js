/// <reference path="../Scripts/jquery-1.10.2.js" />


function enableDisableControl() {
    
    var authMode = $("#ContentPlaceHolder1_DrpAuthentication").val();
  
    if (authMode == "Database") {
        $("#ContentPlaceHolder1_TxtUserId").removeAttr("disabled");
        $("#ContentPlaceHolder1_TxtPassword").removeAttr("disabled");
        $("#ContentPlaceHolder1_ReqFldValUserID").enabled=false;
    }
    else {
        $("#ContentPlaceHolder1_TxtUserId").attr("disabled", "disabled");
        $("#ContentPlaceHolder1_TxtPassword").attr("disabled", "disabled");
    }
    
}

function callAjaxRequest(sender, rags) {
  
    __doPostBack();
}

function CancelPopUp()
{
    debugger;
    if (confirm("Entered data will be lost. Do you want to continue?")) {
        return true;
    }
    else {
        return false;
    }
}

$("#ContentPlaceHolder1_DrpAuthentication").change(function () {
    alert("Test");
});

