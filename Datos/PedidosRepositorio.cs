﻿using SugahriStore.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugahriStore.Datos
{
    public class PedidosRepositorio
    {
        private readonly BaseDeDatosContext _dbContext;

        public PedidosRepositorio()
        {
            _dbContext = new BaseDeDatosContext();
        }

        public void InsertarPedido(Pedido pedido)
        {
            _dbContext.Pedidos.Add(pedido);
            _dbContext.SaveChanges();
        }
        public void InsertarPedidos(List<Pedido> pedidos)
        {
            _dbContext.Pedidos.AddRange(pedidos);
            _dbContext.SaveChanges();
        }

        public Pedido ObtenerPedidoPorId(int id)
        {
            return _dbContext.Pedidos.FirstOrDefault(p => p.Id == id);
        }

        public List<Pedido> ObtenerPedidos()
        {
            return _dbContext.Pedidos.ToList();
        }

        public void ActualizarPedido(Pedido pedidoActualizado)
        {
            _dbContext.Pedidos.Update(pedidoActualizado);
            _dbContext.SaveChanges();
        }

        public void BorrarPedido(int id)
        {
            Pedido pedido = _dbContext.Pedidos.FirstOrDefault(p => p.Id == id);
            if (pedido != null)
            {
                _dbContext.Pedidos.Remove(pedido);
                _dbContext.SaveChanges();
            }
        }
    }
}

