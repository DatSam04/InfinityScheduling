using System.Collections.Generic;
using System.Linq;
using Bunit;
using ContosoCrafts.WebSite.Components;
using ContosoCrafts.WebSite.Services;
using Microsoft.Extensions.DependencyInjection;
#pragma warning disable IDE0005 // Using directive is unnecessary.
#pragma warning restore IDE0005 // Using directive is unnecessary.

using NUnit.Framework;

namespace UnitTests.Components
{
    /// <summary>
    /// Class containing unit test cases to ProductList
    /// </summary>
    public class ProductListTests : BunitTestContext
    {
        #region TestSetup

        /// <summary>
        /// Test initialize
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
        }

        #endregion TestSetup

        #region ProductList

        /// <summary>
        /// Test for ProductList Default Should Return Content
        /// </summary>
        [Test]
        public void ProductList_Default_Should_Return_Content()
        {
            //Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            //Act
            var page = RenderComponent<ProductList>();

            //Get the cards returned
            var result = page.Markup;

            //Assert
            Assert.AreEqual(true, result.Contains("Santorini"));
        }

        #endregion ProductList
        
        #region SelectProduct
        /// <summary>
        /// Test that select product return product content
        /// </summary>
        [Test]
        public void SelectProduct_Valid_ID_jenlooper_Should_Return_Content()
        {
            //Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);
            var id = "MoreInfoButton_jenlooper-cactus";
            var page = RenderComponent<ProductList>();
            var buttonList = page.FindAll("button");
            var button = buttonList.First(m => m.OuterHtml.Contains(id));
            
            //Act
            button.Click();
            var pageMarkup = page.Markup;
            
            //Assert
            Assert.AreEqual(true, pageMarkup.Contains("Famous for white-washed buildings and sunsets over the Aegean Sea"));
        }
        
        /// <summary>
        /// Test that select product in descending Title order return content
        /// </summary>
        [Test]
        public void SelectProduct_Descending_Title_Valid_ID_jenlooper_Should_Return_Content()
        {
            //Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);
            var id = "MoreInfoButton_jenlooper-cactus";
            var page = RenderComponent<ProductList>();
            var descendingFilter = page.FindAll("a.dropdown-item").First(a => a.TextContent == "Z-A");
            
            //Act
            descendingFilter.Click();
            page.Render();
            var buttonList = page.FindAll("button");
            var button = buttonList.First(m => m.OuterHtml.Contains(id));
            button.Click();
            var pageMarkup = page.Markup;
            
            //Assert
            Assert.AreEqual(true, pageMarkup.Contains("Famous for white-washed buildings and sunsets over the Aegean Sea"));
        }
        
        /// <summary>
        /// Test that select product in ascending Maker order return content
        /// </summary>
        [Test]
        public void SelectProduct_Ascending_Maker_Valid_ID_jenlooper_Should_Return_Content()
        {
            //Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);
            var id = "MoreInfoButton_jenlooper-cactus";
            var page = RenderComponent<ProductList>();
            var makerFilter = page.FindAll("a.dropdown-item").First(a => a.TextContent == "Maker");

            //Act
            makerFilter.Click();            
            page.Render();
            var buttonList = page.FindAll("button");
            var button = buttonList.First(m => m.OuterHtml.Contains(id));
            button.Click();
            var pageMarkup = page.Markup;
            
            //Assert
            Assert.AreEqual(true, pageMarkup.Contains("Famous for white-washed buildings and sunsets over the Aegean Sea"));
        }
        
        #endregion SelectProduct
        
        #region GetCurrentRating

        [Test]
        public void GetCurrentRating_Null_Should_Return_Zero()
        {
            //Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);
            var id = "MoreInfoButton_jenlooper-light";
            var page = RenderComponent<ProductList>();
            var buttonList = page.FindAll("button");
            var button = buttonList.First(m => m.OuterHtml.Contains(id));
            
            //Act
            button.Click();
            var pageMarkup = page.Markup;
            
            //Assert
        }
        
