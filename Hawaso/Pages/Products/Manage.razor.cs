﻿using ArticleApp.Models;
using ArticleApp.Models.Products;
using BmsPager;
using Dul.Articles;
using Hawaso.Pages.Customers.Components;
using Hawaso.Pages.Products.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Drawing;
using System.Threading.Tasks;
using UploadApp.Shared;


namespace Hawaso.Pages.Products
{
    public partial class Manage
    {
        [Inject]
        public IProductRepositoryAsync ProductRepositoryAsync { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private BmsPagerBase pager = new BmsPagerBase()
        {
            PageNumber = 1,
            PageIndex = 0,
            PageSize = 3,
            PagerButtonCount = 5
        };

        private List<Product> Products;

        public string EditorFormTitle { get; set; } = "ADD";

        public Product Product { get; set; } = new Product();

        public ProductEditorForm ProductEditorForm { get; set; }

        public ProductDeleteDialog ProductDeleteDialog { get; set; }

        public bool IsInlineDialogShow { get; set; }

        protected override async Task OnInitializedAsync() => await DisplayData();

        private async Task DisplayData()
        {
            //await Task.Delay(3000);
            var articleSet = await ProductRepositoryAsync.GetAllAsync(pager.PageIndex, pager.PageSize);
            pager.RecordCount = articleSet.TotalRecords;
            Products = articleSet.Records.ToList();
        }

        private async void PageIndexChanged(int pageIndex)
        {
            pager.PageIndex = pageIndex;
            pager.PageNumber = pageIndex + 1;

            await DisplayData();

            StateHasChanged();
        }

        private void btnProductName_Click(int ProductId) => NavigationManager.NavigateTo($"/Products/Details/{ProductId}");

        protected void ShowEditorForm()
        {
            EditorFormTitle = "ADD";
            Product = new Product();

            ProductEditorForm.Show();
        }

        protected async void SaveOrUpdated()
        {
            ProductEditorForm.Close();

            await DisplayData();

            StateHasChanged();
        }

        protected void EditBy(Product Product)
        {
            EditorFormTitle = "EDIT";
            this.Product = Product;

            ProductEditorForm.Show();
        }

        protected void DeleteBy(Product Product)
        {
            this.Product = Product;
            ProductDeleteDialog.Show();
        }

        protected async void btnDelete_Click()
        {
            await ProductRepositoryAsync.DeleteAsync(Product.ProductId);

            ProductDeleteDialog.Close();

            Product = new Product();

            await DisplayData();

            StateHasChanged();
        }

        protected void ToggleBy(Product product)
        {
            this.Product = product;
            IsInlineDialogShow = true;
        }

        protected async void btnToggleAbsence_Click()
        {
            Product.Absence = (Product?.Absence == 0) ? 1 : 0; // 토글

            await ProductRepositoryAsync.EditAsync(Product);

            await DisplayData();

            IsInlineDialogShow = false;
            StateHasChanged();
        }

        protected void btnClose_Click()
        {
            IsInlineDialogShow = false;
            Product = new Product();
        }
    }
}
