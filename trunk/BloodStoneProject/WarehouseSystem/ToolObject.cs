﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarehouseSystem
{
    public class ToolObject : StoreObject, IMaterial
    {
        private Material material;

        public ToolObject()
        {

        }

        public ToolObject(string catalogueNumber, string manufacturer, string model, string description, int quantity, Branch category, decimal price, Material material)
            : base(catalogueNumber, manufacturer, model, description, quantity,category, price)
        {
            this.material = material;            
        }

        public Material Material
        {
            get
            {
                return this.material;
            }
            set
            {
                this.material = value;
            }
        }
    }
}