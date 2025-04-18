﻿using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;

namespace ArticleAppBlazorServer.Pages.Uploads.Components
{
    public partial class DeleteDialog
    {
        #region Parameters
        /// <summary>
        /// 부모에서 OnClick 속성에 지정한 이벤트 처리기 실행
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }
        #endregion


        #region Properties
        /// <summary>
        /// 모달 다이얼로그를 표시할건지 여부 
        /// </summary>
        public bool IsShow { get; set; } = false;
        #endregion

        #region Public Methods
        /// <summary>
        /// 폼 보이기 
        /// </summary>
        public void Show() => IsShow = true;

        /// <summary>
        /// 폼 닫기
        /// </summary>
        public void Hide()
        {
            IsShow = false;
        }
        #endregion

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
    }
}
