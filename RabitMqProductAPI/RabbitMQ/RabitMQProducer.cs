using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace RabitMqProductAPI.RabbitMQ
{
    public class RabitMQProducer : IRabitMQProducer
    {
        public void SendProductMessage<T>(T message)
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
            };
            //Create the RabbitMQ connection
            var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare("product", exclusive: false);
            //Serialize the message
            var json = JsonConvert.SerializeObject(message);        
            var body = Encoding.UTF8.GetBytes(json);
            //put the data on the product queue
            channel.BasicPublish(exchange: "", routingKey: "product", body: body);
        }
    }
}
