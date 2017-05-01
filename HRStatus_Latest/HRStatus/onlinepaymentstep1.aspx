<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<!-- saved from url=(0062)http://180.151.100.243/hronlinepayment/onlinepaymentstep1.aspx -->
<html xmlns="http://www.w3.org/1999/xhtml"><head id="Head1"><meta http-equiv="Content-Type" content="text/html; charset=UTF-8"><title>

</title><link href="./onlinepaymentstep1_files/main_new.css" rel="stylesheet" type="text/css">
    <script type="text/javascript" language="javascript">
        function ValidateForm() {
            if (document.getElementById('dropDownListHSRPState').value == '' || document.getElementById('dropDownListHSRPState').value == '--Select State--') {
                alert('Select State..');
                document.getElementById('dropDownListHSRPState').focus();
                return false;
            }
            if (document.getElementById('dropDownListRTOLocation').value == '' || document.getElementById('dropDownListRTOLocation').value == '--Select RTO--') {
                alert('Select RTO..');
                document.getElementById('dropDownListRTOLocation').focus();
                return false;
            }
            if (document.getElementById('TextBoxAuthNo').value == '') {
                alert('Enter Authrization Slip No..');
                document.getElementById('TextBoxAuthNo').focus();
                return false;
            }
            if (document.getElementById('txtRegistrationNo').value == '') {
                alert('Enter Registration No..');
                document.getElementById('txtRegistrationNo').focus();
                return false;
            }
            if (document.getElementById('DropDownListVehicleClass').value == '' || document.getElementById('DropDownListVehicleClass').value == '--Select Vehicle Class--') {
                alert('Select Vehilce Class..');
                document.getElementById('DropDownListVehicleClass').focus();
                return false;
            }


            if (document.getElementById('ddlVehicleClass').value == '' || document.getElementById('ddlVehicleClass').value == '--Select Vehicle Model--') {
                alert('Select Vehilce Class..');
                document.getElementById('ddlVehicleClass').focus();
                return false;
            }
            if (document.getElementById('DropDownListOrderType').value == '' || document.getElementById('DropDownListOrderType').value == '--Select Order Type--') {
                alert('Select Order Type..');
                document.getElementById('DropDownListOrderType').focus();
                return false;
            }



            if (document.getElementById('txtChassisNo').value == '') {
                alert('Enter Chassis No..');
                document.getElementById('txtChassisNo').focus();
                return false;
            }

            if (document.getElementById('txtEngineNo').value == '') {
                alert('Enter Engine No..');
                document.getElementById('txtEngineNo').focus();
                return false;
            }

            if (document.getElementById('txtOwnerName').value == '') {
                alert('Enter The Name Of Owner.');
                document.getElementById('txtOwnerName').focus();
                return false;
            }
            if (document.getElementById('txtAddress').value == '') {
                alert('Enter Owner Address.');
                document.getElementById('txtAddress').focus();
                return false;
            }
            if (document.getElementById('txtPhone').value == '') {
                alert('Enter Phone No.');
                document.getElementById('txtPhone').focus();
                return false;
            }
            if (document.getElementById('txtAmount').value == '') {
                alert('Enter Amount.');
                document.getElementById('txtAmount').focus();
                return false;
            }
            if (document.getElementById('txtTAX').value == '') {
                alert('Enter TAX Amount.');
                document.getElementById('txtTAX').focus();
                return false;
            }

            if (confirm("Are you really want to Continue?")) {
                return true;
            }
            else {
                return false;
            }
        }

        function isNumberKey(evt) {
            //debugger;
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 46 || charCode > 57))
                return false;

            return true;
        }
    </script>
</head>
<body>
    <form method="post" action="./onlinepaymentstep1_files/onlinepaymentstep1.aspx" id="f">
<div class="aspNetHidden">
<input type="hidden" name="__EVENTTARGET" id="__EVENTTARGET" value="">
<input type="hidden" name="__EVENTARGUMENT" id="__EVENTARGUMENT" value="">
<input type="hidden" name="__VIEWSTATE" id="__VIEWSTATE" value="/wEPDwUKLTYzMDczMTAzNQ9kFgICAw9kFgYCEw8QZGQWAWZkAhkPEGRkFgFmZAIbDxBkZBYBZmRkFsLmgUpaQ7VmCK8g5cLn1Q53FpQnn4sWaScTcHjj6A8=">
</div>

