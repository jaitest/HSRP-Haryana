// Our common js


function ValidRegNo(textBoxAuthrizationNo, AllowedRTO) { 
    AllowedRTO = AllowedRTO.split(' ').join('')
    var SplitResult = AllowedRTO.split("/");
    var ii = 0; 
    
    for (var i = 0; i < SplitResult.length; i++) {
        if (SplitResult[i].length == 3) {
            var kk = textBoxAuthrizationNo.substring(0, 3);
            if (kk.toLowerCase() == SplitResult[i].toLowerCase()) {
                ii = 1;
                break;
            }
        }
        else if (SplitResult[i].length == 4) {
             kk = textBoxAuthrizationNo.substring(0, 4);
             if (kk.toLowerCase() == SplitResult[i].toLowerCase()) {
                ii = 1;
                break;
            }
        }
    } 
    if (ii != 1) {
        //alert("Please Provide Valid Registration No.");
        return false;
    }
    else {
        return true; 
    }
}

function invalidChar(_objData1) {
   // debugger;
    var iChars = '@!$%^*+=[]{}|"<>?';

    for (var i = 0; i < _objData1.value.length; i++) {
        if (iChars.indexOf(_objData1.value.charAt(i)) != -1) {
            return true;
        }
    }
}


function invalidChar1(_objData1) {
    // debugger;
    var iChars = '@!$%^+=[]{}|"<>?';

    for (var i = 0; i < _objData1.value.length; i++) {
        if (iChars.indexOf(_objData1.value.charAt(i)) != -1) {
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