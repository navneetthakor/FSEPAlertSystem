using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WorkerService1.Modal;
using WPS_worder_node_1.Modal;

namespace WorkerService1.BL
{
    internal class AdminEmail
    {
        public static EmailContent EmailInfoGetter(string message) 
        {

            //Preparing jsong data to send along with emnail 
            string email = "webdevwithnavneet@gmail.com";
            string name = "Navneet";

            //provide proper info 
            return new EmailContent
            {
                To = email,
                Subject = $"Message from WebPulseStack",
                Body = $"Hello ADMIN {name},<br>" +
                $"<br>We have following important message for you. " +
                $"<br> Message : {message} <br> " +
                $"<br><br>" +
                $" Regards, <br>" +
                $" <br> WebPulse Stack"
            };
        }
    }
}
