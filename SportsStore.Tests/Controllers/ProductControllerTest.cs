using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models.Domain;
using SportsStore.Models.ProductViewModels;
using SportsStore.Tests.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SportsStore.Tests.Controllers
{
    public class ProductControllerTest
    {
        private readonly DummyApplicationDbContext _dummyContext;
        private readonly ProductController _productController;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly Product _runningShoes;
        private readonly int _runningShoesId;
        private readonly int _nonExistingProductId;

        public ProductControllerTest()
        {
            _dummyContext = new DummyApplicationDbContext();
            _mockProductRepository = new Mock<IProductRepository>();
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockProductRepository.Setup(p => p.GetById(1)).Returns(_dummyContext.RunningShoes);
            _nonExistingProductId = 9999;
            _mockProductRepository.Setup(p => p.GetById(_nonExistingProductId)).Returns(null as Product);
            _mockCategoryRepository.Setup(p => p.GetAll()).Returns(_dummyContext.Categories);
            _runningShoes = _dummyContext.RunningShoes;
            _runningShoesId = _runningShoes.ProductId;
            _productController = new ProductController(_mockProductRepository.Object, _mockCategoryRepository.Object)
            {
                TempData = new Mock<ITempDataDictionary>().Object
            };
        }

        #region == Index ==
        [Fact]
        public void Index_AllCategories_PassesAllProductsSortedByNameInModelAndAllCategoriesSortedByNameInViewData()
        {
            _mockProductRepository.Setup(p => p.GetAll()).Returns(_dummyContext.Products);
            var result = Assert.IsType<ViewResult>(_productController.Index());
            List<Product> products = Assert.IsType<List<Product>>(result.Model);
            var categories = Assert.IsType<SelectList>(result.ViewData["Categories"]);
            Assert.Equal(11, products.Count);
            Assert.Equal("Bling-bling King", products[0].Name);
            Assert.Equal("Unsteady chair", products[10].Name);
            Assert.Equal(3, categories.Count());
        }

        [Fact]
        public void Index_ExistingCategory_PassesAllProductsFromThatCategorySortedByNameInModel()
        {
            _mockCategoryRepository.Setup(p => p.GetByIdIncludingProducts(1)).Returns(_dummyContext.Soccer);
            var result = Assert.IsType<ViewResult>(_productController.Index(1));
            var products = Assert.IsType<List<Product>>(result.Model);
            Assert.Equal(4, products.Count);
            Assert.Equal("Corner flags", products[0].Name);
        }
        #endregion

        #region == Edit ==
        [Fact]
        public void EditHttpGet_ValidProductId_PassesProductDetailsInAnEditViewModelAndPassesSelectListWithAllCategoriesInViewData()
        {
            var result = Assert.IsType<ViewResult>(_productController.Edit(1));
            var productVm = Assert.IsType<EditViewModel>(result.Model);
            var categories = Assert.IsType<SelectList>(result.ViewData["Categories"]);
            Assert.Equal(3, categories.Count());
            Assert.Equal("Running shoes", productVm?.Name);
        }

        [Fact]
        public void EditHttpGet_ProductNotFound_ReturnsNotFound()
        {
            Assert.IsType<NotFoundResult>(_productController.Edit(_nonExistingProductId));
        }

        [Fact]
        public void EditHttpPost_ValidEdit_UpdatesAndPersistsTheProductAndRedirectsToIndex()
        {
            _mockProductRepository.Setup(p => p.GetById(2)).Returns(_dummyContext.RunningShoes);
            _mockCategoryRepository.Setup(c => c.GetById(It.IsAny<int>())).Returns(_dummyContext.Soccer);
            var productVm = new EditViewModel(_dummyContext.RunningShoes)
            {
                Name = "RunningShoesGewijzigd",
                Price = 1000
            };
            var result = Assert.IsType<RedirectToActionResult>(_productController.Edit(2, productVm));
            Assert.Equal("RunningShoesGewijzigd", _runningShoes.Name);
            Assert.Equal(1000, _runningShoes.Price);
            Assert.Equal("Protective and fashionable", _runningShoes.Description);
            Assert.Equal("Index", result.ActionName);
            _mockProductRepository.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Fact]
        public void EditHttpPost_InValidEdit_DoesNotChangeNorPersistProductAndRedirectsToActionIndex()
        {
            _mockProductRepository.Setup(m => m.GetById(2)).Returns(_dummyContext.RunningShoes);
            var productVm = new EditViewModel(_dummyContext.RunningShoes) { Price = -1 };
            var result = Assert.IsType<RedirectToActionResult>(_productController.Edit(2, productVm));
            var runningShoes = _dummyContext.RunningShoes;
            Assert.Equal("Running shoes", runningShoes.Name);
            Assert.Equal(95, runningShoes.Price);
            Assert.Equal("Index", result?.ActionName);
            _mockProductRepository.Verify(m => m.SaveChanges(), Times.Never());
        }

        [Fact]
        public void EditHttpPost_ProductNotFound_ReturnsNotFoundResult()
        {
            var productVm = new EditViewModel(_dummyContext.RunningShoes);
            var action = _productController.Edit(_nonExistingProductId, productVm);
            Assert.IsType<NotFoundResult>(action);
        }

        [Fact]
        public void EditHttpPost_ModelStateErrors_DoesNotChangeNorPersistTheProduct()
        {
            _mockProductRepository.Setup(m => m.GetById(_runningShoesId)).Returns(_dummyContext.RunningShoes);
            var productVm = new EditViewModel(_dummyContext.RunningShoes);
            _productController.ModelState.AddModelError("", "Any error");
            _productController.Edit(_runningShoesId, productVm);
            var runningShoes = _dummyContext.RunningShoes;
            Assert.Equal("Running shoes", runningShoes.Name);
            Assert.Equal(95, runningShoes.Price);
            _mockProductRepository.Verify(m => m.SaveChanges(), Times.Never());
        }

        [Fact]
        public void EditHttpPost_ModelStateErrors_PassesEditViewModelInViewResultModelAndSelectListsinViewdata()
        {
            var productVm = new EditViewModel(_dummyContext.RunningShoes);
            _productController.ModelState.AddModelError("", "Any error");
            var result = Assert.IsType<ViewResult>(_productController.Edit(1, productVm));
            productVm = Assert.IsType<EditViewModel>(result.Model);
            Assert.Equal("Running shoes", productVm.Name);
            var categories = Assert.IsType<SelectList>(result.ViewData["Categories"]);
            Assert.Equal(3, categories.Count());
        }

        #endregion

        #region == Create ==
        [Fact]
        public void CreateHttpGet_PassesDetailsOfANewProductInAnEditViewModelToView()
        {
            var result = Assert.IsType<ViewResult>(_productController.Create());
            var productVm = Assert.IsType<EditViewModel>(result.Model);
            Assert.Null(productVm.Name);
        }

        [Fact]
        public void CreateHttpPost_ValidProduct_AddsNewProductToRepositoryAndRedirectsToIndex()
        {
            _mockProductRepository.Setup(p => p.Add(It.IsNotNull<Product>()));
            _mockCategoryRepository.Setup(c => c.GetById(1)).Returns(_dummyContext.Soccer);
            var productVm = new EditViewModel()
            {
                CategoryId = 1,
                Name = "nieuw product",
                Price = 10
            };
            var result = Assert.IsType<RedirectToActionResult>(_productController.Create(productVm));
            Assert.Equal("Index", result.ActionName);
            _mockProductRepository.Verify(m => m.Add(It.IsNotNull<Product>()), Times.Once);
            _mockProductRepository.Verify(m => m.SaveChanges(), Times.Once);
        }


        [Fact]
        public void CreateHttpPost_InvalidProduct_DoesNotCreateNorPersistsProductAndRedirectsToActionIndex()
        {
            _mockProductRepository.Setup(m => m.Add(It.IsAny<Product>()));
            var productVm = new EditViewModel()
            {
                CategoryId = 1,
                Name = "nieuw product",
                Price = -10
            };
            var action = Assert.IsType<RedirectToActionResult>(_productController.Create(productVm));
            Assert.Equal("Index", action?.ActionName);
            _mockProductRepository.Verify(m => m.SaveChanges(), Times.Never());
            _mockProductRepository.Verify(m => m.Add(It.IsAny<Product>()), Times.Never());
        }

        [Fact]
        public void CreateHttpPost_ModelStateErrors_DoesNotChangeNorPersistTheProduct()
        {
            var productVm = new EditViewModel(_dummyContext.RunningShoes);
            _productController.ModelState.AddModelError("", "Any error");
            _productController.Create(productVm);
            var runningShoes = _dummyContext.RunningShoes;
            Assert.Equal("Running shoes", runningShoes.Name);
            Assert.Equal(95, runningShoes.Price);
            _mockProductRepository.Verify(m => m.SaveChanges(), Times.Never());
        }

        [Fact]
        public void CreateHttpPost_ModelStateErrors_PassesEditViewModelInViewResultModelAndPassesSelectListsInViewData()
        {
            var productVm = new EditViewModel(_dummyContext.RunningShoes);
            _productController.ModelState.AddModelError("", "Any error");
            var result = Assert.IsType<ViewResult>(_productController.Create(productVm));
            productVm = Assert.IsType<EditViewModel>(result.Model);
            var categories = Assert.IsType<SelectList>(result.ViewData["Categories"]);
            Assert.Equal(3, categories.Count());
            Assert.Equal("Running shoes", productVm.Name);
        }

        #endregion

        #region == Delete ==
        [Fact]
        public void DeleteHttpGet_ProductFound_PassesProductNameInViewDataToView()
        {
            var result = _productController.Delete(1) as ViewResult;
            Assert.Equal("Running shoes", result?.ViewData["ProductName"]);
        }

        [Fact]
        public void DeleteHttpGet_ProductNotFound_ReturnsNotFound()
        {
            _mockProductRepository.Setup(p => p.GetById(1)).Returns(null as Product);
            var result = _productController.Delete(1);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void DeleteHttpPost_ProductFound_DeletesProductAndRedirectsToIndex()
        {
            _mockProductRepository.Setup(p => p.Delete(_dummyContext.RunningShoes));
            var result = Assert.IsType<RedirectToActionResult>(_productController.DeleteConfirmed(1));
            Assert.Equal("Index", result?.ActionName);
            _mockProductRepository.Verify(m => m.Delete(_dummyContext.RunningShoes), Times.Once);
            _mockProductRepository.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Fact]
        public void DeleteHttpPost_UnsuccessfullDelete_RedirectsToIndex()
        {
            _mockProductRepository.Setup(p => p.GetById(1)).Throws<ArgumentException>();
            var result = Assert.IsType<RedirectToActionResult>(_productController.DeleteConfirmed(1));
            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void DeleteHttpPost_ProductNotFound_ReturnsNotFound()
        {
            var result = _productController.DeleteConfirmed(_nonExistingProductId);
            Assert.IsType<NotFoundResult>(result);
        }
        #endregion
    }
}