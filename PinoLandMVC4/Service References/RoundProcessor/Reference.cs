﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18034
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PinoLandMVC4.RoundProcessor {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="RoundProcessor.IProcessRoundService")]
    public interface IProcessRoundService {
        
        // CODEGEN: Generating message contract since the operation ProcessRound is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProcessRoundService/ProcessRound", ReplyAction="http://tempuri.org/IProcessRoundService/ProcessRoundResponse")]
        PinoLandMVC4.RoundProcessor.ProcessRoundResponse ProcessRound(PinoLandMVC4.RoundProcessor.ProcessRoundRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ProcessRoundRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://schemas.microsoft.com/2003/10/Serialization/", Order=0)]
        public System.Nullable<int> @int;
        
        public ProcessRoundRequest() {
        }
        
        public ProcessRoundRequest(System.Nullable<int> @int) {
            this.@int = @int;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ProcessRoundResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://schemas.microsoft.com/2003/10/Serialization/", Order=0)]
        public System.Nullable<bool> boolean;
        
        public ProcessRoundResponse() {
        }
        
        public ProcessRoundResponse(System.Nullable<bool> boolean) {
            this.boolean = boolean;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IProcessRoundServiceChannel : PinoLandMVC4.RoundProcessor.IProcessRoundService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ProcessRoundServiceClient : System.ServiceModel.ClientBase<PinoLandMVC4.RoundProcessor.IProcessRoundService>, PinoLandMVC4.RoundProcessor.IProcessRoundService {
        
        public ProcessRoundServiceClient() {
        }
        
        public ProcessRoundServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ProcessRoundServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ProcessRoundServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ProcessRoundServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        PinoLandMVC4.RoundProcessor.ProcessRoundResponse PinoLandMVC4.RoundProcessor.IProcessRoundService.ProcessRound(PinoLandMVC4.RoundProcessor.ProcessRoundRequest request) {
            return base.Channel.ProcessRound(request);
        }
        
        public System.Nullable<bool> ProcessRound(System.Nullable<int> @int) {
            PinoLandMVC4.RoundProcessor.ProcessRoundRequest inValue = new PinoLandMVC4.RoundProcessor.ProcessRoundRequest();
            inValue.@int = @int;
            PinoLandMVC4.RoundProcessor.ProcessRoundResponse retVal = ((PinoLandMVC4.RoundProcessor.IProcessRoundService)(this)).ProcessRound(inValue);
            return retVal.boolean;
        }
    }
}
