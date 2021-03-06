﻿using OnlineTestingSystem.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineTestingSystem.BLL.ModelsDTO;
using OnlineTestingSystem.DAL.Interfaces;
using AutoMapper;
using OnlineTestingSystem.DAL.Entities;
using OnlineTestingSystem.BLL.Infrastructure;

namespace OnlineTestingSystem.BLL.Services
{
    public class TestService : ITestService
    {
        IUnitOfWorkTest db;
        IMapper _mapper;

        public TestService(IUnitOfWorkTest uow)
        {
            db = uow;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Test, TestDTO>()
                .ForMember(bll => bll.QuestionCagegoryDTO, dal => dal.MapFrom(b => b.QuestionCagegory))
                .ForMember(bll => bll.TestSessionsDTO, dal => dal.MapFrom(b => b.TestSessions));
                cfg.CreateMap<TestDTO, Test>()
                .ForMember(dal => dal.QuestionCagegory, bll => bll.MapFrom(b => b.QuestionCagegoryDTO))
                .ForMember(dal => dal.TestSessions, bll => bll.MapFrom(b => b.TestSessionsDTO));
                cfg.CreateMap<QuestionDTO, Question>()
                .ForMember(dal => dal.QuestionCategory, bll => bll.MapFrom(b => b.QuestionCategoryDTO))
                .ForMember(dal => dal.QuestionAnswers, bll => bll.MapFrom(b => b.QuestionAnswersDTO));
                cfg.CreateMap<Question, QuestionDTO>()
                .ForMember(bll => bll.QuestionCategoryDTO, dal => dal.MapFrom(b => b.QuestionCategory))
                .ForMember(bll => bll.QuestionAnswersDTO, dal => dal.MapFrom(b => b.QuestionAnswers));
                cfg.CreateMap<QuestionCategory, QuestionCategoryDTO>();
                cfg.CreateMap<QuestionCategoryDTO, QuestionCategory>();
                cfg.CreateMap<TestSession, TestSessionDTO>();
                cfg.CreateMap<TestSessionDTO, TestSession>();
                cfg.CreateMap<QuestionAnswer, QuestionAnswerDTO>();
                
                
            });
            _mapper = config.CreateMapper();

        }

        public void CreateTest(TestDTO test)
        {
            var testToAdd = _mapper.Map<TestDTO, Test>(test);
            db.Tests.Create(testToAdd);
            db.Save();
        }

        public void DeleteTest(int id)
        {
            var testToDelete = GetTestById(id);
            if (testToDelete == null)
                throw new ValidationException("Sorry, but the test doesn't exsist.", "");
            db.Tests.Delete(id);
            db.Save();
        }

        public IEnumerable<TestDTO> GetAllTests()
        {
            var tests = db.Tests.GetAll();
            return _mapper.Map<IEnumerable<Test>, IEnumerable<TestDTO>>(tests);
        }

        public IEnumerable<QuestionAnswerDTO> GetTestAnswers(int testId)
        {
            var test = GetTestById(testId);
            var questions = GetTestQuestions(testId);
            var answers = new List<QuestionAnswerDTO>();
            foreach (var question in questions)
            {
                foreach (var answer in question.QuestionAnswersDTO)
                {
                    answers.Add(answer);
                }
            }
            return answers;
        }

        public TestDTO GetTestById(int id)
        {
            var test = db.Tests.Get(id);
            return _mapper.Map<Test, TestDTO>(test);
        }

        public IEnumerable<QuestionDTO> GetTestQuestions(int testId)
        {
            var test = GetTestById(testId);
            var questions = db.Questions.Find(q => q.QuestionCategoryId == test.QuestionCategoryId);
            return _mapper.Map<IEnumerable<Question>, IEnumerable<QuestionDTO>>(questions);
        }

        public int GetAmountOfQuestions(int testId)
        {
            var test = db.Tests.Get(testId);
            var categoryId = test.QuestionCategoryId;
            return db.Questions.Find(q => q.QuestionCategoryId == categoryId).Count();
        }

        public void UpdateTest(TestDTO test)
        {
            var testDAL = _mapper.Map<TestDTO, Test>(test);
            db.Tests.Update(testDAL);
            db.Save();
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public IEnumerable<TestDTO> GetNTests(int amountToTake, int amountToSkip)
        {
            var tests = db.Tests.GetAll().Skip(amountToSkip).Take(amountToTake);
            return _mapper.Map<IEnumerable<Test>, IEnumerable<TestDTO>>(tests);
        }
    }
}
