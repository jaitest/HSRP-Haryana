// Our common js
function invalidChar(_objData1) {
    var iChars = "!$%^&*+=[]{}|\"<>?";

    for (var i = 0; i < _objData1.value.length; i++) {
        if (iChars.indexOf(_objData1.value.charAt(i)) != -1) {
            return true;
        }
    }
}

function isSpclChar() {
    var iChars = "!@#$%^&*()+=-[]\\\';,./{}|\":<>?";
    for (var i = 0; i < document.getElementById("<%=TextBoxVehicalRegNo.ClientID%>").value.length; i++) {
        if (iChars.indexOf(document.getElementById("<%=TextBoxVehicalRegNo.ClientID%>").value.charAt(i)) != -1) {
            alert("Please Provide Vehicle Registration Number.");

            return false;
        }
        else {
            return true;
        }
    }
}

function isNumberKey(evt) {
    //debugger;
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return true;
}



function emailcheck(str) {

    var at = "@"
    var dot = "."
    var lat = str.indexOf(at)
    var lstr = str.length
    var ldot = str.indexOf(dot)
    if (str.indexOf(at) == -1) {
        alert("Invalid E-mail ID")
        return false
    }

    if (str.indexOf(at) == -1 || str.indexOf(at) == 0 || str.indexOf(at) == lstr) {
        alert("Invalid E-mail ID")
        return false
    }

    if (str.indexOf(dot) == -1 || str.indexOf(dot) == 0 || str.indexOf(dot) == lstr) {
        alert("Invalid E-mail ID")
        return false
    }

    if (str.indexOf(at, (lat + 1)) != -1) {
        alert("Invalid E-mail ID")
        return false
    }

    if (str.substring(lat - 1, lat) == dot || str.substring(lat + 1, lat + 2) == dot) {
        alert("Invalid E-mail ID")
        return false
    }

    if (str.indexOf(dot, (lat + 2)) == -1) {
        alert("Invalid E-mail ID")
        return false
    }

    if (str.indexOf(" ") != -1) {
        alert("Invalid E-mail ID")
        return false
    }

    return true
}