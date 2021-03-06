﻿using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Threading;
using Terrasoft.Core;
using Terrasoft.Core.DB;
using Terrasoft.Web.Common;

namespace GuidedLearningClio
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class DemoWs : BaseService
    {
        #region Properties
        private SystemUserConnection _systemUserConnection;
        private SystemUserConnection SystemUserConnection
        {
            get
            {
                return _systemUserConnection ?? (_systemUserConnection = (SystemUserConnection)AppConnection.SystemUserConnection);
            }
        }
        #endregion

        //[Application Address]/0/rest/[Custom Service Name]/[Custom Service Endpoint]?[Optional Options]
        //http://k_krylov_nb:8090/0/rest/DemoWs/GetMethodname?name=Kirill

        #region Methods : REST
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public string PostMethodName(Dto person)
        {
            UserConnection userConnection = UserConnection ?? SystemUserConnection;
            
            string email = GetEmail(userConnection, person.FirstName);
            return email;
        }


        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
       ResponseFormat = WebMessageFormat.Json)]
        public string socketSample(Dto person)
        {
            UserConnection userConnection = UserConnection ?? SystemUserConnection;
            MsgChannelUtilities.PostMessageToAll(this.ToString(), "Sample message");
            Thread.Sleep(5000);
            return "Ok";
        }

        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, 
            BodyStyle = WebMessageBodyStyle.Wrapped, 
            ResponseFormat = WebMessageFormat.Json)]
        public string GetMethodname(string person)
        {
            //UserConnection userConnection = UserConnection ?? SystemUserConnection;
            return $"you Said {person}";
        }

        #endregion

        #region Methods : Private

        public string GetEmail(UserConnection userConnection, string name) {
            string result = string.Empty;
            Select select = new Select(userConnection)
                .Top(1)
                .Column("Email")
                .From("Contact")
                .Where("Name").IsEqual(Column.Parameter(name)) as Select;


            using (DBExecutor executor = userConnection.EnsureDBConnection())
            {
                result = select.ExecuteScalar<string>(executor);
            }
            return result;
        }


        #endregion
    }

    public class Dto {

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}



