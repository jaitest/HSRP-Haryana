using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DataProvider;


namespace HSRP.Master
{
    public partial class PlateRejection : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string UserType = string.Empty;

        string HSRPRecordID = string.Empty;
        string Mode;
        string orderstatus;
        string frontLaser;
        string RearLaser;
        string UserID;

        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        string embossingcode=string.Empty;
        string AffixationCode=string.Empty;
        string strquery1 = string.Empty;
        string strquery2 = string.Empty;
        string strquery3 = string.Empty;
        string strquery4 = string.Empty;
        int intHSRPStateID;
        string trnasportname, pp;
        string transtr, statename1;
        string fontpath;
        string RTOid=string.Empty;
        string Recordid=string.Empty;
        string sqlstring = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {

            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                UserType = Session["UserType"].ToString();
                RTOLocationID = (Session["UserRTOLocationID"].ToString());

                HSRPStateID = (Session["UserHSRPStateID"].ToString());
            }
            if (!Page.IsPostBack)
            {
                
                Mode = Request.QueryString["Mode"];
                UserID = Session["UID"].ToString();

               
                

                //FilldropDownListHSRPState();
                //FilldropDownListRTOLocation();
            }
            //lblFLNo.Visible=false;
            //lblRLNo.Visible=false;
        }

        DataTable dt = new DataTable();
        //#region DropDown

        //private void FilldropDownListHSRPState()
        //{

        //    UserType = Session["UserType"].ToString();
        //    if (UserType == "0")
        //    {
        //        SQLString = "select HSRPStateName,HSRP_StateID from HSRPState where ActiveStatus='Y' Order by HSRPStateName";
        //        Utils.PopulateDropDownList(dropDownListHSRPState, SQLString.ToString(), CnnString, "--Select State--");
        //        dropDownListHSRPState.SelectedIndex = 0;
        //    }
        //    else
        //    {
        //        dropDownListHSRPState.Enabled = false;
        //        SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + Session["UserHSRPStateID"].ToString() + " and ActiveStatus='Y' Order by HSRPStateName";
        //        DataTable dss = Utils.GetDataTable(SQLString, CnnString);
        //        dropDownListHSRPState.DataSource = dss;
        //        dropDownListHSRPState.DataBind();

        //         FilldropDownListRTOLocation();
        //    }

        //}

        //private void FilldropDownListRTOLocation()
        //{
        //    UserType = Session["UserType"].ToString();
        //    if (dropDownListHSRPState.SelectedValue != "--Select State--")
        //    {
        //        if (UserType == "0")
        //        {

        //            SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + dropDownListHSRPState.SelectedValue.ToString() + " and ActiveStatus!='N' Order by RTOLocationName";
        //            //SQLString = "select RTOLocation.RTOLocationName,RTOLocation.RTOLocationID from RTOLocation inner join UserRTOLocationMapping on RTOLocation.RTOLocationID=UserRTOLocationMapping.RTOLocationID Where RTOLocation.HSRP_StateID=" + intHSRPStateID + " and UserRTOLocationMapping.UserID='" + strUserID + "' and ActiveStatus!='N' Order by RTOLocation.RTOLocationName";

        //        }
        //        else
        //        {
        //            dropDownListRTOLocation.Visible = true;
        //            labelRTOLocation.Visible = true;
        //            UpdateRTOLocation.Update();

        //           // SQLString = "select RTOLocation.RTOLocationName,RTOLocation.RTOLocationID from RTOLocation inner join UserRTOLocationMapping on RTOLocation.RTOLocationID=UserRTOLocationMapping.RTOLocationID Where RTOLocation.HSRP_StateID=" + dropDownListHSRPState.SelectedValue.ToString() + " and UserRTOLocationMapping.UserID='" + strUserID + "' and ActiveStatus!='N' Order by RTOLocation.RTOLocationName";
        //            string UserID = Convert.ToString(Session["UID"]);
        //            SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, (a.RTOLocationCode+', ') as RTOCode, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where UserRTOLocationMapping.UserID='" + UserID + "' ";

        //        }
        //        Utils.PopulateDropDownList(dropDownListRTOLocation, SQLString.ToString(), CnnString, "--Select RTO Location--");
        //        dropDownListRTOLocation.SelectedIndex = 0;
        //    }
        //}

        //protected void dropDownListHSRPState_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (dropDownListHSRPState.SelectedItem.Text != "--Select State--")
        //    {
        //        dropDownListRTOLocation.Visible = true;
        //        labelRTOLocation.Visible = true;
        //        FilldropDownListRTOLocation();
        //        UpdateRTOLocation.Update();
        //    }
        //    else
        //    {

        //        dropDownListHSRPState.Visible = false;
        //        labelRTOLocation.Visible = false;
        //        UpdateRTOLocation.Update();

        //    }
        //}


        //#endregion

        protected void buttonRejectedSave_Click(object sender, EventArgs e)
        {
            lblErrMess.Text = "";
            if (txtoperatorname.Text.Trim() == "")
            {
                lblErrMess.Text = "Opertator Name is blank....";
                return;
            }
            if (ddlEntryType.SelectedIndex<=0 || ddlEntryType.SelectedItem.ToString() == "")
            {
                lblErrMess.Text = "";
                lblErrMess.Text = "Please Select Entry Type";
                return;
            }
            if (ddlRejectType.SelectedItem.ToString() == "--Select Reject Type--" || ddlRejectType.SelectedItem.ToString() == "")
            {
                lblErrMess.Text = "";
                lblErrMess.Text = "Please select Reject Type";
                return;
            }


            sqlstring = "select * from rejectplatEntry where hsrp_stateid='" + HSRPStateID + "' and (FrontLaserCode='" + lblFLNo.Text + "' or RearLaserCode='" + lblRLNo.Text + "')";
            DataTable dtnew = Utils.GetDataTable(sqlstring, CnnString);
            if (dtnew.Rows.Count > 0)
            {
                lblErrMess.Text = "";
                lblErrMess.Text = "Duplicate Record";
                return;
            }

              sqlstring = "select * from hsrprecords where vehicleregno='" + txtVehicleNo.Text + "' and orderstatus='New Order' and (HSRP_Front_LaserCode=null and HSRP_Rear_LaserCode=null)";
             DataTable  dt=Utils.GetDataTable(sqlstring, CnnString);
             if (dt.Rows.Count > 0)
             {
                 lblErrMess.Text = "";
                 lblErrMess.Text = "Laser Code Not found";
                 return;
             }

             sqlstring = "select * from hsrprecords where vehicleregno='" + txtVehicleNo.Text + "' and orderstatus='Closed'";
             DataTable dt1 = Utils.GetDataTable(sqlstring, CnnString);
             if (dt1.Rows.Count > 0)
             {
                 lblErrMess.Text = "";
                 lblErrMess.Text = "Order is Already Closed";
                 return;
             }
            HiddenField4.Value = txtVehicleNo.Text.ToString();
            RTOid = HiddenField1.Value;
            Recordid = HiddenField2.Value;
          //  HSRPStateID = HiddenField3.Value;
            //strquery1 = "select NAVEMBID from rtolocation as a where RTOLocationID =a.RTOLocationID";
            strquery1 = "select NAVEMBID from rtolocation as a where a.RTOLocationID ='"+RTOid+"'";
            dt=Utils.GetDataTable(strquery1,CnnString);
            if (dt.Rows.Count > 0)
            {
                embossingcode = dt.Rows[0]["NAVEMBID"].ToString();
            }
           
           // strquery2 = "select ('AFX'+convert(varchar(15),RTOLocationID))  as [AffixationCenterCode] from rtolocation";
            strquery2 = "select ('AFX'+convert(varchar(15),RTOLocationID))  as [AffixationCenterCode] from rtolocation where RTOLocationID='"+RTOid+"'";
            dt = Utils.GetDataTable(strquery2, CnnString);
            if (dt.Rows.Count > 0)
            {
                AffixationCode = dt.Rows[0]["AffixationCenterCode"].ToString();
            }

            string updateqry = string.Empty;
            int result1;

            // updateqry = "update hsrprecords set HSRP_Front_LaserCode=null,HSRP_Rear_LaserCode=null where hsrp_stateid='" + HSRPStateID + "' and vehicleregno ='" + txtVehicleNo.Text + "'";
            // result1 = Utils.ExecNonQuery(updateqry, CnnString);

            // updateqry = "update rtoinventory set inventorystatus='Rejected',rejectdate=getdate(),remarks='"+ txtRemarks.Text +"' where hsrp_stateid='" + HSRPStateID + "' and laserno ='" + lblFLNo.Text  + "'";
            // result1 = Utils.ExecNonQuery(updateqry, CnnString);

            if (ddlEntryType.SelectedItem.Text == "Embossing" && ddlRejectType.SelectedItem.Text=="Both")
            {
                updateqry = "update hsrprecords set HSRP_Front_LaserCode=null,HSRP_Rear_LaserCode=null,orderstatus='New Order',RejectFlag='Y',orderembossingdate=null where hsrp_stateid='" + HSRPStateID + "' and vehicleregno ='" + txtVehicleNo.Text + "'";
                result1 = Utils.ExecNonQuery(updateqry, CnnString);

                updateqry = "update rtoinventory set inventorystatus='Rejected',rejectdate=getdate(),remarks='" + txtRemarks.Text + "' where hsrp_stateid='" + HSRPStateID + "' and laserno in ('" + lblFLNo.Text + "','"+ lblRLNo.Text +"')";
                result1 = Utils.ExecNonQuery(updateqry, CnnString);

                strquery3 = "insert into rejectplatEntry (EntryType,EntryDate,OriginalRequestID,RejectionType,FrontLaserCode,RearLaserCode,EmbosssingCenterCode,AffixationCenterCode,ReasonforRejection,OperatorName,ResponsibilityCenter,hsrp_stateid,rtolocationid) values('" + ddlEntryType.SelectedItem.ToString() + "',getdate(),'" + Recordid + "','" + ddlRejectType.SelectedItem.ToString() + "','" + lblFLNo.Text + "','" + lblRLNo.Text + "','" + embossingcode + "','" + AffixationCode + "','" + txtRemarks.Text + "','" + txtoperatorname.Text + "','" + RTOid + "','" + HSRPStateID + "','" + RTOid + "')";
                int result = Utils.ExecNonQuery(strquery3, CnnString);
                if (result > 0)
                {
                    lblSucMess.Text = "Record Updated Successfully.";
                    //blank();
                }
            }
            else if (ddlEntryType.SelectedItem.Text == "Embossing" && ddlRejectType.SelectedItem.Text == "Front Plate")
            {
                updateqry = "update hsrprecords set HSRP_Front_LaserCode=null,orderstatus='New Order',orderembossingdate=null,RejectFlag='Y' where hsrp_stateid='" + HSRPStateID + "' and vehicleregno ='" + txtVehicleNo.Text + "'";
                result1 = Utils.ExecNonQuery(updateqry, CnnString);

                updateqry = "update rtoinventory set inventorystatus='Rejected',rejectdate=getdate(),remarks='" + txtRemarks.Text + "' where hsrp_stateid='" + HSRPStateID + "' and laserno ='" + lblFLNo.Text + "'";
                result1 = Utils.ExecNonQuery(updateqry, CnnString);

                strquery3 = "insert into rejectplatEntry (EntryType,EntryDate,OriginalRequestID,RejectionType,FrontLaserCode,EmbosssingCenterCode,AffixationCenterCode,ReasonforRejection,OperatorName,ResponsibilityCenter,hsrp_stateid,rtolocationid) values('" + ddlEntryType.SelectedItem.ToString() + "',getdate(),'" + Recordid + "','" + ddlRejectType.SelectedItem.ToString() + "','" + lblFLNo.Text + "','" + embossingcode + "','" + AffixationCode + "','" + txtRemarks.Text + "','" + txtoperatorname.Text + "','" + RTOid + "','" + HSRPStateID + "','" + RTOid + "')";
                int result = Utils.ExecNonQuery(strquery3, CnnString);
                if (result > 0)
                {
                    lblSucMess.Text = "Record Updated Successfully.";
                    //blank();
                }
            }
            else if (ddlEntryType.SelectedItem.Text == "Embossing" && ddlRejectType.SelectedItem.Text == "Rear Plate")
            {
                updateqry = "update hsrprecords set HSRP_Rear_LaserCode=null,orderstatus='New Order',orderembossingdate=null,RejectFlag='Y' where hsrp_stateid='" + HSRPStateID + "' and vehicleregno ='" + txtVehicleNo.Text + "'";
                result1 = Utils.ExecNonQuery(updateqry, CnnString);

                updateqry = "update rtoinventory set inventorystatus='Rejected',rejectdate=getdate(),remarks='" + txtRemarks.Text + "' where hsrp_stateid='" + HSRPStateID + "' and laserno ='" + lblRLNo.Text + "'";
                result1 = Utils.ExecNonQuery(updateqry, CnnString);

                strquery3 = "insert into rejectplatEntry (EntryType,EntryDate,OriginalRequestID,RejectionType,RearLaserCode,EmbosssingCenterCode,AffixationCenterCode,ReasonforRejection,OperatorName,ResponsibilityCenter,hsrp_stateid,rtolocationid) values('" + ddlEntryType.SelectedItem.ToString() + "',getdate(),'" + Recordid + "','" + ddlRejectType.SelectedItem.ToString() + "','" + lblRLNo.Text + "','" + embossingcode + "','" + AffixationCode + "','" + txtRemarks.Text + "','" + txtoperatorname.Text + "','" + RTOid + "','" + HSRPStateID + "','" + RTOid + "')";
                int result = Utils.ExecNonQuery(strquery3, CnnString);
                if (result > 0)
                {
                    lblSucMess.Text = "Record Updated Successfully.";
                    //blank();
                }
            }
                //Affixation
            else if (ddlEntryType.SelectedItem.Text == "Affixation" && ddlRejectType.SelectedItem.Text == "Both")
            {
                updateqry = "update hsrprecords set HSRP_Front_LaserCode=null,HSRP_Rear_LaserCode=null,orderstatus='New Order',orderembossingdate=null,RejectFlag='Y' where hsrp_stateid='" + HSRPStateID + "' and vehicleregno ='" + txtVehicleNo.Text + "'";
                result1 = Utils.ExecNonQuery(updateqry, CnnString);

                updateqry = "update rtoinventory set inventorystatus='Rejected',rejectdate=getdate(),remarks='" + txtRemarks.Text + "' where hsrp_stateid='" + HSRPStateID + "' and laserno in ('" + lblFLNo.Text + "','" + lblRLNo.Text + "')";
                result1 = Utils.ExecNonQuery(updateqry, CnnString);

                strquery3 = "insert into rejectplatEntry (EntryType,EntryDate,OriginalRequestID,RejectionType,FrontLaserCode,RearLaserCode,EmbosssingCenterCode,AffixationCenterCode,ReasonforRejection,OperatorName,ResponsibilityCenter,hsrp_stateid,rtolocationid) values('" + ddlEntryType.SelectedItem.ToString() + "',getdate(),'" + Recordid + "','" + ddlRejectType.SelectedItem.ToString() + "','" + lblFLNo.Text + "','" + lblRLNo.Text + "','" + embossingcode + "','" + AffixationCode + "','" + txtRemarks.Text + "','" + txtoperatorname.Text + "','" + RTOid + "','" + HSRPStateID + "','" + RTOid + "')";
                int result = Utils.ExecNonQuery(strquery3, CnnString);
                if (result > 0)
                {
                    lblSucMess.Text = "Record Updated Successfully.";
                    //blank();
                }

                ////result1 = Utils.ExecNonQuery("insert into [HSRPRecords_Rejct] select * from [HSRPRecords] where hsrp_stateid='" + HSRPStateID + "' and vehicleregno ='" + txtVehicleNo.Text + "'", CnnString);
                //updateqry = "update hsrprecords set HSRP_Front_LaserCode=null,HSRP_Rear_LaserCode=null, orderstatus='New Order',orderembossingdate=null where hsrp_stateid='" + HSRPStateID + "' and vehicleregno ='" + txtVehicleNo.Text + "'";
                //result1 = Utils.ExecNonQuery(updateqry, CnnString);

                //updateqry = "update rtoinventory set inventorystatus='Rejected',rejectdate=getdate(),remarks='" + txtRemarks.Text + "' where hsrp_stateid='" + HSRPStateID + "' and laserno in ('" + lblFLNo.Text + "','"+ lblRLNo.Text +"')";
                //result1 = Utils.ExecNonQuery(updateqry, CnnString);
                
                //strquery2 = "select CHSRPRecordID from HSRPRecords_Rejct where hsrprecordid='" + Recordid + "'";
                //dt = Utils.GetDataTable(strquery2, CnnString);
                //string strRecordID = dt.Rows[0]["CHSRPRecordID"].ToString();

                //strquery3 = "insert into rejectplatEntry (ReplacementRequestID,EntryType,EntryDate,OriginalRequestID,RejectionType,FrontLaserCode,RearLaserCode,EmbosssingCenterCode,AffixationCenterCode,ReasonforRejection,OperatorName,ResponsibilityCenter,hsrp_stateid,rtolocationid) values('" + strRecordID + "','" + ddlEntryType.SelectedItem.ToString() + "',getdate(),'" + Recordid + "','" + ddlRejectType.SelectedItem.ToString() + "','" + lblFLNo.Text + "','" + lblRLNo.Text + "','" + embossingcode + "','" + AffixationCode + "','" + txtRemarks.Text + "','" + txtoperatorname.Text + "','" + RTOid + "','" + HSRPStateID + "','" + RTOid + "')";
                //int result = Utils.ExecNonQuery(strquery3, CnnString);
                //if (result > 0)
                //{
                    
                //    //affix();
                //    lblSucMess.Text = "Record Updated Successfully.";
                //    //blank();
                //}
            }
            else if (ddlEntryType.SelectedItem.Text == "Affixation" && ddlRejectType.SelectedItem.Text == "Front Plate")
            {
                result1 = Utils.ExecNonQuery("insert into [HSRPRecords_Rejct] select * from [HSRPRecords] where hsrp_stateid='" + HSRPStateID + "' and vehicleregno ='" + txtVehicleNo.Text + "'", CnnString);
                updateqry = "update hsrprecords set HSRP_Front_LaserCode=null,orderstatus='New Order',orderembossingdate=null,RejectFlag='Y' where hsrp_stateid='" + HSRPStateID + "' and vehicleregno ='" + txtVehicleNo.Text + "'";
                result1 = Utils.ExecNonQuery(updateqry, CnnString);

                updateqry = "update rtoinventory set inventorystatus='Rejected',rejectdate=getdate(),remarks='" + txtRemarks.Text + "' where hsrp_stateid='" + HSRPStateID + "' and laserno ='" + lblFLNo.Text + "'";
                result1 = Utils.ExecNonQuery(updateqry, CnnString);

                strquery2 = "select CHSRPRecordID from HSRPRecords_Rejct where hsrprecordid='" + Recordid + "'";
                dt = Utils.GetDataTable(strquery2, CnnString);
                string strRecordID = dt.Rows[0]["CHSRPRecordID"].ToString();

                strquery3 = "insert into rejectplatEntry (ReplacementRequestID,EntryType,EntryDate,OriginalRequestID,RejectionType,FrontLaserCode,EmbosssingCenterCode,AffixationCenterCode,ReasonforRejection,OperatorName,ResponsibilityCenter,hsrp_stateid,rtolocationid) values('"+ strRecordID +"','" + ddlEntryType.SelectedItem.ToString() + "',getdate(),'" + Recordid + "','" + ddlRejectType.SelectedItem.ToString() + "','" + lblFLNo.Text + "','" + embossingcode + "','" + AffixationCode + "','" + txtRemarks.Text + "','" + txtoperatorname.Text + "','" + RTOid + "','" + HSRPStateID + "','" + RTOid + "')";
                int result = Utils.ExecNonQuery(strquery3, CnnString);
                if (result > 0)
                {
                   // affix();
                    lblSucMess.Text = "Record Updated Successfully.";
                  //  blank();
                }
            }
            else if (ddlEntryType.SelectedItem.Text == "Affixation" && ddlRejectType.SelectedItem.Text == "Rear Plate")
            {
              //  affix();
                result1 = Utils.ExecNonQuery("insert into [HSRPRecords_Rejct] select * from [HSRPRecords] where hsrp_stateid='" + HSRPStateID + "' and vehicleregno ='" + txtVehicleNo.Text + "'", CnnString);
               // result1 = Utils.ExecNonQuery("insert into [HSRPRecords_Rejct] select * from [HSRPRecords] where hsrp_stateid='" + HSRPStateID + "' and vehicleregno ='" + txtVehicleNo.Text + "'", CnnString);
                updateqry = "update hsrprecords set HSRP_Rear_LaserCode=null,orderstatus='New Order',orderembossingdate=null,RejectFlag='Y' where hsrp_stateid='" + HSRPStateID + "' and vehicleregno ='" + txtVehicleNo.Text + "'";
                result1 = Utils.ExecNonQuery(updateqry, CnnString);

                updateqry = "update rtoinventory set inventorystatus='Rejected',rejectdate=getdate(),remarks='" + txtRemarks.Text + "' where hsrp_stateid='" + HSRPStateID + "' and laserno ='" + lblRLNo.Text + "'";
                result1 = Utils.ExecNonQuery(updateqry, CnnString);

                strquery2 = "select CHSRPRecordID from HSRPRecords_Rejct where hsrprecordid='" + Recordid + "'";
                dt = Utils.GetDataTable(strquery2, CnnString);
                string strRecordID = dt.Rows[0]["CHSRPRecordID"].ToString();

                strquery3 = "insert into rejectplatEntry (ReplacementRequestID,EntryType,EntryDate,OriginalRequestID,RejectionType,RearLaserCode,EmbosssingCenterCode,AffixationCenterCode,ReasonforRejection,OperatorName,ResponsibilityCenter,hsrp_stateid,rtolocationid) values('"+ strRecordID+"','" + ddlEntryType.SelectedItem.ToString() + "',getdate(),'" + Recordid + "','" + ddlRejectType.SelectedItem.ToString() + "','" + lblRLNo.Text + "','" + embossingcode + "','" + AffixationCode + "','" + txtRemarks.Text + "','" + txtoperatorname.Text + "','" + RTOid + "','" + HSRPStateID + "','" + RTOid + "')";
                int result = Utils.ExecNonQuery(strquery3, CnnString);
                if (result > 0)
                {
                    //affix();
                    lblSucMess.Text = "Record Updated Successfully.";
                    //blank();
                }
            }
            else
            {
                strquery3 = "insert into rejectplatEntry (EntryType,EntryDate,OriginalRequestID,RejectionType,RearLaserCode,EmbosssingCenterCode,AffixationCenterCode,ReasonforRejection,OperatorName,ResponsibilityCenter,hsrp_stateid,rtolocationid) values('" + ddlEntryType.SelectedItem.ToString() + "',getdate(),'" + Recordid + "','" + ddlRejectType.SelectedItem.ToString() + "','" + lblRLNo.Text + "','" + embossingcode + "','" + AffixationCode + "','" + txtRemarks.Text + "','" + txtoperatorname.Text + "','" + RTOid + "','" + HSRPStateID + "','" + RTOid + "')";
                int result = Utils.ExecNonQuery(strquery3, CnnString);
                if (result > 0)
                {
                    lblSucMess.Text = "Record Updated Successfully.";
                    //blank();
                }
            }

           

        }

        protected void affix()
        {
            string VehicleRegNo = HiddenField4.Value;
            strquery3 = "Select * from HSRPRecords where  [VehicleRegNo]='"+VehicleRegNo+"'";
             dt = Utils.GetDataTable(strquery3, CnnString);
             if (dt.Rows.Count > 0)
             {
                 // string HsrpRecordID = dt.Rows[0]["HSRPRecordID"].ToString();
                 string HSRP_StateID = dt.Rows[0]["HSRP_StateID"].ToString();
                 string RTOLocationID = dt.Rows[0]["RTOLocationID"].ToString();
                 string CreationDate = dt.Rows[0]["HSRPRecord_CreationDate"].ToString();
                 string HSRPRecord_AuthorizationNo = dt.Rows[0]["HSRPRecord_AuthorizationNo"].ToString();
                 string AuthorizationDate = dt.Rows[0]["HSRPRecord_AuthorizationDate"].ToString();
                 string OwnerName = dt.Rows[0]["OwnerName"].ToString();
                 string Address1 = dt.Rows[0]["Address1"].ToString();
                 string MobileNo = dt.Rows[0]["MobileNo"].ToString();
                 string OrderNo = dt.Rows[0]["OrderNo"].ToString();
                 string OrderType = dt.Rows[0]["OrderType"].ToString();
                 string OrderDate = dt.Rows[0]["OrderDate"].ToString();
                 string OrderStatus = dt.Rows[0]["OrderStatus"].ToString();
                 string OrderEmbossingDate = dt.Rows[0]["OrderEmbossingDate"].ToString();
                 string VehicleClass = dt.Rows[0]["VehicleClass"].ToString();
                 string VehicleType = dt.Rows[0]["VehicleType"].ToString();
                 string ManufacturerName = dt.Rows[0]["ManufacturerName"].ToString();
                 string ManufacturerModel = dt.Rows[0]["ManufacturerModel"].ToString();
                 string ChassisNo = dt.Rows[0]["ChassisNo"].ToString();
                 string EngineNo = dt.Rows[0]["EngineNo"].ToString();
                 VehicleRegNo = dt.Rows[0]["VehicleRegNo"].ToString();
                 string Front_LaserCode = dt.Rows[0]["HSRP_Front_LaserCode"].ToString();
                 string Rear_LaserCode = dt.Rows[0]["HSRP_Rear_LaserCode"].ToString();
                 string ISFrontPlateSize = dt.Rows[0]["ISFrontPlateSize"].ToString();
                 string FrontPlateSize = dt.Rows[0]["FrontPlateSize"].ToString();
                 string FrontplatePrize = dt.Rows[0]["FrontplatePrize"].ToString();
                 string ISRearPlateSize = dt.Rows[0]["ISRearPlateSize"].ToString();
                 string RearPlateSize = dt.Rows[0]["RearPlateSize"].ToString();

                 string RearPlatePrize = dt.Rows[0]["RearPlatePrize"].ToString();
                 string StickerMandatory = dt.Rows[0]["StickerMandatory"].ToString();
                 string StickerPrize = dt.Rows[0]["StickerPrize"].ToString();

                 string CashReceiptNo = dt.Rows[0]["CashReceiptNo"].ToString();
                 string InvoiceNo = dt.Rows[0]["InvoiceNo"].ToString();
                 string DeliveryChallanNo = dt.Rows[0]["DeliveryChallanNo"].ToString();

                 string ScrewPrize = dt.Rows[0]["ScrewPrize"].ToString();
                 string FixingCharge = dt.Rows[0]["FixingCharge"].ToString();
                 string TotalAmount = dt.Rows[0]["TotalAmount"].ToString();

                 string VAT_Percentage = dt.Rows[0]["VAT_Percentage"].ToString();
                 string VAT_Amount = dt.Rows[0]["VAT_Amount"].ToString();
                 string ServiceTax_Percentage = dt.Rows[0]["ServiceTax_Percentage"].ToString();

                 string ServiceTax_Amount = dt.Rows[0]["ServiceTax_Amount"].ToString();
                 string NetAmount = dt.Rows[0]["NetAmount"].ToString();
                 string OrderClosedDate = dt.Rows[0]["OrderClosedDate"].ToString();

                 string SendRecordToNic = dt.Rows[0]["SendRecordToNic"].ToString();
                 string UserRTOLocationID = dt.Rows[0]["UserRTOLocationID"].ToString();
                 string EmbossingUserID = dt.Rows[0]["EmbossingUserID"].ToString();

                 string RoundOff_NetAmount = dt.Rows[0]["RoundOff_NetAmount"].ToString();
                 string SendtoProductionStatus = dt.Rows[0]["SendtoProductionStatus"].ToString();
                 string sendtoERP = dt.Rows[0]["sendtoERP"].ToString();

                 strquery3 = "insert into HSRPRecords_Rejection(HSRP_StateID,RTOLocationID,HSRPRecord_CreationDate,HSRPRecord_AuthorizationNo," +
                     "HSRPRecord_AuthorizationDate,OwnerName,Address1,MobileNo,OrderNo,OrderType,OrderDate,OrderStatus,OrderEmbossingDate,VehicleClass," +
                "VehicleType,ManufacturerName,ManufacturerModel,ChassisNo,EngineNo,VehicleRegNo,HSRP_Front_LaserCode,HSRP_Rear_LaserCode,ISFrontPlateSize," +
                "FrontPlateSize,FrontplatePrize,ISRearPlateSize,RearPlateSize,RearPlatePrize,StickerMandatory,StickerPrize,CashReceiptNo,InvoiceNo,DeliveryChallanNo," +
                "ScrewPrize,FixingCharge,TotalAmount,VAT_Percentage,VAT_Amount,ServiceTax_Percentage,ServiceTax_Amount,NetAmount,OrderClosedDate,SendRecordToNic," +
                "UserRTOLocationID,EmbossingUserID,RoundOff_NetAmount,SendtoProductionStatus,sendtoERP,oldhsrprecordid) values('" + HSRP_StateID + "','" + RTOLocationID + "','" + CreationDate + "','" + HSRPRecord_AuthorizationNo + "','" + AuthorizationDate + "','" + OwnerName + "','" + Address1 + "','" + MobileNo + "','" + OrderNo + "','" + OrderType + "','" + OrderDate + "','" + OrderStatus + "','" + OrderEmbossingDate + "','" + VehicleClass + "','" + VehicleType + "','" + ManufacturerName + "','" + ManufacturerModel + "','" + ChassisNo + "','" + EngineNo + "','" + VehicleRegNo + "','" + Front_LaserCode + "','" + Rear_LaserCode + "','" + ISFrontPlateSize + "','" + FrontPlateSize + "','" + FrontplatePrize + "','" + ISRearPlateSize + "','" + RearPlateSize + "','" + RearPlatePrize + "','" + StickerMandatory + "','" + StickerPrize + "','" + CashReceiptNo + "','" + InvoiceNo + "','" + DeliveryChallanNo + "','" + ScrewPrize + "','" + FixingCharge + "','" + TotalAmount + "','" + VAT_Percentage + "','" + VAT_Amount + "','" + ServiceTax_Percentage + "','" + ServiceTax_Amount + "','" + NetAmount + "','" + OrderClosedDate + "','" + SendRecordToNic + "','" + UserRTOLocationID + "','" + EmbossingUserID + "','" + RoundOff_NetAmount + "','" + SendtoProductionStatus + "','" + sendtoERP + "','" + Recordid + "')";

                 int check = Utils.ExecNonQuery(strquery3, CnnString);
                 strquery2 = "select HsrpRecordID from HSRPRecords_Rejection where oldhsrprecordid='" + Recordid + "'";
                 dt = Utils.GetDataTable(strquery2, CnnString);
                 string strRecordID = dt.Rows[0]["HsrpRecordID"].ToString();


                 strquery3 = "update rejectplatEntry set ReplacementRequestID='" + strRecordID + "' where OriginalRequestID='" + Recordid + "'";
                 Utils.ExecNonQuery(strquery3, CnnString);

                 if (check > 0)
                 {
                     blank();
                     //Record saved"
                 }
             }
            
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
           
            SQLString = "select OrderType,HSRP_Front_LaserCode,HSRP_Rear_LaserCode,HSRPRecordID,rtolocationID,HSRP_Stateid from HSRPRecords where VehicleRegNo='" + txtVehicleNo.Text + "'";
            DataTable dt = Utils.GetDataTable(SQLString, CnnString);
            if (dt.Rows.Count > 0)
            {
                RTOLocationID = dt.Rows[0]["rtolocationID"].ToString();
                HSRPRecordID = dt.Rows[0]["HSRPRecordID"].ToString();
                HSRPStateID = dt.Rows[0]["HSRP_Stateid"].ToString();

            }

            HiddenField1.Value= RTOLocationID;
            HiddenField2.Value = HSRPRecordID;
            HiddenField3.Value = HSRPStateID;
            DataSet dds = Utils.getDataSet(SQLString, CnnString);
            if (dds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dds.Tables[0].Rows)
                {
                    lblordertype.Text = dr["OrderType"].ToString();
                    lblFLNo.Text = dr["HSRP_Front_LaserCode"].ToString();
                    lblRLNo.Text = dr["HSRP_Rear_LaserCode"].ToString();

                    ddlRejectType.Items.Clear();
                    if (!string.IsNullOrEmpty(lblFLNo.Text) && !string.IsNullOrEmpty(lblRLNo.Text))
                    {
                        ddlRejectType.Items.Add("Both");
                        ddlRejectType.Items.Add("Front Plate");
                        ddlRejectType.Items.Add("Rear Plate");
                    }
                    else if (string.IsNullOrEmpty(lblFLNo.Text) && !string.IsNullOrEmpty(lblRLNo.Text))
                    {
                        ddlRejectType.Items.Add("Rear Plate");
                    }
                    else if (!string.IsNullOrEmpty(lblFLNo.Text) && string.IsNullOrEmpty(lblRLNo.Text))
                    {
                        ddlRejectType.Items.Add("Front Plate");
                    }
                    else if (string.IsNullOrEmpty(lblFLNo.Text) && string.IsNullOrEmpty(lblRLNo.Text))
                    {
                        ddlRejectType.Items.Add("--Select Reject Type--");
                    }


                }

            }
           
        }
        public void blank()
        {
            txtVehicleNo.Text = "";
            txtRemarks.Text = "";
            txtoperatorname.Text = "";
            lblFLNo.Text = "";
            lblordertype.Text = "";
            lblRLNo.Text = "";
            ddlEntryType.SelectedIndex = -1;
            ddlRejectType.SelectedIndex = -1;
        }

        protected void ddlRejectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            

            
            
            if (ddlRejectType.SelectedItem.Text=="Both")
            {
                lblRLNo.Visible = true;
                lblFLNo.Visible = true;
            }
            else if (ddlRejectType.SelectedItem.Text == "Front Plate")
            {
                lblFLNo.Visible = true;
                lblRLNo.Visible = false;
            }
            else
            {
                lblRLNo.Visible = true;
                lblFLNo.Visible=false;
            }

        }



    }

}