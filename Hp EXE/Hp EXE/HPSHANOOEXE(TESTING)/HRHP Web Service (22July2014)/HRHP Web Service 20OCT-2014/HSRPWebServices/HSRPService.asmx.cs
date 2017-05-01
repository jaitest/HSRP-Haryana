using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Serialization;
using System.IO;
using System.Data.Sql;
using System.Text;
using System.ComponentModel;
using System.Net;

namespace HSRPWebServices
{
    /// <summary>
    /// Summary description for HP HSRPService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/", Description = "Web Service For Haryana and Himachal Cash Collection Exe")]
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


      


        #region GetDataByAuthNo
        [WebMethod]
        public DataTable GetDataByAuthNo(string AuthNo)
        {
            string StrRecord = string.Empty;

            string sqlText = "select top 1 * from [dbo].[HSRPRecordsStaggingArea]  where HSRPRecord_AuthorizationNo='" + AuthNo.ToString() + "' order by HSRPRecord_CreationDate desc";

            string CnnString = obj.strProvider;
            DataTable dt = new DataTable();
            dt = utils.GetDataTable(sqlText, CnnString);

            dt.TableName = "HSRPRecords";

            return dt;

        }

        #endregion

        #region GetDataByVehicleRegNo
        [WebMethod]
        public DataTable GetDataByVehicleRegNo(string VehicleRegNo)
        {
            string StrRecord = string.Empty;
            string sqlText = "select top 1 * from [dbo].[HSRPRecordsStaggingArea] where [VehicleRegNo]='" + VehicleRegNo.ToString().Replace(" ", "") + "' order by HSRPRecord_CreationDate desc";
            // utils obj = new utils();
            string CnnString = obj.strProvider;
            DataTable dt = new DataTable();
            dt = utils.GetDataTable(sqlText, CnnString);
            dt.TableName = "HSRPRecords";
            string hsrp_CreationDate = string.Empty;
            if(hsrp_CreationDate=="")
            {
                
            }
            else
            {
               
            }
            return dt;
        }
        #endregion

        #region InsertDataCashCollection
        [WebMethod]
        public string InsertDataCashCollection(string HSRP_StateID, string RTOLocationID, string HSRPRecord_AuthorizationNo, string HSRPRecord_AuthorizationDate, string VehicleRegNo, string OwnerName, string ownerFatherName, string Address1, string MobileNo, string VehicleClass, string OrderType, string StickerMandatory, string isVIP, string NetAmount, string Roundoff_NetAmount, string VehicleType, string OrderStatus, string CashReceiptNo, string ChassisNo, string EngineNo, string DealerCode, string CreatedBy, string SaveMacAddress, string Addrecordby, string ISFrontPlateSize, string ISRearPlateSize, string FrontPlateSize, string RearPlateSize, string Reference, string ManufacturerModel, string vehicleref, string ManufacturerName, string CounterNo, string Affixid)
        {
            string Status = string.Empty;

            string strDate = System.DateTime.Now.ToString("hh:mm tt");
            string strnine = "09:00 AM";
            string strsix = "06:00 PM";
            if (DateTime.Parse(strDate) < DateTime.Parse(strnine) || DateTime.Parse(strDate) > DateTime.Parse(strsix))
            {
                return Status = "Collection Between 09:00 AM to 06:00 PM." + "^" + VehicleRegNo + "^" + HSRPRecord_AuthorizationNo;
            }
            if (string.IsNullOrEmpty(MobileNo))
            {
                Status = "Mobile No Is Not Valid";
            }
            else
            {
                double dmobile = unchecked(Convert.ToDouble(MobileNo));
                bool checkValidation = Enum.IsDefined(typeof(MobileNoCheck), (long)Math.Round(dmobile));

                if (checkValidation)
                {
                    Status = "Mobile No Is Not Valid";
                }
                else
                {
                    if (VehicleType.ToUpper().Equals("SCOOTER") || VehicleType.ToUpper().Equals("MOTOR CYCLE") || VehicleType.ToUpper().Equals("TRACTOR"))
                    {
                        StickerMandatory = "N";
                    }
                    else
                    {
                        StickerMandatory = "Y";

                    }
                   
                    string CnnString = obj.strProvider;

                  
                    int check;
                    using (SqlConnection con = new SqlConnection(CnnString))
                    {
                        using (SqlCommand cmd = new SqlCommand("Transaction_DataCashCollection_HPOrHR", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@HSRP_StateID", HSRP_StateID.ToString());
                            cmd.Parameters.AddWithValue("@RTOLocationID", RTOLocationID.ToString());
                            cmd.Parameters.AddWithValue("@HSRPRecord_AuthorizationNo", HSRPRecord_AuthorizationNo.ToString());
                            cmd.Parameters.AddWithValue("@HSRPRecord_AuthorizationDate", HSRPRecord_AuthorizationDate.ToString());
                            cmd.Parameters.AddWithValue("@VehicleRegNo", VehicleRegNo.ToString());
                            cmd.Parameters.AddWithValue("@OwnerName", OwnerName.ToString());
                            cmd.Parameters.AddWithValue("@ownerFatherName", ownerFatherName.ToString());
                            cmd.Parameters.AddWithValue("@Address1", Address1.ToString());
                            cmd.Parameters.AddWithValue("@MobileNo", MobileNo.ToString());
                            cmd.Parameters.AddWithValue("@VehicleClass", VehicleClass.ToString());
                            cmd.Parameters.AddWithValue("@OrderType", OrderType.ToString());
                            cmd.Parameters.AddWithValue("@StickerMandatory", StickerMandatory.ToString());
                            cmd.Parameters.AddWithValue("@isVIP", isVIP.ToString());
                            cmd.Parameters.AddWithValue("@NetAmount", NetAmount.ToString());
                            cmd.Parameters.AddWithValue("@Roundoff_NetAmount", Roundoff_NetAmount.ToString());
                            cmd.Parameters.AddWithValue("@VehicleType", VehicleType.ToString());
                            cmd.Parameters.AddWithValue("@OrderStatus", OrderStatus.ToString());
                            cmd.Parameters.AddWithValue("@CashReceiptNo", CashReceiptNo.ToString());
                            cmd.Parameters.AddWithValue("@ChassisNo", ChassisNo.ToString());
                            cmd.Parameters.AddWithValue("@EngineNo", EngineNo.ToString());
                            cmd.Parameters.AddWithValue("@DealerCode", DealerCode.ToString());
                            cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy.ToString());
                            cmd.Parameters.AddWithValue("@SaveMacAddress", SaveMacAddress.ToString());
                            cmd.Parameters.AddWithValue("@Addrecordby", Addrecordby.ToString());
                            cmd.Parameters.AddWithValue("@ISFrontPlateSize", ISFrontPlateSize.ToString());
                            cmd.Parameters.AddWithValue("@ISRearPlateSize", ISRearPlateSize.ToString());
                            cmd.Parameters.AddWithValue("@FrontPlateSize", FrontPlateSize.ToString());
                            cmd.Parameters.AddWithValue("@RearPlateSize", RearPlateSize.ToString());
                            cmd.Parameters.AddWithValue("@Reference", Reference.ToString());
                            cmd.Parameters.AddWithValue("@ManufacturerModel", ManufacturerModel.ToString());
                            cmd.Parameters.AddWithValue("@vehicleref", vehicleref.ToString());
                            cmd.Parameters.AddWithValue("@ManufacturerName", ManufacturerName.ToString());
                            cmd.Parameters.AddWithValue("@CounterNo", CounterNo.ToString());
                            cmd.Parameters.AddWithValue("@Affixid", Affixid);
                            //cmd.Parameters.AddWithValue("@RegistrationDate", date);
                            //cmd.Parameters.Add("@ReturnId", SqlDbType.Int);
                            //cmd.Parameters["@ReturnId"].Direction = ParameterDirection.Output;
                            con.Open();
                             check   =   cmd.ExecuteNonQuery();
                            con.Close();
                            //if (cmd.Parameters["@ReturnId"].Value.ToString() != "")
                            //{
                            //    check = Convert.ToInt32(cmd.Parameters["@ReturnId"].Value);
                            //}
                            //else
                            //{
                            //    check = 0;
                            //}

                        }
                    }                
                    
                    
               
                    
                    if (check > 0)
                    {
                        Status = "Record Saved";                        
                    }
                    else
                    { 
                        Status = "Record Not Saved" + "^" + VehicleRegNo + "^" + HSRPRecord_AuthorizationNo;

                    }
                }
            }
            return Status;
        }
        #endregion

