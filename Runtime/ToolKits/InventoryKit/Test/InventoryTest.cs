using NUnit.Framework;

namespace Lost
{
    public class InventoryTest
    {
        [Test]
        public void Test()
        {
            Assert.IsTrue(true);
        }

        [Test] 
        public void TestAddItem()
        {
            Inventory inventory = new Inventory("test","test",true,100);
            inventory.AddItem(new InventoryItem("test",10), 1);
            Assert.IsTrue(inventory.GetQuantity("test") == 1);
            Assert.IsTrue(inventory.InventoryContains("test").Count == 1);
            Assert.IsTrue(inventory.InventoryContains("test")[0]==0,
                $"inventory contains test at {inventory.InventoryContains("test")[0]}");
            
            inventory.RemoveItem(0, 1);
            Assert.IsTrue(inventory.GetQuantity("test") == 0);
            Assert.IsTrue(inventory.InventoryContains("test").Count == 0);
        }
    }
}