﻿namespace QuickOrderPagamento.Infra.MQ
{
    public interface IRabbitMqPub<T> where T : class
    {
        void Publicar(T obj, string routingKey, string queue);
    }
}