<script type="text/javascript">
//<![CDATA[
var theForm = document.forms['f'];
if (!theForm) {
    theForm = document.f;
}
function __doPostBack(eventTarget, eventArgument) {
    if (!theForm.onsubmit || (theForm.onsubmit() != false)) {
        theForm.__EVENTTARGET.value = eventTarget;
        theForm.__EVENTARGUMENT.value = eventArgument;
        theForm.submit();
    }
}
//]]>
</script>


<script src="./onlinepaymentstep1_files/WebResource.axd" type="text/javascript"></script>


<script src="./onlinepaymentstep1_files/ScriptResource.axd" type="text/javascript"></script>
<script type="text/javascript">
//<![CDATA[
if (typeof(Sys) === 'undefined') throw new Error('ASP.NET Ajax client-side framework failed to load.');
//]]>
</script>

<script src="./onlinepaymentstep1_files/ScriptResource(1).axd" type="text/javascript"></script>
<div class="aspNetHidden">

	<input type="hidden" name="__EVENTVALIDATION" id="__EVENTVALIDATION" value="/wEdACP2nO+iMaDLt2bVg07TONHMvocZFRKeUj0k0/fDYFUio834O/GfAV4V4n0wgFZHr3dTjNiM3ZfWtA1tFTZ+neecoE3iqfSGGwg6CHfbWXS77ffheCAX+6BTwA8/K95A9l9kERVKX2y5bZ6spKcXMOQl4A04773UApB4CmJQUYwpwxwFFXHxyI7NAZ9pgU3ca7bTP1lmzJfsGflOW84qjkn1MAx2w1nvQmZgNJukJw5SXb93DxyPsDbY5WIlb+94K4VA5oGIZ/tScW23GdMU7TjhX+j/MGaj+ixWbL12ZnqPZbILRdPnNLHRPyF06cUnDGmH7K4HYWLI4SHFhfBr0ZesH620hJ8Oe2N82TwLZFLbgAReC0WJ/vw2ce6bH+boxf27Fka7enL5CVoSr0OsBf+Trt4UTLX4nD/wrizx2v5DNxOY6vIVC/r/aGlFk75ji431V1W3re0Y7x80mCYAPf7nTpHwbaS4l/t3X6QEMDwCkox7CN5aMIG0vohotszmsOTscYYPpLv/BaVTLpOpJQCWp185BCcBI41H/gtuHnyOoY0+buBhfMExwwWZ1/81DbKaj50dSVxh1rHEj9T9L1ld+e5K83a6MMtWKomgxI5ZB0MHZh9ZuPBRie2vA/HfoRIfcRfNzpAmcbmdbysoE+5vD6m1zchzmxd4I9LljDI0JyMpzVXWcCyMjOk0s7nnzaV3wFxYYfWi3STRI7sJ+66UJJzFv7aOLOx3rWDaxxbOKQbF21udFMknraXaj2ywfp7g4acqlYtlWZuOFc8Olmy7">
</div>

    <script type="text/javascript">
