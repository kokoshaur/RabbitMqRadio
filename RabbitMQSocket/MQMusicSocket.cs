using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMQSocket
{
    public class MQMusicSocket
    {
        public string NameTranskation { private set; get; }


        IConnection _connection;

        IModel _channel;
        EventingBasicConsumer _consumer;

        byte[] _header;

        public MQMusicSocket(string hostName, string nameTranskation)
        {
            if (hostName == null)
                throw new Exception("Хост не может быть пустым");

            NameTranskation = nameTranskation;

            try
            {
                _connection = new ConnectionFactory() { HostName = hostName }?.CreateConnection();
                _channel = _connection?.CreateModel();
            }
            catch (Exception)
            {
                throw new Exception("Канал не найден");
            }

            if (_connection == null || _channel == null)
                throw new Exception("Неверный хост");
        }

        public void StartTranslation(string format = "wav")
        {
            _channel.QueueDeclare(queue: NameTranskation,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: true,
                                 arguments: null);

            _header = Encoding.UTF8.GetBytes(format + (char)0);
        }

        public void Send(byte[] sample)
        {
            byte[] body = new byte[_header.Length + sample.Length];
            _header.CopyTo(body, 0);
            sample.CopyTo(body, _header.Length);

            _channel.BasicPublish(exchange: "",
                                 routingKey: NameTranskation,
                                 basicProperties: null,
                                 body: body);
        }

        public void StartListening(Action<string, byte[]> playSample)
        {
            _consumer = new EventingBasicConsumer(_channel);

            _consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();

                int size = 1;
                while (body[size] != 0 && size != body.Length)
                    size++;

                string format = Encoding.UTF8.GetString(body.Take(size).ToArray());

                byte[] sample = new byte[body.Length - size - 1];

                Array.Copy(body, size + 1, sample, 0, sample.Length);

                playSample(format, sample);
            };

            _channel.BasicConsume(queue: NameTranskation,
                     autoAck: true,
                     consumer: _consumer);
        }
    }
}
