using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using temalab.Models;

namespace temalab.Services
{
    public class CategoryService : IService<Category>
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public void add(Category record, string userName)
        {
        }

        public void add(Category record)
        {
            db.Categories.Add(record);
            db.SaveChanges();
        }

        // SELECT * FROM Category

        public IEnumerable<Category> getAll()
        {
            return db.Categories.ToList();
        }
        // SELECT * FROM Category WHERE id = {id}
        public Category getOne(int? id)
        {
            if (id == null)
            {
                throw new BadRequestException();
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                throw new HttpNotFoundException();
            }
            return category;
        }

        public void remove(int id)
        {
            db.Categories.Remove(getOne(id));
            db.SaveChanges();
        }

        public void edit(Category category)
        {
            db.Entry(category).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void delete(int id)
        {
            db.Categories.Remove(getOne(id));
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }

    }
}