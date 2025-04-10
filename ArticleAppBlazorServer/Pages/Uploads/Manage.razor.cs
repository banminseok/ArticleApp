﻿using ArticleApp.Models;
using ArticleAppBlazorServer.Managers;
using BlazorUtils;
using BmsPager;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using UploadApp.Shared;


namespace ArticleAppBlazorServer.Pages.Uploads
{
    public partial class Manage
    {
        [Inject]
        public ILogger<Index> LoggerReference { get; set; }

        [Inject]
        public IUploadRepository UploadRepositoryReference{ get; set; }

        [Inject]
        public NavigationManager NavigationManagerReference { get; set; }

        [Inject]
        public IFileStorageManager FileStorageManagerReference { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Parameter]
        public int ParentId { get; set; } = 0;

        [Parameter]
        [SupplyParameterFromQuery]
        public string? ParentKey { get; set; } = "";

        public ArticleAppBlazorServer.Pages.Uploads.Components.EditorForm EditFormReference { get; set; }
        public ArticleAppBlazorServer.Pages.Uploads.Components.DeleteDialog DeleteDialogReference { get; set; }

        protected List<Upload> models;
        protected Upload model = new Upload();

        //입력폼 제목
        public string EditorFormTitle { get; set; } = "Create";

        /// <summary>
        /// 공지사항으로 올리기 폼을 띄울건지 여부 
        /// </summary>
        public bool IsInlineDialogShow { get; set; } = false;

        // 페이저 객체 생성
        private BmsPagerBase pager = new BmsPagerBase()
        {
            PageNumber = 1,
            PageIndex = 0,
            PageSize = 3,
            PagerButtonCount = 5
        };


        protected override async Task OnInitializedAsync()
        {
            await DisplayData();
        }