//<![CDATA[
Sys.WebForms.PageRequestManager._initialize('ScriptManager1', 'f', [], [], [], 90, '');
//]]>
</script>

  
 
    <table width="80%" border="0" align="center" cellpadding="0" cellspacing="0">
        <tbody><tr>
            <td>
                &nbsp;
            </td>
            <td width="1024">
                <table width="1024" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tbody><tr>
                        <td valign="top">
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" bgcolor="#f4f2f2">
                                <tbody><tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="99%" border="0" align="center" cellpadding="3" cellspacing="0" class="table_tb">
                                            <tbody><tr>
                                                <td>
                                                    <table width="99%" border="0" cellpadding="3" cellspacing="0" class="table_t">
                                                        <tbody><tr>
                                                            <td>
                                                                <h2>
                                                                    Step-1:Please fill Vehicle info for
                                                                    Online Payment of HSRP</h2>
                                                            </td>
                                                        </tr>
                                                    </tbody></table>
                                                    <table width="99%" border="0" cellpadding="3" cellspacing="2" class="table_tb2">
                                                        <tbody><tr>
                                                            <td>
                                                                <div>
                                                                    
                                                .
                                            <table width="73%">
                                                                        
                                                                        <tbody><tr>
                                                                           <td class="normal_text" nowrap="nowrap">
                                                                                <span id="Label3">Registration No:</span>
                                                                                <span id="Label16" style="color:Red;font-size:12pt;font-weight:bold;">*</span>
                                                                            </td>
                                                                            <td>
                                                                                <input name="txtRegistrationNo" type="text" id="txtRegistrationNo" class="text_box">
                                                                                    &nbsp;
                                                                                <input type="submit" name="Button1" value="Go" id="Button1">
                                                                            </td>
                                                                            
                                                                        </tr>
                                                                        <tr>
                                                                         <td class="normal_text" nowrap="nowrap">
                                                                                <span id="Label2">Authorization Slip No:</span>
                                                                                <span id="Label25" style="color:Red;font-size:12pt;font-weight:bold;">*</span>
                                                                            </td>
                                                                            <td nowrap="nowrap">
                                                                                <input name="TextBoxAuthNo" type="text" readonly="readonly" id="TextBoxAuthNo" class="text_box">
                                                                            
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                <td align="left" class="normal_text" style="width: 12.8%">
                                                    Vehicle Class :<span id="Label1" style="color:Red;font-size:12pt;font-weight:bold;">*</span>
                                                </td>
                                                <td>
                                                    


                                                            <select name="DropDownListVehicleClass" onchange="javascript:setTimeout(&#39;__doPostBack(\&#39;DropDownListVehicleClass\&#39;,\&#39;\&#39;)&#39;, 0)" id="DropDownListVehicleClass" disabled="disabled" tabindex="12" class="aspNetDisabled text_box2" style="width:209px;margin-left: 2px">
	<option selected="selected" value="--Select Vehicle Class--">--Select Vehicle Class--</option>
	<option value="Transport">Transport (Commercial)</option>
	<option value="Non-Transport">Non-Transport (Private)</option>

</select>
                                                       
                                                </td>
                                                </tr>
                                                                        <tr>
                                                                            <td class="normal_text">
                                                                                <span id="Label4">Vehicle Type:</span>
                                                                                <span id="Label17" style="color:Red;font-size:12pt;font-weight:bold;">*</span>
                                                                            </td>
                                                                            <td>
                                                                                <select name="ddlVehicleClass" id="ddlVehicleClass" disabled="disabled" class="aspNetDisabled text_box2">
	<option selected="selected" value="--Select Vehicle Model--">--Select Vehicle Model--</option>
	<option value="SCOOTER">SCOOTER</option>
	<option value="MOTOR CYCLE">MOTOR CYCLE</option>
	<option value="TRACTOR">TRACTOR</option>
	<option value="THREE WHEELER">THREE WHEELER</option>
	<option value="LMV">LMV</option>
	<option value="LMV(CLASS)">LMV(CLASS)</option>
	<option value="MCV/HCV/TRAILERS">MCV/HCV/TRAILERS</option>

</select>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                <td class="normal_text" nowrap="nowrap" align="left" style="width: 13.4%">
                                                    Order Type :
                                                </td>
                                                <td align="left" width="193px">
                                                    
                                                    
                                                           
                                                            <select name="DropDownListOrderType" onchange="javascript:setTimeout(&#39;__doPostBack(\&#39;DropDownListOrderType\&#39;,\&#39;\&#39;)&#39;, 0)" id="DropDownListOrderType" disabled="disabled" tabindex="20" class="aspNetDisabled text_box2" style="margin-left: 3px">
	<option selected="selected" value="--Select Order Type--">--Select Order Type--</option>
	<option value="NB">NEW BOTH PLATES</option>
	<option value="OB">OLD BOTH PLATES</option>
	<option value="DB">DAMAGED BOTH PLATES</option>
	<option value="DF">DAMAGED FRONT PLATE</option>
	<option value="DR">DAMAGED REAR PLATE</option>
	<option value="OS">ONLY STICKER</option>

