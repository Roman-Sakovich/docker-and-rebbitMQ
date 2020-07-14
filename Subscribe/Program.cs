﻿using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Subscribe
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var queueName = "S1";
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                
                channel.ExchangeDeclare(exchange: "order", type: ExchangeType.Fanout);
                channel.QueueDeclare(queue: queueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                channel.QueueBind(queue: queueName,
                    exchange: "order",
                    routingKey: "");


                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] {0}", message);
                };
                channel.BasicConsume(queue: queueName,
                    autoAck: true,
                    consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}