        /// <summary>
        /// NoticeApp - UploadApp - ReplyApp을 거쳐 여기 코드가 더 정리됩니다.
        /// </summary>
        /// <returns></returns>
        private async Task DisplayData()
        {
            
            try
            {
                if(ParentKey!="" && !string.IsNullOrEmpty(ParentKey))
                {
                    var resultsSet = await UploadRepositoryReference.GetAllByParentKeyAsync(pager.PageIndex, pager.PageSize, ParentKey);
                    pager.RecordCount = resultsSet.TotalRecords;
                    models = resultsSet.Records.ToList();
                    LoggerReference.LogInformation($"※※※ ParentKey!=\"\"");

                }
                else if (ParentId != 0)
                {
                    var resultsSet = await UploadRepositoryReference.GetAllByParentIdAsync(pager.PageIndex, pager.PageSize, ParentId);
                    pager.RecordCount = resultsSet.TotalRecords;
                    models = resultsSet.Records.ToList();
                    LoggerReference.LogInformation($"※※※ ParentId != 0");

                }
                else
                {
                    var resultsSet = await UploadRepositoryReference.GetAllAsync(pager.PageIndex, pager.PageSize);
                    pager.RecordCount = resultsSet.TotalRecords;
                    models = resultsSet.Records.ToList();
                    LoggerReference.LogInformation($"※※※ else");

                }
                LoggerReference.LogInformation($"※※※ [2] ParentId: {ParentId} , ParentKey: {ParentKey} , 레코드 수 {pager.RecordCount} , {pager.PageNumber}페이지");
                StateHasChanged(); //현재 페이지를 다시 그림
                /*
                if (ParentId == 0)
                {
                    await Task.Delay(500);
                    var resultsSet = await UploadRepositoryReference.GetAllAsync(pager.PageIndex, pager.PageSize);
                    pager.RecordCount = resultsSet.TotalRecords;
                    models = resultsSet.Records.ToList();
                }
                else
                {
                    var resultsSet = await UploadRepositoryReference.GetAllByParentIdAsync(pager.PageIndex, pager.PageSize, ParentId);
                    pager.RecordCount = resultsSet.TotalRecords;
                    models = resultsSet.Records.ToList();
                }*/
            }
            catch (Exception e)
            {
                LoggerReference.LogInformation($"※※※Error ({nameof(DisplayData)}):{e.Message}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task SearchData()
        {
            try
            {
                if (ParentKey != "" && !string.IsNullOrEmpty(ParentKey))
                {
                    var resultsSet = await UploadRepositoryReference.SearchAllAsync(pager.PageIndex, pager.PageSize, this.searchQuery);
                    pager.RecordCount = resultsSet.TotalRecords;
                    models = resultsSet.Records.ToList();
                }
                else if (ParentId != 0)
                {
                    var resultsSet = await UploadRepositoryReference.SearchAllByParentIdAsync(pager.PageIndex, pager.PageSize, this.searchQuery, this.ParentId);
                    pager.RecordCount = resultsSet.TotalRecords;
                    models = resultsSet.Records.ToList();
                }
                else
                {
                    var resultsSet = await UploadRepositoryReference.SearchAllAsync(pager.PageIndex, pager.PageSize, this.searchQuery);
                    pager.RecordCount = resultsSet.TotalRecords;
                    models = resultsSet.Records.ToList();
                }
                /*    if (ParentId == 0)
                {
                    var resultsSet = await UploadRepositoryReference.SearchAllAsync(pager.PageIndex, pager.PageSize, this.searchQuery);
                    pager.RecordCount = resultsSet.TotalRecords;
                    models = resultsSet.Records.ToList();
                }
                else
                {
                    var resultsSet = await UploadRepositoryReference.SearchAllByParentIdAsync(pager.PageIndex, pager.PageSize, this.searchQuery, this.ParentId);
                    pager.RecordCount = resultsSet.TotalRecords;
                    models = resultsSet.Records.ToList();
                }*/
                StateHasChanged(); //현재 페이지를 다시 그림
                LoggerReference.LogInformation($"※※※ [2] ParentId: {ParentId} , 레코드 수 {pager.RecordCount} , {pager.PageNumber}페이지");
            }
            catch (Exception e)
            {
                LoggerReference.LogInformation($"※※※Error ({nameof(SearchData)}):{e.Message}");
            }
        }

        protected void NameClick(int id)
        {
            LoggerReference.LogInformation($"NameClick: {id}");
            NavigationManagerReference.NavigateTo($"/Uploads/Details/{id}");
        }


        /// <summary>
        /// 페이지 버튼 클릭 이벤트 BmsPagerComponent에서 호출해 주는 이벤트
        /// </summary>
        /// <param name="pageIndex"></param>
        protected async void PageIndexChangedA(int pageIndex)
        {
            pager.PageIndex = pageIndex;
            pager.PageNumber = pageIndex + 1;
            if (this.searchQuery == "")
            {
                await DisplayData();
            }
            else
            {
                await SearchData();
            }


        }

        protected void ShowEditorForm()
        {
            EditorFormTitle = "CREATE";
            this.model = new Upload();
            this.model.ParentKey = ParentKey; // 
            EditFormReference.Show();
        }
        protected void EditBy(Upload model)
        {
            EditorFormTitle = "EDIT";
            this.model = new Upload();
            this.model = model;
            this.model.ParentKey = ParentKey; // 
            EditFormReference.Show();
        }
        protected void DeleteBy(Upload model)
        {
            LoggerReference.LogInformation($"DeleteBy: {model.Id}");
            //삭제 처리 다이얼 로그
            this.model = model;
            DeleteDialogReference.Show();

        }

        protected void ToggleBy(Upload modal)
        {
            this.model = modal; 
            IsInlineDialogShow = true;
        }
        protected async void DownloadBy(Upload model)
        {
            if(!string.IsNullOrEmpty(model.FileName))
            {
                byte[] fileBytes = await FileStorageManagerReference.DownloadAsync(model.FileName, "");
                if (fileBytes != null)
                {
                    // DownCount
                    // model.DownloadCount++;
                    //await UploadRepositoryReference.EditAsync(model);

                    await FileUtil.SaveAs(JSRuntime, model.FileName, fileBytes);
                }
                else
                {
                    LoggerReference.LogInformation($"DownloadBy: {model.Id} - FileBytes is null");
                }
            }
            else
            {
                LoggerReference.LogInformation($"DownloadBy: {model.Id} - FilePath is null");
            }
        }

        protected void ToggleClose()
        {
            IsInlineDialogShow = false;
            this.model = new Upload();
        }
        protected async void ToggleClick()
        {
            this.model.IsPinned = !this.model.IsPinned;
            await UploadRepositoryReference.EditAsync(this.model);
            IsInlineDialogShow = false;
            this.model = new Upload();
            await DisplayData();
        }

        /// <summary>
        /// EditorForm에서 CreateCallback 이벤트가 발생하면 호출되는 메서드
        /// </summary>
        protected async void CreatOrEdit()
        {
            EditFormReference.Hide();
            await DisplayData();
            StateHasChanged();
        }
        /// <summary>
        /// DeleteDialog에서 DeleteCallback 이벤트가 발생하면 호출되는 메서드
        /// </summary>
        protected async void DeleteClick()
        {
            if(!string.IsNullOrEmpty(this.model?.FileName))
            {
                // 첨부 파일 삭제 
                await FileStorageManagerReference.DeleteAsync(this.model.FileName, "");
            }
            await UploadRepositoryReference.DeleteAsync(this.model.Id);
            DeleteDialogReference.Hide();
            this.model = new Upload();
            await DisplayData();
        }

        private string searchQuery = "";

        protected async void Search(string query)
        {
            this.searchQuery = query;

            await SearchData();

            StateHasChanged();
        }

    }
}