</select>
                                                           
                                                        
                                                </td></tr>
                                                                        <tr>
                                                                            <td class="normal_text">
                                                                                <span id="Label5">Chassis No:</span>
                                                                                <span id="Label18" style="color:Red;font-size:12pt;font-weight:bold;">*</span>
                                                                            </td>
                                                                            <td>
                                                                                <input name="txtChassisNo" type="text" readonly="readonly" id="txtChassisNo" class="text_box">
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="normal_text">
                                                                                <span id="Label6">Engine No:</span>
                                                                                <span id="Label19" style="color:Red;font-size:12pt;font-weight:bold;">*</span>
                                                                            </td>
                                                                            <td>
                                                                                <input name="txtEngineNo" type="text" readonly="readonly" id="txtEngineNo" class="text_box">
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="normal_text">
                                                                                <span id="Label8">Owner Name:</span>
                                                                                <span id="Label20" style="color:Red;font-size:12pt;font-weight:bold;">*</span>
                                                                            </td>
                                                                            <td>
                                                                                <input name="txtOwnerName" type="text" readonly="readonly" id="txtOwnerName" class="text_box" style="width:200px;">
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="normal_text">
                                                                                <span id="Label9">Address:</span>
                                                                                <span id="Label21" style="color:Red;font-size:12pt;font-weight:bold;">*</span>
                                                                            </td>
                                                                            <td>
                                                                                <textarea name="txtAddress" rows="2" cols="20" readonly="readonly" id="txtAddress" class="text_box3" style="width:200px;"></textarea>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="normal_text">
                                                                                <span id="Label10">Contact No:</span>
                                                                                <span id="Label22" style="color:Red;font-size:12pt;font-weight:bold;">*</span>
                                                                            </td>
                                                                            <td nowrap="nowrap">
                                                                                <input name="txtPhone" type="text" id="txtPhone" class="text_box" onkeypress="return isNumberKey(event)">
                                                                            &nbsp;(Please Verify)</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="normal_text">
                                                                                <span id="Label11">Email ID:</span>
                                                                            </td>
                                                                            <td nowrap="nowrap">
                                                                                <input name="txtEmail" type="text" id="txtEmail" class="text_box">
                                                                            &nbsp;(Please Verify)</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="normal_text">
                                                                                <span id="Label12">Amount:</span>
                                                                                <span id="Label23" style="color:Red;font-size:12pt;font-weight:bold;">*</span>
                                                                            </td>
                                                                            <td>
                                                                                <input name="txtAmount" type="text" readonly="readonly" id="txtAmount" class="text_box">
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="normal_text">
                                                                                <span id="Label13">Tax: (In %)</span>
                                                                                <span id="Label24" style="color:Red;font-size:12pt;font-weight:bold;">*</span>
                                                                            </td>
                                                                            <td>
                                                                                <input name="txtTAX" type="text" readonly="readonly" id="txtTAX" class="text_box">
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="normal_text">
                                                                                <span id="Label14">Net Amount:</span>
                                                                            </td>
                                                                            <td>
                                                                                <input name="txtNetAmount" type="text" readonly="readonly" id="txtNetAmount" class="text_box">
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                &nbsp;
                                                                                
                                                                            </td>
                                                                            <td align="right" style="padding-right: 50px;">
                                                                                <input type="submit" name="Submit" value="Submit Order And Pay Now" onclick="javascript:return ValidateForm();" id="Submit">
                                                                                </td>
                                                                                </tr>
                                                                   </tbody></table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        
                                                        <tr>
                                                            <td colspan="2" class="normal_text">
                                                                <span id="lblErr"></span>
                                                            </td>
                                                            <td>
                                                                <input type="hidden" name="H_regno" id="H_regno">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4" class="normal_text">
                                                                <h3>Please Note :</h3>
                                                                <b>1.</b>Only Those orders can be booked that have Valid Authorization Number from Transport Department.<br>
                                                                <b>2.</b>Although,you must have received the HSRP Authorization Number, it may not be available at the time of processing your registration. Confirmation email will be sent to you along with required access information upon completion of your order.<br>
                                                                <b>3.</b>Your order will be processed only upon receipt of payments. <br>
                                                                <b>4.</b>Please Verify Contact Number and Email ID.

                                                            </td>
                                                        </tr>
                                                    </tbody></table>                                                    
                                                </td>
                                            </tr>
                                        </tbody></table>
                                    </td>
                                </tr>
                            </tbody></table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </tbody></table>
            </td>
            <td>
            &nbsp;
            </td>
        </tr>
    </tbody></table>

    
    </form>


</body></html>