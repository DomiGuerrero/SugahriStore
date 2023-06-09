﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SugahriStore.Modelos;

namespace SugahriStore.Datos
{
    public class ProductosRepositorio
    {
        private readonly BaseDeDatosContext _context;

        public ProductosRepositorio()
        {
            _context = new BaseDeDatosContext();
        }

        // Obtener todos los productos
        public List<Producto> ObtenerTodos()
        {
            return _context.Productos.ToList();
        }

        // Obtener un producto por su ID
        public Producto ObtenerPorId(int id)
        {
            return _context.Productos.FirstOrDefault(p => p.Id == id);
        }

        // Agregar un nuevo producto a la base de datos
        public void AgregarProducto(Producto producto)
        {
            var productoExistente = _context.Productos.FirstOrDefault(p => p.Id == producto.Id);
            if (productoExistente == null)
            {
                _context.Productos.Add(producto);
                _context.SaveChanges();
            }
        }
        // Agrega una lista de productos a la base de datos, si esta repetido lo actualiza
        public void AgregarProductos(List<Producto> productos)
        {
            foreach (var producto in productos)
            {
                var productoExistente = _context.Productos.FirstOrDefault(p => p.Nombre.Equals(producto.Nombre));
                if (productoExistente == null)
                {
                    _context.Productos.Add(producto);
                }
                else
                {
                    productoExistente.Coste = producto.Coste;
                    productoExistente.Inventario = producto.Inventario;
                    productoExistente.ImageUrl = producto.ImageUrl;
                }
            }
            _context.SaveChanges();
        }


        // Actualizar los datos de un producto existente en la base de datos
        public void Actualizar(Producto producto)
        {
            _context.Entry(producto).State = EntityState.Modified;
            _context.SaveChanges();
        }

        // Eliminar un producto de la base de datos
        public void Eliminar(int id)
        {
            // Buscar el producto por su ID
            Producto producto = _context.Productos.FirstOrDefault(p => p.Id == id);
            if (producto != null)
            {
                // Si se encuentra, eliminarlo y guardar los cambios
                _context.Productos.Remove(producto);
                _context.SaveChanges();
            }
        }
    }
}

