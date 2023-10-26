﻿using QuizMaster.API.Quiz.SeedData;
using QuizMaster.API.Quiz.Services.Repositories;
using QuizMaster.Library.Common.Entities.Questionnaire;

namespace QuizMaster.API.Quiz.Tests.Services
{

	public class QuizTestDataRepository : IQuizRepository
	{
		private readonly IEnumerable<QuestionCategory> _categories;
		private readonly IEnumerable<QuestionType> _types;
		private readonly IEnumerable<QuestionDifficulty> _difficulties;

		public QuizTestDataRepository()
		{
			_categories = QuestionCategories.Categories;
			_types = QuestionTypes.Types;
			_difficulties = QuestionDifficulties.Difficulties;
		}

		public async Task<bool> AddCategoryAsync(QuestionCategory category)
		{
			try
			{
				await Task.FromResult(() => _categories.Append(category));
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return false;
			}
		}

		public async Task<bool> AddDifficultyAsync(QuestionDifficulty difficulty)
		{
			try
			{
				await Task.FromResult(() => _difficulties.Append(difficulty));
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return false;
			}
		}

		public async Task<bool> AddTypeAsync(QuestionType type)
		{
			try
			{
				await Task.FromResult(() => _types.Append(type));
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return false;
			}
		}

		public async Task<IEnumerable<QuestionCategory>> GetAllCategoriesAsync()
		{
			return await Task.FromResult(_categories);
		}

		public async Task<IEnumerable<QuestionDifficulty>> GetAllDifficultiesAsync()
		{
			return await Task.FromResult(_difficulties);
		}

		public Task<IEnumerable<QuestionType>> GetAllTypesAsync()
		{
			throw new NotImplementedException();
		}

		public Task<QuestionCategory?> GetCategoryAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<QuestionCategory?> GetCategoryAsync(string description)
		{
			throw new NotImplementedException();
		}

		public Task<QuestionDifficulty?> GetDifficultyAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<QuestionDifficulty?> GetDifficultyAsync(string description)
		{
			throw new NotImplementedException();
		}

		public Task<QuestionType?> GetTypeAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<QuestionType?> GetTypeAsync(string description)
		{
			throw new NotImplementedException();
		}

		public Task<bool> SaveChangesAsync()
		{
			throw new NotImplementedException();
		}

		public bool UpdateCategory(QuestionCategory category)
		{
			throw new NotImplementedException();
		}

		public bool UpdateDifficulty(QuestionDifficulty difficulty)
		{
			throw new NotImplementedException();
		}

		public bool UpdateType(QuestionType type)
		{
			throw new NotImplementedException();
		}
	}
}