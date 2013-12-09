﻿
namespace WarehouseSystem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.Reflection;
    using System.Windows.Controls;
    using System.Windows;

    public class DesktopStore : Store
    {
        ~DesktopStore()
        {
            try
            {
                SaveStore();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
        }
        public override void LoadStore()
        {          
            try
            {
                //Loading products from file
                var productReader = new StreamReader("ProductList.txt");
                using (productReader)
                {
                    string productToken = productReader.ReadLine();

                    while (productToken != null)
                    {                                               
                        var splitedProductProperties = productToken.Split(new string[] { " ~obj~ " }, StringSplitOptions.RemoveEmptyEntries);
                        Type classType = Type.GetType("WarehouseSystem." + splitedProductProperties[0].Trim(), true);                        
                        var currentSettings = Activator.CreateInstance(classType);
                        splitedProductProperties = splitedProductProperties[1].Split(new string[] { " ~|~ " }, StringSplitOptions.RemoveEmptyEntries);
                        int counter = 0;
                        foreach (var productProperty in splitedProductProperties)
                        {
                            counter++;
                            var propertyInfos = productProperty.Split(new string[] { "~~" }, StringSplitOptions.RemoveEmptyEntries);
                            var property = classType.GetProperty(propertyInfos[0]);
                            var type = property.PropertyType;
                            dynamic newValue;
                            if (type.Name == "Branch")
                            {
                                newValue = Enum.Parse(typeof(Branch), propertyInfos[1]);
                            }
                            else if (type.Name == "Material")
                            {
                                newValue = Enum.Parse(typeof(Material), propertyInfos[1]);
                            }
                            else if (type.Name == "Color")
                            {
                                newValue = Enum.Parse(typeof(Color), propertyInfos[1]);
                            }
                            else if (type.Name == "Dimensions")
                            {
                                var dimensions = propertyInfos[1].Split(new string[] { "x" }, StringSplitOptions.RemoveEmptyEntries);
                                newValue = new Dimensions(double.Parse(dimensions[0]), double.Parse(dimensions[1]));
                            }
                            else
                            {
                                newValue = Convert.ChangeType(propertyInfos[1], type);
                            }
                            property.SetValue(currentSettings, newValue);                           
                        }
                        this.AddProduct(currentSettings as StoreObject);
                        productToken = productReader.ReadLine();                        
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
        }

        public override void SaveStore()
        {
            var productsWriter = new StreamWriter("ProductList.txt");
            using (productsWriter)
            {
                foreach (var product in this.productsList)
                {
                    StringBuilder productsStream = new StringBuilder();
                    productsStream.AppendFormat("{0} ~obj~ ", product.GetType().Name);
                    var classType = product.GetType();
                    bool isItFirstProp = true;
                    foreach (var property in classType.GetProperties())
                    {
                        if (isItFirstProp)
                        {
                            productsStream.AppendFormat("{0}~~{1}", property.Name, property.GetValue(product).ToString());
                            isItFirstProp = false;
                        }
                        else
                        {
                            productsStream.AppendFormat(" ~|~ {0}~~{1}", property.Name, property.GetValue(product).ToString());
                        }
                    }
                    productsWriter.WriteLine(productsStream);
                }
            }
        }

        public Dimensions ParseDimensions(string dimensions)
        {
           if (dimensions.Contains(','))
	       {
               string[] dims = dimensions.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
               return new Dimensions(double.Parse(dims[0]), double.Parse(dims[1]));
	       }
           else if (dimensions.Contains('x'))
           {
               string[] dims = dimensions.Split(new char[] { 'x' }, StringSplitOptions.RemoveEmptyEntries);
               return new Dimensions(double.Parse(dims[0]), double.Parse(dims[1]));
           }

           return new Dimensions(0, 0);
        }

        public void ExportProduct(string input, List<StoreObject> ItemContainer)
        {
            string[] elements = input.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries);
            string catNumber = elements[1];

            foreach (var item in ItemContainer)
            {
                if((item.CatalogueNumber == catNumber) && (item.Quantity > 0))
                {
                    item.Quantity--;
                    MessageBox.Show("Product exported!");
                    break;
                }
                else if((item.CatalogueNumber == catNumber) && (item.Quantity == 0))
                {
                    MessageBox.Show("Product is out of stock!");
                    break;
                }
            }
        }
        
    }
}
