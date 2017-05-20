

///>>> To Check Empty OF Field
function validate(eve) {

    if (document.getElementById('textBoxFirstName').value == "--Enter First Name--") {
        alert('Please Provide First Name');
        document.getElementById('textBoxFirstName').focus();
        return false;
    }
    else if (document.getElementById('textBoxCity').value == "--Enter City Name--") {
        alert('Please Provide City Name');
        document.getElementById('textBoxCity').focus();
        return false;
    }
    else if (document.getElementById('textBoxLastName').value == "--Enter Last Name--") {
        alert('Please Provide Last Name.');
        document.getElementById('textBoxLastName').focus();
        return false;
    }
    else if (document.getElementById('textBoxState').value == "--Enter State Name--") {
        alert("please Provide State.");
        document.getElementById('textBoxState').focus();
        return false;
    }
    else if (document.getElementById('textBoxLoginName').value == "--Enter Login Name--") {
        alert("please Provide Login Name.");
        document.getElementById('textBoxLoginName').focus();
        return false;
    }
    else if (document.getElementById('textBoxZip').value == "--Enter Zip--") {
        alert("please Provide Zip.");
        document.getElementById('textBoxZip').focus();
        return false;
    }
    else if (document.getElementById('textBoxPassword').value == "--Enter Password--") {
        alert("please Provide Password.");
        document.getElementById('textBoxPassword').focus();
        return false;
    }
    else if (document.getElementById('textBoxEmail').value == "--Enter Mail id--") {
        alert("please Provide Email Id.");
        document.getElementById('textBoxEmail').focus();
        return false;
    }
    else if (document.getElementById('textBoxConfirmPassword').value == "--Confirm Password--") {
        alert("please Provide Password Again.");
        document.getElementById('textBoxConfirmPassword').focus();
        return false;
    }
    else if (document.getElementById('textBoxMobile').value == "--Enter Mobile No--") {
        alert("please Provide Mobile Number.");
        document.getElementById('textBoxMobile').focus();
        return false;
    }
    else if (document.getElementById('DropDownListOrg').value == "--Select Organization--") {
        alert("please Select Organization.");
        document.getElementById('DropDownListOrg').focus();
        return false;
    }
    else if (document.getElementById('textBoxAddress1').value == "----Enter Address----") {
        alert("please Provide Address Detail.");
        document.getElementById('textBoxAddress1').focus();
        return false;
    }
    else
    { return true; }
}



//>>>> Email  Checker
function checkEmail() {
    var email = document.getElementById('textBoxEmail');
    var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    if (!filter.test(email.value)) {
        alert('Please provide a valid email address');
        email.value = '';
        email.focus();
        return false;
    }
    else {
        return true;
    }
}






function Mobile(element) {
    var regex = /^\d{10}$/;
    if (element.value.search(regex) == -1) {
        alert("please provide Ten digit Mobile Number only.");
        element.value = '';
        element.focus();
    }
}

function Hidebuttonapcashcollection()
{

    $('#Button3').hide();
    return true;
}


function Hidedownloadbuttonapcashcollection()
{
    $('#btnDownload').hide();
    return true;
}




