using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Reflection;
using System;
using System.Text;
using System.Threading.Channels;

var factory = new ConnectionFactory()
{
    HostName = "localhost",
    UserName = "guest",
    Password = "guest",
};

var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
channel.QueueDeclare("product", exclusive: false);
var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, eventArgs) => {
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Product message received: {message}");
};

//read the message
channel.BasicConsume(queue: "product", autoAck: true, consumer: consumer);
Console.ReadKey();