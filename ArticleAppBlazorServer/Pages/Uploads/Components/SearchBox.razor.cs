using Microsoft.AspNetCore.Components;
using System.Timers;

namespace ArticleAppBlazorServer.Pages.Uploads.Components
{
    public partial class SearchBox: IDisposable
    {
        #region Fields
        private string searchQuery;
        private System.Timers.Timer debounceTimer; 
        #endregion

        #region Properties
        public string SearchQuery
        {
            get => searchQuery;
            set
            {
                searchQuery = value;
                debounceTimer.Stop(); // 텍스트박스에 값을 입력하는 동안 타이머 중지
                debounceTimer.Start(); // 타이머 실행(300밀리초 후에 딱 한 번 실행)
            }
        }
        #endregion

        #region Parameters
        [Parameter(CaptureUnmatchedValues = true)]
        public IDictionary<string, object> AdditionalAttributes { get; set; }

        // 자식 컴포넌트에서 발생한 정보를 부모 컴포넌트에게 전달
        [Parameter]
        public EventCallback<string> SearchQueryChanged { get; set; }

        [Parameter]
        public int Debounce { get; set; } = 300;

        #endregion


        #region Lifecycle Methods
        protected override void OnInitialized()
        {
            debounceTimer = new System.Timers.Timer();  //•	새로운 Timer 객체를 생성합니다. 이 타이머는 일정 시간 간격으로 이벤트를 발생시킬 수 있습니다.
            debounceTimer.Interval = Debounce;  //•	타이머의 간격을 설정합니다. 여기서 Debounce는 밀리초 단위로 설정된 값으로, 기본값은 300밀리초입니다. 즉, 타이머는 300밀리초 간격으로 이벤트를 발생시킵니다.
            debounceTimer.AutoReset = false;  //•	AutoReset 속성을 false로 설정합니다. 이 설정은 타이머가 한 번만 실행되도록 합니다. 타이머가 한 번 실행된 후에는 자동으로 다시 시작되지 않습니다.
            debounceTimer.Elapsed += SearchHandler;  //•	Elapsed 이벤트에 SearchHandler 메서드를 연결합니다.타이머가 설정된 간격(여기서는 300밀리초) 후에 Elapsed 이벤트가 발생하면 SearchHandler 메서드가 호출됩니다.
        }
        #endregion

        #region Event Handlers
        protected void Search()
        {
            SearchQueryChanged.InvokeAsync(SearchQuery); // 부모의 메서드에 검색어 전달
        }

        protected async void SearchHandler(object source, ElapsedEventArgs e)
        {
            await InvokeAsync(() => SearchQueryChanged.InvokeAsync(SearchQuery)); // 부모의 메서드에 검색어 전달
        }
        #endregion

        #region Public Methods
        public void Dispose()
        {
            debounceTimer.Dispose();
        }
        #endregion
    }
}