        #region GetRtoLocationID
        [WebMethod]
        public string GetRtoLocationID(string strRto_Cd, string strHsrp_StateId)
        {
            string strRTOCode = strRto_Cd.Substring(strRto_Cd.Length - 2);
            string StrRecord = string.Empty;
            string sqlText = "select RTOLocationID from Rtolocation where hsrp_stateid='" + strHsrp_StateId + "' and nicrtolocationid='" + strRTOCode + "'";
            string CnnString = obj.strProvider;
            DataTable dt = new DataTable();
            dt = utils.GetDataTable(sqlText, CnnString);
            dt.TableName = "vehicletype";
            return dt.Rows[0]["Rtolocationid"].ToString();
        }
        #endregion

        #region CashRecieptNo,Invoice No, Challan No

        #region GetCash Invoice Challan No
        [WebMethod(Description = "Get the Cash Reciept,Challan No and Invoice No As Per StateId and RtoLocationId")]
        public String GetCashInvoiceChallan(string strHsrp_stateID, string RtoLocationId, string PreFix)
        {
            string CashReciptNo = String.Empty;
            string CnnString = obj.strProvider;
            string SqlString = "select (prefixtext+right('00000'+ convert(varchar,lastno+1),5)) as Receiptno from prefix  where hsrp_stateid='" + strHsrp_stateID + "' and rtolocationid ='" + RtoLocationId + "' and prefixfor='" + PreFix + "'";
            CashReciptNo = utils.getScalarValue(SqlString, CnnString);
            return CashReciptNo;
        }
        #endregion

        #region UpdateCashRecieptNo In Prefix
        [WebMethod(Description = "Update the Cash Reciept,Challan No and Invoice No RtoLocationId Wise")]
        public void UpdateCashInvoiceChallan(string RtoLocationId, string PreFix)
        {
            string CnnString = obj.strProvider;
            string SqlString = "update prefix set lastno=lastno+1 where  rtolocationid ='" + RtoLocationId + "' and prefixfor='" + PreFix + "'";
            int u = utils.ExecNonQuery(SqlString, CnnString);
        }
        #endregion

        #endregion

