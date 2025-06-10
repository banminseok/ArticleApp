using DotNetNote.Models;
using DotNetNote.Models.RecruitManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// ASP.NET Core Fundamentals 강의 - RecruitManager(모집 관리자) 강의 소스
namespace DotNetNote.Controllers.RecruitManager
{
    //[Authorize] // 로그인된 사용자만 테스트 가능 
    public class RecruitManagerController(IRecruitSettingRepository repository) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        #region 모집 추가
        [HttpGet]
        public IActionResult RecruitSettingCreate()
        {
            // View("~/Views/_MiniProjects/RecruitManager/RecruitSettingCreate.cshtml");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RecruitSettingCreate(RecruitSetting model)
        {
            // 정상적인 데이터인지 확인
            if (ModelState.IsValid)
            {
                // 실제 데이터베이스 저장
                //_repo.Add(model);
                await repository.AddAsync(model);

                return RedirectToAction(nameof(RecruitSettingList));
            }

            return View(model);
            //return View("~/Views/_MiniProjects/RecruitManager/RecruitSettingCreate.cshtml", model);
        }
        #endregion

        /// <summary>
        /// 모집 리스트
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> RecruitSettingList()
        {
            // 전체 레코드
            //var recruits = _repo.GetAll(); // 동기 방식
            var recruits = await repository.GetAllAsync(); // 비동기 방식 

            return View(recruits); 
            //return View("~/Views/_MiniProjects/RecruitManager/RecruitSettingList.cshtml", recruits);
        }

        /// <summary>
        /// 모집 상세
        /// </summary>
        public async Task<IActionResult> RecruitSettingDetails(int id)
        {
            ViewData["Id"] = id.ToString();

            //var recruit = _repo.GetById(id); 
            var recruit = await repository.GetByIdAsync(id);

            return View(recruit);
            //return View("~/Views/_MiniProjects/RecruitManager/RecruitSettingDetail.cshtml", recruit);
        }
    }
}
