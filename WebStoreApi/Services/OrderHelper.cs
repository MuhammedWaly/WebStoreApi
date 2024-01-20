namespace WebStoreApi.Services
{
    public class OrderHelper
    {
        public static decimal ShoppingFee { get; } = 5;

        public static Dictionary<string, string> PaymentMethods { get; } = new()
        {
            {"Cash","Cash on Delivery" },
            {"Paypal","Paypal" },
            {"Credit Card","Credit Card" },
        };
        
        public static List<string> PaymentStatues { get; } = new()
        {
            "Prnding","Accepted","Canceled"
        };
        
        public static List<string> OrderStatues { get; } = new()
        {
            "Shipped","Created","Accepted","Canceled","Returned","Delivered"
        };

        public static Dictionary<int, int> GetProductDictionary(string ProductIdentifires)
        {
            var ProductDictaionary = new Dictionary<int, int>();
            if(ProductIdentifires.Length >  0) 
            {
                string[] PoductIdArray = ProductIdentifires.Split('-');
                foreach (var ProductId in PoductIdArray)
                {
                    try
                    {
                        int id = int.Parse(ProductId);

                        if(ProductDictaionary.ContainsKey(id)) 
                        {
                            ProductDictaionary[id] += 1;
                        }
                        else
                        {
                            ProductDictaionary.Add(id, 1);
                        }
                    }
                    catch (Exception) { }
                    
                }
            }

            return ProductDictaionary;
        }
    }
}
