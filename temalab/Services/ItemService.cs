using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using temalab.Models;
using temalab.Services;

namespace temalab.Services
{
    public class ItemService : IService<Item>
    {
        ApplicationDbContext db = new ApplicationDbContext();


        public Item getOne(int? id)
        {
            if (id == null)
            {
                throw new BadRequestException();
            }
            Item item = db.Items.Include(i => i.Owner).Include(i => i.Buyer).Include(i => i.Category).FirstOrDefault(i => i.ID == id);
            if (item == null)
            {
                throw new HttpNotFoundException();
            }
            return item;
        }
        public Item getOne(int? id, string userName)
        {
            if (id == null)
            {
                throw new BadRequestException();
            }
            Item item = db.Items.Include(i => i.Owner).Include(i => i.Buyer).Include(i => i.Category).FirstOrDefault(i => i.ID == id);
            if (item == null)
            {
                throw new HttpNotFoundException();
            }
            return item;
        }

        public IEnumerable<Item> getAll()
        {
            return db.Items.Include(i => i.Buyer).Include(i => i.Owner).Include(i => i.Category).ToList();
        }

        public IEnumerable<Item> getbyCategory(int? categoryID)
        {
            var category = db.Categories.Find(categoryID);
            return db.Items.Include(i => i.Buyer).Include(i => i.Owner).Include(i => i.Category).Where(i => i.Category.CategoryID == category.CategoryID).ToList();
        }

        public IEnumerable<Item> Select(int? categoryID, string searchString)
        {
            var items = getAll();
            if (!string.IsNullOrEmpty(searchString))
            {
                items = items.Where(i => i.Name.Contains(searchString));
            }

            if (categoryID != null)
            {
                items = items.Where(x => x.CategoryID == categoryID);
            }

            return items.ToList();
        }

        public void add(Item item, string userName)
        {
            item.Owner = db.Users.FirstOrDefault(u => u.UserName == userName);
            item.DefaultPic = "~/Images/noimage.jpg";
            db.Items.Add(item);
            db.SaveChanges();
        }

        public void remove(int id)
        {
            db.Items.Remove(getOne(id));
            db.SaveChanges();
        }


        public bool bid(Item item, string userName)
        {
            Item biddedItem = db.Items.Include(i => i.Buyer).Include(i => i.Owner).FirstOrDefault(x => x.ID == item.ID);
            if (item.BiggestTip <= biddedItem.BiggestTip || biddedItem.Owner.UserName == userName)
            {
                return false;
            }

            biddedItem.BiggestTip = item.BiggestTip;
            biddedItem.Buyer = db.Users.FirstOrDefault(u => u.UserName == userName);
            db.SaveChanges();

            return true;
        }
        public IEnumerable<Item> getbyOwner(string name)
        {
            if (name == null)
            {
                throw new BadRequestException();
            }
            var item = db.Items.Include(i => i.Owner).Where(i => i.Owner.UserName == name);
            return item.ToList();
        }

        public IEnumerable<Item> getbyBuyer(string name)
        {
            if (name == null)
            {
                throw new BadRequestException();
            }
            var item = db.Items.Include(i => i.Buyer).Where(i => i.Buyer.UserName == name);
            return item.ToList();
        }


        public void Edit(Item item)
        {
            var oldItem = db.Items.Find(item.ID);
            item.DefaultPic = oldItem.DefaultPic;
            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public void uploadPic(Item item, string path)
        {
            var it = db.Items.FirstOrDefault(i => i.ID == item.ID);

            it.DefaultPic = path;
            db.SaveChanges();
        }
    }
}