using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DotNetNote.Models.RecruitManager
{
    public interface IRecruitRegistrationRepository
    {
        RecruitRegistration Add(RecruitRegistration model);         // 입력
                                                                    // 
        List<RecruitRegistration> GetAll();                         // 출력

        RecruitRegistration GetById(int id);                        // 상세

        RecruitRegistration Update(RecruitRegistration model);      // 수정

        void Remove(int id);                                        // 삭제

        List<RecruitRegistration> GetAll(string boardName, int boardNum);

        List<RecruitRegistration> GetAllByRecruitSettingId(
            int recruitSettingId);

        void RemoveRecruitRegistration(
            string boardName, int boardNum, string username);

        //[1] 이미 등록된 사용자인지 아닌지 확인
        bool IsRecruitRegisteredUser(
            string boardName, int boardNum, string username);
    }
    public class RecruitRegistrationRepository : IRecruitRegistrationRepository
    {
        // 데이터베이스 연결 문자열 가져온 후 DB 개체 생성하기
        private IConfiguration _config;
        private IDbConnection db;
        public RecruitRegistrationRepository(IConfiguration config)
        {
            _config = config;
            db = new SqlConnection(
                _config.GetSection("ConnectionStrings")
                    .GetSection("DefaultConnection").Value);
        }

        public RecruitRegistration Add(RecruitRegistration model)
        {
            throw new NotImplementedException();
        }

        public List<RecruitRegistration> GetAll()
        {
            string sql = "SELECT * FROM RecruitRegistrations ORDER BY Id Desc";
            return db.Query<RecruitRegistration>(sql).ToList();
        }

        /// <summary>
        /// 모집 등록 신청자 리스트 
        /// </summary>
        /// <param name="boardName">모집 게시판 이름</param>
        /// <param name="boardNum">모집 게시판 번호</param>
        /// <returns>모집 등록 신청자 리스트</returns>
        public List<RecruitRegistration> GetAll(string boardName, int boardNum)
        {
            string sql = @"
                SELECT *   
                FROM RecruitRegistrations 
                Where BoardName = @BoardName And BoardNum = @BoardNum";
            return db.Query<RecruitRegistration>(
                sql, new 
                {
                    BoardName = boardName,
                    BoardNum = boardNum
                }).ToList();
        }

        /// <summary>
        /// 모집 등록 신청자 리스트 by RecruitSettingId
        /// </summary>
        public List<RecruitRegistration> GetAllByRecruitSettingId(
            int recruitSettingId)
        {
            string sql = @"
                SELECT *   
                FROM RecruitRegistrations 
                Where RecruitSettingId = @RecruitSettingId";
            return db.Query<RecruitRegistration>(
                sql, new
                {
                    RecruitSettingId = recruitSettingId
                }).ToList();
        }


        public RecruitRegistration GetById(int id)
        {
            throw new NotImplementedException();
        }

        public bool IsRecruitRegisteredUser(string boardName, int boardNum, string username)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 모집 신청 등록 해제 
        /// </summary>
        public void RemoveRecruitRegistration(
            string boardName, int boardNum, string username)
        {
            var sqlDelete = @"
                Delete RecruitRegistrations 
                Where 
                    BoardName = @BoardName 
                    And 
                    BoardNum = @BoardNum 
                    And 
                    Username = @Username";
            var count = db.Execute(
                sqlDelete,
                new
                {
                    BoardName = boardName,
                    BoardNum = boardNum,
                    Username = username
                });
        }


        public RecruitRegistration Update(RecruitRegistration model)
        {
            throw new NotImplementedException();
        }
    }
}
