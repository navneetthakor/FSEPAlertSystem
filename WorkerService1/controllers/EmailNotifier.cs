using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WorkerService1.BL;
using WorkerService1.Modal;

namespace WorkerService1.controllers
{
    internal class EmailNotifier
    {
        public static async Task NotifyError(ConsumeResult<Ignore, string> consumeResult)
        {
            //string to object 
            KafkaMessageContainer? kafkaMessageContainer = JsonConvert.DeserializeObject<KafkaMessageContainer>(consumeResult?.Message.Value);

            //preparing content for email 
            EmailContent emailContent;

            switch (kafkaMessageContainer.emailType)
            {
                case TypeOfEmail.EndpointErrorEmail:
                    emailContent = EndpointErrorEmail.EmailInfoGetter(kafkaMessageContainer.serverModal, kafkaMessageContainer.healthCheckerModal);
                    break;
                case TypeOfEmail.EndpointTestEmail:
                    emailContent = EndpointTestEmail.EmailInfoGetter(kafkaMessageContainer.serverModal, kafkaMessageContainer.healthCheckerModal);
                    break;
                case TypeOfEmail.APIFlowErrorEmail:
                    emailContent = APIFlowErrorEmail.EmailInfoGetter(kafkaMessageContainer.serverModal,kafkaMessageContainer.flowExecutionResult);
                    break;
                case TypeOfEmail.APIFlowTestEmail:
                    emailContent = APIFlowTestEmail.EmailInfoGetter(kafkaMessageContainer.serverModal, kafkaMessageContainer.flowExecutionResult);
                    break;
                default:
                    return;
            }

            //sending email 
            MyMailer.sendEmail(emailContent);

            //notify in teams too.
            TeamsMessageContent msgContent = new TeamsMessageContent()
            {
                MicrosoftId = "059da870d8194da9",
                MessageContent = emailContent.Body
            };

            await MyTeamsNotifier.NotifyAsync(msgContent);

        }
    }
}
