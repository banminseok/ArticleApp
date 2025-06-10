using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DotNetNote.Models.RecruitManager
{
    public interface IRecruitSettingRepository
    {
        RecruitSetting Add(RecruitSetting model);               // 입력 
        Task<RecruitSetting> AddAsync(RecruitSetting model);    // 입력 

        List<RecruitSetting> GetAll();                          // 출력
        Task<IEnumerable<RecruitSetting>> GetAllAsync();        // 출력

        RecruitSetting GetById(int id);                         // 상세
        Task<RecruitSetting> GetByIdAsync(int id);              // 상세

        RecruitSetting Update(RecruitSetting model);            // 수정
        Task<RecruitSetting> UpdateAsync(RecruitSetting model); // 수정

        void Remove(int id);                                    // 삭제

        bool IsRecruitSettings(string boardName, int boardNum);


        bool IsClosedRecruit(string boardName, int boardNum);

        bool IsFinishedRecruit(string boardName, int boardNum);
    }

    public class RecruitSettingRepository : IRecruitSettingRepository
    {
        // 데이터베이스 연결 문자열 가져온 후 DB 개체 생성하기 
        private IConfiguration _config;
        private IDbConnection db;
        public RecruitSettingRepository(IConfiguration config)
        {
            _config = config;
            db = new SqlConnection(
                _config.GetSection("ConnectionStrings")
                    .GetSection("DefaultConnection").Value);
        }

        #region 모집 정보 설정 기록 
        /// <summary>
        /// 모집 정보 설정 기록 
        /// </summary>
        public RecruitSetting Add(RecruitSetting model)
        {
            var sql = @"
                Insert Into RecruitSettings (
                    Remarks,
                    BoardName, 
                    BoardNum, 
                    BoardTitle, 
                    BoardContent,
                    StartDate, 
                    EventDate, 
                    EndDate, 
                    MaxCount
                ) 
                Values (
                    @Remarks,
                    @BoardName, 
                    @BoardNum, 
                    @BoardTitle, 
                    @BoardContent,
                    @StartDate, 
                    @EventDate, 
                    @EndDate, 
                    @MaxCount
                ); 

                Select Cast(SCOPE_IDENTITY() As Int);
            ";
            var id = db.Query<int>(sql, model).Single();
            model.Id = id;
            return model;
        }
        /// <summary>
        /// 모집 정보 설정 기록 
        /// </summary>
        public async Task<RecruitSetting> AddAsync(RecruitSetting model)
        {
            var sql = @"
                Insert Into RecruitSettings (
                    Remarks,
                    BoardName, 
                    BoardNum, 
                    BoardTitle, 
                    BoardContent,
                    StartDate, 
                    EventDate, 
                    EndDate, 
                    MaxCount
                ) 
                Values (
                    @Remarks,
                    @BoardName, 
                    @BoardNum, 
                    @BoardTitle, 
                    @BoardContent,
                    @StartDate, 
                    @EventDate, 
                    @EndDate, 
                    @MaxCount
                ); 

                Select Cast(SCOPE_IDENTITY() As Int);
            ";
            var id = await db.QuerySingleAsync<int>(sql, model);
            model.Id = id;
            return model;
        }
        #endregion

        #region 전체 모집 정보 출력
        /// <summary>
        /// 전체 모집 정보 출력
        /// </summary>
        public List<RecruitSetting> GetAll()
        {
            string sql = @"
                Select * 
                From RecruitSettings
                Order By Id Desc
            ";
            return db.Query<RecruitSetting>(sql).ToList();
        }

        /// <summary>
        /// 전체 모집 정보 출력
        /// </summary>
        public async Task<IEnumerable<RecruitSetting>> GetAllAsync()
        {
            string sql = @"
                Select * 
                From RecruitSettings
                Order By Id Desc
            ";
            return await db.QueryAsync<RecruitSetting>(sql);
        }
        #endregion

        public RecruitSetting GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<RecruitSetting> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public bool IsClosedRecruit(string boardName, int boardNum)
        {
            throw new NotImplementedException();
        }

        public bool IsFinishedRecruit(string boardName, int boardNum)
        {
            throw new NotImplementedException();
        }

        public bool IsRecruitSettings(string boardName, int boardNum)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public RecruitSetting Update(RecruitSetting model)
        {
            throw new NotImplementedException();
        }

        public Task<RecruitSetting> UpdateAsync(RecruitSetting model)
        {
            throw new NotImplementedException();
        }
    }
}
