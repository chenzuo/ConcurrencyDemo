﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18063
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PetConsole.PetService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="PetService.IPetService")]
    public interface IPetService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPetService/GetPetOwner", ReplyAction="http://tempuri.org/IPetService/GetPetOwnerResponse")]
        PetClubLib.Models.PetOwner GetPetOwner();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPetService/GetPetOwner", ReplyAction="http://tempuri.org/IPetService/GetPetOwnerResponse")]
        System.Threading.Tasks.Task<PetClubLib.Models.PetOwner> GetPetOwnerAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPetService/GetPet", ReplyAction="http://tempuri.org/IPetService/GetPetResponse")]
        PetClubLib.Models.PetModel GetPet();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPetService/GetPet", ReplyAction="http://tempuri.org/IPetService/GetPetResponse")]
        System.Threading.Tasks.Task<PetClubLib.Models.PetModel> GetPetAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IPetServiceChannel : PetConsole.PetService.IPetService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class PetServiceClient : System.ServiceModel.ClientBase<PetConsole.PetService.IPetService>, PetConsole.PetService.IPetService {
        
        public PetServiceClient() {
        }
        
        public PetServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public PetServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PetServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PetServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public PetClubLib.Models.PetOwner GetPetOwner() {
            return base.Channel.GetPetOwner();
        }
        
        public System.Threading.Tasks.Task<PetClubLib.Models.PetOwner> GetPetOwnerAsync() {
            return base.Channel.GetPetOwnerAsync();
        }
        
        public PetClubLib.Models.PetModel GetPet() {
            return base.Channel.GetPet();
        }
        
        public System.Threading.Tasks.Task<PetClubLib.Models.PetModel> GetPetAsync() {
            return base.Channel.GetPetAsync();
        }
    }
}
