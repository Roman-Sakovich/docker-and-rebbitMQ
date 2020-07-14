﻿using System;
using RabbitMQ.Client;
using System.Text;

namespace Publish
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "order", type: ExchangeType.Fanout);

                var message = "Hello!!";
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "order",
                    routingKey: "",
                    basicProperties: null,
                    body: body);
                Console.WriteLine(" [x] Sent {0}", message);
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}