        #region Get RTO LOCATION ADDRESS
        [WebMethod(Description = "Get The Rto Location Address Using Rto Location Id")]
        public String GetRtoAddress(string HSRPStateID, string RtoLocationid)
        {
            string StrRecord = string.Empty;
            string sqlText = "select RTOLocationAddress from dbo.RTOLOCATION  where hsrp_stateid ='" + HSRPStateID + "' and Rtolocationid ='" + RtoLocationid + "'";

            string CnnString = obj.strProvider;
            DataTable dt = new DataTable();
            dt = utils.GetDataTable(sqlText, CnnString);
            if (dt.Rows.Count > 1)
            {
                StrRecord = dt.Rows[0]["RTOLOCATIONAddress"].ToString();
            }
            return StrRecord;
        }
        #endregion
        #region CheckOrdertype
        [WebMethod(Description = "This Method Is used to check order type")]
        public void CheckOrderType(out string strOrderType, string OrderType)
        {

            if (OrderType == "NEW BOTH PLATES")
            {
                strOrderType = "NB";
            }
            else if (OrderType == "OLD BOTH PLATES")
            {
                strOrderType = "OB";
            }
            else if (OrderType == "DAMAGED BOTH PLATES")
            {
                strOrderType = "DB";
            }
            else if (OrderType == "DAMAGED FRONT PLATE")
            {
                strOrderType = "DF";
            }
            else if (OrderType == "DAMAGED REAR PLATE")
            {
                strOrderType = "DR";
            }
            else if (OrderType == "ONLY STICKER")
            {
                strOrderType = "OS";
            }
            else
            {
                strOrderType = OrderType;
            }
        }
        #endregion
        #region CheckDuplicateEntry
        [WebMethod]
        public String CheckDuplicateEntry(string HSRPStateID, string AuthNo, string VehicleRegNo, string OrderType)
        {
            string StrRecord = string.Empty;
            string strOrderType = string.Empty;           
            
            CheckOrderType(out strOrderType, OrderType);

            string sqlText = "select COUNT(VehicleRegNo) as co from dbo.HSRPRecords  where hsrp_stateid ='" + HSRPStateID + "' and LTRIM(RTRIM( VehicleRegNo)) ='" + VehicleRegNo.Trim().Replace(" ", "") + "' and OrderType='" + strOrderType + "' and LTRIM(RTRIM( HSRPRecord_AuthorizationNo))='" + AuthNo.Trim().Replace(" ", "") + "'";

            string CnnString = obj.strProvider;
            DataTable dt = new DataTable();
            dt = utils.GetDataTable(sqlText, CnnString);
           
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
            string CnnString = obj.strProvider;
            SqlText = "select * from Users where [UserLoginName]='" + UserName + "' and [Password]='" + Password + "'";
            DataTable dt = new DataTable();
            dt = utils.GetDataTable(SqlText, CnnString);
            if (dt.Rows.Count > 0)
            {
                Status = dt.Rows[0]["ActiveStatus"].ToString();
                StateID = dt.Rows[0]["HSRP_StateID"].ToString();
                RtoLocationID = dt.Rows[0]["RTOLocationID"].ToString();
                UserID = dt.Rows[0]["UserID"].ToString();
                if (Status == "Y" || Status == "y")
                {
                    SqlText = "select * from [MACBase] where [MacAddress]='" + MacAddress + "'";
                    utils.ExecNonQuery("UPDATE users set lastLoginDatetime=GetDate() where userid='" + UserID + "'", CnnString);
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
            return Status + "^" + MacStatus + "^" + UserID + "^" + StateID + "^" + RtoLocationID;
        }
        #endregion
        #region GetCashReceiptNo For HR
        [WebMethod(Description=" Get Cash Receipt No using AuthNo")]
        public string CashReceiptForHR(string AuthNo)
        {
            string CnnString = obj.strProvider;
            string strQuery = string.Empty;
            string strCashNo = string.Empty;
            strQuery = "select CashReceiptNo from hsrprecords where hsrp_stateid=4 and HSRPRecord_AuthorizationNo='"+AuthNo+"'";
            strCashNo = utils.getDataSingleValue(strQuery, CnnString, "CashReceiptNo");
            return strCashNo;

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
            string sqlText = "select vehicletype,vehicletypeid from vehicletype where shortname is not null";
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
            string sqlText = "select VehicleMakerDescription,VehicleMakerID from VehicleMakerMaster where activestatus='Y'";
            //string sqlText = "select VehicleModelDescription,VehicleModelID from VehicleModelMaster";
            string CnnString = obj.strProvider;
            DataTable dt = new DataTable();
            dt = utils.GetDataTable(sqlText, CnnString);
            dt.TableName = "VehicleMakerMaster";
            return dt;
        }


        #endregion

        #region OrderClosed_Validate



        [WebMethod]
        public DataTable RecordClosed(string VehicleRegNo)
        {

            string CnnString = obj.strProvider;
            string Query = string.Empty;

            DataTable dt2 = new DataTable();

            Query = "select * from HSRPRecords where vehicleregno='" + VehicleRegNo + "' and orderstatus in ('Closed')";

            dt2 = utils.GetDataTable(Query, CnnString);
            dt2.TableName = "HSRPRecords";
            return dt2;
        }
        [WebMethod]
        public DataTable RecordNotClosed(string VehicleRegNo)
        {

            string CnnString = obj.strProvider;
            string Query = string.Empty;

            Query = "select * from HSRPRecords where vehicleregno='" + VehicleRegNo + "' and orderstatus not in ('Closed')";
            dt1 = utils.GetDataTable(Query, CnnString);
            dt1.TableName = "HSRPRecords";
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
            string sqlText = "select vehicleregno from HSRPRecords where hsrp_front_lasercode='" + Flaser + "' or hsrp_rear_lasercode='" + Flaser + "'";
            laservalue = utils.getDataSingleValue(sqlText, CnnString, "vehicleregno");
            return laservalue;
        }

        [WebMethod]
        public string RearValidation(string Rlaser)        //same method for Embossing
        {
            string CnnString = obj.strProvider;
            string sqlText = "select vehicleregno from HSRPRecords where hsrp_front_lasercode='" + Rlaser + "' or hsrp_rear_lasercode='" + Rlaser + "'";
            laservalue = utils.getDataSingleValue(sqlText, CnnString, "vehicleregno");
            return laservalue;
        }

        [WebMethod]
        public DataTable OrderStatusNotClosed(string VehiclRegNo)
        {
            string CnnString = obj.strProvider;
            string sqlText = "select * from HSRPRecords where vehicleregno = '" + VehiclRegNo + "' and orderstatus not in ('Closed')";
            dt1 = utils.GetDataTable(sqlText, CnnString);
            dt1.TableName = "HSRPRecords";
            return dt1;
        }

        [WebMethod]
        public int OrderStatusEmbossingDone(string Flaser, string Rlaser, string VehicleRegNo)
        {
            string CnnString = obj.strProvider;
            string sqlText = "update HSRPRecords set  ordercloseddate = getdate(),orderstatus='Closed', hsrp_front_lasercode='" + Flaser + "', hsrp_rear_lasercode ='" + Rlaser + "' where vehicleregno ='" + VehicleRegNo + "' and orderstatus='Embossing Done'";
            int j = utils.ExecNonQuery(sqlText, CnnString);
            return j;

        }

        [WebMethod]
        public int OrderStatusEmbossingDoneUpdate(string Flaser, string Rlaser, string VehicleRegNo)
        {
            string CnnString = obj.strProvider;
            string sqlText = "update HSRPRecords set HSRP_NO_FRONT='" + Flaser + "',HSRP_NO_back= '" + Rlaser + "',HSRP_FIX_DT =getdate() where replace(REGN_NO,' ','') ='" + VehicleRegNo + "'";
            int j = utils.ExecNonQuery(sqlText, utils.CnnString);
            return j;
        }
        [WebMethod]
        public int OrderStatusNewOrder(string Flaser, string Rlaser, string VehicleRegNo)
        {
            string CnnString = obj.strProvider;
            string sqlText = "update HSRPRecords set  ordercloseddate = getdate(),orderembossingdate = getdate(),orderstatus='Closed', hsrp_front_lasercode='" + Flaser + "', hsrp_rear_lasercode ='" + Rlaser + "' where vehicleregno ='" + VehicleRegNo + "' and orderstatus='New Order'";
            int j = utils.ExecNonQuery(sqlText, CnnString);
            return j;
        }

        [WebMethod]
        public int OrderStatusNewOrderUpdate(string Flaser, string Rlaser, string VehicleRegNo)
        {
            string CnnString = obj.strProvider;
            string sqlText = "update HSRPRecords set HSRP_NO_FRONT='" + Flaser + "',HSRP_NO_back= '" + Rlaser + "',HSRP_ISSUE_DT =getdate(),HSRP_FIX_DT =getdate() where replace(REGN_NO,' ','') ='" + VehicleRegNo + "'";
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
            string sqlText = "select * from HSRPRecords where vehicleregno='" + VehiclRegNo + "' and orderstatus='New Order'";
            dt1 = utils.GetDataTable(sqlText, CnnString);
            dt1.TableName = "HSRPRecords";
            return dt1;
        }

        #endregion

        #region Embossing_Save

        [WebMethod]
        public DataTable Embossing_OrderStatusNotClosed(string VehiclRegNo)
        {
            string CnnString = obj.strProvider;
            string sqlText = "select * from HSRPRecords where vehicleregno = '" + VehiclRegNo + "' and orderstatus='New Order'";
            dt1 = utils.GetDataTable(sqlText, CnnString);
            dt1.TableName = "HSRPRecords";
            return dt1;
        }
        [WebMethod]
        public int E_OrderStatusEmbossingDone(string Flaser, string Rlaser, string VehicleRegNo)
        {
            string CnnString = obj.strProvider;
            string sqlText = "update HSRPRecords set  orderembossingdate = getdate(),orderstatus='Embossing Done', hsrp_front_lasercode='" + Flaser + "', hsrp_rear_lasercode ='" + Rlaser + "' where vehicleregno ='" + VehicleRegNo + "' and orderstatus='New Order'";
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
        //    sqlText = "select top 10 vehicleregno from HSRPRecords where vehicleregno like '" + RegNo + "%'";
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
            string sqlText = "select * from HSRPState WHERE HSRP_StateID='" + StateId + "'";
            dt1 = utils.GetDataTable(sqlText, CnnString);
            dt1.TableName = "HSRPState";
            return dt1;

        }
        #endregion

        #region Save SMS Record
        [WebMethod(Description = "Save SMS Record at the Time Of Cash Reciept")]
        public void SaveSMSLog(string strHSRP_StateId, string strMobileNo, string strRegNo, string strAuthNo, string strHsrp_Flag, string strSmsText, string smsresponseid, string smsresponsetext)
        {
            string CnnString = obj.strProvider;
            string sqlText = "insert into [hsrpdemo].[dbo].[SMSlog_HP]([hsrp_stateid],[MobileNo],[REGN_NO],[AUTH_NO],[HSRP_FLAG],[CashReceiptSmsText],[CashReceiptSMSDateTime],[CashReceiptSMSServerResponseID],[CashReceiptSMSServerResponseText]) values" +
                "('" + strHSRP_StateId.Replace(" ", "") + "','" + strMobileNo.Replace(" ", "") + "','" + strRegNo.Replace(" ", "") + "','" + strAuthNo + "','" + strHsrp_Flag + "','" + strSmsText + "',getdate(),'" + smsresponseid + "','" + smsresponsetext + "')";
            try
            {
                utils.ExecNonQuery(sqlText, CnnString);
            }
            catch
            {
            }
        }

        #endregion
        #region GetInfoByVehicleRegNoForHP
        [WebMethod(Description = "Fetch Deatils of VehicleRegno")]
        public DataTable GetDeatilsUsingVehcileRegNo(string HsrpStateID, string VehicleRegno)
        {
            string CnnString = obj.strProvider;
            string SqlQuery = "select VehicleRegNo,Vehicletype,ChassisNo,EngineNo,OrderType,HsrpRecord_AuthorizationNo from hsrprecords where hsrp_stateid='" + HsrpStateID + "' and VehicleRegNo='" + VehicleRegno + "' and orderstatus='New Order'";
            dt1 = utils.GetDataTable(SqlQuery, CnnString);
            dt1.TableName = "dtsend";
            return dt1;
        }

        #endregion

        #region GetInfoByVehicleRegNoForClosedOrder
        [WebMethod(Description = "Fetch Deatils of Closed Order Status VehicleRegno")]
        public DataTable GetDeatilsUsingVehcileRegNoForClosed(string HsrpStateID, string VehicleRegno)
        {
            string CnnString = obj.strProvider;
            string SqlQuery = "select VehicleRegNo,Vehicletype,MobileNo,ChassisNo,orderstatus,EngineNo,OrderType,hsrp_front_lasercode,hsrp_rear_lasercode,OrderStatus,HsrpRecord_AuthorizationNo from hsrprecords where hsrp_stateid='" + HsrpStateID + "' and VehicleRegNo='" + VehicleRegno + "' and  orderstatus not in ('Closed')";
            dt1 = utils.GetDataTable(SqlQuery, CnnString);
            dt1.TableName = "dtsend";
            return dt1;
        }
        #endregion
        #region CheckIsOrderClosedOrNot
        [WebMethod(Description = "Check Order Status Closed Or Not")]
        public DataTable CheckOrderstatus(string strHsrpStateID, string strVehicleReg)
        {
            string CnnString = obj.strProvider;
            string strMessage = string.Empty;
            string SqlQuery = "select * from hsrprecords where hsrp_stateid='" + strHsrpStateID + "' and VehicleRegNo='" + strVehicleReg + "' and orderstatus in ('New Order')";
            dt1 = utils.GetDataTable(SqlQuery, CnnString);
            dt1.TableName = "dtsend";
            return dt1;
        }
        #endregion
        #region CheckOrderStatusEmbossedOrNot
        [WebMethod]
        public DataTable CheckOrderstatusForEmbossing(string strHsrpStateID, string strVehicleReg)
        {
            string CnnString = obj.strProvider;
            string strMessage = string.Empty;
            string SqlQuery = "select * from hsrprecords where hsrp_stateid='" + strHsrpStateID + "' and VehicleRegNo='" + strVehicleReg + "' and orderstatus in ('Embossing Done')";
            dt1 = utils.GetDataTable(SqlQuery, CnnString);
            dt1.TableName = "dtsend";
            return dt1;
        }
        #endregion


        #region CheckLaserCode
        [WebMethod(Description = "Check The Laser No Availablity")]
        public string ValidateLaserCode(string FrontLaserNo, string RearLaserCode)
        {
            string CnnString = obj.strProvider;
            string SqlQuery = string.Empty;
            string Message = string.Empty;
            DataTable dtResult = new DataTable();
            SqlQuery = "Select Laserno,inventorystatus from RtoInventory where Laserno='" + FrontLaserNo + "'";
            dtResult = utils.GetDataTable(SqlQuery, CnnString);
            if (dtResult.Rows.Count > 0)
            {
                if (dtResult.Rows[0]["inventorystatus"].ToString().ToUpper() == "NEW ORDER")
                {
                    Message = "Success";
                }
                else
                {
                    Message = "FrontUsed";
                }
            }
            else if (dtResult.Rows.Count == 0)
            {
                Message = "FrontNotFound";
            }
            SqlQuery = "Select Laserno,inventorystatus from RtoInventory where Laserno='" + RearLaserCode + "'";
            dtResult = utils.GetDataTable(SqlQuery, CnnString);
            if (dtResult.Rows.Count > 0)
            {
                if (dtResult.Rows[0]["inventorystatus"].ToString().ToUpper() == "NEW ORDER")
                {
                    Message = Message + "^Success";
                }
                else
                {
                    Message = Message + "^RearUsed";
                }
            }
            else if (dtResult.Rows.Count == 0)
            {
                Message = Message + "^RearNotFound";
            }
            return Message;
        }
        #endregion
        #region UpdateLaserCode
        [WebMethod]
        public string UpdateLaserCode(string hsrpStateid, string VehicleRegno, string ordertype, string FrontLaser, string Rearlaser)
        {
            string SqlQuery = string.Empty;
            string CnnString = obj.strProvider;
            string strMessage = string.Empty;
            StringBuilder strbulider = new StringBuilder();
            strbulider.Append("update HsrpRecords set  orderembossingdate = getdate(),orderstatus='Embossing Done',");
            string strLsrType = string.Empty;
            switch (ordertype)
            {
                case "DF":
                    strbulider.Append("hsrp_front_lasercode='" + FrontLaser + "'");
                    strLsrType = FrontLaser;
                    break;
                case "DR":
                    strbulider.Append("hsrp_rear_lasercode='" + Rearlaser + "'");
                    strLsrType = Rearlaser;
                    break;
                default:
                    strbulider.Append("hsrp_front_lasercode='" + FrontLaser + "',hsrp_rear_lasercode='" + Rearlaser + "'");
                    strLsrType = FrontLaser + "," + Rearlaser;
                    break;
            }
            strbulider.Append("where hsrp_stateid='" + hsrpStateid + "'and vehicleregno ='" + VehicleRegno + "' and orderstatus='New Order'");


            int iResult = utils.ExecNonQuery(strbulider.ToString(), CnnString);
            if (iResult > 0)
            {

                string strQry = "update RTOInventory set inventorystatus='Embossing Done',EmbossingDate=getdate() from RTOInventory where  laserno in ('" + strLsrType + "')";
                utils.ExecNonQuery(strQry, CnnString);
                strMessage = "Success";

            }
            return strMessage;
        }
        #endregion

        #region CheckVehicleregnoForEmbossingInHSRpRecords



        [WebMethod]
        public string CheckLaserInHsrpRecords(string strOrederType, string strFrontLaser, string strRearlaser)
        {
            string Message = string.Empty;
            string SqlQuery = string.Empty;
            string CnnString = obj.strProvider;
            DataTable dtResult = new DataTable();
            if (strOrederType != "DR")
            {
                SqlQuery = "Select vehicleregno from hsrprecords where hsrp_front_lasercode='" + strFrontLaser + "' or hsrp_rear_lasercode='" + strFrontLaser + "'";
                dtResult = utils.GetDataTable(SqlQuery, CnnString);
                if (dtResult.Rows.Count > 0)
                {
                    Message = "FrontUsed";

                }
            }
            else if (strOrederType != "DF")
            {
                SqlQuery = "select vehicleregno from hsrprecords where  hsrp_front_lasercode='" + strRearlaser + "' or hsrp_rear_lasercode='" + strRearlaser + "'";
                dtResult = utils.GetDataTable(SqlQuery, CnnString);
                if (dtResult.Rows.Count > 0)
                {
                    Message = "RearUsed";
                }
            }

            return Message;

        }
        #endregion
        #region UpdateForClosedRecord
        [WebMethod]
        public string UpdateClosedorder(string strHsrpStateID, string strRegno, string strFront, string strRear)
        {
            string SqlQuery = string.Empty;
            string Message = string.Empty;
            string CnnString = obj.strProvider;
            SqlQuery = "update hsrprecords set ordercloseddate = getdate(),orderstatus='Closed', hsrp_front_lasercode='" + strFront + "', hsrp_rear_lasercode ='" + strRear + "' where vehicleregno ='" + strRegno + "' and orderstatus='Embossing Done'";

            int i = utils.ExecNonQuery(SqlQuery, CnnString);
            if (i > 0)
            {
                return Message = "Success";
            }
            else
            {
                return Message = "Fail";
            }

        }
        #endregion

        #region UpdateHPSmsLog
        [WebMethod]
        public string UpdateHpSmsLog(string strMobileNo, string strMessage, string strResult, string strAuthNo)
        {
            string SqlQuery = string.Empty;
            string CnnString = obj.strProvider;
            string strQueryResult = string.Empty;
            SqlQuery = "update [SMSlog_HP] set [FirstSMSText]='" + strMessage + "',[FirstSMSSentDateTime]=getdate(),[FirstSMSServerResponseID]='" + strResult + "' where [MobileNo]='" + strMobileNo + "' and [AUTH_NO]='" + strAuthNo + "'";
            int i = utils.ExecNonQuery(SqlQuery, CnnString);
            if (i > 0)
            {
                return strQueryResult = "Success";
            }
            else
            {
                return strQueryResult = "Fail";
            }
        }
        #endregion
        #region updateHPSMSLOGClosing
        [WebMethod]
        public string UpdateHpSmsLogOnClosed(string strMobileNo, string strMessage, string strResult, string strAuthNo)
        {
            string SqlQuery = string.Empty;
            string CnnString = obj.strProvider;
            string strQueryResult = string.Empty;
            SqlQuery = "update [SMSlog_HP] set [SecondSMSText]='" + strMessage + "',[SecondSMSSentDateTime]=getdate(),[SecondSMSServerResponseID]='" + strResult + "' where [MobileNo]='" + strMobileNo + "' and [AUTH_NO]='" + strAuthNo + "'";
            int i = utils.ExecNonQuery(SqlQuery, CnnString);
            if (i > 0)
            {
                return strQueryResult = "Success";
            }
            else
            {
                return strQueryResult = "Fail";
            }
        }
        #endregion


        #region ReportData
        [WebMethod(Description = "Method For GetReportDate")]
        public DataTable getReportData(string strhsrpStateid, string strRtoLocationId, string strStartDate, string strEndDate)
        {
            string SqlQuery = string.Empty;
            string CnnString = obj.strProvider;
            SqlQuery = "SELECT HSRPRecord_CreationDate, r.NICRTOLocationID,HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate,VehicleRegNo,OwnerName,address1 ,h.MobileNo,VehicleClass,OrderType,VehicleType,CashReceiptNo, roundoff_netAmount,ChassisNo ,EngineNo,hsrp_front_lasercode,hsrp_rear_lasercode,OrderStatus,OrderEmbossingDate,OrderClosedDate FROM hsrprecords as h inner join rtolocation as r on r.rtolocationid=h.rtolocationid where h.hsrp_stateid='" + strhsrpStateid + "' and h.rtolocationid='" + strRtoLocationId + "' and  (hsrpRecord_CreationDate between '" + strStartDate + "' and '" + strEndDate + "') or (OrderEmbossingDate between '" + strStartDate + "' and '" + strEndDate + "') or (OrderClosedDate between '" + strStartDate + "' and '" + strEndDate + "') ";
            DataTable dtReport = new DataTable("dtReport");
            dtReport = utils.GetDataTable(SqlQuery, CnnString);
            //    dtReport.TableName = "dtnew";
            return dtReport;
        }
        #endregion
        #region ReminderForSMS
        //public string CheckForSMSReminder()
        //{
        //     string SqlQuery = string.Empty;
        //     string CnnString = obj.strProvider;

        //   SqlQuery="select top 10 * from OrderBookingOffLine   WHERE OrderStatus ='Embossing Done' and DATEDIFF(Day,Record_CreationDate,GETDATE()) >8 and fourthsmstext is null   order by RecordID desc";
        //}
        #endregion


        #region MobileNo Check Validation
        public enum MobileNoCheck : long
        {
            Zero = 0000000000,
            One = 1111111111,
            Two = 2222222222,
            Three = 3333333333,
            Four = 4444444444,
            Five = 5555555555,
            Six = 6666666666,
            Seven = 7777777777,
            Eight = 8888888888,
            Nine = 9999999999
        }
        #endregion

        #region ReminderForSMS
        [WebMethod(Description = "For Update SmsLog_HP for Reminders")]
        public void updateFourthSms(string strHsrp_stateid, string strVehicleRegNo, string strMessage, string strResult)
        {
            string SqlQuery = string.Empty;
            string CnnString = obj.strProvider;
            SqlQuery = "update SMSlog_HP set  FourthSMSText ='" + strMessage + "',FourthSMSSentDateTime=getdate(),FourthSMSServerResponseID='" + strResult + "'  where regn_no = '" + strVehicleRegNo + "'";
            utils.ExecNonQuery(SqlQuery, CnnString);

        }
        [WebMethod(Description = "For Update SmsLog_HP for Reminders")]
        public void updateFifthSms(string strHsrp_stateid, string strVehicleRegNo, string strMessage, string strResult)
        {
            string SqlQuery = string.Empty;
            string CnnString = obj.strProvider;
            SqlQuery = "update SMSlog_HP set  FifthSMSText ='" + strMessage + "',FifthSMSDateTime=getdate(),FifthSMSServerResponseID='" + strResult + "'  where regn_no = '" + strVehicleRegNo + "'";
            utils.ExecNonQuery(SqlQuery, CnnString);
        }
        #endregion
        #region LaserCheckForClosed
        [WebMethod]
        public string ValidateLaserCodeForOrderClose(string FrontLaserNo, string RearLaserCode)
        {
            string CnnString = obj.strProvider;
            string SqlQuery = string.Empty;
            string Message = string.Empty;
            DataTable dtResult = new DataTable();
            SqlQuery = "Select Laserno,inventorystatus from RtoInventory where Laserno='" + FrontLaserNo + "'";
            dtResult = utils.GetDataTable(SqlQuery, CnnString);
            if (dtResult.Rows.Count > 0)
            {
                if (dtResult.Rows[0]["inventorystatus"].ToString().ToUpper() == "NEW ORDER" || dtResult.Rows[0]["inventorystatus"].ToString().ToUpper() == "EMBOSSING DONE")
                {
                    Message = "Success";
                }
                else
                {
                    Message = "FrontUsed";
                }
            }
            else if (dtResult.Rows.Count == 0)
            {
                Message = "FrontNotFound";
            }
            SqlQuery = "Select Laserno,inventorystatus from RtoInventory where Laserno='" + RearLaserCode + "'";
            dtResult = utils.GetDataTable(SqlQuery, CnnString);
            if (dtResult.Rows.Count > 0)
            {
                if (dtResult.Rows[0]["inventorystatus"].ToString().ToUpper() == "NEW ORDER" || dtResult.Rows[0]["inventorystatus"].ToString().ToUpper() == "EMBOSSING DONE")
                {
                    Message = Message + "^Success";
                }
                else
                {
                    Message = Message + "^RearUsed";
                }
            }
            else if (dtResult.Rows.Count == 0)
            {
                Message = Message + "^RearNotFound";
            }
            return Message;
        }
        #endregion
        //-------------------------------------- Methods For Portability Request------------------------------------------------------------------
        #region FillRTOLocation
        [WebMethod(Description = "Fill RTO Dropdown List")]
        public DataTable FillRTODropDown(string strStateID)
        {
            string CnnString = obj.strProvider;
            string SqlQuery = string.Empty;
            DataTable dtRTO = new DataTable();
            SqlQuery = "select RTOLocationName,NICRTOLocationID from RTOLocation where hsrp_stateid='" + strStateID + "'";
            dtRTO = utils.GetDataTable(SqlQuery, CnnString);
            dtRTO.TableName = "RTOLocation";
            return dtRTO;

        }

        #endregion
        #region GetRTOLocationID
        [WebMethod(Description = "Get RTO Location Id on basis of NICRTO Location code")]
        public string GetRTO_ID(string strStateID, string strNICcode)
        {
            string CnnString = obj.strProvider;
            string SqlQuery = string.Empty;
            DataTable dtRTOID = new DataTable();
            SqlQuery = "select RTOLocationID from RTOLocation where hsrp_stateid='" + strStateID + "' and NICRTOLocationID='" + strNICcode + "'";
            dtRTOID = utils.GetDataTable(SqlQuery, CnnString);
            dtRTOID.TableName = "RTOLocation";
            return dtRTOID.Rows[0]["Rtolocationid"].ToString();
        }
        #endregion

        #region Insertdata in Portability Request
        [WebMethod(Description = "Save Data in HP_PortabilityData")]
        public string InsertData(string strRTOLocationID, string strVehicleNo, string strChassisNo, string strOrdertype, string strNICRTOCode, string strUserId)
        {
            string CnnString = obj.strProvider;
            string SqlQuery = string.Empty;
            string strStatus = string.Empty;
            SqlQuery = "insert into HP_PortabilityData ([RTOLOCATION_ID],[REGN_NO],[CHASI_NO]," +
                "[HSRP_FLAG],[RTO_CD],[USER_ID]) values('" + strRTOLocationID + "','" + strVehicleNo + "','" + strChassisNo + "','" + strOrdertype + "','" + strNICRTOCode + "','" + strUserId + "')";
            int i = utils.ExecNonQuery(SqlQuery, CnnString);
            if (i > 0)
            {
                return strStatus = "Y";
            }
            else
            {
                return strStatus = "N";
            }


        }
        #endregion

        #region Method For Check Duplicate Entry
        [WebMethod(Description = "Check Duplicate Entry in HP_PortabilityData Table")]
        public string CheckDuplicateRecordInHP_PortabilityData(string strVehicleReg)
        {
            string CnnString = obj.strProvider;
            string SqlQuery = string.Empty;
            string strStatus = string.Empty;
            SqlQuery = "Select * from HP_PortabilityData where REGN_NO='" + strVehicleReg.Replace(" ", "") + "' and RETRIEVE_FLAG in('N','Y','R','E') ";
            DataTable dtCheck = utils.GetDataTable(SqlQuery, CnnString);
            if (dtCheck.Rows.Count > 0)
            {
                return strStatus = "Y";
            }
            else
            {
                return strStatus = "N";
            }

        }

        #endregion
        #region FillDataGridView
        [WebMethod(Description = "Fill  DataGrid View")]
        public DataTable FillDataGridView(string strNICRTOCode)
        {
            string CnnString = obj.strProvider;
            string SqlQuery = string.Empty;
            DataTable dtGridResult = new DataTable();
            SqlQuery = "select ROW_NUMBER() over (order by [REGN_NO]) as [S.NO],[REGN_NO] as [Vehicle RegNo],[REQUEST_DATE] as [Request Date]," +
                "case when [RETRIEVE_FLAG]='N' then 'No Record Found' when [RETRIEVE_FLAG]='Y' then 'Record Found' when [RETRIEVE_FLAG]='R' then 'New Order' when [RETRIEVE_FLAG]='E' then 'Embossing Done' " +
                " when [RETRIEVE_FLAG]='C' or [RETRIEVE_FLAG]='D' then 'Order Closed' end   as [Portable Status] from HP_PortabilityData where [RTO_CD]='" + strNICRTOCode + "'";
            dtGridResult = utils.GetDataTable(SqlQuery, CnnString);
            dtGridResult.TableName = "HP_PortabilityData";
            return dtGridResult;
        }
        #endregion

        #region GetData For Booking
        [WebMethod(Description = "Get Data For Order Booking......")]
        public DataTable GetDataForBooking(string strVehicleRegNo)
        {
            string CnnString = obj.strProvider;
            string strSqlQuery = string.Empty;
            DataTable dtResult = new DataTable();
            strSqlQuery = "select * from HP_PortabilityData where  [REGN_NO]='" + strVehicleRegNo + "'";
            dtResult = utils.GetDataTable(strSqlQuery, CnnString);
            dtResult.TableName = "HP_PortabilityData";
            return dtResult;
        }
        #endregion

        #region UpdateMethodFor Order Closed
        [WebMethod(Description = "Method Used to update Closed Order in Portability table ")]
        public string UpdateForOrderClosed(string strvehicleRegno, string strFront, string strRear, string strNICRtoId)
        {
            string CnnString = obj.strProvider;
            string SqlQuery = string.Empty;
            string strStatus = string.Empty;
            SqlQuery = "update HP_PortabilityData set HSRP_NO_FRONT='" + strFront + "',HSRP_NO_back= '" + strRear + "',HSRP_ISSUE_DT =getdate(),HSRP_FIX_DT =getdate(),RETRIEVE_FLAG='C' where [RTO_CD]='" + strNICRtoId + "' and  REGN_NO ='" + strvehicleRegno + "'";
            int strQueryResult = utils.ExecNonQuery(SqlQuery, CnnString);
            if (strQueryResult > 0)
            {
                return strStatus = "Y";

            }
            else
            {
                return strStatus = "N";
            }
        }
        #endregion

        #region UpdateMethodFor Order Embossing
        [WebMethod(Description = "Method Used to update Embossed Order in Portability table ")]
        public string UpdateForOrderEmbossed(string strvehicleRegno, string strFront, string strRear, string strOrderType, string strNICRtoId)
        {
            string CnnString = obj.strProvider;
            string SqlQuery = string.Empty;
            string strStatus = string.Empty;

            if (strOrderType == "DF")      //added by amit rohila on date 23/10/2013
            {
                SqlQuery = "update HP_PortabilityData set HSRP_NO_FRONT='" + strFront + "',HSRP_ISSUE_DT =getdate(),RETRIEVE_FLAG='E' where  [RTO_CD]='" + strNICRtoId + "' and REGN_NO ='" + strvehicleRegno + "'";
            }
            else if (strOrderType == "DR")
            {
                SqlQuery = "update HP_PortabilityData set HSRP_NO_back= '" + strRear + "',HSRP_ISSUE_DT =getdate(),RETRIEVE_FLAG='E' where  [RTO_CD]='" + strNICRtoId + "' and REGN_NO ='" + strvehicleRegno + "'";
            }
            else
            {
                SqlQuery = "update HP_PortabilityData set HSRP_NO_FRONT='" + strFront + "',HSRP_NO_back= '" + strRear + "',HSRP_ISSUE_DT =getdate(),RETRIEVE_FLAG='E' where [RTO_CD]='" + strNICRtoId + "' and REGN_NO ='" + strvehicleRegno + "'";
            }

            int strQueryResult = utils.ExecNonQuery(SqlQuery, CnnString);
            if (strQueryResult > 0)
            {
                return strStatus = "Y";

            }
            else
            {
                return strStatus = "N";
            }
        }
        #endregion


        #region UpdateMethodFor Order Book
        [WebMethod(Description = "Method Used to update Book Order in Portability table ")]
        public string UpdateForOrderBooking(string strvehicleRegno, string strAuthNo, string strAmount, string strNICRtoId)
        {
            string CnnString = obj.strProvider;
            string SqlQuery = string.Empty;
            string strStatus = string.Empty;
            SqlQuery = "update HP_PortabilityData set HSRP_FIX_AMT='" + strAmount + "',HSRP_AMT_TAKEN_ON=getdate(),RETRIEVE_FLAG='R' where AUTH_NO='" + strAuthNo + "' and [RTO_CD]='" + strNICRtoId + "' and REGN_NO ='" + strvehicleRegno + "'";
            int strQueryResult = utils.ExecNonQuery(SqlQuery, CnnString);
            if (strQueryResult > 0)
            {
                return strStatus = "Y";

            }
            else
            {
                return strStatus = "N";
            }
        }
        #endregion

        #region Extract Value With the Help Of Vehicle Reg No For Sticker
        [WebMethod(Description = "Get Data For Sticker......")]
        public DataSet GetDataForSticker(string strVehicleRegNo)
        {
            DataSet dsResult = new DataSet();
            DataTable dtResult = new DataTable();
            DataTable dtResult2 = new DataTable();
            string CnnString = obj.strProvider;
            string strSqlQuery = string.Empty;
            strSqlQuery = "select * from HP_PortabilityData where VH_CLASS in ('LMV','L.M.V. (CAR)','LIGHT GOODS VEHICLE','LMV(CLASS)','MCV/HCV/TRAILERS','THREE WHEELER') and VehicleRegNo='" + strVehicleRegNo.Replace(" ", "") + "' and  RETRIEVE_FLAG in ('C','E')";
            dtResult = utils.GetDataTable(strSqlQuery, CnnString);
            dtResult.TableName = "HP_Portability";
            strSqlQuery = "select * from hsrpstate where hsrp_stateid=3";
            dtResult2 = utils.GetDataTable(strSqlQuery, CnnString);
            dtResult2.TableName = "State";
            dsResult.Tables.Add(dtResult);
            dsResult.Tables.Add(dtResult2);
            return dsResult;
        }
        #endregion

        

        #region UpdateCashRecieptSMS
        [WebMethod(Description = "Update CashRecieptSMS")]
        public string UpdateCashSMSReciept(DataTable dtSMS)
        {
            string CnnString = obj.strProvider;
            string strSqlQuery = string.Empty;
            string strStatus = string.Empty;
            strSqlQuery = "update  HP_PortabilityData set [MobileNo]='" + dtSMS.Rows[0]["MobileNo"].ToString() + "',[CashReceiptSmsText]='" + dtSMS.Rows[0]["CashReceiptSmsText"].ToString() + "',[CashReceiptSMSDateTime]=getdate(),[CashReceiptSMSServerResponseID]='" + dtSMS.Rows[0]["CashReceiptSMSServerResponseID"].ToString() + "' where [REGN_NO]='" + dtSMS.Rows[0]["REGN_NO"].ToString() + "' and AUTH_NO='" + dtSMS.Rows[0]["AUTH_NO"].ToString() + "'";
            int iResult = utils.ExecNonQuery(strSqlQuery, CnnString);
            if (iResult > 0)
            {
                return strStatus = "Y";

            }
            else
            {
                return strStatus = "N";
            }

        }
        #endregion

        #region UpdateEmbossingSMS
        [WebMethod(Description = "Update CashRecieptSMS")]
        public string UpdateSMSOnEmbossing(DataTable dtSMS)
        {
            string CnnString = obj.strProvider;
            string strSqlQuery = string.Empty;
            string strStatus = string.Empty;
            strSqlQuery = "update  HP_PortabilityData set [FirstSMSText]='"
                + dtSMS.Rows[0]["FirstSMSText"].ToString() + "',[FirstSMSSentDateTime]=getdate(),[FirstSMSServerResponseID]='"
                + dtSMS.Rows[0]["FirstSMSServerResponseID"].ToString() + "' where [REGN_NO]='" + dtSMS.Rows[0]["REGN_NO"].ToString()
                + "' and AUTH_NO='" + dtSMS.Rows[0]["AUTH_NO"].ToString() + "' and [MobileNo]='" + dtSMS.Rows[0]["MobileNo"].ToString() + "'";
            int iResult = utils.ExecNonQuery(strSqlQuery, CnnString);
            if (iResult > 0)
            {
                return strStatus = "Y";
            }
            else
            {
                return strStatus = "N";
            }

        }
        #endregion
        #region UpdateSMSOnClosed
        [WebMethod(Description = "Update on SMSOn Clossing")]
        public string UpdateSMSOnClosed(DataTable dtSMS)
        {
            string CnnString = obj.strProvider;
            string strSqlQuery = string.Empty;
            string strStatus = string.Empty;
            strSqlQuery = "update  HP_PortabilityData set [SecondSMSText]='"
                + dtSMS.Rows[0]["SecondSMSText"].ToString() + "',[SecondSMSSentDateTime]=getdate(),[SecondSMSServerResponseID]='"
                + dtSMS.Rows[0]["SecondSMSServerResponseID"].ToString() + "' where [REGN_NO]='" + dtSMS.Rows[0]["REGN_NO"].ToString()
                + "' and AUTH_NO='" + dtSMS.Rows[0]["AUTH_NO"].ToString() + "' and [MobileNo]='" + dtSMS.Rows[0]["MobileNo"].ToString() + "'";
            int iResult = utils.ExecNonQuery(strSqlQuery, CnnString);
            if (iResult > 0)
            {
                return strStatus = "Y";
            }
            else
            {
                return strStatus = "N";
            }

        }
        #endregion

        #region GetMobileNo
        [WebMethod(Description = "Used For To Get A Mobile No.")]
        public DataTable GetMobileNo(string strVehicleRegNo)
        {
            DataTable dtResult = new DataTable();
            string CnnString = obj.strProvider;
            string strSqlQuery = string.Empty;
            strSqlQuery = "select MobileNo,HSRP_FIX_AMT,RecordsCreationDate,AUTH_NO from HP_PortabilityData where REGN_NO= '" + strVehicleRegNo + "'";
            dtResult = utils.GetDataTable(strSqlQuery, CnnString);
            dtResult.TableName = "HP_Portability";
            return dtResult;

        }
        #endregion


        #region Send Record For Updation From DataSync
        [WebMethod(Description = "Get Record For Updation From DataSync")]
        public DataTable SendRecordForUpdationFromDataSync(String StrRtoCd, string strRETRIEVE_FLAG)
        {
            string CnnString = obj.strProvider;
            DataTable dtSend = new DataTable();
            sqlText = "select * from [hsrpdemo].[dbo].[HP_PortabilityData] where Rto_cd='" + StrRtoCd + "' and RETRIEVE_FLAG='" + strRETRIEVE_FLAG + "'";
            dtSend = utils.GetDataTable(sqlText, CnnString);
            dtSend.TableName = "HP_PortabilityData";
            return dtSend;
        }
        #endregion

        #region Update HP_PortabilityData Table Through DataSync
        [WebMethod(Description = "Update Record Through DataSync")]
        public void UpdateHP_PortablityFromDataSync(DataTable dtReceive, string strColumnStatus)
        {
            string CnnString = obj.strProvider;
            DataTable dtSend = new DataTable();
            for (int iRowCount = 0; iRowCount < dtReceive.Rows.Count; iRowCount++)
            {
                StringBuilder Objsb = new StringBuilder();
                Objsb.Append("Update [hsrpdemo].[dbo].[HP_PortabilityData] set ");
                if (strColumnStatus.ToUpper() == "MORE")
                {
                    Objsb.Append(" AUTH_NO='" + dtReceive.Rows[iRowCount]["AUTH_NO"].ToString() + "',ENG_NO='" + dtReceive.Rows[iRowCount]["ENG_NO"].ToString() + "',O_NAME='" + dtReceive.Rows[iRowCount]["O_NAME"].ToString() + "',O_ADDRESS='" + dtReceive.Rows[iRowCount]["O_ADDRESS"].ToString() + "',REGN_DT='" + dtReceive.Rows[iRowCount]["REGN_DT"].ToString() + "',MAKER='" + dtReceive.Rows[iRowCount]["MAKER"].ToString() + "',MAKER_MODEL='" + dtReceive.Rows[iRowCount]["MAKER_MODEL"].ToString() + "',VH_CLASS='" + dtReceive.Rows[iRowCount]["VH_CLASS"].ToString() + "'," +
                                 " VEH_TYPE='" + dtReceive.Rows[iRowCount]["VEH_TYPE"].ToString() + "',HSRP_FLAG='" + dtReceive.Rows[iRowCount]["HSRP_FLAG"].ToString() + "',DEAL_CD='" + dtReceive.Rows[iRowCount]["DEAL_CD"].ToString() + "',OP_DT='" + dtReceive.Rows[iRowCount]["OP_DT"].ToString() + "',RETRIEVE_FLAG='" + dtReceive.Rows[iRowCount]["RETRIEVE_FLAG"].ToString() + "',ServerReponse='" + dtReceive.Rows[iRowCount]["ServerReponse"].ToString() + "',LastChecked='" + dtReceive.Rows[iRowCount]["LastChecked"].ToString() + "' " +
                                 " where replace(REGN_NO,' ','')='" + dtReceive.Rows[iRowCount]["REGN_NO"].ToString() + "' and CHASI_NO='" + dtReceive.Rows[iRowCount]["CHASI_NO"].ToString() + "'");
                }
                else
                {
                    Objsb.Append("RETRIEVE_FLAG='" + dtReceive.Rows[iRowCount]["RETRIEVE_FLAG"].ToString() + "' where AUTH_NO='" + dtReceive.Rows[iRowCount]["AUTH_NO"].ToString() + "'");
                }

                utils.ExecNonQuery(Objsb.ToString(), CnnString);

            }
        }
        #endregion

        #region Search Records
        [WebMethod(Description = "This Method Is Used For Search Vehicle No ")]
        public DataTable SearchRecord(string strHsrpstateid, string strVehicleRegNo)
        {
            string CnnString = obj.strProvider;
            DataTable dtSearchResult = new DataTable();
            string strQuery = string.Empty;
            strQuery = "Select VehicleRegNo,OwnerName,OrderStatus,OrderType,VehicleType,VehicleClass,HSRP_Front_LaserCode as [Front LaserCode],HSRP_Rear_LaserCode as [Rear LaserCode],RoundOff_NetAmount as Amount from hsrprecords where hsrp_stateid='" + strHsrpstateid + "' and VehicleRegno='" + strVehicleRegNo + "'";
            dtSearchResult = utils.GetDataTable(strQuery, CnnString);
            dtSearchResult.TableName = "HsrpRecords";
            return dtSearchResult;
        }
        #endregion

        #region These Methods Used For HR Old Data Request Records.
        #region Search Record In HR_OldDataRequest
        [WebMethod(Description = "Search Records For Request")]
        public DataSet SearchInHRDataRequest(string strRtoID)
        {
            string CnnString = obj.strProvider;
            DataSet dsSearchResult = new DataSet();
            string strQuery = string.Empty;
            strQuery = "select * from HR_OldDataRequest where RtoLocationid='" + strRtoID + "'";
            dsSearchResult = utils.getDataSet(strQuery, CnnString);
            return dsSearchResult;
        }
        #endregion
        #region Update In HR_OldDataRequest
        [WebMethod(Description = "Update Status For Requested Records")]
        public void UpdateStatus(DataSet dsResult)
        {
            string CnnString = obj.strProvider;
            string strQuery = string.Empty;
            for (int i = 0; i < dsResult.Tables[0].Rows.Count; i++)
            {
                strQuery = "update HR_OldDataRequest set RequestStatus='" + dsResult.Tables[0].Rows[i]["RequestStatus"].ToString() + "' where RtoLocationid='" + dsResult.Tables[0].Rows[i]["RtoLocationid"].ToString() + "' and VehicleRegNo='" + dsResult.Tables[0].Rows[i]["VehicleRegNo"].ToString() + "'";
                utils.ExecNonQuery(strQuery, CnnString);
            }
        }
        #endregion
        #endregion

        //-----------------------------------------------------------These Methods for PrintSticker------------------------------------------------------

        #region FillStateDropDown
        [WebMethod(Description = "this method is used to Fill stateDropDown ")]
        public DataTable FillHSRPState()
        {

            string CnnString = obj.strProvider;
            string strQuery = string.Empty;
            DataTable dtstate = new DataTable();
            strQuery = "select HSRPStateName,HSRP_StateID from hsrpstate";
            dtstate = utils.GetDataTable(strQuery, CnnString);
            dtstate.TableName = "HSRPState";
            return dtstate;
        }
        #endregion
        #region FillRtoLocation
        [WebMethod(Description = "This Method is used for Fill RtoDropDown")]
        public DataTable FillRto(string strStateId)
        {

            string CnnString = obj.strProvider;
            string strQuery = string.Empty;
            DataTable dtRTO = new DataTable();
            strQuery = "select RTOLocationName,RTOLocationId from RTOLocation where HSRP_stateid='4' order by RTOLocationName";
            dtRTO = utils.GetDataTable(strQuery,CnnString);
            dtRTO.TableName = "RTOLocationName";
            return dtRTO;
        }
        #endregion
        #region Fill ProductionSheet DropDown
        [WebMethod(Description = "This Method is used for Fill ProductionSheet DropDown")]
        public DataTable FillProductionSheet(string strStateId, string strRtoId, string strFromdate,string strTodate)
        {
            string From=strFromdate +" 00:00:00";
            string To=strTodate + " 23:59:59";
            string CnnString = obj.strProvider;
            string strQuery = string.Empty;
            strQuery="select  Distinct pdffilename   from hsrprecords  where hsrp_StateID='"+strStateId+"' and rtolocationID='" +strRtoId + "' and  pdffilename is not null and hsrprecord_creationdate between '" + From + "' and  '" + To + "' and HSRP_Front_LaserCode is not null and HSRP_Rear_LaserCode is not null and  hsrp_rear_lasercode<>'' and hsrp_Front_lasercode<>'' group by  pdffilename, convert(varchar,pdfdownloaddate,111)";
            DataTable dtResult = utils.GetDataTable(strQuery, CnnString);
            dtResult.TableName = "hsrprecords";
            return dtResult;
        }
        #endregion
        #region FillGridView
        [WebMethod(Description = "This Method is Used To Fill DataGrid")]
        public DataTable FillDataGridViewSticker(string strStateid,string strRTO, string strSheetName)
        {
            string CnnString = obj.strProvider;
            string strQuery = string.Empty;
            strQuery = "select VehicleRegNo,ChassisNo,EngineNo,OwnerName,OrderStatus,HSRP_Front_LaserCode,HSRP_Rear_LaserCode from hsrprecords where hsrp_stateid='" + strStateid + "' and RtoLocationID='" + strRTO + "' and PdfFileName='" + strSheetName + "' and OrderStatus='Embossing Done' and StickerMandatory='Y'";
            DataTable dtGrid = utils.GetDataTable(strQuery, CnnString);
            dtGrid.TableName = "hsrprecords";
            return dtGrid;
        }
        #endregion
        #region Fetch Details From HsrpState
        [WebMethod(Description = "This Method is used to Fetch Details From HsrpState")]
        public DataTable GetStateInfo(string strStateId)
        {
            string CnnString = obj.strProvider;
            string strQuery = string.Empty;
            strQuery = "select * from hsrpstate where hsrp_stateid='" + strStateId + "'";
            DataTable dtState = utils.GetDataTable(strQuery, CnnString);
            dtState.TableName = "hsrpstate";
            return dtState;


        }

        #endregion
    }
}




