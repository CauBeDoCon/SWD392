using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs;
using SWD392.Enums;

namespace SWD392.Repositories
{
    public class RoutineStepRepository :IRoutineStepRepository
    {
        public readonly ApplicationDbContext _context;
        public readonly ICategoryRepository _categoryRepository;
        public readonly IProductRepository _productRepository;
        public RoutineStepRepository(ApplicationDbContext context, ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            _context = context;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }
        public async Task<int> AddRoutineStepAsync(RoutineStepDto dto, int routineId)
        {
            var newRoutineStep = new RoutineStep{
                RoutineId = routineId,
                Step = dto.step,
                CategoryId = dto.CategoryId 
            };
            await _context.routineSteps.AddAsync(newRoutineStep);
            await _context.SaveChangesAsync();
            return newRoutineStep.Id;
        }
        public async Task<List<RoutineStep>> GetRoutineStepByRouteID(int id){
            return await _context.routineSteps.Where(rs => rs.RoutineId == id).ToListAsync();
        }
        public async Task<List<RoutineStep>> GetRoutineStepByRouteIDAndBasic(int id){
            return await _context.routineSteps.Include(rs => rs.Routine).Where(rs => rs.RoutineId == id && (rs.Routine.RoutineCategory == RoutineCategoryType.BasicDay 
                         || rs.Routine.RoutineCategory == RoutineCategoryType.BasicNight)).ToListAsync();
        }
        public async Task<List<RoutineStep>> GetRoutineStepByRouteIDAndAdvanced(int id){
            return await _context.routineSteps.Include(rs => rs.Routine).Where(rs => rs.RoutineId == id && (rs.Routine.RoutineCategory == RoutineCategoryType.AdvancedDay 
                         || rs.Routine.RoutineCategory == RoutineCategoryType.AdvancedNight)).ToListAsync();
        }
        public async Task CreateRoutineStepsAsync(List<Routine> routines,SkinType skinType)
        {
            var routineStepsToAdd = new List<RoutineStep>();

            foreach (var routine in routines)
            {
                List<string> stepNames = null;

                // Chọn danh sách step dựa vào RoutineCategory
                switch (routine.RoutineCategory)
                {
                    case Enums.RoutineCategoryType.BasicDay:
                        stepNames = new List<string>
                        {
                            "Sua rua mat",
                            "Toner",
                            "Serum",
                            "Kem duong am",
                            "Kem chong nang"
                        };
                        break;
                    case Enums.RoutineCategoryType.BasicNight:
                        stepNames = new List<string>
                        {
                            "Tay trang",
                            "Sua rua mat",
                            "Tay te bao chet",
                            "Toner",
                            "Serum",
                            "Kem mat",
                            "Kem duong am"
                        };
                        break;
                    case Enums.RoutineCategoryType.AdvancedDay:
                        stepNames = new List<string>
                        {
                            "Sua rua mat",
                            "Toner",
                            "Dac tri",
                            "Serum",
                            "Kem mat",
                            "Kem duong am",
                            "Kem chong nang"
                        };
                        break;
                    case Enums.RoutineCategoryType.AdvancedNight:
                        stepNames = new List<string>
                        {
                            "Tay trang",
                            "Sua rua mat",
                            "Toner",
                            "Tay te bao chet",
                            "Dac tri",
                            "Serum",
                            "Kem mat",
                            "Kem duong am"
                        };
                        break;
                }


                // Nếu stepNames null hoặc rỗng thì bỏ qua
                if (stepNames == null || !stepNames.Any()) continue;

                // Tạo RoutineStep cho từng stepName
                int stepIndex = 1;
                foreach (var stepName in stepNames)
                {
                    // Tìm CategoryId
                    var categoryId = await _categoryRepository.GetCategoryIdByNameAsync(stepName);
                    if (categoryId == null)
                    {
                        // Nếu chưa có Category, tùy logic, có thể tự tạo hoặc bỏ qua
                        continue;
                    }

                    // Tạo RoutineStep
                    var routineStep = new RoutineStep
                    {
                        RoutineId = routine.Id,
                        Step = stepIndex,
                        CategoryId = categoryId.Value,
                        ProductId = _productRepository.GetMostProductBasedOnSkinTypeAsync(skinType).Id
                    };

                    routineStepsToAdd.Add(routineStep);
                    stepIndex++;
                }
            }

            // Lưu toàn bộ RoutineStep
            if (routineStepsToAdd.Any())
            {
                await _context.routineSteps.AddRangeAsync(routineStepsToAdd);
                await _context.SaveChangesAsync();
            }
        }

    }
}


