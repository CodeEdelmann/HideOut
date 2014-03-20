using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hide_Out.Entities;

namespace Hide_Out.Controllers
{
    class ItemController
    {

        public List<Item> activeItems { get; set; }

        public ItemController()
        {
            activeItems = new List<Item>();
        }

        public void addItem(Item item)
        {
            activeItems.Add(item);
        }

        public void removeItem(Item item)
        {
            activeItems.Remove(item);
        }

        public void clearItems()
        {
            activeItems.Clear();
        }

        public void updateItems(int CurrentTime)
        {
            foreach (Item item in this.activeItems){
                item.updateTime(CurrentTime);
                if (item.expirationTime >= item.time)
                {
                    activeItems.Remove(item);
                }
            }
        }

        public void pickUp(Item item)
        {
            activeItems.Remove(item);
        }
    }
}
