﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Reflection;

namespace WarehouseSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 


    public partial class MainWindow : Window
    {
        public static List<StoreObject> ItemContainer = new List<StoreObject>();
        public static List<UIElement> PropertyContents = new List<UIElement>();
        public static DesktopStore desktopStore = new DesktopStore();

        public MainWindow()
        {
            InitializeComponent();
            //DesktopStore desktopStore = new DesktopStore();
            this.DataContext = desktopStore;
            ItemContainer = desktopStore.GetAllProducts();

            LoadCategoryTabs();
            LoadCategories(desktopStore);
        }

        private void LoadCategories(DesktopStore desktopStore)
        {
            this.productCategories.ItemsSource = desktopStore.GetCategories();
        }

        private void LoadCategoryTabs()
        {
            var existingCategories = ItemContainer.Select(x => x.Category.ToString()).Distinct().ToList<string>();
            CreateInnerTabsWithContent(existingCategories);
            //List<string> categories = null;
            //foreach (var item in MainWindow.ItemContainer)
            //{
            //    if (categories == null)
            //    {
            //        categories = new List<string>();
            //    }

            //    if (!categories.Contains(item.Category.ToString()))
            //    {
            //        categories.Add(item.Category.ToString());
            //    }
            //}

            //CreateInnerTabsWithContent(categories);
        }

        private void CreateInnerTabsWithContent(List<string> categories)
        {
            foreach (var category in categories)
            {
                TabItem currentItem = new TabItem();
                currentItem.Header = category;
                var categoryStackPannel = new StackPanel();
                var categoryFilter =
                    from x in ItemContainer
                    where x.Category.ToString() == category.ToString()
                    select x;

                foreach (var item in categoryFilter)
                {
                    categoryStackPannel.Children.Add(new Label() { Content = item.ToString() });
                }
                currentItem.Content = categoryStackPannel;
                this.CategorySubContainer.Items.Add(currentItem);


                //Button currentButton = new Button();
                //currentButton.Content = category;
                //currentButton.Margin = new Thickness(3);
                //currentButton.Width = 100;
                //this.CategoryStackPanel.Children.Add(currentButton);
            }
        }

        private void ChangeValue(object sender, SelectionChangedEventArgs e)
        {
            switch (this.productCategories.SelectedIndex)
            {
                case 1: ElectronicObject eo = new ElectronicObject();
                    GenerateInputFields(eo);
                    this.AddTabChildStack.Background = Brushes.LightBlue;
                    break;
                case 2: ConstructionObject co = new ConstructionObject();
                    GenerateInputFields(co);
                    this.AddTabChildStack.Background = Brushes.LightCyan;
                    break;
                case 3: GardenObject go = new GardenObject();
                    GenerateInputFields(go);
                    this.AddTabChildStack.Background = Brushes.LightGreen;
                    break;
                case 4: SanitaryObject so = new SanitaryObject();
                    GenerateInputFields(so);
                    this.AddTabChildStack.Background = Brushes.LightCoral;
                    break;
                case 5: ToolObject to = new ToolObject();
                    GenerateInputFields(to);
                    this.AddTabChildStack.Background = Brushes.LightGoldenrodYellow;
                    break;
                case 6: MachineryObject mo = new MachineryObject();
                    GenerateInputFields(mo);
                    this.AddTabChildStack.Background = Brushes.LightSalmon;
                    break;
                case 7: AutoPartObject ao = new AutoPartObject();
                    GenerateInputFields(ao);
                    this.AddTabChildStack.Background = Brushes.LightGray;
                    break;
                default: AddTabChildStack.Children.RemoveRange(1, AddTabChildStack.Children.Count - 1);
                    AddTabChildStack.Background = Brushes.Transparent;
                    AddButton.IsEnabled = false;
                    break;
            }
        }

        private void GenerateInputFields(StoreObject obj)
        {
            PropertyContents.Clear();
            AddTabChildStack.Children.RemoveRange(1, AddTabChildStack.Children.Count - 1);
            var list = obj.GetType().GetProperties();
            AddButton.IsEnabled = true;

            foreach (var item in list)
            {
                this.AddTabChildStack.Children.Add(new Label { Content = item.Name + ":" });
                if (item.PropertyType.Name == "Branch")
                {
                    TextBox box = new TextBox
                    {
                        Text = this.productCategories.SelectedItem.ToString(),
                        Width = 150,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(10, 0, 0, 3)
                    };
                    this.AddTabChildStack.Children.Add(box);
                    PropertyContents.Add(box);
                }
                else if (item.PropertyType.Name == "Color")
                {
                    var colors = new List<string>();

                    foreach (var color in Enum.GetValues(typeof(Color)))
                    {
                        colors.Add(color.ToString());
                    }
                    ComboBox comboBox = new ComboBox
                    {
                        ItemsSource = colors,
                        Width = 150,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(10, 0, 0, 3)
                    };
                    this.AddTabChildStack.Children.Add(comboBox);
                    PropertyContents.Add(comboBox);
                }
                else if (item.PropertyType.Name == "Material")
                {
                    var materials = new List<string>();

                    foreach (var material in Enum.GetValues(typeof(Material)))
                    {
                        materials.Add(material.ToString());
                    }
                    ComboBox comboBox = new ComboBox
                    {
                        ItemsSource = materials,
                        Width = 150,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(10, 0, 0, 3)
                    };
                    this.AddTabChildStack.Children.Add(comboBox);
                    PropertyContents.Add(comboBox);
                }
                else
                {
                    TextBox box = new TextBox
                    {
                        MaxLength = 20,
                        Width = 150,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(10, 0, 0, 3)
                    };
                    this.AddTabChildStack.Children.Add(box);
                    PropertyContents.Add(box);
                }
            }
        }

        private void AddProduct(object sender, RoutedEventArgs e)
        {
            //bool addProduct = true;
            //var enumValue = Enum.Parse(typeof(Branch), productCategories.SelectedValue.ToString(), true);
            //var getClassNameFromBranch = ((BranchToClassName)((int)enumValue)).ToString();
            ////System.Windows.MessageBox.Show("WarehouseSystem." + getClassNameFromBranch);
            //var product = Activator.CreateInstance(Type.GetType("WarehouseSystem." + getClassNameFromBranch, true));
            //var list = product.GetType().GetProperties();
            //System.Windows.MessageBox.Show(list[0].ToString());
            //int propertyIndex = 0;

            //foreach (var property in PropertyContents)
            //{
            //    var controlType = property.GetType();

            //    if (controlType.Name == "TextBox")
            //    {
            //        //System.Windows.MessageBox.Show(property.GetType().GetProperty("Text").GetValue(property).ToString());
            //        //System.Windows.MessageBox.Show(controlType.GetProperty("Text").ToString());
            //        if (controlType.GetProperty("Text") != null)
            //        {
            //            var controlValue = property.GetType().GetProperty("Text").GetValue(property).ToString();
            //            var propertyType = list[propertyIndex].PropertyType;
            //            dynamic parsedValue;
            //            if (propertyType.Name == "Branch")
            //            {
            //                parsedValue = (Branch)int.Parse(controlValue);
            //            }
            //            else if(propertyType.Name == "Dimensions")
            //            {
            //                parsedValue = new Dimensions();
            //            }
            //            else
            //            {
            //                parsedValue = Convert.ChangeType(controlValue, propertyType);
            //            }
            //            list[propertyIndex].SetValue(product, parsedValue);
            //        }
            //        else
            //        {
            //            addProduct = false;
            //        }
            //    }
            //    else if (controlType.Name == "ComboBox")
            //    {
            //        ////System.Windows.MessageBox.Show(property.GetType().GetProperty("SelectedValue").GetValue(property).ToString());
            //        //System.Windows.MessageBox.Show(controlType.GetProperty("SelectedValue").ToString());
            //        //if (controlType.GetProperty("SelectedValue") != null)
            //        //{
            //        //    var controlValue = property.GetType().GetProperty("SelectedValue").GetValue(property).ToString();
            //        //    ((PropertyInfo)list[propertyIndex]).SetValue(product, controlValue);
            //        //}
            //        //else
            //        //{
            //        //    addProduct = false;
            //        //}
            //    }

            //    if (addProduct != true)
            //    {
            //        System.Windows.MessageBox.Show(propertyIndex.ToString());
            //        break;
            //    }
            //    propertyIndex++;
            //}

            //if (addProduct)
            //{
            //    desktopStore.AddProduct(product as StoreObject);
            //    ItemContainer = desktopStore.GetAllProducts();
            //}
        }
    }
}