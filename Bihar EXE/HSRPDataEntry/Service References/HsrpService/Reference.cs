﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HSRPDataEntry.HsrpService {
    using System.Data;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="HsrpService.HSRPServiceSoap")]
    public interface HSRPServiceSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetDataByAuthNo", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataTable GetDataByAuthNo(string AuthNo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetDataByVehicleRegNo", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataTable GetDataByVehicleRegNo(string VehicleRegNo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/BiharDataCashCollection", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string BiharDataCashCollection(
                    string HSRP_StateID, 
                    string RTOLocationID, 
                    string HSRPRecord_AuthorizationNo, 
                    string HSRPRecord_AuthorizationDate, 
                    string VehicleRegNo, 
                    string OwnerName, 
                    string ownerFatherName, 
                    string Address1, 
                    string MobileNo, 
                    string VehicleClass, 
                    string OrderType, 
                    string StickerMandatory, 
                    string isVIP, 
                    string NetAmount, 
                    string Roundoff_NetAmount, 
                    string VehicleType, 
                    string OrderStatus, 
                    string CashReceiptNo, 
                    string ChassisNo, 
                    string EngineNo, 
                    string DealerCode, 
                    string CreatedBy, 
                    string SaveMacAddress, 
                    string Addrecordby, 
                    string ISFrontPlateSize, 
                    string ISRearPlateSize, 
                    string FrontPlateSize, 
                    string RearPlateSize, 
                    string Reference, 
                    string ManufacturerModel, 
                    string vehicleref, 
                    string ManufacturerName, 
                    string CounterNo, 
                    string Affixid, 
                    string dealername);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/CheckDuplicateEntry", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string CheckDuplicateEntry(string HSRPStateID, string AuthNo, string VehicleRegNo, string OrderType);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetLogInDetail", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetLogInDetail(string UserName, string Password, string MacAddress);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetRateAndTaxDetail", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetRateAndTaxDetail(string StateID, string VehicleModel, string VehicleClass, string OrderType);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetRateAndTaxForVehicle", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetRateAndTaxForVehicle(string StateID, string VehicleType, string VehicleClass, string OrderType);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/PopulateVehicleType", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataTable PopulateVehicleType();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/PopulateVehicleModel", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataTable PopulateVehicleModel(string VehiclMake);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/PopulateVehicleMake", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataTable PopulateVehicleMake();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/RecordClosed", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataTable RecordClosed(string VehicleRegNo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/RecordNotClosed", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataTable RecordNotClosed(string VehicleRegNo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/InventoryStatusF_NotNewOrder", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string InventoryStatusF_NotNewOrder(string Flaser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/InventoryStatusR_NotNewOrder", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string InventoryStatusR_NotNewOrder(string Rlaser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/InventoryStatusF_NewOrder", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string InventoryStatusF_NewOrder(string Flaser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/InventoryStatusR_NewOrder", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string InventoryStatusR_NewOrder(string Rlaser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/FrontValidation", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string FrontValidation(string Flaser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/RearValidation", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string RearValidation(string Rlaser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/OrderStatusNotClosed", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataTable OrderStatusNotClosed(string VehiclRegNo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/OrderStatusEmbossingDone", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        int OrderStatusEmbossingDone(string Flaser, string Rlaser, string VehicleRegNo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/OrderStatusEmbossingDoneUpdate", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        int OrderStatusEmbossingDoneUpdate(string Flaser, string Rlaser, string VehicleRegNo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/OrderStatusNewOrder", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        int OrderStatusNewOrder(string Flaser, string Rlaser, string VehicleRegNo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/OrderStatusNewOrderUpdate", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        int OrderStatusNewOrderUpdate(string Flaser, string Rlaser, string VehicleRegNo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Embossing_Validate", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataTable Embossing_Validate(string VehiclRegNo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Embossing_OrderStatusNotClosed", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataTable Embossing_OrderStatusNotClosed(string VehiclRegNo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/E_OrderStatusEmbossingDone", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        int E_OrderStatusEmbossingDone(string Flaser, string Rlaser, string VehicleRegNo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Embossing_RtoInventory", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        int Embossing_RtoInventory(string Flaser, string Rlaser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Chalan", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataTable Chalan(string StateId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/filldropdownlistAFfix", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataTable filldropdownlistAFfix(string StateId, string RTOID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/FillAddrss", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataTable FillAddrss(string Affixid);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/CashReceiptForHR", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string CashReceiptForHR(string AuthNo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetDateByHoliday", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetDateByHoliday(string holidaydate);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/filldropdowndealername", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataTable filldropdowndealername(string StateId, string RTOID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/InserNewDealerName", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        int InserNewDealerName(int StateId, int RTOID, string Dealername);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface HSRPServiceSoapChannel : HSRPDataEntry.HsrpService.HSRPServiceSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class HSRPServiceSoapClient : System.ServiceModel.ClientBase<HSRPDataEntry.HsrpService.HSRPServiceSoap>, HSRPDataEntry.HsrpService.HSRPServiceSoap {
        
        public HSRPServiceSoapClient() {
        }
        
        public HSRPServiceSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public HSRPServiceSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public HSRPServiceSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public HSRPServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Data.DataTable GetDataByAuthNo(string AuthNo) {
            return base.Channel.GetDataByAuthNo(AuthNo);
        }
        
        public System.Data.DataTable GetDataByVehicleRegNo(string VehicleRegNo) {
            return base.Channel.GetDataByVehicleRegNo(VehicleRegNo);
        }
        
        public string BiharDataCashCollection(
                    string HSRP_StateID, 
                    string RTOLocationID, 
                    string HSRPRecord_AuthorizationNo, 
                    string HSRPRecord_AuthorizationDate, 
                    string VehicleRegNo, 
                    string OwnerName, 
                    string ownerFatherName, 
                    string Address1, 
                    string MobileNo, 
                    string VehicleClass, 
                    string OrderType, 
                    string StickerMandatory, 
                    string isVIP, 
                    string NetAmount, 
                    string Roundoff_NetAmount, 
                    string VehicleType, 
                    string OrderStatus, 
                    string CashReceiptNo, 
                    string ChassisNo, 
                    string EngineNo, 
                    string DealerCode, 
                    string CreatedBy, 
                    string SaveMacAddress, 
                    string Addrecordby, 
                    string ISFrontPlateSize, 
                    string ISRearPlateSize, 
                    string FrontPlateSize, 
                    string RearPlateSize, 
                    string Reference, 
                    string ManufacturerModel, 
                    string vehicleref, 
                    string ManufacturerName, 
                    string CounterNo, 
                    string Affixid, 
                    string dealername) {
            return base.Channel.BiharDataCashCollection(HSRP_StateID, RTOLocationID, HSRPRecord_AuthorizationNo, HSRPRecord_AuthorizationDate, VehicleRegNo, OwnerName, ownerFatherName, Address1, MobileNo, VehicleClass, OrderType, StickerMandatory, isVIP, NetAmount, Roundoff_NetAmount, VehicleType, OrderStatus, CashReceiptNo, ChassisNo, EngineNo, DealerCode, CreatedBy, SaveMacAddress, Addrecordby, ISFrontPlateSize, ISRearPlateSize, FrontPlateSize, RearPlateSize, Reference, ManufacturerModel, vehicleref, ManufacturerName, CounterNo, Affixid, dealername);
        }
        
        public string CheckDuplicateEntry(string HSRPStateID, string AuthNo, string VehicleRegNo, string OrderType) {
            return base.Channel.CheckDuplicateEntry(HSRPStateID, AuthNo, VehicleRegNo, OrderType);
        }
        
        public string GetLogInDetail(string UserName, string Password, string MacAddress) {
            return base.Channel.GetLogInDetail(UserName, Password, MacAddress);
        }
        
        public string GetRateAndTaxDetail(string StateID, string VehicleModel, string VehicleClass, string OrderType) {
            return base.Channel.GetRateAndTaxDetail(StateID, VehicleModel, VehicleClass, OrderType);
        }
        
        public string GetRateAndTaxForVehicle(string StateID, string VehicleType, string VehicleClass, string OrderType) {
            return base.Channel.GetRateAndTaxForVehicle(StateID, VehicleType, VehicleClass, OrderType);
        }
        
        public System.Data.DataTable PopulateVehicleType() {
            return base.Channel.PopulateVehicleType();
        }
        
        public System.Data.DataTable PopulateVehicleModel(string VehiclMake) {
            return base.Channel.PopulateVehicleModel(VehiclMake);
        }
        
        public System.Data.DataTable PopulateVehicleMake() {
            return base.Channel.PopulateVehicleMake();
        }
        
        public System.Data.DataTable RecordClosed(string VehicleRegNo) {
            return base.Channel.RecordClosed(VehicleRegNo);
        }
        
        public System.Data.DataTable RecordNotClosed(string VehicleRegNo) {
            return base.Channel.RecordNotClosed(VehicleRegNo);
        }
        
        public string InventoryStatusF_NotNewOrder(string Flaser) {
            return base.Channel.InventoryStatusF_NotNewOrder(Flaser);
        }
        
        public string InventoryStatusR_NotNewOrder(string Rlaser) {
            return base.Channel.InventoryStatusR_NotNewOrder(Rlaser);
        }
        
        public string InventoryStatusF_NewOrder(string Flaser) {
            return base.Channel.InventoryStatusF_NewOrder(Flaser);
        }
        
        public string InventoryStatusR_NewOrder(string Rlaser) {
            return base.Channel.InventoryStatusR_NewOrder(Rlaser);
        }
        
        public string FrontValidation(string Flaser) {
            return base.Channel.FrontValidation(Flaser);
        }
        
        public string RearValidation(string Rlaser) {
            return base.Channel.RearValidation(Rlaser);
        }
        
        public System.Data.DataTable OrderStatusNotClosed(string VehiclRegNo) {
            return base.Channel.OrderStatusNotClosed(VehiclRegNo);
        }
        
        public int OrderStatusEmbossingDone(string Flaser, string Rlaser, string VehicleRegNo) {
            return base.Channel.OrderStatusEmbossingDone(Flaser, Rlaser, VehicleRegNo);
        }
        
        public int OrderStatusEmbossingDoneUpdate(string Flaser, string Rlaser, string VehicleRegNo) {
            return base.Channel.OrderStatusEmbossingDoneUpdate(Flaser, Rlaser, VehicleRegNo);
        }
        
        public int OrderStatusNewOrder(string Flaser, string Rlaser, string VehicleRegNo) {
            return base.Channel.OrderStatusNewOrder(Flaser, Rlaser, VehicleRegNo);
        }
        
        public int OrderStatusNewOrderUpdate(string Flaser, string Rlaser, string VehicleRegNo) {
            return base.Channel.OrderStatusNewOrderUpdate(Flaser, Rlaser, VehicleRegNo);
        }
        
        public System.Data.DataTable Embossing_Validate(string VehiclRegNo) {
            return base.Channel.Embossing_Validate(VehiclRegNo);
        }
        
        public System.Data.DataTable Embossing_OrderStatusNotClosed(string VehiclRegNo) {
            return base.Channel.Embossing_OrderStatusNotClosed(VehiclRegNo);
        }
        
        public int E_OrderStatusEmbossingDone(string Flaser, string Rlaser, string VehicleRegNo) {
            return base.Channel.E_OrderStatusEmbossingDone(Flaser, Rlaser, VehicleRegNo);
        }
        
        public int Embossing_RtoInventory(string Flaser, string Rlaser) {
            return base.Channel.Embossing_RtoInventory(Flaser, Rlaser);
        }
        
        public System.Data.DataTable Chalan(string StateId) {
            return base.Channel.Chalan(StateId);
        }
        
        public System.Data.DataTable filldropdownlistAFfix(string StateId, string RTOID) {
            return base.Channel.filldropdownlistAFfix(StateId, RTOID);
        }
        
        public System.Data.DataTable FillAddrss(string Affixid) {
            return base.Channel.FillAddrss(Affixid);
        }
        
        public string CashReceiptForHR(string AuthNo) {
            return base.Channel.CashReceiptForHR(AuthNo);
        }
        
        public string GetDateByHoliday(string holidaydate) {
            return base.Channel.GetDateByHoliday(holidaydate);
        }
        
        public System.Data.DataTable filldropdowndealername(string StateId, string RTOID) {
            return base.Channel.filldropdowndealername(StateId, RTOID);
        }
        
        public int InserNewDealerName(int StateId, int RTOID, string Dealername) {
            return base.Channel.InserNewDealerName(StateId, RTOID, Dealername);
        }
    }
}
