using AutoMapper;
using Business.Abstract;
using Business.AutoMapper;
using Business.Concrete;
using Core.Configuration.Abstract;
using Core.Configuration.Concrete;
using Core.Helper.FileHelper;
using Core.Helper.MailHelper;
using DataAccess.Abstract;
using DataAccess.Concrete;
using DataAccess.Concrete.SQLServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Business.DependencyResolver
{
    public static class ServiceRegistration
    {
        public static void RunSQL(this IServiceCollection services)
        {
            #region AddScopeds
                #region Database
                services.AddScoped<AppDbContext>();
                #endregion
                #region User
                services.AddScoped<IUserService, UserManager>();
                #endregion
                #region Role
                services.AddScoped<IRoleService, RoleManager>();
                #endregion
                #region Email
                services.AddScoped<IEmailService, EmailManager>();
                services.AddScoped<IEmailConfiguration, EmailConfiguration>();
            #endregion
                #region Verification Code
                services.AddScoped<IVerificationCodeService, VerificationCodeManager>();
                services.AddScoped<IVerificationCodeDAL, EFVerificationCodeDAL>();
                #endregion
                #region Category
                services.AddScoped<ICategoryService, CategoryManager>();
                services.AddScoped<ICategoryDAL, EFCategoryDAL>();
                #endregion
                #region Category Language
                services.AddScoped<ICategoryLanguageService, CategoryLanguageManager>();
                services.AddScoped<ICategoryLanguageDAL, EFCategoryLanguageDAL>();
                #endregion
                #region CategorySubCategory
                services.AddScoped<ISubCategoryService, SubCategoryManager>();
                services.AddScoped<ISubCategoryDAL,EFSubCategoryDAL>();
            #endregion
                #region Product
                services.AddScoped<IProductService, ProductManager>();
                services.AddScoped<IProductDAL, EFProductDAL>();
                #endregion
                #region Product Language
                services.AddScoped<IProductLanguageService, ProductLanguageManager>();
                services.AddScoped<IProductLanguageDAL, EFProductLanguageDAL>();
            #endregion
                #region Picture
                services.AddScoped<IPictureService, PictureManager>();
                services.AddScoped<IPictureDAL, EFPictureDAL>();
            #endregion
                #region Specification
                services.AddScoped<ISpecificationService, SpecificationManager>();
                services.AddScoped<ISpecificationDAL, EFSpecificationDAL>();
            #endregion
                #region Product Category
                services.AddScoped<IProductCategoryServices, ProductCategoryManager>();
                services.AddScoped<IProductCategoryDAL, EFProductCategoryDAL>();
            #endregion
                #region FileHelper
                services.AddScoped<IFileService, FileManager>();
                #endregion
                #region Stock
                services.AddScoped<IStockDAL, EFStockDAL>();
                services.AddScoped<IStockService,StockManager>();
                #endregion
            #endregion
            #region Mapper Configuration
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile<MappingProfile>();
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            #endregion
            #region Password Options
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            });
            #endregion
            #region Cookie Options
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.HttpOnly = HttpOnlyPolicy.Always;
                options.Secure = CookieSecurePolicy.Always;
            });
            #endregion  
        }
    }
}
