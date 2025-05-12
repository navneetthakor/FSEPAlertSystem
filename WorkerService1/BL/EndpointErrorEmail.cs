using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerService1.Modal;
using WPS_worder_node_1.Modal;

namespace WorkerService1.BL
{
    internal class EndpointErrorEmail
    {
        public static EmailContent EmailInfoGetter(ServerModal sm, HealthCheckerModal hcm)
        {
            ////send request to the user service and get required data.
            ////getting data from env
            string userServiceUrl = Environment.GetEnvironmentVariable("MY_USER_SERVICE_URL");

            ////create rest client 
            RestClient client = new RestClient(userServiceUrl);

            ////new request to serve 
            RestRequest request = new RestRequest($"User/getUserInfo/{sm.Client_id}", Method.Get);

            ////executing request 
            RestResponse rr = client.Execute(request);

            Response? response = JsonConvert.DeserializeObject<Response>(rr.Content);

            //ClientModal cm = response.Other;

            //temprory ----
            ClientModal cm = new ClientModal()
            {
                Client_email = (string)response?.Other?.email,
                Client_name = (string)response?.Other?.username,
                API_flow_name = (string)sm.Api_flow_name
            };

            //provide proper info 
            return new EmailContent
            {
                To = cm.Client_email,
                Subject = $"Unwanted Incident Reported for {cm.Endpoint_name}!!!",
                Body = $"Hello {cm.Client_name},<br><br>Your are receiving this email becuase your endoint {cm.Endpoint_name} faced unwanted event. so please resolve it and reactivate checking when you done. <br> For your reference it is returning following: <br> status code: {hcm.StatusCode} <br> response time: {hcm.ResponseTime} <br> Message: {hcm.ErrorMessage} <br> <br> Regards, <br> WebPulse Stack"
            };
        }

    }
}
