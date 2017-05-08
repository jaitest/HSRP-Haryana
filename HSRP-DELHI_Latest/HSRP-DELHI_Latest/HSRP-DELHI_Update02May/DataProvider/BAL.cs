using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DataProvider
{
    public class BAL
    {

        public BAL()
        {

        }

        public static String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

        public bool FillHSRPRecordDetail(string HSRPAuthNo, ref DataSet ds)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[FillHSRPRecordDetail]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@HSRPAuthNo", HSRPAuthNo));
                SqlDataAdapter Adap = new SqlDataAdapter(cmd);
                try
                {
                    Adap.Fill(ds);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool FillHSRPProduction(string StartDate, string EndDate, string State, string Location,string type, string filename, ref DataTable ds)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[ProductionOrderAgent]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@start", StartDate));
                cmd.Parameters.Add(new SqlParameter("@End", EndDate));
                cmd.Parameters.Add(new SqlParameter("@State", State));
                cmd.Parameters.Add(new SqlParameter("@Location", Location));
                cmd.Parameters.Add(new SqlParameter("@typesch", type));
                cmd.Parameters.Add(new SqlParameter("@FileName", filename));
                SqlDataAdapter Adap = new SqlDataAdapter(cmd);
                try
                {
                    Adap.Fill(ds);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool GetLaserDetails( string StateID, string LocationID, ref DataTable  ds)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[GetDataForProduction]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@StateID", StateID));
                cmd.Parameters.Add(new SqlParameter("@RTOLocation", LocationID));
                SqlDataAdapter Adap = new SqlDataAdapter(cmd);
                try
                {
                    Adap.Fill(ds);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool FillDelString(string VehicleRegNo, ref DataTable ds)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[GetDelhiRecordforOldData]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@VehicleRegNo", VehicleRegNo));
                SqlDataAdapter Adap = new SqlDataAdapter(cmd);
                try
                {
                    Adap.Fill(ds);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }
        public bool FillDelEngString(string EngineNo, ref DataTable ds)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[GetDelhiRecordforOldDataEn]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@EngineNo", EngineNo));
                SqlDataAdapter Adap = new SqlDataAdapter(cmd);
                try
                {
                    Adap.Fill(ds);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool FillHSRPRecordDetail2(string VehicleRegNo, ref DataSet ds)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[FillHSRPRecordDetail2]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@VehicleRegNo", VehicleRegNo));
                SqlDataAdapter Adap = new SqlDataAdapter(cmd);
                try
                {
                    Adap.Fill(ds);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool FillHSRPRecord_HHT(string VehicleRegNo, ref DataSet ds)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[getdata_hht]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@VehicleRegNo", VehicleRegNo));
                SqlDataAdapter Adap = new SqlDataAdapter(cmd);
                try
                {
                    Adap.Fill(ds);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }


        public bool FillHSRPRecordDetail3(string CashReceiptNo, ref DataSet ds)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[FillHSRPRecordDetail3]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CashReceiptNo", CashReceiptNo));
                SqlDataAdapter Adap = new SqlDataAdapter(cmd);
                try
                {
                    Adap.Fill(ds);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }



        public bool FillHSRPRecordDetailEdit(string HSRPAuthNo, ref DataSet ds)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[FillHSRPRecordDetailEdit]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@HSRPAuthNo", HSRPAuthNo));
                SqlDataAdapter Adap = new SqlDataAdapter(cmd);
                try
                {
                    Adap.Fill(ds);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool FillHSRPRecordDetail2Edit(string VehicleRegNo, ref DataSet ds)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[FillHSRPRecordDetail2Edit]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@VehicleRegNo", VehicleRegNo));
                SqlDataAdapter Adap = new SqlDataAdapter(cmd);
                try
                {
                    Adap.Fill(ds);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }


        public bool stickercheck(string VehicleRegNo, ref DataSet ds)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[FillHSRPRecordDetail2Edit]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@VehicleRegNo", VehicleRegNo));
                SqlDataAdapter Adap = new SqlDataAdapter(cmd);
                try
                {
                    Adap.Fill(ds);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }


        public bool FillHSRPRecordDetail3Edit(string CashReceiptNo, ref DataSet ds)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[FillHSRPRecordDetail3Edit]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CashReceiptNo", CashReceiptNo));
                SqlDataAdapter Adap = new SqlDataAdapter(cmd);
                try
                {
                    Adap.Fill(ds);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        //   

        public bool CheckHSRPRecordAuthNo(string HSRPAuthNo, string VehicleRegNo, string StateID, ref int ISExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[CheckHSRPRecordAuthNo]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@HSRPAuthNo", HSRPAuthNo));
                cmd.Parameters.Add(new SqlParameter("@VehicleRegNo", VehicleRegNo));
                
                cmd.Parameters.Add(new SqlParameter("@StateID", StateID));

                SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                isexists.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ISExists = (int)isexists.Value;
                    con.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool CheckHSRPRecordHSRPVehicleReg(string VehicleReg, ref int ISExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[CheckHSRPRecordHSRPVehicleReg]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@VehicleReg", @VehicleReg));

                SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                isexists.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ISExists = (int)isexists.Value;
                    con.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool CheckHSRPRecordHSRPCashReceiptNo(string CashReceiptNo, ref int ISExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[CheckHSRPRecordHSRPVehicleReg]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CashReceiptNo", CashReceiptNo));

                SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                isexists.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ISExists = (int)isexists.Value;
                    con.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool FillHSRPRecordDetailXX(string HSRPAuthNo, ref DataSet ds)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[FillHSRPRecordDetailXX]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@HSRPAuthNo", HSRPAuthNo));
                SqlDataAdapter Adap = new SqlDataAdapter(cmd);
                try
                {
                    Adap.Fill(ds);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool FillHSRPRecordDetail2XX(string VehicleRegNo, ref DataSet ds)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[FillHSRPRecordDetail2XX]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@VehicleRegNo", VehicleRegNo));
                SqlDataAdapter Adap = new SqlDataAdapter(cmd);
                try
                {
                    Adap.Fill(ds);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool FillHSRPRecordDetail3XX(string CashReceiptNo, ref DataSet ds)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[FillHSRPRecordDetail3XX]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CashReceiptNo", CashReceiptNo));
                SqlDataAdapter Adap = new SqlDataAdapter(cmd);
                try
                {
                    Adap.Fill(ds);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }


        public bool FillHSRPRecordDeliveryChallan(string HSRPAuthNo, ref DataTable ds)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[FillHSRPRecordDeliveryChallan]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@HSRPAuthNo", HSRPAuthNo));
                SqlDataAdapter Adap = new SqlDataAdapter(cmd);
                try
                {
                    Adap.Fill(ds);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool InsertVIPHSRPRecords(string HSRPRecord_AuthorizationNo, DateTime HSRPRecord_AuthorizationDate, string OrderNo,
       String OwnerName, String OrderStatus, string Address1, string Address2, string MobileNo, string LandlineNo,
       string EmailID, string VehicleClass, string StringManufacturerName, string StringManufacturerModel, string VehicleType, string VehicleRegNo, string EngineNo,
       string ChassisNo, string OrderType, string ISFrontPlateSize, string FrontPlateSize, string ISRearPlateSize,
       string RearPlateSize, string StickerMandatory, string InvoiceNo, string CashReceiptNo, string VAT_Percentage,
       string VAT_Amount, string ServiceTax_Percentage, string ServiceTax_Amount, string TotalAmount, string NetAmount,
       string HSRPStateID, string RTOLocationID, string DeliveryChallan, string StringFixingCharge, string StringFrontPlatePrize,
       string StringRearPlatePrize, string StringStickerPrize, string StringScrewPrize, ref int isexists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Transaction_VIP_HSRPRecordInsertXX]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@HSRPRecord_AuthorizationNo", HSRPRecord_AuthorizationNo));
                cmd.Parameters.Add(new SqlParameter("@HSRPRecord_AuthorizationDate", HSRPRecord_AuthorizationDate));
                cmd.Parameters.Add(new SqlParameter("@OrderNo", OrderNo));
                cmd.Parameters.Add(new SqlParameter("@OwnerName", OwnerName));
                cmd.Parameters.Add(new SqlParameter("@OrderStatus", OrderStatus));
                cmd.Parameters.Add(new SqlParameter("@Address1", Address1));
                cmd.Parameters.Add(new SqlParameter("@Address2", Address2));
                cmd.Parameters.Add(new SqlParameter("@MobileNo", MobileNo));
                cmd.Parameters.Add(new SqlParameter("@LandlineNo", LandlineNo));
                cmd.Parameters.Add(new SqlParameter("@EmailID", EmailID));
                cmd.Parameters.Add(new SqlParameter("@VehicleClass", VehicleClass));
                cmd.Parameters.Add(new SqlParameter("@ManufacturerName", StringManufacturerName));
                cmd.Parameters.Add(new SqlParameter("@ManufacturerModel", StringManufacturerModel));
                cmd.Parameters.Add(new SqlParameter("@VehicleType", VehicleType));
                cmd.Parameters.Add(new SqlParameter("@VehicleRegNo", VehicleRegNo));
                cmd.Parameters.Add(new SqlParameter("@EngineNo", EngineNo));
                cmd.Parameters.Add(new SqlParameter("@ChassisNo", ChassisNo));
                cmd.Parameters.Add(new SqlParameter("@OrderType", OrderType));
                cmd.Parameters.Add(new SqlParameter("@ISFrontPlateSize", ISFrontPlateSize));
                cmd.Parameters.Add(new SqlParameter("@FrontPlateSize", FrontPlateSize));
                cmd.Parameters.Add(new SqlParameter("@ISRearPlateSize", ISRearPlateSize));
                cmd.Parameters.Add(new SqlParameter("@RearPlateSize", RearPlateSize));
                cmd.Parameters.Add(new SqlParameter("@StickerMandatory", StickerMandatory));
                cmd.Parameters.Add(new SqlParameter("@InvoiceNo", InvoiceNo));
                cmd.Parameters.Add(new SqlParameter("@CashReceiptNo", CashReceiptNo));
                cmd.Parameters.Add(new SqlParameter("@VAT_Percentage", VAT_Percentage));
                cmd.Parameters.Add(new SqlParameter("@VAT_Amount", VAT_Amount));
                cmd.Parameters.Add(new SqlParameter("@ServiceTax_Percentage", ServiceTax_Percentage));
                cmd.Parameters.Add(new SqlParameter("@ServiceTax_Amount", ServiceTax_Amount));
                cmd.Parameters.Add(new SqlParameter("@TotalAmount", TotalAmount));
                cmd.Parameters.Add(new SqlParameter("@NetAmount", NetAmount));
                cmd.Parameters.Add(new SqlParameter("@HSRP_StateID", HSRPStateID));
                cmd.Parameters.Add(new SqlParameter("@RTOLocationID", RTOLocationID));
                cmd.Parameters.Add(new SqlParameter("@DeliveryChallan", DeliveryChallan));
                cmd.Parameters.Add(new SqlParameter("@FixingCharge", StringFixingCharge));
                cmd.Parameters.Add(new SqlParameter("@FrontPlatePrize", StringFrontPlatePrize));
                cmd.Parameters.Add(new SqlParameter("@RearPlatePrize", StringRearPlatePrize));
                cmd.Parameters.Add(new SqlParameter("@StickerPrize", StringStickerPrize));
                cmd.Parameters.Add(new SqlParameter("@ScrewPrize", StringScrewPrize));

                SqlParameter kk = new SqlParameter("@IsExists", SqlDbType.Int);
                kk.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(kk);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    isexists = (int)kk.Value;
                    con.Close();
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            return true;
        }

        public bool InsertHSRPRecords(string HSRPRecord_AuthorizationNo, DateTime HSRPRecord_AuthorizationDate, string OrderNo,
        String OwnerName, String OrderStatus, string Address1, string Address2, string MobileNo, string LandlineNo,
        string EmailID, string VehicleClass, string StringManufacturerName, string StringManufacturerModel, string VehicleType, string VehicleRegNo, string EngineNo,
        string ChassisNo, string OrderType, string ISFrontPlateSize, string FrontPlateSize, string ISRearPlateSize,
        string RearPlateSize, string StickerMandatory, string InvoiceNo, string CashReceiptNo, string VAT_Percentage,
        string VAT_Amount, string ServiceTax_Percentage, string ServiceTax_Amount, string TotalAmount, string NetAmount,
        string HSRPStateID, string RTOLocationID, string DeliveryChallan, string StringFixingCharge, string StringFrontPlatePrize,
        string StringRearPlatePrize, string StringStickerPrize, string StringScrewPrize, string Remarks, int UID, string RTOLocationIDd, ref int isexists, ref string HSRPRecordID)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Transaction_HSRPRecordInsertXX]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@HSRPRecord_AuthorizationNo", HSRPRecord_AuthorizationNo));
                cmd.Parameters.Add(new SqlParameter("@HSRPRecord_AuthorizationDate", HSRPRecord_AuthorizationDate));
                cmd.Parameters.Add(new SqlParameter("@OrderNo", OrderNo));
                cmd.Parameters.Add(new SqlParameter("@OwnerName", OwnerName));
                cmd.Parameters.Add(new SqlParameter("@OrderStatus", OrderStatus));
                cmd.Parameters.Add(new SqlParameter("@Address1", Address1));
                cmd.Parameters.Add(new SqlParameter("@Address2", Address2));
                cmd.Parameters.Add(new SqlParameter("@MobileNo", MobileNo));
                cmd.Parameters.Add(new SqlParameter("@LandlineNo", LandlineNo));
                cmd.Parameters.Add(new SqlParameter("@EmailID", EmailID));
                cmd.Parameters.Add(new SqlParameter("@VehicleClass", VehicleClass));
                cmd.Parameters.Add(new SqlParameter("@ManufacturerName", StringManufacturerName));
                cmd.Parameters.Add(new SqlParameter("@ManufacturerModel", StringManufacturerModel));
                cmd.Parameters.Add(new SqlParameter("@VehicleType", VehicleType));
                cmd.Parameters.Add(new SqlParameter("@VehicleRegNo", VehicleRegNo));
                cmd.Parameters.Add(new SqlParameter("@EngineNo", EngineNo));
                cmd.Parameters.Add(new SqlParameter("@ChassisNo", ChassisNo));
                cmd.Parameters.Add(new SqlParameter("@OrderType", OrderType));
                cmd.Parameters.Add(new SqlParameter("@ISFrontPlateSize", ISFrontPlateSize));
                cmd.Parameters.Add(new SqlParameter("@FrontPlateSize", FrontPlateSize));
                cmd.Parameters.Add(new SqlParameter("@ISRearPlateSize", ISRearPlateSize));
                cmd.Parameters.Add(new SqlParameter("@RearPlateSize", RearPlateSize));
                cmd.Parameters.Add(new SqlParameter("@StickerMandatory", StickerMandatory));
                cmd.Parameters.Add(new SqlParameter("@InvoiceNo", InvoiceNo));
                cmd.Parameters.Add(new SqlParameter("@CashReceiptNo", CashReceiptNo));
                cmd.Parameters.Add(new SqlParameter("@VAT_Percentage", VAT_Percentage));
                cmd.Parameters.Add(new SqlParameter("@VAT_Amount", VAT_Amount));
                cmd.Parameters.Add(new SqlParameter("@ServiceTax_Percentage", ServiceTax_Percentage));
                cmd.Parameters.Add(new SqlParameter("@ServiceTax_Amount", ServiceTax_Amount));
                cmd.Parameters.Add(new SqlParameter("@TotalAmount", TotalAmount));
                cmd.Parameters.Add(new SqlParameter("@NetAmount", NetAmount));
                cmd.Parameters.Add(new SqlParameter("@HSRP_StateID", HSRPStateID));
                cmd.Parameters.Add(new SqlParameter("@RTOLocationID", RTOLocationID));
                cmd.Parameters.Add(new SqlParameter("@DeliveryChallan", DeliveryChallan));
                cmd.Parameters.Add(new SqlParameter("@FixingCharge", StringFixingCharge));
                cmd.Parameters.Add(new SqlParameter("@FrontPlatePrize", StringFrontPlatePrize));
                cmd.Parameters.Add(new SqlParameter("@RearPlatePrize", StringRearPlatePrize));
                cmd.Parameters.Add(new SqlParameter("@StickerPrize", StringStickerPrize));
                cmd.Parameters.Add(new SqlParameter("@ScrewPrize", StringScrewPrize));
                cmd.Parameters.Add(new SqlParameter("@Remarks", Remarks));
                cmd.Parameters.Add(new SqlParameter("@UID", UID));
                cmd.Parameters.Add(new SqlParameter("@RTOLocationIDd", RTOLocationIDd));




                SqlParameter HSRPRecordIDs = new SqlParameter("@HSRPRecordID", SqlDbType.VarChar, 100);
                HSRPRecordIDs.Direction = ParameterDirection.Output;

                SqlParameter kk = new SqlParameter("@IsExists", SqlDbType.Int);
                kk.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(kk);
                cmd.Parameters.Add(HSRPRecordIDs);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    isexists = (int)kk.Value;
                    if (isexists != 1)
                    {
                        HSRPRecordID = (string)HSRPRecordIDs.Value;
                    }
                    con.Close();
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            return true;
        }


        public bool EditHSRPRecords(string HSRPRecord_AuthorizationNo, DateTime HSRPRecord_AuthorizationDate, string OrderNo,
        String OwnerName, String OrderStatus, string Address1, string Address2, string MobileNo, string LandlineNo,
        string EmailID, string VehicleClass, string StringManufacturerName, string StringManufacturerModel, string VehicleType, string VehicleRegNo, string EngineNo,
        string ChassisNo, string OrderType, string ISFrontPlateSize, string FrontPlateSize, string ISRearPlateSize,
        string RearPlateSize, string StickerMandatory, string InvoiceNo, string CashReceiptNo, string VAT_Percentage,
        string VAT_Amount, string ServiceTax_Percentage, string ServiceTax_Amount, string TotalAmount, string NetAmount,
        string HSRPStateID, string RTOLocationID, string DeliveryChallan, string StringFixingCharge, string StringFrontPlatePrize,
        string StringRearPlatePrize, string StringStickerPrize, string StringScrewPrize, string Remarks, int UID, string RTOLocationIDd, ref int ISExists, string HSRPRecordID)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Transaction_HSRPRecordEdit]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@HSRPRecord_AuthorizationNo", HSRPRecord_AuthorizationNo));
                cmd.Parameters.Add(new SqlParameter("@HSRPRecord_AuthorizationDate", HSRPRecord_AuthorizationDate));
                cmd.Parameters.Add(new SqlParameter("@OrderNo", OrderNo));
                cmd.Parameters.Add(new SqlParameter("@OwnerName", OwnerName));
                cmd.Parameters.Add(new SqlParameter("@OrderStatus", OrderStatus));
                cmd.Parameters.Add(new SqlParameter("@Address1", Address1));
                cmd.Parameters.Add(new SqlParameter("@Address2", Address2));
                cmd.Parameters.Add(new SqlParameter("@MobileNo", MobileNo));
                cmd.Parameters.Add(new SqlParameter("@LandlineNo", LandlineNo));
                cmd.Parameters.Add(new SqlParameter("@EmailID", EmailID));
                cmd.Parameters.Add(new SqlParameter("@VehicleClass", VehicleClass));
                cmd.Parameters.Add(new SqlParameter("@ManufacturerName", StringManufacturerName));
                cmd.Parameters.Add(new SqlParameter("@ManufacturerModel", StringManufacturerModel));
                cmd.Parameters.Add(new SqlParameter("@VehicleType", VehicleType));
                cmd.Parameters.Add(new SqlParameter("@VehicleRegNo", VehicleRegNo));
                cmd.Parameters.Add(new SqlParameter("@EngineNo", EngineNo));
                cmd.Parameters.Add(new SqlParameter("@ChassisNo", ChassisNo));
                cmd.Parameters.Add(new SqlParameter("@OrderType", OrderType));
                cmd.Parameters.Add(new SqlParameter("@ISFrontPlateSize", ISFrontPlateSize));
                cmd.Parameters.Add(new SqlParameter("@FrontPlateSize", FrontPlateSize));
                cmd.Parameters.Add(new SqlParameter("@ISRearPlateSize", ISRearPlateSize));
                cmd.Parameters.Add(new SqlParameter("@RearPlateSize", RearPlateSize));
                cmd.Parameters.Add(new SqlParameter("@StickerMandatory", StickerMandatory));
                cmd.Parameters.Add(new SqlParameter("@InvoiceNo", InvoiceNo));
                cmd.Parameters.Add(new SqlParameter("@CashReceiptNo", CashReceiptNo));
                cmd.Parameters.Add(new SqlParameter("@VAT_Percentage", VAT_Percentage));
                cmd.Parameters.Add(new SqlParameter("@VAT_Amount", VAT_Amount));
                cmd.Parameters.Add(new SqlParameter("@ServiceTax_Percentage", ServiceTax_Percentage));
                cmd.Parameters.Add(new SqlParameter("@ServiceTax_Amount", ServiceTax_Amount));
                cmd.Parameters.Add(new SqlParameter("@TotalAmount", TotalAmount));
                cmd.Parameters.Add(new SqlParameter("@NetAmount", NetAmount));
                cmd.Parameters.Add(new SqlParameter("@HSRP_StateID", HSRPStateID));
                cmd.Parameters.Add(new SqlParameter("@RTOLocationID", RTOLocationID));
                cmd.Parameters.Add(new SqlParameter("@DeliveryChallan", DeliveryChallan));
                cmd.Parameters.Add(new SqlParameter("@FixingCharge", StringFixingCharge));
                cmd.Parameters.Add(new SqlParameter("@FrontPlatePrize", StringFrontPlatePrize));
                cmd.Parameters.Add(new SqlParameter("@RearPlatePrize", StringRearPlatePrize));
                cmd.Parameters.Add(new SqlParameter("@StickerPrize", StringStickerPrize));
                cmd.Parameters.Add(new SqlParameter("@ScrewPrize", StringScrewPrize));
                cmd.Parameters.Add(new SqlParameter("@Remarks", Remarks));
                cmd.Parameters.Add(new SqlParameter("@UID", UID));
                cmd.Parameters.Add(new SqlParameter("@RTOLocationIDd", RTOLocationIDd));


                SqlParameter isexists = new SqlParameter("@IsExists", SqlDbType.Int);
                isexists.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(isexists);
                cmd.Parameters.Add(new SqlParameter("@HSRPRecordID", HSRPRecordID));
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ISExists = (int)isexists.Value;
                    con.Close();
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            return true;
        }




        public bool InsertHSRPState(string StateName, string ActiveStatus, ref int ISExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Master_HSRPStateInsert]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@HSRPStateName", StateName));
                cmd.Parameters.Add(new SqlParameter("@ActiveStatus", ActiveStatus));

                SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                isexists.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ISExists = (int)isexists.Value;
                    con.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool Update_Master_HSRPState(String HSRP_StateID, string StateName, string ActiveStatus, ref int ISExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Master_HSRPStateUpdate]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@HSRP_StateID", HSRP_StateID));
                cmd.Parameters.Add(new SqlParameter("@HSRPStateName", StateName));
                cmd.Parameters.Add(new SqlParameter("@ActiveStatus", ActiveStatus));

                SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                isexists.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ISExists = (int)isexists.Value;
                    con.Close();
                }
                catch (Exception)
                {
                    return false;
                }

            }
            return true;
        }

        public bool Insert_Master_HSRP_RTOLocation(int HSRP_StateID, string LocationType, string RTOLocationName, string RTOLocationCode, string RTOLocationAddress, string RTOShippingAddress, string BillingAddress, string ContactPersonName, string MobileNo, string LandLineNo, string EmailID, string ActiveStatus, string EmbossingStation, ref int ISExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Master_InsertRTOLocation]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@HSRP_StateID", HSRP_StateID));
                cmd.Parameters.Add(new SqlParameter("@LocationType", LocationType));
                cmd.Parameters.Add(new SqlParameter("@RTOLocationName", RTOLocationName));
                cmd.Parameters.Add(new SqlParameter("@RTOLocationCode", RTOLocationCode));
                cmd.Parameters.Add(new SqlParameter("@RTOLocationAddress", RTOLocationAddress));
                cmd.Parameters.Add(new SqlParameter("@RTOShippingAddress", RTOShippingAddress));
                cmd.Parameters.Add(new SqlParameter("@RTOBillingAddress", BillingAddress));
                cmd.Parameters.Add(new SqlParameter("@ContactPersonName", ContactPersonName));
                cmd.Parameters.Add(new SqlParameter("@MobileNo", MobileNo));
                cmd.Parameters.Add(new SqlParameter("@LandlineNo", LandLineNo));
                cmd.Parameters.Add(new SqlParameter("@EmailID", EmailID));
                cmd.Parameters.Add(new SqlParameter("@IsEmbossingStation", EmbossingStation));
                cmd.Parameters.Add(new SqlParameter("@ActiveStatus", ActiveStatus));

                SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                isexists.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }


        public bool Update_Master_HSRP_RTOLocation(int RTOLocationID, int HSRP_StateID, string LocationType, string RTOLocationName, string RTOLocationCode, string RTOLocationAddress, string RTOShippingAddress, string BillingAddress, string ContactPersonName, string MobileNo, string LandLineNo, string EmailID, string ActiveStatus, string EmbossingStation, ref int ISExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Master_UpdateRTOLocation]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@RTOLocationID", RTOLocationID));
                cmd.Parameters.Add(new SqlParameter("@HSRP_StateID", HSRP_StateID));
                cmd.Parameters.Add(new SqlParameter("@LocationType", LocationType));
                cmd.Parameters.Add(new SqlParameter("@RTOLocationName", RTOLocationName));
                cmd.Parameters.Add(new SqlParameter("@RTOLocationCode", RTOLocationCode));
                cmd.Parameters.Add(new SqlParameter("@RTOLocationAddress", RTOLocationAddress));
                cmd.Parameters.Add(new SqlParameter("@RTOShippingAddress", RTOShippingAddress));
                cmd.Parameters.Add(new SqlParameter("@RTOBillingAddress", BillingAddress));
                cmd.Parameters.Add(new SqlParameter("@ContactPersonName", ContactPersonName));
                cmd.Parameters.Add(new SqlParameter("@MobileNo", MobileNo));
                cmd.Parameters.Add(new SqlParameter("@LandlineNo", LandLineNo));
                cmd.Parameters.Add(new SqlParameter("@EmailID", EmailID));
                cmd.Parameters.Add(new SqlParameter("@IsEmbossingStation", EmbossingStation));
                cmd.Parameters.Add(new SqlParameter("@ActiveStatus", ActiveStatus));

                SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                isexists.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ISExists = (int)isexists.Value;
                    con.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool InsertHSRPPruduct(string ProductCode, string ProductColor, string ActiveStatus, string ProductDimension, ref int ISExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Master_HSRPProductInsert]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ProductCode", ProductCode));
                cmd.Parameters.Add(new SqlParameter("@ProductColor", ProductColor));
                cmd.Parameters.Add(new SqlParameter("@ActiveStatus", ActiveStatus));
                cmd.Parameters.Add(new SqlParameter("@ProductDimension", ProductDimension));
                SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                isexists.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ISExists = (int)isexists.Value;
                    con.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool UpdateHSRPPruduct(int ProductID, string ProductCode, string ProductColor, string ActiveStatus, string ProductDimension, ref int ISExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Master_HSRPProductUpdate]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ProductID", ProductID));
                cmd.Parameters.Add(new SqlParameter("@ProductCode", ProductCode));
                cmd.Parameters.Add(new SqlParameter("@ProductColor", ProductColor));
                cmd.Parameters.Add(new SqlParameter("@ActiveStatus", ActiveStatus));
                cmd.Parameters.Add(new SqlParameter("@ProductDimension", ProductDimension));
                SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                isexists.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ISExists = (int)isexists.Value;
                    con.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool InsertHSRPPrefix(int HSRP_StateID, int RTOLocationID, string PrefixFor, string PrefixText, ref int ISExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                //                
                SqlCommand cmd = new SqlCommand("[Master_InsertPrefix]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@HSRP_StateID", HSRP_StateID));
                cmd.Parameters.Add(new SqlParameter("@RTOLocationID", RTOLocationID));
                cmd.Parameters.Add(new SqlParameter("@PrefixFor", PrefixFor));
                cmd.Parameters.Add(new SqlParameter("@PrefixText", PrefixText));
                SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                isexists.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ISExists = (int)isexists.Value;
                    con.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool UpdateHSRPPrefix(int SerialPrefixID, int HSRP_StateID, int RTOLocationID, string PrefixFor, string PrefixText, ref int ISExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                //                
                SqlCommand cmd = new SqlCommand("[Master_UpdatePrefix]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@SerialPrefixID", SerialPrefixID));
                cmd.Parameters.Add(new SqlParameter("@HSRP_StateID", HSRP_StateID));
                cmd.Parameters.Add(new SqlParameter("@RTOLocationID", RTOLocationID));
                cmd.Parameters.Add(new SqlParameter("@PrefixFor", PrefixFor));
                cmd.Parameters.Add(new SqlParameter("@PrefixText", PrefixText));
                SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                isexists.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ISExists = (int)isexists.Value;
                    con.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool UpdateHSRPMessageTicker(int TickerID, string MessageText, string MessageTextURL, int HSRP_StateID, int RTOLocationID, DateTime UpdateDate, string ActiveStatus, ref int ISExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                //                
                SqlCommand cmd = new SqlCommand("[Master_UpdateMessageTicker]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@MessageID", TickerID));
                cmd.Parameters.Add(new SqlParameter("@MessageText", MessageText));
                cmd.Parameters.Add(new SqlParameter("@MessageTextURL", MessageTextURL));
                cmd.Parameters.Add(new SqlParameter("@HSRP_StateID", HSRP_StateID));

                cmd.Parameters.Add(new SqlParameter("@RTOLocationID", RTOLocationID));
                cmd.Parameters.Add(new SqlParameter("@UpdateDateTime", UpdateDate));
                cmd.Parameters.Add(new SqlParameter("@ActiveStatus", ActiveStatus));
                SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                isexists.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ISExists = (int)isexists.Value;
                    con.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool InsertHSRPMessageTicker(string MessageText, string MessageTextURL, int HSRP_StateID, int RTOLocationID, DateTime CreatedDateTime, string ActiveStatus, ref int ISExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Master_InsertMessageTicker]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@MessageText", MessageText));
                cmd.Parameters.Add(new SqlParameter("@MessageTextURL", MessageTextURL));
                cmd.Parameters.Add(new SqlParameter("@HSRP_StateID", HSRP_StateID));
                cmd.Parameters.Add(new SqlParameter("@RTOLocationID", RTOLocationID));
                cmd.Parameters.Add(new SqlParameter("@CreatedDateTime", CreatedDateTime));
                cmd.Parameters.Add(new SqlParameter("@ActiveStatus", ActiveStatus));
                SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                isexists.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ISExists = (int)isexists.Value;
                    con.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool UpdateHSRPSecurityQuestion(int QuestionID, int userID, string QuestionText, DateTime UpdateDate, ref int ISExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                //                
                SqlCommand cmd = new SqlCommand("[Master_UpdateSecurityQuestion]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@QuestionID", QuestionID));
                cmd.Parameters.Add(new SqlParameter("@userID", userID));
                cmd.Parameters.Add(new SqlParameter("@QuestionText", QuestionText));
                cmd.Parameters.Add(new SqlParameter("@UpdateDate", UpdateDate));
                SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                isexists.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ISExists = (int)isexists.Value;
                    con.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool InsertHSRPSecurityQuestion(int userID, string QuestionText, DateTime CreateDate, ref int ISExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                //                
                SqlCommand cmd = new SqlCommand("[Master_InsertSecurityQuestion]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@userID", userID));
                cmd.Parameters.Add(new SqlParameter("@QuestionText", QuestionText));
                cmd.Parameters.Add(new SqlParameter("@CreateDate", CreateDate));
                SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                isexists.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ISExists = (int)isexists.Value;
                    con.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool UpdateLaserAssigned(int StateID, int RTOLocationID, string OrderStatus, string FrondLaserCode, string RearLaserCode, string HSRPRecordID, string HSRP_Sticker_LaserCode, ref int ISExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                //string sqlSticker, string sqlFrontLaser, string sqlRearPlate,string HSRP_Sticker_LaserCode, 
                SqlCommand cmd = new SqlCommand("[Transaction_UpdateAssignedLaser]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@HSRPRecordID", HSRPRecordID));
                cmd.Parameters.Add(new SqlParameter("@HSRP_StateID", StateID));
                cmd.Parameters.Add(new SqlParameter("@RTOLocationID", RTOLocationID));
                cmd.Parameters.Add(new SqlParameter("@OrderStatus", OrderStatus));
                cmd.Parameters.Add(new SqlParameter("@HSRP_Front_LaserCode", FrondLaserCode));
                cmd.Parameters.Add(new SqlParameter("@HSRP_Rear_LaserCode", RearLaserCode));
                cmd.Parameters.Add(new SqlParameter("@HSRP_Sticker_LaserCode", HSRP_Sticker_LaserCode));

                SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                isexists.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ISExists = (int)isexists.Value;
                    con.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool Transaction_UpdateAssignedLaser_MakeFree(string OrderStatus, string FrondLaserCode, string RearLaserCode, string HSRPRecordID, ref int ISExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {

                SqlCommand cmd = new SqlCommand("[Transaction_UpdateAssignedLaser_MakeFree]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@HSRPRecordID", HSRPRecordID));
                cmd.Parameters.Add(new SqlParameter("@OrderStatus", OrderStatus));
                cmd.Parameters.Add(new SqlParameter("@HSRP_Front_LaserCode", FrondLaserCode));
                cmd.Parameters.Add(new SqlParameter("@HSRP_Rear_LaserCode", RearLaserCode));
                //SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                //isexists.Direction = ParameterDirection.Output;
                //cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    //  ISExists = (int)isexists.Value;
                    con.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool UpdateEmbossing(int RTOLocationID, int HSRPStateID, string LaserPlateBoxNo, string FrontLaserCode, string Sticker, string RearLaserCode, string OperatorID, string HSRPRecordID)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {

                SqlCommand cmd = new SqlCommand("[Transaction_UpdateEmbossing]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@HSRPRecordID", HSRPRecordID));
                cmd.Parameters.Add(new SqlParameter("@LaserPlateBoxNo", LaserPlateBoxNo));

                cmd.Parameters.Add(new SqlParameter("@RTOLocationID", RTOLocationID));
                cmd.Parameters.Add(new SqlParameter("@HSRPStateID", HSRPStateID));

                cmd.Parameters.Add(new SqlParameter("@HSRP_Front_LaserCode", FrontLaserCode));
                cmd.Parameters.Add(new SqlParameter("@Sticker", Sticker));
                cmd.Parameters.Add(new SqlParameter("@CreatedByID", OperatorID));
                cmd.Parameters.Add(new SqlParameter("@HSRP_Rear_LaserCode", RearLaserCode));
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool PlateFrontRejectEmbossing(string OrderStatus, string FrondLaserCode, string HSRPRecordID)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Transaction_PlateFrontRejectEmbossing]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@HSRPRecordID", HSRPRecordID));
                cmd.Parameters.Add(new SqlParameter("@OrderStatus", OrderStatus));
                cmd.Parameters.Add(new SqlParameter("@HSRP_Front_LaserCode", FrondLaserCode));

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    //  ISExists = (int)isexists.Value;
                    con.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool PlateRearRejectEmbossing(string OrderStatus, string RearLaserCode, string HSRPRecordID)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Transaction_RearFrontRejectEmbossing]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@HSRPRecordID", HSRPRecordID));
                cmd.Parameters.Add(new SqlParameter("@OrderStatus", OrderStatus));
                cmd.Parameters.Add(new SqlParameter("@HSRP_Front_LaserCode", RearLaserCode));
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    //  ISExists = (int)isexists.Value;
                    con.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        



        public bool Transaction_UpdateAssignedLaserRear_MakeFree(string RearLaserCode, string HSRPRecordID)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {

                SqlCommand cmd = new SqlCommand("[Transaction_UpdateAssignedLaserRear_MakeFree]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@HSRPRecordID", HSRPRecordID));
                cmd.Parameters.Add(new SqlParameter("@HSRP_Rear_LaserCode", RearLaserCode));
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    //  ISExists = (int)isexists.Value;
                    con.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool Transaction_UpdateAssignedLaser_MakeFree(string FrontLaserCode, string HSRPRecordID)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {

                SqlCommand cmd = new SqlCommand("[Transaction_UpdateAssignedLaser_MakeFree]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@HSRPRecordID", HSRPRecordID));
                cmd.Parameters.Add(new SqlParameter("@HSRP_Front_LaserCode", FrontLaserCode));
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    //  ISExists = (int)isexists.Value;
                    con.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }


        public int SavePlantDetail(List<string> lst)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Transaction_SavePlant]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@PlantAddress", lst[0]));
                cmd.Parameters.Add(new SqlParameter("@PlantCity", lst[1]));
                cmd.Parameters.Add(new SqlParameter("@PlantState", lst[2]));
                cmd.Parameters.Add(new SqlParameter("@PlantZip", lst[3]));
                cmd.Parameters.Add(new SqlParameter("@ContactPersonName", lst[4]));
                cmd.Parameters.Add(new SqlParameter("@MobileNo", lst[5]));
                cmd.Parameters.Add(new SqlParameter("@LandlineNo", lst[6]));
                cmd.Parameters.Add(new SqlParameter("@EmailID", lst[7]));
                cmd.Parameters.Add(new SqlParameter("@ActiveStatus", lst[8]));

                try
                {
                    con.Open();
                    i = cmd.ExecuteNonQuery();
                    con.Close();
                    return i;

                }
                catch (Exception)
                {
                    return i;
                }

            }
        }


        public int UpdatePlantDetail(List<string> lst)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Transaction_UpdatePlant]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@PlantAddress", lst[0]));
                cmd.Parameters.Add(new SqlParameter("@PlantCity", lst[1]));
                cmd.Parameters.Add(new SqlParameter("@PlantState", lst[2]));
                cmd.Parameters.Add(new SqlParameter("@PlantZip", lst[3]));
                cmd.Parameters.Add(new SqlParameter("@ContactPersonName", lst[4]));
                cmd.Parameters.Add(new SqlParameter("@MobileNo", lst[5]));
                cmd.Parameters.Add(new SqlParameter("@LandlineNo", lst[6]));
                cmd.Parameters.Add(new SqlParameter("@EmailID", lst[7]));
                cmd.Parameters.Add(new SqlParameter("@ActiveStatus", lst[8]));
                cmd.Parameters.Add(new SqlParameter("@PlantID", lst[9]));

                try
                {
                    con.Open();
                    i = cmd.ExecuteNonQuery();
                    con.Close();
                    return i;

                }
                catch (Exception)
                {
                    return i;
                }

            }
        }


        public bool InsertRTOInventory(int ProductID, string UserID, int BatchID, string Prefix, Int64 LaserNowithoutPrefix, string LaserNo, int HSRPStateID, int RTOLocationID, string InventoryStatus, string Remarks, ref int IsExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Transaction_InsertRTOInventory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ProductID", ProductID));
                cmd.Parameters.Add(new SqlParameter("@ReceivedUserID", UserID));
                cmd.Parameters.Add(new SqlParameter("@BatchID", BatchID));
                cmd.Parameters.Add(new SqlParameter("@Prefix", Prefix));
                cmd.Parameters.Add(new SqlParameter("@LaserNowithoutPrefix", LaserNowithoutPrefix));
                cmd.Parameters.Add(new SqlParameter("@LaserNo", LaserNo));
                cmd.Parameters.Add(new SqlParameter("@HSRP_StateID", HSRPStateID));
                cmd.Parameters.Add(new SqlParameter("@RTOLocationID", RTOLocationID));
                cmd.Parameters.Add(new SqlParameter("@InventoryStatus", InventoryStatus));
                cmd.Parameters.Add(new SqlParameter("@Remarks", Remarks));
                SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                isexists.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    IsExists = (int)isexists.Value;
                    con.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        //h(Prefix, intHSRPStateID, intRTOLocationID)
        public bool InsertHSRPPBatch(int ProductID, string BatchCode, string Prefix, Int64 LaserCodeFrom, Int64 LaserCodeTo, DateTime DateofManufacturing, decimal Weight, int BatchCheckedBy, DateTime CreateDateTime, string Remarks, ref int IsExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Transaction_InsertBatch]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ProductID", ProductID));
                cmd.Parameters.Add(new SqlParameter("@BatchCode", BatchCode));
                cmd.Parameters.Add(new SqlParameter("@Prefix", Prefix));
                cmd.Parameters.Add(new SqlParameter("@LaserCodeFrom", LaserCodeFrom));
                cmd.Parameters.Add(new SqlParameter("@LaserCodeTo", LaserCodeTo));
                cmd.Parameters.Add(new SqlParameter("@DateofManufacturing", DateofManufacturing));
                cmd.Parameters.Add(new SqlParameter("@Weight", Weight));
                cmd.Parameters.Add(new SqlParameter("@BatchCheckedBy", BatchCheckedBy));
                cmd.Parameters.Add(new SqlParameter("@Remarks", Remarks));
                cmd.Parameters.Add(new SqlParameter("@CreateDateTime", CreateDateTime));

                SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                isexists.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    IsExists = (int)isexists.Value;
                    con.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }
        public bool UpdateHSRPPBatch(string BatchID, int ProductID, string BatchCode, string Prefix, Int64 LaserCodeFrom, Int64 LaserCodeTo, DateTime DateofManufacturing, decimal Weight, int BatchCheckedBy, DateTime CreateDateTime, string Remarks, ref int IsExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Transaction_Update_Batch]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@BatchID", BatchID));
                cmd.Parameters.Add(new SqlParameter("@ProductID", ProductID));
                cmd.Parameters.Add(new SqlParameter("@BatchCode", BatchCode));
                cmd.Parameters.Add(new SqlParameter("@Prefix", Prefix));
                cmd.Parameters.Add(new SqlParameter("@LaserCodeFrom", LaserCodeFrom));
                cmd.Parameters.Add(new SqlParameter("@LaserCodeTo", LaserCodeTo));
                cmd.Parameters.Add(new SqlParameter("@DateofManufacturing", DateofManufacturing));
                cmd.Parameters.Add(new SqlParameter("@Weight", Weight));
                cmd.Parameters.Add(new SqlParameter("@BatchCheckedBy", BatchCheckedBy));
                cmd.Parameters.Add(new SqlParameter("@Remarks", Remarks));
                cmd.Parameters.Add(new SqlParameter("@CreateDateTime", CreateDateTime));

                SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                isexists.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    IsExists = (int)isexists.Value;
                    con.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public void SaveBankTranction(List<string> lst,out string strResult)
        {            
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Transaction_SaveBankTransaction]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@DepositDate", DateTime.Parse(lst[0])));
                cmd.Parameters.Add(new SqlParameter("@BankName", lst[1]));
                cmd.Parameters.Add(new SqlParameter("@depolocationid", lst[2]));
                cmd.Parameters.Add(new SqlParameter("@BranchName", lst[3]));

                cmd.Parameters.Add(new SqlParameter("@DepositAmount", lst[4]));
                cmd.Parameters.Add(new SqlParameter("@DepositBy", lst[5]));
                cmd.Parameters.Add(new SqlParameter("@StateID", lst[6]));
                cmd.Parameters.Add(new SqlParameter("@RTOLocation", lst[7]));
                cmd.Parameters.Add(new SqlParameter("@UserID", lst[8]));

                cmd.Parameters.Add(new SqlParameter("@BankSlipNo", lst[9]));
                cmd.Parameters.Add(new SqlParameter("@Remarks", lst[10]));
                cmd.Parameters.Add(new SqlParameter("@AccountNo", lst[11]));
                SqlParameter objSQLParm = new SqlParameter("@Result", SqlDbType.VarChar, 100);
                objSQLParm.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(objSQLParm);


                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    strResult = (string)objSQLParm.Value;
                        
                    con.Close();
                }
                catch
                {
                    strResult = "Error";
                }

            }

        }

        public void SaveBankTranctionwithUser(List<string> lst, out string strResult)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Transaction_SaveBankTransactionWithUser]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@DepositDate", DateTime.Parse(lst[0])));
                cmd.Parameters.Add(new SqlParameter("@BankName", lst[1]));           
                cmd.Parameters.Add(new SqlParameter("@BranchName", lst[2]));
                cmd.Parameters.Add(new SqlParameter("@DepositAmount", lst[3]));
                cmd.Parameters.Add(new SqlParameter("@DepositBy", lst[4]));
                cmd.Parameters.Add(new SqlParameter("@StateID", lst[5]));
                cmd.Parameters.Add(new SqlParameter("@RTOLocation", lst[6]));
                cmd.Parameters.Add(new SqlParameter("@UserID", lst[7]));
                cmd.Parameters.Add(new SqlParameter("@BankSlipNo", lst[8]));
                cmd.Parameters.Add(new SqlParameter("@Remarks", lst[9]));
                cmd.Parameters.Add(new SqlParameter("@AccountNo", lst[10]));
                cmd.Parameters.Add(new SqlParameter("@depolocationid", lst[11]));
                cmd.Parameters.Add(new SqlParameter("@DealerID", lst[12]));
                cmd.Parameters.Add(new SqlParameter("@ChequeDate", DateTime.Parse(lst[13])));
                cmd.Parameters.Add(new SqlParameter("@chq_no", (lst[14])));
                SqlParameter objSQLParm = new SqlParameter("@Result", SqlDbType.VarChar, 100);
                objSQLParm.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(objSQLParm);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    strResult = (string)objSQLParm.Value;

                    con.Close();
                }
                catch
                {
                    strResult = "Error";
                }

            }

        }




        public int 
            UpdateBankTranction(List<string> lst)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Transaction_UpdateBankTransaction]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@DepositDate", DateTime.Parse(lst[0])));
                cmd.Parameters.Add(new SqlParameter("@BankName", lst[1]));
                cmd.Parameters.Add(new SqlParameter("@depolocationid", lst[2]));
                cmd.Parameters.Add(new SqlParameter("@BranchName", lst[3]));

                cmd.Parameters.Add(new SqlParameter("@DepositAmount", lst[4]));
                cmd.Parameters.Add(new SqlParameter("@DepositBy", lst[5]));
                cmd.Parameters.Add(new SqlParameter("@StateID", lst[6]));
                cmd.Parameters.Add(new SqlParameter("@RTOLocation", lst[7]));
                cmd.Parameters.Add(new SqlParameter("@UserID", lst[8]));

                cmd.Parameters.Add(new SqlParameter("@BankSlipNo", lst[9]));
                cmd.Parameters.Add(new SqlParameter("@Remarks", lst[10]));
                cmd.Parameters.Add(new SqlParameter("@AccountNo", lst[11]));
                cmd.Parameters.Add(new SqlParameter("@TransactionID", lst[12]));



                try
                {
                    con.Open();
                    i = cmd.ExecuteNonQuery();
                    con.Close();
                    return i;

                }
                catch (Exception)
                {
                    return i;
                }

            }
        }





        public int InsertMachineOperatorProductivity(List<string> lst)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Transaction_SaveMOProductivity]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@MachineID", lst[0]));
                cmd.Parameters.Add(new SqlParameter("@OperatorName", lst[1]));
                cmd.Parameters.Add(new SqlParameter("@ProductID", lst[2]));
                cmd.Parameters.Add(new SqlParameter("@Quantity", lst[3]));
                cmd.Parameters.Add(new SqlParameter("@ScrapQuantity", lst[4]));
                cmd.Parameters.Add(new SqlParameter("@ScrapWeight", lst[5]));
                cmd.Parameters.Add(new SqlParameter("@Remarks", lst[6]));
                cmd.Parameters.Add(new SqlParameter("@StateID", lst[7]));
                cmd.Parameters.Add(new SqlParameter("@RTOLocation", lst[8]));
                cmd.Parameters.Add(new SqlParameter("@UserID", lst[9]));
                cmd.Parameters.Add(new SqlParameter("@PlantID", lst[10]));
                //cmd.Parameters.Add(new SqlParameter("@ProductivityDate", DateTime.Parse(lst[10])));


                try
                {
                    con.Open();
                    i = cmd.ExecuteNonQuery();
                    con.Close();
                    return i;

                }
                catch (Exception)
                {
                    return i;
                }

            }
        }

        public int UpdateMachineOperatorProductivity(List<string> lst)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Transaction_UpdateMOProductivity]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@MachineID", lst[0]));
                cmd.Parameters.Add(new SqlParameter("@OperatorName", lst[1]));
                cmd.Parameters.Add(new SqlParameter("@ProductID", lst[2]));
                cmd.Parameters.Add(new SqlParameter("@Quantity", lst[3]));
                cmd.Parameters.Add(new SqlParameter("@ScrapQuantity", lst[4]));
                cmd.Parameters.Add(new SqlParameter("@ScrapWeight", lst[5]));
                cmd.Parameters.Add(new SqlParameter("@Remarks", lst[6]));
                cmd.Parameters.Add(new SqlParameter("@StateID", lst[7]));
                cmd.Parameters.Add(new SqlParameter("@RTOLocation", lst[8]));
                cmd.Parameters.Add(new SqlParameter("@UserID", lst[9]));
                //cmd.Parameters.Add(new SqlParameter("@ProductivityDate", DateTime.Parse(lst[10])));
                cmd.Parameters.Add(new SqlParameter("@ProductivityID", lst[10]));
                cmd.Parameters.Add(new SqlParameter("@PlantID", lst[11]));

                try
                {
                    con.Open();
                    i = cmd.ExecuteNonQuery();
                    con.Close();
                    return i;

                }
                catch (Exception)
                {
                    return i;
                }

            }
        }

        public bool UpdateAssignInventory(int HSRP_StateID, int RTOLocationID, int BatchID, string Prefix, Int64 LaserNoWithoutPrefix)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {

                SqlCommand cmd = new SqlCommand("[Transaction_AssigenInventory]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@HSRP_StateID", HSRP_StateID));
                cmd.Parameters.Add(new SqlParameter("@RTOLocationID", RTOLocationID));
                cmd.Parameters.Add(new SqlParameter("@BatchID", BatchID));
                cmd.Parameters.Add(new SqlParameter("@Prefix", Prefix));
                cmd.Parameters.Add(new SqlParameter("@LaserNoWithoutPrefix", LaserNoWithoutPrefix));

                //SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                //isexists.Direction = ParameterDirection.Output;
                //cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    //IsExists = (int)isexists.Value;
                    con.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool InsertHSRPPGoodDispatchPlate(int StateID, int RTOLocationID, int DispatchLocationID, string BoxNo, string DispatchRefNo, string BillingAddress, string ShippingAddress, string Remark, ref int ISExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {

                SqlCommand cmd = new SqlCommand("[Transaction_InsertGoodDispatchPlate]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@HSRP_StateID", StateID));
                cmd.Parameters.Add(new SqlParameter("@EmbossingLocation", RTOLocationID));
                cmd.Parameters.Add(new SqlParameter("@DispatchLocation", DispatchLocationID));
                cmd.Parameters.Add(new SqlParameter("@BoxNo", BoxNo));
                cmd.Parameters.Add(new SqlParameter("@DispatchRefNo", DispatchRefNo));
                cmd.Parameters.Add(new SqlParameter("@BillingAddress", BillingAddress));
                cmd.Parameters.Add(new SqlParameter("@ShippingAddress", ShippingAddress));
                cmd.Parameters.Add(new SqlParameter("@Remark", Remark));

                SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                isexists.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }
        public bool UpdateHSRPPGoodDispatchPlate(int StateID, string DispatchID, int RTOLocationID, int DispatchLocationID, string DispatchRefNo, string BillingAddress, string ShippingAddress, string Remark, ref int ISExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {

                SqlCommand cmd = new SqlCommand("[Transaction_UpdateGoodDispatchPlate]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@HSRP_StateID", StateID));
                cmd.Parameters.Add(new SqlParameter("@GoodDispatchPlateID", DispatchID));
                cmd.Parameters.Add(new SqlParameter("@EmbossingLocation", RTOLocationID));
                cmd.Parameters.Add(new SqlParameter("@DispatchLocation", DispatchLocationID));
                cmd.Parameters.Add(new SqlParameter("@DispatchRefNo", DispatchRefNo));
                cmd.Parameters.Add(new SqlParameter("@BillingAddress", BillingAddress));
                cmd.Parameters.Add(new SqlParameter("@ShippingAddress", ShippingAddress));
                cmd.Parameters.Add(new SqlParameter("@Remark", Remark));

                SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                isexists.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }


        public int InsertResources(List<string> lst)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Trans_SaveResources]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ResourcesType", lst[0]));
                cmd.Parameters.Add(new SqlParameter("@quantity", int.Parse(lst[1])));
                cmd.Parameters.Add(new SqlParameter("@ResourcesDetail", lst[2]));
                cmd.Parameters.Add(new SqlParameter("@CreatedBy", lst[3]));
                cmd.Parameters.Add(new SqlParameter("@CreatedDate", DateTime.Parse(lst[4])));
                cmd.Parameters.Add(new SqlParameter("@UpdatedDate", DateTime.Parse(lst[5])));


                try
                {
                    con.Open();
                    i = cmd.ExecuteNonQuery();
                    con.Close();
                    return i;

                }
                catch (Exception)
                {
                    return i;
                }

            }
        }

        public int UpdateResources(List<string> lst)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Trans_UpdateResources]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@ResourcesType", lst[0]));
                cmd.Parameters.Add(new SqlParameter("@quantity", int.Parse(lst[1])));
                cmd.Parameters.Add(new SqlParameter("@ResourcesDetail", lst[2]));
                cmd.Parameters.Add(new SqlParameter("@CreatedBy", lst[3]));
                cmd.Parameters.Add(new SqlParameter("@CreatedDate", DateTime.Parse(lst[4])));
                cmd.Parameters.Add(new SqlParameter("@UpdatedDate", DateTime.Parse(lst[5])));
                cmd.Parameters.Add(new SqlParameter("@ResID", lst[6]));


                try
                {
                    con.Open();
                    i = cmd.ExecuteNonQuery();
                    con.Close();
                    return i;

                }
                catch (Exception)
                {
                    return i;
                }

            }
        }

        public bool MacInActive(int MacBaseRequestsID, String ActiveStatus)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                //                
                SqlCommand cmd = new SqlCommand("[Master_MacInActiveStatus]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@MacBaseRequestsID", MacBaseRequestsID));
                cmd.Parameters.Add(new SqlParameter("@ActiveStatus", ActiveStatus));

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    // ISExists = (int)isexists.Value;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }
        public bool UpdateMacBaser(int MacBaseRequestsID, int HSRP_StateID, int RTOLocationID)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                //                
                SqlCommand cmd = new SqlCommand("[Master_UpdateMacBase]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@MacBaseRequestsID", MacBaseRequestsID));
                cmd.Parameters.Add(new SqlParameter("@HSRP_StateID", HSRP_StateID));

                cmd.Parameters.Add(new SqlParameter("@RTOLocationID", RTOLocationID));

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    // ISExists = (int)isexists.Value;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }
        public bool UpdateHSRPVehicleMake(int VehicleID, int userID, string VehicleMake, DateTime UpdateDate, string ActiveStatus, ref int ISExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                //                
                SqlCommand cmd = new SqlCommand("[Master_UpdateVehicleMake]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@VehicleID", VehicleID));
                cmd.Parameters.Add(new SqlParameter("@userID", userID));
                cmd.Parameters.Add(new SqlParameter("@VehicleMake", VehicleMake));
                cmd.Parameters.Add(new SqlParameter("@UpdateDate", UpdateDate));
                cmd.Parameters.Add(new SqlParameter("@ActiveStatus", ActiveStatus));
                SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                isexists.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ISExists = (int)isexists.Value;
                    con.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool InsertHSRPVehicleMake(int userID, string VehicleMake, DateTime CreateDate, string ActiveStatus, ref int ISExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                //                
                SqlCommand cmd = new SqlCommand("[Master_HSRPInsertVehicleMake]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@userID", userID));
                cmd.Parameters.Add(new SqlParameter("@VehicleMake", VehicleMake));
                cmd.Parameters.Add(new SqlParameter("@CreateDate", CreateDate));
                cmd.Parameters.Add(new SqlParameter("@ActiveStatus", ActiveStatus));
                SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                isexists.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ISExists = (int)isexists.Value;
                    con.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }
        public int SaveVehicleModel(List<string> lst)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Trans_SaveVehicleModel]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@VehicleMakerID", int.Parse(lst[0])));
                cmd.Parameters.Add(new SqlParameter("@VehicleModelDescription", lst[1]));
                cmd.Parameters.Add(new SqlParameter("@ActiveStatus", lst[2]));
                cmd.Parameters.Add(new SqlParameter("@CreatedBy", int.Parse(lst[3])));

                try
                {
                    con.Open();
                    i = cmd.ExecuteNonQuery();
                    con.Close();
                    return i;

                }
                catch (Exception)
                {
                    return i;
                }

            }
        }

        public int UpdateVehicleModel(List<string> lst)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Trans_UpdateVehicleModel]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@VehicleMakerID", int.Parse(lst[0])));
                cmd.Parameters.Add(new SqlParameter("@VehicleModelDescription", lst[1]));
                cmd.Parameters.Add(new SqlParameter("@ActiveStatus", lst[2]));
                cmd.Parameters.Add(new SqlParameter("@CreatedBy", int.Parse(lst[3])));
                cmd.Parameters.Add(new SqlParameter("@VehicleModelID", int.Parse(lst[4])));

                try
                {
                    con.Open();
                    i = cmd.ExecuteNonQuery();
                    con.Close();
                    return i;


                }
                catch (Exception)
                {
                    return i;
                }

            }
        }
        public bool InsertHSRPPInventoryPurchase(int ProductID, string UserID, string BatchCode, string Prefix, string Quantity, decimal CurrentCost, decimal Weight, string InvoiceNo, string InvoiceFrom, int stateID, int LocID, string Remarks, Int64 Laser, string LaserNo, string orderStatus, ref int IsExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Transaction_InsertInventory_Purchase]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ProductID", ProductID));
                cmd.Parameters.Add(new SqlParameter("@UserID", UserID));
                cmd.Parameters.Add(new SqlParameter("@BatchCode", BatchCode));
                cmd.Parameters.Add(new SqlParameter("@Prefix", Prefix));
                cmd.Parameters.Add(new SqlParameter("@Quantity", Quantity));
                cmd.Parameters.Add(new SqlParameter("@CurrentCost", CurrentCost));
                cmd.Parameters.Add(new SqlParameter("@Weight", Weight));
                cmd.Parameters.Add(new SqlParameter("@InvoiceNo", InvoiceNo));
                cmd.Parameters.Add(new SqlParameter("@InvoiceFrom", InvoiceFrom));
                cmd.Parameters.Add(new SqlParameter("@Remarks", Remarks));
                cmd.Parameters.Add(new SqlParameter("@StateID", stateID));
                cmd.Parameters.Add(new SqlParameter("@LocID", LocID));
                cmd.Parameters.Add(new SqlParameter("@Laser", Laser));
                cmd.Parameters.Add(new SqlParameter("@LaserNo", LaserNo));
                cmd.Parameters.Add(new SqlParameter("@orderStatus", orderStatus));

                SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                isexists.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    IsExists = (int)isexists.Value;
                    con.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }


        public bool InsertHSRPPInventory(int ProductID, string UserID, string BatchCode, string Prefix, string Quantity, decimal CurrentCost, decimal Weight, string InvoiceNo, string InvoiceFrom, int stateID, int LocID, string Remarks, Int64 Laser, string LaserNo, string orderStatus, ref int IsExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Transaction_InsertInventory]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ProductID", ProductID));
                cmd.Parameters.Add(new SqlParameter("@UserID", UserID));
                cmd.Parameters.Add(new SqlParameter("@BatchCode", BatchCode));
                cmd.Parameters.Add(new SqlParameter("@Prefix", Prefix));
                cmd.Parameters.Add(new SqlParameter("@Quantity", Quantity));
                cmd.Parameters.Add(new SqlParameter("@CurrentCost", CurrentCost));
                cmd.Parameters.Add(new SqlParameter("@Weight", Weight));
                cmd.Parameters.Add(new SqlParameter("@InvoiceNo", InvoiceNo));
                cmd.Parameters.Add(new SqlParameter("@InvoiceFrom", InvoiceFrom));
                cmd.Parameters.Add(new SqlParameter("@Remarks", Remarks));
                cmd.Parameters.Add(new SqlParameter("@StateID", stateID));
                cmd.Parameters.Add(new SqlParameter("@LocID", LocID));
                cmd.Parameters.Add(new SqlParameter("@Laser", Laser));
                cmd.Parameters.Add(new SqlParameter("@LaserNo", LaserNo));
                cmd.Parameters.Add(new SqlParameter("@orderStatus", orderStatus));

                SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                isexists.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    IsExists = (int)isexists.Value;
                    con.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool UpdateHSRPPInventory(string Prefix, int intHSRPStateID, int intRTOLocationID)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[UpdateInventory_RTOInventory]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Prefix", Prefix));
                cmd.Parameters.Add(new SqlParameter("@StateID", intHSRPStateID));
                cmd.Parameters.Add(new SqlParameter("@RTOLocationID", intRTOLocationID));

                //SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                //isexists.Direction = ParameterDirection.Output;
                //cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    //IsExists = (int)isexists.Value;
                    con.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public int UpdateInvoiceNo(string HSRPRecordID, int HSRPStateID, string UserID, string OldRegPlate)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Invoice_UpdateInvoiceNo]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@HSRPRecordID", HSRPRecordID));
                cmd.Parameters.Add(new SqlParameter("@HSRPStateID", HSRPStateID));
                cmd.Parameters.Add(new SqlParameter("@UserID", UserID));
                cmd.Parameters.Add(new SqlParameter("@OldRegPlate", OldRegPlate));

                try
                {
                    con.Open();
                    i = cmd.ExecuteNonQuery();
                    con.Close();
                    return i;
                }
                catch (Exception)
                {
                    return i;
                }
            }
        }

        //ambrish Custommer Registration

        SqlConnection con;
        SqlDataAdapter da;
        SqlDataReader dr;
        DataSet ds;

        public int InsertCustomerRegistration(List<string> lst)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Invoice_InsertCustomerRegistration]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Name", lst[0]));
                cmd.Parameters.Add(new SqlParameter("@Address1", lst[1]));
                cmd.Parameters.Add(new SqlParameter("@Address2", lst[2]));
                cmd.Parameters.Add(new SqlParameter("@City", lst[3]));
                cmd.Parameters.Add(new SqlParameter("@State", lst[4]));
                cmd.Parameters.Add(new SqlParameter("@Country", lst[5]));
                cmd.Parameters.Add(new SqlParameter("@PinNo", lst[6]));
                cmd.Parameters.Add(new SqlParameter("@ContactPerson", lst[7]));
                cmd.Parameters.Add(new SqlParameter("@MobileNo", lst[8]));
                cmd.Parameters.Add(new SqlParameter("@LandlineNo", lst[9]));
                cmd.Parameters.Add(new SqlParameter("@EmailID", lst[10]));
                cmd.Parameters.Add(new SqlParameter("@TinNo", lst[11]));
                cmd.Parameters.Add(new SqlParameter("@VatNo", lst[12]));
                cmd.Parameters.Add(new SqlParameter("@CST", lst[13]));
                cmd.Parameters.Add(new SqlParameter("@ExciseNo", lst[14]));
                cmd.Parameters.Add(new SqlParameter("@Remarks", lst[15]));
                cmd.Parameters.Add(new SqlParameter("@ActiveMachine", lst[16]));
                cmd.Parameters.Add(new SqlParameter("@BillingCity", lst[17]));
                cmd.Parameters.Add(new SqlParameter("@BillingState", lst[18]));
                cmd.Parameters.Add(new SqlParameter("@BillingCountry", lst[19]));
                cmd.Parameters.Add(new SqlParameter("@BillingPinNo", lst[20]));
                try
                {
                    con.Open();
                    i = cmd.ExecuteNonQuery();
                    con.Close();
                    return i;

                }
                catch (Exception)
                {
                    return i;
                }

            }
        }

        public DataTable ShowCustomerDetail()
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlDataAdapter dAd = new SqlDataAdapter("Invoice_ShowCustomerDetail", conn);
            dAd.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataSet dSet = new DataSet();
            try
            {
                dAd.Fill(dSet, "Accounts");
                return dSet.Tables["Accounts"];
            }
            catch
            {
                throw;
            }
            finally
            {
                dSet.Dispose();
                dAd.Dispose();
                conn.Close();
                conn.Dispose();
            }
        }

        //fetch the value for edit 

        public DataSet EditCustomerDetail(string CustomerID)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@CustomerID", CustomerID);
            DataSet ds = ExecuteDataSet("Invoice_EditCustomerDetail", param, true, false);
            return ds;
        }


        //edit end
        public int UpdateCustomerRegistration(List<string> lst)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Invoice_UpdateCustomerRegistration]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Name", lst[0]));
                cmd.Parameters.Add(new SqlParameter("@Address1", lst[1]));
                cmd.Parameters.Add(new SqlParameter("@Address2", lst[2]));
                cmd.Parameters.Add(new SqlParameter("@City", lst[3]));
                cmd.Parameters.Add(new SqlParameter("@State", lst[4]));
                cmd.Parameters.Add(new SqlParameter("@Country", lst[5]));
                cmd.Parameters.Add(new SqlParameter("@PinNo", lst[6]));
                cmd.Parameters.Add(new SqlParameter("@ContactPerson", lst[7]));
                cmd.Parameters.Add(new SqlParameter("@MobileNo", lst[8]));
                cmd.Parameters.Add(new SqlParameter("@LandlineNo", lst[9]));
                cmd.Parameters.Add(new SqlParameter("@EmailID", lst[10]));
                cmd.Parameters.Add(new SqlParameter("@TinNo", lst[11]));
                cmd.Parameters.Add(new SqlParameter("@VatNo", lst[12]));
                cmd.Parameters.Add(new SqlParameter("@CST", lst[13]));
                cmd.Parameters.Add(new SqlParameter("@ExciseNo", lst[14]));
                cmd.Parameters.Add(new SqlParameter("@Remarks", lst[15]));
                cmd.Parameters.Add(new SqlParameter("@ActiveMachine", lst[16]));
                cmd.Parameters.Add(new SqlParameter("@BillingCity", lst[17]));
                cmd.Parameters.Add(new SqlParameter("@BillingState", lst[18]));
                cmd.Parameters.Add(new SqlParameter("@BillingCountry", lst[19]));
                cmd.Parameters.Add(new SqlParameter("@BillingPinNo", lst[20]));
                cmd.Parameters.Add(new SqlParameter("@CustomerID", lst[21]));
                try
                {
                    con.Open();
                    i = cmd.ExecuteNonQuery();
                    con.Close();
                    return i;

                }
                catch (Exception)
                {
                    return i;
                }
            }
        }

        // Customer registration end


        // Vendor Registration start
        public int InsertVendorRegistration(List<string> lst)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Invoice_InsertVendorRegistration]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Name", lst[0]));
                cmd.Parameters.Add(new SqlParameter("@Address1", lst[1]));
                // cmd.Parameters.Add(new SqlParameter("@Address2", lst[2]));
                cmd.Parameters.Add(new SqlParameter("@City", lst[2]));
                cmd.Parameters.Add(new SqlParameter("@State", lst[3]));
                cmd.Parameters.Add(new SqlParameter("@Country", lst[4]));
                cmd.Parameters.Add(new SqlParameter("@PinNo", lst[5]));
                cmd.Parameters.Add(new SqlParameter("@ContactPerson", lst[6]));
                cmd.Parameters.Add(new SqlParameter("@MobileNo", lst[7]));
                cmd.Parameters.Add(new SqlParameter("@LandlineNo", lst[8]));
                cmd.Parameters.Add(new SqlParameter("@EmailID", lst[9]));
                cmd.Parameters.Add(new SqlParameter("@TinNo", lst[10]));
                cmd.Parameters.Add(new SqlParameter("@VatNo", lst[11]));
                cmd.Parameters.Add(new SqlParameter("@CST", lst[12]));
                cmd.Parameters.Add(new SqlParameter("@ExciseNo", lst[13]));
                cmd.Parameters.Add(new SqlParameter("@Remarks", lst[14]));
                cmd.Parameters.Add(new SqlParameter("@ActiveMachine", lst[15]));

                //cmd.Parameters.Add(new SqlParameter("@BillingCity", lst[17]));
                //cmd.Parameters.Add(new SqlParameter("@BillingState", lst[18]));
                //cmd.Parameters.Add(new SqlParameter("@BillingCountry", lst[19]));
                //cmd.Parameters.Add(new SqlParameter("@BillingPinNo", lst[20]));
                try
                {
                    con.Open();
                    i = cmd.ExecuteNonQuery();
                    con.Close();
                    return i;

                }
                catch (Exception)
                {
                    return i;
                }

            }
        }

        public DataTable ShowVendorDetail()
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlDataAdapter dAd = new SqlDataAdapter("Invoice_ShowVendorDetail", conn);
            dAd.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataSet dSet = new DataSet();
            try
            {
                dAd.Fill(dSet, "Accounts");
                return dSet.Tables["Accounts"];
            }
            catch
            {
                throw;
            }
            finally
            {
                dSet.Dispose();
                dAd.Dispose();
                conn.Close();
                conn.Dispose();
            }
        }

        //fetch the value for edit 

        public DataSet EditVendorDetail(string VendorID)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@VendorID", VendorID);
            DataSet ds = ExecuteDataSet("Invoice_EditVendorDetail", param, true, false);
            return ds;
        }

        public DataSet ExecuteDataSet(string text, SqlParameter[] param, bool procedure, bool view)
        {

            con = new SqlConnection(ConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand();
                if (procedure == true)
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                }
                else if (view == true)
                {
                    cmd.CommandType = CommandType.Text;
                }
                else
                {
                    cmd.CommandType = CommandType.Text;

                }
                cmd.CommandText = text;
                cmd.Connection = con;
                if (param != null)
                {
                    foreach (SqlParameter par in param)
                    {
                        cmd.Parameters.Add(par);
                    }
                }
                da = new SqlDataAdapter(cmd);
                ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                con.Close();
            }
        }
        //edit end
        public int UpdateVendorRegistration(List<string> lst)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Invoice_UpdateVendorRegistration]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Name", lst[0]));
                cmd.Parameters.Add(new SqlParameter("@Address1", lst[1]));
                //cmd.Parameters.Add(new SqlParameter("@Address2", lst[2]));
                cmd.Parameters.Add(new SqlParameter("@City", lst[2]));
                cmd.Parameters.Add(new SqlParameter("@State", lst[3]));
                cmd.Parameters.Add(new SqlParameter("@Country", lst[4]));
                cmd.Parameters.Add(new SqlParameter("@PinNo", lst[5]));
                cmd.Parameters.Add(new SqlParameter("@ContactPerson", lst[6]));
                cmd.Parameters.Add(new SqlParameter("@MobileNo", lst[7]));
                cmd.Parameters.Add(new SqlParameter("@LandlineNo", lst[8]));
                cmd.Parameters.Add(new SqlParameter("@EmailID", lst[9]));
                cmd.Parameters.Add(new SqlParameter("@TinNo", lst[10]));
                cmd.Parameters.Add(new SqlParameter("@VatNo", lst[11]));
                cmd.Parameters.Add(new SqlParameter("@CST", lst[12]));
                cmd.Parameters.Add(new SqlParameter("@ExciseNo", lst[13]));
                cmd.Parameters.Add(new SqlParameter("@Remarks", lst[14]));
                cmd.Parameters.Add(new SqlParameter("@ActiveMachine", lst[15]));
                //cmd.Parameters.Add(new SqlParameter("@BillingCity", lst[17]));
                //cmd.Parameters.Add(new SqlParameter("@BillingState", lst[18]));
                //cmd.Parameters.Add(new SqlParameter("@BillingCountry", lst[19]));
                //cmd.Parameters.Add(new SqlParameter("@BillingPinNo", lst[20]));

                cmd.Parameters.Add(new SqlParameter("@VendorID", lst[16]));


                try
                {
                    con.Open();
                    i = cmd.ExecuteNonQuery();
                    con.Close();
                    return i;

                }
                catch (Exception)
                {
                    return i;
                }
            }
        }


        // Vendor Registration end





        public int InsertInvoiceProduct(List<string> lst)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Invoice_InsertInvoiceProduct]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ProductName", lst[0]));
                cmd.Parameters.Add(new SqlParameter("@ProductCode", lst[1]));
                cmd.Parameters.Add(new SqlParameter("@ProductColor", lst[2]));
                cmd.Parameters.Add(new SqlParameter("@ProductCost", lst[3]));
                cmd.Parameters.Add(new SqlParameter("@P_MeasurementUnit", lst[4]));
                cmd.Parameters.Add(new SqlParameter("@ProductDescription", lst[5]));
                cmd.Parameters.Add(new SqlParameter("@ActiveStatus", lst[6]));

                try
                {
                    con.Open();
                    i = cmd.ExecuteNonQuery();
                    con.Close();
                    return i;

                }
                catch (Exception)
                {
                    return i;
                }
            }
        }

        public int UpdateInvoiceProduct(List<string> lst)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Invoice_UpdateInvoiceProduct]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ProductName", lst[0]));
                cmd.Parameters.Add(new SqlParameter("@ProductCode", lst[1]));
                cmd.Parameters.Add(new SqlParameter("@ProductColor", lst[2]));
                cmd.Parameters.Add(new SqlParameter("@ProductCost", lst[3]));
                cmd.Parameters.Add(new SqlParameter("@P_MeasurementUnit", lst[4]));
                cmd.Parameters.Add(new SqlParameter("@ProductDescription", lst[5]));
                cmd.Parameters.Add(new SqlParameter("@ActiveStatus", lst[6]));
                cmd.Parameters.Add(new SqlParameter("@ProductID", lst[7]));
                try
                {
                    con.Open();
                    i = cmd.ExecuteNonQuery();
                    con.Close();
                    return i;

                }
                catch (Exception)
                {
                    return i;
                }
            }
        }


        public DataSet EditProductDetail(string ProductID)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@ProductID", ProductID);
            DataSet ds = ExecuteDataSet("Invoice_EditInvoiceProduct", param, true, false);
            return ds;
        }

        public DataTable ShowInvoiceProduct()
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlDataAdapter dAd = new SqlDataAdapter("Invoice_ShowInvoiceProduct", conn);
            dAd.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataSet dSet = new DataSet();
            try
            {
                dAd.Fill(dSet, "Accounts");
                return dSet.Tables["Accounts"];
            }
            catch
            {
                throw;
            }
            finally
            {
                dSet.Dispose();
                dAd.Dispose();
                conn.Close();
                conn.Dispose();
            }
        }

        //product end

        //invoice Order 




        public int InsertInvoiceOreder(List<string> lst)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {

                SqlCommand cmd = new SqlCommand("[Invoice_InsertInvoiceOreder]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@ProductID", int.Parse(lst[0])));
                cmd.Parameters.Add(new SqlParameter("@Quantity", int.Parse(lst[1])));
                cmd.Parameters.Add(new SqlParameter("@Rate", decimal.Parse(lst[2])));
                cmd.Parameters.Add(new SqlParameter("@MeasurementUnit", lst[3]));
                cmd.Parameters.Add(new SqlParameter("@LaserCodeNoFrom", lst[4]));
                cmd.Parameters.Add(new SqlParameter("@LaserCodeNoTo", lst[5]));
                cmd.Parameters.Add(new SqlParameter("@HeaderID", int.Parse(lst[6])));


                try
                {
                    con.Open();
                    i = cmd.ExecuteNonQuery();
                    con.Close();
                    return i;

                }
                catch (Exception)
                {
                    return i;
                }
            }
        }

        public DataTable ShowInvoiceOrder()
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlDataAdapter dAd = new SqlDataAdapter("Invoice_ShowInvoiceOrder", conn);
            dAd.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataSet dSet = new DataSet();
            try
            {
                dAd.Fill(dSet, "Accounts");
                return dSet.Tables["Accounts"];
            }
            catch
            {
                throw;
            }
            finally
            {
                dSet.Dispose();
                dAd.Dispose();
                conn.Close();
                conn.Dispose();
            }
        }

        public DataSet FetchAddressByCustomerID(int Customerid)
        {

            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@Customerid", Customerid);
            DataSet ds = ExecuteDataSet("Invoice_FetchAddressByCustomerID", param, true, false);
            return ds;

        }

        public int InsertHeader(List<string> lst1)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {

                SqlCommand cmd = new SqlCommand("[Invoice_InsertHeader]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CustomerID", int.Parse(lst1[0])));
                cmd.Parameters.Add(new SqlParameter("@TransportName", lst1[1]));

                cmd.Parameters.Add(new SqlParameter("@GoodsReceiptNote", lst1[2]));
                cmd.Parameters.Add(new SqlParameter("@TransportVia", lst1[3]));
                cmd.Parameters.Add(new SqlParameter("@BillingAddress", lst1[4]));
                cmd.Parameters.Add(new SqlParameter("@ShippingAddress", lst1[5]));


                cmd.Parameters.Add(new SqlParameter("@Status", lst1[6]));


                try
                {
                    con.Open();
                    i = cmd.ExecuteNonQuery();
                    con.Close();
                    return i;

                }
                catch (Exception)
                {
                    return i;
                }
            }
        }

        public DataTable FetchHeaderID()
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlDataAdapter dAd = new SqlDataAdapter("Invoice_FetchHeaderID", conn);
            dAd.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataSet dSet = new DataSet();
            try
            {
                dAd.Fill(dSet, "Accounts");
                return dSet.Tables["Accounts"];
            }
            catch
            {
                throw;
            }
            finally
            {
                dSet.Dispose();
                dAd.Dispose();
                conn.Close();
                conn.Dispose();
            }
        }



        public DataSet editInvoiceOrderbyID(string InvoiceID)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@ID", InvoiceID);
            DataSet ds = ExecuteDataSet("Invoice_editInvoiceOrderbyID", param, true, false);
            return ds;
        }

        public int UpdateInvoiceHeader(List<string> lst1)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {

                SqlCommand cmd = new SqlCommand("[Invoice_UpdateInvoiceHeader]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CustomerID", int.Parse(lst1[0])));
                cmd.Parameters.Add(new SqlParameter("@TransportName", lst1[1]));

                cmd.Parameters.Add(new SqlParameter("@GoodsReceiptNote", lst1[2]));
                cmd.Parameters.Add(new SqlParameter("@TransportVia", lst1[3]));
                cmd.Parameters.Add(new SqlParameter("@BillingAddress", lst1[4]));
                cmd.Parameters.Add(new SqlParameter("@ShippingAddress", lst1[5]));
                cmd.Parameters.Add(new SqlParameter("@Status", lst1[6]));
                cmd.Parameters.Add(new SqlParameter("@HeaderID", int.Parse(lst1[7])));



                try
                {
                    con.Open();
                    i = cmd.ExecuteNonQuery();
                    con.Close();
                    return i;

                }
                catch (Exception)
                {
                    return i;
                }
            }
        }

        public int UpdateInvoiceOrder(List<string> lst4)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {

                SqlCommand cmd = new SqlCommand("[Invoice_UpdateInvoiceOrder]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ProductID", int.Parse(lst4[0])));
                cmd.Parameters.Add(new SqlParameter("@Quantity", int.Parse(lst4[1])));
                cmd.Parameters.Add(new SqlParameter("@Rate", decimal.Parse(lst4[2])));
                cmd.Parameters.Add(new SqlParameter("@MeasurementUnit", lst4[3]));
                cmd.Parameters.Add(new SqlParameter("@LaserCodeNoFrom", lst4[4]));
                cmd.Parameters.Add(new SqlParameter("@LaserCodeNoTo", lst4[5]));
                cmd.Parameters.Add(new SqlParameter("@HeaderID", int.Parse(lst4[6])));
                cmd.Parameters.Add(new SqlParameter("@ID", int.Parse(lst4[7])));
                try
                {
                    con.Open();
                    i = cmd.ExecuteNonQuery();
                    con.Close();
                    return i;

                }
                catch (Exception)
                {
                    return i;
                }
            }
        }


        public DataSet ShowInvoiceOrderbyID(string p)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@ID", p);
            DataSet ds = ExecuteDataSet("Invoice_ShowInvoiceOrderbyID", param, true, false);
            return ds;
        }

        public DataSet ShowViewInvoicePDF(string OrderDate1, string InvoiceID)
        {
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@OrderDate", OrderDate1);
            param[1] = new SqlParameter("@HeaderID", InvoiceID);
            DataSet ds = ExecuteDataSet("Invoice_FetchInvoiceDetailByDatePDf", param, true, false);
            return ds;
        }



        public DataSet FetchAddressByProductID(int ProductID)
        {

            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@ProductID", ProductID);
            DataSet ds = ExecuteDataSet("Invoice_FetchAddressByProductID", param, true, false);
            return ds;

        }



        //PO detail





        public int InsertVendorOreder(List<string> lst)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {

                SqlCommand cmd = new SqlCommand("[Invoice_InsertPOOreder]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@ProductID", int.Parse(lst[0])));
                cmd.Parameters.Add(new SqlParameter("@Quantity", int.Parse(lst[1])));
                cmd.Parameters.Add(new SqlParameter("@Rate", decimal.Parse(lst[2])));
                cmd.Parameters.Add(new SqlParameter("@MesurmentUnit", lst[3]));
                cmd.Parameters.Add(new SqlParameter("@POheaderID", int.Parse(lst[4])));



                try
                {
                    con.Open();
                    i = cmd.ExecuteNonQuery();
                    con.Close();
                    return i;

                }
                catch (Exception)
                {
                    return i;
                }
            }
        }

        public DataTable FetchVendorHeaderID()
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlDataAdapter dAd = new SqlDataAdapter("Invoice_FetchPOHeaderID", conn);
            dAd.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataSet dSet = new DataSet();
            try
            {
                dAd.Fill(dSet, "Accounts");
                return dSet.Tables["Accounts"];
            }
            catch
            {
                throw;
            }
            finally
            {
                dSet.Dispose();
                dAd.Dispose();
                conn.Close();
                conn.Dispose();
            }
        }

        public int InsertVendorHeader(List<string> lst1, ref int id)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {

                SqlCommand cmd = new SqlCommand("[Invoice_InsertPOHeader]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@VendorID", int.Parse(lst1[0])));
                cmd.Parameters.Add(new SqlParameter("@BillingAddress", lst1[1]));
                cmd.Parameters.Add(new SqlParameter("@ShippingAddress", lst1[2]));
                cmd.Parameters.Add(new SqlParameter("@Remark", lst1[3]));
                cmd.Parameters.Add(new SqlParameter("@PaymentTerm", lst1[4]));
                cmd.Parameters.Add(new SqlParameter("@Date", lst1[5]));
                cmd.Parameters.Add(new SqlParameter("@Status", lst1[6]));
                SqlParameter CMD = new SqlParameter("@id", SqlDbType.Int);
                CMD.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(CMD);
                try
                {
                    con.Open();
                    i = cmd.ExecuteNonQuery();
                    id = (int)CMD.Value;
                    con.Close();
                    return i;

                }
                catch (Exception)
                {
                    return i;
                }
            }
        }

        public int UpdatePOOrder(List<string> lst4)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {

                SqlCommand cmd = new SqlCommand("[Invoice_UpdatePOOrder]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ProductID", int.Parse(lst4[0])));
                cmd.Parameters.Add(new SqlParameter("@Quantity", int.Parse(lst4[1])));
                cmd.Parameters.Add(new SqlParameter("@Rate", decimal.Parse(lst4[2])));
                cmd.Parameters.Add(new SqlParameter("@MesurmentUnit", lst4[3]));
                cmd.Parameters.Add(new SqlParameter("@POheaderID", int.Parse(lst4[4])));
                cmd.Parameters.Add(new SqlParameter("@POorderID", int.Parse(lst4[5])));
                try
                {
                    con.Open();
                    i = cmd.ExecuteNonQuery();
                    con.Close();
                    return i;

                }
                catch (Exception)
                {
                    return i;
                }
            }
        }

        public DataSet editVendorOrderbyID(string VendorID)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@POheaderID", VendorID);
            DataSet ds = ExecuteDataSet("Invoice_editPOOrderbyID", param, true, false);
            return ds;
        }

        public int UpdateVendorHeader(List<string> lst1)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {

                SqlCommand cmd = new SqlCommand("[Invoice_UpdatePOHeader]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@VendorID", int.Parse(lst1[0])));
                cmd.Parameters.Add(new SqlParameter("@BillingAddress", lst1[1]));
                cmd.Parameters.Add(new SqlParameter("@ShippingAddress", lst1[2]));
                cmd.Parameters.Add(new SqlParameter("@Remark", lst1[3]));
                cmd.Parameters.Add(new SqlParameter("@PaymentTerm", lst1[4]));
                cmd.Parameters.Add(new SqlParameter("@Date", lst1[5]));
                cmd.Parameters.Add(new SqlParameter("@Status", lst1[6]));
                cmd.Parameters.Add(new SqlParameter("@POheaderID", int.Parse(lst1[7])));



                try
                {
                    con.Open();
                    i = cmd.ExecuteNonQuery();
                    con.Close();
                    return i;

                }
                catch (Exception)
                {
                    return i;
                }
            }
        }



        public DataSet ShowViewPO(string POOrderDate)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@POOrderDate", POOrderDate);
            DataSet ds = ExecuteDataSet("Invoice_ShowViewPOByDate", param, true, false);
            return ds;
        }

        public DataSet ShowViewPO2(string POOrderDate)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@POOrderDate", POOrderDate);
            DataSet ds = ExecuteDataSet("Invoice_ShowViewPOByDate2", param, true, false);
            return ds;
        }

        public DataSet editVendorOrderbyID1(string id)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@POorderID", id);
            DataSet ds = ExecuteDataSet("Invoice_editPOOrderbyID1", param, true, false);
            return ds;
        }

        public bool GetdownloadReport(string state, string location,string start, string end, string typesch, ref DataTable ds)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[GetDownloadProduction]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@State", state));
                cmd.Parameters.Add(new SqlParameter("@Location", location));
                cmd.Parameters.Add(new SqlParameter("@start", start));
                cmd.Parameters.Add(new SqlParameter("@End", end));
                cmd.Parameters.Add(new SqlParameter("@typesch", typesch));
                SqlDataAdapter Adap = new SqlDataAdapter(cmd);
                try
                {
                    Adap.Fill(ds);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public DataTable Show1()
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlDataAdapter dAd = new SqlDataAdapter("Invoice_POshow", conn);
            dAd.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataSet dSet = new DataSet();
            try
            {
                dAd.Fill(dSet, "Accounts");
                return dSet.Tables["Accounts"];
            }
            catch
            {
                throw;
            }
            finally
            {
                dSet.Dispose();
                dAd.Dispose();
                conn.Close();
                conn.Dispose();
            }
        }

        public DataSet ShowInsertValue(string p)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@POheaderID", p);
            DataSet ds = ExecuteDataSet("Invoice_ShowInsertValue", param, true, false);
            return ds;
        }

        public int deletePO(int ID)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {

                SqlCommand cmd = new SqlCommand("[Invoice_deletePOByID]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@POorderID", ID));

                try
                {
                    con.Open();
                    i = cmd.ExecuteNonQuery();
                    con.Close();
                    return i;

                }
                catch (Exception)
                {
                    return i;
                }
            }
        }

        public DataSet ShowViewPO1(string POOrderDate, string POID)
        {
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@POOrderDate", POOrderDate);
            param[1] = new SqlParameter("@POheaderID", POID);
            DataSet ds = ExecuteDataSet("Invoice_ShowViewPOByDate1", param, true, false);
            return ds;
        }

        public DataSet ShowViewPO3(string POID)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@POheaderID", POID);
            DataSet ds = ExecuteDataSet("Invoice_ShowViewPOByDate3", param, true, false);
            return ds;
        }




        // invoice 

        public DataSet ShowInsertValueInvoice(string p)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@HeaderID", p);
            DataSet ds = ExecuteDataSet("Invoice_ShowInsertValueInvoice", param, true, false);
            return ds;
        }

        public DataSet dataEntryreport(string FromDate, string p, char p_2)
        {
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@reportDate", FromDate);
            param[1] = new SqlParameter("@reportstateid", p);
            param[2] = new SqlParameter("@reportlocationid", p_2);
            DataSet ds = ExecuteDataSet("Report_userWisePRODUCTION", param, true, false);
            return ds;
        }

        public DataSet ShowViewInvoice(string OrderDate1)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@OrderDate", OrderDate1);
            DataSet ds = ExecuteDataSet("Invoice_FetchInvoiceDetailByDate1", param, true, false);
            return ds;
        }



        public DataSet ShowViewinvoice1(string headerID)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@HeaderID", headerID);
            DataSet ds = ExecuteDataSet("Invoice_FetchInvoiceDetailByHeaderID", param, true, false);
            return ds;
        }

        //collection Detail start
        public DataTable Fetchcollection_details()
        {

            String ConnectionStringrto = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringRTO"].ToString();

            SqlConnection conn = new SqlConnection(ConnectionStringrto);
            SqlDataAdapter dAd = new SqlDataAdapter("Invoice_Fetchcollection_details", conn);
            dAd.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataSet dSet = new DataSet();
            try
            {
                dAd.Fill(dSet, "Accounts");
                return dSet.Tables["Accounts"];
            }
            catch
            {
                throw;
            }
            finally
            {
                dSet.Dispose();
                dAd.Dispose();
                conn.Close();
                conn.Dispose();
            }
        }




        public int Insertcollection_details(List<string> lst1)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {

                SqlCommand cmd = new SqlCommand("[Insertcollection_details]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@prefix", lst1[0]));
                cmd.Parameters.Add(new SqlParameter("@billno", lst1[1]));
                cmd.Parameters.Add(new SqlParameter("@hrsp", lst1[2]));
                cmd.Parameters.Add(new SqlParameter("@name", lst1[3]));
                cmd.Parameters.Add(new SqlParameter("@mobileno", lst1[4]));
                cmd.Parameters.Add(new SqlParameter("@regno", lst1[5]));
                cmd.Parameters.Add(new SqlParameter("@vclass", lst1[6]));
                cmd.Parameters.Add(new SqlParameter("@vtype", lst1[7]));
                cmd.Parameters.Add(new SqlParameter("@ttype", lst1[8]));
                cmd.Parameters.Add(new SqlParameter("@stateid", lst1[9]));
                cmd.Parameters.Add(new SqlParameter("@regid", lst1[10]));
                cmd.Parameters.Add(new SqlParameter("@rate", lst1[11]));
                cmd.Parameters.Add(new SqlParameter("@tax", lst1[12]));
                cmd.Parameters.Add(new SqlParameter("@amount", lst1[13]));
                cmd.Parameters.Add(new SqlParameter("@username", lst1[14]));
                cmd.Parameters.Add(new SqlParameter("@dateandtime", lst1[15]));
                cmd.Parameters.Add(new SqlParameter("@machine_id", lst1[16]));
                cmd.Parameters.Add(new SqlParameter("@void_bit", lst1[17]));

                //SqlParameter CMD = new SqlParameter("@id", SqlDbType.Int);
                //CMD.Direction = ParameterDirection.Output;
                //cmd.Parameters.Add(CMD);
                try
                {
                    con.Open();
                    i = cmd.ExecuteNonQuery();
                    // id = (int)CMD.Value;
                    con.Close();
                    return i;

                }
                catch (Exception)
                {
                    return i;
                }
            }
        }





        public int Updatecollection_details(List<string> lst1)
        {

            int i = 0;
            String ConnectionStringrto = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringRTO"].ToString();
            using (SqlConnection con = new SqlConnection(ConnectionStringrto))
            {

                SqlCommand cmd = new SqlCommand("[Updatecollection_details]", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.Add(new SqlParameter("@id", lst1[0]));
                //SqlParameter CMD = new SqlParameter("@id", SqlDbType.Int);
                //CMD.Direction = ParameterDirection.Output;
                //cmd.Parameters.Add(CMD);
                try
                {
                    con.Open();
                    i = cmd.ExecuteNonQuery();
                    // id = (int)CMD.Value;
                    con.Close();
                    return i;

                }
                catch (Exception)
                {
                    return i;
                }
            }
        }
        public bool TrVehicleRecord(string VehicleRegNo, int StateID, ref DataTable ds)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[GetTFVehicleRegNo]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@VehicleRegNo", VehicleRegNo));
                cmd.Parameters.Add(new SqlParameter("@StateID", StateID));
                SqlDataAdapter Adap = new SqlDataAdapter(cmd);
                try
                {
                    Adap.Fill(ds);
                    con.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        //collenction Detail end

        //tanuj
        public bool InsertHSRPRecords(string macbaseaddress,string HSRPRecord_AuthorizationNo, DateTime HSRPRecord_AuthorizationDate, string OrderNo,
        String OwnerName, String OrderStatus, string Address1, string Address2, string MobileNo, string LandlineNo,
        string EmailID, string VehicleClass, string StringManufacturerName, string StringManufacturerModel, string VehicleType, string VehicleRegNo, string EngineNo,
        string ChassisNo, string OrderType, string ISFrontPlateSize, string FrontPlateSize, string ISRearPlateSize,
        string RearPlateSize, string StickerMandatory, string InvoiceNo,string CashReceiptNo_HHT, string CashReceiptNo, string CashReceiptDate, string VAT_Percentage,
        string VAT_Amount, string ServiceTax_Percentage, string ServiceTax_Amount, string TotalAmount, string NetAmount,
        string HSRPStateID, string RTOLocationID, string DeliveryChallan, string StringFixingCharge, string StringFrontPlatePrize,
        string StringRearPlatePrize, string StringStickerPrize, string StringScrewPrize, string Remarks, int UID, string RTOLocationIDd, ref int isexists, ref string HSRPRecordID)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Transaction_HSRPRecordInsertXX_New2]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@MacbaseAddress", macbaseaddress));
                cmd.Parameters.Add(new SqlParameter("@HSRPRecord_AuthorizationNo", HSRPRecord_AuthorizationNo));
                cmd.Parameters.Add(new SqlParameter("@HSRPRecord_AuthorizationDate", HSRPRecord_AuthorizationDate));
                cmd.Parameters.Add(new SqlParameter("@OrderNo", OrderNo));
                cmd.Parameters.Add(new SqlParameter("@OwnerName", OwnerName));
                cmd.Parameters.Add(new SqlParameter("@OrderStatus", OrderStatus));
                cmd.Parameters.Add(new SqlParameter("@Address1", Address1));
                cmd.Parameters.Add(new SqlParameter("@Address2", Address2));
                cmd.Parameters.Add(new SqlParameter("@MobileNo", MobileNo));
                cmd.Parameters.Add(new SqlParameter("@LandlineNo", LandlineNo));
                cmd.Parameters.Add(new SqlParameter("@EmailID", EmailID));
                cmd.Parameters.Add(new SqlParameter("@VehicleClass", VehicleClass));
                cmd.Parameters.Add(new SqlParameter("@ManufacturerName", StringManufacturerName));
                cmd.Parameters.Add(new SqlParameter("@ManufacturerModel", StringManufacturerModel));
                cmd.Parameters.Add(new SqlParameter("@VehicleType", VehicleType));
                cmd.Parameters.Add(new SqlParameter("@VehicleRegNo", VehicleRegNo));
                cmd.Parameters.Add(new SqlParameter("@EngineNo", EngineNo));
                cmd.Parameters.Add(new SqlParameter("@ChassisNo", ChassisNo));
                cmd.Parameters.Add(new SqlParameter("@OrderType", OrderType));
                cmd.Parameters.Add(new SqlParameter("@ISFrontPlateSize", ISFrontPlateSize));
                cmd.Parameters.Add(new SqlParameter("@FrontPlateSize", FrontPlateSize));
                cmd.Parameters.Add(new SqlParameter("@ISRearPlateSize", ISRearPlateSize));
                cmd.Parameters.Add(new SqlParameter("@RearPlateSize", RearPlateSize));
                cmd.Parameters.Add(new SqlParameter("@StickerMandatory", StickerMandatory));
                cmd.Parameters.Add(new SqlParameter("@InvoiceNo", InvoiceNo));
                cmd.Parameters.Add(new SqlParameter("@CashReceiptNo", CashReceiptNo));
                cmd.Parameters.Add(new SqlParameter("@CashReceiptNo_HHT", CashReceiptNo_HHT));
                cmd.Parameters.Add(new SqlParameter("@CashReceiptDatetime_HHT", CashReceiptDate));
                cmd.Parameters.Add(new SqlParameter("@VAT_Percentage", VAT_Percentage));
                cmd.Parameters.Add(new SqlParameter("@VAT_Amount", VAT_Amount));
                cmd.Parameters.Add(new SqlParameter("@ServiceTax_Percentage", ServiceTax_Percentage));
                cmd.Parameters.Add(new SqlParameter("@ServiceTax_Amount", ServiceTax_Amount));
                cmd.Parameters.Add(new SqlParameter("@TotalAmount", TotalAmount));
                cmd.Parameters.Add(new SqlParameter("@NetAmount", NetAmount));
                cmd.Parameters.Add(new SqlParameter("@HSRP_StateID", HSRPStateID));
                cmd.Parameters.Add(new SqlParameter("@RTOLocationID", RTOLocationID));
                cmd.Parameters.Add(new SqlParameter("@DeliveryChallan", DeliveryChallan));
                cmd.Parameters.Add(new SqlParameter("@FixingCharge", StringFixingCharge));
                cmd.Parameters.Add(new SqlParameter("@FrontPlatePrize", StringFrontPlatePrize));
                cmd.Parameters.Add(new SqlParameter("@RearPlatePrize", StringRearPlatePrize));
                cmd.Parameters.Add(new SqlParameter("@StickerPrize", StringStickerPrize));
                cmd.Parameters.Add(new SqlParameter("@ScrewPrize", StringScrewPrize));
                cmd.Parameters.Add(new SqlParameter("@Remarks", Remarks));
                cmd.Parameters.Add(new SqlParameter("@UID", UID));
                cmd.Parameters.Add(new SqlParameter("@RTOLocationIDd", RTOLocationIDd));


                SqlParameter HSRPRecordIDs = new SqlParameter("@HSRPRecordID", SqlDbType.VarChar, 100);
                HSRPRecordIDs.Direction = ParameterDirection.Output;

                SqlParameter kk = new SqlParameter("@IsExists", SqlDbType.Int);
                kk.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(kk);
                cmd.Parameters.Add(HSRPRecordIDs);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    isexists = (int)kk.Value;
                    if (isexists != 1)
                    {
                        HSRPRecordID = (string)HSRPRecordIDs.Value;
                    }
                    con.Close();
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            return true;
        }
        public bool QuickAssignLaser_AP(int StateID, string FrondLaserCode, string RearLaserCode, string VRegNo, int userid,string vehicleclass,string vehicletype,string ordertype, ref int ISExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                //string sqlSticker, string sqlFrontLaser, string sqlRearPlate,string HSRP_Sticker_LaserCode, 
                SqlCommand cmd = new SqlCommand("[QuickLaserClose_AP]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@StateId", StateID));
                cmd.Parameters.Add(new SqlParameter("@FrontLaser", FrondLaserCode));
                cmd.Parameters.Add(new SqlParameter("@RearLaser", RearLaserCode));
                cmd.Parameters.Add(new SqlParameter("@VRegNo", VRegNo));
                cmd.Parameters.Add(new SqlParameter("@userid", userid));
                cmd.Parameters.Add(new SqlParameter("@vehicletype", vehicletype));
                cmd.Parameters.Add(new SqlParameter("@vehicleclass", vehicleclass));
                cmd.Parameters.Add(new SqlParameter("@ordertype", ordertype));
                SqlParameter isexists = new SqlParameter("@IsExists", SqlDbType.Int);
                isexists.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ISExists = (int)isexists.Value;
                    con.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }
        public bool QuickAssignLaser(int StateID, string FrondLaserCode, string RearLaserCode, string VRegNo,  int userid, ref int ISExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                //string sqlSticker, string sqlFrontLaser, string sqlRearPlate,string HSRP_Sticker_LaserCode, 
                SqlCommand cmd = new SqlCommand("[QuickLaserClose]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@StateId", StateID));
                cmd.Parameters.Add(new SqlParameter("@FrontLaser", FrondLaserCode));
                cmd.Parameters.Add(new SqlParameter("@RearLaser", RearLaserCode));
                cmd.Parameters.Add(new SqlParameter("@VRegNo", VRegNo));
                cmd.Parameters.Add(new SqlParameter("@userid", userid));
                SqlParameter isexists = new SqlParameter("@IsExists", SqlDbType.Int);
                isexists.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ISExists = (int)isexists.Value;
                    con.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }
        public bool FillHSRPRecordDeliveryChallan2(string HSRPAuthNo, ref DataTable ds)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[FillHSRPRecordDeliveryChallan2]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@HSRPAuthNo", HSRPAuthNo));
                SqlDataAdapter Adap = new SqlDataAdapter(cmd);
                try
                {
                    Adap.Fill(ds);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool InsertHSRPPInventory1(int ProductID, string UserID, string BatchCode, string Prefix, string Quantity, decimal CurrentCost, decimal Weight, int stateID, int LocID, string Remarks, Int64 Laser, string LaserNo, string orderStatus, ref int IsExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Transaction_InsertInventorystock]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ProductID", ProductID));
                cmd.Parameters.Add(new SqlParameter("@UserID", UserID));
                cmd.Parameters.Add(new SqlParameter("@BatchCode", BatchCode));
                cmd.Parameters.Add(new SqlParameter("@Prefix", Prefix));
                cmd.Parameters.Add(new SqlParameter("@Quantity", Quantity));
                cmd.Parameters.Add(new SqlParameter("@CurrentCost", CurrentCost));
                cmd.Parameters.Add(new SqlParameter("@Weight", Weight));
                //  cmd.Parameters.Add(new SqlParameter("@InvoiceNo", InvoiceNo));
                // cmd.Parameters.Add(new SqlParameter("@InvoiceFrom", InvoiceFrom));
                cmd.Parameters.Add(new SqlParameter("@Remarks", Remarks));
                cmd.Parameters.Add(new SqlParameter("@StateID", stateID));
                cmd.Parameters.Add(new SqlParameter("@LocID", LocID));
                cmd.Parameters.Add(new SqlParameter("@Laser", Laser));
                cmd.Parameters.Add(new SqlParameter("@LaserNo", LaserNo));
                cmd.Parameters.Add(new SqlParameter("@orderStatus", orderStatus));

                SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                isexists.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    IsExists = (int)isexists.Value;
                    con.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }


        public bool InsertDelhiOldDataCashCollection(List<string> lst)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Transaction_SaveDelhiOldDataCashCollection]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@HSRPRecord_CreationDate", lst[0]));
                cmd.Parameters.Add(new SqlParameter("@HSRP_StateID", lst[1]));
                cmd.Parameters.Add(new SqlParameter("@RTOLocationID ", lst[2]));
                cmd.Parameters.Add(new SqlParameter("@HSRPRecord_AuthorizationNo   ", lst[3]));
                cmd.Parameters.Add(new SqlParameter("@HSRPRecord_AuthorizationDate    ", lst[4]));
                cmd.Parameters.Add(new SqlParameter("@VehicleRegNo", lst[5]));
                cmd.Parameters.Add(new SqlParameter("@OwnerName", lst[6]));
                cmd.Parameters.Add(new SqlParameter("@ownerFatherName", lst[7]));
                cmd.Parameters.Add(new SqlParameter("@Address1", lst[8]));
                cmd.Parameters.Add(new SqlParameter("@MobileNo  ", lst[9]));
                cmd.Parameters.Add(new SqlParameter("@VehicleClass    ", lst[10]));
                cmd.Parameters.Add(new SqlParameter("@OrderType", lst[11]));
                cmd.Parameters.Add(new SqlParameter("@StickerMandatory ", lst[12]));
                cmd.Parameters.Add(new SqlParameter("@isVIP   ", lst[13]));
                cmd.Parameters.Add(new SqlParameter("@NetAmount", lst[14]));
                cmd.Parameters.Add(new SqlParameter("@Roundoff_NetAmount", lst[15]));
                cmd.Parameters.Add(new SqlParameter("@VehicleType  ", lst[16]));
                cmd.Parameters.Add(new SqlParameter("@OrderStatus    ", lst[17]));
                cmd.Parameters.Add(new SqlParameter("@CashReceiptNo     ", lst[18]));
                cmd.Parameters.Add(new SqlParameter("@ChassisNo", lst[19]));
                cmd.Parameters.Add(new SqlParameter("@EngineNo", lst[20]));
                cmd.Parameters.Add(new SqlParameter("@DealerCode  ", lst[21]));
                cmd.Parameters.Add(new SqlParameter("@CreatedBy", lst[22]));
                cmd.Parameters.Add(new SqlParameter("@SaveMacAddress", lst[23]));
                cmd.Parameters.Add(new SqlParameter("@Addrecordby", lst[24]));
                cmd.Parameters.Add(new SqlParameter("@ISFrontPlateSize", lst[25]));
                cmd.Parameters.Add(new SqlParameter("@ISRearPlateSize  ", lst[26]));
                cmd.Parameters.Add(new SqlParameter("@FrontPlateSize  ", lst[27]));
                cmd.Parameters.Add(new SqlParameter("@RearPlateSize    ", lst[28]));
                cmd.Parameters.Add(new SqlParameter("@Reference      ", lst[29]));
                cmd.Parameters.Add(new SqlParameter("@ManufacturerModel        ", lst[30]));
                cmd.Parameters.Add(new SqlParameter("@vehicleref        ", lst[31]));
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();

                }
                catch (Exception e)
                {
                    return false;
                }
            }
            return true;
        }





       
    }
    


}
