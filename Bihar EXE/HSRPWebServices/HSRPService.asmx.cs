using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Serialization;
using System.IO;
using System.Net;

namespace HSRPWebServices
{
    /// <summary>
    /// Summary description for HSRPService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class HSRPService : System.Web.Services.WebService
    {
        utils obj = new utils();
        // string CnnString = obj.strProvider;
        DataTable dt1 = new DataTable("dt");
        SqlDataReader PReader;



        #region GetDataByEngineNumber
        [WebMethod]
        public DataTable GetDataByAuthNo(string AuthNo)
        {
            string StrRecord = string.Empty;


            string sqlText = "select * from HSRPRecords where HSRPRecord_AuthorizationNo='" + AuthNo.ToString() + "'";

            string CnnString = obj.strProvider;
            DataTable dt = new DataTable();
            dt = utils.GetDataTable(sqlText, CnnString);

            dt.TableName = "HSRPRecords_bihar";

            return dt;

        }

        #endregion

        #region GetDataByVehicleRegNo
        [WebMethod]
        public DataTable GetDataByVehicleRegNo(string VehicleRegNo)
        {

            string StrRecord = string.Empty;
            string sqlText = "select * from HSRPRecords where VehicleRegNo='" + VehicleRegNo.ToString() + "'";
            // utils obj = new utils();
            string CnnString = obj.strProvider;
            DataTable dt = new DataTable();
            dt = utils.GetDataTable(sqlText, CnnString);
            dt.TableName = "HSRPRecords_bihar";
            return dt;

        }
        #endregion

        #region BiharDataCashCollection
        [WebMethod]
        public string BiharDataCashCollection(string HSRP_StateID, string RTOLocationID, string HSRPRecord_AuthorizationNo, string HSRPRecord_AuthorizationDate, string VehicleRegNo, string OwnerName, string ownerFatherName, string Address1, string MobileNo, string VehicleClass, string OrderType, string StickerMandatory, string isVIP, string NetAmount, string Roundoff_NetAmount, string VehicleType, string OrderStatus, string CashReceiptNo, string ChassisNo, string EngineNo, string DealerCode, string CreatedBy, string SaveMacAddress, string Addrecordby, string ISFrontPlateSize, string ISRearPlateSize, string FrontPlateSize, string RearPlateSize, string Reference, string ManufacturerModel, string vehicleref, string ManufacturerName, string CounterNo , string Affixid,string dealername)
        {
            string Status = string.Empty;
            //utils obj = new utils();
            string CnnString = obj.strProvider;
            string sqlText = "exec Transaction_DataCashCollection_Bihar'" + HSRP_StateID.ToString() + "','" + RTOLocationID.ToString() + "','" + HSRPRecord_AuthorizationNo.ToString() + "','" + HSRPRecord_AuthorizationDate.ToString() + "','" + VehicleRegNo.ToString() + "','" + OwnerName.ToString() + "','" + ownerFatherName.ToString() + "','" + Address1.ToString() + "','" + MobileNo.ToString() + "','" + VehicleClass.ToString() + "','" + OrderType.ToString() + "','" + StickerMandatory.ToString() + "','" + isVIP.ToString() + "','" + NetAmount.ToString() + "','" + Roundoff_NetAmount.ToString() + "','" + VehicleType.ToString() + "','" + OrderStatus.ToString() + "','" + CashReceiptNo.ToString() + "','" + ChassisNo.ToString() + "','" + EngineNo.ToString() + "','" + DealerCode.ToString() + "','" + CreatedBy.ToString() + "','" + SaveMacAddress.ToString() + "','" + Addrecordby.ToString() + "','" + ISFrontPlateSize.ToString() + "','" + ISRearPlateSize.ToString() + "','" + FrontPlateSize.ToString() + "','" + RearPlateSize.ToString() + "','" + Reference.ToString() + "','" + ManufacturerModel.ToString() + "','" + vehicleref.ToString() + "','" + ManufacturerName.ToString() + "','" + CounterNo.ToString() + "','" + Affixid + "','" + dealername + "'";

           int check = utils.ExecNonQuery(sqlText, CnnString);
            //int check = 0;
            if (check !=0)
            {
                if (MobileNo.Length == 10)
                {

                    //string SMSText = " Cash Rs." + Math.Round(decimal.Parse(lblAmount.Text), 0) + " received against HSRP Authorization No. " + lblAuthNo.Text + " on " + System.DateTime.Now.ToString("dd/MM/yyyy") + " receipt number " + cashrc + ". HSRP Team.";

                    string SMSText = " Cash Rs." + Math.Round(decimal.Parse(NetAmount), 0) + " collected against Vehicle No. " + VehicleRegNo + " receipt number " + CashReceiptNo + "dated  " + System.DateTime.Now.ToString("dd/MM/yyyy") + ".HSRP Team";
                    string SMSType = "1";
                    // string SMSText = "Your H.S.R.P. " + vehicleno + " is ready for Affixation, Please visit your H.S.R.P. center for Affixation Between 10 AM - 5.30 PM. HSRP Team.";
                    string sendURL = "http://103.16.101.52:8080/bulksms/bulksms?username=sse-biharhsrp&password=biharhsr&type=0&dlr=1&destination=" + MobileNo + "&source=BRHSRP&message=" + SMSText;
                    HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(sendURL);
                    myRequest.Method = "GET";
                    WebResponse myResponse = myRequest.GetResponse();
                    StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                    string result = sr.ReadToEnd();
                    sr.Close();
                    myResponse.Close();
                    System.Threading.Thread.Sleep(350);

                    utils.ExecNonQuery("insert into BRSMSDetail(RtoLocationID,AuthorizationNumber,VehicleRegNo,MobileNo,SentResponseCode,smstext,SMSType) values('" + RTOLocationID + "','" + HSRPRecord_AuthorizationNo.ToString() + "','" + VehicleRegNo.ToString() + "'," + MobileNo.ToString() + ",'" + result + "','" + SMSText + "','" + SMSType + "')", CnnString);
                   // ExecNonQuery("update  " + tablename + " set [SMSSentStatus] ='Y' where [regno]='" + vehicleno + "'", CnnString);
                }
                Status = "Record Saved";
            }
            else
            {
                Status = "Record Not Saved" + "^" + VehicleRegNo + "^" + HSRPRecord_AuthorizationNo;
            }
            return Status;
        }
        #endregion

        #region CheckDuplicateEntry
        [WebMethod]
        public String CheckDuplicateEntry(string HSRPStateID, string AuthNo, string VehicleRegNo, string OrderType)
        {
            string StrRecord = string.Empty;
            string sqlText = "select COUNT(VehicleRegNo) as co from dbo.HSRPRecords  where hsrp_stateid ='" + HSRPStateID + "' and VehicleRegNo ='" + VehicleRegNo + "' and OrderType='" + OrderType + "' and HSRPRecord_AuthorizationNo='"+AuthNo+"'";

            string CnnString = obj.strProvider;
            DataTable dt = new DataTable();
            dt = utils.GetDataTable(sqlText, CnnString);
            //dt.Rows[0]["co"].ToString();
            if (dt.Rows[0]["co"].ToString() != "0")
            {
                StrRecord = "Fail" + "^" + VehicleRegNo.ToString();
            }
            else
            {
                StrRecord = "Pass";
            }
            return StrRecord;
        }
        #endregion

        #region GetLogInDetail
        [WebMethod]
        public string GetLogInDetail(string UserName, string Password, string MacAddress)
        {
            string SqlText = string.Empty;
            string Status = string.Empty;
            string MacStatus = string.Empty;
            string StateID = string.Empty;
            string RtoLocationID = string.Empty;
            string UserID = string.Empty;
            string RTOLocationAddress = string.Empty;
            string strUsername = string.Empty;
            string CnnString = obj.strProvider;
            SqlText = "select U.ActiveStatus,U.Userid,(U.Userfirstname+''+U.UserLastName) as Usersname,U.HSRP_StateID,R.RtolocationID,R.RTOLocationAddress from  Users U inner join RTOLocation R ON R.RTOLocationID=U.RTOLocationID where [UserLoginName]='" + UserName + "' and [Password]='" + Password + "'";
            DataTable dt = new DataTable();
            dt = utils.GetDataTable(SqlText, CnnString);
            if (dt.Rows.Count > 0)
            {
                Status = dt.Rows[0]["ActiveStatus"].ToString();
                StateID = dt.Rows[0]["HSRP_StateID"].ToString();
                RtoLocationID = dt.Rows[0]["RTOLocationID"].ToString();
                RTOLocationAddress = dt.Rows[0]["RTOLocationAddress"].ToString();
                UserID = dt.Rows[0]["UserID"].ToString();
                strUsername = dt.Rows[0]["Usersname"].ToString();
                if (Status == "Y" || Status == "y")
                {
                    SqlText = "select * from [MACBase] where [MacAddress]='" + MacAddress + "'";
                    dt = utils.GetDataTable(SqlText, CnnString);
                    if (dt.Rows.Count > 0)
                    {
                        MacStatus = MacAddress;

                    }
                    else
                    {
                        MacStatus = "";
                        //Status = "A";
                    }
                }
                else
                {
                    Status = "N";
                }

            }
            else
            {
                Status = "W";

            }
            return Status + "^" + MacStatus + "^" + UserID + "^" + StateID + "^" + RtoLocationID + "^"+ RTOLocationAddress+"^" +strUsername;
        }
        #endregion

        #region GetRateAndTaxDetail
        [WebMethod]
        public string GetRateAndTaxDetail(string StateID, string VehicleModel, string VehicleClass, string OrderType)
        {

            String StrFrontPlatePrice = "0";
            string StrRearPlatePrice = "0";
            string FrontPlateID = string.Empty;
            string RearPlateID = string.Empty;
            string StickerID = string.Empty;
            string screwrate = string.Empty;
            string Discount = string.Empty;
            string Tax = string.Empty;
            string TotalAmount = string.Empty;
            string VATAMOUNT = string.Empty;
            string StrIsFrontPlate = string.Empty;
            string StrIsRearPlate = string.Empty;
            string strAmount = string.Empty;
            string strTax = string.Empty;

            string SQLString = "select dbo.hsrpplateamt ('" + StateID + "','" + VehicleModel + "','" + VehicleClass + "','" + OrderType + "') as Amount";

            DataTable dt = utils.GetDataTable(SQLString, utils.CnnString);
            strAmount = dt.Rows[0]["Amount"].ToString();

            SQLString = "select FrontPlateID,RearPlateID,StickerID from RegistrationPlateDetail  where hsrp_stateid ='" + StateID + "' and vehicletype='" + VehicleModel + "' and vehicleclass='" + VehicleClass + "' and ordertype='" + OrderType + "'";
            DataTable dtPlateSize = utils.GetDataTable(SQLString, utils.CnnString);
            if (dtPlateSize.Rows.Count > 0)
            {
                if (dtPlateSize.Rows[0]["FrontPlateID"].ToString() != "")
                {
                    FrontPlateID = dtPlateSize.Rows[0]["FrontPlateID"].ToString();
                    SQLString = "select cost from ProductCost where productid = '" + dtPlateSize.Rows[0]["FrontPlateID"].ToString() + "' and hsrp_stateid ='" + StateID + "'";
                    dt = utils.GetDataTable(SQLString, utils.CnnString);
                    if (dt.Rows.Count > 0)
                    {
                        StrFrontPlatePrice = dt.Rows[0]["Cost"].ToString();
                        StrIsFrontPlate = "Y";
                    }
                    else
                    {
                        StrFrontPlatePrice = "0";
                    }
                }
                if (dtPlateSize.Rows[0]["RearPlateID"].ToString() != "")
                {
                    RearPlateID = dtPlateSize.Rows[0]["RearPlateID"].ToString();
                    SQLString = "Select cost from productCost where productid='" + dtPlateSize.Rows[0]["RearPlateID"].ToString() + "' and hsrp_stateid='" + StateID + "'";
                    dt = utils.GetDataTable(SQLString, utils.CnnString);
                    if (dt.Rows.Count > 0)
                    {
                        StrRearPlatePrice = dt.Rows[0]["Cost"].ToString();
                        StrIsRearPlate = "Y";
                    }
                    else
                    {
                        StrRearPlatePrice = "0";
                    }
                }
                SQLString = "select cost from ProductCost where productid ='" + dtPlateSize.Rows[0]["StickerID"].ToString() + "'  and hsrp_stateid ='" + StateID + "'";
                dt = utils.GetDataTable(SQLString, utils.CnnString);
                if (dt.Rows.Count > 0)
                {
                    StickerID = dt.Rows[0]["Cost"].ToString();
                }
                else
                {
                    StickerID = "0";
                }

                //screwrate
                SQLString = "select productcost from Product where productcode='SNAP LOCK' and hsrp_stateid ='" + StateID + "'";
                dt = utils.GetDataTable(SQLString, utils.CnnString);
                if (dt.Rows.Count > 0)
                {
                    screwrate = dt.Rows[0]["ProductCost"].ToString();
                }
                else
                {
                    screwrate = "0";
                }
                SQLString = "select discountamount from Discount where hsrp_stateid ='" + StateID + "' and vehicletype='" + VehicleModel + "' and vehicleclass='" + VehicleClass + "'";
                dt = utils.GetDataTable(SQLString, utils.CnnString);
                if (dt.Rows.Count > 0)
                {
                    Discount = dt.Rows[0]["discountamount"].ToString();
                }
                else
                {
                    Discount = "0";
                }
                SQLString = " select taxpercentage from tax where hsrp_stateid ='" + StateID + "' and taxpercentage > 0";
                dt = utils.GetDataTable(SQLString, utils.CnnString);
                if (dt.Rows.Count > 0)
                {
                    Tax = dt.Rows[0]["taxpercentage"].ToString();
                }
                else
                {
                    Tax = "0";
                }
                float strnetamount1;


                strnetamount1 = float.Parse(StrFrontPlatePrice) + float.Parse(StrRearPlatePrice) + float.Parse(screwrate) + float.Parse(StickerID) + float.Parse(Discount);

                TotalAmount = strnetamount1.ToString();
                float NetAmountTax = (strnetamount1 / 100);
                NetAmountTax = NetAmountTax * (float.Parse(Tax));


                // hiddenfieldTax.Value=NetAmountTax.ToString();
                decimal vatamount = Math.Round(Convert.ToDecimal(NetAmountTax), 2);
                VATAMOUNT = vatamount.ToString();

                float netAmount = (NetAmountTax) + (strnetamount1);
                Decimal NetAmountDecimal = Math.Round(Convert.ToDecimal(netAmount), 2);
                //hiddenfieldNetAmount.Value = Math.Round(netAmount).ToString();

            }

            return StrFrontPlatePrice + "^" + StrIsFrontPlate + "^" + StrRearPlatePrice + "^" + StrIsRearPlate + "^" + StickerID + "^" + screwrate + "^" + Discount + "^" + VATAMOUNT + "^" + FrontPlateID + "^" + RearPlateID;

        }
        #endregion

        #region GetRateAndTaxForVehicle
        [WebMethod]
        public string GetRateAndTaxForVehicle(string StateID, string VehicleType, string VehicleClass, string OrderType)
        {
            string Amount = string.Empty;
            string tax = string.Empty;
            //  string orderType = "OB";
            string CnnString = obj.strProvider;
            string NetAmount = string.Empty;
            DataTable dt = new DataTable();

            string SQLString = "select dbo.hsrpplateamt ('" + StateID + "','" + VehicleType + "','" + VehicleClass + "','" + OrderType + "') as Amount";
            dt = utils.GetDataTable(SQLString, CnnString);

            if (dt.Rows.Count > 0)
            {
                string[] a = dt.Rows[0]["Amount"].ToString().Split('.');
                if (int.Parse(a[1]) > 50)
                {

                    a[0] = (int.Parse(a[0]) + 1).ToString();
                    Amount = a[0];

                }
                else
                    Amount = a[0];
            }

            SQLString = "select dbo.hsrpplatetax ('" + StateID + "','" + VehicleType + "','" + VehicleClass + "','" + OrderType + "') as Tax";
            dt = utils.GetDataTable(SQLString, CnnString);
            if (dt.Rows.Count > 0)
            {
                tax = dt.Rows[0]["Tax"].ToString();
            }
            NetAmount = (float.Parse(Amount) + float.Parse(tax)).ToString();
            return Amount + "^" + tax + "^" + NetAmount;
        }
        #endregion


        #region PopulateDropDown

        [WebMethod]
        public DataTable PopulateVehicleType()
        {
            string StrRecord = string.Empty;
            string sqlText = "select vehicletype from vehicletype where shortname is not null ";
            string CnnString = obj.strProvider;
            DataTable dt = new DataTable();
            dt = utils.GetDataTable(sqlText, CnnString);
            dt.TableName = "vehicletype";
            return dt;

        }

        [WebMethod]
        public DataTable PopulateVehicleModel(string VehiclMake)
        {
            string StrRecord = string.Empty;
            string sqlText = "select VehicleModelDescription,VehicleModelID from VehicleModelMaster where VehicleMakerID='" + VehiclMake + "'";
            //string sqlText = "select VehicleMakerDescription,VehicleMakerID from VehicleMakerMaster";
            string CnnString = obj.strProvider;
            DataTable dt = new DataTable();
            dt = utils.GetDataTable(sqlText, CnnString);
            dt.TableName = "VehicleModelMaster";
            return dt;
        }

        [WebMethod]
        public DataTable PopulateVehicleMake()
        {
            string StrRecord = string.Empty;
            string sqlText = "select VehicleMakerDescription,VehicleMakerID from VehicleMakerMaster";
            //string sqlText = "select VehicleModelDescription,VehicleModelID from VehicleModelMaster";
            string CnnString = obj.strProvider;
            DataTable dt1 = new DataTable();
            dt1 = utils.GetDataTable(sqlText, CnnString);
            dt1.TableName = "VehicleMakerMaster";
            return dt1;
        }


        #endregion


        #region OrderClosed_Validate



        [WebMethod]
        public DataTable RecordClosed(string VehicleRegNo)
        {

            string CnnString = obj.strProvider;
            string Query = string.Empty;

            DataTable dt2 = new DataTable();

            Query = "select * from HSRPRecords_AP where vehicleregno='" + VehicleRegNo + "' and orderstatus in ('Closed')";

            dt2 = utils.GetDataTable(Query, CnnString);
            dt2.TableName = "HSRPRecords_AP";
            return dt2;
        }
        [WebMethod]
        public DataTable RecordNotClosed(string VehicleRegNo)
        {

            string CnnString = obj.strProvider;
            string Query = string.Empty;

            Query = "select * from HSRPRecords_AP where vehicleregno='" + VehicleRegNo + "' and orderstatus not in ('Closed')";
            dt1 = utils.GetDataTable(Query, CnnString);
            dt1.TableName = "HSRPRecords_AP";
            return dt1;
        }


        #endregion

        #region  OrderAffixed

        string sqlText = string.Empty;
        string laservalue = string.Empty;
        [WebMethod]
        public string InventoryStatusF_NotNewOrder(string Flaser)   //same method for Embossing
        {
            string CnnString = obj.strProvider;
            string sqlText = "select laserno from RTOInventory where laserno='" + Flaser + "' and inventorystatus not in ('New Order')";
            laservalue = utils.getDataSingleValue(sqlText, CnnString, "laserno");
            return laservalue;
        }
        [WebMethod]
        public string InventoryStatusR_NotNewOrder(string Rlaser)     //same method for Embossing
        {
            string CnnString = obj.strProvider;
            string sqlText = "select laserno from RTOInventory where laserno='" + Rlaser + "' and inventorystatus not in ('New Order')";
            string laservalue = utils.getDataSingleValue(sqlText, CnnString, "laserno");
            return laservalue;
        }
        [WebMethod]
        public String InventoryStatusF_NewOrder(string Flaser)            //same method for Embossing
        {
            string CnnString = obj.strProvider;
            string sqlText = "select laserno from RTOInventory where laserno='" + Flaser + "' and inventorystatus  ='New Order'";
            laservalue = utils.getDataSingleValue(sqlText, CnnString, "laserno");
            return laservalue;
        }
        [WebMethod]
        public string InventoryStatusR_NewOrder(string Rlaser)          //same method for Embossing
        {
            string CnnString = obj.strProvider;
            string sqlText = "select laserno from RTOInventory where  laserno='" + Rlaser + "' and inventorystatus = 'New Order'";
            laservalue = utils.getDataSingleValue(sqlText, CnnString, "laserno");
            return laservalue;
        }
        #region UpdateRecords

        [WebMethod]
        public string FrontValidation(string Flaser)     //same method for Embossing
        {
            string CnnString = obj.strProvider;
            string sqlText = "select vehicleregno from HSRPRecords_AP where hsrp_front_lasercode='" + Flaser + "' or hsrp_rear_lasercode='" + Flaser + "'";
            laservalue = utils.getDataSingleValue(sqlText, CnnString, "vehicleregno");
            return laservalue;
        }

        [WebMethod]
        public string RearValidation(string Rlaser)        //same method for Embossing
        {
            string CnnString = obj.strProvider;
            string sqlText = "select vehicleregno from HSRPRecords_AP where hsrp_front_lasercode='" + Rlaser + "' or hsrp_rear_lasercode='" + Rlaser + "'";
            laservalue = utils.getDataSingleValue(sqlText, CnnString, "vehicleregno");
            return laservalue;
        }

        [WebMethod]
        public DataTable OrderStatusNotClosed(string VehiclRegNo)
        {
            string CnnString = obj.strProvider;
            string sqlText = "select * from HSRPRecords_AP where vehicleregno = '" + VehiclRegNo + "' and orderstatus not in ('Closed')";
            dt1 = utils.GetDataTable(sqlText, CnnString);
            dt1.TableName = "HSRPRecords_AP";
            return dt1;
        }

        [WebMethod]
        public int OrderStatusEmbossingDone(string Flaser, string Rlaser, string VehicleRegNo)
        {
            string CnnString = obj.strProvider;
            string sqlText = "update HSRPRecords_AP set  ordercloseddate = getdate(),orderstatus='Closed', hsrp_front_lasercode='" + Flaser + "', hsrp_rear_lasercode ='" + Rlaser + "' where vehicleregno ='" + VehicleRegNo + "' and orderstatus='Embossing Done'";
            int j = utils.ExecNonQuery(sqlText, CnnString);
            return j;

        }

        [WebMethod]
        public int OrderStatusEmbossingDoneUpdate(string Flaser, string Rlaser, string VehicleRegNo)
        {
            string CnnString = obj.strProvider;
            string sqlText = "update HSRPRecords_AP set HSRP_NO_FRONT='" + Flaser + "',HSRP_NO_back= '" + Rlaser + "',HSRP_FIX_DT =getdate() where replace(REGN_NO,' ','') ='" + VehicleRegNo + "'";
            int j = utils.ExecNonQuery(sqlText, utils.CnnString);
            return j;
        }
        [WebMethod]
        public int OrderStatusNewOrder(string Flaser, string Rlaser, string VehicleRegNo)
        {
            string CnnString = obj.strProvider;
            string sqlText = "update HSRPRecords_AP set  ordercloseddate = getdate(),orderembossingdate = getdate(),orderstatus='Closed', hsrp_front_lasercode='" + Flaser + "', hsrp_rear_lasercode ='" + Rlaser + "' where vehicleregno ='" + VehicleRegNo + "' and orderstatus='New Order'";
            int j = utils.ExecNonQuery(sqlText, CnnString);
            return j;
        }

        [WebMethod]
        public int OrderStatusNewOrderUpdate(string Flaser, string Rlaser, string VehicleRegNo)
        {
            string CnnString = obj.strProvider;
            string sqlText = "update HSRPRecords_AP set HSRP_NO_FRONT='" + Flaser + "',HSRP_NO_back= '" + Rlaser + "',HSRP_ISSUE_DT =getdate(),HSRP_FIX_DT =getdate() where replace(REGN_NO,' ','') ='" + VehicleRegNo + "'";
            int j = utils.ExecNonQuery(sqlText, CnnString);
            return j;

        }
        #endregion

        #endregion


        #region Embossing_Validate
        [WebMethod]
        public DataTable Embossing_Validate(string VehiclRegNo)
        {
            string CnnString = obj.strProvider;
            string sqlText = "select * from HSRPRecords_AP where vehicleregno='" + VehiclRegNo + "' and orderstatus='New Order'";
            dt1 = utils.GetDataTable(sqlText, CnnString);
            dt1.TableName = "HSRPRecords_AP";
            return dt1;
        }




        #endregion


        #region Embossing_Save

        [WebMethod]
        public DataTable Embossing_OrderStatusNotClosed(string VehiclRegNo)
        {
            string CnnString = obj.strProvider;
            string sqlText = "select * from HSRPRecords_AP where vehicleregno = '" + VehiclRegNo + "' and orderstatus='New Order'";
            dt1 = utils.GetDataTable(sqlText, CnnString);
            dt1.TableName = "HSRPRecords_AP";
            return dt1;
        }
        [WebMethod]
        public int E_OrderStatusEmbossingDone(string Flaser, string Rlaser, string VehicleRegNo)
        {
            string CnnString = obj.strProvider;
            string sqlText = "update HSRPRecords_AP set  orderembossingdate = getdate(),orderstatus='Embossing Done', hsrp_front_lasercode='" + Flaser + "', hsrp_rear_lasercode ='" + Rlaser + "' where vehicleregno ='" + VehicleRegNo + "' and orderstatus='New Order'";
            int j = utils.ExecNonQuery(sqlText, CnnString);
            return j;

        }
        [WebMethod]
        public int Embossing_RtoInventory(string Flaser, string Rlaser)
        {
            string CnnString = obj.strProvider;
            string sqlText = "update RTOInventory set inventorystatus='Embossing Done',EmbossingDate=getdate() from RTOInventory where  laserno in ('" + Flaser + "','" + Rlaser + "') ";
            int j = utils.ExecNonQuery(sqlText, CnnString);
            return j;
        }


        //[WebMethod]
        //public SqlDataReader Embossing_Flaser(string Flaser, string InventoryStatus)
        //{
        //    string CnnString = obj.strProvider;
        //    string sqlText = string.Empty;
        //    //SqlCommand cmd = new SqlCommand("select top 10 [LaserNo] from [RTOInventory] where LaserNo like '%" + Flaser + "' and [InventoryStatus]='New Order'", obj.strProvider);
        //    //SqlDataReader dr = cmd.ExecuteReader();
        //    //return dr;
        //    sqlText = "select top 10 [LaserNo] from [RTOInventory] where LaserNo like '%" + Flaser + "' and [InventoryStatus]='New Order'";
        //    obj.strProvider = CnnString;
        //    obj.CommandTimeOut = 600;
        //    obj.sqlText = sqlText.ToString();
        //    PReader = obj.GetReader();

        //    return PReader;

        //}

        //[WebMethod]
        //public SqlDataReader Embossing_Rlaser(string Rlaser, string InventoryStatus)
        //{
        //    string CnnString = obj.strProvider;
        //    string sqlText = string.Empty;
        //    //SqlCommand cmd = new SqlCommand("select top 10 [LaserNo] from [RTOInventory] where LaserNo like '%" + Flaser + "' and [InventoryStatus]='New Order'", obj.strProvider);
        //    //SqlDataReader dr = cmd.ExecuteReader();
        //    //return dr;
        //    sqlText = "select top 10 [LaserNo] from [RTOInventory] where LaserNo like '%" + Rlaser + "' and [InventoryStatus]='New Order'";
        //    obj.strProvider = CnnString;
        //    obj.CommandTimeOut = 600;
        //    obj.sqlText = sqlText.ToString();
        //    SqlDataReader PReader = obj.GetReader();
        //    return PReader;

        //}

        //[WebMethod]
        //public SqlDataReader Embossing_RegNo(string RegNo)
        //{
        //    string CnnString = obj.strProvider;
        //    string sqlText = string.Empty;
        //    //SqlCommand cmd = new SqlCommand("select top 10 [LaserNo] from [RTOInventory] where LaserNo like '%" + Flaser + "' and [InventoryStatus]='New Order'", obj.strProvider);
        //    //SqlDataReader dr = cmd.ExecuteReader();
        //    //return dr;
        //    sqlText = "select top 10 vehicleregno from HSRPRecords_AP where vehicleregno like '" + RegNo + "%'";
        //    obj.strProvider = CnnString;
        //    obj.CommandTimeOut = 600;
        //    obj.sqlText = sqlText.ToString();
        //    SqlDataReader PReader = obj.GetReader();
        //    return PReader;

        //}



        #endregion

        # region Chalan
        [WebMethod]
        public DataTable Chalan(string StateId)
        {
            string CnnString = obj.strProvider;
            string sqlText = "select * from HSRPState WHERE HSRP_StateID='" + StateId+ "'";
            dt1 = utils.GetDataTable(sqlText, CnnString);
            dt1.TableName = "HSRPState";
            return dt1;

        }


        #endregion

        # region filldropdownlist
        [WebMethod]
        public DataTable filldropdownlistAFfix(string StateId, string RTOID)
        {
            string CnnString = obj.strProvider;
            string sqlText = "select affixcenterdesc,Affix_id,address1 from [dbo].[AffixationCenters] where state_id='" + StateId + "' and rto_id='" + RTOID + "' order by affixcenterdesc";
            dt1 = utils.GetDataTable(sqlText, CnnString);
            dt1.TableName = "AffixationCenters";
            return dt1;

        }
        #endregion


        # region filldropdownlist
        [WebMethod]
        public DataTable FillAddrss(string Affixid)
        {
            string CnnString = obj.strProvider;
            string sqlText = "select Address1 from [AffixationCenters] where Affix_Id='" + Affixid + "'";
            dt1 = utils.GetDataTable(sqlText, CnnString);
            dt1.TableName = "AffixationCenters";
            return dt1;

        }
        #endregion

        #region GetCashReceiptNo For BR
        [WebMethod(Description = " Get Cash Receipt No using AuthNo")]
        public string CashReceiptForHR(string AuthNo)
        {
            string CnnString = obj.strProvider;
            string strQuery = string.Empty;
            string strCashNo = string.Empty;
            strQuery = "select CashReceiptNo from hsrprecords where hsrp_stateid=1 and HSRPRecord_AuthorizationNo='" + AuthNo + "'";
            strCashNo = utils.getDataSingleValue(strQuery, CnnString, "CashReceiptNo");
            return strCashNo;

        }
        #endregion


       

        #region GetDateByHoliday

        [WebMethod(Description = " Get Holiday Details")]
        public string GetDateByHoliday(string holidaydate)
        {
            string CnnString = obj.strProvider;
            string strQuery = string.Empty;
            string strCashNo = string.Empty;
            strQuery = "select blockDate from HolidayDateTime where hsrp_stateid=1 and convert(date,blockDate)=Convert(date,'" + holidaydate + "')";
            strCashNo = utils.getDataSingleValue(strQuery, CnnString, "blockDate");
            return strCashNo;

        }

        #endregion


        # region filldropdownlistdealername
        [WebMethod(Description = " Get Dealer Name RTOLocation Wise")]
        public DataTable filldropdowndealername(string StateId, string RTOID)
        {
            string CnnString = obj.strProvider;
            string sqlText = "select dealerName,dealerid,Address from DealerMaster where hsrp_stateid='" + StateId + "' and RTOLocationID='" + RTOID + "'";           
            dt1 = utils.GetDataTable(sqlText, CnnString);
            dt1.TableName = "Dealermaster";
            return dt1;

        }
        #endregion
     

        #region InserNewDealerName
        [WebMethod(Description = " Insert Dealer Name RTOLocation Wise")]
        public int InserNewDealerName(int StateId, int RTOID ,string Dealername )
        {
           string CnnString = obj.strProvider;
           string sqlText = "insert into  DealerMaster(dealerName, hsrp_stateid,RTOLocationID) values( '" + Dealername + "','" + StateId + "' , '" + RTOID + "')";
           int k = utils.ExecNonQuery(sqlText, CnnString);

            return k;

        }
        #endregion



        //[XmlInclude(typeof(DataTable))]
    }
}

    
    

