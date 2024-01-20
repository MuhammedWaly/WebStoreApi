﻿using Microsoft.EntityFrameworkCore;

namespace WebStoreApi.Models
{
    public class OrderItem
    {
        public int Id {  get; set; }

        public int ProductId {  get; set; }

        public int OrderId {  get; set; }

        [Precision(16,2)]
        public decimal UnitPrice {  get; set; }

        public int Quantity {  get; set; }

        public Order Order {  get; set; }

        public Product Product {  get; set; }
    }
}
