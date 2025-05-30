﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmsPager
{
    /// <summary>
    /// 페이징 처리를 위한 공통 기본 클래스: DulPagerBase
    /// https://github.com/VisualAcademy/DulPager
    /// </summary>
    public class BmsPagerBase
    {
        /// <summary>
        /// 페이저가 사용되는 페이지
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 총 몇 개의 페이지가 만들어지는지: Math.Ceiling(총 레코드 수 / 10(한 페이지에서 보여줄))
        /// </summary>
        public int PageCount { get; set; } = 5;

        /// <summary>
        /// 레코드 카운트: 현재 테이블에 몇 개의 레코드가 있는지 지정
        /// </summary>
        public int RecordCount { get; set; } = 50;

        /// <summary>
        /// 페이지 사이즈: 한 페이지에 몇 개의 레코드를 보여줄건지 결정 
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// 페이지 인덱스: 현재 보여줄 페이지 번호의 인덱스(PageNumber - 1)
        /// </summary>
        public int PageIndex { get; set; } = 0;

        /// <summary>
        /// 페이지 번호: 현재 보여줄 페이지 번호: 1 페이지, 2 페이지, ... 
        /// </summary>
        public int PageNumber { get; set; } = 1; // 1로 초기화 

        /// <summary>
        /// 페이저에 몇 개씩 페이지 버튼을 표시할지
        /// </summary>
        public int PagerButtonCount { get; set; } = 3;

        #region 검색 리스트 관련 속성들
        /// <summary>
        /// 기본 리스트면 false, 검색 결과에 대한 페이징 리스트면 true
        /// </summary>
        public bool SearchMode { get; set; } = false;

        /// <summary>
        /// 검색할 필드: Name, Title, Content
        /// </summary>
        public string SearchField { get; set; } = "";

        /// <summary>
        /// 검색할 내용
        /// </summary>
        public string SearchQuery { get; set; } = "";
        #endregion
    }
}
