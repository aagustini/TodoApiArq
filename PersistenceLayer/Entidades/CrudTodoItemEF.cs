using Entidades.Modelos;
using Microsoft.EntityFrameworkCore;
using PersistenceLayer.Interfaces;
using PersistenceLayer.Repositorisos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PersistenceLayer.Entidades
{
    public class CrudTodoItemEF : CrudEF<TodoItem>, ICrudTodoItem
    {


        private readonly DbContextOptions<TodoContext> _optionsbuilder;

        public CrudTodoItemEF()
        {
            _optionsbuilder = new DbContextOptions<TodoContext>();
        }

        public int getPendentes()
        {
            using (var data = new TodoContext(_optionsbuilder))
            {
                return data.TodoItems.Where(t=>t.IsComplete==false).Count();
            }
        }
    }

}