        #endregion GetCurrentRating

        #region AddRating

        /// <summary>
        /// Test for testing the AddRating button.
        /// </summary>
        [Test]
        public void AddRating_Valid_RatingClick_Should_ReturnNewRating()
        {
            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            const string TestButtonId = "MoreInfoButton_jenlooper-light";
            const string PreVoteString = "Be the first to vote!";
            const string PostVoteString = "out of 5";

            // Arrange: Built and find the More Info button
            var cut = RenderComponent<ProductList>();
            var moreInfoButton = cut.FindAll("Button").First(element => element.OuterHtml.Contains(TestButtonId));

            // Arrange: Click button and save markup
            moreInfoButton.Click();
            var preVoteMarkup = cut.Markup;

            // Act

            // Act: Find voting button
            // var voteButton = cut.FindAll("span").First(element => element.OuterHtml.Contains(VoteButtonId));
            var firstStar = cut.FindAll("span").First(span => span.ClassList.Contains("fa-star"));
            // Act: Click button and save markup
            // voteButton.Click();
            firstStar.Click();
            var postVoteMarkup = cut.Markup;


            // Reset

            // Assert
            Assert.AreEqual(true, preVoteMarkup.Contains(PreVoteString));
            Assert.AreEqual(true, postVoteMarkup.Contains(PostVoteString));
            Assert.AreNotEqual(preVoteMarkup, postVoteMarkup);
        }

        [Test]
        public void AddRating_Valid_Should_Update_Star()
        {
            //Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);
            var cut = RenderComponent<ProductList>();
            var moreInfoButton = cut.FindAll("button")
                .First(e => e.OuterHtml.Contains("MoreInfoButton_jenlooper-light"));

            // Act
            moreInfoButton.Click();
            var stars = cut.FindAll("span.fa-star");
            stars[0].Click();
            
            var markup = cut.Markup;
            
            // Assert
            Assert.AreEqual(true, markup.Contains("checked"));
        }

        #endregion AddRating

        #region SetFilter
        /// <summary>
        /// Test that all 3 set filter method will modify the product list order
        /// </summary>
        [Test]
        public void SetFilter_Valid_Should_Change_List_Order()
        {
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);
            var cut = RenderComponent<ProductList>();
            var alphabeticalLink = cut.FindAll("a.dropdown-item").First(a => a.TextContent == "A-Z");
            var descendingLink = cut.FindAll("a.dropdown-item").First(a => a.TextContent == "Z-A");
            var makerLink = cut.FindAll("a.dropdown-item").First(a => a.TextContent == "Maker");

            // Act to set filter on ascending order by Title
            alphabeticalLink.Click();
            cut.Render(); // re-render component
            var productsAlphabetical = cut.FindAll(".card .card-title").Select(e => e.TextContent).ToList();
            var expectedAlphabetical = TestHelper.ProductService.GetProducts()
                .OrderBy(p => p.Title)
                .Select(p => p.Title).ToList();
            //Assert
            Assert.AreEqual(expectedAlphabetical, productsAlphabetical);
            
            //Act to set filter on descending order by Title
            descendingLink.Click();
            cut.Render();
            var titlesDescending = cut.FindAll(".card .card-title").Select(e => e.TextContent).ToList();
            var expectedDescending = TestHelper.ProductService.GetProducts()
                .OrderByDescending(p => p.Title)
                .Select(p => p.Title).ToList();
            //Assert
            Assert.AreEqual(expectedDescending, titlesDescending);
            
            //Act to set filter on ascending order by Maker
            makerLink.Click();
            cut.Render();
            var titlesByMaker = cut.FindAll(".card .card-title").Select(e => e.TextContent).ToList();
            var expectedByMaker = TestHelper.ProductService.GetProducts()
                .OrderBy(p => p.Maker)
                .Select(p => p.Title).ToList();
            //Assert
            Assert.AreEqual(expectedByMaker, titlesByMaker);
        }
        
        #endregion SetFilter

    }
}
