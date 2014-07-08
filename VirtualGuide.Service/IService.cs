using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using VirtualGuide.Models;

namespace VirtualGuide.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService" in both code and config file together.
    [ServiceContract]
    public interface IService
    {

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: Add your service operations here

        [OperationContract]
        [WebGet]
        List<Travel> GetTravelsList();

        [OperationContract]
        [WebGet(UriTemplate = "Travel/{id}")]
        Travel GetTravelById(string id);

        [OperationContract]
        [WebInvoke(UriTemplate = "Travel/Add/{name}")]
        void AddTravel(string name);

        [OperationContract]
        [WebInvoke(UriTemplate = "Travel/Edit/{id}/{name}")]
        void UpdateTravel(string id, string name);

        [OperationContract]
        [WebInvoke(UriTemplate = "Travel/Delete/{id}")]
        void DeleteTravel(string id);
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
